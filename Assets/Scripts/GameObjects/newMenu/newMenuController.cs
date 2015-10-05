using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class newMenuController : Photon.MonoBehaviour 
{
	public int totalNbResultLimit;
	public int refreshInterval;
	public bool isTutorialLaunched;
	public GameObject playPopUpObject;
	public GameObject transparentBackgroundObject;
	public float startButtonPosition;
	public float endButtonPosition;
	public float speed;
	public GUISkin popUpSkin;
	
	public static newMenuController instance;
	private newMenuModel model;
	private int currentPage;
	private int pageHovered;
	private float timer;
	private bool toMoveButtons;
	private bool toMoveBackButtons;
	private Vector3 currentButtonPosition;
	private GameObject playPopUp;
	private GameObject transparentBackground;
	private bool isPlayPopUpDisplayed;
	private Rect centralWindow;

	private bool isDisconnectedViewDisplayed;
	private NewMenuDisconnectedPopUpView disconnectedView;

	public const string roomNamePrefix = "GarrukGame";
	private int nbPlayers;


	void Awake()
	{
		instance = this;
		this.toMoveButtons = false;
		this.model = new newMenuModel ();
	}
	void Start () 
	{	
		StartCoroutine (this.initialization());
	}
	void Update()
	{
		timer += Time.deltaTime;

		if (timer > refreshInterval) 
		{
			timer=timer-refreshInterval;
			StartCoroutine (this.getUserData());
		}
		if(toMoveButtons)
		{
			this.toMoveButtons=false;
			for(int i=0;i<6;i++)
			{
				this.currentButtonPosition= gameObject.transform.Find("Button"+i).localPosition;
				if(i!=this.pageHovered && this.currentButtonPosition.x!=this.startButtonPosition)
				{
					this.currentButtonPosition.x-= this.speed * Time.deltaTime;
					if(this.currentButtonPosition.x<this.startButtonPosition)
					{
						this.currentButtonPosition.x=this.startButtonPosition;

					}
					else
					{
						this.toMoveButtons=true;
					}
					gameObject.transform.Find("Button"+i).localPosition=this.currentButtonPosition;
				}
				else if(i==this.pageHovered && this.currentButtonPosition.x!=this.endButtonPosition)
				{
					this.currentButtonPosition.x+= this.speed * Time.deltaTime;
					if(this.currentButtonPosition.x>this.endButtonPosition)
					{
						this.currentButtonPosition.x=this.endButtonPosition;
					}
					else
					{
						this.toMoveButtons=true;
					}
					gameObject.transform.Find("Button"+i).localPosition=this.currentButtonPosition;
				}
			}
		}
		else
		{
			if(this.toMoveBackButtons)
			{
				this.pageHovered=this.currentPage;
				this.toMoveButtons=true;
				this.toMoveBackButtons=false;
			}
		}
	}
	public void displayPlayPopUp()
	{
		this.transparentBackground=Instantiate(this.transparentBackgroundObject) as GameObject;
		this.transparentBackground.transform.position = new Vector3 (0, 0, -1f);
		this.playPopUp=Instantiate(this.playPopUpObject) as GameObject;
		this.playPopUp.transform.position = new Vector3 (0f, 0f, -2f);
		this.setCurrentPage (5);
		this.isPlayPopUpDisplayed = true;
	}
	public void hidePlayPopUp()
	{
		Destroy (this.playPopUp);
		Destroy (this.transparentBackground);
		this.isPlayPopUpDisplayed = false;
		if(Application.loadedLevelName=="newMyGame")
		{
			this.setCurrentPage (1);
		}
		else if(Application.loadedLevelName=="newStore")
		{
			this.setCurrentPage (2);
		}
		else if(Application.loadedLevelName=="newMarket")
		{
			this.setCurrentPage (3);
		}
		else if(Application.loadedLevelName=="NewSkillBook")
		{
			this.setCurrentPage (4);
		}
		else
		{
			this.setCurrentPage (0);
		}
		Vector3 tempPosition;
		tempPosition= gameObject.transform.Find("Button"+5).localPosition;
		tempPosition.x = this.startButtonPosition;
		gameObject.transform.Find("Button"+5).localPosition=tempPosition;
	}
	public void moveButton(int value)
	{
		this.toMoveButtons = true;
		this.pageHovered = value;
		if(this.pageHovered!=this.currentPage)
		{
			this.toMoveBackButtons=true;
		}
	}
	public void setCurrentPage(int i)
	{
		this.currentPage = i;
		Vector3 tempPosition;
		tempPosition= gameObject.transform.Find("Button"+i).localPosition;
		tempPosition.x = this.endButtonPosition;
		gameObject.transform.Find("Button"+i).localPosition=tempPosition;
	}
	public void setNbNotificationsNonRead(int value)
	{
		ApplicationModel.nbNotificationsNonRead = value;
		this.refreshMenuObject ();
	}
	public IEnumerator getUserData()
	{
		yield return StartCoroutine (model.refreshUserData (this.totalNbResultLimit));
		if(Application.loadedLevelName!="NewHomePage")
		{
			ApplicationModel.nbNotificationsNonRead = model.player.nonReadNotifications;
		}
		ApplicationModel.credits = model.player.Money;
		this.refreshMenuObject ();
	}
	public IEnumerator initialization()
	{
		yield return StartCoroutine (model.loadUserData (this.totalNbResultLimit));
		if(Application.loadedLevelName!="NewHomePage")
		{
			ApplicationModel.nbNotificationsNonRead = model.player.nonReadNotifications;
		}
		ApplicationModel.credits = model.player.Money;

		this.initializeMenuObject ();
		this.refreshMenuObject ();
		StartCoroutine (setUsersPicture ());
	}
	public IEnumerator setUsersPicture()
	{
		yield return StartCoroutine (model.player.setThumbProfilePicture ());
		gameObject.transform.Find ("Picture").GetComponent<SpriteRenderer> ().sprite = model.player.texture;
	}
	public void initializeMenuObject()
	{
		if(ApplicationModel.isAdmin)
		{
			this.gameObject.transform.Find("Toolbar").FindChild("AdminButton").gameObject.SetActive(true);
		}
		
		for (int i=0;i<6;i++)
		{
			this.gameObject.transform.FindChild("Button"+i).FindChild("Text").GetComponent<TextMeshPro>().text=model.buttonsLabels[i];
		}
		
		this.gameObject.transform.FindChild("User").FindChild("Username").GetComponent<TextMeshPro>().text=ApplicationModel.username;
	}
	public void resizeMeunObject(float worldHeight, float worldWidth)
	{
		this.gameObject.transform.position =new Vector3(-worldWidth / 2f + 1.5625f, 0, 0);
		this.gameObject.transform.Find ("Toolbar").position = new Vector3 (worldWidth / 2f - 0.3665f - 0.2f * (worldHeight / worldWidth), worldHeight / 2f - 0.2f - 0.1805f, 0f);
		this.centralWindow = new Rect (Screen.width * 0.25f, 0.12f * Screen.height,Screen.width * 0.50f, 0.40f * Screen.height);
	}
	public void refreshMenuObject()
	{
		this.gameObject.transform.FindChild("User").FindChild("Credits").GetComponent<TextMeshPro>().text=ApplicationModel.credits.ToString();
		if(ApplicationModel.nbNotificationsNonRead>0)
		{
			this.gameObject.transform.FindChild("Notifications").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("Notifications").FindChild("Text").GetComponent<TextMeshPro>().text=ApplicationModel.nbNotificationsNonRead.ToString();
		}
		else
		{
			this.gameObject.transform.FindChild("Notifications").gameObject.SetActive(false);
		}
	}
	public void changePage(int i)
	{
		switch(i)
		{
		case 0:
			this.homePageLink();
			break;
		case 1:
			this.myGameLink();
			break;
		case 2:
			this.storeLink();
			break;
		case 3:
			this.marketLink();
			break;
		case 4:
			this.skillBookLink();
			break;
		case 5:
			this.playLink();
			break;
		}
	}
	public void logOutLink() 
	{
		if(isDisconnectedViewDisplayed)
		{
			this.hideDisconnectedPopUp();
		}
		ApplicationModel.username = "";
		ApplicationModel.toDeconnect = true;
		PhotonNetwork.Disconnect();
		Application.LoadLevel("Authentication");
	}
	public void homePageLink()
	{
		Application.LoadLevel("NewHomePage");
	}
	public void profileLink() 
	{
		Application.LoadLevel("Profile");
	}
	public void myGameLink() 
	{
		if(this.isTutorialLaunched)
		{
			TutorialObjectController.instance.actionIsDone();
		}
		else
		{
			Application.LoadLevel("newMyGame");
		}
	}
	public void marketLink() 
	{
		Application.LoadLevel("newMarket");
	}
	public void skillBookLink() 
	{
		Application.LoadLevel("NewSkillBook");
	}
	public void storeLink() 
	{
		if(this.isTutorialLaunched)
		{
			TutorialObjectController.instance.actionIsDone();
		}
		else
		{
			Application.LoadLevel("newStore");
		}
	}
	public void playLink() 
	{
		this.displayPlayPopUp();
	}
	public void adminBoardLink() 
	{
		Application.LoadLevel("AdminBoard");
	}
	public void setButtonsGui(bool value)
	{
//		for(int i=0;i<view.menuVM.buttonsEnabled.Length;i++)
//		{
//			view.menuVM.buttonsEnabled[i]=value;
//		}
	}
	public void setButtonGui(int index, bool value)
	{
//		view.menuVM.buttonsEnabled[index]=value;
	}
	public void setTutorialLaunched(bool value)
	{
		this.isTutorialLaunched = value;
	}
	public bool getIsTutorialLaunched()
	{
		return isTutorialLaunched;
	}
	public bool isAPopUpDisplayed()
	{
		if(isPlayPopUpDisplayed)
		{
			return true;
		}
		if(isDisconnectedViewDisplayed)
		{
			return true;
		}
		return false;
	}
	public void returnPressed()
	{
		if(isDisconnectedViewDisplayed)
		{
			this.logOutLink();
		}
	}
	public void escapePressed()
	{
		if(isPlayPopUpDisplayed)
		{
			this.hidePlayPopUp();
		}
		if(isDisconnectedViewDisplayed)
		{
			this.hideDisconnectedPopUp();
		}
	}
	public Vector3 getButtonPosition(int id)
	{
		return gameObject.transform.FindChild ("Button" + id).position;
	}
	public void displayDisconnectedPopUp()
	{
		this.isDisconnectedViewDisplayed = true;
		this.disconnectedView = Camera.main.gameObject.AddComponent <NewMenuDisconnectedPopUpView>();
		disconnectedView.popUpVM.centralWindowStyle = new GUIStyle(this.popUpSkin.window);
		disconnectedView.popUpVM.centralWindowTitleStyle = new GUIStyle (this.popUpSkin.customStyles [0]);
		disconnectedView.popUpVM.centralWindowButtonStyle = new GUIStyle (this.popUpSkin.button);
		disconnectedView.popUpVM.transparentStyle = new GUIStyle (this.popUpSkin.customStyles [2]);
		this.disconnectedPopUpResize ();
	}
	public void disconnectedPopUpResize()
	{
		disconnectedView.popUpVM.centralWindow = this.centralWindow;
		disconnectedView.popUpVM.resize ();
	}
	public void hideDisconnectedPopUp()
	{
		Destroy (this.disconnectedView);
		this.isDisconnectedViewDisplayed = false;
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
		Debug.Log("Can't join random room! - creating a new room");
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

