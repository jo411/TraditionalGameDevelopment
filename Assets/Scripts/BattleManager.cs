using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

    


    protected static BattleManager instance; // Needed
    public GameObject entityPrefab;

    public GameObject playerUI;
    public GameObject enemyUI;

    public GameObject turnMarker;

    public List<Transform> playerPositions;
    public List<Transform> enemyPositions;

    private List<GameObject> players;
    private List<GameObject> enemies;

    private List<GameObject> turnOrder;

    private int turn = -1;

    public InputField input;//TODO: Bad use something else obviously 

    Entity current;

    bool isAiTurn = false;

    float waitTimer=0f;

    float aiDelay = 2f;
    float delayTimer = 0f;
    void Start()
    {

        instance = this;
        players = new List<GameObject>();
        enemies = new List<GameObject>();
        turnOrder = new List<GameObject>();

        placeEntities();//no game objects exists before this
        turnOrder.AddRange(players);
        turnOrder.AddRange(enemies);

        turnOrder = turnOrder.OrderByDescending(x => x.GetComponent<Entity>().getSpeed()).ToList();//sort by speed

        turnMarker = Instantiate(turnMarker);

        advanceTurn();//set turn to zero
        swapUI();
    }

	
	// Update is called once per frame
	void Update () {
        //if (isAiTurn) aiTurn();//run the ai turn if applicable

        //waitTimer += Time.deltaTime;//update time

        if(isAiTurn)
        {
            delayTimer += Time.deltaTime;
        }
        if(delayTimer>=aiDelay)
        {
            delayTimer = 0;
            aiTurn();
        }

	}

    public void placeEntities()
    {
        
        foreach(Transform current in enemyPositions)
        {
            enemies.Add(createEntity("Enemy Entity", current));               
        }


        foreach (Transform current in playerPositions)
        {
            players.Add(createEntity("Player Entity", current));
        }
    }

    public void buttonAction(int action)
    {
        string text = input.text;
        int target = 0;
        if(!int.TryParse(text, out target))
        {
            target = 0;
        }

        if (target < 0) target = 0;
        if (target > enemies.Count - 1) target = enemies.Count - 1;

        attack(current, enemies[target].GetComponent<Entity>(), current.attacks[action]);     
        advanceTurn();
    }
   
  
    public void attack(Entity attacker, Entity defender, Attack attack)
    {
        AttackResult res = attacker.attackOther(defender, attack);
    }


    public void advanceTurn()
    {
        turn++;
        if(turn>turnOrder.Count-1)
        {
            turn = 0;
        }
        current = turnOrder[turn].GetComponent<Entity>();
        swapUI();

       
            isAiTurn = enemies.Contains(turnOrder[turn]);

        turnMarker.transform.position = current.transform.position + new Vector3(0, 2, 0);
        Debug.Log("turn");
    }

    public void aiTurn()
    {      
        int target = Calculator.rand.Next(0, players.Count);
        attack(current, players[target].GetComponent<Entity>(), current.chooseAttack());
        advanceTurn();
    }

    public void swapUI()
    {
        if (players.Contains(turnOrder[turn]))
        {
            playerUI.SetActive(true);
            enemyUI.SetActive(false);
            
        }
        else
        {
            playerUI.SetActive(false);
            enemyUI.SetActive(true);
        }
    }


    public static GameObject createEntity(string name,Transform pos)
    {
        GameObject newEntity = Object.Instantiate(instance.entityPrefab,pos);
        newEntity.GetComponent<Entity>().Initialize(name);
        newEntity.transform.position = pos.position;
        return newEntity;
    }

    void waitForTime(float time)//TODO: This is bad bad bad probably. Do this without a busy loop
    {
        waitTimer = 0;
        while(waitTimer<time)
        {

        }

        
    }



}
