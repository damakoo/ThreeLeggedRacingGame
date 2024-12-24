using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardsList : MonoBehaviour
{
    [SerializeField] BlackJackManager _blackJackManager; 
    //public List<CardState_opponent> MyCardsList_opponent;
    //public List<CardState_opponent> YourCardsList_opponent;
    public CardState MyFieldCard { get; set; }
    //public CardState YourFieldCard { get; set; }
    //[SerializeField] Transform YourFieldCardtransform;
    PracticeSet _PracticeSet;
    public void SetPracticeSet(PracticeSet _practiceset)
    {
        _PracticeSet = _practiceset;
    }
    public void ReInitializeCards()
    {
        _PracticeSet.MySelectedTime = new List<float>();
        _PracticeSet.YourSelectedTime = new List<float>();
        for (int i = 0; i < _PracticeSet.NumberofSet; i++)
        {
            _PracticeSet.MySelectedTime.Add(0);
            _PracticeSet.YourSelectedTime.Add(0);
        }

    }
}
