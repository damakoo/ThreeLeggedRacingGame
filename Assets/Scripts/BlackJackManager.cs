using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static BlackJackManager;
using Unity.VisualScripting;

public class BlackJackManager : MonoBehaviour
{
    [SerializeField] CardsList _cardslist;
    [SerializeField] int TimeLimit;
    [SerializeField] int ResultsTime = 5;
    [SerializeField] int WaitingTime = 3;
    [SerializeField] int NumberofSet = 10;
    [SerializeField] TextMeshProUGUI FinishUI;
    [SerializeField] BlackJackRecorder _blackJackRecorder;
    [SerializeField] TextMeshProUGUI MyScoreUI;
    [SerializeField] GameObject ClientUi;
    [SerializeField] DecideHostorClient _decideHostorClient;
    [SerializeField] GameObject StartingUi;
    [SerializeField] GameObject StartingUi_button;
    [SerializeField] GameObject ShowTargetUI;
    [SerializeField] GameObject ShowTargetPos;
    [SerializeField] GameObject WaitforStartUi;
    [SerializeField] GameObject _SceneReloaderHost;
    [SerializeField] GameObject _SceneReloaderClient;
    [SerializeField] GameObject TimeLimitObj;
    [SerializeField] GameObject TimeLimit_Bet;
    [SerializeField] GameObject TimeLimit_notBet;
    [SerializeField] GameObject AllTrialFinishedUI;
    [SerializeField] TextMeshProUGUI TimeLimitObj_str;
    public GameObject Clubs;
    public GameObject Spades;
    public GameObject Hearts;
    public GameObject Diamonds;
    [SerializeField] GameObject ClubsInitialpos;
    [SerializeField] GameObject SpadesInitialpos;
    [SerializeField] GameObject HeartsInitialpos;
    [SerializeField] GameObject DiamondsInitialpos;
    [SerializeField] GameObject StartLinePos;
    [SerializeField] GameObject GoalLinepos;
    [SerializeField] WriteLine BlackWriteLine;
    [SerializeField] WriteLine RedWriteLine;
    [SerializeField] GameObject _MovableArea;
    [SerializeField] float MouseMoveRatio = 30;
    public float AffordedDisntace;
    //[SerializeField] TextMeshProUGUI YourScoreUI;
    public PracticeSet _PracticeSet { get; set; }
    public int MyConnectedNumber { get; set; } = 0;
    public Vector3 cursorPosition { get; set; }
    Vector3 newcursorPosition;
    Vector3 DeltacursorPosition;
    Vector3 MovedPos;
    float BlackDistance;
    float RedDistance;


