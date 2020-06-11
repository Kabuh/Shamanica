using System.Collections.Generic;

//take any skill you want buffet
public static class AbilityTraits
{
    public static void Damage(int Damage, List<Hero> targets) {
        foreach (Hero item in targets) {
            if (item != null) {
                item.currentHP = item.currentHP - Damage;
                item.RefreshText();

                if (item.currentHP <= 0)
                {
                    item.Death();
                }
            }
        }
    }

    public static void Damage(int Damage, Hero item)
    {
            item.currentHP = item.currentHP - Damage;
            item.RefreshText();

            if (item.currentHP <= 0)
            {
                item.Death();
            }
    }


    public static void HPBuff(int buffValue, List<Hero> targets) {
        foreach (Hero item in targets)
        {
            if (item != null) {
                if (!item.hasHPBuff)
                {
                    float buffAmmount = item.MaxHP * ((float)buffValue / 100);
                    item.currentMaxHP += (int)buffAmmount;
                    item.currentHP += (int)buffAmmount;
                    item.RefreshText();
                    item.hasHPBuff = true;
                }
            }
        }
    }
}
