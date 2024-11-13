using Photon.Pun;
using Photon.Pun.Demo.SlotRacer.Utils;
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
        ////Randomize cantraband object
        //GameObject randomContrabandObject = _contrabandObjects[Random.Range(0, _contrabandObjects.Count)].gameObject;
        ////on every object
        //for (int i = 0; i < _contrabandLocations.Count; i++)
        //{
        //    //check if no conbtraband already
        //    if (_contrabandLocations[i].childCount == 0)
        //    {
        //        //Instantiate object
        //        Instantiate(randomContrabandObject, _contrabandLocations[i].position, (randomContrabandObject.transform.rotation * _contrabandLocations[l_index].rotation), _contrabandLocations[i]);
        //        _occupiedContrabandLocations.Add(_contrabandLocations[l_index].gameObject);
        //        _currentContrabandInsideVehicle.Add(randomContrabandObject);
        //    }
        //}

        //Randomize cantraband object
        //on every object
        print(1);
        int amountRandomItem = Random.Range(0, 4);

        print(2);
        for (int i = 0; i < amountRandomItem; i++)
        {
            print(3);
            usedIndexes.Add(GetRandomInt(0, _contrabandLocations.Count));
        }
        print(4);
        for (int i = 0; i < usedIndexes.Count; i++)
        {
            print(usedIndexes[i] + " Randomized num");
            if (_contrabandLocations[usedIndexes[i]].childCount == 0)
            {
                print(5);
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

        //while (usedIndexes.Contains(num))
        //{
        //    print("While");
        //    num = Random.Range(min, max);
        //}

        return num;
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
