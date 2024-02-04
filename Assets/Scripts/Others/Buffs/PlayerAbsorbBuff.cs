using UnityEngine;

public class PlayerAbsorbBuff : PlayerBuffs
{
    //Nên bố trí vị trí buff này sao cho
    //Khoảng thgian ăn buff 1 và buff 2 không gần kề nhau tránh bug (có thể có)

    [SerializeField] private float _tempHPDuration;
    [SerializeField] private float _tempHPRunOutDuration;
    [SerializeField] private Transform _tempShieldIcon; //sign báo hiệu vẫn còn thgian buff
    [SerializeField] private Transform _tempShieldIconPos;

    //Thêm hiệu ứng để biết hết Buff tương tự như Shield ?

    public float TempHPDuration { get { return _tempHPDuration; } }

    public float TempHPRunOutDuration { get { return _tempHPRunOutDuration; } }

    public override void Awake()
    {
        
    }

    public override void Start()
    {
        _tempShieldIcon.gameObject.SetActive(false);
    }

    public override void Update()
    {
        if (_isAllowToUpdate)
        {
            //Debug.Log("Updateee");
            if (Time.time - _entryTime >= _buffDuration)
            {
                _isAllowToUpdate = false;
                _tempShieldIcon.gameObject.SetActive(false);
            }
            else
                _tempShieldIcon.transform.position = _tempShieldIconPos.position;
        }
    }

    public override void ApplyBuff()
    {
        base.ApplyBuff();
        _tempShieldIcon.gameObject.SetActive(true);
        SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.AbsorbBuffSfx, 1.0f);
    }
}
