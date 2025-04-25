using UnityEngine;

public class VideoInput : MonoBehaviour
{
    [SerializeField] private VideoController m_VideoController;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_VideoController.PlayRpc();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_VideoController.PauseRpc();
        }

        if (Input.GetKeyDown(KeyCode.Period))
        {
            m_VideoController.UpdateTimeRpc(5);
        }

        if (Input.GetKeyDown(KeyCode.Comma))
        {
            m_VideoController.UpdateTimeRpc(-5);
        }
    }
}
