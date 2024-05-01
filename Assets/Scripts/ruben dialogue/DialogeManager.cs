using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.NetworkInformation;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using UnityEditor;

public class DialogeManager : MonoBehaviour
{
    [Header("List")]
    [SerializeField] private List<Item> textList = new List<Item>();
    [SerializeField] private float textSpeed = 0.5f;
    [SerializeField] private string[] changeWord;
    [SerializeField] private string driverName;
    [SerializeField] private List<GameObject> buttons;
    
    
    private bool _textStart = false, _check;
    private int _index, _lineIndex,indexssss, indexSes, currentLineIndex, dialogueIndex;
    public string[] _words;
    public string updatedLine, wordToType;

    public PlayerMovement _playerMovement;
    public PlayerLook _playerLook;
    public TextMeshProUGUI TextComponent;
    public GameObject Text;
    
    public int selectedLineIndex = -1, randomIndex;
    private int CLI = -1;
    
    public DocVerifyPro doc;

    public int selectedDialogueIndex;
    
    private carBehaviorDialogue CarBehavior;
    
    public Item BlankItem;
    public List<Item> ItemDatabase = new List<Item>();

    public TextAsset text;

    public string textName, teamName, questionName, MadnessName;
    
    public List<Item> matchingItems = new List<Item>();
    private void Start()
    {
        CarBehavior = FindObjectOfType<carBehaviorDialogue>();
        InitializeVariables();
        loadItemData();
    }

    private void InitializeVariables()
    {
        _index = _lineIndex = currentLineIndex = indexSes = 0;
        _textStart = false;
    }

    private void Update()
    {
        if (_textStart && Input.GetMouseButtonDown(0))
        {
            if (_check)
            {
                print(_check + "is chack");
                _check = false;
                NextLine();
            }
            else
            {
                print("denk denks");
                Cursor.lockState = CursorLockMode.None;
                ProcessDialogueLine();
            }
        }
    }
    

    public void TextStart(PlayerMovement playerMovement, PlayerLook playerLook)
    {
        TextComponent.text = string.Empty;
        foreach (var button in buttons)
        {
            button.SetActive(true);
        }

        _playerMovement = playerMovement;
        _playerMovement.enabled = false;
        
        _playerLook = playerLook;
        _playerLook.enabled = false;

        Cursor.lockState = CursorLockMode.None;
    }
    private void ProcessDialogueLine()
    {
        float minMadnessDifference = float.MaxValue;
        
        for (int i = 0; i < textList[_index].lines.Length; i++)
        {
            float madnessDifference = Mathf.Abs(textList[_index].madness - CarBehavior.madnessTimer);
            
            if (madnessDifference < minMadnessDifference)
            {
                minMadnessDifference = madnessDifference;
            }
        }
        
        if (selectedLineIndex != -1)
        {
            print("dang");
            string selectedline = textList[_index].lines;
            _words = selectedline.Split(' ');
            updatedLine = string.Empty;
            foreach (string word in _words)
            {
                bool isChecked = false;
                foreach (string wordToChange in changeWord)
                {
                    if (word == wordToChange)
                    {
                        updatedLine += driverName;
                        isChecked = true;
                        break;
                    }
                }
                if (!isChecked)
                {
                    _check = true;
                    updatedLine += word + " ";
                }
            }

            TextComponent.text = updatedLine.Trim();
            StopAllCoroutines();
        }
       
    }
    public void StartDialogueButton(int buttonIndex)
    {
        matchingItems.Clear();
        foreach (var item in ItemDatabase)
        { 
            if (item.team == _playerLook.team && item.question == buttonIndex)
            {
                matchingItems.Add(item);
            }
        }

        if (matchingItems.Count > 0)
        {
            randomIndex = Random.Range(0, matchingItems.Count);
            StartDialogue(matchingItems[randomIndex]);
            
        }
    }
    

    public void StartDialogue(Item selectedItem)
    {
        _words = selectedItem.lines.Split(' ');

        selectedLineIndex = selectedItem.question;
        selectedDialogueIndex = selectedItem.team;
        
        Text.SetActive(true);
        foreach (var butt in buttons)
        {
            butt.SetActive(false);
        }

        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        foreach (string word in _words)
        {
            wordToType = word;

            foreach (string wordToChange in changeWord)
            {
                if (word == wordToChange)
                {
                    wordToType = driverName;
                    break;
                }
            }

            foreach (char c in wordToType)
            {
                TextComponent.text += c;
                _textStart = true;
                yield return new WaitForSeconds(textSpeed);
            }
            TextComponent.text += ' ';
        }
        yield return new WaitForSeconds(textSpeed);
    }

    private void NextLine()
    {
        Cursor.lockState = CursorLockMode.None;
        
        Text.SetActive(false);
        
        InitializeVariables();
        
        if (_playerLook.team == 1)
        {
            booleanOn();
        }
        
        foreach (var button in buttons)
        {
            button.SetActive(true);
        }
    }

    public void booleanOn()
    {
        doc.papers = true;
    }

    public void EndDialogueButton()
    {
        _playerLook.enabled = true;
        _playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;

        foreach (var button in buttons)
        {
            button.SetActive(false);
        }
    }
    public void loadItemData()
    {
        ItemDatabase.Clear();
        
        List<Dictionary<string, object>> data = CSVReader.Read(text.ToString());
        
        for (var i = 0; i < data.Count; i++) 
        {
            if (data[i].ContainsKey(textName))
            {
                string text = data[i][textName].ToString();
                int team = int.Parse(data[i][teamName].ToString(), System.Globalization.NumberStyles.Integer);
                int question = int.Parse(data[i][questionName].ToString(), System.Globalization.NumberStyles.Integer);
                int madness = int.Parse(data[i][MadnessName].ToString(), System.Globalization.NumberStyles.Integer);
                
                AddItem(text ,team ,question, madness);       
            }
            else
            {
                Debug.Log(" help");
            }
        }
    }

    void AddItem(string text, int team , int question, int madness)
    {
        Item tempItem = new Item(BlankItem);
        
        tempItem.lines = text;
        tempItem.team = team;
        tempItem.question = question;
        tempItem.madness = madness;
        
        ItemDatabase.Add(tempItem);
    }
}

[CustomEditor(typeof(DialogeManager))]
public class dialogeManagerButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DialogeManager loadExcel = (DialogeManager)target;

        GUILayout.Label(" Reload Item DataBase", EditorStyles.boldLabel);
        if (GUILayout.Button("reload Items"))
        {
            loadExcel.loadItemData();
        }
    }
}

