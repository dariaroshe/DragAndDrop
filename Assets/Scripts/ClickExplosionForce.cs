using UnityEngine;

namespace DefaultNamespace
{
    public class ClickExplosionForce : MonoBehaviour
    {
        [SerializeField] private float _explosionForce;
        [SerializeField] private float _explosionRadius;
        [SerializeField] private float _explosionHeight;
    
        private Camera _camera;
        private Plane _plane;

        private void Start()
        {
            _plane = new Plane(Vector3.up, new Vector3(0, _explosionHeight, 0));
            _camera = Camera.main;
        }

        private void Update()
        {
            TryMakeExplosion();
        }

        private void TryMakeExplosion()
        {
            if (!Input.GetMouseButtonDown(1))
            {
                return;
            }

            var mousePosition = Input.mousePosition;
            var ray = _camera.ScreenPointToRay(mousePosition);

            if (_plane.Raycast(ray, out var point))
            {
                var hitPosition = ray.GetPoint(point);
                ApplyExplosionForce(hitPosition);
            }
        }

        private void ApplyExplosionForce(Vector3 hitPosition)
        {
            var allObjects = Physics.OverlapSphere(hitPosition, _explosionRadius);

            foreach (var impactedCollider in allObjects)
            {
                if (impactedCollider.attachedRigidbody != null)
                {
                    impactedCollider.attachedRigidbody.AddExplosionForce(_explosionForce, hitPosition,
                        _explosionRadius);
                }
            }
        }
    }
}