using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnComponent : MonoBehaviour
{
    public static TurnComponent Instance;

    public List<int> HeroIDs = new List<int>();
    public List<GameObject> CharPrefabs = new List<GameObject>();

    Dictionary<int, GameObject> elementDict = new Dictionary<int, GameObject>();

    public GameObject CharPrefab;
    public GameObject CurrentTurnIndicator;

    public BattleGrid TeamOneGrid;
    public BattleGrid TeamTwoGrid;

    List<Hero> PlayerList;
    public  List<Hero> AIList;

    public List<Hero> turnList = new List<Hero>();

    Hero currentTurnHero;
    int classTurnCounter = 0;

    public GameObject gameOverTextGO;
    TextMeshProUGUI gameOverText;

    public int[] AllyIDHeroList = new int[8];
    public int[] EnemyIDHeroList = new int[8];

    private void Awake()
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

        for (int index = 0; index < HeroIDs.Count; index++) {
            elementDict.Add(HeroIDs[index], CharPrefabs[index]);
        }

        PlayerList = new List<Hero>();
        AIList = new List<Hero>();

        gameOverText = gameOverTextGO.GetComponent<TextMeshProUGUI>();
    }

    //spawns character gameobjects on allready created slot-grids for both team
    public void PreGameSetup() {
        
        SpawnChars(AllyIDHeroList, TeamOneGrid, out PlayerList);
        SpawnChars(EnemyIDHeroList, TeamTwoGrid, out AIList);
        SortLists();

        AIHandler.AIallies = TeamTwoGrid;
        AIHandler.AIenemies = TeamOneGrid;

    }

    Quaternion heroSpawnRotation = new Quaternion();
    
    //func about how to spawn characters based on array of IDs
    void SpawnChars(int[] IDsArray, BattleGrid currentGrid, out List<Hero> list) {
        list = new List<Hero>();
        int currentHeroID;

        heroSpawnRotation = CharPrefab.transform.rotation;
        for (int index = 0; index < IDsArray.Length; index++) {
            if (IDsArray[index] > 0) {
                currentHeroID = IDsArray[index];
                CharPrefab = elementDict[currentHeroID];

                GameObject character = Instantiate(
                    CharPrefab,
                    new Vector3(currentGrid.Slots[index].xWorldPos, 0, currentGrid.Slots[index].zWorldPos),
                    heroSpawnRotation
                    );
                Hero currentHero = character.GetComponent<Hero>();
                currentHero.HeroID = currentHeroID;
                currentHero.Initiate();
                currentHero.mySlot = currentGrid.Slots[index];
                currentGrid.Slots[index].myHero = currentHero;
                list.Add(currentHero);
                turnList.Add(currentHero);
            }
        }
    }

    //function that can be called to resort character turn data after beff/debuff to initiative(speed) stat
    public void SortLists() {
        turnList = Sorter(turnList);
    }

    //ad-hoc solution to actually sort objects based on the parameter needed by my project
    //needs rewrite if List.Sort has better solution
    List<Hero> Sorter(List<Hero> list) {
        List<Hero> tempList = new List<Hero>();
        foreach (Hero item in list) {
            int index = -1;
            if (tempList.Count == 0)
            {
                tempList.Add(item);
            }
            else {
                for (int i = 0; i < tempList.Count; i++)
                {
                    if (item.currentInitiative >= tempList[i].currentInitiative)
                    {
                        index = i;
                        i = tempList.Count;
                        tempList.Insert(index, item);
                        break;
                    } 
                    
                }
                if (index == -1) {
                    tempList.Add(item);
                }
            }
        }
        return tempList;
    }

    //dafault game turn behaviour
    public void MakeGameTurn() {

        //placeholder for battle ending
        if (PlayerList.Count == 0 || AIList.Count == 0)
        {
            gameOverTextGO.SetActive(true);
            return;
        }

        currentTurnHero = turnList[classTurnCounter];
        CurrentTurnIndicator.SetActive(true);
        Vector3 indicatorSetter = currentTurnHero.gameObject.transform.position;
        CurrentTurnIndicator.transform.position = new Vector3(indicatorSetter.x, 2, indicatorSetter.z);
        

        if (currentTurnHero.mySlot.side == Affiliation.Ally) {
            classTurnCounter++;
            if (classTurnCounter >= turnList.Count)
            {
                classTurnCounter = 0;
            }
        } else 
        {
            AIHandler.ChooseBehaviour(currentTurnHero, AIList, PlayerList);

            classTurnCounter++;
            if (classTurnCounter >= turnList.Count)
            {
                classTurnCounter = 0;
            }

            

            MakeGameTurn();
        }
    }

    //call for abstract methods of Buff/Attack based on character class
    public void MakeAction() {
        if (CurrentTurnIndicator.activeSelf == true) {
            CurrentTurnIndicator.SetActive(false);

            if (currentTurnHero?.mySlot.myGrid.side == Controller.Instance.ChosenHeroes[0].mySlot.myGrid.side)
            {
                currentTurnHero.Buff(Controller.Instance.ChosenHeroes);
            }
            else {
                currentTurnHero.Attack(Controller.Instance.ChosenHeroes);
            }
            MakeGameTurn();
        }
    }

    public void PurgeFromList(Hero hero) {

        turnList.Remove(hero);
        if (hero.mySlot.side == Affiliation.Ally)
        {
            PlayerList.Remove(hero);
        }
        else
        {
            AIList.Remove(hero);
        }
    }
}
