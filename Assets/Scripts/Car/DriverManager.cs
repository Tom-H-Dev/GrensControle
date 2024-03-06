using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverManager : MonoBehaviour
{
    [Header("Driver Information")]
    private int _driverAge;
    private float _driverLength;
    private string _driverName;
    private string _driverSex;
    private string _driverBirthDate;
    private Sprite _driverDocumentImage;
    private string _driverNationality;

    [Header("Defensie Pas")]
    private string _driverDefensieDateOfIssue;
    private string _driverDefensieDateOfExpiry;
    private string _driverDefensieDocumentNumber;
    private string _driverDefensiePersNo;
    private bool _driverIsGeust;

    [Header("Rijbewijs")]
    private string _driverDateOfIssue;
    private string _driverDateOfExpiry;
    private string _driverCity;
    private int _driverBSN;

    [Header("Random Information")]
    //private List<int> _driverAges = new List<int>()

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            RandomizeInfo();
        }
    }

    public void RandomizeInfo()
    {

    }
}
