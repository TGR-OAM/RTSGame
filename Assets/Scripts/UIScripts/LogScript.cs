using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UIScripts
{
    public struct MessageData
    {
        public Color color;
        public int fontSize;

        public float timeBeforeDestruction;

        public string text;

        public MessageData( Color color,int fontSize, float timeBeforeDestruction,string text)
        {
            this.color = color;
            this.fontSize = fontSize;
            this.timeBeforeDestruction = timeBeforeDestruction;
            this.text = text;
        }
    }


    public class LogScript : MonoBehaviour
    {
        public GameObject LogTextMessagePrefab;

        public void InitMessage(MessageData MData)
        {
            GameObject NewMassege = GameObject.Instantiate(LogTextMessagePrefab);
            NewMassege.name = "Message at " + Time.realtimeSinceStartup.ToString();
            NewMassege.GetComponent<MessageScript>().InitMessage(MData);
            NewMassege.transform.parent = this.transform;
        }

    }
}