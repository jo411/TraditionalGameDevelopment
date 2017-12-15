using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddArms : MonoBehaviour {

    //public GameObject armPrefab;   

    private GameObject leftArm;
    private GameObject rightArm;
    private ArmList armList;

    private Entity entity;

	// Use this for initialization
	void Start () {        
        leftArm = transform.Find("Left").gameObject;
        rightArm = transform.Find("Right").gameObject;
        armList = GameObject.Find("ArmManager").GetComponent<ArmList>();
        Debug.Log(GameObject.Find("ArmManager").GetComponent<ArmList>());

        loadArms();
    }
	
    public void loadArms()
    {
        entity = GetComponent<Entity>();
        if(armList==null)
        {
            Debug.Log("BAd");
        }
        attachArm(leftArm, armList.getTaggedArm(entity.leftArm.attack.tag));
        attachArm(rightArm, armList.getTaggedArm(entity.rightArm.attack.tag));
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
