using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {

    

    protected static BattleManager instance; // Needed
    public GameObject entityPrefab;




    public List<Transform> playerPositions;
    public List<Transform> enemyPositions;

    private List<GameObject> players;
    private List<GameObject> enemies;
    void Start()
    {
        players = new List<GameObject>();
        enemies = new List<GameObject>();
        instance = this;
        placeEntities();
    }

	
	// Update is called once per frame
	void Update () {
		
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

    public static GameObject createEntity(string name,Transform pos)
    {
        GameObject newEntity = Object.Instantiate(instance.entityPrefab,pos);
        newEntity.GetComponent<Entity>().Initialize(name);
        newEntity.transform.position = pos.position;
        return newEntity;
    }

}
