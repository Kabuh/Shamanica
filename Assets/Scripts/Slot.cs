using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public float xWorldPos;
    public float zWorldPos;

    public int myRow;
    public int myPlace;

    public Affiliation side;

    public BattleGrid myGrid;
    public Hero myHero;

    bool filled;

    public void Setup()
    {
        xWorldPos = this.gameObject.transform.position.x;
        zWorldPos = this.gameObject.transform.position.z;
    }

    private void OnMouseOver()
    {
        Controller.Instance.ChooseTargets();
    }
}
