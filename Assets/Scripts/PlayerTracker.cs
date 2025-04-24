using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    [SerializeField] private List<Transform> m_Players = new();

    public static PlayerTracker Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterPlayer(Transform player)
    {
        m_Players.Add(player);
    }

    public void UnregisterPlayer(Transform player)
    {
        m_Players.Remove(player);
    }

    public Transform GetClosestPlayer(Vector3 currentPosition)
    {
        if (m_Players.Count == 0)
            return null;

        Transform closestPlayer = m_Players[0];

        float smallestDistance = float.MaxValue;

        foreach (Transform player in m_Players)
        {
            float distance = Vector3.Distance(player.position, currentPosition);

            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }
}
