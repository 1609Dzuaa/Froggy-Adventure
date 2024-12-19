using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;
using static GameConstants;

public class Fruit : GameObjectManager
{
    [SerializeField] FruitData _fruitData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PLAYER_TAG))
        {
            EventsManager.Instance.NotifyObservers(EEvents.OnCollectFruit, _fruitData);
            GameObject go = Pool.Instance.GetObjectInPool(EPoolable.CollectFruits);
            go.SetActive(true);
            go.transform.position = collision.ClosestPoint(transform.position);
            MarkAsDeleted();
            Destroy(gameObject);
        }
    }
}
