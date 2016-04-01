using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine.SceneManagement;

public class BackOfficePhotonController : Photon.MonoBehaviour 
{
	
	public const string roomNamePrefix = "GarrukGame";
	private int nbPlayers;
	private int deckLoaded;
	
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
		PhotonNetwork.JoinRandomRoom(null, 0, ExitGames.Client.Photon.MatchmakingMode.FillRoom, sqlLobby, sqlLobbyFilter);
	}
	void OnPhotonRandomJoinFailed()
	{
		if(ApplicationModel.player.ChosenGameType<=20)
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
		ApplicationModel.player.IsFirstPlayer = true;
		this.nbPlayers = 0;
		RoomOptions newRoomOptions = new RoomOptions();
		newRoomOptions.isOpen = true;
		newRoomOptions.isVisible = true;
		newRoomOptions.maxPlayers = 2;
		newRoomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "C0", ApplicationModel.player.ChosenGameType } }; // CO pour une partie simple
		newRoomOptions.customRoomPropertiesForLobby = new string[] { "C0" }; // C0 est récupérable dans le lobby
		
		TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);
		PhotonNetwork.CreateRoom(roomNamePrefix + Guid.NewGuid().ToString("N"), newRoomOptions, sqlLobby);
	}
	
	void OnJoinedRoom()
	{
<<<<<<< HEAD
=======
		ApplicationModel.player.ToLaunchGameTutorial = true ;
>>>>>>> origin/master
		photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, ApplicationModel.player.Username);
	}
	
	[PunRPC]
	IEnumerator AddPlayerToList(int id, string loginName)
	{
		if(ApplicationModel.player.ToLaunchGameTutorial)
		{
			this.CreateTutorialDeck();
			this.startGame();
			yield break;
		}
		else
		{
			Deck deck;
			deck = new Deck(ApplicationModel.player.SelectedDeckId);
			yield return StartCoroutine(deck.RetrieveCards());
			if (ApplicationModel.player.Username == loginName)
			{
				ApplicationModel.myPlayerName=loginName;
				ApplicationModel.player.MyDeck=deck;
				print("deck :"+ApplicationModel.myPlayerName);
			} 
			else
			{
				ApplicationModel.hisPlayerName=loginName;
				ApplicationModel.opponentDeck=deck;
				print("deck :"+ApplicationModel.hisPlayerName);
			}
			this.nbPlayers++;
			if(this.nbPlayers==2)
			{
				this.startGame();
			}
		}
	}
	private void startGame()
	{
		if(ApplicationModel.player.IsFirstPlayer==true)
		{
			PhotonNetwork.room.open = false;
		}
		BackOfficeController.instance.launchPreMatchLoadingScreen();
		SoundController.instance.playMusic(new int[]{3,4});
	}
	void OnDisconnectedFromPhoton()
	{
		ApplicationModel.player.ToDeconnect=true;
		SceneManager.LoadScene("Authentication");
	}

	private void CreateTutorialDeck()
	{
		ApplicationModel.myPlayerName=ApplicationModel.player.Username;
		ApplicationModel.hisPlayerName="Garruk";
		ApplicationModel.opponentDeck=new Deck();
		ApplicationModel.opponentDeck.cards=new List<Card>();

		List<Skill> skills = new List<Skill>();
		skills.Add (new Skill("Aguerri", 68, 1, 1, 2, 0, "", 0, 0));
		skills.Add (new Skill("Frénésie", 18, 1, 2, 6, 0, "", 0, 80));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.opponentDeck.cards.Add(new Card(-1, "Predator", 35, 2, 0, 3, 16, skills));
		
		skills = new List<Skill>();
		skills.Add (new Skill("Furtif", 66, 1, 1, 3, 0, "", 0, 0));
		skills.Add (new Skill("Estoc", 11, 1, 1, 1, 0, "", 0, 80));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.opponentDeck.cards.Add(new Card(-1, "Flash", 24, 1, 0, 6, 11, skills));
				
		skills = new List<Skill>();
		skills.Add (new Skill("Rapide", 71, 1, 1, 4, 0, "", 0, 0));
		skills.Add (new Skill("Massue", 63, 1, 1, 1, 0, "", 0, 100));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.opponentDeck.cards.Add(new Card(-1, "Alien", 38, 2, 0, 3, 21, skills));
				
		skills = new List<Skill>();
		skills.Add (new Skill("Tank", 70, 1, 1, 2, 0, "", 0, 0));
		skills.Add (new Skill("Attaque 360", 17, 1, 2, 6, 0, "", 0, 80));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.opponentDeck.cards.Add(new Card(-1, "Psycho", 42, 2, 0, 2, 17, skills));

		ApplicationModel.player.MyDeck=new Deck();
		ApplicationModel.player.MyDeck.cards=new List<Card>();

		skills = new List<Skill>();
		skills.Add (new Skill("Aguerri", 68, 1, 1, 2, 0, "", 0, 0));
		skills.Add (new Skill("Frénésie", 18, 1, 2, 6, 0, "", 0, 80));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.player.MyDeck.cards.Add(new Card(-1, "Predator", 35, 2, 0, 3, 16, skills));
		
		skills = new List<Skill>();
		skills.Add (new Skill("Furtif", 66, 1, 1, 3, 0, "", 0, 0));
		skills.Add (new Skill("Estoc", 11, 1, 1, 1, 0, "", 0, 80));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.player.MyDeck.cards.Add(new Card(-1, "Flash", 24, 1, 0, 6, 11, skills));
				
		skills = new List<Skill>();
		skills.Add (new Skill("Rapide", 71, 1, 1, 4, 0, "", 0, 0));
		skills.Add (new Skill("Massue", 63, 1, 1, 1, 0, "", 0, 100));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.player.MyDeck.cards.Add(new Card(-1, "Alien", 38, 2, 0, 3, 21, skills));
				
		skills = new List<Skill>();
		skills.Add (new Skill("Tank", 70, 1, 1, 2, 0, "", 0, 0));
		skills.Add (new Skill("Attaque 360", 17, 1, 2, 6, 0, "", 0, 80));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.player.MyDeck.cards.Add(new Card(-1, "Psycho", 42, 2, 0, 2, 17, skills));
	}
}

