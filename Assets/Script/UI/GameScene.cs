using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField]
    private Transform overlayPanel;
    [SerializeField]
    private Transform winPanel;
    [SerializeField]
    private Transform losePanel;
    [SerializeField]
    private Button replayButton;
    [SerializeField]
    private Button homeButton;

    [SerializeField]
    private Text timeLeft;
    [SerializeField]
    private Text levelName;

    private void Start()
    {
        levelName.text = "LEVEL " + (LevelManager.instance.currentLevelIndex + 1).ToString();
    }

    public void UpdatePlayerTimeLeft(float playerTimeLeft)
    {
        int min = (int)playerTimeLeft / 60;
        int sec = (int)playerTimeLeft % 60;
        this.timeLeft.text = min.ToString("00") + " : " + sec.ToString("00");
    }

    public void PopupWinPanelGameScene()
    {
        overlayPanel.gameObject.SetActive(true);
        winPanel.gameObject.SetActive(true);
        FadePanelInScene(overlayPanel.GetComponent<CanvasGroup>(), winPanel.GetComponent<RectTransform>());
        homeButton.interactable = false;
        replayButton.interactable = false;
    }

    public void PopupLosePanelGameScene()
    {
        overlayPanel.gameObject.SetActive(true);
        losePanel.gameObject.SetActive(true);
        FadePanelInScene(overlayPanel.GetComponent<CanvasGroup>(), losePanel.GetComponent<RectTransform>());
        homeButton.interactable = false;
        replayButton.interactable = false;
    }

    private void FadePanelInScene(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);

        rectTransform.localScale = Vector3.zero;
        rectTransform.DOScale(1, .3f).SetEase(Ease.OutBack).SetUpdate(true);
    }
    private void OnApplicationQuit()
    {
        DOTween.KillAll();
    }
}
