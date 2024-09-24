using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo _playerInfo;

    public int _selectedPlayer;

    public GameObject[] _allPlayers;

    private void OnEnable()
    {
        if (PlayerInfo._playerInfo == null)
        {
            PlayerInfo._playerInfo = this;
        }
        else
        {
            if(PlayerInfo._playerInfo != this)
            {
                Destroy(PlayerInfo._playerInfo.gameObject);
                PlayerInfo._playerInfo = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MyPlayer"))
        {
            _selectedPlayer = PlayerPrefs.GetInt("MyPlayer");
        }
        else
        {
            _selectedPlayer = 0;
            PlayerPrefs.SetInt("MyPlayer", _selectedPlayer);
        }
    }
}
