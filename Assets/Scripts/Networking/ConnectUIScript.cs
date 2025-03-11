using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class ConnectUIScript : MonoBehaviour
{
    [SerializeField] private Button _hostButton, _clientButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _hostButton.onClick.AddListener(HostButtonOnClick);
        _clientButton.onClick.AddListener(ClientButtonOnClick);
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
