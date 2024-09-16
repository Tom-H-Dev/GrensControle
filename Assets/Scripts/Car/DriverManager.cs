using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Header("Defensiepas")]
    public string _driverDefensieDateOfIssue;
    public string _driverDefensieDateOfExpiry;
    public string _driverDefensieDocumentNumber;
    public string _driverDefensiePersNo;
    public bool _driverIsGeust;

    [Header("Defensie Information")]
    public string _driverRank;
    public string _guestpersonFirstName;
    public string _guestpersonLastName;
    public string _guestPersonRank;
    public string _workBuilding;
    public string _timeOnBase;

    [Header("Drivers lisence")]
    public string _driverDateOfIssue;
    public string _driverDateOfExpiry;
    public int _driverBSN;

    [Header("Falsified")]
    public bool _isFalsified = false;
    [Range(0, 100)]
    public float _falsifiedPercentage = 20;

    [Header("Given Information")] //This is the inforamtion given by the driver if they have falsified information
    public string _givenFitstName;
    public string _givenLastName;
    public string _givenBirthDate;
    public string _givenNationality;
    public string _givenPersNo;
    public string _givenDocumentNo;
    public string _givenIssueDateDefensie;
    public string _givenExpiryDateDefensie;
    public string _givenIssueDate;
    public string _givenExpiryDate;
    public int _givenBSN;
    public string _givenDriverRank;

    [Header("Driver Models")]
    [SerializeField] private GameObject _driverMale;
    [SerializeField] private GameObject _driverFemale;

    #region radomize info lists
    [Header("Random Information")]
    private static List<int> _driverAges = new List<int>() { 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 45, 46, 47, 48, 49, 50 };
    private static List<string> _driverFirstNameMale = new List<string>() { "Pieter", "Jan", "Dirk", "Willem", "Hans", "Rutger", "Joris", "Bas", "Marco", "Frank", "Robert", "Edwin", "Patrick", "Ronald", "Daniel", "Erik", "Kevin", "Raymond", "Stefan", "Mark", "Tom", "Ahmad", "Mohammed", "Abdul", "Hichem", "Yeshir", "Finn", "Ruben", "Zhahir", "Vins", "Rico", "Bon", "Luuk", "Tim" };
    private static List<string> _driverFirstNameFemale = new List<string>() { "Anna", "Marie", "Petra", "Ingrid", "Yvonne", "Bianca", "Saskia", "Linda", "Miranda", "Deborah", "Sharon", "Vanessa", "Jessica", "Samantha", "Irene", "Esther", "Nicole", "Kimberley", "Amanda", "Nathalie", "Luca", "Kim" };
    private static List<string> _driverLastNames = new List<string>() { "De Jong", "Jansen", "Van Dijk", "Smit", "De Vries", "Peters", "Molenaar", "Kroon", "De Bruijn", "Blok", "Visser", "Boer", "Meijer", "Bakker", "De Wit", "Dekker", "Wolf", "Kwakman", "Van den Berg", "De Haas", "Holewijn", "Kortekaas", "Lambooij", "Kossen", "Krijgsman", "Alkaf", "Pol", "Verhoeven", "Hollemans","Veken", "Tkachov" };
    private static List<string> _driverSexes = new List<string>() { "Male", "Female" };
    private static List<string> _driverNationalities = new List<string>() { "Nederland" };
    private static List<string> _months = new List<string> { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
    private static List<char>   _buildingNames = Enumerable.Range('A', 26).Select(x => (char)x).Concat(Enumerable.Range('a', 26).Select(x => (char)x)).ToList();
    private static List<string> _guestPersonFirstNames = new List<string>() { "Pieter", "Jan", "Dirk", "Willem", "Hans", "Rutger", "Joris", "Bas", "Marco", "Frank", "Robert", "Edwin", "Patrick", "Ronald", "Daniel", "Erik", "Kevin", "Raymond", "Stefan", "Mark", "Tom", "Ahmad", "Mohammed", "Abdul", "Hichem", "Yeshir", "Finn", "Ruben", "Zhahir", "Vins", "Rico" };
    private static List<string> _guestPersonLastNames = new List<string>{ "De Jong", "Jansen", "Van Dijk", "Smit", "De Vries", "Peters", "Molenaar", "Kroon", "De Bruijn", "Blok", "Visser", "Boer", "Meijer", "Bakker", "De Wit", "Dekker", "Wolf", "Kwakman", "Van den Berg", "De Haas", "Holewijn", "Kortekaas", "Lambooij", "Kossen", "Krijgsman", "Alkaf", "Pol", "Verhoeven" };
    private static List<string> _driverRanks = new List<string> { "Sergeant", "Sergeant-Majoor", "Adjudant", "Adjudant-Onderofficier", "Tweede Luitenant", "Eerste Luitenant", "Kapitein", "Majoor", "Luitenant-Kolonel", "Kolonel", "Brigadegeneraal", "Generaal-Majoor", "Luitenant-Generaal", "Generaal" };
    private static List<string> _timeOnBaseTimes = new List<string>{"vijftien minuten", "dertig minuten", "vijfenveertig minuten", "één uur", "één uur en vijftien minuten", "één uur en dertig minuten", "één uur en vijfenveertig minuten", "twee uur", "twee uur en vijftien minuten", "twee uur en dertig minuten", "twee uur en vijfenveertig minuten", "drie uur", "drie uur en vijftien minuten", "drie uur en dertig minuten", "drie uur en vijfenveertig minuten","vier uur", "vier uur en vijftien minuten", "vier uur en dertig minuten", "vier uur en vijfenveertig minuten","vijf uur", "vijf uur en vijftien minuten", "vijf uur en dertig minuten", "vijf uur en vijfenveertig minuten","zes uur", "zes uur en vijftien minuten", "zes uur en dertig minuten", "zes uur en vijfenveertig minuten","zeven uur", "zeven uur en vijftien minuten", "zeven uur en dertig minuten", "zeven uur en vijfenveertig minuten","acht uur", "acht uur en vijftien minuten", "acht uur en dertig minuten", "acht uur en vijfenveertig minuten","negen uur", "negen uur en vijftien minuten", "negen uur en dertig minuten", "negen uur en vijfenveertig minuten","tien uur"};

    private static int _thisYear = System.DateTime.Now.Year;
    #endregion

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GenerateData();
        }
    }

    private void GenerateData()
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
            _driverBirthDate = Random.Range(1, 28) + " " + l_month + " " + (_thisYear - _driverAge);
        else if (l_month == "APR" || l_month == "JUN" || l_month == "SEP" || l_month == "NOV") // 30 days
            _driverBirthDate = Random.Range(1, 30) + " " + l_month + " " + (_thisYear - _driverAge);
        else if (l_month == "JAN" || l_month == "MAR" || l_month == "MEI" || l_month == "JUL" || l_month == "AUG" || l_month == "OCT" || l_month == "DEC") // 31 days
            _driverBirthDate = Random.Range(1, 31) + " " + l_month + " " + (_thisYear - _driverAge);

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

        _guestpersonFirstName = _guestPersonFirstNames[Random.Range(0, _guestPersonFirstNames.Count)];
        _guestpersonLastName = _guestPersonLastNames[Random.Range(0, _guestPersonLastNames.Count)];
        _guestPersonRank = _driverRanks[Random.Range(0, _driverRanks.Count)];
        _workBuilding = _buildingNames[Random.Range(0, _buildingNames.Count)].ToString();
        _timeOnBase = _timeOnBaseTimes[Random.Range(0, _timeOnBaseTimes.Count)];

        float l_r = Random.Range(0, 100);
        l_month = _months[Random.Range(0, _months.Count)];
        int l_day = 0;
        int l_yearsAgo = Random.Range(1, 6);
        if (l_r < _falsifiedPercentage) //fasle info  
        {
            _isFalsified = true;
            //issue date
            _driverRank = _driverRanks[Random.Range(0, _driverRanks.Count)];


            if (l_month == "FEB") // 28 days
                l_day = Random.Range(1, 28);
            else if (l_month == "APR" || l_month == "JUN" || l_month == "SEP" || l_month == "NOV")  // 30 days
                l_day = Random.Range(1, 30);
            else if (l_month == "JAN" || l_month == "MAA" || l_month == "MEI" || l_month == "JUL" || l_month == "AUG" || l_month == "OCT" || l_month == "DEC") // 31 days
                l_day = Random.Range(1, 31);

            _driverDateOfIssue = l_day + " " + l_month + " " + (_thisYear - l_yearsAgo - 10);
            _driverDateOfExpiry = l_day + " " + l_month + " " + (_thisYear + (10 - l_yearsAgo - 10));

            //Defensiepas
            //issue date
            if (!_driverIsGeust)
            {
                l_month = _months[Random.Range(0, _months.Count)];
                l_yearsAgo = Random.Range(1, 2);

                if (l_month == "FEB") // 28 days
                    l_day = Random.Range(1, 28);
                else if (l_month == "APR" || l_month == "JUN" || l_month == "SEP" || l_month == "NOV")  // 30 days
                    l_day = Random.Range(1, 30);
                else if (l_month == "JAN" || l_month == "MAR" || l_month == "MEI" || l_month == "JUL" || l_month == "AUG" || l_month == "OCT" || l_month == "DEC") // 31 days
                    l_day = Random.Range(1, 31);

                _driverDefensieDateOfIssue = l_day + " " + l_month + " " + (_thisYear - l_yearsAgo - 10);
                _driverDefensieDateOfExpiry = l_day + " " + l_month + " " + (_thisYear + (3 - l_yearsAgo - 10));

                _driverDefensiePersNo = (int)Random.Range(111111, 999999) + "";
                _driverDefensieDocumentNumber = (int)Random.Range(1, 9) + " " + (int)Random.Range(111, 999) + " " + (int)Random.Range(1111, 9999) + "-" + (int)Random.Range(1111, 9999) + " " + (int)Random.Range(1111, 9999) + " " + (int)Random.Range(1111, 9999);
            }
        }
        else //not false
        {

            _driverRank = _driverRanks[Random.Range(0, _driverRanks.Count)];
            
            //issue date

            if (l_month == "FEB") // 28 days
                l_day = Random.Range(1, 28);
            else if (l_month == "APR" || l_month == "JUN" || l_month == "SEP" || l_month == "NOV")  // 30 days
                l_day = Random.Range(1, 30);
            else if (l_month == "JAN" || l_month == "MAA" || l_month == "MEI" || l_month == "JUL" || l_month == "AUG" || l_month == "OCT" || l_month == "DEC") // 31 days
                l_day = Random.Range(1, 31);

            _driverDateOfIssue = l_day + " " + l_month + " " + (_thisYear - l_yearsAgo);
            _driverDateOfExpiry = l_day + " " + l_month + " " + (_thisYear + (10 - l_yearsAgo));

            //Defensiepas
            //issue date
            if (!_driverIsGeust)
            {
                l_month = _months[Random.Range(0, _months.Count)];
                l_yearsAgo = Random.Range(1, 2);

                if (l_month == "FEB") // 28 days
                    l_day = Random.Range(1, 28);
                else if (l_month == "APR" || l_month == "JUN" || l_month == "SEP" || l_month == "NOV")  // 30 days
                    l_day = Random.Range(1, 30);
                else if (l_month == "JAN" || l_month == "MAR" || l_month == "MEI" || l_month == "JUL" || l_month == "AUG" || l_month == "OCT" || l_month == "DEC") // 31 days
                    l_day = Random.Range(1, 31);

                _driverDefensieDateOfIssue = l_day + " " + l_month + " " + (_thisYear - l_yearsAgo);
                _driverDefensieDateOfExpiry = l_day + " " + l_month + " " + (_thisYear + (3 - l_yearsAgo));

                _driverDefensiePersNo = (int)Random.Range(111111, 999999) + "";
                _driverDefensieDocumentNumber = (int)Random.Range(1, 9) + " " + (int)Random.Range(111, 999) + " " + (int)Random.Range(1111, 9999) + "-" + (int)Random.Range(1111, 9999) + " " + (int)Random.Range(1111, 9999) + " " + (int)Random.Range(1111, 9999);
            }
        }

        //random cahnce to be false
        l_r = Random.Range(0, 100);
        if (l_r < _falsifiedPercentage)
        {
            //Debug.Log("False Inforamtion");
            _isFalsified = true;

            //First and last name
            if (_driverSex == "Male")
            {
                _givenFitstName = _driverFirstNameMale[Random.Range(0, _driverFirstNameMale.Count)];
            }
            else if (_driverSex == "Female")
            {
                _givenFitstName = _driverFirstNameFemale[Random.Range(0, _driverFirstNameFemale.Count)];
            }
            else Debug.LogError("Different sex detected that is not in the list!");
            _givenLastName = _driverLastNames[Random.Range(0, _driverLastNames.Count)];

            //Birthdates
            l_month = _months[Random.Range(0, _months.Count)];
            if (l_month == "FEB") // 28 days
                _givenBirthDate = Random.Range(1, 28) + " " + l_month + " " + (_thisYear - _driverAge);
            else if (l_month == "APR" || l_month == "JUN" || l_month == "SEP" || l_month == "NOV") // 30 days
                _givenBirthDate = Random.Range(1, 30) + " " + l_month + " " + (_thisYear - _driverAge);
            else if (l_month == "JAN" || l_month == "MAR" || l_month == "MEI" || l_month == "JUL" || l_month == "AUG" || l_month == "OCT" || l_month == "DEC") // 31 days
                _givenBirthDate = Random.Range(1, 31) + " " + l_month + " " + (_thisYear - _driverAge);

            //Defensie document no
            _givenNationality = _driverNationalities[Random.Range(0, _driverNationalities.Count)];
            _givenPersNo = (int)Random.Range(111111, 999999) + "";
            _givenDocumentNo = (int)Random.Range(1, 9) + " " + (int)Random.Range(111, 999) + " " + (int)Random.Range(1111, 9999) + "-" + (int)Random.Range(1111, 9999) + " " + (int)Random.Range(1111, 9999) + " " + (int)Random.Range(1111, 9999);

            //Defensie document dates
            l_month = _months[Random.Range(0, _months.Count)];
            l_yearsAgo = Random.Range(1, 2);

            if (l_month == "FEB") // 28 days
                l_day = Random.Range(1, 28);
            else if (l_month == "APR" || l_month == "JUN" || l_month == "SEP" || l_month == "NOV")  // 30 days
                l_day = Random.Range(1, 30);
            else if (l_month == "JAN" || l_month == "MAR" || l_month == "MEI" || l_month == "JUL" || l_month == "AUG" || l_month == "OCT" || l_month == "DEC") // 31 days
                l_day = Random.Range(1, 31);

            _givenIssueDateDefensie = l_day + " " + l_month + " " + (_thisYear - l_yearsAgo);
            _givenExpiryDateDefensie = l_day + " " + l_month + " " + (_thisYear + (3 - l_yearsAgo));

            l_month = _months[Random.Range(0, _months.Count)];
            l_day = 0;
            l_yearsAgo = Random.Range(1, 6);

            if (l_month == "FEB") // 28 days
                l_day = Random.Range(1, 28);
            else if (l_month == "APR" || l_month == "JUN" || l_month == "SEP" || l_month == "NOV")  // 30 days
                l_day = Random.Range(1, 30);
            else if (l_month == "JAN" || l_month == "MAA" || l_month == "MEI" || l_month == "JUL" || l_month == "AUG" || l_month == "OCT" || l_month == "DEC") // 31 days
                l_day = Random.Range(1, 31);

            _givenIssueDate = l_day + " " + l_month + " " + (_thisYear - l_yearsAgo);
            _givenExpiryDate = l_day + " " + l_month + " " + (_thisYear + (10 - l_yearsAgo));

            _givenBSN = Random.Range(111111111, 999999999);
        }
        else
        {
            _givenFitstName = string.Empty;
            _givenLastName = string.Empty;
            _givenBirthDate = string.Empty;
            _givenNationality = string.Empty;
            _givenPersNo = string.Empty;
            _givenDocumentNo = string.Empty;
            _givenIssueDateDefensie = string.Empty;
            _givenExpiryDateDefensie = string.Empty;
            _givenIssueDate = string.Empty;
            _givenExpiryDate = string.Empty;
            _givenBSN = 0;
            _isFalsified = false;
        }

        GetComponent<PhotonView>().RPC("UpdateDataOverNetwork", RpcTarget.AllBufferedViaServer, _driverAge, _driverLength, _driverFirstName,
                                                                                                _driverLastName, _driverSex, _driverBirthDate,
                                                                                                _driverNationality, _driverDefensieDateOfIssue,
                                                                                                _driverDefensieDateOfExpiry, _driverDefensieDocumentNumber, _driverDefensiePersNo,
                                                                                                _driverIsGeust, _driverDateOfIssue, _driverDateOfExpiry,
                                                                                                _driverBSN, _isFalsified, _falsifiedPercentage,
                                                                                                _givenFitstName, _givenLastName, _givenBirthDate,
                                                                                                _givenNationality, _givenPersNo, _givenDocumentNo,
                                                                                                _givenIssueDateDefensie, _givenExpiryDateDefensie, _givenIssueDate,
                                                                                                _givenExpiryDate, _givenBSN, _driverRank, _givenDriverRank, _guestpersonFirstName,
                                                                                                _guestpersonLastName, _guestPersonRank, _workBuilding, _timeOnBase);
    }

    [PunRPC]
    public void UpdateDataOverNetwork(int l_driverAge, float l_driverLength, string l_driverFirstName,
                                      string l_driverLastName, string l_driverSex, string l_driverBirthDate,
                                      string l_driverNationality, string l_driverDefensieDateOfIssue,
                                      string l_driverDefensieDateOfExpiry, string l_driverDefensieDocumentNumber, string l_driverDefensiePersNo,
                                      bool l_driverIsGeust, string l_driverDateOfIssue, string l_driverDateOfExpiry,
                                      int l_driverBSN, bool l_isFalsified, float l_falsifiedPercentage,
                                      string l_givenFitstName, string l_givenLastName, string l_givenBirthDate,
                                      string l_givenNationality, string l_givenPersNo, string l_givenDocumentNo,
                                      string l_givenIssueDateDefensie, string l_givenExpiryDateDefensie, string l_givenIssueDate,
                                      string l_givenExpiryDate, int l_givenBSN, string l_driverRank, string l_givenDriverRank,
                                      string l_guestPersonName, string l_guestPersonLastName, string l_guestPersonRank, string l_workBuilding,
                                      string l_timeOnBase)
    {
        _driverAge =                        l_driverAge;
        _driverLength =                     l_driverLength;
        _driverFirstName =                  l_driverFirstName;
        _driverLastName =                   l_driverLastName;
        _driverSex =                        l_driverSex;
        _driverBirthDate =                  l_driverBirthDate;
        _driverNationality =                l_driverNationality;
        _driverDefensieDateOfIssue =        l_driverDefensieDateOfIssue;
        _driverDefensieDateOfExpiry =       l_driverDefensieDateOfExpiry;
        _driverDefensieDocumentNumber =     l_driverDefensieDocumentNumber;
        _driverDefensiePersNo =             l_driverDefensiePersNo;
        _driverIsGeust =                    l_driverIsGeust;
        _driverDateOfIssue =                l_driverDateOfIssue;
        _driverDateOfExpiry =               l_driverDateOfExpiry;
        _driverBSN =                        l_driverBSN;
        _isFalsified =                      l_isFalsified;
        _falsifiedPercentage =              l_falsifiedPercentage;
        _givenFitstName =                   l_givenFitstName;
        _givenLastName =                    l_givenLastName;
        _givenBirthDate =                   l_givenBirthDate;
        _givenNationality =                 l_givenNationality;
        _givenPersNo =                      l_givenPersNo;
        _givenDocumentNo =                  l_givenDocumentNo;
        _givenIssueDateDefensie =           l_givenIssueDateDefensie;
        _givenExpiryDateDefensie =          l_givenExpiryDateDefensie;
        _givenIssueDate =                   l_givenIssueDate;
        _givenExpiryDate =                  l_givenExpiryDate;
        _givenBSN =                         l_givenBSN;
        _driverRank =                       l_driverRank;
        _givenDriverRank =                  l_givenDriverRank;
        _guestpersonFirstName =             l_guestPersonName;
        _guestpersonLastName =              l_guestPersonLastName;
        _guestPersonRank =                  l_guestPersonRank;
        _workBuilding =                     l_workBuilding;
        _timeOnBase =                       l_timeOnBase;
    }

    [PunRPC]
    private void SetDriverModels()
    {
        if (_driverSex == "Male")
        {
            _driverMale.SetActive(true);
            _driverFemale.SetActive(false);
        }
        else
        {
            _driverMale.SetActive(false);
            _driverFemale.SetActive(true);
        }
    }
}
