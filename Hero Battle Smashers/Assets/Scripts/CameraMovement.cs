using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private Vector3 _offset;
    private Vector3 _offsetY = new Vector3(0f, 0.5f, 0f);

    void Start()
    {
        _offset = _target.position - transform.position;
    }

    void LateUpdate()
    {
        transform.position = _target.position - _offset;
        transform.LookAt(_target.position + _offsetY);
    }

    public void OffsetAdd(int times)
    {
        StartCoroutine(moveAwayCoroutine(0.5f * times));
    }

    public void MoveAway()
    {
        StartCoroutine(moveAwayCoroutine(7f));
    }

    private IEnumerator moveAwayCoroutine(float f)
    {
        Vector3 newOffset = new Vector3(_offset.x, _offset.y - f, _offset.z + f);
        while (_offset.z < newOffset.z)
        {
            _offset = Vector3.Lerp(_offset, newOffset, Time.deltaTime);
            yield return null;
        }
    }
}
