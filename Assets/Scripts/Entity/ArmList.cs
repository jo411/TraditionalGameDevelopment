using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmList : MonoBehaviour {

    [SerializeField]
    public  List<GameObject> arms;

        // Use this for initialization
      
        
        
        void Start()
    {
        DontDestroyOnLoad(this);
    } 
    public  GameObject getRandomArm()
    {
        return arms[Calculator.rand.Next(0, arms.Count)];
    }

    public GameObject getTaggedArm(string tag)
    {
        foreach(GameObject current in arms)
        {
            TagList tags = current.GetComponent<TagList>();
            if (tags!=null && tags.hasTag(tag))
            {
                return current;
            }
        }
        return getRandomArm();
    }

}
