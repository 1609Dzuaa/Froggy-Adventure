using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    //Class này dùng để điều khiển việc vẽ HP lên màn hình

    [Header("HP Icon")]
    [SerializeField] private Image[] _hpIcons = new Image[10];
    [SerializeField] private Sprite _hpIcon;
    [SerializeField] private Sprite _lostHPIcon;

    [Header("Manager Reference")]
    [SerializeField] PlayerStateManager _playerStateManager; //Hard - reference (Shouldn't do this)

    // Update is called once per frame
    private void Update()
    {
        //Với cách duyệt này thì mỗi khi thay đổi MaxHP thì
        //phải thêm Image HP Icon trên Canvas
        //=>Thêm sẵn 7 mạng r, chỉ cần enable nó
        for (int i = 0; i < _playerStateManager.GetMaxHP(); i++)
        {
            if (i < _playerStateManager.GetHP())
                _hpIcons[i].sprite = _hpIcon;
            else
                _hpIcons[i].sprite = _lostHPIcon;
        }
    }
}
