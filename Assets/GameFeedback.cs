using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameFeedback : MonoBehaviour {
    StringBuilder sb;
	// Use this for initialization
	void Start () {
        sb = new StringBuilder();
        DontDestroyOnLoad(transform.gameObject);
    }


    public void logMessage(string message)
    {
        sb.Append(message + "\n");
    }

    public string getMessage()
    {
        return sb.ToString();
    }

    public void clear()
    {
        sb = new StringBuilder();
    }
}
