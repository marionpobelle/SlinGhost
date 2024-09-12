using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroup _p20Image;
    [SerializeField] private CanvasGroup _cnamMagelisImage;
    [SerializeField] private CanvasGroup _wwiseImage;

    [Header("Fade Effect")]
    [SerializeField, Tooltip("Time taken in seconds for the UI element to fade.")] private float _fadeTime = 1.0f;
    [SerializeField, Tooltip("Delay in between the images in seconds.")] private float _delayTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayIntro());    
    }

    private IEnumerator PlayIntro()
    {
        AkSoundEngine.SetState("Music", "Menu");
        AkSoundEngine.PostEvent("Music", gameObject);
        FadeIn(_p20Image);
        yield return new WaitForSeconds(_fadeTime);
        FadeOut(_p20Image);
        yield return new WaitForSeconds(_fadeTime);
        yield return new WaitForSeconds(_delayTime);
        FadeIn(_cnamMagelisImage);
        yield return new WaitForSeconds(_fadeTime);
        FadeOut(_cnamMagelisImage);
        yield return new WaitForSeconds(_fadeTime);
        yield return new WaitForSeconds(_delayTime);
        FadeIn(_wwiseImage);
        yield return new WaitForSeconds(_fadeTime);
        FadeOut(_wwiseImage);
        yield return new WaitForSeconds(_fadeTime);
        yield return new WaitForSeconds(_delayTime);
        SceneManager.LoadScene("Menu");
    }

    public void FadeIn(CanvasGroup canvasGroup)
    {
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, _fadeTime);
    }

    public void FadeOut(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0, _fadeTime).OnComplete(() => canvasGroup.gameObject.SetActive(false));

    }
}
