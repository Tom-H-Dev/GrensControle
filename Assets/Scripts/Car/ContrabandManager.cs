using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContrabandManager : MonoBehaviour
{
    [SerializeField] List<GameObject> _contrabandObjects = new List<GameObject>();
    [SerializeField] List<Transform> _contrabandLocations = new List<Transform>();
    [SerializeField] List<Transform> _contrabandContainerObjects = new List<Transform>();
    [SerializeField] List<Transform> _contrabandContainerLocations = new List<Transform>();

    [SerializeField] List<GameObject> _currentContrabandInsideVehicle = new List<GameObject>();
    [SerializeField] List<GameObject> _occupiedContrabandLocations = new List<GameObject>();

    [SerializeField][Range(0, 100)] float contrabandChance;
    public bool _hasContraband;
    public int _latestContrabandIndex;
    public List<int> usedIndexes = new List<int>();
    public List<int> _contrabandItems = new List<int>();
    public int randomContraCount;

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
        randomContraCount = Random.Range(0, 4);
        for (int i = 0; i < randomContraCount; i++)
        {
            //usedIndexes.Add(GetRandomInt(0, _contrabandLocations.Count));
            GetComponent<PhotonView>().RPC("SetUserIndexes", RpcTarget.AllBufferedViaServer, GetRandomInt(0, _contrabandLocations.Count));
            GetComponent<PhotonView>().RPC("SetContrabandObjects", RpcTarget.AllBufferedViaServer, GetRandomInt(0, _contrabandObjects.Count));
        }
        GetComponent<PhotonView>().RPC("GenerateContrabandSpawn", RpcTarget.AllBufferedViaServer);
    }
    [PunRPC]
    private void GenerateContrabandSpawn()
    {
        if (_hasContraband)
        {
            for (int i = 0; i < randomContraCount; i++)
            {
                if (_contrabandLocations[usedIndexes[i]].childCount == 0)
                {
                    GetComponent<PhotonView>().RPC("SpawnContraband", RpcTarget.AllBufferedViaServer, i);
                }
            }
        }
    }

    [PunRPC]
    private void SetUserIndexes(int i)
    {
        usedIndexes.Add(i);
    }

    [PunRPC]
    private void SetContrabandObjects(int i)
    {
        _contrabandItems.Add(i);
    }

    private int GetRandomInt(int min, int max)
    {
        int num = Random.Range(min, max);

        return num;
    }
    [PunRPC]
    private void SpawnContraband(int l_positionIndex)
    {
        if (_contrabandLocations[usedIndexes[l_positionIndex]].childCount == 0)
        {
            GameObject l_contrband = PhotonNetwork.InstantiateRoomObject(_contrabandObjects[_contrabandItems[l_positionIndex]].name, _contrabandLocations[usedIndexes[l_positionIndex]].position, (_contrabandObjects[_contrabandItems[l_positionIndex]].transform.rotation * _contrabandLocations[usedIndexes[l_positionIndex]].rotation));
            l_contrband.transform.SetParent(_contrabandLocations[usedIndexes[l_positionIndex]]);
            _occupiedContrabandLocations.Add(_contrabandLocations[usedIndexes[l_positionIndex]].gameObject);
            _currentContrabandInsideVehicle.Add(_contrabandObjects[_contrabandItems[l_positionIndex]].gameObject);
        }
    }


    [PunRPC]
    private void SyncBools(bool l_hasContraband)
    {
        _hasContraband = l_hasContraband;
    }

}
