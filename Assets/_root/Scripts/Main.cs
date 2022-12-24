using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


namespace SpaceShip
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private ShipSettings _shipSettings;
        [SerializeField] private PlanetSettings _planetSettings;
        [Header("Main Menu")]
        [SerializeField] private GameObject _mainMenuHolder;
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _clientButton;
        [SerializeField] private TMP_InputField _playerName;

        private readonly Dictionary<GameObject, string> _connectedPlayers = new Dictionary<GameObject, string>();
        public IReadOnlyDictionary<GameObject, string> ConnectedPlayers => _connectedPlayers;
        public string PlayerName => _playerName.text;


        private void Start()
        {
            _hostButton.onClick.AddListener(OnHostButton);
            _clientButton.onClick.AddListener(OnClientButton);
            _mainMenuHolder.SetActive(true);
        }

        private void OnHostButton()
        {
            NetworkManager.Singleton.StartHost();
            CloseMenu();
        }

        private void OnClientButton()
        { 
            NetworkManager.Singleton.StartClient();
            CloseMenu();
        }

        private void CloseMenu()
        {
            _mainMenuHolder.SetActive(false);
        }

        public void AddConnectedPlayer(GameObject player)
        {
            _connectedPlayers.Add(player, player.name);
        }

        public void RemoveConnectedPlayer(GameObject player) 
        { 
            _connectedPlayers.Remove(player);
        }

        private void OnDestroy()
        {
            _hostButton.onClick.RemoveAllListeners();
            _clientButton.onClick.RemoveAllListeners();
        }
    }
}
