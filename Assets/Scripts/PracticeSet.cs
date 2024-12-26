using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Text.RegularExpressions;
using System.Linq;


public class PracticeSet : MonoBehaviourPunCallbacks
{
    public int playerindex;
    BlackJackManager _BlackJackManager;
    private PhotonView _PhotonView;
    public int MySelectedCard { get; set; }
    public int YourSelectedCard { get; set; }
    public List<float> MySelectedTime { get; set; }
    public List<float> YourSelectedTime { get; set; }
    public float TimeLeft { get; set; } = 0;
    public bool FirstPressed { get; set; } = false;
    public bool SecondPressed { get; set; } = false;
    public bool ThirdPressed { get; set; } = false;
    public bool FourthPressed { get; set; } = false;
    public bool BlackCleared { get; set; } = false;
    public bool RedCleared { get; set; } = false;

    public Vector3 Clubs;
    public Vector3 Spades;
    public Vector3 Hearts;
    public Vector3 Diamonds;
    public void SetClubs(Vector3 Clubpos)
    {
        Clubs = Clubpos;
        _PhotonView.RPC("UpdateClubsOnAllClients", RpcTarget.Others, Clubpos.x, Clubpos.y, Clubpos.z);
    }
    [PunRPC]
    void UpdateClubsOnAllClients(float Clubpos_x, float Clubpos_y, float Clubpos_z)
    {
        Clubs = new Vector3(Clubpos_x, Clubpos_y, Clubpos_z);
    }
    public void SetSpades(Vector3 Spadepos)
    {
        Spades = Spadepos;
        _PhotonView.RPC("UpdateSpadesOnAllClients", RpcTarget.Others, Spadepos.x, Spadepos.y, Spadepos.z);
    }
    [PunRPC]
    void UpdateSpadesOnAllClients(float Spadepos_x, float Spadepos_y, float Spadepos_z)
    {
        Spades = new Vector3(Spadepos_x, Spadepos_y, Spadepos_z);
    }
    public void SetHearts(Vector3 Heartpos)
    {
        Hearts = Heartpos;
        _PhotonView.RPC("UpdateHeartsOnAllClients", RpcTarget.Others, Heartpos.x, Heartpos.y, Heartpos.z);
    }
    [PunRPC]
    void UpdateHeartsOnAllClients(float Heartpos_x, float Heartpos_y, float Heartpos_z)
    {
        Hearts = new Vector3(Heartpos_x, Heartpos_y, Heartpos_z);
    }
    public void SetDiamonds(Vector3 Diamondpos)
    {
        Diamonds = Diamondpos;
        _PhotonView.RPC("UpdateDiamondsOnAllClients", RpcTarget.Others, Diamondpos.x, Diamondpos.y, Diamondpos.z);
    }
    [PunRPC]
    void UpdateDiamondsOnAllClients(float Diamondpos_x, float Diamondpos_y, float Diamondpos_z)
    {
        Diamonds = new Vector3(Diamondpos_x, Diamondpos_y, Diamondpos_z);
    }

