using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public SceneChanger sceneChanger;
    public GameScene gameScene;

    #region Game status
    private Level currentLevelData;
    private bool isGameWin = false;
    private bool isGameLose = false;
    private float timeLeft;
    #endregion

    private void Start()
    {
        currentLevelData = LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex);
        GameObject map = Instantiate(currentLevelData.map);
        timeLeft = currentLevelData.timeLimit;
        gameScene.UpdatePlayerTimeLeft(timeLeft);
        Time.timeScale = 1;
    }

    public void PlayerWinThisLevel()
    {
        if (isGameWin || isGameLose)
        {
            return;
        }
        LevelManager.instance.levelData.SetLevelData(LevelManager.instance.currentLevelIndex, true, true);
        if (LevelManager.instance.levelData.GetLevels().Count > LevelManager.instance.currentLevelIndex + 1)
        {
            if (LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex + 1).isPlayable == false)
            {
                LevelManager.instance.levelData.SetLevelData(LevelManager.instance.currentLevelIndex + 1, true, false);
            }
        }
        isGameWin = true;

        gameScene.PopupWinPanelGameScene();
        LevelManager.instance.levelData.SaveDataAsJSONFomat();
    }

    private void Update()
    {
        DecreaseTimeLeftAndShowUI();
        CheckTimeLeftToCheckLose();
    }

    public void DecreaseTimeLeftAndShowUI()
    {
        if(isGameWin || isGameLose)
        {
            return;
        }   
        timeLeft -= Time.deltaTime;
        gameScene.UpdatePlayerTimeLeft(timeLeft);
    }

    public void CheckTimeLeftToCheckLose()
    {
        if (timeLeft <= 0)
        {
            if (isGameWin || isGameLose)
            {
                return;
            }
            PlayerLoseThisLevelAndShowUI();
        }
    }

    public void PlayerLoseThisLevelAndShowUI()
    {
        isGameLose = true;
        gameScene.PopupLosePanelGameScene();
    }

    public bool IsThisGameFinalOrWin()
    {
        return isGameWin;
    }

    public bool IsThisGameFinalOrLose()
    {
        return isGameLose;
    }

    private void OnApplicationQuit()
    {
        DOTween.KillAll();
    }
}

