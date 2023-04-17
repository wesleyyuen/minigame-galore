using System.Collections.Generic;
using UnityEngine;

namespace Fighting
{
    public class CameraFollowTarget : MonoBehaviour
    {
        [SerializeField] private List<Transform> _targets;
        [SerializeField] private float _smoothTime;
        [Header("Zoom")]
        [SerializeField] private float _zoomMax;
        [SerializeField] private float _zoomMin;
        [Header("Pan")]
        [SerializeField] private Vector3 _halfFocusBounds;
        private Camera _camera;
        private Bounds _cameraBounds;
        private Vector3 _cameraPosition;
        private float _zoom;
        private Vector3 _cameraVelocityRef;
        private float _cameraZoomVelocityRef;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            var pos = transform.position;
            Bounds bounds = new Bounds();
            bounds.Encapsulate(new Vector3(pos.x - _halfFocusBounds.x, pos.y - _halfFocusBounds.y, pos.z - _halfFocusBounds.z));
            bounds.Encapsulate(new Vector3(pos.x + _halfFocusBounds.x, pos.y + _halfFocusBounds.y, pos.z + _halfFocusBounds.z));
            _cameraBounds = bounds;
            Utility.DrawBounds(_cameraBounds);
        }

        private void LateUpdate()
        {
            CalculateCameraLocation();

            transform.position = Vector3.SmoothDamp(transform.position, _cameraPosition, ref _cameraVelocityRef, _smoothTime);
            _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, _zoom, ref _cameraZoomVelocityRef, _smoothTime * 0.1f);
        }

        private void CalculateCameraLocation()
        {
            var total = Vector3.zero;
            var playerBounds = new Bounds();

            foreach (var target in _targets)
            {
                if (target == null) continue;
                var pos = target.position;
                if (!_cameraBounds.Contains(pos)) continue;

                var x = Mathf.Clamp(pos.x, _cameraBounds.min.x, _cameraBounds.max.x);
                var y = Mathf.Clamp(pos.y, _cameraBounds.min.y, _cameraBounds.max.y);
                var z = Mathf.Clamp(pos.z, _cameraBounds.min.z, _cameraBounds.max.z);
                pos = new Vector3(x,y,z);
                
                total += pos;
                playerBounds.Encapsulate(pos);
            }

            var midpoint = total / _targets.Count;

            var lerpPercent = Mathf.InverseLerp(0, (_halfFocusBounds.x + _halfFocusBounds.y) / 2f, playerBounds.extents.x + playerBounds.extents.y);
            _zoom = Mathf.Lerp(_zoomMax, _zoomMin, lerpPercent);

            _cameraPosition = new Vector3(midpoint.x, midpoint.y, transform.position.z);
        }
    }
}