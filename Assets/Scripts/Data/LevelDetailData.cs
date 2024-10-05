using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameEnums;
using static GameConstants;

public struct DataDetail
{
    public LevelStaticData StaticData;
    public LevelProgressData ProgressData;

    public DataDetail(LevelStaticData staticData, LevelProgressData progressData)
    {
        StaticData = staticData;
        ProgressData = progressData;
    }
}

//Truyền các thông tin quan trọng khi vào gameplay:
//Thời gian của level
//List các skill đã được active (mua) trong shop (chỉ các skill Limited)
public struct LevelInfo
{
    public List<Skills> ListActiveSkills;
    public int LevelTimeAllow;

    public LevelInfo(List<Skills> listActiveSkills, int timeAllow)
    {
        ListActiveSkills = listActiveSkills;
        LevelTimeAllow = timeAllow;
    }
}

public class LevelDetailData : MonoBehaviour
{
    [SerializeField] Image _imageLevel;
    [SerializeField] TextMeshProUGUI _levelDescribe;
    [SerializeField] TextMeshProUGUI _timeDisplay;
    LevelStaticData _LvlSData;
    LevelProgressData _LvlProgressData;
    int _indexLevel;
    int _levelTimeAllow;

    // Start is called before the first frame update
    void Awake()
    {
        EventsManager.Instance.SubcribeToAnEvent(EEvents.LevelOnSelected, ShowDetail);
        //Debug.Log("Subbed");
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.LevelOnSelected, ShowDetail);
    }

    private void ShowDetail(object obj)
    {
        gameObject.SetActive(true);
        DataDetail Detail = (DataDetail)obj;
        _imageLevel.sprite = Detail.StaticData.ImageDescribe;
        TimeDisplayHelper.DisplayTime(ref _timeDisplay, Detail.ProgressData.TimeCompleted, Detail.StaticData.TimeAllow);
        _levelDescribe.text = Detail.StaticData.Describe;
        _indexLevel = Detail.StaticData.OrderID;
        _levelTimeAllow = Detail.StaticData.TimeAllow;
    }

    public void ButtonPlayOnClick()
    {
        if (PlayerHealthManager.Instance.CurrentHP <= 1)
        {
            string content = (PlayerHealthManager.Instance.CurrentHP == 0)
                ? "You Don't Have Any HealthPoint Left, Go Buy It In The Shop !"
                : "You Only Have One HealthPoint Left, Buy It In The Shop Now ?";
            NotificationParam param = new(content, true, () =>
            {
                UIManager.Instance.TogglePopup(EPopup.Notification, false);
                UIManager.Instance.TogglePopup(EPopup.Shop, true);
            });
            ShowNotificationHelper.ShowNotification(param);
            EventsManager.Instance.NotifyObservers(EEvents.OnPopupLevelCanToggle, false);
        }
        else
            StartLevel();
    }

    private void StartLevel()
    {
        UIManager.Instance.AnimateAndTransitionScene(_indexLevel);
        //EventsManager.Instance.NotifyObservers(EEvents.OnPopupLevelCanToggle, true);
        List<Skills> listActiveSkills = ToggleAbilityItemHelper.GetListActivatedSkills();
        LevelInfo levelInfo = new(listActiveSkills, _levelTimeAllow);
        EventsManager.Instance.NotifyObservers(EEvents.OnSetupLevel, levelInfo);
    }
}
