using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outro : MonoBehaviour
{
    [SerializeField]
    private ShowTextByCharacter _textHandler;
    [SerializeField]
    private UnityEngine.UI.Image _fadeScreen;

    public void StartOutro() {
        StartCoroutine(OutroFade());
    }

    private IEnumerator OutroFade() {
        _fadeScreen.gameObject.SetActive(true);
        //fade out to black
        Color toOpaque = new Color(_fadeScreen.color.r, _fadeScreen.color.g, _fadeScreen.color.b, 0.0f);
        while (_fadeScreen.color.a < 1.0f) {
            _fadeScreen.color = toOpaque;
            toOpaque.a = Mathf.MoveTowards(toOpaque.a, 1.0f, Time.deltaTime / 2.0f);
            yield return 0.0f;
        }
        _textHandler.StartShowText();
        yield return 0.0f;
    }
}
