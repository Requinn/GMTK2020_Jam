using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Show provided string letter by letter, pausing after periods.
/// </summary>
public class ShowTextByCharacter : MonoBehaviour
{
    [SerializeField, Multiline]
    private string _textToShow = "The use of rock has had a huge impact on the cultural and technological development of the human race. Rock has been used by humans and other hominids for at least 2.5 million years. Lithic technology marks some of the oldest and continuously used technologies. The mining of rock for its metal content has been one of the most important factors of human advancement, and has progressed at different rates in different places, in part because of the kind of metals available from the rock of a region.";
    [SerializeField]
    private float _timeBetweenChar = 0.05f, _timeBetweenSentences = 0.75f;
    [SerializeField]
    private TextMeshProUGUI _textField;

    private bool _isTyping = false;
    public UnityEvent OnTextTypingFinished;

    public Button button;
    public Button nextButton;

    public GameObject[] disableOnStartObjects;

    public AudioSource typing;
    public AudioClip typingSound;

    private void Start()
    {
        _textField.text = "";
    }

    public void StartShowText() {
        if (_isTyping) { return; }
        _textField.gameObject.SetActive(true);
        _textField.text = "";
        StartCoroutine(ShowText());
        button.gameObject.SetActive(false);

        for (int i = 0; i < disableOnStartObjects.Length; i++)
        {
            disableOnStartObjects[i].SetActive(false);
        }
    }

    private IEnumerator ShowText() {
        _isTyping = true;
        WaitForSeconds charWait = new WaitForSeconds(_timeBetweenChar);
        WaitForSeconds sentenceWait = new WaitForSeconds(_timeBetweenSentences);
        int i = 0;
        StringBuilder sb = new StringBuilder();
        while (i < _textToShow.Length) {
        
            sb.Append(_textToShow[i]);
            _textField.text = sb.ToString();
            if (_textToShow[i] == ' ') {
                typing.PlayOneShot(typingSound, 0.7F);
            }
            else if (_textToShow[i] == '.' && (i + 1 < _textToShow.Length && _textToShow[i + 1] == ' ')) {                
                yield return sentenceWait;
            }else {
                yield return charWait;
            }
            i++;
            yield return 0.0f;
        }
        _isTyping = false;
        typing.volume = 0;
        yield return new WaitForSeconds(1.0f);
        OnTextTypingFinished.Invoke();
        nextButton.gameObject.SetActive(true);
        yield return 0.0f;
    }
    
    
}
