using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBox : MonoBehaviour
{
    [SerializeField] private List<GameObject> _weaponPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _coinsPackPrefabs = new List<GameObject>();
    [SerializeField] private GameObject _powerPrefab;

    private void OnDestroy()
    {
        if (!this.gameObject.scene.isLoaded) return;
        int random = Random.Range(1, 4);
        GameObject spawnObject;
        switch (random)
        {
            case 1:
                {
                    int index = Random.Range(0, _coinsPackPrefabs.Count);
                    spawnObject = _coinsPackPrefabs[index];
                    Instantiate(_coinsPackPrefabs[index], transform.position, Quaternion.identity);
                    break;
                }
            case 2:
                {
                    int index = Random.Range(0, _weaponPrefabs.Count);
                    spawnObject = _weaponPrefabs[index];
                    break;
                }
            case 3:
                {
                    spawnObject = _powerPrefab;
                    break;
                }
            default:
                spawnObject = null;
                break;
        }
        if (spawnObject != null)
            Instantiate(spawnObject, transform.position, Quaternion.identity);
    }


}
