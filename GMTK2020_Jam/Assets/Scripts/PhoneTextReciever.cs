using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

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
    private AudioSource audioSource;
    [SerializeField]
    private GameObject _closeButton;

    [SerializeField]
    private GameObject _respondToText;
    [SerializeField]
    private Text _responseButtonText;

    [SerializeField]
    private ScrollRect _scroll;

    public UnityEvent onFinalDialogue;
    private int[] _dialogueSteps = { 3, 2, 2, 1 }; //player > npc > player > npc
    private bool[] beatsComplete = { false, false, false, false, false };
    private bool _isShowingMessages = false;
    private int _finalDialogueStep = 0, _finalDialogueInter = 0, _totalFinalStepCounter = 0;
    public AudioMixerSnapshot[] snapshots;

    private void Start() {
        HidePhone();
    }

    public void ShowPhone() {
        _phoneObj.SetActive(true);
        snapshots[0].TransitionTo(.01f); //transition to lowpass
    }

    public void HidePhone() {
        _phoneObj.SetActive(false);
        _closeButton.SetActive(false);
        _respondToText.SetActive(false);
        for (int i = 0; i < _beats.Length; i++)  {
            _beats[i].rootObject.SetActive(false);
        }
        snapshots[1].TransitionTo(.01f); //transition to standard pass
    }

    public void StartPhoneSequence(int storyBeat) {
        if (_isShowingMessages) return;
        ShowPhone();
        if (!beatsComplete[storyBeat]) {
            beatsComplete[storyBeat] = true;
            //final story beat, has player input to drive it forward
            if (storyBeat == 4) {
                _scroll.content = _beats[4].rootObject.GetComponent<RectTransform>();
                _isShowingMessages = true;
                _beats[4].rootObject.SetActive(true);
                _responseButtonText.text = "Send the text";
                _respondToText.SetActive(true);
                //StartCoroutine(StartConvoSequence());
            }
            else {
                _scroll.content = _beats[storyBeat].rootObject.GetComponent<RectTransform>();
                StartCoroutine(ShowMessages(_beats[storyBeat]));
            }
        }else {
            _beats[storyBeat].rootObject.SetActive(true);
            _closeButton.SetActive(true);
        }
    }

    private IEnumerator ShowMessages(StoryBeat beat) {
        _isShowingMessages = true;
        WaitForSeconds delay = new WaitForSeconds(_messageDelay);
        int count = 0;
        beat.rootObject.SetActive(true);
        while (count < beat.messages.Length) {
            if (count == 0) {
                yield return new WaitForSeconds(0.5f);
            }
            else {
                yield return delay;
            }
            beat.messages[count].SetActive(true);
            audioSource.PlayOneShot(_phonePing);
            count++;
            yield return 0.0f;
        }
        _isShowingMessages = false;
        _closeButton.SetActive(true);
        yield return 0.0f;
    }

    /// <summary>
    /// Respond to the player by setting the current step dialogue on
    /// </summary>
    private IEnumerator StartConvoSequence() {
        while (_finalDialogueInter < _dialogueSteps[_finalDialogueStep]) {
            yield return new WaitForSeconds(_messageDelay);
            _beats[4].messages[_totalFinalStepCounter].SetActive(true);
            audioSource.PlayOneShot(_phonePing);
            _totalFinalStepCounter++;
            _finalDialogueInter++;
        }
        _finalDialogueInter = 0;
        //Final dialogue line recieved, wait and go to outro
        if (_totalFinalStepCounter == _beats[4].messages.Length) {
            yield return new WaitForSeconds(2f);
            StartCoroutine(OutroFade());
        }
        else {
            _finalDialogueStep++;
            yield return new WaitForSeconds(0.75f);
            _responseButtonText.text = "Send a reply";
            _respondToText.SetActive(true);
        }
        yield return 0.0f;
    }

    /// <summary>
    /// Called by a button to respond to texts in the final story beat
    /// </summary>
    public void SendReply() {
        //keep going through our lines
        if(_finalDialogueInter < _dialogueSteps[_finalDialogueStep]) {
            _beats[4].messages[_totalFinalStepCounter].SetActive(true);
            _totalFinalStepCounter++;
            _finalDialogueInter++;
        }
        //until we hit the last one and then trigger the respons coroutine
        if(_finalDialogueInter == _dialogueSteps[_finalDialogueStep]) {
            _finalDialogueInter = 0;
            _finalDialogueStep++;
            _respondToText.SetActive(false);
            StartCoroutine(StartConvoSequence());
        }
    }

    private IEnumerator OutroFade() {
        onFinalDialogue.Invoke();
        yield return 0.0f;
    }
}

[System.Serializable]
public class StoryBeat {
    public GameObject rootObject;
    public GameObject[] messages;
}