    public void SetPlayerIndex(int _playerindex)
    {
        playerindex = _playerindex;
        _PhotonView.RPC("UpdateSetPlayerIndexOnAllClients", RpcTarget.Others, _playerindex);
    }
    [PunRPC]
    void UpdateSetPlayerIndexOnAllClients(int _playerindex)
    {
        playerindex = _playerindex;
    }
    public void SetFirstPressed(bool _FirstPressed)
    {
        FirstPressed = _FirstPressed;
        _PhotonView.RPC("UpdateFirstPressedOnAllClients", RpcTarget.Others, _FirstPressed);
    }
    [PunRPC]
    void UpdateFirstPressedOnAllClients(bool _FirstPressed)
    {
        FirstPressed = _FirstPressed;
    }
    public void SetBlackCleared(bool _BlackCleared)
    {
        BlackCleared = _BlackCleared;
        _PhotonView.RPC("UpdateBlackClearedOnAllClients", RpcTarget.Others, _BlackCleared);
    }
    [PunRPC]
    void UpdateBlackClearedOnAllClients(bool _BlackCleared)
    {
        BlackCleared = _BlackCleared;
    }
    public void SetRedCleared(bool _RedCleared)
    {
        RedCleared = _RedCleared;
        _PhotonView.RPC("UpdateRedClearedOnAllClients", RpcTarget.Others, _RedCleared);
    }
    [PunRPC]
    void UpdateRedClearedOnAllClients(bool _RedCleared)
    {
        RedCleared = _RedCleared;
    }
    public void SetSecondPressed(bool _SecondPressed)
    {
        SecondPressed = _SecondPressed;
        _PhotonView.RPC("UpdateSecondPressedOnAllClients", RpcTarget.Others, _SecondPressed);
    }
    [PunRPC]
    void UpdateSecondPressedOnAllClients(bool _SecondPressed)
    {
        SecondPressed = _SecondPressed;
    }
    public void SetThirdPressed(bool _ThirdPressed)
    {
        ThirdPressed = _ThirdPressed;
        _PhotonView.RPC("UpdateThirdPressedOnAllClients", RpcTarget.Others, _ThirdPressed);
    }
    [PunRPC]
    void UpdateThirdPressedOnAllClients(bool _ThirdPressed)
    {
        ThirdPressed = _ThirdPressed;
    }
    public void SetFourthPressed(bool _FourthPressed)
    {
        FourthPressed = _FourthPressed;
        _PhotonView.RPC("UpdateFourthPressedOnAllClients", RpcTarget.Others, _FourthPressed);
    }
    [PunRPC]
    void UpdateFourthPressedOnAllClients(bool _FourthPressed)
    {
        FourthPressed = _FourthPressed;
    }
    public void SetTimeLeft(float _timeleft)
    {
        TimeLeft = _timeleft;
        _PhotonView.RPC("UpdateTimeLeftOnAllClients", RpcTarget.Others, _timeleft);
    }
    [PunRPC]
    void UpdateTimeLeftOnAllClients(float _timeleft)
    {
        // ここでカードデータを再構築
        TimeLeft = _timeleft;
    }
    public void SetMySelectedTime(float time, int trial)
    {
        MySelectedTime[trial] = time;
        _PhotonView.RPC("UpdateMySelectedTimeOnAllClients", RpcTarget.Others, time, trial);
    }
    [PunRPC]
    void UpdateMySelectedTimeOnAllClients(float time, int trial)
    {
        // ここでカードデータを再構築
        MySelectedTime[trial] = time;
    }
    public void SetYourSelectedTime(float time, int trial)
    {
        YourSelectedTime[trial] = time;
        _PhotonView.RPC("UpdateYourSelectedTimeOnAllClients", RpcTarget.Others, time, trial);
    }
    [PunRPC]
    void UpdateYourSelectedTimeOnAllClients(float time, int trial)
    {
        // ここでカードデータを再構築
        YourSelectedTime[trial] = time;
    }
    public void SetMySelectedCard(int card)
    {
        MySelectedCard = card;
        _PhotonView.RPC("UpdateMySelectedCardOnAllClients", RpcTarget.Others, card);
    }
    [PunRPC]
    void UpdateMySelectedCardOnAllClients(int _Number)
    {
        // ここでカードデータを再構築
        MySelectedCard = _Number;
    }
    public void SetYourSelectedCard(int card)
    {
        YourSelectedCard = card;
        _PhotonView.RPC("UpdateYourSelectedCardOnAllClients", RpcTarget.Others, card);
    }
    [PunRPC]
    void UpdateYourSelectedCardOnAllClients(int _Number)
    {
        // ここでカードデータを再構築
        YourSelectedCard = _Number;
    }
    public List<List<int>> MyCardsPracticeList { get; set; } = new List<List<int>>();
    public List<List<int>> YourCardsPracticeList { get; set; } = new List<List<int>>();
    public List<int> FieldCardsPracticeList /*{ get; set; }*/ = new List<int>();
    public List<int> SpawnObj_x { get; set; } = new List<int>();
    public List<int> SpawnObj_y { get; set; } = new List<int>();
    public List<int> SpawnObjsize_x { get; set; } = new List<int>();
    public List<int> SpawnObjsize_y { get; set; } = new List<int>();
    public void SetSpawnObjsize_x(List<int> _SpawnObjsize_x)
    {
        List<int> temp = _SpawnObjsize_x;
        SpawnObjsize_x = temp;
        _PhotonView.RPC("UpdateSpawnObjsize_xOnAllClients", RpcTarget.Others, SerializeFieldCard(_SpawnObjsize_x));
    }
    [PunRPC]
    void UpdateSpawnObjsize_xOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        SpawnObjsize_x = DeserializeFieldCard(serializeCards);
    }
    public void SetSpawnObjsize_y(List<int> _SpawnObjsize_y)
    {
        List<int> temp = _SpawnObjsize_y;
        SpawnObjsize_y = temp;
        _PhotonView.RPC("UpdateSpawnObjsize_yOnAllClients", RpcTarget.Others, SerializeFieldCard(_SpawnObjsize_y));
    }
    [PunRPC]
    void UpdateSpawnObjsize_yOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        SpawnObjsize_y = DeserializeFieldCard(serializeCards);
    }
    public void SetSpawnObj_x(List<int> _SpawnObj_x)
    {
        List<int> temp = _SpawnObj_x;
        SpawnObj_x = temp;
        _PhotonView.RPC("UpdateSpawnObj_xOnAllClients", RpcTarget.Others, SerializeFieldCard(_SpawnObj_x));
    }
    [PunRPC]
    void UpdateSpawnObj_xOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        SpawnObj_x = DeserializeFieldCard(serializeCards);
    }
    public void SetSpawnObj_y(List<int> _SpawnObj_y)
    {
        List<int> temp = _SpawnObj_y;
        SpawnObj_y = temp;
        _PhotonView.RPC("UpdateSpawnObj_yOnAllClients", RpcTarget.Others, SerializeFieldCard(_SpawnObj_y));
    }
    [PunRPC]
    void UpdateSpawnObj_yOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        SpawnObj_y = DeserializeFieldCard(serializeCards);
    }
    public void SetMyCardsPracticeList(List<List<int>> _MyCardsPracticeList)
    {
        List<List<int>> temp = _MyCardsPracticeList;
        MyCardsPracticeList = temp;
        _PhotonView.RPC("UpdateMyCardsPracticeListOnAllClients", RpcTarget.Others, SerializeCardList(_MyCardsPracticeList));
    }
    [PunRPC]
    void UpdateMyCardsPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        MyCardsPracticeList = DeserializeCardList(serializeCards);
    }
    public void SetYourCardsPracticeList(List<List<int>> _YourCardsPracticeList)
    {
        List<List<int>> temp = _YourCardsPracticeList;
        YourCardsPracticeList = temp;
        _PhotonView.RPC("UpdateYourCardsPracticeListOnAllClients", RpcTarget.Others, SerializeCardList(_YourCardsPracticeList));
    }
    [PunRPC]
    void UpdateYourCardsPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        YourCardsPracticeList = DeserializeCardList(serializeCards);
    }
    public void SetFieldCardsList(List<int> _FieldCardsPracticeList)
    {
        List<int> temp = FieldCardsPracticeList;
        FieldCardsPracticeList = temp;
        _PhotonView.RPC("UpdateFieldCardsPracticeListOnAllClients", RpcTarget.Others, SerializeFieldCard(_FieldCardsPracticeList));
    }
    [PunRPC]
    void UpdateFieldCardsPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        FieldCardsPracticeList = DeserializeFieldCard(serializeCards);
    }

    private string SerializeCardList(List<List<int>> cards)
    {

        string cards_json = "[";
        for (int i = 0; i < cards.Count; i++)
        {
            cards_json += JsonHelper.ToJson(cards[i]) + ",";
        }
        cards_json = cards_json.Remove(cards_json.Length - 1);
        cards_json += "]";
        return cards_json;
    }

    private List<List<int>> DeserializeCardList(string json)
    {
        Regex regex = new Regex(@"\d+");

        List<int> numbers = new List<int>();
        foreach (Match match in regex.Matches(json))
        {
            numbers.Add(int.Parse(match.Value));
        }
        List<List<int>> cardList = new List<List<int>>();
        // JSON 文字列を int[] の配列に変換
        for (int i = 0; i < NumberofSet; i++)
        {
            List<int> Element = new List<int>();
            for (int j = 0; j < NumberofCards; j++)
            {
                Element.Add(numbers[i * NumberofCards + j]);
            }
            cardList.Add(Element);
        }
        return cardList;
    }

    private string SerializeFieldCard(List<int> cards)
    {
        return JsonHelper.ToJson(cards);
    }

    private List<int> DeserializeFieldCard(string serializedCards)
    {
        Regex regex = new Regex(@"\d+");

        List<int> numbers = new List<int>();
        foreach (Match match in regex.Matches(serializedCards))
        {
            numbers.Add(int.Parse(match.Value));
        }
        return numbers;
    }
    private string SerializeObj(List<float> cards)
    {
        return JsonHelper.ToJson(cards);
    }

    private List<float> DeserializeObj(string serializedCards)
    {
        Regex regex = new Regex(@"\d+");

        List<float> numbers = new List<float>();
        foreach (Match match in regex.Matches(serializedCards))
        {
            numbers.Add(float.Parse(match.Value));
        }
        return numbers;
    }
    // ヘルパークラスを定義
