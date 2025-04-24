using Unity.Netcode;
using UnityEngine;

public class PlayerTrackerRegister : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        PlayerTracker.Instance.RegisterPlayer(transform);
    }

    public override void OnNetworkDespawn()
    {
        PlayerTracker.Instance.UnregisterPlayer(transform);
    }
}
