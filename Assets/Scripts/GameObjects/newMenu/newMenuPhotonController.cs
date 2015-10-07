using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class newMenuPhotonController : Photon.MonoBehaviour 
{

	public const string roomNamePrefix = "GarrukGame";
	private int nbPlayers;

	public void leaveRoom()
	{
		PhotonNetwork.LeaveRoom ();
	}
	public void joinRandomRoom()
	{
		this.nbPlayers = 0;
		TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);    
		string sqlLobbyFilter = "C0 = " + ApplicationModel.gameType;
		PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, sqlLobby, sqlLobbyFilter);
	}
	void OnPhotonRandomJoinFailed()
	{
		if(ApplicationModel.gameType<=2)
		{
			Debug.Log("Can't join random room! - creating a new room");
			this.CreateNewRoom ();
		}
		else
		{
			newMenuController.instance.joinInvitationRoomFailed();
		}
	}
	public void CreateNewRoom()
	{
		this.nbPlayers = 0;
		RoomOptions newRoomOptions = new RoomOptions();
		newRoomOptions.isOpen = true;
		newRoomOptions.isVisible = true;
		newRoomOptions.maxPlayers = 2;
		newRoomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "C0", ApplicationModel.gameType } }; // CO pour une partie simple
		newRoomOptions.customRoomPropertiesForLobby = new string[] { "C0" }; // C0 est récupérable dans le lobby
		
		TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);
		PhotonNetwork.CreateRoom(roomNamePrefix + Guid.NewGuid().ToString("N"), newRoomOptions, sqlLobby);
		ApplicationModel.isFirstPlayer = true;
	}
	void OnJoinedRoom()
	{
		photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, ApplicationModel.username);
		if (ApplicationModel.launchGameTutorial)
		{
			photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID + 1, "Garruk");
			PhotonNetwork.room.open = false;
			Application.LoadLevel("Game");
		}
	}
	
	[RPC]
	void AddPlayerToList(int id, string loginName)
	{
		print(loginName+" se connecte");
		
		if (ApplicationModel.username == loginName)
		{
			//GameView.instance.setMyPlayerName(loginName);
			ApplicationModel.myPlayerName=loginName;
			//print (myPlayerName);
		} 
		else
		{
			//GameView.instance.setHisPlayerName(loginName);
			ApplicationModel.hisPlayerName=loginName;
			//print (hisPlayerName);
		}
		
		this.nbPlayers++;
		if(this.nbPlayers==2)
		{
			if(ApplicationModel.isFirstPlayer==true)
			{
				PhotonNetwork.room.open = false;
			}
			Application.LoadLevel("Game");
		}
	}
	void OnDisconnectedFromPhoton()
	{
		Application.LoadLevel("Authentication");
	}

}

