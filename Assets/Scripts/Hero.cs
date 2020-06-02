using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class Hero : MonoBehaviour
{
    public TextMeshProUGUI text;

    public int HeroID = 0;
    StatTable MyStatTable;

    public Slot mySlot;

    private int maxHP;
    private int defaultInitialive;
    private int defaultDamage;

    public int MaxHP { get => maxHP; }
    public int DefaultInitialive { get => defaultInitialive; }
    public int DefaultDamage { get => defaultDamage; }


    public int currentHP;
    public int currentMaxHP;
    public int currentInitiative;
    public int currentDamage;

    public int buffAmount;

    public bool hasHPBuff = false;



    public void Initiate()
    {
        MyStatTable = StatLibraryIO.Instance.GetStatTableByID(HeroID);

        currentHP = maxHP = currentMaxHP = MyStatTable.maxHP;
        currentInitiative = defaultInitialive = MyStatTable.initiative;
        currentDamage = defaultDamage = MyStatTable.damage;

        buffAmount = MyStatTable.buff;

        RefreshText();
    }

    public void RefreshText()
    {
        text.text = currentHP.ToString(); 
    }

    public abstract void Attack(List<Hero> targets);
    public abstract void Buff(List<Hero> targets);

    public void Death() {
        TurnComponent.Instance.turnList.Remove(this);
        mySlot.myHero = null;
        Destroy(this.gameObject);
        
    }
}
