using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonNetWorkStarter: MonoBehaviourPunCallbacks
{
    [SerializeField] DecideHostorClient _DecideHostorClient;
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("RoomA", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        GameObject practiceset = PhotonNetwork.Instantiate("PracticeSet", Vector3.zero, Quaternion.identity);
        _DecideHostorClient._practiceSet = practiceset.GetComponent<PracticeSet>();
        _DecideHostorClient.isConnecting = true;
        // ���������ԖڂɎQ����������ݒ�
        SetPlayerConnectedNumber();
    }

    private void SetPlayerConnectedNumber()
    {
        // �����̔ԍ����擾
        int playerIndex = GetPlayerIndex(PhotonNetwork.LocalPlayer);
        if (_DecideHostorClient != null)
        {
            _DecideHostorClient.ConnectedNumber = playerIndex;
        }
    }

    private int GetPlayerIndex(Player localPlayer)
    {
        // �v���C���[���X�g���擾���āA�����̏��Ԃ����
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == localPlayer)
            {
                return i + 1; // ���Ԃ�1����n�߂�
            }
        }
        return -1; // �G���[�l
    }
}