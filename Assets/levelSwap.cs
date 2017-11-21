using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelSwap : MonoBehaviour {

    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

public void loadGame(string name)
    {
        SceneManager.LoadScene(name);
    }
}
