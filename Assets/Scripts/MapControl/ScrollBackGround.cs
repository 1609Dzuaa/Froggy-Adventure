using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBackGround : MonoBehaviour
{
    [SerializeField] RawImage _rawImg;
    [SerializeField] float _x, _y;

    // Update is called once per frame
    void Update()
    {
        _rawImg.uvRect = new Rect(_rawImg.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _rawImg.uvRect.size);
    }
}