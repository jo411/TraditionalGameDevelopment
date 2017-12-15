using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagList : MonoBehaviour {

    public List<string> tags;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public bool hasTag(string query)
    {
        return tags.Contains(query);
    }
}
