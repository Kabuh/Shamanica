using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public static Controller Instance;

    Slot currentSlot;
    Slot lastRememberedSlot;

    public GameObject teamOneSlotmaker;
    public GameObject teamTwoSlotmaker;
    SlotMaker currentSlotmaker;
    MeshRenderer currentMaterial;

    public GameObject selector;

    BattleGrid currentGrid;

    List<Slot> chosenSlots = new List<Slot>();
    public List<Hero> ChosenHeroes = new List<Hero>();

    Dictionary <Affiliation, SlotMaker> rulerDict = new Dictionary<Affiliation, SlotMaker>();

    Ray ray;
    int direction = 1;

    void setDirection(int destination, int origin) {
        if (destination >= origin)
        {
            direction = 1;
        }
        else {
            direction = - 1;
        }
    }

    // is there a solution to make it event instead of Update?

    //private Button mousebutton; 
    //private void Start()
    //{
    //    Input.GetMouseButtonDown(0).AddListener
    //}


    void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        #endregion

        rulerDict.Add(Affiliation.Ally, teamOneSlotmaker.GetComponent<SlotMaker>());
        rulerDict.Add(Affiliation.Enemy, teamTwoSlotmaker.GetComponent<SlotMaker>());
    }

    //method that handles how targets are chosen and how to visualise it 
    // maybe i gotta separate visual and functional part...
    public void ChooseTargets() {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            lastRememberedSlot = hit.collider.gameObject.GetComponent<Slot>();
            
            if (currentSlot == null)
            {
                currentSlot = lastRememberedSlot.myGrid.Slots[0];
            }

        }

        if (currentSlot != null && !chosenSlots.Contains(lastRememberedSlot))
        {
            setDirection(lastRememberedSlot.myPlace, currentSlot.myPlace);

            currentGrid = currentSlot.myGrid;
            currentSlotmaker = rulerDict[currentGrid.side];
            
            currentMaterial = selector.GetComponent<MeshRenderer>();
            if (currentGrid.side == Affiliation.Ally)
            {
                selector.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", Color.green);
            }
            else if (currentGrid.side == Affiliation.Enemy)
            {
                selector.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", Color.red);
            }

            int checkPlace = Mathf.Clamp(currentSlot.myPlace, 1, 3);

            chosenSlots.Clear();
            chosenSlots.Add(currentGrid.GetSlotByCoord(1, checkPlace));
            chosenSlots.Add(currentGrid.GetSlotByCoord(2, checkPlace));
            chosenSlots.Add(currentGrid.GetSlotByCoord(1, checkPlace + 1));
            chosenSlots.Add(currentGrid.GetSlotByCoord(2, checkPlace + 1));

            selector.SetActive(true);
            selector.transform.position = new Vector3(
                currentSlotmaker.lowerRuler.transform.position.x + 2 * currentSlotmaker.slotHalfWidth,
                0.5f,
                currentSlotmaker.lowerRuler.transform.position.z + (2 * checkPlace) * currentSlotmaker.slotHalfHeight
                );


            currentSlot = lastRememberedSlot.myGrid.GetSlotByCoord(1, currentSlot.myPlace + direction);
        }
    }



    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (selector.gameObject.activeSelf) {
                MakeHeroList(chosenSlots);
                TurnComponent.Instance.MakeAction();
                ChosenHeroes.Clear();
            }  
        }
    }

    //outputs smaller list throwing away empty slots
    void MakeHeroList(List<Slot> slots)
    {
        foreach (Slot item in slots)
        {
            if (item.myHero != null)
            {
                ChosenHeroes.Add(item.myHero);
            }
        }
    }
}
