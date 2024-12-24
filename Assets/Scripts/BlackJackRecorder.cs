using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using TMPro;

public class BlackJackRecorder : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void DownloadFile(string filename, string content);

    [SerializeField] BlackJackManager _BlackJackManager;
    private PracticeSet _PracticeSet => _BlackJackManager._PracticeSet;
    //[SerializeField] CSVWriter _CSVWriter;
    public List<Vector3> ClubsPos { get; set; } = new List<Vector3>();
    public List<Vector3> SpadesPos { get; set; } = new List<Vector3>();
    public List<Vector3> HeartsPos { get; set; } = new List<Vector3>();
    public List<Vector3> DiamondsPos { get; set; } = new List<Vector3>();
    public List<int> MyNumberList { get; set; } = new List<int>();
    public List<int> YourNumberList { get; set; } = new List<int>();
    public List<int> ScoreList { get; set; } = new List<int>();
    private List<List<int>> MyCardsPracticeList => _PracticeSet.MyCardsPracticeList;
    private List<List<int>> YourCardsPracticeList => _PracticeSet.YourCardsPracticeList;
    private List<int> FieldCardsPracticeList => _PracticeSet.FieldCardsPracticeList;
    private int TrialAll => _PracticeSet.TrialAll;
    private List<float> MySelectedTime => _PracticeSet.MySelectedTime;
    private List<float> YourSelectedTime => _PracticeSet.YourSelectedTime;
    public int Trial = 1;
    private void FixedUpdate()
    {
        if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectCards)
        {
            ClubsPos.Add(_PracticeSet.Clubs);
            SpadesPos.Add(_PracticeSet.Spades);
            HeartsPos.Add(_PracticeSet.Hearts);
            DiamondsPos.Add(_PracticeSet.Diamonds);
        }


    }

    private string _Title;
    private void Start()
    {
        _Title = "Day" + System.DateTime.Now.Day.ToString() + "_" + System.DateTime.Now.Hour.ToString() + "h_" + System.DateTime.Now.Minute.ToString() + "min_" + System.DateTime.Now.Second.ToString() + "sec";
    }
    string WriteContent()
    {
        string Content = "";
        Content += "ClubsPos_x,ClubsPos_y,ClubsPos_z,SpadesPos_x,SpadesPos_y,SpadesPos_z,HeartsPos_x,HeartsPos_y,HeartsPos_z,DiamondsPos_x,DiamondsPos_y,DiamondsPos_z\n";
        for (int i = 0; i < ClubsPos.Count; i++)
        {
            Content += ClubsPos[i].x.ToString() + "," + ClubsPos[i].y.ToString() + "," + ClubsPos[i].z.ToString() + ",";
            Content += SpadesPos[i].x.ToString() + "," + SpadesPos[i].y.ToString() + "," + SpadesPos[i].z.ToString() + ",";
            Content += HeartsPos[i].x.ToString() + "," + HeartsPos[i].y.ToString() + "," + HeartsPos[i].z.ToString() + ",";
            Content += DiamondsPos[i].x.ToString() + "," + DiamondsPos[i].y.ToString() + "," + DiamondsPos[i].z.ToString() + "\n";
        }
        return Content;
    }
    public void ExportCsv(string wintype)
    {
        DownloadFile("result_blackjack_" + _Title + "_" + Trial.ToString() +"_"+ wintype + "win" +  ".csv", WriteContent());
    }

    /*public void WriteResult()
    {
        string Content = "";
        Content += "FieldNumber";
        for (int i = 0; i < MyCardsPracticeList[0].Count; i++) Content += ",MyCards" + (i + 1).ToString();
        for (int i = 0; i < YourCardsPracticeList[0].Count; i++) Content += ",YourCards" + (i + 1).ToString();
        Content += ",MyNumber,YourNumber,Score\n";
        for(int i = 0;i < TrialAll; i++)
        {
            Content += FieldCardsPracticeList[i].ToString();
            for (int j = 0; j < MyCardsPracticeList[i].Count; j++) Content += "," + MyCardsPracticeList[i][j].ToString();
            for (int j = 0; j < YourCardsPracticeList[i].Count; j++) Content += "," + YourCardsPracticeList[i][j].ToString();
            Content += "," + MyNumberList[i].ToString() + "," + YourNumberList[i].ToString() + "," + ScoreList[i].ToString() + "\n";
        }
        _CSVWriter.WriteCSV(Content);
    }*/
    public void Initialize()
    {
        ClubsPos = new List<Vector3>();
        SpadesPos = new List<Vector3>();
        HeartsPos = new List<Vector3>();
        DiamondsPos = new List<Vector3>();
    }
}
