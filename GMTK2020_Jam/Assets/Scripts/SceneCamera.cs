using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SceneCamera : MonoBehaviour
{
    [SerializeField]
    private SceneData[] _scenes;
    private int _currentScene;

    public void Awake() {
        //sort in case I guess, to ensure index matches scene ID
        Array.Sort(_scenes);
        //turn off all cameras, except first
        _currentScene = 0;
        for(int i = 1; i < _scenes.Length; i++){
            _scenes[i].camera.gameObject.SetActive(false);
        }
    }

    public void NextScene() {
        ChangeCameraTo(_currentScene + 1);
    }

    public void PreviousScene() {
        ChangeCameraTo(_currentScene - 1);
    }

    public void ChangeCameraTo(int sceneID) {
        if (sceneID < 0 || sceneID > _scenes.Length) return;

        _scenes[_currentScene].camera.gameObject.SetActive(false);
        _scenes[sceneID].camera.gameObject.SetActive(true);
        _currentScene = sceneID;
    }
}

[Serializable]
public class SceneData {
    public Camera camera;
    [Min(0)]
    public int sceneID;

    SceneData(Camera c, int id) {
        camera = c;
        sceneID = id;
    }

    public override bool Equals(object obj) {
        return base.Equals(((SceneData)obj).sceneID);
    }
}
