using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContrabandManager : MonoBehaviour
{
    [SerializeField] List<Transform> _contrabandObjects = new List<Transform>();
    [SerializeField] List<Transform> _contrabandLocations = new List<Transform>();
    [SerializeField] GameObject[] _currentContrabandInsideVehicle;
    [SerializeField][Range(0, 100)] float contrabandChance;
    [SerializeField][Range(0, 100)] float multipleContrabandChance;
    public bool _hasContraband;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int randomContrabandChance = Random.Range(0, 100);
            if (randomContrabandChance < contrabandChance)
            {
                //print(gameObject.name + " Has contraband");
                for (int i = 0; i < _contrabandLocations.Count; i++)
                {
                    randomContrabandChance = Random.Range(0, 100);
                    if (randomContrabandChance < multipleContrabandChance)
                    {
                        GetComponent<PhotonView>().RPC("SyncContraband", RpcTarget.AllBufferedViaServer, true, i);
                    }
                }
                GetComponent<PhotonView>().RPC("SyncBools", RpcTarget.AllBufferedViaServer, true);
            }
            else GetComponent<PhotonView>().RPC("SyncBools", RpcTarget.AllBufferedViaServer, false);
        }
    }

    [PunRPC]
    private void SyncContraband(bool l_multipleContraband, int l_index)
    {
        GameObject randomContrabandObject = _contrabandObjects[Random.Range(0, _contrabandObjects.Count)].gameObject;

        Instantiate(randomContrabandObject, _contrabandLocations[l_index].position, randomContrabandObject.transform.rotation, _contrabandLocations[l_index]);

        
    }

    [PunRPC]
    private void SyncBools(bool l_hasContraband)
    {
        _hasContraband = l_hasContraband;
    }

}
