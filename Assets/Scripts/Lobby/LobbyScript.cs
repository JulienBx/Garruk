using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LobbyScript : Photon.MonoBehaviour {

//	public string gameName = "lobby";
	public GUIStyle style;

//	private const string typeName = "oojump_garruk";
	private bool isConnecting = false;
	public Dictionary<int, string> playersName = new Dictionary<int, string>();
//	private HostData[] hostList;
	private bool attemptToPlay = false;

	private const string roomName = "RoomName";
	private RoomInfo[] roomsList;

	void Awake()
	{
//		RefreshHostList();
	}

	void Start()
	{
		style.normal.textColor = Color.red;
		PhotonNetwork.ConnectUsingSettings("0.1");
	}

	void Update()
	{
//		if (!Network.isClient && !Network.isServer) {
//			RefreshHostList();
//		}
	}

	void OnGUI()
	{


		if (playersName.Count > 0)
		{
			int i = 0;
			GUI.Label(new Rect(10, 0, 500, 50), "Liste des utilisateurs connectés");
			foreach(KeyValuePair<int, string> entry in playersName)
			{
				i++;
				if (entry.Value == ApplicationModel.username)
				{
					GUI.Label(new Rect(10, i * 30, 100, 50), entry.Value, style);
				}
				else
				{
					GUI.Label(new Rect(10, i * 30, 100, 50), entry.Value);
				}
			}
		}
		if (GUI.Button(new Rect(500, 0, 200, 50), "rejoindre un match"))
	    {
			attemptToPlay = true;
			Network.Disconnect();
			MasterServer.UnregisterHost();

		}

		//Debug.Log(PhotonNetwork.connectionStateDetailed.ToString());
	}

	void OnJoinedLobby()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room!");
		PhotonNetwork.CreateRoom(roomName + Guid.NewGuid().ToString("N"), true, true, 5);
	}
	
	void OnReceivedRoomListUpdate()
	{
		roomsList = PhotonNetwork.GetRoomList();
	}
	void OnJoinedRoom()
	{
		Debug.Log("Connected to Room");
		photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, ApplicationModel.username);
	}
	void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		photonView.RPC("RemovePlayerFromList", PhotonTargets.AllBuffered, player.ID);
	}

	void OnLeftRoom()
	{
		//photonView.RPC("RemovePlayerFromList", PhotonTargets.OthersBuffered, Network.player.guid);
	}
	
	/*private void StartServer()
	{
		Network.InitializeServer(5, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}

	private void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}
*/
	// RPC

	[RPC]
	void AddPlayerToList(int id, string loginName)
	{
		playersName.Add(id, loginName);
	}

	[RPC]
	void RemovePlayerFromList(int id)
	{
		playersName.Remove(id);
	}
	// Messages

//	void OnServerInitialized()
//	{
//		Debug.Log("serveur initialisé");
//		networkView.RPC("AddPlayerToList", RPCMode.AllBuffered, Network.player.guid, ApplicationModel.username);
//	}

//	void OnConnectedToServer()
//	{
//		Debug.Log("client connecté");
//		networkView.RPC("AddPlayerToList", RPCMode.AllBuffered, Network.player.guid, ApplicationModel.username);
//	}

//	void OnDisconnectedFromServer()
//	{
//		if (attemptToPlay)
//		{
//			Application.LoadLevel("GamePage");
//		}
//	}

//	void OnPlayerDisconnected(NetworkPlayer player)
//	{
//		networkView.RPC("RemovePlayerFromList", RPCMode.AllBuffered, player.guid);
//		//Network.RemoveRPCs(player);
//		//Network.DestroyPlayerObjects(player);
//	
//	}

//	// MasterServerEvent
//	
//	void OnMasterServerEvent(MasterServerEvent msEvent)
//	{
//		if (!attemptToPlay)
//		{
//			if (msEvent == MasterServerEvent.HostListReceived) 
//			{
//				if (!Network.isClient && !Network.isServer) {
//					hostList = MasterServer.PollHostList();
//					if (hostList.Length > 0 )
//					{
//						if (!isConnecting)
//						{
//							isConnecting = true;
//							JoinServer(hostList[0]);
//						}
//					}
//					else
//					{
//						playersName.Clear();
//						StartServer();
//					}
//				}
//			}
//		}
//	}
}
