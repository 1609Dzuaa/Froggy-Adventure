using UnityEngine;

public class SawController : MovingObjectController
{
    [SerializeField] float _rotateSpeed;
    [SerializeField] bool _isBossGate;

    // Bug null ref do đky event ở OnEnable r setactive(false) ở start
    /// => đky event tận 2 lần

    protected override void Start()
    {
        if (_isBossGate)
        {
            gameObject.SetActive(false);
            EventsManager.Instance.SubcribeToAnEvent(GameEnums.EEvents.BossGateOnClose, ActiveGate);
        }
    }
    private void OnDestroy()
    {
        if (_isBossGate)
        {
            EventsManager.Instance.UnSubcribeToAnEvent(GameEnums.EEvents.BossGateOnClose, ActiveGate);
            //Debug.Log("Unsub");
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!_isBossGate)
            base.Update();
        transform.Rotate(0f, 0f, 360f * _rotateSpeed * Time.deltaTime);
    }

    /*/// <summary>
    /// Hàm dưới để active gate khi Player enter Boss Room
    /// </summary>*/

    private void ActiveGate(object obj)
    {
        Debug.Log("activated");
        GameObject activeVfx = Pool.Instance.GetObjectInPool(GameEnums.EPoolable.RedExplode);
        activeVfx.SetActive(true);
        activeVfx.transform.position = transform.position;
        gameObject.SetActive(true);
    }
}
