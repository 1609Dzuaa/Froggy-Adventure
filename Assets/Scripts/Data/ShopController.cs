using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static GameConstants;
using static GameEnums;

//gridlayoutgroup
public class ShopController : MonoBehaviour
{
    [SerializeField] float _tweenDuration;//0.3, InOutQuad
    [SerializeField] Ease _ease;

    [Header("Là vị trí Y mà Grid sẽ đc set để cân bằng trong ScrollRect")]
    [SerializeField] float _startPositionY;

    [Header("Prefab Items để Instantiate")]
    [SerializeField] ItemShop[] _arrItemPrefabs;
    ItemShop[] _arrItems;

    [Header("Ref cái túi để gọi hàm Setup Dictionary bên túi, Shouldn't do this way;)")]
    [SerializeField] PlayerBagController _playerBag;

    private void Awake()
    {
        CreateItem();
        ResetScaleItems(_arrItems);
        StartCoroutine(InitFiles());
        _playerBag.SetupDictionary();
    }

    private IEnumerator InitFiles()
    {
        yield return null;

        //đợi 1 frame để check xem object bị destroy ch
        //r mới init files
        if (gameObject)
        {
            InitFileHelper.InitAbilityAndFruitFiles(_arrItems);
            Debug.Log("INIT FILES in ShopController");
        }
    }

    private void CreateItem()
    {
        _arrItems = new ItemShop[_arrItemPrefabs.Length];
        for (int i = 0; i < _arrItemPrefabs.Length; i++)
        {
            ItemShop itemShop = Instantiate(_arrItemPrefabs[i], transform);
            _arrItems[i] = itemShop;
        }
    }

    void Start()
    {
        //HandleCreateSkillFile();
    }

    private void OnEnable()
    {
        //TweenItems();
        StartCoroutine(TweenItems(_arrItems));

        //set lại vị trí của Grid Shop do size của nó thay đổi trong lúc Instantiate Items
        //để việc hiện scroll rect Shop không bị mất nửa phần trên và dưới
        //Debug.Log("posY: " + transform.localPosition.y);
        transform.localPosition = new Vector3(transform.localPosition.x, _startPositionY);

        //Debug.Log("Called");
    }

    private void OnDisable()
    {
        ResetScaleItems(_arrItems);
    }

    /*private void TweenItems()
    {
        Sequence sequence = DOTween.Sequence();
        bool isplay = false;
        for (int i = 0; i < _arrItemPrefabs.Length; i++)
        {
            sequence.Append(_arrItemPrefabs[i].transform.DOScale(Vector3.one, _tweenDuration).SetEase(_ease));
            if(!isplay)
            {
                isplay = true;
                sequence.Play();
            }
        }
        Debug.Log("Current Frame: " + Time.frameCount);
    }*/

    private IEnumerator TweenItems(ItemShop[] arr)
    {
        //gọi như bthg thì lỗi ?! nên xài coroutine gọi ở frame sau
        //Debug.Log("Current Frame: " + Time.frameCount);

        yield return null;

        //Debug.Log("Current Frame: " + Time.frameCount);
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < arr.Length; i++)
            sequence.Append(arr[i].transform.DOScale(Vector3.one, _tweenDuration).SetEase(_ease));
    }

    private void ResetScaleItems(ItemShop[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
            arr[i].transform.localScale = Vector3.zero;
    }

    public void ChangePos()
    {
        Debug.Log("hello");
    }
}