[System.Serializable]
public class FloatListWrapper
{
    public List<float> floats;
}
    [System.Serializable]
    private class SerializationWrapper<T>
    {
        public T data;

        public SerializationWrapper(T data)
        {
            this.data = data;
        }
    }

    public List<List<int>> MyCardsSuitPracticeList = new List<List<int>>();
    public List<List<int>> YourCardsSuitPracticeList = new List<List<int>>();
    public List<int> FieldCardsSuitPracticeList = new List<int>();
    public void SetMyCardsSuitPracticeList(List<List<int>> _MyCardsSuitPracticeList)
    {
        List<List<int>> temp = _MyCardsSuitPracticeList;
        MyCardsSuitPracticeList = temp;
        _PhotonView.RPC("UpdateMyCardsSuitPracticeListOnAllClients", RpcTarget.Others, SerializeCardList(_MyCardsSuitPracticeList));
    }
    [PunRPC]
    void UpdateMyCardsSuitPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        MyCardsSuitPracticeList = DeserializeCardList(serializeCards);
    }
    public void SetYourCardsSuitPracticeList(List<List<int>> _YourCardsSuitPracticeList)
    {
        List<List<int>> temp = _YourCardsSuitPracticeList;
        YourCardsSuitPracticeList = temp;
        _PhotonView.RPC("UpdateYourCardsSuitPracticeListOnAllClients", RpcTarget.Others, SerializeCardList(_YourCardsSuitPracticeList));
    }
    [PunRPC]
    void UpdateYourCardsSuitPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        YourCardsSuitPracticeList = DeserializeCardList(serializeCards);
    }
    public void SetFieldCardsSuitList(List<int> _FieldCardsSuitPracticeList)
    {
        List<int> temp = FieldCardsSuitPracticeList;
        FieldCardsSuitPracticeList = temp;
        _PhotonView.RPC("UpdateFieldCardsSuitPracticeListOnAllClients", RpcTarget.Others, SerializeFieldCard(_FieldCardsSuitPracticeList));
    }
    [PunRPC]
    void UpdateFieldCardsSuitPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        FieldCardsSuitPracticeList = DeserializeFieldCard(serializeCards);
    }

    public enum BlackJackStateList
    {
        BeforeStart,
        WaitForNextTrial,
        ShowMyCards,
        SelectCards,
        SelectBet,
        ShowResult,
        Finished,
    }
    public BlackJackStateList BlackJackState = BlackJackStateList.BeforeStart;

    public void SetBlackJackState(BlackJackStateList _BlackJackState)
    {
        BlackJackState = _BlackJackState;
        _PhotonView.RPC("UpdateBlackJackStateListOnAllClients", RpcTarget.Others, SerializeBlackJackState(_BlackJackState));
    }
    [PunRPC]
    void UpdateBlackJackStateListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        BlackJackState = DeserializeBlackJackState(serializeCards);
    }

    private string SerializeBlackJackState(BlackJackStateList _BlackJackState)
    {
        return JsonUtility.ToJson(new SerializationWrapper<BlackJackStateList>(_BlackJackState));
    }

    private BlackJackStateList DeserializeBlackJackState(string serializedCards)
    {
        return JsonUtility.FromJson<SerializationWrapper<BlackJackStateList>>(serializedCards).data;
    }

    public int TrialAll;
    public int NumberofCards = 5;


    public int NumberofSet { get; set; } = 5;

    List<int> MyCards;
    List<int> YourCards;
    private static int FieldCardsSuit = 0;
    private void Start()
    {
        _PhotonView = GetComponent<PhotonView>();
        _BlackJackManager = GameObject.FindWithTag("Manager").GetComponent<BlackJackManager>();
    }
    public void UpdateParameter(int max_x, int max_y, int minsize_x, int maxsize_x, int minsize_y, int maxsize_y, int NumberofObj)
    {
        List<int> SpawnObj_x_temp = new List<int>();
        List<int> SpawnObj_y_temp = new List<int>();
        List<int> SpawnObjsize_x_temp = new List<int>();
        List<int> SpawnObjsize_y_temp = new List<int>();
        for (int i = 0; i < NumberofObj * 2; i++)
        {
            SpawnObj_x_temp.Add(Random.Range(-max_x, max_x) + 10000);
            SpawnObj_y_temp.Add(Random.Range(-max_y, max_y) + 10000);
            SpawnObjsize_x_temp.Add(Random.Range(minsize_x, maxsize_x));
            SpawnObjsize_y_temp.Add(Random.Range(minsize_y, maxsize_y));
        }

        SetSpawnObj_x(SpawnObj_x_temp);
        SetSpawnObj_y(SpawnObj_y_temp);
        SetSpawnObjsize_x(SpawnObjsize_x_temp);
        SetSpawnObjsize_y(SpawnObjsize_y_temp);
        ClearObstacles();

    }
    public void ReUpdateParameter(int max_x, int max_y, int minsize_x, int maxsize_x, int minsize_y, int maxsize_y, int NumberofObj) 
    {
        List<int> SpawnObj_x_temp = new List<int>();
        List<int> SpawnObj_y_temp = new List<int>();
        List<int> SpawnObjsize_x_temp = new List<int>();
        List<int> SpawnObjsize_y_temp = new List<int>();
        for (int i = 0; i < NumberofObj * 2; i++)
        {
            SpawnObj_x_temp.Add(Random.Range(-max_x, max_x) + 10000);
            SpawnObj_y_temp.Add(Random.Range(-max_y, max_y) + 10000);
            SpawnObjsize_x_temp.Add(Random.Range(minsize_x, maxsize_x));
            SpawnObjsize_y_temp.Add(Random.Range(minsize_y, maxsize_y));
        }

        SetSpawnObj_x(SpawnObj_x_temp);
        SetSpawnObj_y(SpawnObj_y_temp);
        SetSpawnObjsize_x(SpawnObjsize_x_temp);
        SetSpawnObjsize_y(SpawnObjsize_y_temp);
        ClearObstacles();

    }

    public void ClearObstacles()
    {
        _BlackJackManager.ClearObstacles();
        _PhotonView.RPC("RPCClearObstacles", RpcTarget.Others);
    }
    [PunRPC]
    void RPCClearObstacles()
    {
        // ここでカードデータを再構築
        _BlackJackManager.ClearObstacles();
    }
    void ShuffleCards()
    {
        for (int i = 0; i < MyCards.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, MyCards.Count);
            int temp = MyCards[i];
            MyCards[i] = MyCards[randomIndex];
            MyCards[randomIndex] = temp;
        }
        for (int i = 0; i < YourCards.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, YourCards.Count);
            int temp = YourCards[i];
            YourCards[i] = YourCards[randomIndex];
            YourCards[randomIndex] = temp;
        }
    }

    public void MoveToWaitForNextTrial(int _nowTrial)
    {
        _BlackJackManager.MoveToWaitForNextTrial(_nowTrial);
        _PhotonView.RPC("RPCMoveToWaitForNextTrial", RpcTarget.Others, _nowTrial);
    }
    [PunRPC]
    void RPCMoveToWaitForNextTrial(int _nowTrial)
    {
        // ここでカードデータを再構築
        _BlackJackManager.MoveToWaitForNextTrial(_nowTrial);
    }

    public void MoveToShowMyCards(int hostorClient)
    {
        _BlackJackManager.MoveToShowMyCards(0);
        _PhotonView.RPC("RPCMoveToShowMyCards", RpcTarget.Others);
    }
    [PunRPC]
    void RPCMoveToShowMyCards()
    {
        // ここでカードデータを再構築
        _BlackJackManager.MoveToShowMyCards(1);
    }

    public void MoveToSelectCards()
    {
        _BlackJackManager.MoveToSelectCards();
        _PhotonView.RPC("RPCMoveToSelectCards", RpcTarget.Others);
    }
    [PunRPC]
    void RPCMoveToSelectCards()
    {
        // ここでカードデータを再構築
        _BlackJackManager.MoveToSelectCards();
    }
    public void MoveToSelectBet()
    {
        _BlackJackManager.MoveToSelectBet();
        _PhotonView.RPC("RPCMoveToSelectBet", RpcTarget.Others);
    }
    [PunRPC]
    void RPCMoveToSelectBet()
    {
        // ここでカードデータを再構築
        _BlackJackManager.MoveToSelectBet();
    }
    public void MoveToShowResult()
    {
        _BlackJackManager.MoveToShowResult();
        _PhotonView.RPC("RPCMoveToShowResult", RpcTarget.Others);
    }
    [PunRPC]
    void RPCMoveToShowResult()
    {
        // ここでカードデータを再構築
        _BlackJackManager.MoveToShowResult();
    }
    public void MakeReadyHost()
    {
        _BlackJackManager.MakeReadyHost();
        _PhotonView.RPC("RPCMakeReadyHost", RpcTarget.Others);
    }
    [PunRPC]
    void RPCMakeReadyHost()
    {
        // ここでカードデータを再構築
        _BlackJackManager.MakeReadyHost();
    }
    public void MakeReadyClient()
    {
        _BlackJackManager.MakeReadyClient();
        _PhotonView.RPC("RPCMakeReadyClient", RpcTarget.Others);
    }
    [PunRPC]
    void RPCMakeReadyClient()
    {
        // ここでカードデータを再構築
        _BlackJackManager.MakeReadyClient();
    }
    public void Restart()
    {
        _BlackJackManager.Restart();
        _PhotonView.RPC("RPCRestart", RpcTarget.Others);
    }
    [PunRPC]
    void RPCRestart()
    {
        // ここでカードデータを再構築
        _BlackJackManager.Restart();
    }
    public void GameStartUi()
    {
        _BlackJackManager.GameStartUI();
        _PhotonView.RPC("RPCGameStartUi", RpcTarget.Others);
    }
    [PunRPC]
    void RPCGameStartUi()
    {
        // ここでカードデータを再構築
        _BlackJackManager.GameStartUI();
    }
    List<int> GenerateRandomList(int min, int max)
    {
        List<int> result = new List<int>();
        for (int i = min; i <= max; i++)
        {
            result.Add(i);
        }

        // シャッフル
        int n = result.Count;
        System.Random rng = new System.Random();
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int value = result[k];
            result[k] = result[n];
            result[n] = value;
        }

        return result;
    }
    public void MoveAllSuitsInitialPos()
    {
        _BlackJackManager.MoveAllSuitsInitialPos();
        _PhotonView.RPC("RPCMoveAllSuitsInitialPos", RpcTarget.Others);
    }
    [PunRPC]
    void RPCMoveAllSuitsInitialPos()
    {
        // ここでカードデータを再構築
        _BlackJackManager.MoveAllSuitsInitialPos();
    }

}
