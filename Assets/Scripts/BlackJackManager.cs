using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static BlackJackManager;
using Unity.VisualScripting;

public class BlackJackManager : MonoBehaviour
{
    [SerializeField] bool useSuit = false;
    [SerializeField] CardsList _cardslist;
    [SerializeField] int TimeLimit;
    [SerializeField] int ShowMyCardsTime = 5;
    [SerializeField] int ResultsTime = 5;
    [SerializeField] int WaitingTime = 3;
    [SerializeField] int BetTime = 4;
    [SerializeField] int NumberofSet = 10;
    [SerializeField] TextMeshProUGUI FinishUI;
    [SerializeField] BlackJackRecorder _blackJackRecorder;
    [SerializeField] TextMeshProUGUI MyScoreUI;
    [SerializeField] GameObject ClientUi;
    [SerializeField] GameObject BetUi;
    [SerializeField] GameObject CardListObject;
    [SerializeField] DecideHostorClient _decideHostorClient;
    [SerializeField] GameObject StartingUi;
    [SerializeField] GameObject StartingUi_button;
    [SerializeField] GameObject ShowTargetUI;
    [SerializeField] GameObject ShowTargetPos;
    [SerializeField] GameObject WaitforStartUi;
    [SerializeField] GameObject _SceneReloaderHost;
    [SerializeField] GameObject _SceneReloaderClient;
    [SerializeField] List<TextMeshProUGUI> BetUiChild;
    [SerializeField] GameObject TimeLimitObj;
    [SerializeField] GameObject TimeLimit_Bet;
    [SerializeField] GameObject TimeLimit_notBet;
    [SerializeField] GameObject AllTrialFinishedUI;
    [SerializeField] TextMeshProUGUI TimeLimitObj_str;
    [SerializeField] GameObject Clubs;
    [SerializeField] GameObject Spades;
    [SerializeField] GameObject Hearts;
    [SerializeField] GameObject Diamonds;
    [SerializeField] GameObject ClubsInitialpos;
    [SerializeField] GameObject SpadesInitialpos;
    [SerializeField] GameObject HeartsInitialpos;
    [SerializeField] GameObject DiamondsInitialpos;
    [SerializeField] GameObject StartLinePos;
    [SerializeField] GameObject GoalLinepos;
    [SerializeField] float AffordedDisntace; 
    //[SerializeField] TextMeshProUGUI YourScoreUI;
    public PracticeSet _PracticeSet { get; set; }
    private List<int> MaxScoreList = new List<int>();
    private List<int> ScoreList = new List<int>();
    private int NOTSELCETEDNUMBER = 101;
    public int MyConnectedNumber = 0;
    public Vector3 cursorPosition;
    Vector3 newcursorPosition;
    Vector3 DeltacursorPosition;
    float BlackDistance;
    float RedDistance;


    public enum HostorClient
    {
        Host = 0,
        Client = 1
    }
    public HostorClient _hostorclient { get; set; }
    private enum HowShowCard
    {
        KeyBoard,
        Time
    }
    [SerializeField] HowShowCard _HowShowCard;
    int nowTrial = 0;
    float nowTime = 0;
    private int Score = 0;
    public bool hasPracticeSet { get; set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        FinishUI.text = "";
        TimeLimitObj_str.text = "";
    }