    public enum HostorClient
    {
        Host = 0,
        Client = 1
    }
    public HostorClient _hostorclient { get; set; }
    int nowTrial = 0;
    float nowTime = 0;
    public bool hasPracticeSet { get; set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        FinishUI.text = "";
        TimeLimitObj_str.text = "";

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("x:" + Input.mousePosition.x.ToString() + "\n" + "y:" + Input.mousePosition.y.ToString() + "\n" + "z:" + Input.mousePosition.z.ToString());
        if (hasPracticeSet)
        {
            if (_hostorclient == HostorClient.Host)
            {
                if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.BeforeStart)
                {
                    StartingGame();
                    if (_PracticeSet.FirstPressed && _PracticeSet.SecondPressed && _PracticeSet.ThirdPressed && _PracticeSet.FourthPressed)
                    {
                        PhotonMoveToWaitForNextTrial(nowTrial);
                        _PracticeSet.SetFirstPressed(false);
                        _PracticeSet.SetSecondPressed(false);
                        _PracticeSet.SetThirdPressed(false);
                        _PracticeSet.SetFourthPressed(false);
                    }
                    //if (Input.GetKeyDown(KeyCode.Space)) PhotonMoveToWaitForNextTrial(nowTrial);
                }
                else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.WaitForNextTrial)
                {
                    //if (Input.GetKeyDown(KeyCode.Space)) MoveToShowMyCards();
                    nowTime += Time.deltaTime;
                    _PracticeSet.SetTimeLeft(WaitingTime - nowTime);
                    if (nowTime > WaitingTime)
                    {
                        nowTime = 0;
                        PhotonMoveToShowMyCards();
                    }
                }
                else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.ShowMyCards)
                {
                    PhotonMoveToSelectCards();
                }
                else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectCards)
                {
                    //nowTime += Time.deltaTime;
                    //_PracticeSet.SetTimeLeft(TimeLimit - nowTime);
                    BlackJacking();
                    //if (nowTime > TimeLimit) PhotonMoveToSelectBet();
                }
                else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectBet)
                {
                    PhotonMoveToShowResult();
                }
                else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.ShowResult)
                {
                    //if (Input.GetKeyDown(KeyCode.Space)) MoveToWaitForNextTrial();
                    nowTime += Time.deltaTime;
                    _PracticeSet.SetTimeLeft(ResultsTime - nowTime);
                    if (nowTime > ResultsTime)
                    {
                        nowTime = 0;
                        PhotonMoveToWaitForNextTrial(nowTrial);
                    }
                }
                else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.Finished)
                {
                    if (_PracticeSet.FirstPressed && _PracticeSet.SecondPressed && _PracticeSet.ThirdPressed && _PracticeSet.FourthPressed)
                    {
                        PhotonRestart();
                    }
                }

            }
            else if (_hostorclient == HostorClient.Client && _PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.BeforeStart)
            {
                StartingGame();
            }
            else if (_hostorclient == HostorClient.Client && _PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectCards)
            {
                BlackJacking();
            }
            else if (_hostorclient == HostorClient.Client && _PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectBet)
            {
            }
            else if (_hostorclient == HostorClient.Client)
            {
                nowTime = 0;
            }

            if (_PracticeSet.BlackJackState != PracticeSet.BlackJackStateList.BeforeStart) TimeLimitObj_str.text = "Time: " + Mathf.CeilToInt(_PracticeSet.TimeLeft).ToString();
        }
    }
    public void SetPracticeSet(PracticeSet _practiceset)
    {
        _PracticeSet = _practiceset;
        _cardslist.SetPracticeSet(_practiceset);
        hasPracticeSet = true;
    }


    public void UpdateParameter()
    {
        _PracticeSet.UpdateParameter();
    }
    public void ReUpdateParameter()
    {
        _PracticeSet.ReUpdateParameter();
    }
    public void ReInitializeCard()
    {
        _cardslist.ReInitializeCards();
    }
    void BlackJacking()
    {
        //newcursorPosition = Input.mousePosition;
        //DeltacursorPosition = (newcursorPosition - cursorPosition);
        DeltacursorPosition = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"),0) / MouseMoveRatio;
        BlackDistance = Vector3.Magnitude(_PracticeSet.Clubs - _PracticeSet.Spades);
        RedDistance = Vector3.Magnitude(_PracticeSet.Hearts - _PracticeSet.Diamonds);

        if (MyConnectedNumber == 1)
        {
            if (_PracticeSet.Clubs.x < StartLinePos.transform.position.x && _PracticeSet.Clubs.x + DeltacursorPosition.x > StartLinePos.transform.position.x)
            {
                if (BlackDistance < AffordedDisntace)
                {
                    MovedPos = _PracticeSet.Clubs + DeltacursorPosition;
                }
                else
                {
                    MovedPos = _PracticeSet.Clubs + new Vector3(0, DeltacursorPosition.y, DeltacursorPosition.z);
                }
            }
            else
            {
                MovedPos = _PracticeSet.Clubs + DeltacursorPosition;
            }
            _PracticeSet.SetClubs(MoveSuitWithinFrame(MovedPos));
        }
        else if (MyConnectedNumber == 2)
        {
            if (_PracticeSet.Spades.x < StartLinePos.transform.position.x && _PracticeSet.Spades.x + DeltacursorPosition.x > StartLinePos.transform.position.x)
            {
                if (BlackDistance < AffordedDisntace)
                {
                    MovedPos = _PracticeSet.Spades + DeltacursorPosition;
                }
                else
                {
                    MovedPos = _PracticeSet.Spades + new Vector3(0, DeltacursorPosition.y, DeltacursorPosition.z);
                }
            }
            else
            {
                MovedPos = _PracticeSet.Spades + DeltacursorPosition;
            }
            _PracticeSet.SetSpades(MoveSuitWithinFrame(MovedPos));
        }
        else if (MyConnectedNumber == 3)
        {
            if (_PracticeSet.Hearts.x < StartLinePos.transform.position.x && _PracticeSet.Hearts.x + DeltacursorPosition.x > StartLinePos.transform.position.x)
            {
                if (RedDistance < AffordedDisntace)
                {
                    MovedPos = _PracticeSet.Hearts + DeltacursorPosition;
                }
                else
                {
                    MovedPos = _PracticeSet.Hearts + new Vector3(0, DeltacursorPosition.y, DeltacursorPosition.z);
                }
            }
            else
            {
                MovedPos = _PracticeSet.Hearts + DeltacursorPosition;
            }
            _PracticeSet.SetHearts(MoveSuitWithinFrame(MovedPos));
        }
        else if (MyConnectedNumber == 4)
        {
            if (_PracticeSet.Diamonds.x < StartLinePos.transform.position.x && _PracticeSet.Diamonds.x + DeltacursorPosition.x > StartLinePos.transform.position.x)
            {
                if (RedDistance < AffordedDisntace)
                {
                    MovedPos = _PracticeSet.Diamonds + DeltacursorPosition;
                }
                else
                {
                    MovedPos = _PracticeSet.Diamonds + new Vector3(0, DeltacursorPosition.y, DeltacursorPosition.z);
                }
            }
            else
            {
                MovedPos = _PracticeSet.Diamonds + DeltacursorPosition;
            }
            _PracticeSet.SetDiamonds(MoveSuitWithinFrame(MovedPos));
        }

        if (_hostorclient == HostorClient.Host)
        {

            if (_PracticeSet.Clubs.x > StartLinePos.transform.position.x || _PracticeSet.Spades.x > StartLinePos.transform.position.x)
            {
                if (BlackDistance > AffordedDisntace)
                {
                    _PracticeSet.SetClubs(ClubsInitialpos.transform.position);
                    _PracticeSet.SetSpades(SpadesInitialpos.transform.position);
                }
            }


            if (_PracticeSet.Hearts.x > GoalLinepos.transform.position.x || _PracticeSet.Diamonds.x > GoalLinepos.transform.position.x)
            {
                _PracticeSet.SetRedCleared(true);
            }

            if (_PracticeSet.Hearts.x > StartLinePos.transform.position.x || _PracticeSet.Diamonds.x > StartLinePos.transform.position.x)
            {
                if (RedDistance > AffordedDisntace)
                {
                    _PracticeSet.SetHearts(HeartsInitialpos.transform.position);
                    _PracticeSet.SetDiamonds(DiamondsInitialpos.transform.position);
                }
            }



            if (_PracticeSet.Clubs.x > GoalLinepos.transform.position.x || _PracticeSet.Spades.x > GoalLinepos.transform.position.x)
            {
                _PracticeSet.SetBlackCleared(true);
                PhotonMoveToShowResult();
            }

            if (_PracticeSet.Hearts.x > GoalLinepos.transform.position.x || _PracticeSet.Diamonds.x > GoalLinepos.transform.position.x)
            {
                _PracticeSet.SetRedCleared(true);
                PhotonMoveToShowResult();

            }
        }

        Clubs.transform.position = _PracticeSet.Clubs;
        Spades.transform.position = _PracticeSet.Spades;
        Hearts.transform.position = _PracticeSet.Hearts;
        Diamonds.transform.position = _PracticeSet.Diamonds;

        BlackWriteLine.WritingLine(Clubs.transform.position, Spades.transform.position);
        RedWriteLine.WritingLine(Hearts.transform.position, Diamonds.transform.position);
        //cursorPosition = newcursorPosition;

    }
    public void GameStartUI()
    {
        StartingUi.SetActive(true);
        StartingUi_button.SetActive(true);
        ShowTargetUI.SetActive(true);
        FadeorAppearAllSuit(false);
        if (MyConnectedNumber == 1)
        {
            Clubs.SetActive(true);
            Clubs.transform.position = ShowTargetPos.transform.position;
        }
        else if (MyConnectedNumber == 2)
        {
            Spades.SetActive(true);
            Spades.transform.position = ShowTargetPos.transform.position;
        }
        else if (MyConnectedNumber == 3)
        {
            Hearts.SetActive(true);
            Hearts.transform.position = ShowTargetPos.transform.position;
        }
        else if (MyConnectedNumber == 4)
        {
            Diamonds.SetActive(true);
            Diamonds.transform.position = ShowTargetPos.transform.position;
        }

    }
    public void FadeorAppearAllSuit(bool _visible)
    {
        Clubs.SetActive(_visible);
        Spades.SetActive(_visible);
        Hearts.SetActive(_visible);
        Diamonds.SetActive(_visible);
    }
    private void SetAllSuitsInitialPos()
    {
        _PracticeSet.SetClubs(ClubsInitialpos.transform.position);
        _PracticeSet.SetSpades(SpadesInitialpos.transform.position);
        _PracticeSet.SetHearts(HeartsInitialpos.transform.position);
        _PracticeSet.SetDiamonds(DiamondsInitialpos.transform.position);
        _PracticeSet.MoveAllSuitsInitialPos();
    }
    public void MoveAllSuitsInitialPos()
    {
        FadeorAppearAllSuit(true);
        Clubs.transform.position = ClubsInitialpos.transform.position;
        Spades.transform.position = SpadesInitialpos.transform.position;
        Hearts.transform.position = HeartsInitialpos.transform.position;
        Diamonds.transform.position = DiamondsInitialpos.transform.position;
    }

    public void PhotonGameStartUI()
    {
        _PracticeSet.GameStartUi();
    }
    void StartingGame()
    {
        Cursor.lockState = CursorLockMode.None;

        // �}�E�X�{�^�����N���b�N���ꂽ���m�F
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

            // ���C�L���X�g���g�p���ăI�u�W�F�N�g�����o
            if (hit && hit.collider.gameObject.CompareTag("Start"))
            {
                if (MyConnectedNumber == 1)
                {
                    _PracticeSet.SetFirstPressed(true);
                }
                else if (MyConnectedNumber == 2)
                {
                    _PracticeSet.SetSecondPressed(true);
                }
                else if (MyConnectedNumber == 3)
                {
                    _PracticeSet.SetThirdPressed(true);
                }
                else if (MyConnectedNumber == 4)
                {
                    _PracticeSet.SetFourthPressed(true);
                }
                WaitforStartUi.SetActive(true);
                StartingUi.SetActive(false);
                StartingUi_button.SetActive(false);
            }
        }
    }

    public void MoveToShowMyCards(int hostorClient)
    {
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.ShowMyCards;
        TimeLimitObj.transform.position = TimeLimit_notBet.transform.position;
    }
    public void PhotonMoveToShowMyCards()
    {
        _PracticeSet.MoveToShowMyCards((int)_hostorclient);
    }
    public void PhotonMoveToSelectBet()
    {
        _PracticeSet.MoveToSelectBet();
        PhotonMoveToShowResult();
    }
    public void MoveToSelectBet()
    {
        nowTime = 0;
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.SelectBet;
        TimeLimitObj.transform.position = TimeLimit_Bet.transform.position;
    }
    public void MoveToSelectCards()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ShowTargetUI.SetActive(false);
        TimeLimitObj.SetActive(false);
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.SelectCards;
        TimeLimitObj.transform.position = TimeLimit_notBet.transform.position;
        cursorPosition = Input.mousePosition;

    }
    public void PhotonMoveToSelectCards()
    {
        SetAllSuitsInitialPos();
        _PracticeSet.MoveToSelectCards();
    }
    public void MoveToShowResult()
    {
        // ロックを解除
        Cursor.lockState = CursorLockMode.None;
        // カーソルを表示
        Cursor.visible = true;
        //YourScoreUI.text = Score.ToString();
        nowTime = 0;
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.Finished;
        _blackJackRecorder.ExportCsv(_PracticeSet.BlackCleared?"Black":"Red");
        if (MyConnectedNumber == 1 || MyConnectedNumber == 2)
        {
            MyScoreUI.text = "You" + (_PracticeSet.BlackCleared ? "Win!!" : "Lose!!") + "\n" + "Trial: " + _blackJackRecorder.Trial.ToString() + "/" + NumberofSet.ToString();
        }
        else if (MyConnectedNumber == 3 || MyConnectedNumber == 4)
        {
            MyScoreUI.text = "You" + (_PracticeSet.RedCleared ? "Win!!" : "Lose!!") + "\n" + "Trial: " + _blackJackRecorder.Trial.ToString() + "/" + NumberofSet.ToString();
        }
        //_blackJackRecorder.WriteResult();
        //_blackJackRecorder.ExportCsv();
        if (_blackJackRecorder.Trial == NumberofSet)
        {
            AllTrialFinishedUI.SetActive(true);
        }
        else
        {
            _SceneReloaderHost.SetActive(true);
        }

        TimeLimitObj_str.text = "";
    }
    public void PhotonMoveToShowResult()
    {
        _PracticeSet.MoveToShowResult();
    }
    public void MoveToWaitForNextTrial(int _nowTrial)
    {
        _blackJackRecorder.Initialize();
        WaitforStartUi.SetActive(false);
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.WaitForNextTrial;
        nowTrial = _nowTrial;
        MyScoreUI.text = "";
        //YourScoreUI.text = "";
        SetClientUI(false);
        TimeLimitObj.SetActive(true);
        TimeLimitObj.transform.position = TimeLimit_notBet.transform.position;
    }
    public void PhotonMoveToWaitForNextTrial(int _nowTrial)
    {
        _PracticeSet.MoveToWaitForNextTrial(_nowTrial);
    }
    public void MakeReadyHost()
    {
        _decideHostorClient.HostReady = true;
    }
    public void MakeReadyClient()
    {
        _decideHostorClient.ClientReady = true;
    }
    public void PhotonMakeReadyHost()
    {
        _PracticeSet.MakeReadyHost();
    }
    public void PhotonMakeReadyClient()
    {
        _PracticeSet.MakeReadyClient();
    }
    public void SetClientUI(bool setactive)
    {
        ClientUi.SetActive(setactive);
    }
    public void PhotonRestart()
    {
        _PracticeSet.SetFirstPressed(false);
        _PracticeSet.SetSecondPressed(false);
        _PracticeSet.SetThirdPressed(false);
        _PracticeSet.SetFourthPressed(false);
        ReUpdateParameter();
        _PracticeSet.Restart();
    }
    public void Restart()
    {
        TimeLimitObj_str.text = "";
        _SceneReloaderClient.SetActive(false);
        _blackJackRecorder.Trial += 1;
        FinishUI.text = "";
        nowTrial = 0;
        nowTime = 0;
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.BeforeStart;
        MyScoreUI.text = "";
        PhotonGameStartUI();
    }
    public void PressedReload()
    {
        _SceneReloaderHost.SetActive(false);
        if (MyConnectedNumber == 1)
        {
            _PracticeSet.SetFirstPressed(true);
        }
        else if (MyConnectedNumber == 2)
        {
            _PracticeSet.SetSecondPressed(true);
        }
        else if (MyConnectedNumber == 3)
        {
            _PracticeSet.SetThirdPressed(true);
        }
        else if (MyConnectedNumber == 4)
        {
            _PracticeSet.SetFourthPressed(true);
        }
        _SceneReloaderClient.SetActive(true);
    }
    private Vector3 MoveSuitWithinFrame(Vector3 _originpos)
    {
        return new Vector3(
            _originpos.x > 0 ? Mathf.Min(_originpos.x, _MovableArea.transform.position.x + _MovableArea.transform.localScale.x / 2) : Mathf.Max(_originpos.x, _MovableArea.transform.position.x - _MovableArea.transform.localScale.x / 2),
            _originpos.y > 0 ? Mathf.Min(_originpos.y, _MovableArea.transform.position.y + _MovableArea.transform.localScale.y / 2) : Mathf.Max(_originpos.y, _MovableArea.transform.position.y - _MovableArea.transform.localScale.y / 2),
            _originpos.z);
    }
}
