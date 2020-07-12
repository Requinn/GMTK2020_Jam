using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class can optionally fade out the camera and duck audio sources.
/// </summary>
public class SceneLoad : MonoBehaviour
{
    [SerializeField]
    private Image _fadeScreen;
    
    [Space(5)]
    [Header("Fade Out")]
    [SerializeField]
    private float _fadeOutTime = 1.0f;
    [SerializeField] 
    private AudioSource[] _fadeOutAudioSources;
    
    [Space(5)]
    [Header("Fade In")]
    [SerializeField] 
    private bool _fadeInOnLoad = false;
    [SerializeField]
    private float _fadeInTime = 1.0f;
    [SerializeField] 
    private AudioSource[] _fadeInAudioSources;
    
    public bool IsFading { get; private set; }

//    [SerializeField] 
//    private AudioSourceStartParams[] _audioSourceStartParams;
//
//    [System.Serializable]
//    public class AudioSourceStartParams
//    {
//        public AudioSource source;
//        public bool fadeIn;
//        public float startDelay;
//    }

    private void Awake()
    {
        if (_fadeScreen)
        {
            _fadeScreen.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        if (_fadeInOnLoad)
        {
            StartCoroutine(CoFadeInScene(_fadeInTime));
        }
    }

    public void LoadSceneByID(int sceneId) {
        SceneManager.LoadScene(sceneId);
    }

    public void LoadSceneByName(string name) {
        SceneManager.LoadScene(name);
    }

    public void FadeLoadSceneByID(int sceneId)
    {
        StartCoroutine(CoFadeLoadScene(sceneId, _fadeOutTime));
    }
    
    private IEnumerator CoFadeLoadScene(int sceneId, float duration)
    {
        Color fadeColor = (_fadeScreen)? _fadeScreen.color : Color.black;
        fadeColor.a = 0f;
        if (_fadeScreen)
        {
            _fadeScreen.transform.SetAsLastSibling();
            _fadeScreen.gameObject.SetActive(true);
            _fadeScreen.color = fadeColor;
        }
        
        // Get the volume that each source we're fading out is starting at.
        List<float> fadeAudioSourceStartVolumes = new List<float>();
        for (int i = 0; i < _fadeOutAudioSources.Length; i++)
        {
            fadeAudioSourceStartVolumes.Add(_fadeOutAudioSources[i].volume);
        }
        
        // Run tweens.
        float timer = 0f;
        IsFading = true;
        while(timer < _fadeOutTime)
        {
            timer += Time.deltaTime;
            float lerp = timer / duration;
            
            // Fade screen to black.
            if (_fadeScreen)
            {
                fadeColor.a = Mathf.Lerp(0f, 1f, lerp);
                _fadeScreen.color = fadeColor;
            }

            for (int i = 0; i < _fadeOutAudioSources.Length; i++)
            {
                _fadeOutAudioSources[i].volume = Mathf.Lerp(fadeAudioSourceStartVolumes[i], 0f, lerp);
            }
            
            yield return null;
        }

        IsFading = false;
        LoadSceneByID(sceneId);
    }
    
    private IEnumerator CoFadeInScene(float duration)
    {
        Color fadeColor = (_fadeScreen)? _fadeScreen.color : Color.black;
        fadeColor.a = 1f;
        if (_fadeScreen)
        {
            _fadeScreen.transform.SetAsLastSibling();
            _fadeScreen.gameObject.SetActive(true);
            _fadeScreen.color = fadeColor;
        }
        
        // Get the volume that each source we're fading out is starting at.
        List<float> fadeAudioSourceStartVolumes = new List<float>();
        for (int i = 0; i < _fadeInAudioSources.Length; i++)
        {
            fadeAudioSourceStartVolumes.Add(_fadeInAudioSources[i].volume);
            _fadeInAudioSources[i].volume = 0f;
        }
        
        // Run tweens.
        float timer = 0f;
        IsFading = true;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            float lerp = timer / duration;
            
            // Fade screen to black.
            if (_fadeScreen)
            {
                fadeColor.a = Mathf.Lerp(1f, 0f, lerp);
                _fadeScreen.color = fadeColor;
            }

            for (int i = 0; i < _fadeInAudioSources.Length; i++)
            {
                _fadeInAudioSources[i].volume = Mathf.Lerp(0f, fadeAudioSourceStartVolumes[i], lerp);
            }
            
            yield return null;
        }
        
        IsFading = false;
    }
    
    
    
}
