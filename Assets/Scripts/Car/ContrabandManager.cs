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
        GameObject randomContrabandObject = _contrabandObjects[Random.Range(0, _contrabandObjects.Count)].gameObject;
        for (int i = 0; i < _contrabandLocations.Count; i++)
        {
            if (_contrabandLocations[i].childCount == 0)
            {
                Instantiate(randomContrabandObject, _contrabandLocations[i].position, (randomContrabandObject.transform.rotation * _contrabandLocations[l_index].rotation), _contrabandLocations[i]);
                _occupiedContrabandLocations.Add(_contrabandLocations[l_index].gameObject);
                _currentContrabandInsideVehicle.Add(randomContrabandObject);
            }
        }
    }

    private void CheckIfUsed()
    {

    }

    [PunRPC]
    private void SyncBools(bool l_hasContraband)
    {
        _hasContraband = l_hasContraband;
    }

}
