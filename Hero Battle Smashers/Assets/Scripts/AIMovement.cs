using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [SerializeField] private Vector2 _minMaxTime;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotSpeed;
    [SerializeField] private float _killGainFactor;
    [SerializeField] private float _gainSpeed;
    [SerializeField] private CharacterController _charController;
    [SerializeField] private Animator _animator;
    [SerializeField] private SceneController _sceneController;    
    private Vector3 _direction;
    private Quaternion _qDirection;
    private float _movementTime;

    // Weapon Part
    [SerializeField] private WeaponEquipment _weaponEquipment;
    [SerializeField] private Transform _weaponPlace;
    private WeaponCollision _weaponCollision;

    [SerializeField] private Transform _healthbar;
    [SerializeField] private float _dieSpeed;

    private bool _alive;

    private void Start()
    {
        _alive = true;
        _movementTime = Random.Range(_minMaxTime.x, _minMaxTime.y);
        _direction = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        _direction = _direction.normalized * _speed * Time.deltaTime * Time.timeScale;
        _qDirection = Quaternion.LookRotation(_direction);

        _weaponCollision = _weaponEquipment.GetRandomWeaponCollision(_weaponPlace, transform);
    }

    private void Update()
    {
        if (_alive)
        {
            if (_movementTime <= 0)
            {
                int hits = _weaponCollision.EnemyHit();

                if (hits > 0)
                    StartCoroutine(gainPower(Mathf.Pow(_killGainFactor, hits)));

                _movementTime = Random.Range(_minMaxTime.x, _minMaxTime.y);
                _direction = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
                _direction = _direction.normalized * _speed * Time.deltaTime * Time.timeScale;
                _qDirection = Quaternion.LookRotation(_direction);
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, _qDirection, _rotSpeed * Time.deltaTime);
            _animator.SetFloat("Speed", _direction.sqrMagnitude * 1000);
            _charController.Move(_direction);
            _movementTime -= Time.deltaTime;
        }
    }

    public void Die()
    {
        if (!_alive)
            return;
        _alive = false;
        _animator.SetFloat("Speed", 0);
        _weaponCollision.transform.parent.gameObject.SetActive(false);
        _sceneController.EnemyDown();
        StartCoroutine(CoroutineDie());
    }

    private IEnumerator CoroutineDie()
    {
        this.transform.Rotate(-75, 0, 0);
        Vector3 newScale = new Vector3(0, _healthbar.localScale.y, _healthbar.localScale.z);
        while (_healthbar.localScale.x > 0.1f)
        {
            _healthbar.localScale = Vector3.Lerp(_healthbar.localScale, newScale, _dieSpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(this.gameObject);
    }

    private IEnumerator gainPower(float factor)
    {
        Debug.Log("Enemy gains!");
        Vector3 newScale = new Vector3(transform.localScale.x * factor, transform.localScale.y * factor, transform.localScale.z * factor);
        while (newScale.x > transform.localScale.x)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, newScale, _gainSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
