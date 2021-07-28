using System;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public GameObject canvas;
    private bool _isDragging = false;

    private GameObject _startParent;
    private Vector2 _startPosition;
    private GameObject _dropzone;
    private bool _isOverDropzone;

    private void Start()
    {
        canvas = GameObject.FindWithTag("mainCanvas");
    }

    void Update()
    {
        if (_isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.SetParent(canvas.transform, true);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _isOverDropzone = true;
        _dropzone = other.gameObject;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        _isOverDropzone = true;
        _dropzone = null;
    }

    public void SetIsDragging(bool value)
    {
        _isDragging = value;
        if (value)
        {
            _startParent = transform.parent.gameObject;
            _startPosition = transform.position;
        }
        else if (_isOverDropzone)
        {
            transform.SetParent(_dropzone.transform, false);
        }
        else
        {
            transform.position = _startPosition;
            transform.SetParent(_startParent.transform);
        }
    }
}