using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneInteractable : InteractableObject
{
    [Space(5)] 
    private PhoneTextReciever text;

    protected override void OnInteractSuccess()
    {
        base.OnInteractSuccess();
        
    }

    protected override void OnInteractFailed()
    {
        base.OnInteractFailed();
        
    }
}
