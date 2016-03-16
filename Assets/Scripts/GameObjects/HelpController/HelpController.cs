using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class HelpController : MonoBehaviour 
{
	public static HelpController instance;
	private HelpRessources ressources;

	// Companion gameobjects

	private GameObject companionDialogBox;
	private GameObject companionDialogTitle;
	private GameObject companionNextButton;
	private GameObject companionNextButtonTitle;
	private GameObject companion;

	// background gameobject

	private GameObject background;

	public int sequenceId;

	// Companion settings

	private string companionTextToDisplay;
	private string companionTextDisplayed;
	private bool toWriteCompanionText;
	private float companionTextTimer;
	private bool displayCompanionNextButton;
	private bool isLeftSide;

	// Background settings

	private bool isSquareBackground;
	private Rect backgroundRect;
	private float backgroundClickableSectionXRatio;
	private float backgroundClickableSectionYRatio;

	// General settings

	private bool toShowCompanion;
	private bool toShowBackground;


	void Update()
	{
		if(toWriteCompanionText)
		{
			this.companionTextTimer=this.companionTextTimer+Time.deltaTime;
			if(this.companionTextTimer>0.03f)
			{
				this.companionTextTimer=0f;
				this.companionTextDisplayed=this.companionTextToDisplay.Substring(0,this.companionTextDisplayed.Length+1);
				this.companionDialogTitle.GetComponent<TextMeshPro>().text=this.companionTextDisplayed;
				if(this.companionTextDisplayed.Length>=this.companionTextToDisplay.Length)
				{
					this.toWriteCompanionText=false;
				}
			}
		}
	}
	public void initialize()
	{
		instance = this;
		this.sequenceId=-1;
		this.ressources = this.gameObject.GetComponent<HelpRessources> ();
		this.companion = this.gameObject.transform.FindChild ("Companion").gameObject;
		this.companionDialogBox = this.companion.transform.FindChild ("Dialog").gameObject;
		this.companionDialogTitle = this.companionDialogBox.transform.FindChild ("Title").gameObject;
		this.companionNextButton = this.companion.transform.FindChild ("NextButton").gameObject;
		this.companionNextButtonTitle = this.companionNextButton.transform.FindChild ("Title").gameObject;
		this.background=this.gameObject.transform.FindChild("Background").gameObject;
		this.companion.SetActive(false);
		this.background.SetActive(false);
	}
	public void resize()
	{
		this.gameObject.transform.position=ApplicationDesignRules.tutorialPosition;
		this.companion.transform.localScale=ApplicationDesignRules.companionScale;


	}
	public void helpHandler()
	{
		this.startHelp();
	}
	public virtual void startHelp()
	{
		this.sequenceId = 0;
		this.launchSequence ();
	}
	public virtual void companionNextButtonHandler()
	{
	}
	private void resetSettings()
	{
		this.toShowCompanion = false;
		this.toWriteCompanionText=false;
		this.toShowBackground = false;
	}
	public void launchSequence()
	{
		this.resetSettings();
		this.getSequenceSettings();
		this.showSequence();
	}
	private void showSequence()
	{
		this.showCompanion();
		this.showBackground();
	}
	public virtual void getSequenceSettings()
	{
	}
	public void setCompanion(string textToDisplay, bool displayNextButton, bool isLeftSide)
	{
		this.toShowCompanion=true;
		this.companionTextToDisplay=textToDisplay;
		this.displayCompanionNextButton=displayNextButton;
		this.isLeftSide=isLeftSide;
	}
	private void showCompanion()
	{
		if(this.toShowCompanion)
		{
			this.companion.SetActive(true);
			if(this.companionTextToDisplay!="")
			{
				this.companionDialogBox.SetActive(true);
				this.companionDialogTitle.SetActive(true);
				Vector3 dialogBoxPosition = this.companionDialogBox.transform.localPosition;
				Vector3 dialogTitlePosition = this.companionDialogTitle.transform.localPosition;
				if (companionTextToDisplay.Length < 100) 
				{
					this.companionDialogBox.GetComponent<SpriteRenderer> ().sprite = ressources.dialogs [0];
					dialogBoxPosition.y=2.4f;
					dialogTitlePosition.y=0.86f;
				} 
				else if (companionTextToDisplay.Length< 300) 
				{
					this.companionDialogBox.GetComponent<SpriteRenderer> ().sprite = ressources.dialogs [1];
					dialogBoxPosition.y=3f;
					dialogTitlePosition.y=1.42f;
				} 
				else 
				{
					this.companionDialogBox.GetComponent<SpriteRenderer> ().sprite = ressources.dialogs [2];
					dialogBoxPosition.y=3.55f;
					dialogTitlePosition.y=1.92f;
				}
				this.companionDialogBox.transform.localPosition=dialogBoxPosition;
				this.companionTextDisplayed="";
				this.toWriteCompanionText=true;
			}
			else
			{
				this.companionDialogBox.SetActive(false);
				this.companionDialogTitle.SetActive(false);
			}
			this.companionNextButton.SetActive(this.displayCompanionNextButton);
			if(this.isLeftSide)
			{
				this.companion.transform.rotation=Quaternion.Euler(0,0,0);
				this.companionDialogTitle.transform.rotation=Quaternion.Euler(0,0,0);
				this.companionNextButtonTitle.transform.rotation=Quaternion.Euler(0,0,0);
				if(ApplicationDesignRules.isMobileScreen)
				{
					this.companion.transform.localPosition=new Vector3(0.2f-ApplicationDesignRules.worldWidth/2f+ApplicationDesignRules.leftMargin+ApplicationDesignRules.companionWorldSize.x/2f,0.2f-ApplicationDesignRules.worldHeight/2f+ApplicationDesignRules.bottomBarWorldSize.y/2+ApplicationDesignRules.companionWorldSize.y/2f,0);
				}
				else
				{
					this.companion.transform.localPosition=new Vector3(0.2f-ApplicationDesignRules.worldWidth/2f+ApplicationDesignRules.leftMargin+ApplicationDesignRules.companionWorldSize.x/2f,0.2f-ApplicationDesignRules.worldHeight/2f+ApplicationDesignRules.downMargin+ApplicationDesignRules.companionWorldSize.y/2f,0);
				}
			}
			else
			{
				this.companion.transform.localRotation=Quaternion.Euler(0,180,0);
				this.companionDialogTitle.transform.localRotation=Quaternion.Euler(0,180,0);
				this.companionNextButtonTitle.transform.localRotation=Quaternion.Euler(0,180,0);
				if(ApplicationDesignRules.isMobileScreen)
				{
					this.companion.transform.localPosition=new Vector3(-0.2f+ApplicationDesignRules.worldWidth/2f-ApplicationDesignRules.rightMargin-ApplicationDesignRules.companionWorldSize.x/2f,0.2f-ApplicationDesignRules.worldHeight/2f+ApplicationDesignRules.bottomBarWorldSize.y/2+ApplicationDesignRules.companionWorldSize.y/2f,0);
				}
				else
				{
					this.companion.transform.localPosition=new Vector3(-0.2f+ApplicationDesignRules.worldWidth/2f-ApplicationDesignRules.rightMargin-ApplicationDesignRules.companionWorldSize.x/2f,0.2f-ApplicationDesignRules.worldHeight/2f+ApplicationDesignRules.downMargin+ApplicationDesignRules.companionWorldSize.y/2f,0);
				}
			}
		}
		else
		{
			this.companion.SetActive(false);
		}
	}
	public void setBackground(bool isSquareBackground, Rect backgroundRect, float backgroundClickableSectionXRatio, float backgroundClickableSectionYRatio)
	{
		this.toShowBackground=true;
		this.isSquareBackground=isSquareBackground;
		this.backgroundRect=backgroundRect;
		this.backgroundClickableSectionXRatio=backgroundClickableSectionXRatio;
		this.backgroundClickableSectionYRatio=backgroundClickableSectionYRatio;
	}
	private void showBackground()
	{
		if(toShowBackground)
		{
			this.background.SetActive(true);
			if(this.isSquareBackground)
			{
				this.background.GetComponent<TutorialBackgroundController> ().setSprite (1);
			}
			else
			{
				this.background.GetComponent<TutorialBackgroundController> ().setSprite (0);
			}
			this.background.GetComponent<TutorialBackgroundController> ().resize (this.backgroundRect,this.backgroundClickableSectionXRatio,this.backgroundClickableSectionYRatio);
		}
		else
		{
			this.background.SetActive(false);
		}
	}
}

