using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine.SceneManagement;

public class PhotonC : Photon.MonoBehaviour 
{
	GameObject preMatchScreen;

	public static PhotonC instance;
	float waitingTime;
	public bool isWaiting;
	float timeLimit=10f;
	bool reconnecting ;

	bool isQuittingGame ;

	void Awake(){
		this.isWaiting = false;
		instance = this;
		this.reconnecting = false;
	}

	public void addTime(float f){
		this.waitingTime+=f;
		if(this.waitingTime>this.timeLimit){
			this.isWaiting=false;
			PhotonNetwork.room.visible = false;
			this.createIARoom();
		}
	}

	public void createIARoom(){
		Debug.Log("Je passe en mode IA");
		Game.instance.setIA(true);
		this.closeAndStart();
	}

	public void displayLoadingScreen(){
		this.preMatchScreen.SetActive(true);
		this.preMatchScreen.GetComponent<PreMatchScreenController>().reset();
	}

	public void displayLoadingScreen(string s){
		this.preMatchScreen.GetComponent<PreMatchScreenController>().changeLoadingScreenLabel(s);
		this.displayLoadingScreen();
	}

	public void findRoom(){
		this.preMatchScreen=this.gameObject.transform.FindChild("PreMatchScreen").gameObject;
		if(ApplicationModel.player.ToLaunchGameIA || !ApplicationModel.player.IsOnline){
			ApplicationModel.player.IsFirstPlayer = true;
			Game.instance.setFirstPlayer(true);
			this.createIARoom();
			PlayerPrefs.SetInt ("offlineGame", 1);
		}
		else{
			if(PlayerPrefs.HasKey("currentGame")){
				Debug.Log("J'essaye de me reconnecter à ma room");
				ApplicationModel.player.IsFirstPlayer = false;
	            ApplicationModel.player.ToLaunchGameIA = false;
				this.reconnecting = true;
				PhotonNetwork.JoinRoom(PlayerPrefs.GetString("currentGame"));
			}
			else{
				Debug.Log("J'essaye de trouver une room ouverte");
				this.displayLoadingScreen();
				if(ApplicationModel.player.ChosenGameType>20){
					this.changeLoadingScreenLabel(WordingSocial.getReference(19));  
				}
				else if(ApplicationModel.player.ChosenGameType<=20 && !ApplicationModel.player.ToLaunchGameTutorial)
				{
					this.changeLoadingScreenLabel (WordingGameModes.getReference(7));
				}
				else
				{
					this.changeLoadingScreenLabel(WordingSocial.getReference(19));
				}

				if(ApplicationModel.player.ToLaunchChallengeGame || ApplicationModel.player.ToLaunchGameTutorial)
		        {
		            this.CreateNewRoom();
		        }
		        else
		        {
		            TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);    
		            string sqlLobbyFilter = "C0 = " + ApplicationModel.player.ChosenGameType;
		            ApplicationModel.player.IsFirstPlayer = false;
		            ApplicationModel.player.ToLaunchGameIA = false;
		            PhotonNetwork.JoinRandomRoom(null, 0, ExitGames.Client.Photon.MatchmakingMode.FillRoom, sqlLobby, sqlLobbyFilter);
	       		}
			}
		}
	}

	void OnJoinedLobby(){
		if(this.reconnecting){
			PhotonNetwork.JoinRoom(PlayerPrefs.GetString("currentGame"));
		}
	}

	public void reconnectToRoom(){
		this.reconnecting = true ;
		PhotonNetwork.Reconnect();
		print("J'essaye de me RECONNECT");
	}

	public void changeLoadingScreenLabel(string label)
	{
		this.preMatchScreen.GetComponent<PreMatchScreenController> ().changeLoadingScreenLabel (label);
	}

	void OnJoinedRoom()
    {
		if(ApplicationModel.player.ChosenGameType<=20 && !ApplicationModel.player.ToLaunchGameTutorial)
        {
            this.isWaiting=true ;
            this.waitingTime=0f;
        }
        if(ApplicationModel.player.ToLaunchChallengeGame==true)
        {
            ApplicationModel.player.ToLaunchChallengeGame=false;
        }

    	if(!this.reconnecting){
			print("J'ai rejoint la room "+PhotonNetwork.room.name);
	       	PlayerPrefs.SetString("currentGame", PhotonNetwork.room.name);

			print("J'add mon nom à la room");

			GameRPC.instance.addPlayerToRoom(ApplicationModel.player.Username);
			

	        if(!ApplicationModel.player.ToLaunchGameTutorial && Game.instance.isFirstPlayer())
	        {
	        	if(ApplicationModel.player.ChosenGameType<=20)
	            {
	        	    this.displayLoadingScreenButton(true);
	            }
	        }
			if(ApplicationModel.player.ToLaunchGameTutorial)
	        {
				ApplicationModel.hisPlayerName="Garruk";
	            this.hideLoadingScreen();
				Game.instance.createBackground();
	        }
        }
        else{
        	this.reconnecting = false ;
        }
    }

    public bool isReconnecting(){
    	return this.reconnecting;
    }

	void OnPhotonRandomJoinFailed(){
        if(ApplicationModel.player.ChosenGameType>20)
        {
			Debug.Log("N'arrive pas à rejoindre le défi - rééssaye");
	        this.findRoom();
        }
        else
        {
            Debug.Log("N'arrive pas à rejoindre la room - en crée une");
            this.CreateNewRoom ();
        }
    }
	public void OnPhotonJoinRoomFailed()
    {
		Debug.Log("La room ciblée n'existe plus !");
		PlayerPrefs.DeleteKey("currentGame");
		hideLoadingScreen ();
		if (this.reconnecting) 
		{
			ApplicationModel.player.HasLostConnectionDuringGame = true;
			SceneManager.LoadScene ("EndGame");
			reconnecting = false;
		} 
		else 
		{
			SceneManager.LoadScene("NewHomePage");
		}
    }
	public void CreateNewRoom()
    {
        print("Création de la room");
        Game.instance.setFirstPlayer(true);
		
        RoomOptions newRoomOptions = new RoomOptions();
        newRoomOptions.isOpen = true;
        newRoomOptions.isVisible = true;
        newRoomOptions.maxPlayers = 2;
        newRoomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable(){{"C0", ApplicationModel.player.ChosenGameType}};
        newRoomOptions.customRoomPropertiesForLobby = new string[]{"C0"};
        TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);
        PhotonNetwork.CreateRoom("GarrukGame" + Guid.NewGuid().ToString("N"), newRoomOptions, sqlLobby);
    }

    public void addPlayer(string s){
    	print("J'add "+s);
	    if(!s.Equals(ApplicationModel.player.Username)){
	    	this.isWaiting = false ;
			if(Game.instance.isFirstPlayer()){
				PhotonNetwork.room.visible = false;
				this.closeAndStart();
	    	}
	    }
    }

    public void closeAndStart(){
		if(Game.instance.isIA() || Game.instance.isTutorial()){
			Game.instance.createBackground();
		}
		else{
			StartCoroutine(GameRPC.instance.launchRPC("createBackgroundRPC"));
		}
    }

	public void displayLoadingScreenButton(bool value)
	{
		print("Je hide2 "+value);
		this.preMatchScreen.transform.FindChild("button").GetComponent<PreMatchScreenButtonController>().reset();
        this.preMatchScreen.GetComponent<PreMatchScreenController> ().displayButton (value);
	}

	public void hideLoadingScreen()
	{
		this.displayLoadingScreenButton(false);
		this.preMatchScreen.SetActive(false);

		if(ApplicationModel.player.IsInviting)
		{
			ApplicationModel.player.IsInviting=false;
		}
	}
}
