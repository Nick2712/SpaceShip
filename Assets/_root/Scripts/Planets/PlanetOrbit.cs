using Unity.Netcode;
using UnityEngine;


namespace SpaceShip
{
    public class PlanetOrbit : NetworkBehaviour
    {
        [SerializeField] private GameObject _viewPrefab;
        [SerializeField] private float _circleInSecond = 1f;
        [SerializeField] private float _offsetSin = 1;
        [SerializeField] private float _offsetCos = 1;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _distanceToSun;
        private Vector3 _aroundPoint = Vector3.zero;
        private float _currentAng;
        private float _currentRotationAngle;
        private const float _circleRadians = Mathf.PI * 2;


        private void Start()
        {
            if (IsServer)
            {
                var sun = FindObjectOfType<Sun>();
                if (sun != null) _aroundPoint = sun.transform.position;
                var startPosition = _aroundPoint;
                startPosition.x += _distanceToSun;
                transform.position = startPosition;
            }
            
            Instantiate(_viewPrefab, transform).transform.localPosition = Vector3.zero;
        }

        private void FixedUpdate()
        {
            HasAuthorityMovement();
        }

        private void HasAuthorityMovement()
        {
            if (!IsServer)
            {
                return;
            }
            var p = _aroundPoint;
            p.x += Mathf.Sin(_currentAng) * _distanceToSun * _offsetSin;
            p.z += Mathf.Cos(_currentAng) * _distanceToSun * _offsetCos;
            transform.position = p;
            _currentRotationAngle += Time.fixedDeltaTime * _rotationSpeed;
            _currentRotationAngle = Mathf.Clamp(_currentRotationAngle, 0, 361);
            if (_currentRotationAngle >= 360)
            {
                _currentRotationAngle = 0;
            }
            transform.rotation = Quaternion.AngleAxis(_currentRotationAngle,
                transform.up);
            _currentAng += _circleRadians * _circleInSecond * Time.fixedDeltaTime;
        }
    }
}
