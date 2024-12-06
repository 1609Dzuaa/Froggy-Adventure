using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static GameConstants;
using static GameEnums;
using System.IO;

//gridlayoutgroup
public class ShopController : MonoBehaviour
{
    [SerializeField] float _tweenDuration;//0.3, InOutQuad
    [SerializeField] Ease _ease;

    [Header("Là vị trí Y mà Grid sẽ đc set để cân bằng trong ScrollRect")]
    [SerializeField] float _startPositionY;

    [Header("Prefab Items để Instantiate")]
    public ItemShop[] ArrItemPrefabs;
    ItemShop[] _arrItems;

    public void CreateItemAndInitFiles()
    {
        _arrItems = new ItemShop[ArrItemPrefabs.Length];
        for (int i = 0; i < ArrItemPrefabs.Length; i++)
        {
            ItemShop itemShop = Instantiate(ArrItemPrefabs[i], transform);
            _arrItems[i] = itemShop;
        }
        InitAbilityAndFruitFiles(_arrItems);
        ResetScaleItems();
    }

    private void InitAbilityAndFruitFiles(ItemShop[] arrParam)
    {
        string skillsFilePath = Application.persistentDataPath + SKILLS_DATA_PATH;
        if (!File.Exists(skillsFilePath))
        {
            List<Skills> listSkills = new();
            List<Fruits> listFruits = new();
            HashSet<ESkills> hashSkill = new();
            HashSet<EFruits> hashFruits = new();

            //Fill in List Skill và Fruit
            foreach (var item in arrParam)
            {
                if (item is AbilityItemShop)
                {
                    var ability = (AbilityItemShop)item;

                    //add skill vào list skills
                    if (!hashSkill.Contains(ability.SISData.Ability.AbilityName))
                    {
                        var name = ability.SISData.Ability.AbilityName;
                        var isLimited = ability.SISData.Ability.IsLimited;
                        hashSkill.Add(name);
                        Skills sk = new(name, DEFAULT_UNLOCK_ITEM, isLimited);
                        listSkills.Add(sk);
                    }

                    //add fruit vào list fruits
                    if (!hashFruits.Contains(ability.SISData.Ability.FruitName))
                    {
                        var name = ability.SISData.Ability.FruitName;
                        hashFruits.Add(name);
                        Fruits fr = new(name, DEFAULT_ITEM_COUNT);
                        listFruits.Add(fr);
                    }
                }
            }

            SkillsController sC = new(listSkills);
            JSONDataHelper.SaveToJSon<SkillsController>(sC, skillsFilePath);
            Debug.Log("persistentDataPath: " + skillsFilePath);

            string fruitsFilePath = Application.persistentDataPath + FRUITS_DATA_PATH;
            FruitsIventory fI = new(listFruits);
            JSONDataHelper.SaveToJSon<FruitsIventory>(fI, fruitsFilePath);
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
        ResetScaleItems();
    }

    /*private void TweenItems()
    {
        Sequence sequence = DOTween.Sequence();
        bool isplay = false;
        for (int i = 0; i < ArrItemPrefabs.Length; i++)
        {
            sequence.Append(ArrItemPrefabs[i].transform.DOScale(Vector3.one, _tweenDuration).SetEase(_ease));
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

        if (arr != null)
        {
            //Debug.Log("Current Frame: " + Time.frameCount);
            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < arr.Length; i++)
                sequence.Append(arr[i].transform.DOScale(Vector3.one, _tweenDuration).SetEase(_ease));
        }
    }

    public void ResetScaleItems()
    {
        if (_arrItems == null) return;

        for (int i = 0; i < _arrItems.Length; i++)
            _arrItems[i].transform.localScale = Vector3.zero;
    }
}
