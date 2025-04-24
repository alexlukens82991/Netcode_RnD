using System;
using Unity.Netcode;
using UnityEngine;

public class StagingManager : MonoBehaviour
{
    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        print($"Client {clientId} connected!");
    }

}
