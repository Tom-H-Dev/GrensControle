using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

[Serializable] public class Item
{
    //public List<CustomPro> myList = new List<CustomPro>();
    public string lines;
    public int team;
    public int question;
    public int madness;
    public bool answer;
    public List<GameObject> answerbutton;
    
    public Item(Item d)
    {
        lines = d.lines;
        team = d.team;
        question = d.question;
        madness = d.madness;
    }
}

[Serializable] public enum key 
{ 
    objective, 
    noObjective
}



