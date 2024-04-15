using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DialogeManager : MonoBehaviour
{
    [Header("List")]
    [SerializeField] private List<customlist> textList = new List<customlist>();

    public PlayerMovement _playerMovement;
    public PlayerLook _playerLook;

    [SerializeField] private float textSpeed = 0.5f;

    [Header("Words")]
    public string[] changeWord;

    public string driverName;
    
    public string DriverName;
    
    /*
    [Header("cameraPos")]
    [SerializeField] private GameObject MainPlayerCamera;
    [SerializeField] private GameObject CameraWhenTalking;
    //werk
    */
    
    private bool _textStart = false;
    public bool _check;
    private string[] _words;
    private string updatedLine;
    private string wordToType;

    public int _index, _lineIndex, indexSes, currentLineIndex;

    private DriverManager infoDriver;
    
    public TextMeshProUGUI TextComponent;
    public GameObject Text;

    void Start()
    {
        infoDriver = GetComponent<DriverManager>();
        foreach (var button in textList[_index].myList[currentLineIndex].Buttons)
        {
            button.SetActive(false);
        }
        infoDriver._driverFirstName = driverName;
    }

    void Update()
    {
        if (_textStart)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_check)
                {
                    _check = false;
                    NextLine();
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    if (_lineIndex < textList[_index].myList.Count && indexSes < textList[_index].myList[_lineIndex].lines.Length)
                    {
                        _words = textList[_index].myList[_lineIndex].lines[indexSes].ToString().Split(' ');
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
                    else
                    {
                        foreach (string word in _words)
                        {
                            bool isChecked = false;
                            foreach (string wordToChange in changeWord)
                            {
                                if (word == wordToChange)
                                { ;
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
            }
        }
    }

    public void StartText(PlayerMovement playerMovement, PlayerLook playerLook)
    {
        TextComponent.text = string.Empty;
        
        foreach (var button in textList[_index].myList[currentLineIndex].Buttons)
        {
            button.SetActive(true);   
        }

        _playerMovement = playerMovement;
        playerMovement.enabled = false;

        _playerLook = playerLook;
        playerLook.enabled = false;

        Cursor.lockState = CursorLockMode.None;
    }

    public void StartDialogueButton(int buttonIndex)
    {
        TextComponent.text = string.Empty;
        _index = buttonIndex;
        _lineIndex = 0;
        currentLineIndex = 0;
        _check = false;
        
        int dialogueIndex = FindDialogueForTeam(_playerLook.team);

        if (dialogueIndex != -1)
        {
            indexSes = dialogueIndex;
            StartDialogue();
            
            foreach (var button in textList[_index].myList[currentLineIndex].Buttons)
            {
                button.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("No dialogue found for the player's team.");
        }
    }

    int FindDialogueForTeam(int team)
    {
        for (int i = 0; i < textList[_index].myList.Count; i++)
        {
            if (textList[_index].myList[i].TeamDialogue == team)
            {
                return i;
            }
        }
        return -1;
    }

    public void StartDialogue()
    {
        Text.SetActive(true);
        StartCoroutine(TypeLine(textList[_index].myList[indexSes].lines[0]));
    }

    IEnumerator TypeLine(string line)
    {
        _words = line.Split(' ');

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

    void NextLine()
    {
        Cursor.lockState = CursorLockMode.None;

        if (currentLineIndex < textList[_index].myList[_lineIndex].lines.Length - 1)
        {
            currentLineIndex++;

            TextComponent.text = string.Empty;
            StartCoroutine(TypeLine(textList[_index].myList[_lineIndex].lines[currentLineIndex]));
            _check = false;
        }
        else
        {
            Text.SetActive(false);
            _index = 0;
            _lineIndex = 0;
            currentLineIndex = 0;
            indexSes = 0;
            _check = false;
            foreach (var button in textList[_index].myList[currentLineIndex].Buttons)
            {
                button.SetActive(true);
            }
        }
    }

    public void EndDialogueButton()
    {
        _playerLook.enabled = true;
        _playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;

        foreach (var button in textList[_index].myList[currentLineIndex].Buttons)
        {
            button.SetActive(false);
        }
    }
}

