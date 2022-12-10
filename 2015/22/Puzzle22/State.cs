using System.Text;

namespace Puzzle22;

public readonly record struct State
{
    public State()
    {
    }

    public State(Player player, Boss boss)
    {
        Player = player;
        Boss = boss;
    }

    public readonly Player Player { get; init; }
    public readonly Boss Boss { get; init; }

    public readonly int ShieldTimer { get; init; }
    public readonly int PoisonTimer { get; init; }
    public readonly int RechargeTimer { get; init; }

    public bool IsFinished => Player.IsDead || Boss.IsDead;
    public bool IsWin => IsFinished && Boss.IsDead && !Player.IsDead;

    public override string ToString() =>
        $"Player has {Player.Hp} hit points, {Player.Armor} armor, {Player.Mana} mana\nBoss has {Boss.Hp} hit points";
}