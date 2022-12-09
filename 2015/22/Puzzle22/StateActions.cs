using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Puzzle22;

public static class StateActions
{
    // Actions
    // MagicMissile: 53 mana, instant 4 dmg
    // Drain: 73 mana, instant 2 dmg, instant 2 heal
    // Shield: 113 mana, 7 armor, 6 turns
    // Poison: 173 mana, 3 dmg, 6 turns
    // Recharge: 229 mana, +101 mana, 5 turns

    public static readonly Dictionary<PlayerAction, int> ManaCost = new()
    {
        { PlayerAction.MagicMissile, 53 },
        { PlayerAction.Drain, 73 },
        { PlayerAction.Shield, 113 },
        { PlayerAction.Poison, 173 },
        { PlayerAction.Recharge, 229 },
    };

    public static State MakeAction(this State state, PlayerAction action)
    {
        Assert.IsFalse(state.Player.IsDead, "Player is dead");
        Assert.IsFalse(state.Boss.IsDead, "Boss is dead");

        // player turn

        // do timer actions
        var afterPlayerTimerActions = state.DoTimerAction();

        // do player action
        var afterPlayerTurn = afterPlayerTimerActions.DoPlayerAction(action);

        // check dead
        if (afterPlayerTurn.Boss.IsDead)
        {
            return afterPlayerTurn;
        }

        // boss turn

        // do timer actions
        var afterBossTimerActions = afterPlayerTurn.DoTimerAction();

        // do boss action
        return afterBossTimerActions.DoBossAction();
    }

    public static State MakeActionHard(this State state, PlayerAction action)
    {
        Assert.IsFalse(state.Player.IsDead, "Player is dead");
        Assert.IsFalse(state.Boss.IsDead, "Boss is dead");

        // player turn

        var afterPlayerDamage = state with { Player = state.Player.TakeDamage(1) };
        if (afterPlayerDamage.Player.IsDead)
        {
            return afterPlayerDamage;
        }

        // do timer actions
        var afterPlayerTimerActions = afterPlayerDamage.DoTimerAction();

        // do player action
        var afterPlayerTurn = afterPlayerTimerActions.DoPlayerAction(action);

        // check dead
        if (afterPlayerTurn.Boss.IsDead)
        {
            return afterPlayerTurn;
        }

        // boss turn

        // do timer actions
        var afterBossTimerActions = afterPlayerTurn.DoTimerAction();

        // do boss action
        return afterBossTimerActions.DoBossAction();
    }


    private static State DoPlayerAction(this State state, PlayerAction action)
    {
        var cost = ManaCost[action];
        Assert.IsTrue(state.Player.Mana >= cost, "Not enough mana");

        var afterUsingMana = state with { Player = state.Player.SpendMana(cost) };
        return action switch
        {
            PlayerAction.MagicMissile => afterUsingMana.CastMagicMissile(),
            PlayerAction.Drain => afterUsingMana.CastDrain(),
            PlayerAction.Shield => afterUsingMana.CastShield(),
            PlayerAction.Poison => afterUsingMana.CastPoison(),
            PlayerAction.Recharge => afterUsingMana.CastRecharge(),
            _ => throw new InvalidOperationException(),
        };
    }

    private static State DoBossAction(this State state) =>
        state.Boss.IsDead
            ? state
            : state.ShieldTimer > 0
                ? (state with { Player = state.Player.TakeDamage(Math.Max(state.Boss.Damage - 7, 1)) })
                : (state with { Player = state.Player.TakeDamage(state.Boss.Damage) });

    private static State DoTimerAction(this State state) => state.ShieldTick().PoisonTick().RechargeTick();

    private static State ShieldTick(this State state) =>
        state.ShieldTimer > 0
            ? (state with { ShieldTimer = state.ShieldTimer - 1 })
            : state;

    private static State PoisonTick(this State state) =>
        state.Boss.IsDead
            ? state
            : state.PoisonTimer > 0
                ? (state with { Boss = state.Boss.TakeDamage(3), PoisonTimer = state.PoisonTimer - 1 })
                : state;

    private static State RechargeTick(this State state) =>
        state.Player.IsDead
            ? state
            : state.RechargeTimer > 0
                ? (state with { Player = state.Player.RechargeMana(101), RechargeTimer = state.RechargeTimer - 1 })
                : state;

    private static State CastMagicMissile(this State state) =>
        state with { Boss = state.Boss.TakeDamage(4) };

    private static State CastDrain(this State state) =>
        state with { Boss = state.Boss.TakeDamage(2), Player = state.Player.Heal(2) };

    private static State CastShield(this State state)
    {
        Assert.IsTrue(state.ShieldTimer == 0, "Shield is already active");
        return state with { ShieldTimer = 6 };
    }

    private static State CastPoison(this State state)
    {
        Assert.IsTrue(state.PoisonTimer == 0, "Poison is already active");
        return state with { PoisonTimer = 6 };
    }

    private static State CastRecharge(this State state)
    {
        Assert.IsTrue(state.RechargeTimer == 0, "Recharge is already active");
        return state with { RechargeTimer = 5 };
    }
}