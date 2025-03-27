
using UnityEngine;
using Photon.Pun;


public class NetworkManager : MonoBehaviourPunCallbacks
{



    


    void Start()
    {

        PhotonNetwork.ConnectUsingSettings();

    }



    public override void OnConnectedToMaster()
    {
        Debug.Log("Estoy dentro papu");
        PhotonNetwork.JoinRandomRoom();
    }



    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No has entrado papu");

        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 4});
    }

    public override void OnJoinedRoom()
    {
       Debug.Log("Te has metido papu");

        PhotonNetwork.Instantiate("Player 1", Vector3.up, Quaternion.identity);
    }

   
}
