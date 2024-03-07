using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DriverManager : MonoBehaviour
{
    [Header("Driver Information")]
    public int _driverAge;
    public float _driverLength;
    public string _driverFirstName;
    public string _driverLastName;
    public string _driverSex;
    public string _driverBirthDate;
    public Sprite _driverDocumentImage;
    public string _driverNationality;

    [Header("Defensie Pas")]
    public string _driverDefensieDateOfIssue;
    public string _driverDefensieDateOfExpiry;
    public string _driverDefensieDocumentNumber;
    public string _driverDefensiePersNo;
    public bool _driverIsGeust;

    [Header("Drivers lisence")]
    public string _driverDateOfIssue;
    public string _driverDateOfExpiry;
    public string _driverCity;
    public int _driverBSN;

    #region radomize info lists
    [Header("Random Information")]
    private static List<int> _driverAges = new List<int>() { 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 45, 46, 47, 48, 49, 50 };
    private static List<string> _driverFirstNameMale = new List<string>() { "Pieter", "Jan", "Dirk", "Willem", "Hans", "Rutger", "Joris", "Bas", "Marco", "Frank", "Robert", "Edwin", "Patrick", "Ronald", "Daniel", "Erik", "Kevin", "Raymond", "Stefan", "Mark", "Tom" };
    private static List<string> _driverFirstNameFemale = new List<string>() { "Anna", "Marie", "Petra", "Ingrid", "Yvonne", "Bianca", "Saskia", "Linda", "Miranda", "Deborah", "Sharon", "Vanessa", "Jessica", "Samantha", "Irene", "Esther", "Nicole", "Kimberley", "Amanda", "Nathalie" };
    private static List<string> _driverLastNames = new List<string>() { "De Jong", "Jansen", "Van Dijk", "Smit", "De Vries", "Peters", "Molenaar", "Kroon", "De Bruijn", "Blok", "Visser", "Boer", "Meijer", "Bakker", "De Wit", "Dekker", "Wolf", "Kwakman", "Van den Berg", "De Haas", "Holewijn" };
    private static List<string> _driverSexes = new List<string>() { "Male", "Female" };
    private static List<string> _driverNationalities = new List<string>() { "Nederland" };
    private static List<string> _months = new List<string> { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
    private static int _thisYear = System.DateTime.Now.Year;
    #endregion

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            RandomizeInfo();
        }
    }

    public void RandomizeInfo()
    {
        //Personal information
        _driverAge = _driverAges[Random.Range(0, _driverAges.Count)];
        _driverSex = _driverSexes[Random.Range(0, _driverSexes.Count)];
        _driverLastName = _driverLastNames[Random.Range(0, _driverLastNames.Count)];
        _driverNationality = _driverNationalities[Random.Range(0, _driverNationalities.Count)];
        _driverLength = Random.Range(1.59f, 2.1f);
        _driverLength = (Mathf.Round(_driverLength * 100)) / 100.0f;

        //Birthdates
        string l_month = _months[Random.Range(0, _months.Count)];
        if (l_month == "FEB") // 28 days
            _driverBirthDate = Random.Range(0, 28) + " " + l_month + " " + (_thisYear - _driverAge);
        else if (l_month == "APR" || l_month == "JUN" || l_month == "SEP" || l_month == "NOV") // 30 days
            _driverBirthDate = Random.Range(0, 30) + " " + l_month + " " + (_thisYear - _driverAge);
        else if (l_month == "JAN" || l_month == "MAR" || l_month == "MAY" || l_month == "JUL" || l_month == "AUG" || l_month == "OCT" || l_month == "DEC") // 31 days
            _driverBirthDate = Random.Range(0, 31) + " " + l_month + " " + (_thisYear - _driverAge);

        //Driver sex
        if (_driverSex == "Male")
        {
            _driverFirstName = _driverFirstNameMale[Random.Range(0, _driverFirstNameMale.Count)];
        }
        else if (_driverSex == "Female")
        {
            _driverFirstName = _driverFirstNameFemale[Random.Range(0, _driverFirstNameFemale.Count)];
        }
        else Debug.LogError("Different sex detected that is not in the list!");

        //Drivers Lisence
        _driverBSN = Random.Range(111111111, 999999999);
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                RandomizeInfo();
            }
        }
    }
}
