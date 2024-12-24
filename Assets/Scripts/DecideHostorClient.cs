using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.Burst.CompilerServices;

public class DecideHostorClient : MonoBehaviour
{
    public bool HostReady { get; set; } = false;
    public bool ClientReady { get; set; } = false;
    [SerializeField] BlackJackManager _BlackJackManager;
    [SerializeField] GameObject WaitforAnother;
    bool _DecideHostorClient = false;
    public bool isConnecting = false;
    private bool setplayerindex = false;
    private bool FindPracticeSet = false;
    public int ConnectedNumber = 0;
    public PracticeSet _practiceSet { get; set; }

    // Update is called once per frame
    void Update()
    {
        if (isConnecting)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1 && !_DecideHostorClient)
            {
                _BlackJackManager._hostorclient = BlackJackManager.HostorClient.Host;
                _BlackJackManager.MyConnectedNumber = 1;
                _DecideHostorClient = true;
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && !_DecideHostorClient)
            {
                _BlackJackManager._hostorclient = BlackJackManager.HostorClient.Client;
                _BlackJackManager.MyConnectedNumber = 2;
                _DecideHostorClient = true;
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 3 && !_DecideHostorClient)
            {
                _BlackJackManager._hostorclient = BlackJackManager.HostorClient.Client;
                _BlackJackManager.MyConnectedNumber = 3;
                _DecideHostorClient = true;
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 4 && !_DecideHostorClient)
            {
                _BlackJackManager._hostorclient = BlackJackManager.HostorClient.Client;
                _BlackJackManager.MyConnectedNumber = 4;
                _DecideHostorClient = true;
            }


            if (PhotonNetwork.CurrentRoom.PlayerCount > 3 && _DecideHostorClient && !setplayerindex)
            {
                _practiceSet.SetPlayerIndex(ConnectedNumber);
                setplayerindex = true;
            }
            if (setplayerindex && !FindPracticeSet)
            {
                PhotonView[] photonviews = FindObjectsOfType<PhotonView>();
                foreach (var _photonview in photonviews)
                {
                    if (_photonview.gameObject.GetComponent<PracticeSet>().playerindex == 1)
                    {
                        _practiceSet = _photonview.gameObject.GetComponent<PracticeSet>();
                        _BlackJackManager.SetPracticeSet(_practiceSet);
                        FindPracticeSet = true;
                    }
                }
            }
            if (FindPracticeSet)
            {
                if (_BlackJackManager._hostorclient == BlackJackManager.HostorClient.Host)
                {
                    _BlackJackManager.UpdateParameter();
                    _BlackJackManager.PhotonGameStartUI();
                }
                WaitforAnother.SetActive(false);
                this.gameObject.SetActive(false);

            }
        }
    }
}

