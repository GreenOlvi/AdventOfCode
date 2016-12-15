using FluentAssertions;
using NUnit.Framework;
using Puzzle10;

namespace Puzzle10Tests
{
    [TestFixture]
    public class SolverTests
    {
        [Test]
        public void BalancerTest()
        {
            var balancer = new Balancer((botId, lower, higher) =>
            {
                if (botId == 1)
                {
                    lower.Should().Be(2);
                    higher.Should().Be(3);
                }
            });

            balancer.GiveValue(2, 5);
            balancer.SetUpBot(2, balancer.GetBot(1), balancer.GetBot(0));
            balancer.GiveValue(1, 3);
            balancer.SetUpBot(1, balancer.GetOutput(1), balancer.GetBot(0));
            balancer.SetUpBot(0, balancer.GetOutput(2), balancer.GetOutput(0));
            balancer.GiveValue(2, 2);

            balancer.GetOutput(0).Chip.Should().Be(5);
            balancer.GetOutput(2).Chip.Should().Be(3);
        }
    }
}
