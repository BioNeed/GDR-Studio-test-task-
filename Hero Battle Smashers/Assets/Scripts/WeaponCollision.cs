using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
    [SerializeField] private List<Collider> _colliderList = new List<Collider>();

    public int PlayerHit()
    {
        int hits = 0;
        for (int i = 0; i < _colliderList.Count; i++)
        {
            if (_colliderList[i] != null) {
                if (_colliderList[i].gameObject.CompareTag("Enemy"))
                {
                    hits++;
                    _colliderList[i].GetComponent<AIMovement>().Die();
                }
                else if (_colliderList[i].gameObject.CompareTag("LootBox"))
                {
                    Destroy(_colliderList[i].gameObject);
                }
            }
        }
        return hits;
    }

    public int EnemyHit()
    {
        int hitCount = 0;
        for (int i = 0; i < _colliderList.Count; i++)
        {
            if (_colliderList[i] != null)
            {
                if (_colliderList[i].gameObject.CompareTag("Enemy"))
                {
                    hitCount++;
                    _colliderList[i].GetComponent<AIMovement>().Die();
                }
                else if (_colliderList[i].gameObject.CompareTag("Player"))
                {
                    _colliderList[i].gameObject.GetComponent<PlayerMovement>().Lose();
                }
            }
        }
        return hitCount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_colliderList.Contains(other))
        {
            _colliderList.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_colliderList.Contains(other))
        {
            _colliderList.Remove(other);
        }
    }
}
