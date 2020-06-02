using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMaker : MonoBehaviour
{

    public GameObject SlotPrefab;
    public GameObject lowerRuler;
    public GameObject upperRuler;

    public BattleGrid myGrid;
    public int gridRows;
    public int gridPlaces;
    public Affiliation side;

    private float gridWidth;
    private float gridHeight;

    public float slotHalfWidth;
    public float slotHalfHeight;

    public void Initiate()
    {
        myGrid = new BattleGrid(side, gridPlaces, gridRows);

        gridWidth = upperRuler.transform.position.x - lowerRuler.transform.position.x;
        gridHeight = upperRuler.transform.position.z - lowerRuler.transform.position.z;

        slotHalfWidth = (gridWidth / myGrid.rows) / 2;
        slotHalfHeight = (gridHeight / myGrid.placesInRows) / 2;
    }

    void writeGrid(Affiliation x, BattleGrid grid)
    {
        switch (x)
        {
            case Affiliation.Ally:
                TurnComponent.Instance.TeamOneGrid = grid;
                break;
            case Affiliation.Enemy:
                TurnComponent.Instance.TeamTwoGrid = grid;
                break;
        }
    }

    public void MakeSlotGrid() {
        lowerRuler.GetComponent<MeshRenderer>().enabled = false;
        upperRuler.GetComponent<MeshRenderer>().enabled = false;



        for (int k = 0; k < myGrid.rows; k++) {
            for (int l = 0; l < myGrid.placesInRows; l++) {
                GameObject cloneSlotObj = Instantiate(
                    SlotPrefab, 
                    new Vector3(
                        slotHalfWidth * (2 * k + 1) + lowerRuler.transform.position.x, 
                        0,
                        slotHalfHeight * (2 * l + 1) + lowerRuler.transform.position.z),
                    Quaternion.identity
                    );
                Slot cloneSlotEntity = cloneSlotObj?.GetComponent<Slot>();

                myGrid.Slots.Add(cloneSlotEntity);

                cloneSlotEntity.myRow = k+1;
                cloneSlotEntity.myPlace = l+1;
                cloneSlotEntity.side = this.side;
                cloneSlotEntity.myGrid = this.myGrid;
                cloneSlotEntity.Setup();
            }
        }

        writeGrid(side, this.myGrid);

        if (myGrid.Slots.Count < gridRows * gridPlaces) {
            Debug.Log("Not all slots created/added to List: " + myGrid.Slots.Count);
        }

    }

}
