namespace Puzzle22;

public readonly record struct Boss
{
    public Boss()
    {
    }

    public Boss(int hp, int damage)
    {
        Hp = hp;
        Damage = damage;
    }

    public readonly int Hp { get; init; }
    public readonly int Damage { get; init; }
    public bool IsDead => Hp <= 0;

    public Boss TakeDamage(int damage) => this with { Hp = Hp - damage };
}