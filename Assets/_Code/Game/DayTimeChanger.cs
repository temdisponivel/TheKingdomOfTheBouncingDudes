using System;
using UnityEngine;

namespace BounceDudes
{
    public class DayTimeChanger : MonoBehaviour
    {
        public GameObject MorningPosition;
        public GameObject AfternonPosition;
        public GameObject NightPosition;
        
        public void Start()
        {
            switch (GameManager.Instance.CurrentDayTimeSequence)
            {
                case DayTimeSequence.Morning:
                    this.transform.position = MorningPosition.transform.position;
                    GameManager.Instance.CurrentDayTimeSequence = DayTimeSequence.MorningMorning;
                    break;
                case DayTimeSequence.MorningMorning:
                    this.transform.position = MorningPosition.transform.position;
                    GameManager.Instance.CurrentDayTimeSequence = DayTimeSequence.Afternon;
                    break;
                case DayTimeSequence.Afternon:
                    this.transform.position = AfternonPosition.transform.position;
                    GameManager.Instance.CurrentDayTimeSequence = DayTimeSequence.Night;
                    break;
                case DayTimeSequence.Night:
                    this.transform.position = NightPosition.transform.position;
                    GameManager.Instance.CurrentDayTimeSequence = DayTimeSequence.Morning;
                    break;
            }
        }
    }
}
