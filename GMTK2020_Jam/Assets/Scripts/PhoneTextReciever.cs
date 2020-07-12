﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneTextReciever : MonoBehaviour
{
    [SerializeField]
    private GameObject _phoneObj;
    [SerializeField]
    private StoryBeat[] _beats;
    [SerializeField]
    private float _messageDelay = 0.5f;
    
    [SerializeField]
    private AudioClip _phonePing;
    [SerializeField]
    private AudioSource _phoneAudioSrc;

    private bool _isShowingMessages = false;

    private void Start() {
        HidePhone();
    }

    public void ShowPhone() {
        _phoneObj.SetActive(true);
    }

    public void HidePhone() {
        _phoneObj.SetActive(false);
        for(int i = 0; i < _beats.Length; i++)  {
            _beats[i].rootObject.SetActive(false);
        }
    }

    public void StartPhoneSequence(int storyBeat) {
        if (_isShowingMessages) return;
        StartCoroutine(ShowMessages(_beats[storyBeat]));
    }

    private IEnumerator ShowMessages(StoryBeat beat) {
        _isShowingMessages = true;
        WaitForSeconds delay = new WaitForSeconds(_messageDelay);
        int count = 0;
        beat.rootObject.SetActive(false);
        while (count < beat.messages.Length) {
            yield return delay;
            beat.messages[count].SetActive(true);
            count++;
            yield return 0.0f;
        }
        _isShowingMessages = false;
        yield return 0.0f;
    }
}

[System.Serializable]
public class StoryBeat {
    public GameObject rootObject;
    public GameObject[] messages;
}