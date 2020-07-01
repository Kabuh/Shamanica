using System.Collections.Generic;

//take any skill you want buffet
public static class AbilityTraits
{
    static int callbackCounter;

    public static void Damage(int Damage, List<Hero> targets) {

        foreach (Hero item in targets) {
            if (item != null) {
                item.CurrentHP = item.CurrentHP - Damage;
                item.AttackAnimate();
            }
        }
    }

    public static void Damage(int Damage, Hero item)
    {
        item.CurrentHP = item.CurrentHP - Damage;
        item.AttackAnimate();
            

        if (item.CurrentHP <= 0)
        {
            item.Death();
        }
    }


    public static void HPBuff(int buffValue, List<Hero> targets) {
        foreach (Hero item in targets)
        {
            if (item != null) {
                if (!item.HasHPBuff)
                {
                    float buffAmmount = item.MaxHP * ((float)buffValue / 100);
                    item.CurrentMaxHP += (int)buffAmmount;
                    item.CurrentHP += (int)buffAmmount;
                    item.BuffAnimate();
                    item.HasHPBuff = true;
                }
            }
        }
    }
}
