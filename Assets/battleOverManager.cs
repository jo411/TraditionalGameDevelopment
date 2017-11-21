using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class battleOverManager : MonoBehaviour {
    List<GameObject> players;

    public GameObject UI;
	// Use this for initialization
	void Start () {
        players = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void loadPlayers(List<GameObject> playerList)
    {
        players = playerList;
        displayData();
    }

    void displayData()
    {
        int count = 0;
        foreach (GameObject display in GameObject.FindGameObjectsWithTag("PlayerDisplay"))
        {
            
            display.GetComponentInChildren<Text>().text = players[count].GetComponent<Entity>().eName;
            count++;
        }
        count = 0;
        foreach (GameObject pane in GameObject.FindGameObjectsWithTag("PlayerText"))
        {
            Text display = pane.GetComponentInChildren<Text>();
             display.text = players[count].GetComponent<Entity>().ToString()+"\nArms:\n" + "Right:\n"+players[count].GetComponent<Entity>().arms[0]+"\n" + "Left:\n" + players[count].GetComponent<Entity>().arms[1] + "\n";
            count++;
        }

    }

    public List<GameObject> getPlayers()
    {
        return players;
    }
}
