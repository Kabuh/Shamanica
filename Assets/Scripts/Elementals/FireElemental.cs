using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElemental : Hero
{
    public override void Attack(List<Hero> targets)
    {
        AbilityTraits.Damage(CurrentDamage, targets);
    }

    public override void Buff(List<Hero> targets)
    {
        AbilityTraits.HPBuff(BuffAmount, targets);
    }
}
