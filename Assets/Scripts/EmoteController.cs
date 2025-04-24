using Unity.Netcode;
using UnityEngine;

public class EmoteController : NetworkBehaviour
{
    [SerializeField] private Animator m_Animator;

    [Rpc(SendTo.ClientsAndHost)]
    public void TriggerEmoteRpc(string emote)
    {
        m_Animator.SetTrigger(emote);
    }
}
