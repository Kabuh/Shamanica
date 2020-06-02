using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIHandler
{
    static Hero AIhero;
    public static List<Hero> targets;


    

    static void ChooseBehaviour(Hero heroToCalculate, List<Hero> allyList, List<Hero> enemyList) {
        AIhero = heroToCalculate;
        float lowestEnemyHealthPartition;
        float lowestFriendHealthPartition;

        checkPartition(enemyList, out lowestEnemyHealthPartition);
        checkPartition(allyList, out lowestFriendHealthPartition);

        List<Hero> targets;

        if (lowestEnemyHealthPartition < lowestFriendHealthPartition)
        {
            MakeAttackZone(AIhero, out targets);
            AIhero.Attack(targets);
        }
        else {
            AIhero.Buff();
        }
    }

    static void MakeAttackZone(Hero hero, out List<Hero> targets)
    {

    }

    static void MakeBuffZone(Hero hero, out List<Hero> targets)
    {

    }

    static void checkPartition(List<Hero> list, out float partition) {
        float lowestHealthPartition = 101.0f;
        partition = lowestHealthPartition;
        foreach (Hero item in list)
        {
            partition = item.currentHP / item.currentMaxHP;
            if (partition < lowestHealthPartition)
            {
                lowestHealthPartition = partition;
            }
        }
    }

}

