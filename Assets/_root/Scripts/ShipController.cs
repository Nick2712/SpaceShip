using Unity.Collections;
using Unity.Netcode;
using UnityEngine;


namespace SpaceShip
{
    public class ShipController : NetworkBehaviour
    {
        [SerializeField] private GameObject _sun;
        [SerializeField] private GameObject[] _planets;

        [SerializeField] private Transform _cameraAttach;
        [SerializeField] private ShipSettings _shipSettings;
        private CameraOrbit _cameraOrbit;
        private PlayerLabel _playerLabel;
        private Rigidbody _rb;
        private float _shipSpeed;
        private Main _main;

        private NetworkVariable<FixedString128Bytes> _name = new NetworkVariable<FixedString128Bytes>(default,
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        private void OnGUI()
        {
            if (_cameraOrbit == null) return;
            //if (IsOwner) _cameraOrbit.ShowPlayerLabels(_playerLabel);
        }

        private void Start()
        {
            _main = FindObjectOfType<Main>();

            transform.position = new Vector3(10, 10, 10);
            if (IsOwner) OnStartAuthority();
            var ship = Instantiate(_shipSettings.ShipPrefabs[0], transform);
            ship.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            ship.transform.localPosition = Vector3.zero;
            ship.transform.localRotation = Quaternion.identity;
            DestroyImmediate(ship.GetComponent<MeshCollider>());

            name = _name.Value.ToString();
            _main.AddConnectedPlayer(gameObject);
        }

        private void Update()
        {
            if (IsOwner) HasAuthorityMovement(Time.deltaTime);
        }

        public void OnStartAuthority()
        {
            if (!TryGetComponent(out _rb)) return;
            _cameraOrbit = FindObjectOfType<CameraOrbit>();
            _cameraOrbit.Initiate(_cameraAttach == null ? transform :
                _cameraAttach);
            _playerLabel = GetComponentInChildren<PlayerLabel>();

            if (IsServer) Instantiates();

            _name.Value = _main.PlayerName;
        }

        private void HasAuthorityMovement(float deltaTime)
        {
            var isFaster = Input.GetKey(KeyCode.LeftShift);
            var speed = _shipSettings.ShipSpeed;
            var faster = isFaster ? _shipSettings.Faster : 1.0f;
            _shipSpeed = Mathf.Lerp(_shipSpeed, speed * faster,
                _shipSettings.Acceleration);
            var currentFov = isFaster
                ? _shipSettings.FasterFov
                : _shipSettings.NormalFov;
            _cameraOrbit.SetFov(currentFov, _shipSettings.ChangeFovSpeed);
            var velocity =
                _cameraOrbit.transform.TransformDirection(Vector3.forward) * _shipSpeed;
            _rb.velocity = velocity * deltaTime;
            if (!Input.GetKey(KeyCode.C))
            {
                var targetRotation = Quaternion.LookRotation(
                    Quaternion.AngleAxis(_cameraOrbit.LookAngle, -transform.right) * velocity);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, deltaTime * speed);
            }
        }

        private void Instantiates()
        {
            Instantiate(_sun).GetComponent<NetworkObject>().Spawn();
            foreach (var planet in _planets)
                Instantiate(planet).GetComponent<NetworkObject>().Spawn();
        }

        private void LateUpdate()
        {
            if (IsOwner) _cameraOrbit?.CameraMovement();
        }
    }
}
