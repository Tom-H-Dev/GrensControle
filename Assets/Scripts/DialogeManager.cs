using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;

public class DialogeManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> MyGam = new List<GameObject>();
    
    public PlayerMovement PlayerMV;
    public PlayerLook PlayerLK;
    
    [SerializeField] private float textspeed = 0.5f;
    
    [SerializeField] private GameObject ButtonContainer;
    
    private bool resetDialoge = true;
    private int _Index, _LineIndex;

    [SerializeField] public List<customlist> mylist = new List<customlist>();

    public bool textStart = false;
    private bool check;
    private DialogeManager STD;
    
    public string changeWord;
    public string TheWord;
    public string[] words;
    public string updatedLine;
    void Start()
    {
        foreach (GameObject Custom in MyGam)
        {
            Custom.SetActive(false);
        }
    }
    public void startText(PlayerMovement l_player, PlayerLook l_look)
    {
        mylist[_Index].TextComponent.text = string.Empty;
        foreach (GameObject custom in MyGam)
        {
            custom.SetActive(true);
        }

        PlayerMV = l_player;
        l_player.enabled = false;

        PlayerLK = l_look;
        l_look.enabled = false;
        
        Cursor.lockState = CursorLockMode.None;
    }
    public void talk(int buttonIndex)
    {
        mylist[_Index].TextComponent.text = string.Empty;
        _Index = buttonIndex;
        _LineIndex = 0;
        startDailogo();
        
        foreach (GameObject Custom in MyGam)
        {
            Custom.SetActive(false);
        }
    }
    void Update()
    {
        if (textStart == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (mylist[_Index].TextComponent.text == mylist[_Index].lines[_LineIndex].ToString())
                {
                    NextLine();
                    print("jacks");
                }
                else if (check == true)
                {
                    NextLine();
                    print("dragon ball");
                }
                else
                {
                    words = mylist[_Index].lines[_LineIndex].ToString().Split(' ');
                    updatedLine = " ";

                    foreach (string word in words)
                    {
                        if (word == changeWord)
                        {
                            updatedLine += TheWord + " ";
                            Debug.Log("test");
                        }
                        else
                        {
                            updatedLine += word + " ";
                            Debug.Log("fest");
                        }
                    }

                    check = true;
                    mylist[_Index].TextComponent.text = updatedLine.Trim();
                    StopAllCoroutines();
                }
                
            }    
        }
    }

    public void startDailogo()
    {
        mylist[_Index].Text.SetActive(true);
        StartCoroutine(TypeLine(mylist[_Index].lines[_LineIndex]));     
    }

    IEnumerator TypeLine(string line)
    {
        words = line.Split(' ');
        
        
        foreach (string word in words)
        {
            string wordToType = word;

            if (word == changeWord)
            {
                wordToType = TheWord;
            }
            foreach (char c in wordToType)
            {
                mylist[_Index].TextComponent.text += c;
                textStart = true;
                yield return new WaitForSeconds(textspeed);
            }
            //mylist[index].TextComponent.text += ' ';
        }
        yield return new WaitForSeconds(textspeed);
    }
    void NextLine()
    {
        _LineIndex++;
        if (_LineIndex < mylist[_Index].lines.Length)
        {
            mylist[_Index].TextComponent.text = string.Empty;
            StartCoroutine(TypeLine(mylist[_Index].lines[_LineIndex]));
            check = false;
        }
        else
        { 
            mylist[_Index].Text.SetActive(false);
            _Index = 0;
            _LineIndex = 0;
            foreach (GameObject Custom in MyGam)
            {
                Custom.SetActive(true);
            }
        }
    }
    public void endDialogue()
    {
        PlayerLK.enabled = true;
        PlayerMV.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        
        foreach (GameObject custom in MyGam)
        {
            custom.SetActive(false);
        }
    }
}
