using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _dragHeight = 1.5f;
    [SerializeField] private Vector2 _gameSize;
    [SerializeField] private Vector3 _respawnPosition;
    
    private bool _isDragging;
    private Plane _dragPlane;
    private Camera _mainCamera;
    private Rigidbody _rigidbody;
    private Vector3 _targetPosition;

    void Start()
    {
        _mainCamera = Camera.main;
        _dragPlane = new Plane(Vector3.up, new Vector3(0, _dragHeight, 0));
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Drag();
    }

    private void Drag()
    {
        if (!_isDragging)
        {
            return;
        }

        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (_dragPlane.Raycast(ray, out var enter))
        {
            var point = ray.GetPoint(enter);
            _targetPosition = point;
            _targetPosition.x = Mathf.Clamp(point.x, -_gameSize.x, _gameSize.x);
            _targetPosition.z = Mathf.Clamp(point.z, -_gameSize.y, _gameSize.y);
        }
    }

    private void FixedUpdate()
    {
        if (!_isDragging)
        {
            return;
        }
        
        _rigidbody.position = Vector3.Lerp(_rigidbody.position, _targetPosition, _speed * Time.fixedDeltaTime);
    }

    public void OnMouseDown()
    {
        _rigidbody.isKinematic = true;
        _isDragging = true;
    }

    public void OnMouseUp()
    {
        _rigidbody.isKinematic = false;
        _isDragging = false;
    }
}