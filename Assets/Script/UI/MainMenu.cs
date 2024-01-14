using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Transform gameLogo;
    [SerializeField]
    private Transform tutorPanel;
    [SerializeField]
    private Transform guideLine;
    [SerializeField]
    private Transform components;
    [SerializeField]
    private Transform playButton;


    private void Start()
    {
        tutorPanel.gameObject.SetActive(false);
        gameLogo.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 400, 0);
        gameLogo.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -20), .6f, false).SetEase(Ease.OutElastic).SetUpdate(true);
    }

    public void ShowTutorPanel()
    {
        tutorPanel.gameObject.SetActive(true);
        guideLine.gameObject.SetActive(true);
        FadeIn(tutorPanel.GetComponent<CanvasGroup>());
        components.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
    }

    public void HideTutorPanel()
    {
        StartCoroutine(FadeOut(tutorPanel.GetComponent<CanvasGroup>()));
        components.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
    }

    private void FadeIn(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0, .3f).SetUpdate(true);

        yield return new WaitForSecondsRealtime(.3f);
        guideLine.gameObject.SetActive(true);
        tutorPanel.gameObject.SetActive(false);

    }

    private void OnApplicationQuit()
    {
        DOTween.KillAll();
    }
}
