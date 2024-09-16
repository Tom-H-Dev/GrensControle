using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using Photon.Pun;

public class DialogueManager : MonoBehaviour
{
    [Header("List")]
    [Tooltip("How fast the text types closer to the 0 the faster it types.")]
    [SerializeField] private float textSpeed = 0.5f;

    [Tooltip("A list of all the question indexes that do not have an effect on the happiness of the driver")]
    [SerializeField] private List<int> _neutralQuestionIndexes = new List<int>();

    [Tooltip("If any of these word are being typed change it with driver name and secondname.")]
    [SerializeField] private string[] changeWord;

    [Tooltip("Player 2 buttons when talking to driver")]
    [SerializeField] private List<GameObject> Player2Buttons;

    [Tooltip("Player 1 buttons that activiet when talking to the driver.")]
    [SerializeField] private List<GameObject> Player1Buttons;

    [SerializeField] private List<Item> ItemDatabase = new List<Item>();

    [SerializeField] private TextAsset csvFile;

    [SerializeField] private Item BlankItem;

    [Header("Answer Ranges")]
    float _angryAnswerMin = 0;
    float _angryAnswerMax = 30;
    float _neutralAnswerMin = 30.0001f;
    float _neutralAnswerMax = 70;
    float _happyAnswerMin = 70.0001f;
    float _happyAnswerMax = 100;


    [Header("Question information")]
    private int _questionNumberId = 0;
    private int _happinessAddOrRemove = 5;
    //[SerializeField] private TextAsset text;

    //[SerializeField] private string textName, teamName, questionName, MadnessName;

    [Space(5)]
    [SerializeField] private TextMeshProUGUI TextComponent;
    [SerializeField] private GameObject TextObject;
    [SerializeField] private List<GameObject> _endConvoButtons;

    private int selectedLineIndex = -1, randomIndex;

    private string driverName;
    private string DriverSecondName;
    private string _driverGuestName;
    private string _driverGuestLastName;
    private string _driverRank;
    private string _driverGuestRank;
    private string _buildingDefensie;
    private string _driverTimeOnBase;
    private string _afdeling;

    private string[] _words;
    private string _updatedLine, _wordToType;

    private bool _textStart = false, _check;

    private int _index, index2;
    private int indexlist;

    //private Item BlankItem;

    private PlayerUI _IsTalking; //is for the pas manu when this is false ecape works
    private PlayerMovement _playerMovement;
    private PlayerLook _playerLook;
    private DocVerifyPro _doc;
    public carBehaviorDialogue _carBehavior;
    private DriverManager _driverManager;
    private PhotonView _photonView;

    private string selectedline;

    private MessageStates _driverState;
    private void Start()
    {
        //_carBehavior = FindObjectOfType<carBehaviorDialogue>();
        _doc = FindObjectOfType<DocVerifyPro>();
        _photonView = GetComponent<PhotonView>();
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        index2 = _index = 0;
        _textStart = false;
    }

