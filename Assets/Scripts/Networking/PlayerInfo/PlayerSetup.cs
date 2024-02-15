using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSetup : MonoBehaviour
{
    private PhotonView _photonView;
    //public int _playerValue;
    //public GameObject _myPlayer;
    //public GameObject _players;

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        if (!_photonView.IsMine)
        {
            var position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-2, 5));
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), position, Quaternion.identity);
            return;
        }
    }

   // [PunRPC]
   // void RPC_AddPlayer(int whichCharacter)
   // {
   //     _playerValue = whichCharacter;
   //    _players = Instantiate(PlayerInfo._playerInfo._allPlayers[whichCharacter], transform.position, transform.rotation, transform);
   //  } 
}
