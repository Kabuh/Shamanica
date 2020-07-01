using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//main
public class GameManager : MonoBehaviour
{
    public SlotPlacer AllySlotPlacer;
    public SlotPlacer EnemySlotPlacer;

    public GameObject Game;

    void Start()
    {
        StatLibraryIO.Instance.Init();

        AllySlotPlacer.Initiate();
        EnemySlotPlacer.Initiate();

        AllySlotPlacer.MakeSlotGrid();
        EnemySlotPlacer.MakeSlotGrid();

        TurnComponent.Instance.PreGameSetup();
        TurnComponent.Instance.MakeGameTurn();
    }
}
