using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using UnityEngine.U2D;

public class LoadSpriteHelper : MonoBehaviour
{
    [SerializeField] SpriteAtlas _spriteAtlas;
    [SerializeField] string _spriteName;

    private void Awake()
    {
        GetComponent<Image>().sprite = _spriteAtlas.GetSprite(_spriteName);
    }
}
