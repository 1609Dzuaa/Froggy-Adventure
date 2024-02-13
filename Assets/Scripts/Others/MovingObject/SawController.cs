using UnityEngine;

public class SawController : MovingObjectController
{
    [SerializeField] float _rotateSpeed;
    [SerializeField] bool _isBossGate;

    private void OnEnable()
    {
        if (_isBossGate)
            EventsManager.Instance.SubcribeToAnEvent(GameEnums.EEvents.BossGateOnClose, ActiveGate);
    }

    private void OnDestroy()
    {
        if (_isBossGate)
            EventsManager.Instance.UnSubcribeToAnEvent(GameEnums.EEvents.BossGateOnClose, ActiveGate);
    }

    protected override void Start()
    {
        if(_isBossGate)
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        transform.Rotate(0f, 0f, 360f * _rotateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Hàm dưới để active gate khi Player enter Boss Room
    /// </summary>
    
    private void ActiveGate(object obj)
    {
        GameObject activeSfx = Pool.Instance.GetObjectInPool(GameEnums.EPoolable.BrownExplosion);
        activeSfx.SetActive(true);
        activeSfx.transform.position = transform.position;
        gameObject.SetActive(true);
        Debug.Log("activated");
    }
}
