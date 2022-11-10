using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _weaponPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _coinsPackPrefabs = new List<GameObject>();
    [SerializeField] private GameObject _powerPrefab;
    [SerializeField] private Vector3 _size;
    [SerializeField] private int _enemyCount;
    [SerializeField] private UIController _UIController;
    [SerializeField] private PlayerMovement _playerMovement;
    private int _coinsPicked;
    private bool _lose;

    private void Awake()
    {
        Time.timeScale = 0;
    }

    void Start()
    {
        _lose = false;
        _UIController.SetEnemyStartCount(_enemyCount);
    }

    public void CoinPicked()
    {
        _coinsPicked++;
        if (_coinsPicked % 4 == 0)
        {
            int index = Random.Range(0, _coinsPackPrefabs.Count);
            Vector3 position = new Vector3(Random.Range(0, _size.x / 2), 0.6f, Random.Range(0, _size.z / 2));
            Instantiate(_coinsPackPrefabs[index], position, Quaternion.identity);
        }
        _UIController.SetCoinsCount(_coinsPicked);
    }

    public void WeaponPicked()
    {
        int index = Random.Range(0, _weaponPrefabs.Count);
        Vector3 position = new Vector3(Random.Range(0, _size.x / 2), 0.5f, Random.Range(0, _size.z / 2));
        Instantiate(_weaponPrefabs[index], position, Quaternion.identity);
    }

    public void PowerPicked()
    {
        Vector3 position = new Vector3(Random.Range(0, _size.x / 2), 0.3f, Random.Range(0, _size.z / 2));
        Instantiate(_powerPrefab, position, Quaternion.identity);
    }

    public void EnemyDown()
    {
        _enemyCount--;
        _UIController.SetEnemyCount(_enemyCount);
        if (_enemyCount <= 0 && _lose == false)
        {
            _UIController.UIWin();
            _playerMovement.Win();
        }
    }
    public void Lose()
    {
        _lose = true;
        _UIController.UILose();
    }
}
