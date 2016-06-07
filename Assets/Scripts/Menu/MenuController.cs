using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour 
{
	public static MenuController instance;

	private int nbNotificationsNonRead;

	private float totalNbResultLimit;
	public string[] buttonsLabels;
	public string[] mobileButtonsLabels;


	public void initialize()
	{
		instance = this;
		this.buttonsLabels = new string[6];
		this.buttonsLabels [0] = WordingMenu.getReference(0);
		this.buttonsLabels [1] = WordingMenu.getReference(1);
		this.buttonsLabels [2] = WordingMenu.getReference(2);
		this.buttonsLabels [3] = WordingMenu.getReference(3);
		this.buttonsLabels [4] = WordingMenu.getReference(4);
		this.buttonsLabels [5] = WordingMenu.getReference(5);
		this.mobileButtonsLabels = new string[6];
		this.mobileButtonsLabels [0] = WordingMenu.getReference(6);
		this.mobileButtonsLabels [1] = WordingMenu.getReference(7);
		this.mobileButtonsLabels [2] = WordingMenu.getReference(8);
		this.mobileButtonsLabels [3] = WordingMenu.getReference(9);
		this.mobileButtonsLabels [4] = WordingMenu.getReference(10);
		this.mobileButtonsLabels [5] = WordingMenu.getReference(11);
		this.initializeMenuObject ();
		this.changeThumbPicture();
	}
	public void setCurrentPage(int i)
	{
		if(!ApplicationDesignRules.isMobileScreen)
		{
			gameObject.transform.Find ("Button" + i).GetComponent<MenuButtonController> ().setIsSelected(true);
			gameObject.transform.Find ("Button" + i).GetComponent<MenuButtonController> ().setHoveredState();
		}
		else
		{
			gameObject.transform.FindChild ("MobileButton" + i).GetComponent<MobileMenuButtonController> ().setIsSelected(true);
			gameObject.transform.FindChild ("MobileButton" + i).GetComponent<MobileMenuButtonController> ().setHoveredState();
		}
	}
	public void initializeMenuObject()
	{
		this.gameObject.transform.Find ("LogoBlock").FindChild ("AdminButton").gameObject.AddComponent<MenuAdminController> ();
		this.gameObject.transform.Find ("LogoBlock").FindChild ("DisconnectButton").gameObject.AddComponent<MenuDisconnectController> ();
		this.gameObject.transform.FindChild ("UserBlock").FindChild ("Username").gameObject.AddComponent<MenuUserUsernameController> ();
		this.gameObject.transform.FindChild ("MobileUsername").gameObject.AddComponent<MobileMenuUsernameController> ();
		this.gameObject.transform.FindChild ("MobilePicture").gameObject.AddComponent<MobileMenuPictureController> ();
		this.gameObject.transform.FindChild ("MobileNotificationsButton").gameObject.AddComponent<MobileMenuNotificationsController> ();
		this.gameObject.transform.FindChild ("UserBlock").FindChild ("Picture").gameObject.AddComponent<MenuUserPictureController> ();
		this.gameObject.transform.FindChild ("UserBlock").FindChild ("Bell").gameObject.AddComponent<MenuNotificationsController> ();
		this.gameObject.transform.FindChild ("UserBlock").FindChild ("Credits").gameObject.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild ("UserBlock").FindChild ("Help").gameObject.AddComponent<MenuHelpController> ();
		this.gameObject.transform.FindChild ("MobileHelpButton").gameObject.AddComponent<MobileMenuHelpController> ();
	
		if(ApplicationModel.player.IsAdmin)
		{
			this.gameObject.transform.Find("LogoBlock").FindChild("AdminButton").gameObject.SetActive(true);
			this.gameObject.transform.Find("LogoBlock").FindChild("AdminButton").transform.localPosition=new Vector3(-2.73f,-0.3f,0f);
			this.gameObject.transform.Find("LogoBlock").FindChild("DisconnectButton").transform.localPosition=new Vector3(-2.73f,0.3f,0f);
		}
		else
		{
			this.gameObject.transform.Find("LogoBlock").FindChild("AdminButton").gameObject.SetActive(false);
			this.gameObject.transform.Find("LogoBlock").FindChild("DisconnectButton").transform.localPosition=new Vector3(-2.73f,0f,0f);
		}
		for (int i=0;i<6;i++)
		{
			this.gameObject.transform.FindChild("Button"+i).FindChild("Title").GetComponent<TextMeshPro>().text=this.buttonsLabels[i].ToUpper();
			this.gameObject.transform.FindChild("MobileButton"+i).FindChild("Title").GetComponent<TextMeshPro>().text=this.mobileButtonsLabels[i];
			//this.gameObject.transform.FindChild("BottomBar").FindChild("Button"+i).FindChild("Title").GetComponent<TextMeshPro>().text=model.buttonsLabels[i];
			this.gameObject.transform.FindChild("Button"+i).gameObject.AddComponent<MenuButtonController>();
			this.gameObject.transform.FindChild("MobileButton"+i).gameObject.AddComponent<MobileMenuButtonController>();
			this.gameObject.transform.FindChild("Button"+i).GetComponent<MenuButtonController>().setId(i);
			this.gameObject.transform.FindChild("MobileButton"+i).GetComponent<MobileMenuButtonController>().setId(i);
		}
		this.gameObject.transform.FindChild("UserBlock").FindChild("Username").GetComponent<TextMeshPro>().text=ApplicationModel.player.Username;
		this.gameObject.transform.FindChild ("MobileUsername").GetComponent<TextMeshPro> ().text = ApplicationModel.player.Username;
		this.nbNotificationsNonRead=ApplicationModel.player.NbNotificationsNonRead;
	}
	public void resize()
	{
		gameObject.transform.position = new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y + ApplicationDesignRules.worldHeight / 2f - 5f, 0f);
		if(!ApplicationDesignRules.isMobileScreen)
		{
			gameObject.transform.FindChild("LogoBlock").gameObject.SetActive(true);
			gameObject.transform.FindChild("UserBlock").gameObject.SetActive(true);
			gameObject.transform.FindChild("MobilePicture").gameObject.SetActive(false);
			gameObject.transform.FindChild("MobileUsername").gameObject.SetActive(false);
			gameObject.transform.FindChild("TopBar").gameObject.SetActive(false);
			gameObject.transform.FindChild("BottomBar").gameObject.SetActive(false);
			gameObject.transform.FindChild("MobileHelpButton").gameObject.SetActive(false);
			gameObject.transform.FindChild("MobileNotificationsButton").gameObject.SetActive(false);
			gameObject.transform.FindChild("MobileNotifications").gameObject.SetActive(false);
			gameObject.transform.FindChild("MobileCristalsBar").gameObject.SetActive(false);
			gameObject.transform.FindChild("MobileDivisionIcon").gameObject.SetActive(false);

			float buttonsBorderWidth = 1500f;
			float buttonsWorldScaleX = (ApplicationDesignRules.worldWidth-ApplicationDesignRules.leftMargin-ApplicationDesignRules.rightMargin)/(buttonsBorderWidth / ApplicationDesignRules.pixelPerUnit);
			gameObject.transform.FindChild ("ButtonsBorder").localScale = new Vector3 (buttonsWorldScaleX, 0.63f, 1f);
			
			float buttonsTotalSize=0f;
			float buttonsGap=0f;
			float textRatio=1f;
			float fontSize = 2.5f;
			gameObject.transform.FindChild("ButtonsBorder").gameObject.SetActive(true);
			for(int i=0;i<6;i++)
			{
				this.gameObject.transform.FindChild("Button"+i).gameObject.SetActive(true);
				this.gameObject.transform.FindChild("Button"+i).FindChild("Title").GetComponent<TextMeshPro>().fontSize=textRatio*fontSize;
				buttonsTotalSize=buttonsTotalSize+this.gameObject.transform.FindChild("Button"+i).FindChild("Title").GetComponent<TextMeshPro>().bounds.size.x;
			}
			if((ApplicationDesignRules.worldWidth-1.5f-ApplicationDesignRules.rightMargin-ApplicationDesignRules.leftMargin)<buttonsTotalSize)
			{
				textRatio=(ApplicationDesignRules.worldWidth-1.5f-ApplicationDesignRules.rightMargin-ApplicationDesignRules.leftMargin)/buttonsTotalSize;
			}
			buttonsTotalSize = 0f;
			for(int i=0;i<6;i++)
			{
				this.gameObject.transform.FindChild("Button"+i).FindChild("Title").GetComponent<TextMeshPro>().fontSize=textRatio*fontSize;
				buttonsTotalSize=buttonsTotalSize+this.gameObject.transform.FindChild("Button"+i).FindChild("Title").GetComponent<TextMeshPro>().bounds.size.x;
			}
			buttonsGap = (ApplicationDesignRules.worldWidth - ApplicationDesignRules.leftMargin-ApplicationDesignRules.rightMargin- buttonsTotalSize) / 6f;
			
			for(int i=0;i<6;i++)
			{
				this.gameObject.transform.FindChild("MobileButton"+i).gameObject.SetActive(false);
				float previousButtonsTotalSize=0f;
				for(int j=0;j<i;j++)
				{
					previousButtonsTotalSize=previousButtonsTotalSize+this.gameObject.transform.FindChild("Button"+j).FindChild("Title").GetComponent<TextMeshPro>().bounds.size.x;
				}
				Vector3 buttonPosition=gameObject.transform.FindChild("Button"+i).transform.position;
				buttonPosition.x=buttonsGap/2f+(-ApplicationDesignRules.worldWidth / 2f)+ApplicationDesignRules.leftMargin+(i)*(buttonsGap)+this.gameObject.transform.FindChild("Button"+i).FindChild("Title").GetComponent<TextMeshPro>().bounds.size.x/2f+previousButtonsTotalSize;
				gameObject.transform.FindChild("Button"+i).transform.position=buttonPosition;
			}

			float logoBlockWidth = 693f;
			float logoBlockScale = 0.7f;
			float logoBlockWorldWidth = (logoBlockWidth / ApplicationDesignRules.pixelPerUnit)*logoBlockScale;
			Vector3 logoBlockPosition = gameObject.transform.FindChild ("LogoBlock").transform.position;
			logoBlockPosition.x = (-ApplicationDesignRules.worldWidth / 2f) + ApplicationDesignRules.leftMargin + logoBlockWorldWidth / 2f;
			gameObject.transform.FindChild ("LogoBlock").transform.position = logoBlockPosition;
			
			float userBlockWidth = 766f;
			float userBlockWorldWidth = userBlockWidth / ApplicationDesignRules.pixelPerUnit*logoBlockScale;
			Vector3 userBlockPosition = gameObject.transform.FindChild ("UserBlock").transform.position;
			userBlockPosition.x = (ApplicationDesignRules.worldWidth / 2f) - ApplicationDesignRules.rightMargin - userBlockWorldWidth / 2f;
			gameObject.transform.FindChild ("UserBlock").transform.position = userBlockPosition;
		}
		else
		{
			gameObject.transform.FindChild("LogoBlock").gameObject.SetActive(false);
			gameObject.transform.FindChild("UserBlock").gameObject.SetActive(false);
			gameObject.transform.FindChild("MobilePicture").gameObject.SetActive(true);
			gameObject.transform.FindChild("MobileDivisionIcon").gameObject.SetActive(true);
			gameObject.transform.FindChild("MobileUsername").gameObject.SetActive(true);
			gameObject.transform.FindChild("MobileHelpButton").gameObject.SetActive(true);
			gameObject.transform.FindChild("MobileNotificationsButton").gameObject.SetActive(true);
			gameObject.transform.FindChild("MobileCristalsBar").gameObject.SetActive(true);
			gameObject.transform.FindChild("TopBar").gameObject.SetActive(true);
			gameObject.transform.FindChild("TopBar").localScale=ApplicationDesignRules.topBarScale;
			gameObject.transform.FindChild("TopBar").position=new Vector3(ApplicationDesignRules.menuPosition.x,ApplicationDesignRules.menuPosition.y+ApplicationDesignRules.worldHeight/2f-ApplicationDesignRules.topBarWorldSize.y/2f+0.05f,0f);
			gameObject.transform.FindChild("BottomBar").gameObject.SetActive(true);
			gameObject.transform.FindChild("BottomBar").localScale=ApplicationDesignRules.bottomBarScale;
			gameObject.transform.FindChild("BottomBar").position=new Vector3(ApplicationDesignRules.menuPosition.x,ApplicationDesignRules.menuPosition.y-ApplicationDesignRules.worldHeight/2f+ApplicationDesignRules.bottomBarWorldSize.y/2f-0.05f,0f);
			gameObject.transform.FindChild("ButtonsBorder").gameObject.SetActive(false);
			gameObject.transform.FindChild("MobilePicture").transform.localScale=ApplicationDesignRules.thumbScale;
			gameObject.transform.FindChild("MobilePicture").transform.position=new Vector3(gameObject.transform.FindChild("TopBar").position.x-ApplicationDesignRules.topBarWorldSize.x/2f+ApplicationDesignRules.blockHorizontalSpacing+ApplicationDesignRules.thumbWorldSize.x/2f,gameObject.transform.FindChild("TopBar").position.y,gameObject.transform.FindChild("TopBar").position.z);
			gameObject.transform.FindChild("MobileDivisionIcon").transform.localScale=ApplicationDesignRules.divisionIconScale;
			gameObject.transform.FindChild("MobileDivisionIcon").transform.position=gameObject.transform.FindChild("MobilePicture").position+ApplicationDesignRules.divisionIconDistance;
			gameObject.transform.FindChild("MobileCristalsBar").transform.localScale=ApplicationDesignRules.reductionRatio*new Vector3(0.35f,0.35f,0.35f);

			Vector2 mobileCristalsWorldSize = new Vector2(gameObject.transform.FindChild("MobileCristalsBar").transform.localScale.y*(700f/ApplicationDesignRules.pixelPerUnit),gameObject.transform.FindChild("MobileCristalsBar").transform.localScale.y*(121f/ApplicationDesignRules.pixelPerUnit));
			gameObject.transform.FindChild("MobileCristalsBar").transform.position=new Vector3(gameObject.transform.FindChild("MobilePicture").position.x+ApplicationDesignRules.thumbWorldSize.x/2f+0.1f+mobileCristalsWorldSize.x/2f,gameObject.transform.FindChild("TopBar").position.y-ApplicationDesignRules.thumbWorldSize.y/2f+mobileCristalsWorldSize.y/2f,gameObject.transform.FindChild("TopBar").position.z);

			gameObject.transform.FindChild("MobileUsername").transform.localScale=ApplicationDesignRules.reductionRatio*new Vector3(1f,1f,1f);
			gameObject.transform.FindChild("MobileUsername").transform.position=new Vector3(gameObject.transform.FindChild("MobilePicture").position.x+ApplicationDesignRules.thumbWorldSize.x/2f+0.2f,gameObject.transform.FindChild("TopBar").position.y+ApplicationDesignRules.thumbWorldSize.y/2f-mobileCristalsWorldSize.y/2f,gameObject.transform.FindChild("TopBar").position.z);

			gameObject.transform.FindChild("MobileHelpButton").transform.localScale=ApplicationDesignRules.reductionRatio*new Vector3(0.7f,0.7f,0.7f);
			gameObject.transform.FindChild("MobileNotificationsButton").transform.localScale=ApplicationDesignRules.reductionRatio*new Vector3(0.7f,0.7f,0.7f);

			Vector2 mobileButtonsWorldSize = new Vector2(gameObject.transform.FindChild("MobileHelpButton").transform.localScale.y*(120f/ApplicationDesignRules.pixelPerUnit),gameObject.transform.FindChild("MobileHelpButton").transform.localScale.y*(121f/ApplicationDesignRules.pixelPerUnit));
			gameObject.transform.FindChild("MobileHelpButton").transform.position=new Vector3(gameObject.transform.FindChild("TopBar").position.x+ApplicationDesignRules.topBarWorldSize.x/2f-ApplicationDesignRules.blockHorizontalSpacing-mobileButtonsWorldSize.x/2f,gameObject.transform.FindChild("TopBar").position.y,0f);
			gameObject.transform.FindChild("MobileNotificationsButton").transform.position=new Vector3(gameObject.transform.FindChild("TopBar").position.x+ApplicationDesignRules.topBarWorldSize.x/2f-0.1f-ApplicationDesignRules.blockHorizontalSpacing-1.5f*mobileButtonsWorldSize.x,gameObject.transform.FindChild("TopBar").position.y,0f);

			gameObject.transform.FindChild("MobileNotifications").transform.localScale=ApplicationDesignRules.reductionRatio*new Vector3(1f,1f,1f);
			gameObject.transform.FindChild("MobileNotifications").transform.position=new Vector3(gameObject.transform.FindChild("MobileNotificationsButton").position.x-0.25f*mobileButtonsWorldSize.x,gameObject.transform.FindChild("MobileNotificationsButton").position.y+0.25f*mobileButtonsWorldSize.x,gameObject.transform.FindChild("MobileNotificationsButton").position.z);

			float gapBetweenButtons = (ApplicationDesignRules.bottomBarWorldSize.x-6f*ApplicationDesignRules.buttonMenuWorldSize.x - 2f*ApplicationDesignRules.blockHorizontalSpacing)/5f;
			for(int i=0;i<6;i++)
			{
				this.gameObject.transform.FindChild("Button"+i).gameObject.SetActive(false);
				this.gameObject.transform.FindChild("MobileButton"+i).gameObject.SetActive(true);
				this.gameObject.transform.FindChild("MobileButton"+i).localScale=ApplicationDesignRules.buttonMenuScale;
				this.gameObject.transform.FindChild("MobileButton"+i).position=new Vector3(this.gameObject.transform.FindChild("BottomBar").position.x-(2.5f-i)*(gapBetweenButtons+ApplicationDesignRules.buttonMenuWorldSize.x),this.gameObject.transform.FindChild("BottomBar").position.y,0f);
			}
		}
	}
	public void refreshMenuObject()
	{
		string displayedDivision="";
		if(ApplicationDesignRules.isMobileScreen)
		{
			this.gameObject.transform.FindChild ("MobileCristalsBar").FindChild ("Title").GetComponent<TextMeshPro> ().text = ApplicationModel.player.Money.ToString ();
			displayedDivision = this.gameObject.transform.FindChild("MobileDivisionIcon").FindChild("Title").GetComponent<TextMeshPro>().text;
			if(displayedDivision!=ApplicationModel.player.CurrentDivision.Id.ToString())
			{
				this.gameObject.transform.FindChild("MobileDivisionIcon").GetComponent<DivisionIconController>().setDivision(ApplicationModel.player.CurrentDivision.Id);
			}
			if(ApplicationModel.player.TrainingStatus==-1)
			{
				this.gameObject.transform.FindChild("MobileDivisionIcon").gameObject.SetActive(true);
			}
			else
			{
				this.gameObject.transform.FindChild("MobileDivisionIcon").gameObject.SetActive(false);
			}
			if(this.nbNotificationsNonRead>0)
			{
				this.gameObject.transform.FindChild("MobileNotificationsButton").GetComponent<MobileMenuNotificationsController>().reset();
				this.gameObject.transform.FindChild("MobileNotifications").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("MobileNotificationsButton").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
				this.gameObject.transform.FindChild("MobileNotifications").GetComponent<TextMeshPro>().text=this.nbNotificationsNonRead.ToString();
			}
			else
			{
				this.gameObject.transform.FindChild("MobileNotificationsButton").GetComponent<MobileMenuNotificationsController>().setIsActive(false);
				this.gameObject.transform.FindChild("MobileNotificationsButton").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.greySpriteColor;
				this.gameObject.transform.FindChild("MobileNotifications").gameObject.SetActive(false);
			}
		}
		else
		{
			this.gameObject.transform.FindChild("UserBlock").FindChild("Credits").GetComponent<TextMeshPro>().text=ApplicationModel.player.Money.ToString();
			displayedDivision = this.gameObject.transform.FindChild("UserBlock").FindChild("DivisionIcon").FindChild("Title").GetComponent<TextMeshPro>().text;
			if(displayedDivision!=ApplicationModel.player.CurrentDivision.Id.ToString())
			{
				this.gameObject.transform.FindChild("UserBlock").FindChild("DivisionIcon").GetComponent<DivisionIconController>().setDivision(ApplicationModel.player.CurrentDivision.Id);
			}
			if(ApplicationModel.player.TrainingStatus==-1)
			{
				this.gameObject.transform.FindChild("UserBlock").FindChild("DivisionIcon").gameObject.SetActive(true);
			}
			else
			{
				this.gameObject.transform.FindChild("UserBlock").FindChild("DivisionIcon").gameObject.SetActive(false);
			}
			if(this.nbNotificationsNonRead>0)
			{
				this.gameObject.transform.FindChild("UserBlock").FindChild("Bell").GetComponent<MenuNotificationsController>().reset();
				this.gameObject.transform.FindChild("UserBlock").FindChild("Notifications").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("UserBlock").FindChild("Bell").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
				this.gameObject.transform.FindChild("UserBlock").FindChild("Notifications").GetComponent<TextMeshPro>().text=this.nbNotificationsNonRead.ToString();
			}
			else
			{
				this.gameObject.transform.FindChild("UserBlock").FindChild("Bell").GetComponent<MenuNotificationsController>().setIsActive(false);
				this.gameObject.transform.FindChild("UserBlock").FindChild("Bell").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.greySpriteColor;
				this.gameObject.transform.FindChild("UserBlock").FindChild("Notifications").gameObject.SetActive(false);
			}
		}
	}
	public void setNbNotificationsNonRead(int nb)
	{
		this.nbNotificationsNonRead=nb;
	}
	public void changePage(int i)
	{
		SoundController.instance.playSound(10);
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
		BackOfficeController.instance.toDisconnect ();
	}
	public void notificationsLink()
	{
		if (HelpController.instance.canAccess (-1)) {
			SoundController.instance.playSound (10);
			ApplicationModel.player.GoToNotifications = true;
			if (SceneManager.GetActiveScene ().name == "NewHomePage" && NewHomePageController.instance.getNonReadNotificationsOnCurrentPage () > 0) {
				NewHomePageController.instance.displayNotifications ();
			} else {
				this.homePageLink ();
			}
		}
	}
	public void homePageLink()
	{
		if (HelpController.instance.canAccess (-1)) {
			BackOfficeController.instance.loadScene ("NewHomePage");
		}
	}
	public void profileLink() 
	{
		if (HelpController.instance.canAccess (-1)) 
		{
			SoundController.instance.playSound (10);
			BackOfficeController.instance.loadScene ("NewProfile");
		}
	}
	public void myGameLink() 
	{
		if (HelpController.instance.canAccess (2012)) {
			BackOfficeController.instance.loadScene ("newMyGame");
		}
	}
	public void marketLink() 
	{
		if (HelpController.instance.canAccess (-1)) {
			BackOfficeController.instance.loadScene ("newMarket");
		}
	}
	public void skillBookLink() 
	{
		if (HelpController.instance.canAccess (-1)) {
			BackOfficeController.instance.loadScene ("NewSkillBook");
		}
	}
	public void storeLink() 
	{
		if (HelpController.instance.canAccess (1003)) 
		{
			BackOfficeController.instance.loadScene ("newStore");
		}
	}
	public void playLink() 
	{
		if (HelpController.instance.canAccess (-1)) {
			BackOfficeController.instance.displayPlayPopUp ();
		}
	}
	public void adminBoardLink() 
	{
		if (HelpController.instance.canAccess (-1)) {
			BackOfficeController.instance.loadScene ("AdminBoard");
		}
	}
	public void changeThumbPicture()
	{
        gameObject.transform.FindChild("UserBlock").FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnLargeProfilePicture(ApplicationModel.player.IdProfilePicture);
        gameObject.transform.FindChild("MobilePicture").GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnLargeProfilePicture(ApplicationModel.player.IdProfilePicture);
	}
	#region TUTORIAL FUNCTIONS

	public void helpHandler()
	{
		if (HelpController.instance.canAccess (-1)) {
			SoundController.instance.playSound (10);
			HelpController.instance.helpHandler ();
		}
	}
	public Vector3 returnButtonPosition(int id)
	{
		return gameObject.transform.FindChild("Button"+id).transform.position;
	}
	public Vector3 getButtonPosition(int id)
	{
		Vector3 buttonPosition = new Vector3();
		if(ApplicationDesignRules.isMobileScreen)
		{
			buttonPosition = gameObject.transform.FindChild ("MobileButton" + id).position;
		}
		else
		{
			buttonPosition = gameObject.transform.FindChild ("Button" + id).position;
		}
		buttonPosition.x = buttonPosition.x - ApplicationDesignRules.menuPosition.x;
		buttonPosition.y = buttonPosition.y - ApplicationDesignRules.menuPosition.y;
		return buttonPosition;
	}
	public Vector3 getHelpButtonPosition()
	{
		Vector3 helpButtonPosition = new Vector3();;
		if(ApplicationDesignRules.isMobileScreen)
		{
			helpButtonPosition=gameObject.transform.FindChild ("MobileHelpButton").position;
		}
		else
		{
			helpButtonPosition=gameObject.transform.FindChild ("UserBlock").FindChild ("Help").position;	
		}
		helpButtonPosition.x = helpButtonPosition.x - ApplicationDesignRules.menuPosition.x;
		helpButtonPosition.y = helpButtonPosition.y - ApplicationDesignRules.menuPosition.y;
		return helpButtonPosition;
	}
	#endregion
}

