using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIHandler
{
    static Hero AIhero;
    public static List<Hero> targets;

    public static BattleGrid AIallies;
    public static BattleGrid AIenemies;




    public static void ChooseBehaviour(Hero heroToCalculate, List<Hero> allyList, List<Hero> enemyList) {
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
            MakeAttackZone(AIhero, lowestHealthEnemy, out targets, enemyList);
            AIhero.Attack(targets);
        }
        else {
            if (friendThatNeedsBuff.hasHPBuff)
            {
                CheckBuff(allyList, out friendThatNeedsBuff);
            }

            if (friendThatNeedsBuff != null) {
                MakeBuffZone(AIhero, friendThatNeedsBuff, out targets);
                AIhero.Buff(targets);
            } else
            {
                MakeAttackZone(AIhero, lowestHealthEnemy, out targets, enemyList);
                AIhero.Attack(targets);
            }
        }
    }

    static void MakeAttackZone(Hero hero, Hero initialTarget, out List<Hero> targets, List<Hero> enemyList)
    {
        targets = new List<Hero>();
        int place;
        if (initialTarget != null)
        {
            place = Mathf.Clamp(initialTarget.mySlot.myPlace, 1, 3);
        } else { 
            place = Mathf.Clamp(enemyList[0].mySlot.myPlace, 1, 3);                     //default behaviour, change later to attack lowest max HP target
        }

        //make into square-shape target function
        targets.Add(AIenemies.GetSlotByCoord(1, place).myHero);
        targets.Add(AIenemies.GetSlotByCoord(2, place).myHero);
        ++place;
        targets.Add(AIenemies.GetSlotByCoord(1, place).myHero);
        targets.Add(AIenemies.GetSlotByCoord(2, place).myHero);
    }

    static void MakeBuffZone(Hero hero, Hero initialTarget, out List<Hero> targets)
    {
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

        //make into square-shape target function
        targets.Add(AIallies.GetSlotByCoord(1, place).myHero);
        targets.Add(AIallies.GetSlotByCoord(2, place).myHero);
        ++place;
        targets.Add(AIallies.GetSlotByCoord(1, place).myHero);
        targets.Add(AIallies.GetSlotByCoord(2, place).myHero);
    }

    static void CheckPartition(List<Hero> list, out float partition, out Hero hero) {
        float lowestHealthPartition = 101.0f;
        partition = lowestHealthPartition;
        hero = null;
        foreach (Hero item in list)
        {
            lowestHealthPartition = item.currentHP / item.currentMaxHP;
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
            if (item.hasHPBuff) {
                hero = item;
            }
        }
    }

}

