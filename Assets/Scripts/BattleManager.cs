﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

    int rounds = 0;
    bool battleOver = false;

    protected static BattleManager instance; // Needed
    public GameObject entityPrefab;

    public GameObject playerUI;
    public GameObject enemyUI;
    public GameObject SplashUI;

    public GameObject turnMarker;


    public List<Transform> playerPositions;
    public List<Transform> enemyPositions;

    public List<GameObject> allPlayers;

    public List<GameObject> players;
    public List<GameObject> enemies;

    public List<GameObject> turnOrder;

    private int turn = -1;

    private GameObject selectedInfoPane;
    private GameObject leftPanel;
    private GameObject rightPanel;
    // public InputField input;//TODO: Bad use something else obviously 

    public LayerMask inputMask;//masks out UI and other layers from the click input

    Entity current;
    Entity currentTarget;

    int currentAttack = 0;
    //int currentTarget = 0;

    bool isAiTurn = false;
    bool playerReady = false;
    bool destroyReady = true;
    bool gameReady = true;//Is the game ready to be played

    float waitTimer=0f;

    float aiDelay = 3f;
    float delayTimer = 0f;

    float gameOverDelay = 5f;
    float gameOverTimer = 0f;

    private SoundRequest audioSystem;

    public List<GameObject> stagePrefabs;
    private GameObject currentStage;

    public void resetStage()
    {
        rounds++;
        players = new List<GameObject>();       
        enemies = new List<GameObject>();
        playerPositions = new List<Transform>();
        enemyPositions = new List<Transform>();
        turnOrder = new List<GameObject>();

        setupStage();
        fillPositions();
        placeEntities();//no game objects exists before this  
        
        List<string> enemyNames = new List<string>();

        foreach (GameObject enemy in enemies)
        {
            enemyNames.Add(enemy.GetComponent<Entity>().eName);
        }

        turnOrder.AddRange(players);
        turnOrder.AddRange(enemies);

        turnOrder = turnOrder.OrderByDescending(x => x.GetComponent<Entity>().getSpeed()).ToList();//sort by speed
        turn = -1;

         current=null;
         currentTarget=null;
         currentAttack = 0;  

         isAiTurn = false;
         waitTimer = 0f;       
         delayTimer = 0f;
        gameOverTimer = 0f;
        battleOver = false;
        playerReady = false;

        SplashUI.SetActive(false);

        audioSystem.requestSong("Combat");
        advanceTurn();//set turn to zero
    }
    private void detatchPlayers()
    {
        if(players.Count>0)
        {
            foreach (Transform current in playerPositions)
            {
                current.DetachChildren();
            }
        }
       for(int i=0;i<allPlayers.Count; i++)
        {
            allPlayers[i].SetActive(false);
        }
    }

    public void destroyStage()
    {
        detatchPlayers();
        Destroy(currentStage);
    }

    public void setupStage()
    {
        GameObject stagePrefab = stagePrefabs[Calculator.rand.Next(0, stagePrefabs.Count)]; //   


        currentStage = Instantiate(stagePrefab);
        currentStage.transform.position = new Vector3(0, 0, 0);//center the stage


        GameObject cameraObj = GameObject.FindGameObjectWithTag("Camera Marker");

        Transform cameraPos = cameraObj == null ? null : cameraObj.transform;

        if (cameraPos == null)
        {
            Debug.LogError("This stage doesn't have a camera marker set up: " + this.name);
        }
        placeCamera(cameraPos); 

    }
   
    public void placeCamera(Transform newPos)
    {
        Camera.main.transform.position = newPos.position;
        Camera.main.transform.rotation = newPos.rotation;

    }
    public void fillPositions()
    {
            for (int i = 1; i <= 4; i++)//search for Players
            {
                GameObject playerSearch = GameObject.Find("P" + i);
                if (playerSearch != null)
                {
                    playerPositions.Add(playerSearch.transform);
                }

            }
        
      

        for (int i = 1; i <= 4; i++)//search for Enemies
        {
            GameObject playerSearch = GameObject.Find("E" + i);
            if (playerSearch != null)
            {
                enemyPositions.Add(playerSearch.transform);
            }

        }       

    }

    void Start()
    {
      //  targetMenu = GameObject.FindGameObjectWithTag("TargetMenu").GetComponent<Dropdown>();
        selectedInfoPane = GameObject.FindGameObjectWithTag("InfoPane");
        leftPanel = GameObject.FindGameObjectWithTag("LeftPane");
        rightPanel = GameObject.FindGameObjectWithTag("RightPane");
        
        audioSystem = GameObject.FindObjectOfType<SoundRequest>();
        if(audioSystem==null)
        {
            Debug.LogError("No sound manager was found!");
        }

       // selectedInfoPane.SetActive(false);

        instance = this;

        turnMarker = Instantiate(turnMarker);
        // targetMenu.AddOptions(enemyNames);
        resetStage();        
        
    }
    //TODO: Fix issues with updating infopanel on turn swap.
	
	// Update is called once per frame
	void Update () {
        //if (isAiTurn) aiTurn();//run the ai turn if applicable
        if (gameReady)
        {
            //uncomment to be able to end game on right click
            if (Input.GetMouseButtonDown(1))
            {
                gameOver(true);
            }


            if (!destroyReady)
            {
                destroyReady = true;
                resetStage();
                return;
            }

            if (battleOver)
            {
                if (currentStage != null)//a stage has been created 
                {
                    destroyStage();
                    destroyReady = false;

                }
                SplashUI.SetActive(false);
                gameOverTimer += Time.deltaTime;
                if (gameOverTimer >= gameOverDelay)
                {
                    // resetStage();             

                }
            }
            else
            {


                if (isAiTurn)
                {
                    delayTimer += Time.deltaTime;
                }
                else
                {
                    checkClicked();
                    if (playerReady)
                    {
                        playerReady = false;
                        advanceTurn();
                    }
                }
                if (delayTimer >= aiDelay)
                {
                    delayTimer = 0;
                    aiTurn();
                }

            }


        }
        else//handle pre game things
        {
            if (!SplashUI.activeInHierarchy)
            {
                playerUI.SetActive(false);
                enemyUI.SetActive(false);
                SplashUI.SetActive(true);
            }
        }

	}


    public void playerReadyToStart()
    {
        gameReady = true;
        allPlayers = GameObject.FindObjectOfType<battleOverManager>().getPlayers();
        SplashUI.SetActive(false);
    }

    public void checkClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {           
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f,inputMask))
            {

                if(hit.collider.gameObject.tag.Equals("Entity"))
                {                    
                    targetClicked(hit.collider.gameObject.GetComponent<Entity>());                    
                }                
            }
        }
    }

    public void placeEntities()
    {
        
        foreach(Transform current in enemyPositions)
        {
            enemies.Add(createEntity(NameGen.getName(), current,rounds* 50,false));               
        }


        if (allPlayers.Count == 0)//first time no players TODO: later we should have some char creation maybe
        {
            for(int i=0; i<4;i++)
            {
                allPlayers.Add(createEntity(NameGen.getName(),null ,255,true));
                allPlayers[i].SetActive(false);               
            }
        }

        
            for(int i=0;i<playerPositions.Count;i++)
            {
                 players.Add(allPlayers[i]);
                 players[i].transform.SetParent(playerPositions[i]);
                 players[i].transform.localPosition = new Vector3(0, 0, 0);
                 players[i].SetActive(true);
            }      
    }

    public void toggleExtraInfo()
    {
        selectedInfoPane.SetActive(!selectedInfoPane.activeInHierarchy);
    }

    private void setInfoPaneText(string text)
    {
        selectedInfoPane.GetComponentInChildren<Text>().text = text;
    }

    private void setLeftPanelText(string text)
    {
        leftPanel.GetComponentInChildren<Text>().text = text;
    }

    private void setRightPanelText(string text)
    {
        rightPanel.GetComponentInChildren<Text>().text = text;
    }

    public void targetClicked(Entity clicked)
    {
        if (clicked == null) { return; }
        if (clicked.isDead()) { return; }//can't click dead entities

        deselectTarget(currentTarget);
        currentTarget = clicked;
        selectTarget(currentTarget);
        setInfoPaneText(currentTarget.ToString());
    }

    private void selectTarget(Entity target)
    {
        if (target == null) { return; }

        target.gameObject.GetComponent<cakeslice.Outline>().enabled = true;
     
    }

    private void deselectTarget(Entity target)
    {
        if (target == null) { return; }
        target.gameObject.GetComponent<cakeslice.Outline>().enabled = false;

    }

    public void clearHighlights()
    {
       
        deselectTarget(currentTarget);
    }

    public void leftButtonAction(int action)
    {
        playerAttackWith(current.leftArm);
        playerReady = true;
    }

    public void rightButtonAction(int action)
    {
        playerAttackWith(current.rightArm);
        playerReady = true;
    }

    public void infoButtonAction(int action)
    {
        setInfoPaneText(currentTarget.ToString());
        toggleExtraInfo();
    }

    public void playerAttackWith(Arm arm)
    {
        clearHighlights();
        attack(current, currentTarget, arm.attack);
    }

    public void attack(Entity attacker, Entity defender, Attack attack)
    {
        if(attack.usesLeft<=0)
        {
            attack = AttackList.getDefaultAttack();
        }
        else
        {
            attack.usesLeft--;
        }

        audioSystem.requestSound(attack.sound);
        AttackResult res = attacker.attackOther(defender, attack);
       foreach(string current in res.log)
        {
            Debug.Log(current);
        }
    }

    public Entity getFirstEnemyTarget()//returns the first entity with health remaining else NULL
    {
        foreach(GameObject current in enemies)
        {
            if (!current.GetComponent<Entity>().isDead())
            {
                return current.GetComponent<Entity>();
            }

        }
        return null;//Should never happen
    }

    public void advanceTurn()
    {  

        if(playerLost())
        {
            gameOver(false);
        }else if(aiLost())
        {
            gameOver(true);
        }else
        {

            targetClicked(currentTarget);

            turn++;
            //TODO: maybe bring these back
           // currentAttack = 0;
           // currentTarget = 0;

            if (turn > turnOrder.Count - 1)
            {
                turn = 0;
            }
            current = turnOrder[turn].GetComponent<Entity>();
            swapUI();

            setLeftPanelText(current.leftArm.attack.ToString());
            setRightPanelText(current.rightArm.attack.ToString());



            isAiTurn = enemies.Contains(turnOrder[turn]);

            if (isAiTurn){ clearHighlights(); }

            targetClicked(getFirstEnemyTarget());

            turnMarker.transform.position = current.transform.position + new Vector3(0, 7.5f, 0);

            foreach (GameObject currentEntity in turnOrder)
            {
                Entity e = currentEntity.GetComponent<Entity>();
               e.turnTick(current.Equals(e));
            }

            if (current.isDead())
            {
                //playerReady = true;
                advanceTurn();//skip this turn if a player is dead. will not infinite recurse as there must be at least once living char to reach this point
            }



        }   
      
    }

    public void aiTurn()
    {
        int target = aiTargetSelect();        
        attack(current, players[target].GetComponent<Entity>(), current.chooseAttack());
        advanceTurn();
    }

    private int aiTargetSelect()
    {
        int target = Calculator.rand.Next(0, players.Count);
        while (players[target].GetComponent<Entity>().isDead())
        {
            target = Calculator.rand.Next(0, players.Count);           
        }
        return target;
    }
    public void swapUI()
    {
        if (players.Contains(turnOrder[turn]))
        {
            playerUI.SetActive(true);
            enemyUI.SetActive(false);

            // selectTarget(currentTarget);//TODO: temp try a better fix
            selectedInfoPane.SetActive(false);
        }
        else
        {
            playerUI.SetActive(false);
            enemyUI.SetActive(true);
          
        }
    }


    public static GameObject createEntity(string name,Transform pos, int difficulty, bool player)
    {
        difficulty = 100; 
        GameObject newEntity;
        if (pos!=null)
        {
             newEntity = Object.Instantiate(instance.entityPrefab, pos);
            newEntity.GetComponent<Entity>().Initialize(name,difficulty);//TODO: use difficulty scaling
            newEntity.transform.position = pos.position;
            return newEntity;
        }
         newEntity = Object.Instantiate(instance.entityPrefab);
        newEntity.GetComponent<Entity>().Initialize(name, difficulty);           //TODO: use difficulty scaling 
        if(player)
        newEntity.GetComponent<Entity>().namebar.GetComponent<Text>().color = Color.green;
        return newEntity;

    }

    private List<Arm> lootEnemies()
    {
        List<Arm> loot = new List<Arm>();
        foreach (GameObject entity in enemies)
        {
            Arm lootedArm = entity.GetComponent<Entity>().dropLoot();
            if(lootedArm!=null)loot.Add(lootedArm);
        }
        return loot;
    }

 

    private bool playerLost()
    {
        Debug.Log(players.Count);
        foreach(GameObject curr in players)
        {
            if(!curr.GetComponent<Entity>().isDead())
            {
                return false;
            }
        }
        return true;
    }
    private bool aiLost()
    {
        foreach (GameObject curr in enemies)
        {
            if (!curr.GetComponent<Entity>().isDead())
            {
                return false;
            }
        }
        return true;
    }
    private void gameOver(bool won)
    {
        playerUI.SetActive(false);
        enemyUI.SetActive(false);
        SplashUI.SetActive(true);
        SplashUI.GetComponentInChildren<Text>().text = "Battle Over! You " +( won ? "won!" : "lost...");
        battleOver = true;
        audioSystem.stopAll();
        gameReady = false;

        GameObject.FindObjectOfType<battleOverManager>().loadData(allPlayers, lootEnemies());//TODO: Send in arm loot here
        if(!won)
        {
            GameObject.FindObjectOfType<GameFeedback>().logMessage("You lost! Your team survived "+ rounds +" rounds!");
            GameObject.FindObjectOfType<levelSwap>().loadGame("GameOver");
        }

    }

}
