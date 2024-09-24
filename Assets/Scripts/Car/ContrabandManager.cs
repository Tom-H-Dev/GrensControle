using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContrabandManager : MonoBehaviour
{
    [SerializeField] List<Transform> _contrabandObjects = new List<Transform>();
    [SerializeField] List<Transform> _contrabandLocations = new List<Transform>();
    [SerializeField] List<Transform> _contrabandContainerObjects = new List<Transform>();
    [SerializeField] List<Transform> _contrabandContainerLocations = new List<Transform>();
    [SerializeField] List<GameObject> _currentContrabandInsideVehicle = new List<GameObject>();
    [SerializeField][Range(0, 100)] float contrabandChance;
    [SerializeField][Range(0, 100)] float multipleContrabandChance;
    public bool _hasContraband;

    private void Start()
    {
        GenerateContraband();
    }

    private void GenerateContraband()
    {
        if (PhotonNetwork.IsMasterClient) // kies role voordat je speelt
        {
            print("Is master client");
            
            int randomContrabandChance = Random.Range(0, 100);
            if (randomContrabandChance < contrabandChance)
            {
                print(gameObject.name + " Has contraband");
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
        GameObject randomContrabandObject;

        do
        {
            randomContrabandObject = _contrabandObjects[Random.Range(0, _contrabandObjects.Count)].gameObject;
        }
        while (_currentContrabandInsideVehicle.Contains(randomContrabandObject));

        Instantiate(randomContrabandObject, _contrabandLocations[l_index].position, randomContrabandObject.transform.rotation, _contrabandLocations[l_index]);
        _currentContrabandInsideVehicle.Add(randomContrabandObject);  
    }

    [PunRPC]
    private void SyncBools(bool l_hasContraband)
    {
        _hasContraband = l_hasContraband;
    }

}
