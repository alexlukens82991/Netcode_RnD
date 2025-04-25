using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Services.Relay.Models;

public class RelayManager : MonoBehaviour
{
    [SerializeField] private Button m_HostBtn;
    [SerializeField] private Button m_JoinBtn;
    [SerializeField] private TextMeshProUGUI m_CodeTxt;
    [SerializeField] private TMP_InputField m_CodeInputField;
    [SerializeField] private Canvas m_Canvas;

    async void Start()
    {
        await UnityServices.InitializeAsync();

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        m_HostBtn.onClick.AddListener(CreateRelay);
        m_JoinBtn.onClick.AddListener(() => JoinRelay(m_CodeInputField.text));
    }

    async void CreateRelay()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        m_CodeTxt.text = "Code: " + joinCode;

        var relayServerData = AllocationUtils.ToRelayServerData(allocation, "dtls");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartHost();

        m_Canvas.enabled = false;
    }

    async void JoinRelay(string joinCode)
    {
        JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

        var relayServerData = AllocationUtils.ToRelayServerData(joinAllocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        NetworkManager.Singleton.StartClient();

        m_Canvas.enabled = false;
    }
}
