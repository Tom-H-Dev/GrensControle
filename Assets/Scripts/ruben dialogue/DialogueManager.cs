using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    private int lastIndex = 0;
    private int _answerIndex;

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
        index2 = _index;
        _textStart = false;
    }

    private void Update()
    {
        if (_textStart && Input.GetMouseButtonDown(0))
        {
            if (_check)
            {
                _check = false;
                selectedline = ItemDatabase[_index].Text[0].lines;
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

    /// <summary>
    /// Starts the interaction with the driver. All the data gets set from the driver.
    /// </summary>
    /// <param name="playerMovement">: The script from the player movement to turn off the movement from the player.
    /// <param name="playerLook">: The script that refers to the PlayerLook to lock the rotation of the camera while the player is talking to the driver.
    public void TextStart(PlayerMovement playerMovement, PlayerLook playerLook)
    {
        _IsTalking = playerMovement.GetComponent<PlayerUI>();
        _driverManager = BarrierManager.instance._vehicle.GetComponent<DriverManager>();

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
        _driverGuestName = _driverManager._guestpersonFirstName;
        _driverGuestLastName = _driverManager._guestpersonLastName;
        _driverGuestRank = _driverManager._guestPersonRank;


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

    /// <summary>
    /// Processes the dialogue when the user clicks trough the dialogue typing to generate it at once and updates the the happiness locally.
    /// </summary>
    private void ProcessDialogueLine()
    {
        float minMadnessDifference = float.MaxValue;

        for (int i = 0; i < ItemDatabase[_index].Text[0].lines.Length; i++)
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
                    else if (word == changeWord[1])//#functie#
                    {
                        isChecked = true; //UNUSED
                        break;
                    }
                    else if (word == changeWord[2])//#naam#
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

    /// <summary>
    /// The script on the buttons to ask the questions.
    /// </summary>
    /// <param name="l_buttonIndex">: The index from the button to know what type of question got asked and on what question the player is.
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
            _answerIndex = int.Parse(l_anwerIndex);
            SearchForAnswerToGive(int.Parse(l_anwerIndex), l_buttonIndex);
        }

    }

    /// <summary>
    /// Updates the happiness of the driver based on the question asked by the driver.
    /// </summary>
    /// <param name="l_answerIndex">: The last digit of the button index from the questions.
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

    /// <summary>
    /// Searches all the ItemDatabase enteries to search for the correct answer to give.
    /// </summary>
    /// <param name="l_answerIndex">: The question index from the answer.
    /// <param name="l_buttonIndex">: The index of the button.
    private void SearchForAnswerToGive(int l_answerIndex, int l_buttonIndex)
    {
        //Check if first is higher or equal to 4 to end convo
        bool l_isNeutralQuestion = false;
        for (int i = 0; i < _neutralQuestionIndexes.Count; i++)
        {
            if (_neutralQuestionIndexes[i] == l_buttonIndex)
            {
                l_isNeutralQuestion = true;
            }
        }

        for (int i = 0; i < ItemDatabase.Count; i++)
        {
            if (!l_isNeutralQuestion)
            {
                if (_driverManager._driverIsGuest && GetLastDigit(l_buttonIndex) >= 4)
                {
                    print("is Guest");
                    string l_guestAnswerIndex = "";
                    switch (GetLastDigit(l_buttonIndex))
                    {
                        case 4:
                            l_guestAnswerIndex = "24";
                            break;
                        case 5:
                            l_guestAnswerIndex = "25";
                            break;
                        case 6:
                            l_guestAnswerIndex = "25";
                            break;
                        default:
                            Debug.LogError("No last index found");
                            break;
                    }
                    print(l_guestAnswerIndex);
                    if (ItemDatabase[i].question == int.Parse(l_guestAnswerIndex) && ItemDatabase[i].team == _playerLook.team)
                    {
                        _index = ItemDatabase.IndexOf(ItemDatabase[i]);
                        lastIndex = _index;
                        StartDialogue(ItemDatabase[i].Text[0].lines, lastIndex);
                    }
                }
                else
                {
                    if (ItemDatabase[i].question == l_answerIndex && ItemDatabase[i].team == _playerLook.team)
                    {
                        _index = ItemDatabase.IndexOf(ItemDatabase[i]);
                        lastIndex = _index;
                        StartDialogue(ItemDatabase[i].Text[0].lines, lastIndex);
                    }
                }
            }
            else
            {
                if (l_buttonIndex == 20) //If the question is wether the driver is works here or is a guest
                {
                    if (_driverManager._driverIsGuest && ItemDatabase[i].question == 202)
                    {
                        _index = ItemDatabase.IndexOf(ItemDatabase[i]);
                        lastIndex = _index;
                        StartDialogue(ItemDatabase[i].Text[0].lines, lastIndex);
                    }
                    else if (!_driverManager._driverIsGuest && ItemDatabase[i].question == 201)
                    {
                        _index = ItemDatabase.IndexOf(ItemDatabase[i]);
                        lastIndex = _index;
                        StartDialogue(ItemDatabase[i].Text[0].lines, lastIndex);
                    }
                }
                if (l_buttonIndex == 99999)
                {
                    if (ItemDatabase[i].question == 99999 && ItemDatabase[i].team == _playerLook.team)
                    {
                        _index = ItemDatabase.IndexOf(ItemDatabase[i]);
                        lastIndex = _index;
                        StartDialogue(ItemDatabase[i].Text[0].lines, lastIndex);
                    }
                }
                if (l_buttonIndex == 9000)
                {
                    if (ItemDatabase[i].question == 9000 && ItemDatabase[i].team == _playerLook.team)
                    {
                        _index = ItemDatabase.IndexOf(ItemDatabase[i]);
                        lastIndex = _index;
                        StartDialogue(ItemDatabase[i].Text[0].lines, lastIndex);
                    }

                }
            }
        }
    }

    /// <summary>
    /// This function will return the first digit of an integer sent by the user.
    /// </summary>
    /// <param name="n">: This is the number the user wants to get the first digit of.
    /// <returns></returns>
    private static int GetFirstDigit(int n)
    {
        // Remove last digit from number 
        // till only one digit is left 
        while (n >= 10)
            n /= 10;

        // return the first digit 
        return n;
    }

    /// <summary>
    /// This function will return the last digit of an integer sent by the user.
    /// </summary>
    /// <param name="n">: This is the numver the user wants to get the last digit of.
    /// <returns></returns>
    private static int GetLastDigit(int n)
    {
        // return the last digit 
        return (n % 10);
    }

    /// <summary>
    /// Starts the dialogue routine.
    /// </summary>
    /// <param name="ll">: The lines it needs to type.
    /// <param name="l_databaseIndex">: The index of the ItemDatabase where the line ll is from.
    public void StartDialogue(string ll, int l_databaseIndex)
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

    /// <summary>
    /// Turns all the buttons off in the ItemDatabase, also turns the start buttons for player 1 and player 2 off.
    /// </summary>
    void turnoff()
    {

        for (int i = 0; i < ItemDatabase.Count; i++)
        {
            for (int j = 0; j < ItemDatabase[i].answerbutton.Count; j++)
            {
                ItemDatabase[i].answerbutton[j].SetActive(false);
            }
        }
        foreach (var butt in Player2Buttons)
        {
            butt.SetActive(false);
        }
        foreach (var button1 in Player1Buttons)
        {
            button1.SetActive(false);
        }
    }

    /// <summary>
    /// Types the line word for word based on the words that are added and changes the wordsin case the prefix gets found in the sentence. While typing the sentace out.
    /// </summary>
    private IEnumerator TypeLine()
    {
        foreach (string word in _words)
        {
            _wordToType = word;

            foreach (string wordToChange in changeWord)
            {
                if (word == changeWord[0]) //#tijd#
                {
                    _wordToType = string.Empty;
                    string l_timeOnBase = _driverTimeOnBase;
                    _wordToType += l_timeOnBase;
                    break;
                }
                else if (word == changeWord[2])//#naam#
                {
                    _wordToType = string.Empty;
                    string l_driverName = driverName + " " + DriverSecondName;
                    _wordToType += l_driverName;
                    break;
                }
                else if (word == changeWord[3])//#bezoekNaam#
                {
                    _wordToType = string.Empty;
                    string l_guestName = _driverGuestRank + " " + _driverGuestName + " " + _driverGuestLastName;
                    _wordToType += l_guestName;
                    break;
                }
                else if (word == changeWord[4])//#gebouw#
                {
                    _wordToType = string.Empty;
                    string l_building = _buildingDefensie;
                    _wordToType += l_building;
                    break;
                }
                else if (word == changeWord[5])//#AfdelingOfLocatie#
                {
                    _wordToType = string.Empty;
                    string l_afdeling = _afdeling;
                    _wordToType += l_afdeling;
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

    /// <summary>
    /// NextLine() passes the next set of questions and checks if the driver is a quest and the dialogue needs to be cut short.
    /// </summary>
    private void NextLine()
    {
        Cursor.lockState = CursorLockMode.None;

        TextObject.SetActive(false);

        InitializeVariables();

        if (_playerLook.team == 1)
        {
            PlayerOnePapaers();
        }

        if (_playerLook.team == 1)
        {
            ActivateButtons(ItemDatabase[_index].answerbutton);
        }
        else
        {
            if (GetFirstDigit(_answerIndex) >= 3)
            {
                if (_driverManager._driverIsGuest)
                {
                    ActivateButtons(ItemDatabase[20].answerbutton);
                }
                else
                {
                    ActivateButtons(ItemDatabase[_index].answerbutton);
                }
            }
            else
            {
                ActivateButtons(ItemDatabase[_index].answerbutton);
            }
        }
    }

    /// <summary>
    /// Activates the buttons passed in the parameter playerButtons for the next user question for the dialogue.
    /// </summary>
    /// <param name="playerButtons">: A list of all the buttons that need to be turned on for the next question.
    private void ActivateButtons(List<GameObject> playerButtons)
    {
        foreach (var button in playerButtons)
        {
            button.SetActive(true);
        }
    }

    /// <summary>
    /// In this function there is an RPC request to all the other users connected to the server to syncronize all the data from the driver to the computer so player 1 can check the data.
    /// </summary>
    public void PlayerOnePapaers()
    {
        _doc.GetComponent<PhotonView>().RPC("SetPapers", RpcTarget.AllBufferedViaServer, true);
    }

    /// <summary>
    /// Ends the dialogue cycle and closes all the dialogues for the player
    /// </summary>
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