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
    
    public key keys;
    
    public Item(Item d)
    {
        lines = d.lines;
        team = d.team;
        question = d.question;
        madness = d.madness;
    }
}

[Serializable] public class CustomPro
{
    public List<custompls> list = new List<custompls>();
    public int TeamDialogue;
    public GameObject[] Buttons;
}

[Serializable] public class custompls
{
    public string[] lines;
    public float madness;
}

[Serializable] public enum key 
{ 
    objective, 
    noObjective
}



