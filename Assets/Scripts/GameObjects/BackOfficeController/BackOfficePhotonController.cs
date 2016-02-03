using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class BackOfficePhotonController : Photon.MonoBehaviour 
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
		string sqlLobbyFilter = "C0 = " + ApplicationModel.player.ChosenGameType;
		ApplicationModel.player.IsFirstPlayer = false;
		PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, sqlLobby, sqlLobbyFilter);
	}
	void OnPhotonRandomJoinFailed()
	{
		if(ApplicationModel.player.ChosenGameType<=2)
		{
			Debug.Log("Can't join random room! - creating a new room");
			this.CreateNewRoom ();
		}
		else
		{
			BackOfficeController.instance.joinInvitationRoomFailed();
		}
	}
	public void CreateNewRoom()
	{
		this.nbPlayers = 0;
		RoomOptions newRoomOptions = new RoomOptions();
		newRoomOptions.isOpen = true;
		newRoomOptions.isVisible = true;
		newRoomOptions.maxPlayers = 2;
		newRoomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "C0", ApplicationModel.player.ChosenGameType } }; // CO pour une partie simple
		newRoomOptions.customRoomPropertiesForLobby = new string[] { "C0" }; // C0 est récupérable dans le lobby
		
		TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);
		PhotonNetwork.CreateRoom(roomNamePrefix + Guid.NewGuid().ToString("N"), newRoomOptions, sqlLobby);
		ApplicationModel.player.IsFirstPlayer = true;
	}
	
	void OnJoinedRoom()
	{
		photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, ApplicationModel.player.Username);

		//Forcer le tuto
		ApplicationModel.player.ToLaunchGameTutorial = true ;
		
		if (ApplicationModel.player.ToLaunchGameTutorial)
		{
			print("Le tuto est lancé");
			PhotonNetwork.room.open = false;
			Application.LoadLevel("Game");
			ApplicationModel.player.IsFirstPlayer = true ;
		}
	}
	
	[RPC]
	void AddPlayerToList(int id, string loginName)
	{
		print(loginName+" se connecte");
		
		if (ApplicationModel.player.Username == loginName)
		{
			ApplicationModel.myPlayerName=loginName;
		} 
		else
		{
			ApplicationModel.hisPlayerName=loginName;
		}
		
		this.nbPlayers++;
		if(this.nbPlayers==2)
		{
			if(ApplicationModel.player.IsFirstPlayer==true)
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

