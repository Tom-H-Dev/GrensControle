using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DialogeManager : MonoBehaviour
{
    [Header("List")]
    [SerializeField] private List<customlist> textList = new List<customlist>();
    [SerializeField] private float textSpeed = 0.5f;
    [SerializeField] private string[] changeWord;
    [SerializeField] private string driverName;
    
    private bool _textStart = false, _check;
    private int _index, _lineIndex,indexssss, indexSes, currentLineIndex, dialogueIndex;
    public string[] _words;
    public string updatedLine, wordToType;

    public PlayerMovement _playerMovement;
    public PlayerLook _playerLook;
    public TextMeshProUGUI TextComponent;
    public GameObject Text;

    public List<GameObject> cars = new List<GameObject>();
    
    public string driverTag = "Driver";
    public float Range;

    public float timer;
    
    public float madnessTimer;
    
    public int selectedLineIndex = -1, randomIndex;
    public int CDI = -1;
    private int CLI = -1;


    public DocVerifyPro doc;

    public int selectedDialogueIndex;
    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        _index = _lineIndex = currentLineIndex = indexSes = 0;
        _textStart = false;
    }

    private void Update()
    {
        
        timer -= Time.deltaTime;
        
        if (timer <= 0)
        {
            cars.Clear();
            inCollider();    
        }

        if (cars.Count > 0)
        {
            if (madnessTimer <= 0)
            {
                madnessTimer -= Time.deltaTime;
            }
        }
       
        
        if (_textStart && Input.GetMouseButtonDown(0))
        {
            if (_check)
            {
                _check = false;
                NextLine();
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                ProcessDialogueLine();
            }
        }
    }

    private void inCollider()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Range);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(driverTag))
            {
                cars.Add(collider.gameObject);
                timer = 2;
            }
        }
    }

    public void TextStart()
    {
        TextComponent.text = string.Empty;
        foreach (var button in textList[_index].myList[currentLineIndex].Buttons)
        {
            button.SetActive(true);
        }

        
        _playerMovement.enabled = false;
        
        _playerLook.enabled = false;

        Cursor.lockState = CursorLockMode.None;
    }
    private void ProcessDialogueLine()
    {
        float minMadnessDifference = float.MaxValue;
        
        for (int i = 0; i < textList[_index].myList[_lineIndex].list.Count; i++)
        {
            float madnessDifference = Mathf.Abs(textList[_index].myList[_lineIndex].list[indexssss].madness - madnessTimer);
            
            if (madnessDifference < minMadnessDifference)
            {
                minMadnessDifference = madnessDifference;
                selectedLineIndex = randomIndex;
                indexssss = selectedDialogueIndex;
            }
        }

        if (selectedLineIndex != -1)
        {
            string selectedline = textList[_index].myList[_lineIndex].list[indexssss].lines[selectedLineIndex];
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
        TextComponent.text = string.Empty;
        _index = buttonIndex;
        _lineIndex = 0;
        currentLineIndex = 0;
        _check = false;

        dialogueIndex = FindDialogueForTeam(_playerLook.team);

        if (dialogueIndex != -1)
        {
            indexSes = dialogueIndex;
            StartDialogue();
            foreach (var button in textList[_index].myList[currentLineIndex].Buttons)
            {
                button.SetActive(false);
            }
        }
    }

    private int FindDialogueForTeam(int team)
    {
        foreach (var list in textList)
        {
            for (int i = 0; i < list.myList.Count; i++)
            {
                if (list.myList[i].TeamDialogue == team)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    public void StartDialogue()
    {
        float minMadness = float.MaxValue;

        for (int i = 0; i < textList[_index].myList[indexSes].list.Count; i++)
        {
            if (textList[_index].myList[indexSes].list[i].madness > 0 && textList[_index].myList[indexSes].list[i].madness < minMadness && textList[_index].myList[indexSes].list[i].madness >= madnessTimer)
            {
                minMadness = textList[_index].myList[indexSes].list[i].madness;
                selectedDialogueIndex = i;
            }
        }

        if (selectedDialogueIndex != -1)
        {
            Text.SetActive(true);
            randomIndex = Random.Range(0, textList[_index].myList[indexSes].list[selectedDialogueIndex].lines.Length);
            StartCoroutine(TypeLine(textList[_index].myList[indexSes].list[selectedDialogueIndex].lines[randomIndex]));
        }
    }

    private IEnumerator TypeLine(string line)
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

    private void NextLine()
    {
        Cursor.lockState = CursorLockMode.None;
        
        Text.SetActive(false);
        
        InitializeVariables();

        switch (textList[_index].myList[indexSes].keys)
        {
            case key.objective:
                booleanOn();
                break;
            case key.noObjective:
                
                break;
        }
        
        foreach (var button in textList[_index].myList[currentLineIndex].Buttons)
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

        foreach (var button in textList[_index].myList[currentLineIndex].Buttons)
        {
            button.SetActive(false);
        }
    }
    private void OnDrawGizmosSelected()
    {
        
        Gizmos.DrawWireCube(transform.position, new Vector3(Range,Range,Range));
    }
}

