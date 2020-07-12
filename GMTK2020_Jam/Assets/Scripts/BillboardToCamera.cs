using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Turn an object to face the camera
/// </summary>
public class BillboardToCamera : MonoBehaviour
{
    private Camera _mainCamera;
    public void OnEnable() {
        _mainCamera = Camera.main;
    }
    public void Update() {
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }
        if (gameObject.activeInHierarchy && _mainCamera != null)
        {
            transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.forward,
                _mainCamera.transform.rotation * Vector3.up);
            
            
            //transform.forward = Camera.main.transform.forward;
        }
    }
}
