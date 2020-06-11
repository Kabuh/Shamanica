using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initiator : MonoBehaviour
{
    public GameObject FileReader;

    public GameObject AllySlotMaker;
    public GameObject EnemySlotMaker;

    SlotMaker allySlotMakerScript;
    SlotMaker enemySlotMakerScript;

    public GameObject Game;

    void Start()
    {
        if (StatLibraryIO.Instance.readComplete) {
            allySlotMakerScript = AllySlotMaker.GetComponent<SlotMaker>();
            enemySlotMakerScript = EnemySlotMaker.GetComponent<SlotMaker>();

            allySlotMakerScript.Initiate();
            enemySlotMakerScript.Initiate();

            allySlotMakerScript.MakeSlotGrid();
            enemySlotMakerScript.MakeSlotGrid();

            TurnComponent.Instance.PreGameSetup();
            TurnComponent.Instance.MakeGameTurn();
        }
    }
}
