using Unity.Netcode;
using UnityEngine;


namespace SpaceShip
{
    public class Sun : NetworkBehaviour
    {
        [SerializeField] private GameObject _sunViewPrefab;
        [SerializeField] private Vector3 _sunPosition = Vector3.zero;


        private void Start()
        {
            Instantiate(_sunViewPrefab, transform).transform.localPosition = Vector3.zero;
            transform.position = _sunPosition;
        }
    }
}
