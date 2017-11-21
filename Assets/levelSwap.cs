using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelSwap : MonoBehaviour {

public void loadGame()
    {
        SceneManager.LoadScene("Game");
    }
}
