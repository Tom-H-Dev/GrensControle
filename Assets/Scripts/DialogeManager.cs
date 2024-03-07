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
    private int index;
    private int Lineindex;

    [SerializeField] public List<customlist> mylist = new List<customlist>();

    public bool textStart = false;
    private DialogeManager STD;

    private string[] words;
    private string wordToType;

    public string changeWord;
    public string TheWord;
    void Start()
    {
        foreach (GameObject Custom in MyGam)
        {
            Custom.SetActive(false);
        }
    }
    public void startText()
    {
        mylist[index].TextComponent.text = string.Empty;
        foreach (GameObject custom in MyGam)
        {
            custom.SetActive(true);
        }

        PlayerLK.enabled = false;
        PlayerMV.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }
    public void talk(int buttonIndex)
    {
        mylist[index].TextComponent.text = string.Empty;
        index = buttonIndex;
        Lineindex = 0;
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
                if (mylist[index].TextComponent.text == mylist[index].lines[Lineindex].ToString())
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    mylist[index].TextComponent.text = mylist[index].lines[Lineindex].ToString();
                }
            }    
        }
    }

    public void startDailogo()
    {
        mylist[index].Text.SetActive(true);
        StartCoroutine(TypeLine(mylist[index].lines[Lineindex]));     
    }

    IEnumerator TypeLine(string line)
    {
        words = line.Split(' ');
        
        
        foreach (char c in line)
        {
            foreach (string word in words)
            {
                wordToType = word;

                if (word == changeWord)
                {
                    wordToType = TheWord;
                }
            }
            mylist[index].TextComponent.text += c;
            textStart = true;
            yield return new WaitForSeconds(textspeed);
        }
        mylist[index].TextComponent.text += ' ';
        yield return new WaitForSeconds(textspeed);
    }
    void NextLine()
    {
        Lineindex++;
        if (Lineindex < mylist[index].lines.Length)
        {
            mylist[index].TextComponent.text = string.Empty;
            StartCoroutine(TypeLine(mylist[index].lines[Lineindex]));
        }
        else
        { 
            mylist[index].Text.SetActive(false);
            index = 0;
            Lineindex = 0;
            foreach (GameObject Custom in MyGam)
            {
                
                Custom.SetActive(true);
            }
        }
    }
}
