using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace Puzzle10
{
    public class Solver
    {
        public Solver(params string[] input)
        {
            Actions = input.Select(line => ParseLine(line)).ToList();
        }

        private List<Action<Balancer>> Actions { get; }

        public int Solve1()
        {
            var resultId = -1;
            var balancer = new Balancer((bot, low, high) =>
            {
                if (low == 17 && high == 61)
                    resultId = bot;
            });

            foreach (var action in Actions)
            {
                action(balancer);
            }

            return resultId;
        }

        public int Solve2()
        {
            var balancer = new Balancer();

            foreach (var action in Actions)
            {
                action(balancer);
            }

            return balancer.GetOutput(0).Chip*balancer.GetOutput(1).Chip*balancer.GetOutput(2).Chip;
        }

        private static readonly Regex ValueRegex =
            new Regex(@"value (?<value>\d+) goes to bot (?<taker_id>\d+)", RegexOptions.Compiled);

        private static readonly Regex SetUpRegex =
            new Regex(@"bot (?<giver_id>\d+) gives low to (?<low_kind>bot|output) (?<low_id>\d+) and high to (?<high_kind>bot|output) (?<high_id>\d+)", RegexOptions.Compiled);

        private Action<Balancer> ParseLine(string line)
        {
            if (SetUpRegex.IsMatch(line))
            {
                var match = SetUpRegex.Match(line);
                var giverId = int.Parse(match.Groups["giver_id"].Value);
                var lowKind = match.Groups["low_kind"].Value;
                var lowId = int.Parse(match.Groups["low_id"].Value);
                var highKind = match.Groups["high_kind"].Value;
                var highId = int.Parse(match.Groups["high_id"].Value);

                var getLowTaker = lowKind == "bot"
                    ? (Func<Balancer, Balancer.ITaker>) (balancer => balancer.GetBot(lowId))
                    : (balancer => balancer.GetOutput(lowId));

                var getHighTaker = highKind == "bot"
                    ? (Func<Balancer, Balancer.ITaker>) (balancer => balancer.GetBot(highId))
                    : (balancer => balancer.GetOutput(highId));

                return balancer =>
                {
                    var low = getLowTaker(balancer);
                    var high = getHighTaker(balancer);
                    balancer.SetUpBot(giverId, low, high);
                };
            }

            if (ValueRegex.IsMatch(line))
            {
                var match = ValueRegex.Match(line);
                var value = int.Parse(match.Groups["value"].Value);
                var takerId = int.Parse(match.Groups["taker_id"].Value);

                return balancer =>
                {
                    balancer.GiveValue(takerId, value);
                };
            }

            throw new ArgumentException("Could not parse line '" + line + "'");
        } 
    }

    public class Balancer
    {
        public Balancer()
        {
            Bots = new Dictionary<int, Bot>();
            Outputs = new Dictionary<int, Output>();
            OnCompare = (bot, lower, higher) => { };
        }

        public Balancer(Action<int, int, int> onCompare) : this()
        {
            OnCompare = onCompare;
        }

        public Dictionary<int, Bot> Bots { get; private set; }
        public Dictionary<int, Output> Outputs { get; }

        public Action<int, int, int> OnCompare { get; set; }

        public void GiveValue(int botId, int value)
        {
            GetBot(botId);
            Bots[botId].Take(value);
        }

        public void SetUpBot(int botId, ITaker lowerTaker, ITaker higherTaker)
        {
            var bot = GetBot(botId);
            bot.LowerTaker = lowerTaker;
            bot.HigherTaker = higherTaker;
        }

        public Bot GetBot(int botId)
        {
            if (!Bots.ContainsKey(botId))
            {
                Bots.Add(botId, new Bot(this, botId));
            }

            return Bots[botId];
        }

        public Output GetOutput(int outputId)
        {
            if (!Outputs.ContainsKey(outputId))
            {
                Outputs.Add(outputId, new Output(outputId));
            }

            return Outputs[outputId];
        }

        public interface ITaker
        {
            int Id { get; }
            void Take(int chip);
        }

        public class Bot : ITaker
        {
            private ITaker _higherTaker;
            private ITaker _lowerTaker;

            public Bot(Balancer balancer, int id)
            {
                Balancer = balancer;
                Id = id;
                Chips = new List<int>(2);
            }

            private Balancer Balancer { get; }
            public int Id { get; }

            public ITaker HigherTaker
            {
                get { return _higherTaker; }
                set
                {
                    _higherTaker = value;
                    CompareAndGive();
                }
            }

            public ITaker LowerTaker
            {
                get { return _lowerTaker; }
                set
                {
                    _lowerTaker = value;
                    CompareAndGive();
                }
            }

            public List<int> Chips { get; private set; }

            public void Take(int chip)
            {
                Chips.Add(chip);
                CompareAndGive();
            }

            private void CompareAndGive()
            {
                if (CanGive())
                {
                    var chips = Chips.OrderBy(c => c).ToArray();
                    Balancer.OnCompare(Id, chips[0], chips[1]);
                    LowerTaker.Take(chips[0]);
                    HigherTaker.Take(chips[1]);
                }
            }

            private bool CanGive()
            {
                return (Chips.Count == 2 && LowerTaker != null && HigherTaker != null);
            }
        }

        public class Output : ITaker
        {
            public Output(int id)
            {
                Id = id;
                HasChip = false;
            }

            public void Take(int chip)
            {
                if (HasChip) 
                    throw new Exception("Output " + Id + " already has a chip");

                Chip = chip;
                HasChip = true;
            }

            public int Id { get; }
            public bool HasChip { get; private set; }
            public int Chip { get; private set; }
        }
    }
}