    private void Update()
    {
        if (_textStart && Input.GetMouseButtonDown(0))
        {
            if (_check)
            {
                _check = false;
                selectedline = ItemDatabase[_index].Text[index2].lines;
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
        _IsTalking = playerMovement.GetComponent<PlayerUI>();
        _driverManager = RouteManager.instance._activeCars[0].GetComponent<DriverManager>();

        if (_driverManager._isFalsified)
        {
            driverName = _driverManager._givenFitstName;
            DriverSecondName = _driverManager._givenLastName;
        }
        else
        {
            driverName = _driverManager._driverFirstName;
            DriverSecondName = _driverManager._driverLastName;
        }

        if (_driverManager._driverIsGeust)
        {
            _driverGuestName = _driverManager._guestpersonFirstName;
            _driverGuestLastName = _driverManager._guestpersonLastName;
            _driverGuestRank = _driverManager._guestPersonRank;
        }

        _buildingDefensie = _driverManager._workBuilding;
        _driverTimeOnBase = _driverManager._timeOnBase;
        _afdeling = _driverManager._afdeling;



        TextComponent.text = string.Empty;

        _playerMovement = playerMovement;
        _playerMovement._canMove = false;

        _playerLook = playerLook;
        _playerLook._canLook = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _IsTalking._isDoingSomething = true;


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

        for (int i = 0; i < ItemDatabase[_index].Text[index2].lines.Length; i++)
        {
            float madnessDifference = Mathf.Abs(ItemDatabase[_index].madness - _carBehavior.madnessTimer);

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
                    if (word == changeWord[0]) //#tijd#
                    {
                        string l_timeOnBase = _driverTimeOnBase;
                        _updatedLine += l_timeOnBase;
                        isChecked = true;
                        break;
                    }
                    else if (word == changeWord[1])//#funcite#
                    {
                        isChecked = true; //UNUSED
                        break;
                    }
                    else if (word == changeWord[2])//#naam
                    {
                        string l_driverName = driverName + " " + DriverSecondName;
                        _updatedLine += l_driverName;
                        isChecked = true;
                        break;
                    }
                    else if (word == changeWord[3])//#bezoekNaam
                    {
                        string l_guestName = _driverGuestRank + " " + _driverGuestName + " " + _driverGuestLastName;
                        _updatedLine += l_guestName;
                        isChecked = true;
                        break;
                    }
                    else if (word == changeWord[4])//#gebouw#
                    {
                        string l_building = _buildingDefensie;
                        _updatedLine += l_building;
                        isChecked = true;
                        break;
                    }
                    else if (word == changeWord[5])//#AfdelingOfLocatie#
                    {
                        string l_afdeling = _afdeling;
                        _updatedLine += l_afdeling;
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
    public void StartDialogueButton(int l_buttonIndex)
    {
        TextComponent.text = string.Empty;

        _questionNumberId = GetFirstDigit(l_buttonIndex);
        string l_anwerIndex = _questionNumberId.ToString();
        _photonView.RPC("UpdateDriverHappiness", RpcTarget.AllBufferedViaServer, GetLastDigit(l_buttonIndex));

        if (_carBehavior.happiness >= 0 && _carBehavior.happiness <= 100 && l_buttonIndex != 99999)
        {
            if (_carBehavior.happiness >= _angryAnswerMin && _carBehavior.happiness <= _angryAnswerMax)
            {
                _driverState = MessageStates.Angry;
                l_anwerIndex += "1";
            }
            else if (_carBehavior.happiness >= _neutralAnswerMin && _carBehavior.happiness <= _neutralAnswerMax)
            {
                _driverState = MessageStates.Neutral;
                l_anwerIndex += "2";
            }
            else if (_carBehavior.happiness >= _happyAnswerMin && _carBehavior.happiness <= _happyAnswerMax)
            {
                _driverState = MessageStates.Happy;
                l_anwerIndex += "3";
            }
            else Debug.LogError("Something with the happiness went wrong");
        }
        if (l_buttonIndex == 99999)
        {
            SearchForAnswerToGive(99999, l_buttonIndex);
        }
        else if (l_buttonIndex == 9000)
        {
            SearchForAnswerToGive(9000, l_buttonIndex);
        }
        else
        {
            SearchForAnswerToGive(int.Parse(l_anwerIndex), l_buttonIndex);
        }

    }

    [PunRPC]
    public void UpdateDriverHappiness(int l_answerIndex)
    {
        switch (l_answerIndex)
        {
            case 0:
                //Neutral answer
                _happinessAddOrRemove = 0;
                break;
            case 1:
                //Angry answer
                _happinessAddOrRemove = -5;
                break;
            case 2:
                //Neutral answer
                _happinessAddOrRemove = 0;
                break;
            case 3:
                //Happy answer
                _happinessAddOrRemove = 5;
                break;
            case 4:
                //Angry answer
                _happinessAddOrRemove = -5;
                break;
            case 5:
                //Neutral answer
                _happinessAddOrRemove = 0;
                break;
            case 6:
                //Happy answer
                _happinessAddOrRemove = 5;
                break;
            case 9:
                _happinessAddOrRemove = 0;
                break;
            default:
                Debug.LogError("Anser index not found!");
                break;
        }

        if (_carBehavior.happiness + _happinessAddOrRemove <= 100 && _carBehavior.happiness > 0)
            _carBehavior.happiness += _happinessAddOrRemove;
        else _carBehavior.happiness = 100;
    }

    private void SearchForAnswerToGive(int l_answerIndex, int l_buttonIndex)
    {
        //Check if first is higher or equal to 4 to end convo

        bool l_isNeutralQuestion = false;
        for (int i = 0; i < _neutralQuestionIndexes.Count; i++)
        {
            if (i == l_buttonIndex)
            {
                l_isNeutralQuestion = true;
            }
        }

        for (int i = 0; i < ItemDatabase.Count; i++)
        {
            if (!l_isNeutralQuestion)
            {
                if (ItemDatabase[i].question == l_answerIndex && ItemDatabase[i].team == _playerLook.team)
                {
                    _index = ItemDatabase.IndexOf(ItemDatabase[i]);
                    StartDialogue(ItemDatabase[i].Text[0].lines);
                }
            }
            else
            {
                if (l_buttonIndex == 20) //If the question is wether the driver is works here or is a guest
                {
                    if (_driverManager._driverIsGeust && ItemDatabase[i].question == 202)
                    {
                        _index = ItemDatabase.IndexOf(ItemDatabase[i]);
                        StartDialogue(ItemDatabase[i].Text[0].lines);
                    }
                    else if (!_driverManager._driverIsGeust && ItemDatabase[i].question == 201)
                    {
                        _index = ItemDatabase.IndexOf(ItemDatabase[i]);
                        StartDialogue(ItemDatabase[i].Text[0].lines);
                    }
                }
                if (l_buttonIndex == 99999)
                {
                    if (ItemDatabase[i].question == 99999 && ItemDatabase[i].team == _playerLook.team)
                    {
                        _index = ItemDatabase.IndexOf(ItemDatabase[i]);
                        StartDialogue(ItemDatabase[i].Text[0].lines);
                    }
                }
                if (l_buttonIndex == 9000)
                {
                    if (ItemDatabase[i].question == 9000 && ItemDatabase[i].team == _playerLook.team)
                    {
                        _index = ItemDatabase.IndexOf(ItemDatabase[i]);
                        StartDialogue(ItemDatabase[i].Text[0].lines);
                    }

                }
            }
        }
    }
    private static int GetFirstDigit(int n)
    {
        // Remove last digit from number 
        // till only one digit is left 
        while (n >= 10)
            n /= 10;

        // return the first digit 
        return n;
    }

    private static int GetLastDigit(int n)
    {
        // return the last digit 
        return (n % 10);
    }


    public void StartDialogue(string ll)
    {
        lucas closestLine = null;
        float minHappinessDifference = float.MaxValue;
        foreach (var line in ItemDatabase[_index].Text)
        {
            float happinessDifference = Mathf.Abs(line.happy - _carBehavior.happiness);
            if (happinessDifference < minHappinessDifference)
            {
                minHappinessDifference = happinessDifference;
                closestLine = line;
            }
        }

        if (closestLine != null)
        {
            ll = closestLine.lines;
        }

        _words = ll.Split(' ');

        selectedLineIndex = ItemDatabase[_index].question;

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
            ActivateButtons(Player1Buttons);
        }
        else
        {
            ActivateButtons(Player2Buttons);
        }
    }

    private void ActivateButtons(List<GameObject> playerButtons)
    {
        indexlist = _index;

        if (ItemDatabase[_index].answer)
        {
            foreach (var ansButton in ItemDatabase[_index].answerbutton)
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

        _IsTalking._isDoingSomething = false;


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

        List<Dictionary<string, object>> data = CSVReader.Read(csvFile.text);

        for (var i = 0; i < data.Count; i++)
        {
            if (data[i].ContainsKey("textName"))
            {
                string text = data[i]["textName"].ToString();
                int team = int.Parse(data[i]["teamName"].ToString(), System.Globalization.NumberStyles.Integer);
                int question = int.Parse(data[i]["questionName"].ToString(), System.Globalization.NumberStyles.Integer);
                int madness = int.Parse(data[i]["MadnessName"].ToString(), System.Globalization.NumberStyles.Integer);

                AddItem(text, team, question, madness);
            }
        }
    }

    void AddItem(string text, int team, int question, int madness)
    {
        Item tempItem = new Item(BlankItem);

        tempItem.lines = text;
        tempItem.team = team;
        tempItem.question = question;
        tempItem.madness = madness;

        ItemDatabase.Add(tempItem);
    }
}

enum MessageStates
{
    Angry,
    Neutral,
    Happy
}