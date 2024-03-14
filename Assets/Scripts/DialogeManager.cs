using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DialogeManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> buttonList = new List<GameObject>();
    
    public PlayerMovement _playerMovement;
    public PlayerLook _playerLook;
    
    [SerializeField] private float textspeed = 0.5f; //speed of the return of courotine so closer to 0 the faster it is.
    
    private int _index, _lineIndex, indexbuttons;

    [SerializeField] public List<customlist> mylist = new List<customlist>();

    private bool _textStart = false;
    private bool _check;
    
    public string[] changeWord;
    public string DriverName;
    private string[] words;
    private string updatedLine;
    private string wordToType;

    private DriverManager InfoDriver;

    
    void Start()
    {
        DriverManager InfoDriver = GetComponent<DriverManager>();
        foreach (GameObject Custom in buttonList)
        {
            Custom.SetActive(false);
        }
        InfoDriver._driverFirstName = DriverName;
    }
    public void startText(PlayerMovement l_player, PlayerLook l_look)
    {
        mylist[_index].TextComponent.text = string.Empty;
        /*foreach (GameObject custom in buttonList)
        {
            custom.SetActive(true);
        }*/
        foreach (GameObject custom in buttonList)
        {
            custom.SetActive(true);
        }

        _playerMovement = l_player;
        l_player.enabled = false;

        _playerLook = l_look;
        l_look.enabled = false;
        
        Cursor.lockState = CursorLockMode.None;
    }
    public void talk(int buttonIndex)
    {
        mylist[_index].TextComponent.text = string.Empty;
        _index = buttonIndex;
        _lineIndex = 0;
        startDailogo();
        _check = false;
        /*foreach (GameObject Custom in mylist[_index].Buttons)
        {
            Custom.SetActive(false);
        }*/
        foreach (GameObject custom in buttonList)
        {
            custom.SetActive(false);
        }
    }
    void Update()
    {
        if (_textStart == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (mylist[_index].TextComponent.text == mylist[_index].lines[_lineIndex].ToString())
                {
                    NextLine();
                }
                else if (_check == true)
                {
                    NextLine();
                }
                else
                {
                    words = mylist[_index].lines[_lineIndex].ToString().Split(' ');
                    updatedLine = " ";

                    foreach (string word in words)
                    {
                        bool ischecked = false;
                        foreach (string WordToChange in changeWord)
                        {
                            if (word == WordToChange)
                            {
                                updatedLine += DriverName + " ";
                                ischecked = true;
                                break;
                            }    
                        }

                        if (!ischecked)
                        {
                            updatedLine += word + " ";
                        }
                    }

                    _check = true;
                    mylist[_index].TextComponent.text = updatedLine.Trim();
                    StopAllCoroutines();
                }
                
            }    
        }
    }

    public void startDailogo()
    {
        mylist[_index].Text.SetActive(true);
        StartCoroutine(TypeLine(mylist[_index].lines[_lineIndex]));     
    }

    IEnumerator TypeLine(string line)
    {
        words = line.Split(' ');
        
        
        foreach (string word in words)
        {
            wordToType = word;
            
            foreach (string WordToChange in changeWord)
            {
                if (word == WordToChange)
                {
                    wordToType = DriverName;
                    break;
                }
            }
            foreach (char c in wordToType)
            {
                mylist[_index].TextComponent.text += c;
                _textStart = true;
                yield return new WaitForSeconds(textspeed);
            }
            mylist[_index].TextComponent.text += ' ';
        }
        yield return new WaitForSeconds(textspeed);
    }
    void NextLine()
    {
        _lineIndex++;
        if (_lineIndex < mylist[_index].lines.Length)
        {
            mylist[_index].TextComponent.text = string.Empty;
            StartCoroutine(TypeLine(mylist[_index].lines[_lineIndex]));
            _check = false;
        }
        else
        { 
            mylist[_index].Text.SetActive(false);
            _index = 0;
            _lineIndex = 0;
            /*foreach (GameObject button in mylist[indexbuttons].Buttons)
            {
                button.SetActive(true);
                indexbuttons += 1;
            }*/
            foreach (GameObject custom in buttonList)
            {
                custom.SetActive(true);
            }
        }
    }
    public void endDialogue()
    {
        _playerLook.enabled = true;
        _playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        
        foreach (GameObject custom in buttonList)
        {
            custom.SetActive(false);
        }
    }
}
