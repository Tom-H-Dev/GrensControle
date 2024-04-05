using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CorrectCarManager : MonoBehaviour
{
    public static CorrectCarManager instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;
    }

    public List<LastFiveList> _lastFive = new List<LastFiveList>();
    public int _totalVehicles;
    public int _correctVehicles;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Quote))
        {
            ChangeList(false, false);
        }
    }

    public void ChangeList(bool l_wasCorrect, bool l_playerChoice)
    {
        _totalVehicles++;
        if (l_wasCorrect)
            _correctVehicles++;

        for (int i = 0; i < 5; i++)
        {
            if (i != 4)
            {
                _lastFive[i + 1]._playerChoice = _lastFive[i]._playerChoice;
                _lastFive[i + 1]._isCorrect = _lastFive[i]._isCorrect;

                _lastFive[i + 1]._licensePlateText.text = _lastFive[i]._licensePlateText.text;

                _lastFive[i + 1]._correctText.text = _lastFive[i]._correctText.text;
                _lastFive[i + 1]._correctText.color = _lastFive[i]._correctText.color;

                _lastFive[i + 1]._playerChoiceText.text = _lastFive[i]._playerChoiceText.text;
                _lastFive[i + 1]._playerChoiceText.color = _lastFive[i]._playerChoiceText.color;
            }
        }

        for (int i = 4; i > -1; i--)
        {
            if (i != 0)
            {
                _lastFive[i]._playerChoice = _lastFive[i - 1]._playerChoice;
                _lastFive[i]._isCorrect = _lastFive[i - 1]._isCorrect;

                _lastFive[i]._licensePlateText.text = _lastFive[i - 1]._licensePlateText.text;

                _lastFive[i]._correctText.text = _lastFive[i - 1]._correctText.text;
                _lastFive[i]._correctText.color = _lastFive[i - 1]._correctText.color;

                _lastFive[i]._playerChoiceText.text = _lastFive[i - 1]._playerChoiceText.text;
                _lastFive[i]._playerChoiceText.color = _lastFive[i - 1]._playerChoiceText.color;
            }

        }

        _lastFive[0]._playerChoice = l_playerChoice;
        _lastFive[0]._isCorrect = l_wasCorrect;

        _lastFive[0]._licensePlateText.text = "drivers" + Random.Range(0, 999);
        if (l_wasCorrect)
        {
            _lastFive[0]._correctText.text = "Ja";
            _lastFive[0]._correctText.color = Color.green;
        }
        else
        {
            _lastFive[0]._correctText.text = "Nee";
            _lastFive[0]._correctText.color = Color.red;
        }

        if (l_playerChoice)
        {
            _lastFive[0]._playerChoiceText.text = "Ja";
            _lastFive[0]._playerChoiceText.color = Color.green;
        }
        else
        {
            _lastFive[0]._playerChoiceText.text = "Nee";
            _lastFive[0]._playerChoiceText.color = Color.red;
        }
    }
}
