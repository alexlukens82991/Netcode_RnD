using Unity.Netcode;
using UnityEngine;

public class StagingUI : MonoBehaviour
{
    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        int numberConnected = NetworkManager.Singleton.ConnectedClients.Count;

        print($"Clients connected: {numberConnected}/2");
    }
}
