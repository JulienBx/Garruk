using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class newMenuController : MonoBehaviour 
{
	public int totalNbResultLimit;
	public int refreshInterval;
	public bool isTutorialLaunched;
	
	public static newMenuController instance;
	private newMenuModel model;
	private int currentPage;
	private int pageHovered;
	private float timer;
	private bool toMoveButtons;
	private bool toMoveBackButtons;
	private float speed;
	private float startButtonPosition;
	private float endButtonPosition;
	private Vector3 currentButtonPosition;
	
	void Awake()
	{
		instance = this;
		this.speed = 15;
		this.startButtonPosition = -1.5625f;
		this.endButtonPosition = -1f;
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
			for(int i=0;i<5;i++)
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
		yield return StartCoroutine (model.loadUserData (this.totalNbResultLimit));
		if(Application.loadedLevelName!="HomePage")
		{
			ApplicationModel.nbNotificationsNonRead = model.player.nonReadNotifications;
		}
		ApplicationModel.credits = model.player.Money;
		this.refreshMenuObject ();
	}
	public IEnumerator initialization()
	{
		yield return StartCoroutine (model.loadUserData (this.totalNbResultLimit));
		if(Application.loadedLevelName!="HomePage")
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
		gameObject.transform.Find ("User").FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = Sprite.Create (model.player.texture, new Rect (0, 0, model.player.texture.width, model.player.texture.height), new Vector2 (0.5f, 0.5f));
	}
	public void initializeMenuObject()
	{
		if(ApplicationModel.isAdmin)
		{
			this.gameObject.transform.Find("Toolbar").FindChild("AdminButton").gameObject.SetActive(true);
		}
		
		for (int i=0;i<5;i++)
		{
			this.gameObject.transform.FindChild("Button"+i).FindChild("Text").GetComponent<TextMesh>().text=model.buttonsLabels[i];
			this.gameObject.transform.FindChild("Button"+i).FindChild("Text").GetComponent<MeshRenderer>().sortingLayerName = this.gameObject.transform.FindChild("Button"+i).GetComponent<SpriteRenderer>().sortingLayerName;
		}
		
		this.gameObject.transform.FindChild("User").FindChild("Username").GetComponent<MeshRenderer>().sortingLayerName = this.gameObject.transform.FindChild("User").GetComponent<SpriteRenderer>().sortingLayerName;
		this.gameObject.transform.FindChild("User").FindChild("Credits").GetComponent<MeshRenderer>().sortingLayerName = this.gameObject.transform.FindChild("User").GetComponent<SpriteRenderer>().sortingLayerName;
		this.gameObject.transform.FindChild("Notifications").FindChild("Text").GetComponent<MeshRenderer>().sortingLayerName = this.gameObject.transform.FindChild("Notifications").GetComponent<SpriteRenderer>().sortingLayerName;
		
		this.gameObject.transform.FindChild("User").FindChild("Username").GetComponent<TextMesh>().text=ApplicationModel.username;
		if(ApplicationModel.username.Length>10)
		{
			float scale = (0.08f-(ApplicationModel.username.Length-10)*0.008f);
			this.gameObject.transform.FindChild("User").FindChild("Username").localScale=new Vector3(scale,scale,0);
		}
	}
	public void resizeMeunObject(float worldHeight, float worldWidth)
	{
		this.gameObject.transform.position =new Vector3(-worldWidth / 2f + 1.5625f, 0, 0);
		this.gameObject.transform.Find ("Toolbar").position = new Vector3 (worldWidth / 2f - 0.3665f - 0.2f * (worldHeight / worldWidth), worldHeight / 2f - 0.2f - 0.1805f, 0f);
	}
	public void refreshMenuObject()
	{
		this.gameObject.transform.FindChild("User").FindChild("Credits").GetComponent<TextMesh>().text=ApplicationModel.credits.ToString();
		if(ApplicationModel.credits.ToString().Length>7)
		{
			float scale = (0.065f-(ApplicationModel.credits.ToString().Length-7)*0.008f);
			this.gameObject.transform.FindChild("User").FindChild("Credits").localScale=new Vector3(scale,scale,0);
		}
		if(ApplicationModel.nbNotificationsNonRead>0)
		{
			this.gameObject.transform.FindChild("Notifications").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("Notifications").FindChild("Text").GetComponent<TextMesh>().text=ApplicationModel.nbNotificationsNonRead.ToString();
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
			this.lobbyLink();
			break;
		}
	}
	public void checkPhoton()
	{
		if(Application.loadedLevelName=="Lobby")
		{
			PhotonNetwork.Disconnect();
		}
	}
	public void logOutLink() 
	{
		ApplicationModel.username = "";
		ApplicationModel.toDeconnect = true;
		if(Application.loadedLevelName=="Lobby")
		{
			PhotonNetwork.Disconnect();
		}
		Application.LoadLevel("Authentication");
	}
	public void homePageLink()
	{
		this.checkPhoton ();
		Application.LoadLevel("HomePage");
	}
	public void profileLink() 
	{
		this.checkPhoton ();
		Application.LoadLevel("Profile");
	}
	public void myGameLink() 
	{
		this.checkPhoton ();
		if(this.isTutorialLaunched)
		{
			TutorialObjectController.instance.actionIsDone();
		}
		else
		{
			Application.LoadLevel("MyGame");
		}
	}
	public void marketLink() 
	{
		this.checkPhoton ();
		Application.LoadLevel("Market");
	}
	public void storeLink() 
	{
		this.checkPhoton ();
		if(this.isTutorialLaunched)
		{
			TutorialObjectController.instance.actionIsDone();
		}
		else
		{
			Application.LoadLevel("Store");
		}
	}
	public void lobbyLink() 
	{
		this.checkPhoton ();
		if(this.isTutorialLaunched)
		{
			TutorialObjectController.instance.actionIsDone();
		}
		else
		{
			Application.LoadLevel("Lobby");
		}
	}
	public void adminBoardLink() 
	{
		if(Application.loadedLevelName=="Lobby"){
			PhotonNetwork.Disconnect();
		}
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
}

