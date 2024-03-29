﻿namespace Puzzle22;

public readonly record struct Player
{
    public Player()
    {
    }

    public Player(int hp, int mana, int armor = 0)
    {
        Hp = hp;
        Mana = mana;
        Armor = armor;
    }

    public readonly int Hp { get; init; }
    public readonly int Mana { get; init; }
    public readonly int Armor { get; init; }
    public bool IsDead => Hp <= 0;

    public Player TakeDamage(int points) => this with { Hp = Hp - points };
    public Player Heal(int points) => this with { Hp = Hp + points };
    public Player SpendMana(int points) => this with { Mana = Mana - points };
    public Player RechargeMana(int points) => this with { Mana = Mana + points };
    public Player SetArmor(int points) => this with { Armor = points };
}