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

    private void Awake()
    {
        CreateItem();
        ResetScaleItems(_arrItems);
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
        HandleCreateSkillFile();
    }

    private void HandleCreateSkillFile()
    {
        PlayerPrefs.DeleteAll();
        HashSet<EFruits> hashFruits = new(); 
        if (!PlayerPrefs.HasKey(LIST_SKILL_CREATED))
        {
            List<Skills> listSkills = new();
            List<Fruits> listFruits = new();
            for (int i = 0; i < _arrItems.Length; i++)
            {
                if (_arrItems[i] is AbilityItemShop)
                {
                    var abilityItem = (AbilityItemShop)_arrItems[i];
                    Skills skill = new Skills(abilityItem.SISData.Ability.AbilityName, false);
                    listSkills.Add(skill);

                    if (!hashFruits.Contains(abilityItem.SISData.Ability.FruitName))
                    {
                        hashFruits.Add(abilityItem.SISData.Ability.FruitName);
                        Fruits fr = new Fruits(abilityItem.SISData.Ability.FruitName, 0);
                        listFruits.Add(fr);
                    }
                }
            }

            string skillsFilePath = Application.dataPath + SKILLS_DATA_PATH;
            SkillsController sC = new(listSkills);
            JSONDataHelper.SaveToJSon<SkillsController>(sC, skillsFilePath);

            string fruitsFilePath = Application.dataPath + FRUITS_DATA_PATH;
            FruitsIventory fI = new(listFruits);
            JSONDataHelper.SaveToJSon<FruitsIventory>(fI, fruitsFilePath);

            PlayerPrefs.SetInt(LIST_SKILL_CREATED, CREATED);
        }
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
