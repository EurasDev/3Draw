using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ChatBox : MonoBehaviour
{
    string[] commands = { "/clear", "/debug", "/save", "/export", "/filter" };
    GameObject inputBox;
    GameObject log;
    List<string> msgs = new List<string>();
    Text text;

    void Start()
    {
        inputBox = GameObject.FindGameObjectWithTag("chatbox");
        log = GameObject.FindGameObjectWithTag("chatpanel");
        text = log.GetComponentInChildren<Text>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            inputBox.GetComponent<InputField>().ActivateInputField();
        }
    }

    public void OnSubmit(string str)
    {
        msgs.Add(str);
        text.text = string.Empty;

        if (msgs.Count >= 7)
        {
            msgs.RemoveAt(0);
        }


        foreach (string s in msgs)
        {
            foreach(string c in commands)
            {
                if(s == c)
                {

                }
            }
            if(s[0] == '/')
            {
                switch (s)
                {
                    default:
                        text.text += "Unknown command" + Environment.NewLine;
                        break;

                    case "/help":
                        text.text += "/clear | /debug {on/off} | /save | /export | /filter {on/off}" + Environment.NewLine;
                        break;

                    case "/clear":
                        ClearBlocks();
                        text.text += "All block entities removed." + Environment.NewLine;
                        break;

                    case "/debug on":
                        text.text += "Debug mode activated." + Environment.NewLine;
                        break;
                    case "/debug off":
                        text.text += "Debug mode deactivated." + Environment.NewLine;
                        break;

                    case "/save":
                        text.text += "Current build has been saved." + Environment.NewLine;
                        break;

                    case "/export":
                        text.text += "Current build has been exported." + Environment.NewLine;
                        break;

                    case "/filter on":
                        text.text += "Swearing filter is now enabled." + Environment.NewLine;
                        break;
                    case "/filter off":
                        text.text += "Swearing filter is now disabled." + Environment.NewLine;
                        break;
                }
            }
            else
            {
                text.text += s + Environment.NewLine;
            }
        }
        inputBox.GetComponent<InputField>().text = string.Empty;
    }


    public void ClearBlocks()
    {
        foreach (GameObject go in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        {
            if (go.name == "Build Block")
            {
                Destroy(go.transform.gameObject);
                Place.blockCount = 0;
            }
        }
    }
}
