using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class DialogueManager : MonoBehaviour
{
    [Header("List")] [Tooltip("how fast the text types closer to the 0 the faster it types")] 
    [SerializeField] private float textSpeed = 0.5f;

    [Tooltip("if any of these word are being typed change it with driver name and")] 
    [SerializeField] private string[] changeWord;
    
    [Tooltip("player 2 buttons when talking to driver")] 
    [SerializeField] private List<GameObject> Player2Buttons;

    [Tooltip("player 1 buttons when talking to driver")] 
    [SerializeField] private List<GameObject> Player1Buttons;

    [SerializeField] private List<Item> ItemDatabase = new List<Item>();

    [SerializeField] private TextAsset text;

    [SerializeField] private string textName, teamName, questionName, MadnessName;
    
    
    [SerializeField] private TextMeshProUGUI TextComponent;
    [SerializeField] private GameObject TextObject;

    [SerializeField] private int selectedLineIndex = -1, randomIndex;
    
    private string driverName;
    private string DriverSecondName;
    
    private bool _textStart = false, _check;
    private int _index;
    private string[] _words;
    private string _updatedLine, _wordToType;

    private PlayerMovement _playerMovement;
    private PlayerLook _playerLook;
    
    private DocVerifyPro _doc;

    private carBehaviorDialogue CarBehavior;
    private DriverManager _driverManager;

    private Item BlankItem;
   
    
    private List<Item> _matchingItems = new List<Item>();
    
    private PlayerUI _DoingSomething;
    
    private int indexlist;
    
    private void Start()
    {
        CarBehavior = FindObjectOfType<carBehaviorDialogue>();
        _doc = FindObjectOfType<DocVerifyPro>();
        
        InitializeVariables();
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
                string selectedline = ItemDatabase[_index].lines;
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
        _DoingSomething = playerMovement.GetComponent<PlayerUI>();
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
            
        _DoingSomething._isDoingSomething = true;      
            
        
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
        
        for (int i = 0; i < ItemDatabase[_index].lines.Length; i++)
        {
            float madnessDifference = Mathf.Abs(ItemDatabase[_index].madness - CarBehavior.madnessTimer);
            
            if (madnessDifference < minMadnessDifference)
            {
                minMadnessDifference = madnessDifference;
            }
        }
        
        if (selectedLineIndex != -1)
        {
            _updatedLine = string.Empty;
            foreach (string word in _words)
            {
                bool isChecked = false;
                foreach (string wordToChange in changeWord)
                {
                    if (word == wordToChange)
                    {
                        string test = driverName + " " + DriverSecondName;
                        _updatedLine += test;
                        isChecked = true;
                        break;
                    }
                }
                if (!isChecked)
                {
                    _check = true;
                    _updatedLine += word + " ";
                }
            }

            TextComponent.text = _updatedLine.Trim();
            StopAllCoroutines();
        }
       
    }
    public void StartDialogueButton(int buttonIndex)
    {
        
        TextComponent.text = string.Empty; 
        _matchingItems.Clear();
        foreach (var item in ItemDatabase)
        { 
            if (item.team == _playerLook.team && item.question == buttonIndex)
            {
                _matchingItems.Add(item);
            }
        }

        if (_matchingItems.Count > 0)
        {
            randomIndex = Random.Range(0, _matchingItems.Count);
            StartDialogue(_matchingItems[randomIndex]);
            
        }
    }
    

    public void StartDialogue(Item selectedItem)
    {
        _words = selectedItem.lines.Split(' ');

        selectedLineIndex = selectedItem.question;
        
        TextObject.SetActive(true);
        
        turnoff();

        StartCoroutine(TypeLine());
    }

    void turnoff()
    {
        foreach (var butt in Player2Buttons)
        {
            butt.SetActive(false);
            foreach (var button1 in Player1Buttons)
            {
                button1.SetActive(false);
                foreach (var ansButton in ItemDatabase[indexlist].answerbutton)
                {
                    ansButton.SetActive(false);
                }
            }
        }
    }
    private IEnumerator TypeLine()
    {
        foreach (string word in _words)
        {
            _wordToType = word;

            foreach (string wordToChange in changeWord)
            {
                if (word == wordToChange)
                {
                    _wordToType = String.Empty;
                    string test = driverName + " " + DriverSecondName;
                    _wordToType += test;
                    break;
                }
            }

            foreach (char c in _wordToType)
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
        indexlist = selectedLineIndex;
        
        
        if (ItemDatabase[selectedLineIndex].answer == true)
        {
            
            foreach (var ansButton in ItemDatabase[selectedLineIndex].answerbutton)
            {
                ansButton.SetActive(true);
            }
        }
        else
        {
            foreach (var button in playerButtons)
            {
                button.SetActive(true);
            }   
        }
    }
    public void booleanOn()
    {
        _doc.papers = true;
    }

    public void EndDialogueButton()
    {
        _playerLook._canLook = true;
        _playerMovement._canMove = true;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        _DoingSomething._isDoingSomething = false;

        
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


