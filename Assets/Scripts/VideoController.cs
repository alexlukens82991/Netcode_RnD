using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : NetworkBehaviour
{
    [SerializeField] private VideoPlayer m_VideoPlayer;

    [SerializeField] private double m_CurrentTime;


    private void Update()
    {
        m_CurrentTime = m_VideoPlayer.time;
    }

    [Rpc(SendTo.Everyone)]
    public void PlayRpc()
    {
        m_VideoPlayer.time = m_CurrentTime;
        m_VideoPlayer.Play();
        print("Video play!");
    }

    [Rpc(SendTo.Everyone)]
    public void PauseRpc()
    {
        m_VideoPlayer.Pause();
        print("Video pause!");

        m_VideoPlayer.time = m_CurrentTime;
    }

    [Rpc(SendTo.Everyone)]
    public void UpdateTimeRpc(double changeInTime)
    {
        m_VideoPlayer.time += changeInTime;
    }

}
