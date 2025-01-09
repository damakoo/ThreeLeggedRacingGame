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
    public List<bool> FirstPressing { get; set; } = new List<bool>();
    public List<bool> SecondPressing { get; set; } = new List<bool>();
    public List<bool> ThirdPressing { get; set; } = new List<bool>();
    public List<bool> FourthPressing { get; set; } = new List<bool>();
    public List<float> FirstPressingTime { get; set; } = new List<float>();
    public List<float> SecondPressingTime { get; set; } = new List<float>();
    public List<float> ThirdPressingTime { get; set; } = new List<float>();
    public List<float> FourthPressingTime { get; set; } = new List<float>();
    public List<bool> BlackGoaled { get; set; } = new List<bool>();
    public List<bool> RedGoaled { get; set; } = new List<bool>();
    private int TrialAll => _PracticeSet.TrialAll;
    float _Time = 0;
    public List<float> Times = new List<float>();
    public int Trial = 1;
    private void FixedUpdate()
    {
        if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectCards)
        {
            _Time += Time.fixedDeltaTime;
            Times.Add(_Time);
            ClubsPos.Add(_BlackJackManager.Clubs.transform.position);
            SpadesPos.Add(_BlackJackManager.Spades.transform.position);
            HeartsPos.Add(_BlackJackManager.Hearts.transform.position);
            DiamondsPos.Add(_BlackJackManager.Diamonds.transform.position);
            FirstPressing.Add(_PracticeSet.FirstPressing);
            SecondPressing.Add(_PracticeSet.SecondPressing);
            ThirdPressing.Add(_PracticeSet.ThirdPressing);
            FourthPressing.Add(_PracticeSet.FourthPressing);
            FirstPressingTime.Add(_PracticeSet.FirstPressedTime);
            SecondPressingTime.Add(_PracticeSet.SecondPressedTime);
            ThirdPressingTime.Add(_PracticeSet.ThirdPressedTime);
            FourthPressingTime.Add(_PracticeSet.FourthPressedTime);
            BlackGoaled.Add(_PracticeSet.BlackCleared);
            RedGoaled.Add(_PracticeSet.RedCleared);
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
        Content += "ClubsPos_x,ClubsPos_y,ClubsPos_z,SpadesPos_x,SpadesPos_y,SpadesPos_z,HeartsPos_x,HeartsPos_y,HeartsPos_z,DiamondsPos_x,DiamondsPos_y,DiamondsPos_z,FirstPressing,SecondPressing,ThirdPressing,FourthPressing,FirstPressedTime,SecondPressedTime,ThirdPressedTime,FourthPressedTime,Time,BlackGoaled,RedGoaled,hasobstacle,ClubHeartholepos1x,ClubHeartholepos2x,ClubHeartholepos3x,ClubHeartholesize1x,ClubHeartholesize2x,ClubHeartholesize3x,ClubHeartholepos11y,ClubHeartholepos12y,ClubHeartholepos21y,ClubHeartholepos22y,ClubHeartholepos31y,ClubHeartholepos32y,ClubHeartholesize11y,ClubHeartholesize12y,ClubHeartholesize21y,ClubHeartholesize22y,ClubHeartholesize31y,ClubHeartholesize32y,SpadeDiamondholepos1x,SpadeDiamondholepos2x,SpadeDiamondholepos3x,SpadeDiamondholesize1x,SpadeDiamondholesize2x,SpadeDiamondholesize3x,SpadeDiamondholepos11y,SpadeDiamondholepos12y,SpadeDiamondholepos21y,SpadeDiamondholepos22y,SpadeDiamondholepos31y,SpadeDiamondholepos32y,SpadeDiamondholesize11y,SpadeDiamondholesize12y,SpadeDiamondholesize21y,SpadeDiamondholesize22y,SpadeDiamondholesize31y,SpadeDiamondholesize32y\n";
        for (int i = 0; i < ClubsPos.Count; i++)
        {
            Content += ClubsPos[i].x.ToString() + "," + ClubsPos[i].y.ToString() + "," + ClubsPos[i].z.ToString() + ","
            + SpadesPos[i].x.ToString() + "," + SpadesPos[i].y.ToString() + "," + SpadesPos[i].z.ToString() + ","
            + HeartsPos[i].x.ToString() + "," + HeartsPos[i].y.ToString() + "," + HeartsPos[i].z.ToString() + ","
            + DiamondsPos[i].x.ToString() + "," + DiamondsPos[i].y.ToString() + "," + DiamondsPos[i].z.ToString() + ","
            + FirstPressing[i].ToString() + "," + SecondPressing[i].ToString() + "," + ThirdPressing[i].ToString() + "," + FourthPressing[i].ToString() + ","
            + FirstPressingTime[i].ToString() + "," + SecondPressingTime[i].ToString() + "," + ThirdPressingTime[i].ToString() + "," + FourthPressingTime[i].ToString() + "," + Times[i].ToString() + ","
            + BlackGoaled[i].ToString() + "," + RedGoaled.ToString() + "," + _BlackJackManager.hasObstacle.ToString() + ","
            + _PracticeSet.SpawnObj_x[0].ToString() + "," + _PracticeSet.SpawnObj_x[1].ToString() + "," + _PracticeSet.SpawnObj_x[2].ToString() + "," + _PracticeSet.SpawnObjsize_x[0].ToString() + "," + _PracticeSet.SpawnObjsize_x[1].ToString() + "," + _PracticeSet.SpawnObjsize_x[2].ToString() + ","
            + _PracticeSet.SpawnObj_y1[0].ToString() + "," + _PracticeSet.SpawnObj_y2[0].ToString() + "," + _PracticeSet.SpawnObj_y1[1].ToString() + "," + _PracticeSet.SpawnObj_y2[1].ToString() + "," + _PracticeSet.SpawnObj_y1[2].ToString() + "," + _PracticeSet.SpawnObj_y2[2].ToString() + ","
            + _PracticeSet.SpawnObjsize_y1[0].ToString() + "," + _PracticeSet.SpawnObjsize_y2[0].ToString() + "," + _PracticeSet.SpawnObjsize_y1[1].ToString() + "," + _PracticeSet.SpawnObjsize_y2[1].ToString() + "," + _PracticeSet.SpawnObjsize_y1[2].ToString() + "," + _PracticeSet.SpawnObjsize_y2[2].ToString() + ","
            + _PracticeSet.SpawnObj_x[4].ToString() + "," + _PracticeSet.SpawnObj_x[5].ToString() + "," + _PracticeSet.SpawnObj_x[5].ToString() + "," + _PracticeSet.SpawnObjsize_x[3].ToString() + "," + _PracticeSet.SpawnObjsize_x[4].ToString() + "," + _PracticeSet.SpawnObjsize_x[5].ToString() + ","
            + _PracticeSet.SpawnObj_y1[4].ToString() + "," + _PracticeSet.SpawnObj_y2[4].ToString() + "," + _PracticeSet.SpawnObj_y1[4].ToString() + "," + _PracticeSet.SpawnObj_y2[4].ToString() + "," + _PracticeSet.SpawnObj_y1[5].ToString() + "," + _PracticeSet.SpawnObj_y2[5].ToString() + ","
            + _PracticeSet.SpawnObjsize_y1[4].ToString() + "," + _PracticeSet.SpawnObjsize_y2[4].ToString() + "," + _PracticeSet.SpawnObjsize_y1[4].ToString() + "," + _PracticeSet.SpawnObjsize_y2[4].ToString() + "," + _PracticeSet.SpawnObjsize_y1[5].ToString() + "," + _PracticeSet.SpawnObjsize_y2[5].ToString() + ","
            + "\n";
        }
        return Content;
    }
    public void ExportCsv(string wintype)
    {
        DownloadFile("result_blackjack_" + _Title + "_" + Trial.ToString() + "_" + wintype + "win" + ".csv", WriteContent());
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
        FirstPressing = new List<bool>();
        SecondPressing = new List<bool>();
        ThirdPressing = new List<bool>();
        FourthPressing = new List<bool>();
        FirstPressingTime = new List<float>();
        SecondPressingTime = new List<float>();
        ThirdPressingTime = new List<float>();
        FourthPressingTime = new List<float>();
        BlackGoaled = new List<bool>();
        RedGoaled = new List<bool>();
        Times = new List<float>();
    }
}
