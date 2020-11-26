using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text), typeof(ContentSizeFitter))]
public class MessageScript : MonoBehaviour
{
    bool isDestructin = false;
    Text thisText;

    public void InitMessage(MessageData MData)
    {
        thisText = this.GetComponent<Text>();

        //maybe in future turn this to implicit operator =
        thisText.fontSize = MData.fontSize;
        thisText.color = MData.color;
        thisText.text = MData.text;

        Debug.Log(MData.text);

        Invoke("InitDestruction", MData.timeBeforeDestruction);
    }

    void InitDestruction()
    {
        isDestructin = true;
        this.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        GameObject.Destroy(this.transform.gameObject,1f);
    }

    private void Update()
    {
        if(isDestructin)
        {
            thisText.color = new Color(thisText.color.r, thisText.color.g, thisText.color.b, Mathf.Lerp(thisText.color.a, 0, .1f));
            thisText.fontSize = (int)Mathf.Lerp(thisText.fontSize,0,.1f);
            thisText.rectTransform.sizeDelta = new Vector2(thisText.rectTransform.rect.width,Mathf.Lerp(thisText.rectTransform.rect.height,0,.05f));
        }
    }
}
