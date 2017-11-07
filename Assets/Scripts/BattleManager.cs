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
    

    private List<Transform> playerPositions;
    private List<Transform> enemyPositions;

    private List<GameObject> players;
    private List<GameObject> enemies;

    private List<GameObject> turnOrder;

    private int turn = -1;

    private Dropdown attackMenu;//selectors for attacks
 //   private Dropdown targetMenu;//selector for targets 

    private GameObject infoPane;
    // public InputField input;//TODO: Bad use something else obviously 

    public LayerMask inputMask;//masks out UI and other layers from the click input

    Entity current;
    Entity currentTarget;

    int currentAttack = 0;
    //int currentTarget = 0;

    bool isAiTurn = false;

    float waitTimer=0f;

    float aiDelay = 3f;
    float delayTimer = 0f;
    void Start()
    {
        attackMenu = GameObject.FindGameObjectWithTag("AttackMenu").GetComponent<Dropdown>();
      //  targetMenu = GameObject.FindGameObjectWithTag("TargetMenu").GetComponent<Dropdown>();
        infoPane = GameObject.FindGameObjectWithTag("InfoPane");

        infoPane.SetActive(false);

        instance = this;
        players = new List<GameObject>();
        enemies = new List<GameObject>();
        turnOrder = new List<GameObject>();

        placeEntities();//no game objects exists before this     

        List<string> enemyNames = new List<string>();
        foreach(GameObject enemy in enemies)
        {
            enemyNames.Add(enemy.GetComponent<Entity>().eName);
        }
       // targetMenu.AddOptions(enemyNames);
      
        turnOrder.AddRange(players);
        turnOrder.AddRange(enemies);

        turnOrder = turnOrder.OrderByDescending(x => x.GetComponent<Entity>().getSpeed()).ToList();//sort by speed

        turnMarker = Instantiate(turnMarker);

        advanceTurn();//set turn to zero
        
        
    }
    //TODO: Fix issues with updating infopanel on turn swap.
	
	// Update is called once per frame
	void Update () {
        //if (isAiTurn) aiTurn();//run the ai turn if applicable

        //waitTimer += Time.deltaTime;//update time

        if(isAiTurn)
        {
            delayTimer += Time.deltaTime;
        }
        else
        {
            checkClicked();
        }
        if(delayTimer>=aiDelay)
        {
            delayTimer = 0;
            aiTurn();
        }

       
	}

    public void checkClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {           
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f,inputMask))
            {
                Debug.Log(hit.collider.gameObject.name);
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
            enemies.Add(createEntity(NameGen.getName(), current));               
        }


        foreach (Transform current in playerPositions)
        {
            players.Add(createEntity(NameGen.getName(), current));
        }
    }

     public void HelpCallback(int selection)
    {
        switch (selection)
        {
            case 0:
                setInfoPaneText(current.attacks[currentAttack].ToString());                
                break;
            case 1:
                setInfoPaneText(currentTarget.ToString());               
                break;
            
        }
    }

    public void toggleExtraInfo()
    {
        infoPane.SetActive(!infoPane.activeInHierarchy);
    }
    private void setInfoPaneText(string text)
    {
        infoPane.GetComponentInChildren<Text>().text = text;
    }
    public void attackMenuAction(int selected)
    {
        currentAttack = selected;
        HelpCallback(0);
    }
    public void targetMenuAction(int selected)
    {
       // deselectTarget(currentTarget);
       // currentTarget = selected;
        //selectTarget(currentTarget);
       // HelpCallback(1);

    }

    public void targetClicked(Entity clicked)
    {
        if (clicked == null) { return; }
        deselectTarget(currentTarget);
        currentTarget = clicked;
        selectTarget(currentTarget);
        HelpCallback(1);
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

    public void buttonAction(int action)
    {
        clearHighlights();
        attack(current, currentTarget, current.attacks[currentAttack]);
             
        advanceTurn();
       
    }
   
  
    public void attack(Entity attacker, Entity defender, Attack attack)
    {
        AttackResult res = attacker.attackOther(defender, attack);
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

           

            isAiTurn = enemies.Contains(turnOrder[turn]);

            if (isAiTurn){ clearHighlights(); }

            targetClicked(getFirstEnemyTarget());

            turnMarker.transform.position = current.transform.position + new Vector3(0, 2, 0);

            foreach (GameObject currentEntity in turnOrder)
            {
                Entity e = currentEntity.GetComponent<Entity>();
               e.turnTick(current.Equals(e));
            }

            if (current.isDead())
            {
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

            attackMenu.options.Clear();//clear previous attacks
            List<string> attackNames = new List<string>();
            foreach(Attack attack in current.attacks)
            {
                attackNames.Add(attack.name);
            }
            attackMenu.AddOptions(attackNames);

           // selectTarget(currentTarget);//TODO: temp try a better fix
            HelpCallback(0);
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

    private bool playerLost()
    {
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
        enemyUI.SetActive(true);
        enemyUI.GetComponentInChildren<Text>().text = "Match Over! You " +( won ? "won" : "lost");
        //  Application.Quit();

        //TODO: Terible hacks, currently the game crashes unity on game over need to fix

#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif


    }

}
