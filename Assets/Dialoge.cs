using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dialoge : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TextComponent;
    
    [SerializeField] private  string[] lines;

    [SerializeField] private float textspeed = 0.5f;

    [SerializeField] private GameObject button;
    
    private int index;
    
    void Start()
    {
        TextComponent.text = string.Empty;
        startDailogo();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TextComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                TextComponent.text = lines[index];
            }
        }
    }

    void startDailogo()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            TextComponent.text += c;
            yield return new WaitForSeconds(textspeed);
        }
    }
    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            TextComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            button.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
