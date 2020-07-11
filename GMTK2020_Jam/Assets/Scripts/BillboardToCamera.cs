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
        if (gameObject.activeInHierarchy) transform.forward = _mainCamera.transform.forward;
    }
}
