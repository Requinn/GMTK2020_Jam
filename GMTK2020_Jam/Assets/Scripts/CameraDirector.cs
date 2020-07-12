using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class CameraDirector : MonoBehaviour
{
    [SerializeField]
    private SceneData[] _scenes;
    [SerializeField]
    private float _screenFadeTime = 1.0f;
    [SerializeField]
    private Image _fadeScreen; 
    [SerializeField]
    private int _currentScene;
    private bool _isFading = false;
    
    //[Space(5)]
    //

    public void Awake() {
        //sort in case I guess, to ensure index matches scene ID
        Array.Sort(_scenes);
        //turn off all cameras, except first
        //_currentScene = 0;
        _fadeScreen.color = new Color(_fadeScreen.color.r, _fadeScreen.color.g, _fadeScreen.color.b, 0.0f); //alpha 0 for everyone
        _fadeScreen.gameObject.SetActive(false);

        for (int i = 0; i < _scenes.Length; i++){
            _scenes[i].sceneRoot.SetActive(false);
            _scenes[i].camera.gameObject.SetActive(false);
            _scenes[i].camera.GetComponent<AudioListener>().enabled = false;
        }
        
        _scenes[_currentScene].sceneRoot.SetActive(true);
        _scenes[_currentScene].camera.gameObject.SetActive(true);
        _scenes[_currentScene].camera.GetComponent<AudioListener>().enabled = true;
        
        //ChangeCameraTo(_currentScene);
    }

    public void NextScene() {
        if (_currentScene < _scenes.Length - 1) {
            ChangeCameraTo(_currentScene + 1);
        }
    }

    public void PreviousScene() {
        if (_currentScene > 0) {
            ChangeCameraTo(_currentScene - 1);
        }
    }

    /// <summary>
    /// Fade the screne fade and swap camera while the screen is opaque
    /// </summary>
    /// <param name="toScene"></param>
    /// <returns></returns>
    private IEnumerator FadeCameras(int toScene) {
        _isFading = true;
        _fadeScreen.gameObject.SetActive(true);
        //fade out to black
        Color toOpaque = new Color(_fadeScreen.color.r, _fadeScreen.color.g, _fadeScreen.color.b, 0.0f);
        while(_fadeScreen.color.a < 1.0f) {
            _fadeScreen.color = toOpaque;
            toOpaque.a = Mathf.MoveTowards(toOpaque.a, 1.0f, Time.deltaTime / _screenFadeTime);
            yield return 0.0f;
        }
        //turn current camera off and go to new camera 
        _scenes[_currentScene].sceneRoot.SetActive(false);
        _scenes[_currentScene].camera.gameObject.SetActive(false);
        _scenes[_currentScene].camera.GetComponent<AudioListener>().enabled = false;
        
        _scenes[toScene].sceneRoot.SetActive(true);
        _scenes[toScene].camera.gameObject.SetActive(true);
        _scenes[toScene].camera.GetComponent<AudioListener>().enabled = true;

        //fade new scene back in
        Color toTransparent = new Color(_fadeScreen.color.r, _fadeScreen.color.g, _fadeScreen.color.b, 1.0f);
        while (_fadeScreen.color.a > 0.0f) {
            _fadeScreen.color = toTransparent;
            toTransparent.a = Mathf.MoveTowards(toTransparent.a, 0.0f, Time.deltaTime / _screenFadeTime);
            yield return 0.0f;
        }
        _fadeScreen.gameObject.SetActive(false);
        //update currentID
        _currentScene = toScene;
        _isFading = false;
        yield return 0.0f;
    }

    public void ChangeCameraTo(int sceneID) {
        if (_isFading) return;
        StartCoroutine(FadeCameras(sceneID));
    }
}

[Serializable]
public class SceneData : IComparable<SceneData> {
    public Camera camera;
    public GameObject sceneRoot;
    public AudioClip musicTrack;
    public AudioClip ambienceTrack;
    //public AudioMixerGroup sceneMixer;
    [Min(0)]
    public int sceneID;
    SceneData(Camera c, int id) {
        camera = c;
        sceneID = id;
    }

    public int CompareTo(SceneData obj) {
        if (sceneID > obj.sceneID) return 1;
        else if (sceneID < obj.sceneID) return -1;
        else return 0;
    }

    public override bool Equals(object obj) {
        return base.Equals(((SceneData)obj).sceneID);
    }
}
