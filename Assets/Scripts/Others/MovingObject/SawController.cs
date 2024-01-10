using UnityEngine;

public class SawController : MovingObjectController
{
    [SerializeField] float _rotateSpeed;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        transform.Rotate(0f, 0f, 360f * _rotateSpeed * Time.deltaTime);
    }
}
