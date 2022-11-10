using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponEquipment : MonoBehaviour
{
    [SerializeField] private List<WeaponPrefab> _weapons = new List<WeaponPrefab>();
    [SerializeField] private Transform _weaponPlace;
    [SerializeField] private Transform _characterTransform;

    private int _currentWeaponID;
    private GameObject _weaponModel;
    private GameObject _weaponRange;
    private WeaponCollision[] _weaponCollision;

    void Start()
    {
        _currentWeaponID = 1;
        _weaponModel = Instantiate(_weapons[_currentWeaponID].GetWeaponModel(), _weaponPlace.position, 
            _weaponPlace.rotation, _weaponPlace);
        _weaponRange = Instantiate(_weapons[_currentWeaponID].GetWeaponRange(), _characterTransform.position, 
            _characterTransform.rotation, _characterTransform);
        _weaponCollision = _weaponRange.GetComponentsInChildren<WeaponCollision>();
    }

    public WeaponCollision GetCurrentWeaponCollision()
    {
        return _weaponCollision[0];
    }

    public void SetCurrentWeapon(int i)
    {
        if (_currentWeaponID == i)
            return;
        _currentWeaponID = i;
        Destroy(_weaponModel);
        Destroy(_weaponRange);
        _weaponModel = Instantiate(_weapons[_currentWeaponID].GetWeaponModel(), _weaponPlace.position, _weaponPlace.rotation, _weaponPlace);
        _weaponRange = Instantiate(_weapons[_currentWeaponID].GetWeaponRange(), _characterTransform.position,
            _characterTransform.rotation, _characterTransform);
        _weaponCollision = _weaponRange.GetComponentsInChildren<WeaponCollision>();
    }

    public WeaponCollision GetRandomWeaponCollision(Transform weaponPlace, Transform enemyTransform)
    {
        int id = Random.Range(0, _weapons.Count);
        Instantiate(_weapons[id].GetWeaponModel(), weaponPlace.position, weaponPlace.rotation, weaponPlace);
        GameObject weaponRange = Instantiate(_weapons[id].GetWeaponRange(), enemyTransform.position,
            enemyTransform.rotation, enemyTransform);
        WeaponCollision[] weaponCollisions = weaponRange.GetComponentsInChildren<WeaponCollision>();
        return weaponCollisions[0];
    }
}

[Serializable]
class WeaponPrefab
{
    [SerializeField] private GameObject _weaponModel;
    [SerializeField] private GameObject _weaponRange;

    public GameObject GetWeaponModel()
    {
        return _weaponModel;
    }

    public GameObject GetWeaponRange()
    {
        return _weaponRange;
    }
}
