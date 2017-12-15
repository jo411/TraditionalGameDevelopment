using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddArms : MonoBehaviour {

    //public GameObject armPrefab;   

    private GameObject leftArm;
    private GameObject rightArm;
    private ArmList armList;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
        leftArm = transform.Find("Left").gameObject;
        rightArm = transform.Find("Right").gameObject;
        armList = GameObject.Find("ArmManager").GetComponent<ArmList>();

        attachArm(leftArm, armList.getRandomArm());
        attachArm(rightArm, armList.getRandomArm());
    }
	
    public void attachArm(GameObject arm, GameObject armPrefab)
    {
        GameObject newArm= Instantiate(armPrefab);     
        newArm.transform.position = arm.transform.position;

        newArm.transform.rotation = arm.transform.rotation;

       newArm.transform.SetParent(arm.transform);
    }
    void Awake()
    {
       
    }

	// Update is called once per frame
	void Update () {
		
	}
}
