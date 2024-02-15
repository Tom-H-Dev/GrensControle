using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
 
    public void OnClickCharacterPick(int whichCharacter)
    {
        if(PlayerInfo._playerInfo != null)
        {
            PlayerInfo._playerInfo._selectedPlayer = whichCharacter;
            PlayerPrefs.SetInt("MyPlayer", whichCharacter);
        }
    }
}
