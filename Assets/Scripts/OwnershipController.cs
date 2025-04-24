using Unity.Netcode;
using UnityEngine;

public class OwnershipController : NetworkBehaviour
{
    [SerializeField] private MonoBehaviour[] m_TargetScripts;
    [SerializeField] private Camera[] m_TargetCameras;
    [SerializeField] private AudioListener[] m_TargetAudioListeners;

    private void Awake()
    {
        foreach (MonoBehaviour script in m_TargetScripts)
        {
            script.enabled = false;
        }

        foreach (Camera script in m_TargetCameras)
        {
            script.enabled = false;
        }

        foreach (AudioListener script in m_TargetAudioListeners)
        {
            script.enabled = false;
        }
    }

    private void Start()
    {
        if (IsOwner)
        {
            foreach (MonoBehaviour script in m_TargetScripts)
            {
                script.enabled = true;
            }

            foreach (Camera script in m_TargetCameras)
            {
                script.enabled = true;
            }

            foreach (AudioListener script in m_TargetAudioListeners)
            {
                script.enabled = true;
            }
        }
    }
}
