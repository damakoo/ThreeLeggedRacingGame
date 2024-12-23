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
        // 自分が何番目に参加したかを設定
        SetPlayerConnectedNumber();
    }

    private void SetPlayerConnectedNumber()
    {
        // 自分の番号を取得
        int playerIndex = GetPlayerIndex(PhotonNetwork.LocalPlayer);
        if (_DecideHostorClient != null)
        {
            _DecideHostorClient.ConnectedNumber = playerIndex;
        }
    }

    private int GetPlayerIndex(Player localPlayer)
    {
        // プレイヤーリストを取得して、自分の順番を特定
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == localPlayer)
            {
                return i + 1; // 順番を1から始める
            }
        }
        return -1; // エラー値
    }
}