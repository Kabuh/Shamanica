using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Hero : MonoBehaviour
{
    public TextMeshProUGUI text;

    public int HeroID = 0;
    StatTable MyStatTable;

    public Slot mySlot;

    private int defaultMaxHP;
    private int defaultInitialive;
    private int defaultDamage;

    public int MaxHP { get => defaultMaxHP; }
    public int DefaultInitialive { get => defaultInitialive; }
    public int DefaultDamage { get => defaultDamage; }


    public int CurrentHP { get; set; }
    public int CurrentMaxHP { get; set; }
    public int CurrentInitiative { get; set; }
    public int CurrentDamage { get; set; }

    public int BuffAmount;
    public string TargetType = "cubical";

    //
    public bool HasHPBuff = false;


    public GameObject mySprite;
    public GameObject buffSprite;
    public GameObject fireAttackSprite;



    public void Initiate()
    {
        MyStatTable = StatLibraryIO.Instance.GetStatTableByID(HeroID);

        CurrentHP = defaultMaxHP = CurrentMaxHP = MyStatTable.maxHP;
        CurrentInitiative = defaultInitialive = MyStatTable.initiative;
        CurrentDamage = defaultDamage = MyStatTable.damage;

        BuffAmount = MyStatTable.buff;

         RefreshText();
    }

    public void RefreshText()
    {
        text.text = CurrentHP.ToString(); 
    }

    public void TextBuff() {
        text.color = Color.green;
    }

    public abstract void Attack(List<Hero> targets);
    public abstract void Buff(List<Hero> targets);

    public void BuffAnimate() {
        AnimationsFX.Instance.AnimateSprite(buffSprite, this, false);
    }

    public void AttackAnimate() {
        AnimationsFX.Instance.AnimateSprite(fireAttackSprite, this, true);
    }

    public void Death() {
        TurnComponent.Instance.PurgeFromList(this);
        mySlot.myHero = null;
        Destroy(this.gameObject);
        
    }
}
