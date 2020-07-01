using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIHandler
{
    static Hero AIhero;
    public static List<Hero> targets;

    public static BattleGrid AIalliesGrid;
    public static BattleGrid AIenemiesGrid;

    delegate void TargetMaker(BattleGrid grid, Hero initialTarget, out List<Hero> targets);
    static TargetMaker Cubical = new TargetMaker(CubicalTarget);

    static Dictionary<string, TargetMaker> TargetFieldType = new Dictionary<string, TargetMaker> {
        { "cubical", Cubical }
    };


    public static void ChooseBehaviour(Hero heroToCalculate, List<Hero> allyList, List<Hero> enemyList, out int targetAmount) {
        AIhero = heroToCalculate;
        float lowestEnemyHealthPartition;
        Hero lowestHealthEnemy;
        float lowestFriendHealthPartition;
        Hero friendThatNeedsBuff;

        CheckPartition(enemyList, out lowestEnemyHealthPartition, out lowestHealthEnemy);
        CheckPartition(allyList, out lowestFriendHealthPartition, out friendThatNeedsBuff);
        


        List<Hero> targets;

        if (lowestEnemyHealthPartition <= lowestFriendHealthPartition)
        {
            Debug.Log("gonna attack that weak fool");
            if (lowestHealthEnemy == null) {
                lowestHealthEnemy = enemyList[0];
            }
            TargetFieldType[AIhero.TargetType](AIenemiesGrid, lowestHealthEnemy, out targets);
            targetAmount = targets.Count;
            AIhero.Attack(targets);
        }
        else {
            if (friendThatNeedsBuff.HasHPBuff)
            {
                Debug.Log("gonna prepare to protect my buddy");
                CheckBuff(allyList, out friendThatNeedsBuff);
            }

            if (friendThatNeedsBuff != null) {
                Debug.Log("protecting my buddy");
                TargetFieldType[AIhero.TargetType](AIalliesGrid, friendThatNeedsBuff, out targets);
                targetAmount = targets.Count;
                AIhero.Buff(targets);
            } else
            {
                Debug.Log("nothing else to do but atack");
                if (lowestHealthEnemy == null){
                    lowestHealthEnemy = enemyList[0];
                }
                TargetFieldType[AIhero.TargetType](AIenemiesGrid, lowestHealthEnemy, out targets);
                targetAmount = targets.Count;
                AIhero.Attack(targets);
            }
        } 
    }

    //static void MakeAttackZone(Hero hero, Hero initialTarget, out List<Hero> targets, List<Hero> enemyList)
    //{
    //    targets = new List<Hero>();
    //    int place;
    //    if (initialTarget != null)
    //    {
    //        place = Mathf.Clamp(initialTarget.mySlot.myPlace, 1, 3);
    //    } else { 
    //        place = Mathf.Clamp(enemyList[0].mySlot.myPlace, 1, 3);                     //default behaviour, change later to attack lowest max HP target
    //    }

    //    //make into square-shape target function
    //    TryAddTarget(AIenemies.GetSlotByCoord(1, place));
    //    TryAddTarget(AIenemies.GetSlotByCoord(2, place));
    //    ++place;
    //    TryAddTarget(AIenemies.GetSlotByCoord(1, place));
    //    TryAddTarget(AIenemies.GetSlotByCoord(2, place));
    //}

    //static void MakeBuffZone(Hero hero, Hero initialTarget, out List<Hero> targets)
    //{
    //    targets = new List<Hero>();
    //    int place = 0;
    //    if (initialTarget != null)
    //    {
    //        place = Mathf.Clamp(initialTarget.mySlot.myPlace, 1, 3);
    //    }
    //    else
    //    {
    //        Debug.LogError("unintended AI solution");
    //        return;
    //    }

    //    //make into square-shape target function

        
    //}

    static void CubicalTarget(BattleGrid grid, Hero initialTarget, out List<Hero> targets) {
        targets = new List<Hero>();
        int place = 0;
        if (initialTarget != null)
        {
            place = Mathf.Clamp(initialTarget.mySlot.myPlace, 1, 3);
        }
        else
        {
            Debug.LogError("unintended AI solution");
            return;
        }


        List<Slot> slots = new List<Slot>();

        slots.Add(grid.GetSlotByCoord(1, place));
        slots.Add(grid.GetSlotByCoord(2, place));
        place++;
        slots.Add(grid.GetSlotByCoord(1, place));
        slots.Add(grid.GetSlotByCoord(2, place));

        ExtractSlotHeroes(slots, out targets);
    }



    static void ExtractSlotHeroes(List<Slot> slots, out List<Hero> heroes) {
        heroes = new List<Hero>();

        foreach (Slot slot in slots) {
            if (slot.myHero != null) {
                heroes.Add(slot.myHero);
            }
        }
    }

    static void RemoveNulls(Slot chosenSlot) {
        if (chosenSlot.myHero != null) {
            targets.Add(chosenSlot.myHero);
        }
    }

    static void CheckPartition(List<Hero> list, out float partition, out Hero hero) {
        float lowestHealthPartition = 101.0f;
        partition = lowestHealthPartition;
        hero = null;
        foreach (Hero item in list)
        {
            lowestHealthPartition = item.CurrentHP / item.CurrentMaxHP;
            if (lowestHealthPartition < partition)
            {
                partition = lowestHealthPartition;
                hero = item;
            }
        }
    }

    static void CheckBuff(List<Hero> list, out Hero hero)
    {
        hero = null;
        foreach (Hero item in list)
        {
            if (!item.HasHPBuff) {
                hero = item;
            }
        }
    }

}

