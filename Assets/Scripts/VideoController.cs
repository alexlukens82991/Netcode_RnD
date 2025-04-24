using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : NetworkBehaviour
{
    [SerializeField] private VideoPlayer m_VideoPlayer;

    private double m_CurrentTime;

    private void Update()
    {
        m_CurrentTime = m_VideoPlayer.time;
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void PlayRpc()
    {
        m_VideoPlayer.time = m_CurrentTime;
        m_VideoPlayer.Play();
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void PauseRpc()
    {
        m_VideoPlayer.Pause();
        m_VideoPlayer.time = m_CurrentTime;
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void UpdateTimeRpc(double changeInTime)
    {
        m_VideoPlayer.time += changeInTime;
    }

}
