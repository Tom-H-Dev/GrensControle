using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DocVerifyPro : MonoBehaviour
{
    [Header("User Interface")]
    [SerializeField] private TMP_InputField _bsnInputField;
    [SerializeField] private DriverManager _driverManager;
    [SerializeField] private TMP_Text _naamText, _achternaamText, _leeftijfText, _geslachtText, _geboorteDatumText, _nationaliteitText, _defensiePersNoText, _errorText;
    private string _naamString, _achternaamString, _leeftijfString, _geslachtString, _geboorteDatumString, _nationaliteitString, _defensiePersNoString;
    private void Start()
    {
        OpenComputerApp();
    }
    public void BSNInfoGetter()
    {
        int l_pared = int.Parse(_bsnInputField.text);
        if (l_pared == _driverManager._driverBSN)
        {
            if (_driverManager._isFalsified)
            {
                Debug.Log("BSN is vals. persoon is illegaal.");
                //Give false information on drivers lisence and defensiepas
            }
            else
            {
                Debug.Log("BSN Klopt. Persoon Klops ook.");

                _naamText.text = _naamString + " " + _driverManager._driverFirstName;
                _achternaamText.text = _achternaamString + " " + _driverManager._driverLastName;
                _leeftijfText.text = _leeftijfString + " " + _driverManager._driverAge;
                _geslachtText.text = _geslachtString + " " + _driverManager._driverSex;
                _geboorteDatumText.text = _geboorteDatumString + " " + _driverManager._driverBirthDate;
                _nationaliteitText.text = _nationaliteitString + " " + _driverManager._driverNationality;
                _defensiePersNoText.text = _defensiePersNoString + " " + _driverManager._driverDefensiePersNo;
                _errorText.text = string.Empty;
            }
        }
        else
        {
            _errorText.text = "Geen informatie gevonden/Niemand gevonden";
            _naamText.text = _naamString;
            _achternaamText.text = _achternaamString;
            _leeftijfText.text = _leeftijfString;
            _geslachtText.text = _geslachtString;
            _geboorteDatumText.text = _geboorteDatumString;
            _nationaliteitText.text = _nationaliteitString;
            _defensiePersNoText.text = _defensiePersNoString;
        }
    }

    public void OpenComputerApp()
    {
        _naamString =           _naamText.text;
        _achternaamString =     _achternaamText.text;
        _leeftijfString =       _leeftijfText.text;
        _geslachtString =       _geslachtText.text;
        _geboorteDatumString =  _geboorteDatumText.text;
        _nationaliteitString =  _nationaliteitText.text;
        _defensiePersNoString = _defensiePersNoText.text;
        _errorText.text =       string.Empty;
    }
}
