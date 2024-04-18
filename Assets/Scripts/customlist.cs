using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable] public class customlist
{
    public List<CustomPro> myList = new List<CustomPro>();
}

[Serializable] public class CustomPro
{
    public List<custompls> list = new List<custompls>();
    public int TeamDialogue;
    public GameObject[] Buttons;
    public key keys;
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



