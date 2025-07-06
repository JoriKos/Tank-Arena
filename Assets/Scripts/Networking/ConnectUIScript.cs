using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class ConnectUIScript : MonoBehaviour
{
    [SerializeField] private Button _hostButton, _clientButton;
    [SerializeField] private bool _startHost, _startClient;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _hostButton.onClick.AddListener(HostButtonOnClick);
        _clientButton.onClick.AddListener(ClientButtonOnClick);

        if (_startHost)
            NetworkManager.Singleton.StartHost();
        
        if (_startClient)
            NetworkManager.Singleton.StartClient();



    }

    private void HostButtonOnClick()
    {
        NetworkManager.Singleton.StartHost();
    }
    
    private void ClientButtonOnClick()
    {
        NetworkManager.Singleton.StartClient();
    }
}
