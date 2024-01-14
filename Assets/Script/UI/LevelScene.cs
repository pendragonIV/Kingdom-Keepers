using DG.Tweening;
using System.Collections;
using UnityEngine;

public class LevelScene : MonoBehaviour
{
    public static LevelScene instance;
    private const float DELAY_TIME = .3f;
    [SerializeField]
    private Transform levelHolderPrefab;
    [SerializeField]
    private Transform levelsContainer;

    public Transform sceneTransition;

    [SerializeField]
    private GameObject hoderCMS;

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

    void Start()
    {
        PrepareLevels();
    }
    public void PlayChangeScene()
    {
        sceneTransition.GetComponent<Animator>().Play("SceneTransitionReverse");
    }

    private void PrepareLevels()
    {
        for (int i = 0; i < LevelManager.instance.levelData.GetLevels().Count; i++)
        {
            Transform holder = Instantiate(levelHolderPrefab, levelsContainer);
            holder.name = i.ToString();
            Level level = LevelManager.instance.levelData.GetLevelAt(i);
            if (LevelManager.instance.levelData.GetLevelAt(i).isPlayable)
            {
                holder.GetComponent<LevelHolder>().EnableHolder();
            }
            else
            {
                holder.GetComponent<LevelHolder>().DisableHolder();
            }
            Transform holderContainer = holder.GetChild(0);
            holderContainer.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, -60f);
            holderContainer.GetComponent<CanvasGroup>().alpha = 0;
            holderContainer.GetComponent<CanvasGroup>().DOFade(1, .5f).SetDelay(i * DELAY_TIME);
            holderContainer.GetComponent<RectTransform>().DOAnchorPosY(0, 0.5f).SetEase(Ease.InSine).SetDelay(i * DELAY_TIME);
        }
        StartCoroutine(AddCMS(LevelManager.instance.levelData.GetLevels().Count * DELAY_TIME));
    }
    private IEnumerator AddCMS(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GameObject CMS = Instantiate(hoderCMS, levelsContainer);
        CMS.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, -285 - 60f);
        CMS.GetComponent<CanvasGroup>().alpha = 0;
        CMS.GetComponent<CanvasGroup>().DOFade(1, .5f);
        CMS.GetComponent<RectTransform>().DOAnchorPosY(-285, 0.5f).SetEase(Ease.InSine);
    }
    private void OnApplicationQuit()
    {
        DOTween.KillAll();
    }

}
