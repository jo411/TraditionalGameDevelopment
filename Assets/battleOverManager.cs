using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class battleOverManager : MonoBehaviour {
    List<GameObject> players;
    List<Arm> loot;
    public GameObject UI;
    public Dropdown lootDropDown;
    public Dropdown playerDropDown;
    public GameObject armDisplay;

    int currentArm = 0;
    int currentPlayer = 0;
	// Use this for initialization
	void Start () {
        players = new List<GameObject>();
      
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void loadData(List<GameObject> playerList, List<Arm>arms)
    {
        loot = new List<Arm>();
        players = new List<GameObject>();
        foreach(GameObject current in playerList)
        {
            players.Add(current);
        }

        foreach (Arm current in arms)
        {
            loot.Add(current);
        }
        //players = playerList;
        //loot = arms;

        currentArm = players.Count>0?0:-1;
        currentPlayer = loot.Count > 0 ? 0 : -1;
        displayData();
    }

    void displayData()
    {
        

        int count = 0;
        List<string> names = new List<string>();
        foreach (GameObject display in GameObject.FindGameObjectsWithTag("PlayerDisplay"))
        {
            string name = players[count].GetComponent<Entity>().eName;
            names.Add(name);
            display.GetComponentInChildren<Text>().text = name;
            count++;
        }
        playerDropDown.AddOptions(names);
        count = 0;
        foreach (GameObject pane in GameObject.FindGameObjectsWithTag("PlayerText"))
        {
            Text display = pane.GetComponentInChildren<Text>();
            display.text = players[count].GetComponent<Entity>().ToString()+"\nArms:\n" + "Right:\n"+players[count].GetComponent<Entity>().rightArm+"\n" + "Left:\n" + players[count].GetComponent<Entity>().leftArm + "\n";
            count++;
        }

        loadLoot();
    }
    public void loadLoot()
    {
        
        List<string> names = new List<string>();
        foreach (Arm current in loot)
        {
            current.attack.usesLeft = current.attack.uses;
            names.Add(current.attack.name);
        }
        lootDropDown.AddOptions(names);
    }


    public void armChosen(int index)
    {
        currentArm = lootDropDown.value;
        if (currentArm < 0) return;
        armDisplay.GetComponentInChildren<Text>().text = loot[currentArm].ToString();
    }

    public void playerChosen(int index)
    {
        Debug.Log(index);
        currentPlayer = playerDropDown.value;
    }

    public void equipArm()
    {
        bool right = GameObject.Find("Toggle").GetComponent<Toggle>().isOn;

        if (currentPlayer < 0 || currentArm < 0) return;

        loot[currentArm].isRight = right;
        players[currentPlayer].GetComponent<Entity>().equipArm(loot[currentArm]);
        loot.RemoveAt(currentArm);
        displayData();
    }

    public void eatArm()
    {
        if (currentPlayer < 0 || currentArm < 0) return;
        Entity current = players[currentPlayer].GetComponent<Entity>();
        current.setHP(current.getMaxHP());
        loot.RemoveAt(currentArm);
        displayData();
    }

    public List<GameObject> getPlayers()
    {
        return players;
    }
}
