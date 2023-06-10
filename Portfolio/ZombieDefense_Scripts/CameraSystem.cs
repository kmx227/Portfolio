using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private float _panSpeed = 20f;
    [SerializeField] private float _panBorderThickness = 10f;
    [SerializeField] private Vector2 _panLimit;
    [SerializeField] private float _scrollSpeed = 20f;
    [SerializeField] private float _minY = 10f;
    [SerializeField] private float _maxY = 20f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;    
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _pos = transform.position;

        if(Input.mousePosition.y >= Screen.height - _panBorderThickness || Input.GetKey(KeyCode.W))
        {
            _pos.y += _panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= _panBorderThickness || Input.GetKey(KeyCode.S))
        {
            _pos.y -= _panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - _panBorderThickness || Input.GetKey(KeyCode.D))
        {
            _pos.x += _panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= _panBorderThickness || Input.GetKey(KeyCode.A))
        {
            _pos.x -= _panSpeed * Time.deltaTime;
        }

        _pos.x = Mathf.Clamp(_pos.x, -_panLimit.x, _panLimit.x);
        _pos.y = Mathf.Clamp(_pos.y, -_panLimit.y, _panLimit.y);

        transform.position = _pos;

        float _scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Camera.main.orthographicSize > 20)
        {
            Camera.main.orthographicSize = 20;
        }
        else if (Camera.main.orthographicSize < 5)
        {
            Camera.main.orthographicSize = 5;
        }
        else
        {
            Camera.main.orthographicSize -= _scroll * _scrollSpeed * 100f * Time.deltaTime;
        }
    }
}
