using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotSpeed;
    [SerializeField] private float _colaGainFactor;
    [SerializeField] private float _killGainFactor;
    [SerializeField] private float _gainSpeed;
    [SerializeField] private FixedJoystick _fixedJoystick;
    [SerializeField] private CharacterController _charController;
    [SerializeField] private Animator _animator;
    [SerializeField] private WeaponEquipment _weaponEquipment;
    [SerializeField] private SceneController _sceneController;
    [SerializeField] private CameraMovement _cameraMovement;
    private WeaponCollision _weaponCollision;

    private Vector3 _direction;
    private bool _isMoving = false;

    private void Update()
    {
        _direction = Vector3.zero;
        if (_fixedJoystick.Horizontal != 0 || _fixedJoystick.Vertical != 0)
        {
            _isMoving = true;
            _direction = new Vector3(_fixedJoystick.Horizontal, 0, _fixedJoystick.Vertical);
            _direction = _direction.normalized * _speed * Time.deltaTime;
            Quaternion qDirection = Quaternion.LookRotation(_direction);                                           
            transform.rotation = Quaternion.Lerp(transform.rotation, qDirection, _rotSpeed * Time.deltaTime);      
        }

        // Выстрел в конце движения
        else if (_isMoving)
        {
            _weaponCollision = _weaponEquipment.GetCurrentWeaponCollision();
            int hits = _weaponCollision.PlayerHit();
            if (hits > 0)
            {
                StartCoroutine(gainPower(Mathf.Pow(_killGainFactor, hits)));
                _cameraMovement.OffsetAdd(hits);
            }
            _isMoving = false;
        }
        _animator.SetFloat("Speed", _direction.sqrMagnitude * 1000);
        _charController.Move(_direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            _sceneController.CoinPicked();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("BaseballBat"))
        {
            _weaponEquipment.SetCurrentWeapon(0);
            _sceneController.WeaponPicked();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("MachinePistol"))
        {
            _weaponEquipment.SetCurrentWeapon(1);
            _sceneController.WeaponPicked();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Cola"))
        {
            StartCoroutine(gainPower(_colaGainFactor));
            _sceneController.PowerPicked();
            Destroy(other.gameObject);
        }
    }

    private IEnumerator gainPower(float factor)
    {
        Vector3 newScale = new Vector3(transform.localScale.x * factor, transform.localScale.y * factor, transform.localScale.z * factor);
        while (newScale.x > transform.localScale.x)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, newScale, _gainSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void Lose()
    {
        _sceneController.Lose();
        _cameraMovement.enabled = false;    // For never calling Behaviour scripts
        _fixedJoystick.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void Win()
    {
        _cameraMovement.MoveAway();
        _fixedJoystick.gameObject.SetActive(false);
    }
}
