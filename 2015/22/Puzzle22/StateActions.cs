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

    public static readonly Dictionary<Spell, int> ManaCost = new()
    {
        { Spell.MagicMissile, 53 },
        { Spell.Drain, 73 },
        { Spell.Shield, 113 },
        { Spell.Poison, 173 },
        { Spell.Recharge, 229 },
    };

    public static bool Debug { get; set; } = false;

    private static void Log(object message)
    {
        if (Debug)
        {
            Console.WriteLine(message.ToString());
        }
    }

    public static State MakeAction(this State state, Spell action)
    {
        Assert.IsFalse(state.Player.IsDead, "Player is dead");
        Assert.IsFalse(state.Boss.IsDead, "Boss is dead");

        // player turn

        Log("\n-- Player turn --");
        Log(state);

        // do timer actions
        var afterPlayerTimerActions = state.DoTimerAction();

        // check dead
        if (afterPlayerTimerActions.Boss.IsDead)
        {
            Log("Boss is dead");
            return afterPlayerTimerActions;
        }
        Assert.IsFalse(afterPlayerTimerActions.Player.IsDead, "Player is dead");
        Assert.IsFalse(afterPlayerTimerActions.Boss.IsDead, "Boss is dead");

        // do player action
        var afterPlayerTurn = afterPlayerTimerActions.CastSpell(action);

        // check dead
        if (afterPlayerTurn.Boss.IsDead)
        {
            Log("Boss is dead");
            return afterPlayerTurn;
        }
        Assert.IsFalse(afterPlayerTurn.Player.IsDead, "Player is dead");
        Assert.IsFalse(afterPlayerTurn.Boss.IsDead, "Boss is dead");

        // boss turn

        Log("\n-- Boss turn --");
        Log(afterPlayerTurn);

        // do timer actions
        var afterBossTimerActions = afterPlayerTurn.DoTimerAction();

        // check dead
        if (afterBossTimerActions.Boss.IsDead)
        {
            Log("Boss is dead");
            return afterBossTimerActions;
        }
        Assert.IsFalse(afterBossTimerActions.Player.IsDead, "Player is dead");
        Assert.IsFalse(afterBossTimerActions.Boss.IsDead, "Boss is dead");

        // do boss action
        var afterBossAction = afterBossTimerActions.DoBossAction();

        // check player dead
        if (afterBossAction.Player.IsDead)
        {
            Log("Player is dead");
        }
        Assert.IsFalse(afterBossAction.Boss.IsDead, "Boss is dead");

        return afterBossAction;
    }

    public static State MakeActionHard(this State state, Spell action)
    {
        Assert.IsFalse(state.Player.IsDead, "Player is dead");
        Assert.IsFalse(state.Boss.IsDead, "Boss is dead");

        // Take additional damage

        var afterPlayerDamage = state with { Player = state.Player.TakeDamage(1) };
        if (afterPlayerDamage.Player.IsDead)
        {
            return afterPlayerDamage;
        }
        Assert.IsFalse(afterPlayerDamage.Player.IsDead, "Player is dead");
        Assert.IsFalse(afterPlayerDamage.Boss.IsDead, "Boss is dead");

        return afterPlayerDamage.MakeAction(action);
    }


    private static State CastSpell(this State state, Spell action)
    {
        Assert.IsFalse(state.Player.IsDead, "Player is dead");
        Assert.IsFalse(state.Boss.IsDead, "Boss is dead");

        var cost = ManaCost[action];
        Assert.IsTrue(state.Player.Mana >= cost, "Not enough mana");

        var afterUsingMana = state with { Player = state.Player.SpendMana(cost) };
        return action switch
        {
            Spell.MagicMissile => afterUsingMana.CastMagicMissile(),
            Spell.Drain => afterUsingMana.CastDrain(),
            Spell.Shield => afterUsingMana.CastShield(),
            Spell.Poison => afterUsingMana.CastPoison(),
            Spell.Recharge => afterUsingMana.CastRecharge(),
            _ => throw new InvalidOperationException(),
        };
    }

    private static State DoBossAction(this State state)
    {
        Assert.IsFalse(state.Player.IsDead, "Player is dead");
        Assert.IsFalse(state.Boss.IsDead, "Boss is dead");

        var dmg = Math.Max(state.Boss.Damage - state.Player.Armor, 1);
        Log($"Boss attacks for {dmg} damage.");

        return state with { Player = state.Player.TakeDamage(dmg) };
    }

    private static State DoTimerAction(this State state) => state.ShieldTick().PoisonTick().RechargeTick();

    private static State ShieldTick(this State state)
    {
        Assert.IsFalse(state.Player.IsDead, "Player is dead");
        Assert.IsFalse(state.Boss.IsDead, "Boss is dead");

        if (state.ShieldTimer == 0)
        {
            return state;
        }
        var afterTick = state with { ShieldTimer = state.ShieldTimer - 1 };
        Log($"Shield timer is now {afterTick.ShieldTimer}");

        if (afterTick.ShieldTimer == 0)
        {
            var afterWearOff = afterTick with { Player = afterTick.Player.SetArmor(0) };
            Log($"Shield wears off, decreasing armor by {state.Player.Armor - afterWearOff.Player.Armor}");
            return afterWearOff;
        }

        return afterTick;
    }

    private static State PoisonTick(this State state)
    {
        Assert.IsFalse(state.Player.IsDead, "Player is dead");
        Assert.IsFalse(state.Boss.IsDead, "Boss is dead");

        if (state.PoisonTimer == 0)
        {
            return state;
        }

        var afterTick = state with { Boss = state.Boss.TakeDamage(3), PoisonTimer = state.PoisonTimer - 1 };
        Log($"Poison deals {state.Boss.Hp - afterTick.Boss.Hp} damage; its timer is now {afterTick.PoisonTimer}");

        return afterTick;
    }

    private static State RechargeTick(this State state)
    {
        Assert.IsFalse(state.Player.IsDead, "Player is dead");
        //Assert.IsFalse(state.Boss.IsDead, "Boss is dead");

        if (state.RechargeTimer == 0)
        {
            return state;
        }

        var afterTick = state with { Player = state.Player.RechargeMana(101), RechargeTimer = state.RechargeTimer - 1 };
        Log($"Recharge provides {afterTick.Player.Mana - state.Player.Mana} mana; its timer is now {afterTick.RechargeTimer}");

        return afterTick;
    }

    private static State CastMagicMissile(this State state)
    {
        Assert.IsFalse(state.Player.IsDead, "Player is dead");
        Assert.IsFalse(state.Boss.IsDead, "Boss is dead");

        var afterTick =  state with { Boss = state.Boss.TakeDamage(4) };
        Log($"Player casts Magic Missile, dealing {state.Boss.Hp - afterTick.Boss.Hp} damage");

        return afterTick;
    }

    private static State CastDrain(this State state)
    {
        Assert.IsFalse(state.Player.IsDead, "Player is dead");
        Assert.IsFalse(state.Boss.IsDead, "Boss is dead");

        var afterTick = state with { Boss = state.Boss.TakeDamage(2), Player = state.Player.Heal(2) };
        Log($"Player casts Drain, dealing {state.Boss.Hp - afterTick.Boss.Hp}, and healing {afterTick.Player.Hp - state.Player.Hp} hit points");

        return afterTick;
    }

    private static State CastShield(this State state)
    {
        Assert.IsTrue(state.ShieldTimer == 0, "Shield is already active");
        Assert.IsFalse(state.Player.IsDead, "Player is dead");
        Assert.IsFalse(state.Boss.IsDead, "Boss is dead");

        var afterTick = state with
        {
            ShieldTimer = 6,
            Player = state.Player.SetArmor(7),
        };

        Log($"Player casts Shield, increasing armor by {afterTick.Player.Armor - state.Player.Armor}.");

        return afterTick;
    }

    private static State CastPoison(this State state)
    {
        Assert.IsTrue(state.PoisonTimer == 0, "Poison is already active");
        Assert.IsFalse(state.Player.IsDead, "Player is dead");
        Assert.IsFalse(state.Boss.IsDead, "Boss is dead");

        Log($"Player casts Poison");

        return state with { PoisonTimer = 6 };
    }

    private static State CastRecharge(this State state)
    {
        Assert.IsTrue(state.RechargeTimer == 0, "Recharge is already active");
        Assert.IsFalse(state.Player.IsDead, "Player is dead");
        Assert.IsFalse(state.Boss.IsDead, "Boss is dead");

        Log($"Player casts Recharge");

        return state with { RechargeTimer = 5 };
    }
}