using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneInteractable : InteractableObject
{
    [Space(5)] 
    private PhoneTextReciever text;

    [SerializeField]
    private int _storyBeat = 0;
    [SerializeField]
    private PhoneTextReciever _phoneHandler;

    protected override void OnInteractSuccess()
    {
        base.OnInteractSuccess();
        _phoneHandler.StartPhoneSequence(_storyBeat);
    }

    protected override void OnInteractFailed()
    {
        base.OnInteractFailed();
        
    }
}
