using UnityEngine;
using System.Collections;

public class CameraFacingBillboard : MonoBehaviour
{
    Camera m_Camera;

    public void Start()
    {
     
        m_Camera = Camera.main;
     
        
    }
    void Update()
    {
            transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,m_Camera.transform.rotation * Vector3.up);
    }
}