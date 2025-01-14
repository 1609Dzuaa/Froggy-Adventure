using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossParticleController : MonoBehaviour
{
    //Điều khiển hạt xoay vòng Boss - xử lý như Trap vs Player
    [SerializeField] Transform _bossRef;
    [SerializeField] float _rotateSpeed;
    [SerializeField] float _radius;
    [SerializeField] float _angleIndex;

    /// <summary>
    /// Một frame quét đc góc bnhieu thì tính cos góc đó * bán kính
    /// sẽ tìm ra đc đoạn x từ tâm tới vị trí x mới trong frame đó. 
    /// Vị trí x mới sẽ đc tính = vị trí x của tâm + đoạn x vừa tính đc
    /// Tương tự với y nhưng sẽ tính sin góc đó
    /// Phép chiếu và Pytago cơ bản ^.^
    /// </summary>
    void Update()
    {
        _angleIndex += Time.deltaTime * _rotateSpeed;
        float xOffset = Mathf.Cos(_angleIndex) * _radius;
        float yOffset = Mathf.Sin(_angleIndex) * _radius;
        transform.position = new Vector3(_bossRef.position.x + xOffset, _bossRef.position.y + yOffset, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG))
        {
            //Debug.Log("Particle DMG");
            EventsManager.NotifyObservers(GameEnums.EEvents.PlayerOnTakeDamage, _bossRef.GetComponent<BossStateManager>().GetIsFacingRight());
        }
    }
}