    // Update is called once per frame
    void Update()
    {
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
                    if (_PracticeSet.FirstPressed && _PracticeSet.SecondPressed)
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
                nowTime += Time.deltaTime;
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
    public void InitializeCard()
    {
        _cardslist.InitializeCards();
    }
    public void ReInitializeCard()
    {
        _cardslist.ReInitializeCards();
    }
    void BlackJacking()
    {
        newcursorPosition = Input.mousePosition;
        DeltacursorPosition = newcursorPosition - cursorPosition;
        BlackDistance = Vector3.Magnitude(_PracticeSet.Clubs - _PracticeSet.Spades);
        RedDistance = Vector3.Magnitude(_PracticeSet.Clubs - _PracticeSet.Spades);

        if (MyConnectedNumber == 1)
        {
            if(_PracticeSet.Clubs.x < StartLinePos.transform.position.x && _PracticeSet.Clubs.x + DeltacursorPosition.x > StartLinePos.transform.position.x)
            {
                if(BlackDistance < AffordedDisntace)
                {
                    _PracticeSet.SetClubs(_PracticeSet.Clubs + DeltacursorPosition);
                }
                else
                {
                    _PracticeSet.SetClubs(_PracticeSet.Clubs + new Vector3(0, DeltacursorPosition.y, DeltacursorPosition.z));
                }
            }
            else
            {
                _PracticeSet.SetClubs(_PracticeSet.Clubs + DeltacursorPosition);
            }
        }
        else if (MyConnectedNumber == 2)
        {
            if (_PracticeSet.Spades.x < StartLinePos.transform.position.x && _PracticeSet.Spades.x + DeltacursorPosition.x > StartLinePos.transform.position.x)
            {
                if (BlackDistance < AffordedDisntace)
                {
                    _PracticeSet.SetSpades(_PracticeSet.Spades + DeltacursorPosition);
                }
                else
                {
                    _PracticeSet.SetSpades(_PracticeSet.Spades + new Vector3(0, DeltacursorPosition.y, DeltacursorPosition.z));
                }
            }
            else
            {
                _PracticeSet.SetSpades(_PracticeSet.Spades + DeltacursorPosition);
            }
        }
        else if (MyConnectedNumber == 3)
        {
            if (_PracticeSet.Hearts.x < StartLinePos.transform.position.x && _PracticeSet.Hearts.x + DeltacursorPosition.x > StartLinePos.transform.position.x)
            {
                if (BlackDistance < AffordedDisntace)
                {
                    _PracticeSet.SetHearts(_PracticeSet.Hearts + DeltacursorPosition);
                }
                else
                {
                    _PracticeSet.SetHearts(_PracticeSet.Hearts + new Vector3(0, DeltacursorPosition.y, DeltacursorPosition.z));
                }
            }
            else
            {
                _PracticeSet.SetHearts(_PracticeSet.Hearts + DeltacursorPosition);
            }
        }
        else if (MyConnectedNumber == 4)
        {
            if (_PracticeSet.Diamonds.x < StartLinePos.transform.position.x && _PracticeSet.Diamonds.x + DeltacursorPosition.x > StartLinePos.transform.position.x)
            {
                if (BlackDistance < AffordedDisntace)
                {
                    _PracticeSet.SetDiamonds(_PracticeSet.Diamonds + DeltacursorPosition);
                }
                else
                {
                    _PracticeSet.SetDiamonds(_PracticeSet.Diamonds + new Vector3(0, DeltacursorPosition.y, DeltacursorPosition.z));
                }
            }
            else
            {
                _PracticeSet.SetDiamonds(_PracticeSet.Diamonds + DeltacursorPosition);
            }
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
            }
            
            if (_PracticeSet.Hearts.x > GoalLinepos.transform.position.x || _PracticeSet.Diamonds.x > GoalLinepos.transform.position.x)
            {
                _PracticeSet.SetRedCleared(true);

            }
        }

        Clubs.transform.position = _PracticeSet.Clubs;
        Spades.transform.position = _PracticeSet.Spades;
        Hearts.transform.position = _PracticeSet.Hearts;
        Diamonds.transform.position = _PracticeSet.Diamonds;

    }
    void SelectBetting()
    {
        // �}�E�X�{�^�����N���b�N���ꂽ���m�F
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

            // ���C�L���X�g���g�p���ăI�u�W�F�N�g�����o
            if (hit && hit.collider.gameObject.CompareTag("Bet"))
            {
                TextMeshProUGUI textMesh;
                if (hit.collider.gameObject.TryGetComponent<TextMeshProUGUI>(out textMesh))
                {
                    string text = textMesh.text;

                    // 文字列から数字を抽出してint型に変換
                    int number;
                    if (int.TryParse(text, out number))
                    {
                        foreach (TextMeshProUGUI child in BetUiChild) child.color = Color.white;
                        textMesh.color = Color.yellow;
                        if (_hostorclient == HostorClient.Host)
                        {
                            _PracticeSet.SetMySelectedBet(number);
                        }
                        else if (_hostorclient == HostorClient.Client)
                        {
                            _PracticeSet.SetYourSelectedBet(number);
                        }
                    }
                    else
                    {
                        Debug.LogError("文字列に数字が含まれていません。");
                    }
                }
                else
                {
                    Debug.LogError("TextMeshProUGUIコンポーネントが見つかりませんでした。");
                }
            }
        }
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
        if (hostorClient == (int)HostorClient.Host)
        {
            _cardslist.MyCardsOpen();
        }
        else if (hostorClient == (int)HostorClient.Client)
        {
            _cardslist.YourCardsOpen();
        }
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
        _PracticeSet.MySelectedBet = 0;
        _PracticeSet.YourSelectedBet = 0;
        CardListObject.SetActive(false);
        BetUi.SetActive(true);
        foreach (TextMeshProUGUI child in BetUiChild) child.color = Color.white;
        nowTime = 0;
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.SelectBet;
        TimeLimitObj.transform.position = TimeLimit_Bet.transform.position;
    }
    public void MoveToSelectCards()
    {
        SetAllSuitsInitialPos();
        _cardslist.AllOpen();
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.SelectCards;
        TimeLimitObj.transform.position = TimeLimit_notBet.transform.position;
    }
    public void PhotonMoveToSelectCards()
    {
        _PracticeSet.MoveToSelectCards();
    }
    public void MoveToShowResult()
    {
        CardListObject.SetActive(true);
        BetUi.SetActive(false);
        if (_PracticeSet.MySelectedCard != NOTSELCETEDNUMBER) _cardslist.MyCardsList[_PracticeSet.MySelectedCard].Clicked();
        if (_PracticeSet.YourSelectedCard != NOTSELCETEDNUMBER) _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].Clicked();
        TimeLimitObj.transform.position = TimeLimit_notBet.transform.position;

        /*foreach (var card in _cardslist.MyCardsList_opponent)
        {
            if (card.Number == _PracticeSet.YourSelectedCard.Number) card.Clicked();
        }
        foreach (var card in _cardslist.YourCardsList_opponent)
        {
            if (card.Number == _PracticeSet.MySelectedCard.Number) card.Clicked();
        }*/
        Score = CalculateResult();
        _blackJackRecorder.RecordResult((_PracticeSet.MySelectedCard == NOTSELCETEDNUMBER) ? 0 : _cardslist.MyCardsList[_PracticeSet.MySelectedCard].Number, (_PracticeSet.YourSelectedCard == NOTSELCETEDNUMBER) ? 0 : _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].Number, (useSuit) ? CalculateSuitScore() : Score, _PracticeSet.MySelectedBet, _PracticeSet.YourSelectedBet);
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.ShowResult;
        MyScoreUI.text = "Score:" + ((useSuit) ? CalculateScorewithSuit() : Score.ToString());
        ScoreList.Add(Score);
        if (useSuit)
        {
            RecordMaxSuitScore();
        }
        else
        {
            RecordMaxScore();
        }
        //YourScoreUI.text = Score.ToString();
        nowTime = 0;
        nowTrial += 1;
        if (nowTrial == _PracticeSet.TrialAll)
        {
            _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.Finished;
            FinishUI.text = "Finished! \n ScoreAll:" + ReturnSum(ScoreList).ToString() + "/" + ReturnSum(MaxScoreList).ToString() + "\n" + "Trial: " + _blackJackRecorder.Trial.ToString() + "/" + NumberofSet.ToString();
            //_blackJackRecorder.WriteResult();
            _blackJackRecorder.ExportCsv();
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
    }
    public void PhotonMoveToShowResult()
    {
        _PracticeSet.MoveToShowResult();
    }
    public void MoveToWaitForNextTrial(int _nowTrial)
    {
        WaitforStartUi.SetActive(false);
        _cardslist.AllClose();
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.WaitForNextTrial;
        nowTrial = _nowTrial;
        _cardslist.SetCards(_nowTrial);
        MyScoreUI.text = "";
        //YourScoreUI.text = "";
        _PracticeSet.MySelectedCard = NOTSELCETEDNUMBER;
        _PracticeSet.YourSelectedCard = NOTSELCETEDNUMBER;
        SetClientUI(false);
        TimeLimitObj.transform.position = TimeLimit_notBet.transform.position;
    }
    public void PhotonMoveToWaitForNextTrial(int _nowTrial)
    {
        _PracticeSet.MoveToWaitForNextTrial(_nowTrial);
    }
    private int CalculateResult()
    {
        return (_PracticeSet.MySelectedCard == NOTSELCETEDNUMBER || _PracticeSet.YourSelectedCard == NOTSELCETEDNUMBER) ? 0 : (_cardslist.MyCardsList[_PracticeSet.MySelectedCard].Number + _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].Number + _PracticeSet.FieldCardsPracticeList[nowTrial] > 21) ? 0 : _cardslist.MyCardsList[_PracticeSet.MySelectedCard].Number + _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].Number + _PracticeSet.FieldCardsPracticeList[nowTrial];
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
    private string CalculateScorewithSuit()
    {
        string result = Score.ToString();
        if (_cardslist.MyCardsList[_PracticeSet.MySelectedCard].suit.GetColor() == _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].suit.GetColor())
        {
            if (_cardslist.MyCardsList[_PracticeSet.MySelectedCard].suit == _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].suit)
            {
                result += " x 1.2 = " + Mathf.Ceil(Score * 1.2f).ToString();
            }
            else
            {
                result += " x 1.1 = " + Mathf.Ceil(Score * 1.1f).ToString();
            }
        }
        else
        {
            result += " x 1.0 = " + Score.ToString();
        }
        return result;
    }
    private int CalculateSuitScore()
    {
        if (_cardslist.MyCardsList[_PracticeSet.MySelectedCard].suit.GetColor() == _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].suit.GetColor())
        {
            if (_cardslist.MyCardsList[_PracticeSet.MySelectedCard].suit == _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].suit)
            {
                return (int)Mathf.Ceil(Score * 1.2f);
            }
            else
            {
                return (int)Mathf.Ceil(Score * 1.1f);
            }
        }
        else
        {
            return Score;
        }
    }
    private void RecordMaxScore()
    {
        int MaxScore = 0;
        for (int i = 0; i < _cardslist.MyCardsList.Count; i++)
        {
            for (int j = 0; j < _cardslist.YourCardsList.Count; j++)
            {
                int _score = (_cardslist.MyCardsList[i].Number + _cardslist.YourCardsList[j].Number + _PracticeSet.FieldCardsPracticeList[nowTrial] > 21) ? 0 : _cardslist.MyCardsList[i].Number + _cardslist.YourCardsList[j].Number + _PracticeSet.FieldCardsPracticeList[nowTrial];
                if (MaxScore < _score) MaxScore = _score;
            }
        }
        MaxScoreList.Add(MaxScore);
    }
    private void RecordMaxSuitScore()
    {
        int MaxScore = 0;
        for (int i = 0; i < _cardslist.MyCardsList.Count; i++)
        {
            for (int j = 0; j < _cardslist.YourCardsList.Count; j++)
            {
                int _score = (_cardslist.MyCardsList[i].Number + _cardslist.YourCardsList[j].Number + _PracticeSet.FieldCardsPracticeList[nowTrial] > 21) ? 0 : _cardslist.MyCardsList[i].Number + _cardslist.YourCardsList[j].Number + _PracticeSet.FieldCardsPracticeList[nowTrial];
                if (_cardslist.MyCardsList[i].suit.GetColor() == _cardslist.YourCardsList[j].suit.GetColor())
                {
                    if (_cardslist.MyCardsList[i].suit == _cardslist.YourCardsList[j].suit)
                    {
                        _score = (int)Mathf.Ceil(_score * 1.2f);
                    }
                    else
                    {
                        _score = (int)Mathf.Ceil(Score * 1.1f);
                    }
                }
                if (MaxScore < _score) MaxScore = _score;
            }
        }
        MaxScoreList.Add(MaxScore);
    }
    private int ReturnSum(List<int> _list)
    {
        int result = 0;
        foreach (var element in _list)
        {
            result += element;
        }
        return result;
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
        MaxScoreList = new List<int>();
        _SceneReloaderClient.SetActive(false);
        _blackJackRecorder.Trial += 1;
        FinishUI.text = "";
        _cardslist.AllClose();
        ScoreList = new List<int>();
        nowTrial = 0;
        nowTime = 0;
        _blackJackRecorder.Initialize();
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
}
