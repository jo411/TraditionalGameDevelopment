using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmList : MonoBehaviour {

    [SerializeField]
    public  List<GameObject> arms;

        // Use this for initialization
       
    public  GameObject getRandomArm()
    {
        return arms[Calculator.rand.Next(0, arms.Count)];
    }

}
