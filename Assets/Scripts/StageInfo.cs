using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo : MonoBehaviour {

    public List<Transform> playerPositions;
    public List<Transform> enemyPositions;
    public Transform cameraPos;

    public void Start()
    {
       GameObject cameraObj= GameObject.FindGameObjectWithTag("Camera Marker");

        cameraPos = cameraObj == null ? null : cameraObj.transform;

        if(cameraPos==null)
        {
            Debug.LogError("This stage doesn't have a camera marker set up: " + this.name);
        }

        for (int i = 1; i <= 4; i++)//search for Players
        {
            GameObject playerSearch = GameObject.Find("P" + i);
            if(playerSearch!=null)
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
}
