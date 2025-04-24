using Unity.Netcode;
using UnityEngine;

public class NetworkedCharacterAnimation : NetworkBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private FirstPersonMovement m_FirstPersonMovement;

    public NetworkVariable<Vector3> LinearVelocity = new NetworkVariable<Vector3>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public Vector3 Debug_LinearVelo;
    private void Update()
    {
        m_Animator.SetFloat("X", LinearVelocity.Value.x);
        m_Animator.SetFloat("Y", LinearVelocity.Value.y);

        Debug_LinearVelo = LinearVelocity.Value;
    }
}
