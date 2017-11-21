using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadGameFeedBack : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Text>().text = GameObject.FindObjectOfType<GameFeedback>().getMessage();
        GameObject.FindObjectOfType<GameFeedback>().clear();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
