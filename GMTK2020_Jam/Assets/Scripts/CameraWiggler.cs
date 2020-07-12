using System;
using UnityEngine;

public class CameraWiggler : MonoBehaviour
{
    public float sensitivityX = 15f;
    public float sensitivityY = 15f;
    private float edgePercent = 0.05f;

    public float horzDegreeDelta = 5;
    public float vertDegreeDelta = 5;

    private Vector3 defaultRotation;
    
    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis
    
    private void Start()
    {
        defaultRotation = transform.localRotation.eulerAngles;
        //rotY = defaultRotation.y;
        //rotX = defaultRotation.x;
    }

    void Update()
    {
        float vert = ((Input.mousePosition.y >= Screen.height * (1- edgePercent) ? 1 : 0) +
                     ((Input.mousePosition.y < Screen.height * (edgePercent)) ? -1 : 0));
        float horz = ((Input.mousePosition.x >= Screen.width * (1- edgePercent) ? 1 : 0) +
                      ((Input.mousePosition.x < Screen.width * (edgePercent)) ? -1 : 0));
        
        if (Mathf.Abs(vert) > 0 || Mathf.Abs(horz) > 0)
        {
            // do the wiggle
            rotY += horz * sensitivityX * Time.deltaTime;
            rotX += -vert * sensitivityY * Time.deltaTime;
 
            rotX = Mathf.Clamp(rotX, -horzDegreeDelta, horzDegreeDelta);
            rotY = Mathf.Clamp(rotY, -vertDegreeDelta, vertDegreeDelta);
            
            transform.localRotation = Quaternion.Euler(defaultRotation.x + rotX, defaultRotation.y + rotY, defaultRotation.z);
        }
        else
        {
            // return to normal

            float signY = Mathf.Sign(rotY);
            if (!Mathf.Approximately(rotY, 0f))//(Mathf.Abs(rotY) > 0)
            {
                rotY += -signY * sensitivityX * Time.deltaTime;
                
                if (Math.Abs(signY - Mathf.Sign(rotY)) > Mathf.Epsilon)
                {
                    rotY = 0;
                }
            }

            float signX = Mathf.Sign(rotX);
            if (!Mathf.Approximately(rotX, 0f))//Mathf.Abs(rotX) > 0))
            {
                rotX += -signX * sensitivityY * Time.deltaTime;
                
                if (Math.Abs(signX - Mathf.Sign(rotX)) > Mathf.Epsilon)
                {
                    rotX = 0;
                }
            }
 
            rotX = Mathf.Clamp(rotX, -horzDegreeDelta, horzDegreeDelta);
            rotY = Mathf.Clamp(rotY, -vertDegreeDelta, vertDegreeDelta);
            
            transform.localRotation = Quaternion.Euler(defaultRotation.x + rotX, defaultRotation.y + rotY, defaultRotation.z);
            
        }
        
    }
    
}
