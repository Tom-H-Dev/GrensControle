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
    [SerializeField] List<GameObject> _occupiedContrabandLocations = new List<GameObject>();

    [SerializeField][Range(0, 100)] float contrabandChance;
    public bool _hasContraband;
    public List<int> usedIndexes = new List<int>();

    private void Start()
    {
        GenerateContraband();
    }

    private void GenerateContraband()
    {
        if (PhotonNetwork.IsMasterClient) // kies role voordat je speelt
        {
            int randomContrabandChance = Random.Range(0, 100);

            if (randomContrabandChance < contrabandChance)
            {
                GetComponent<PhotonView>().RPC("SyncContraband", RpcTarget.AllBufferedViaServer, true, Random.Range(0, _contrabandLocations.Count));
                GetComponent<PhotonView>().RPC("SyncBools", RpcTarget.AllBufferedViaServer, true);
            }
            else GetComponent<PhotonView>().RPC("SyncBools", RpcTarget.AllBufferedViaServer, false);
        }
    }

    [PunRPC]
    private void SyncContraband(bool l_multipleContraband, int l_index)
    {
        int amountRandomItem = Random.Range(0, 4);
        for (int i = 0; i < amountRandomItem; i++)
        {
            usedIndexes.Add(GetRandomInt(0, _contrabandLocations.Count));
        }
        for (int i = 0; i < usedIndexes.Count; i++)
        {
            if (_contrabandLocations[usedIndexes[i]].childCount == 0)
            {
                GameObject randomContrabandObject = _contrabandObjects[Random.Range(0, _contrabandObjects.Count)].gameObject;
                //Instantiate object
                Instantiate(randomContrabandObject, _contrabandLocations[usedIndexes[i]].position, (randomContrabandObject.transform.rotation * _contrabandLocations[usedIndexes[i]].rotation), _contrabandLocations[usedIndexes[i]]);
                _occupiedContrabandLocations.Add(_contrabandLocations[usedIndexes[i]].gameObject);
                _currentContrabandInsideVehicle.Add(randomContrabandObject);
            }
        }
    }
    private int GetRandomInt(int min, int max)
    {
        int num = Random.Range(min, max);

        return num;
    }

    [PunRPC]
    private void SyncBools(bool l_hasContraband)
    {
        _hasContraband = l_hasContraband;
    }

}
