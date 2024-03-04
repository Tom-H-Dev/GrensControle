using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialoge : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TextComponent;
    

    [SerializeField] private float textspeed = 0.5f;

    [SerializeField] private GameObject button;
    [SerializeField] private GameObject ButtonContainer;


    private bool resetDialoge = true;
    private int index;
    private int Lineindex;

    [SerializeField] public List<customlist> mylist = new List<customlist>();
    

    [SerializeField] private GameObject Startd;
    private StartDialoge STD;
    
    void Start()
    {
        STD = Startd.GetComponent<StartDialoge>();
        TextComponent.text = string.Empty;
        startDailogo();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TextComponent.text == mylist[index].lines[Lineindex].ToString())
            {
                NextLine();
                print("jack");
            }
            else
            {
                StopAllCoroutines();
                TextComponent.text = mylist[index].lines[Lineindex].ToString();
                print("fack");
            }
        }
    }

    void startDailogo()
    {
        
        StartCoroutine(TypeLine(mylist[index].lines[Lineindex]));     
    }

    IEnumerator TypeLine(string line)
    {
        foreach (char c in line)
        {
            TextComponent.text += c;
            yield return new WaitForSeconds(textspeed);
        }
    }
    void NextLine()
    {
        Lineindex++;
        if (Lineindex < mylist[index].lines.Length)
        {
            TextComponent.text = string.Empty;
            StartCoroutine(TypeLine(mylist[index].lines[Lineindex]));
        }
        else
        {
            Lineindex = 0;
            index++;
            if (index < mylist.Count)
            {
                startDailogo();
            }
            else
            {
                index = 0;
                Lineindex = 0;
                STD.enabled = true;
                //button.SetActive(false);
                gameObject.SetActive(false);
                ButtonContainer.SetActive(true);
                TextComponent.text = string.Empty;
            }
        }
    }

    
}
