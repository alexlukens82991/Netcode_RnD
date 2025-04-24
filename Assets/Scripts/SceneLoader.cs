using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class SceneLoader : NetworkBehaviour
{
    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        print($"Client {clientId} connected!");

        if (NetworkManager.Singleton.ConnectedClients.Count == 2 && IsHost)
        {
            StartCoroutine(LoadSceneRoutine());
        }
    }

    private IEnumerator LoadSceneRoutine()
    {
        print("Loading scene...");

        yield return new WaitForSeconds(1);
        print("3");

        yield return new WaitForSeconds(1);
        print("2");

        yield return new WaitForSeconds(1);
        print("1");

        yield return new WaitForSeconds(1);

        NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
