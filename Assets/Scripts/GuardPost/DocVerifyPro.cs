using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DocVerifyPro : MonoBehaviourPun
{
    [Header("User Interface")]
    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private TMP_InputField _persNrField;
    [SerializeField] private GameObject _noPersonFound;
    //insert new header for person information

    [SerializeField] private TMP_Text _naamText, _achternaamText, _leeftijfText, _geslachtText, _geboorteDatumText, _nationaliteitText, _defensiePersNoText, _errorText;
    //Insert for base string text
    [Tooltip("Base strings of the information screen")]
    private string _naamString, _achternaamString, _leeftijfString, _geslachtString, _geboorteDatumString, _nationaliteitString, _defensiePersNoString;
    //Insert new header for drivers license
    [Tooltip("The drivers license information UI")]
    [SerializeField] private TMP_Text _driversLisenceFirstName, _driversLisenceLastName, _driversLisenceBirthDate, _driversLisenceIssue, _driversLisenceExpiry, _driverBSN;

    [Header("Defensiepas User Interface")]
    [SerializeField] private TMP_Text _defesieSex;
    [SerializeField] private TMP_Text _defesieLisenceFirstName, _defesieLisenceLastName, _defesieLisenceBirthDate, _defesieLisenceIssue, _defesieLisenceExpiry, _defesiePersNr;

    [Header("Scripts")]
    [SerializeField] private DriverManager _driverManager;
    [SerializeField] private BarrierManager _entranceBarrierManager;

    public bool papers;

    private void Start()
    {
        OpenComputerApp();
    }

    public void OnNewCarEnterArea()
    {
        _driverManager = null;
        _driverManager = _entranceBarrierManager._vehicle.GetComponent<DriverManager>();
    }

    /// <summary>
    /// When the player gets the drivers papers the information is set to the computer UI.
    /// </summary>
    public void OnGetDriverPapers()
    {
        if (_driverManager._isFalsified)
        {
            _driversLisenceFirstName.text = _driverManager._givenFitstName;
            _driversLisenceLastName.text = _driverManager._givenLastName;
            _driversLisenceBirthDate.text = _driverManager._givenBirthDate;
            _driversLisenceIssue.text = _driverManager._givenIssueDate;
            _driversLisenceExpiry.text = _driverManager._givenExpiryDate;

            _defesiePersNr.text = _driverManager._givenPersNo;
            _defesieLisenceFirstName.text = _driverManager._givenFitstName;
            _defesieLisenceLastName.text = _driverManager._givenLastName;
            _defesieLisenceBirthDate.text = _driverManager._givenBirthDate;
            _defesieLisenceIssue.text = _driverManager._givenIssueDate;
            _defesieLisenceExpiry.text = _driverManager._givenExpiryDate;

            GetComponent<Hefboom>()._isCorrect = false;
            GetComponent<Hefboom>()._idCorrect = false;
            GetComponent<Hefboom>()._driverSus = true;
        }
        else
        {
            _driversLisenceFirstName.text = _driverManager._driverFirstName;
            _driversLisenceLastName.text = _driverManager._driverLastName;
            _driversLisenceBirthDate.text = _driverManager._driverBirthDate;
            _driversLisenceIssue.text = _driverManager._driverDateOfIssue;
            _driversLisenceExpiry.text = _driverManager._driverDateOfExpiry;

            _defesiePersNr.text = _driverManager._driverDefensiePersNo;
            _defesieLisenceFirstName.text = _driverManager._driverFirstName;
            _defesieLisenceLastName.text = _driverManager._driverLastName;
            _defesieLisenceBirthDate.text = _driverManager._driverBirthDate;
            _defesieLisenceIssue.text = _driverManager._driverDefensieDateOfIssue;
            _defesieLisenceExpiry.text = _driverManager._driverDefensieDateOfExpiry;
            
            GetComponent<Hefboom>()._isCorrect = true;
            GetComponent<Hefboom>()._idCorrect = true;
            GetComponent<Hefboom>()._driverSus = false;
        }
        GetComponent<Hefboom>()._illegalItems = _entranceBarrierManager._vehicle.GetComponent<ContrabandManager>()._hasContraband;
        GetComponent<Hefboom>()._lis = _entranceBarrierManager._vehicle.GetComponent<CarAI>()._licensePlate;
        _defesieSex.text = _driverManager._driverSex;
        _driverBSN.text = _driverManager._driverBSN.ToString();
    }

    public void OpenComputerApp()
    {
        _naamString = _naamText.text;
        _achternaamString = _achternaamText.text;
        _leeftijfString = _leeftijfText.text;
        _geslachtString = _geslachtText.text;
        _geboorteDatumString = _geboorteDatumText.text;
        _nationaliteitString = _nationaliteitText.text;
        _defensiePersNoString = _defensiePersNoText.text;
        _errorText.text = string.Empty;
    }

    private void Update()
    {
        if (papers == true)
        {
            OnNewCarEnterArea();
            OnGetDriverPapers();
        }
    }

    [PunRPC]
    public void SetPapers(bool l_value)
    {
        papers = l_value;
    }

    public void ZoekOpNaam()
    {
        string l_name = _driverManager._driverFirstName + " " + _driverManager._driverLastName;
        if (_nameInputField.text == l_name)
        {
            if (_driverManager._isFalsified)
            {
                Debug.Log("BSN is vals. persoon is illegaal.");
                //Give false information on drivers license and defensiepas
            }
            else
            {
                Debug.Log("BSN Klopt. Persoon Klops ook.");
            }

            _naamText.text = _naamString + " " + _driverManager._driverFirstName;
            _achternaamText.text = _achternaamString + " " + _driverManager._driverLastName;
            _leeftijfText.text = _leeftijfString + " " + _driverManager._driverAge;
            _geslachtText.text = _geslachtString + " " + _driverManager._driverSex;
            _geboorteDatumText.text = _geboorteDatumString + " " + _driverManager._driverBirthDate;
            _nationaliteitText.text = _nationaliteitString + " " + _driverManager._driverNationality;
            _defensiePersNoText.text = _defensiePersNoString + " " + _driverManager._driverDefensiePersNo;
            _errorText.text = string.Empty;
        }
        else
        {
            _noPersonFound.SetActive(true);
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

    public void ZoekOpPersNr()
    {
        string l_name = _driverManager._driverDefensiePersNo;
        if (_persNrField.text == l_name)
        {
            if (_driverManager._isFalsified)
            {
                Debug.Log("BSN is vals. persoon is illegaal.");
                //Give false information on drivers lisence and defensiepas
            }
            else
            {
                Debug.Log("BSN Klopt. Persoon Klops ook.");
            }

            _naamText.text = _naamString + " " + _driverManager._driverFirstName;
            _achternaamText.text = _achternaamString + " " + _driverManager._driverLastName;
            _leeftijfText.text = _leeftijfString + " " + _driverManager._driverAge;
            _geslachtText.text = _geslachtString + " " + _driverManager._driverSex;
            _geboorteDatumText.text = _geboorteDatumString + " " + _driverManager._driverBirthDate;
            _nationaliteitText.text = _nationaliteitString + " " + _driverManager._driverNationality;
            _defensiePersNoText.text = _defensiePersNoString + " " + _driverManager._driverDefensiePersNo;
            _errorText.text = string.Empty;
        }
        else
        {
            _noPersonFound.SetActive(true);
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

    public void ResetText()
    {
        _naamText.text = _naamString;
        _achternaamText.text = _achternaamString;
        _leeftijfText.text = _leeftijfString;
        _geslachtText.text = _geslachtString;
        _geboorteDatumText.text = _geboorteDatumString;
        _nationaliteitText.text = _nationaliteitString;
        _defensiePersNoText.text = _defensiePersNoString;
    }
}
