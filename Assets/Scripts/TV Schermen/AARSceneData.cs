using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class AARSceneData : MonoBehaviour
{
    public static AARSceneData instance;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public List<AARData> _data = new List<AARData>();
    int numbers = 0;
    int _totalVehicles;

    [PunRPC]
    public void SetAARData(bool l_idWrong, bool l_driverSus, bool l_illigalItem, bool l_wasCarAllowed, bool l_playerChoice, bool l_wasLetThrough, string l_lis, string l_name, string l_givenName)
    {
        _totalVehicles++;
        if (l_wasCarAllowed != l_playerChoice)
        {
            numbers++;
            _data[numbers - 1]._wasIDWrong = !l_idWrong;
            _data[numbers - 1]._wasDriverSuspisious = l_driverSus;
            _data[numbers - 1]._wereIlligalItemsInCar = l_illigalItem;
            _data[numbers - 1]._wasCarAllowed = l_wasCarAllowed;
            _data[numbers - 1]._playerChoice = l_playerChoice;
            _data[numbers - 1]._wasLetThrough = l_wasLetThrough;
            _data[numbers - 1]._vehicleNumber = _totalVehicles;
            _data[numbers - 1]._licecnsePlate = l_lis;
            _data[numbers - 1]._driverName = l_name;
            _data[numbers - 1]._givenDriverName = l_givenName;
        }
        if (numbers >= 3)
            SceneManager.LoadScene("AAR");
        
    }
}
