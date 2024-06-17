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

public class DialogueManager : MonoBehaviour
{
    [Header("List")]
    [HideInInspector]
    [SerializeField] private List<Item> textList = new List<Item>();
    [Tooltip("how fast the text types closer to the 0 the faster it types")]
    [SerializeField] private float textSpeed = 0.5f;
    [Tooltip("if any of these word are being typed change it with driver name and")]
    [SerializeField] private string[] changeWord;
    [HideInInspector]
    [SerializeField] private string driverName;
    [HideInInspector]
    [SerializeField] private string DriverSecondName;
    
    [Tooltip("player 2 buttons when talking to driver")]
    [SerializeField] private List<GameObject> Player2Buttons;
    [Tooltip("player 1 buttons when talking to driver")]
    [SerializeField] private List<GameObject> Player1Buttons;

    [HideInInspector]
    [SerializeField] private List<GameObject> carsInRange;
    
    private bool _textStart = false, _check;
    private int _index;
    private string[] _words;
    private string updatedLine, wordToType;

    private PlayerMovement _playerMovement;
    private PlayerLook _playerLook;
    
    public TextMeshProUGUI TextComponent;
    public GameObject TextObject;
    
    public int selectedLineIndex = -1, randomIndex;
    private int CLI = -1;
    
    public DocVerifyPro doc;

    public int selectedDialogueIndex;
    
    private carBehaviorDialogue CarBehavior;
    public DriverManager _driverManager;
    
    public Item BlankItem;
    public List<Item> ItemDatabase = new List<Item>();

    public TextAsset text;

    public string textName, teamName, questionName, MadnessName;
    
    public List<Item> matchingItems = new List<Item>();
    
    private PlayerUI DoingSomething;
    private void Start()
    {
        //TextObject = GameObject.Find("BackGroundText");
        //TextComponent = GameObject.Find("DriverText").GetComponent<TextMeshProUGUI>();
            
        CarBehavior = FindObjectOfType<carBehaviorDialogue>();
        InitializeVariables();
        //loadItemData();
    }

    private void InitializeVariables()
    {
        
        _index = 0;
        _textStart = false;
    }

    private void Update()
    {
        if (_textStart && Input.GetMouseButtonDown(0))
        {
            if (_check)
            {
                _check = false;
                string selectedline = textList[_index].lines;
                _words = selectedline.Split(' ');
                NextLine();
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                ProcessDialogueLine();
            }
        }
    }
    

    public void TextStart(PlayerMovement playerMovement, PlayerLook playerLook)
    {
        DoingSomething = playerMovement.GetComponent<PlayerUI>();
        _driverManager = RouteManager.instance._activeCars[0].GetComponent<DriverManager>();
        
        if (_driverManager._isFalsified == true)
        {
            driverName = _driverManager._givenFitstName;
            DriverSecondName = _driverManager._givenLastName;
        }
        else
        { 
            driverName = _driverManager._driverFirstName;
            DriverSecondName = _driverManager._driverLastName;
        }
        
        TextComponent.text = string.Empty;
        
        _playerMovement = playerMovement;
        _playerMovement._canMove = false;
        
        _playerLook = playerLook;
        _playerLook._canLook = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
            
        DoingSomething._isDoingSomething = true;      
            
        
        if (_playerLook.team == 1)
        {
            foreach (var button1 in Player1Buttons) 
            {
                button1.SetActive(true);
            }   
        }
        else
        {
            foreach (var button2 in Player2Buttons)
            {
                button2.SetActive(true);
            }   
        }
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
            updatedLine = string.Empty;
            foreach (string word in _words)
            {
                bool isChecked = false;
                foreach (string wordToChange in changeWord)
                {
                    if (word == wordToChange)
                    {
                        string test = driverName + " " + DriverSecondName;
                        updatedLine += test;
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
        
        TextComponent.text = string.Empty; 
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
        
        TextObject.SetActive(true);
        foreach (var butt in Player2Buttons)
        {
            butt.SetActive(false);
            foreach (var button1 in Player1Buttons)
            {
                button1.SetActive(false);
            }
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
                    wordToType = String.Empty;
                    string test = driverName + " " + DriverSecondName;
                    wordToType += test;
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
        
        TextObject.SetActive(false);
        
        InitializeVariables();
        
        if (_playerLook.team == 1)
        {
            booleanOn();
        }

        if (_playerLook.team == 1)
        {
            print("fuck");
            ActivateButtons(Player1Buttons);
        }
        else
        {print(" huck");
            ActivateButtons(Player2Buttons);
        }
    }

    private void ActivateButtons(List<GameObject> playerButtons)
    {
        print(" hughdwadwa");
        if (selectedLineIndex != -1 && textList[selectedLineIndex].answer == true)
        {
            print( " dwauid");
            foreach (var ansButton in textList[selectedLineIndex].answerbutton)
            {
                print(" past");
                ansButton.SetActive(true);
            }
        }
        else
        {
            foreach (var button in playerButtons)
            {
                print(" check");
                button.SetActive(true);
            }   
        }
    }
    public void booleanOn()
    {
        doc.papers = true;
    }

    public void EndDialogueButton()
    {
        _playerLook._canLook = true;
        _playerMovement._canMove = true;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        DoingSomething._isDoingSomething = false;

        
        foreach (var button in Player2Buttons)
        {
            button.SetActive(false);
            foreach (var button1 in Player1Buttons)
            {
                button1.SetActive(false);
            }
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


