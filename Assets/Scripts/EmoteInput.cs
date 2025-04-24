using UnityEngine;

public class EmoteInput : MonoBehaviour
{
    [SerializeField] private EmoteController m_EmoteController;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            m_EmoteController.TriggerEmoteRpc("Breakdance");
        }
    }
}
