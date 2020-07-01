using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public float XworldPos;
    public float ZworldPos;

    public int myRow;
    public int myPlace;

    public Affiliation side;

    public BattleGrid myGrid;
    public Hero myHero = null;

    private void Awake()
    {
        XworldPos = this.gameObject.transform.position.x;
        ZworldPos = this.gameObject.transform.position.z;
    }

    private void OnMouseOver()
    {
        Controller.Instance.ChooseTargets();
    }
}
