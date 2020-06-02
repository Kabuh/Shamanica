using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGrid
{
    public Affiliation side;
    public int rows;
    public int placesInRows;

    public int SlotsTaken { get { return Slots.Count; }}

    public List<Slot> Slots = new List<Slot>();

    public BattleGrid(Affiliation affiliation, int setHeight, int setWidth) {
        side = affiliation;
        rows = setWidth;
        placesInRows = setHeight;
    }

    public Slot GetSlotByCoord(int row, int place) {
        int index = (place+((row - 1) * placesInRows))-1;       // if 3rd place in 2nd row => 4places 1st in row + 3 => additional -1 for zero index;
        Slot slot;
        if (index <= Slots.Count) {
             slot = Slots[index];
            return slot;
        }
        slot = null;
        return slot;
    }

    //public void DebugSlots() {
    //    for (int i = 0; i < Slots.Count; i++) {
    //        Debug.Log("SlotN:" + i + " x=" + Slots[i].myRow + " y=" + Slots[i].myPlace);
    //    }
    //}       
}
