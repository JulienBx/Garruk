using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class GameTutoController : MonoBehaviour 
{
	public static GameTutoController instance;
	private HelpRessources ressources;

	// Companion gameobjects

	private GameObject companionDialogBox;
	private GameObject companionDialogTitle;
	private GameObject companionNextButton;
	private GameObject companionNextButtonTitle;
	private GameObject companion;

	private GameObject background;
	private GameObject arrow;

	// Companion settings

	private bool toWriteCompanionText;
	private string companionTextDisplayed;
	private float companionTextTimer;
	private bool displayCompanionNextButton;
	private bool isCompanionOnLeftSide;
	private bool toSlideCompanion;
	private Vector3 startCompanionSlidingPosition;
	private Vector3 endCompanionSlidingPosition;
	private float companionYPosition;

	// Background settings

	private bool isSquareBackground;
	private Rect backgroundRect;
	private float backgroundClickableSectionXRatio;
	private float backgroundClickableSectionYRatio;

	// Arrow settings

	private string arrowOrientation;
	private Vector3 startArrowPosition;
	private Vector3 currentArrowPosition;
	private Vector3 endArrowPosition;
	private bool toMoveArrow;
	private bool isArrowMovingBack;
	private float arrowTimer;
	private float arrowSpeed;

	// General settings

	private bool toDisplayHelpController;
	private bool isTutorial;
	public bool isMiniCompanionClicked;
	private bool canSwipe;
	private bool canScroll;
	private bool toDetectScrolling;

	void Update()
	{
//		if(this.toDisplayHelpController)
//		{
			if(this.toWriteCompanionText)
			{
				this.drawCompanionText();
			}
			if (this.toSlideCompanion) 
			{
				this.slideCompanion();
			}
//			if (this.isFlashingBlock) 
//			{
//				this.drawFlashingBlock();
//			}
			if(this.toMoveArrow)
			{
				this.drawArrow();
			}
//			if(this.isMovingScrolling)
//			{
//				this.drawScrolling();
//			}
//			if (this.isMovingDragging) 
//			{
//				this.drawDragging ();
//			}
//			if (this.isFlashingMiniCompanion) 
//			{
//				this.drawMiniCompanion ();
//			}
//			if(this.toDetectScrolling && this.getIsScrolling())
//			{
//				this.launchTutorialSequence();
//			}
//		}
	}
	void Awake()
	{
		print("Je me réveille");
		instance = this;
		this.arrowSpeed=2.5f;
		this.ressources = this.gameObject.GetComponent<HelpRessources> ();
		this.companion = this.gameObject.transform.FindChild("Companion").gameObject;
		this.companionDialogBox = this.companion.transform.FindChild("Dialog").gameObject;
		this.companionDialogTitle = this.companionDialogBox.transform.FindChild ("Title").gameObject;
		this.companionNextButton = this.companion.transform.FindChild ("NextButton").gameObject;
		this.companionNextButtonTitle = this.companionNextButton.transform.FindChild ("Title").gameObject;
		this.background=this.gameObject.transform.FindChild("Background").gameObject;
		this.arrow = this.gameObject.transform.FindChild ("Arrow").gameObject;
		this.showSequence(false, false, false);
		this.resize();
	}

	public void resize()
	{
		this.transform.localPosition= new Vector3(0f,0f,0f);
		//this.gameObject.transform.position=ApplicationDesignRules.helpPosition;
		this.companion.transform.localScale=ApplicationDesignRules.companionScale;
		this.arrow.transform.localScale=ApplicationDesignRules.helpArrowScale;
	}

	public void companionNextButtonHandler()
	{
		SoundController.instance.playSound(8);
		GameView.instance.hitNextTutorial();
	}

	public void showSequence(bool b1, bool b2, bool b3)
	{
		this.showCompanion(b1);
		this.showBackground(b2);
		this.showArrow(b3);
	}

	public void setCompanion(string textToDisplay, bool displayNextButton, bool isLeftSide, bool toSlideCompanion, float companionYPosition)
	{
		this.companionTextDisplayed=textToDisplay;
		this.displayCompanionNextButton=displayNextButton;
		this.isCompanionOnLeftSide=isLeftSide;
		this.toSlideCompanion = toSlideCompanion;
		this.companionYPosition = companionYPosition;
	}

	private void showCompanion(bool b)
	{
		if(b){
			Vector3 dialogBoxPosition = this.companionDialogBox.transform.localPosition;
			Vector3 dialogTitlePosition = this.companionDialogTitle.transform.localPosition;
		
			if (this.companionTextDisplayed.Length < 100) 
			{
				this.companionDialogBox.GetComponent<SpriteRenderer> ().sprite = ressources.dialogs [0];
				dialogBoxPosition.y=2.4f;
				dialogTitlePosition.y=0.86f;
			} 
			else if (companionTextDisplayed.Length< 300) 
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
			this.companionDialogTitle.transform.localPosition = dialogTitlePosition;

			this.companionDialogTitle.GetComponent<TextMeshPro>().text=this.companionTextDisplayed;
			this.companionDialogTitle.GetComponent<TextMeshPro>().maxVisibleCharacters=0;

			if(this.isCompanionOnLeftSide)
			{
				this.companion.transform.rotation=Quaternion.Euler(0,0,0);
				this.companionDialogTitle.transform.rotation=Quaternion.Euler(0,0,0);
				this.companionNextButtonTitle.transform.rotation=Quaternion.Euler(0,0,0);
				if(ApplicationDesignRules.isMobileScreen)
				{
					this.endCompanionSlidingPosition=new Vector3(0.2f-ApplicationDesignRules.worldWidth/2f+ApplicationDesignRules.leftMargin+ApplicationDesignRules.companionWorldSize.x/2f,this.companionYPosition+0.2f-ApplicationDesignRules.worldHeight/2f+ApplicationDesignRules.companionWorldSize.y/2f,-9.5f);
				}
				else
				{
					this.endCompanionSlidingPosition=new Vector3(0.2f-ApplicationDesignRules.worldWidth/2f+ApplicationDesignRules.leftMargin+ApplicationDesignRules.companionWorldSize.x/2f,this.companionYPosition+0.2f-ApplicationDesignRules.worldHeight/2f+ApplicationDesignRules.downMargin+ApplicationDesignRules.companionWorldSize.y/2f,-9.5f);
				}
			}
			else
			{
				this.companion.transform.localRotation=Quaternion.Euler(0,180,0);
				this.companionDialogTitle.transform.localRotation=Quaternion.Euler(0,180,0);
				this.companionNextButtonTitle.transform.localRotation=Quaternion.Euler(0,180,0);
				if(ApplicationDesignRules.isMobileScreen)
				{
					this.endCompanionSlidingPosition=new Vector3(-0.2f+ApplicationDesignRules.worldWidth/2f-ApplicationDesignRules.rightMargin-ApplicationDesignRules.companionWorldSize.x/2f,this.companionYPosition+0.2f-ApplicationDesignRules.worldHeight/2f+ApplicationDesignRules.companionWorldSize.y/2f,-9.5f);
				}
				else
				{
					this.endCompanionSlidingPosition=new Vector3(-0.2f+ApplicationDesignRules.worldWidth/2f-ApplicationDesignRules.rightMargin-ApplicationDesignRules.companionWorldSize.x/2f,this.companionYPosition+0.2f-ApplicationDesignRules.worldHeight/2f+ApplicationDesignRules.downMargin+ApplicationDesignRules.companionWorldSize.y/2f,-9.5f);
				}
			}

			if (this.toSlideCompanion) 
			{
				if (this.isCompanionOnLeftSide) 
				{
					this.startCompanionSlidingPosition = new Vector3 (this.endCompanionSlidingPosition.x - 5f, this.endCompanionSlidingPosition.y, this.endCompanionSlidingPosition.z);
				} 
				else 
				{
					this.startCompanionSlidingPosition = new Vector3 (this.endCompanionSlidingPosition.x + 5f, this.endCompanionSlidingPosition.y, this.endCompanionSlidingPosition.z);
				}
				this.companion.transform.localPosition = this.startCompanionSlidingPosition;
			} 
			else 
			{
				this.companion.transform.localPosition = this.endCompanionSlidingPosition;
			}
		}
		this.toWriteCompanionText = b ;
		this.companion.GetComponent<SpriteRenderer>().enabled = b ;
		this.companion.transform.FindChild("Background").GetComponent<SpriteRenderer>().enabled = b ;
		this.companionNextButton.GetComponent<HelpCompanionGameController>().reset ();
		this.companionDialogBox.GetComponent<SpriteRenderer>().enabled = b ;
		this.companionDialogBox.transform.FindChild("Title").GetComponent<MeshRenderer>().enabled = b ;
		this.companionNextButton.GetComponent<BoxCollider2D>().enabled = b ;
		this.companionNextButton.GetComponent<SpriteRenderer>().enabled = b ;
		this.companionNextButton.transform.FindChild("Title").GetComponent<MeshRenderer>().enabled = b ;
	}

	private void slideCompanion()
	{
		Vector3 companionCurrentPosition = this.companion.transform.localPosition;
		if (this.isCompanionOnLeftSide) 
		{
			companionCurrentPosition.x = companionCurrentPosition.x + 20f*Time.deltaTime;
			if (companionCurrentPosition.x >= this.endCompanionSlidingPosition.x) 
			{
				this.toSlideCompanion = false;
				companionCurrentPosition.x = endCompanionSlidingPosition.x;
			}
		} 
		else 
		{
			companionCurrentPosition.x = companionCurrentPosition.x - 20f*Time.deltaTime;
			if (companionCurrentPosition.x <= this.endCompanionSlidingPosition.x) 
			{
				this.toSlideCompanion = false;
				companionCurrentPosition.x = endCompanionSlidingPosition.x;
			}
		}
		this.companion.transform.localPosition = companionCurrentPosition;
	}

	private void drawCompanionText()
	{
		this.companionTextTimer=this.companionTextTimer+Time.deltaTime;
		if(this.companionTextTimer>0.015f)
		{
			this.companionTextTimer=0f;
			this.companionDialogTitle.GetComponent<TextMeshPro>().maxVisibleCharacters++;
			if(this.companionDialogTitle.GetComponent<TextMeshPro>().maxVisibleCharacters>=this.companionTextDisplayed.Length)
			{
				this.toWriteCompanionText=false;
			}
		}
	}

	public void setBackground(bool isSquareBackground, Rect backgroundRect, float backgroundClickableSectionXRatio, float backgroundClickableSectionYRatio)
	{
		this.isSquareBackground=isSquareBackground;
		this.backgroundRect=backgroundRect;
		this.backgroundClickableSectionXRatio=backgroundClickableSectionXRatio;
		this.backgroundClickableSectionYRatio=backgroundClickableSectionYRatio;
	}

	private void showBackground(bool b)
	{
		if(b)
		{
			if(this.isSquareBackground)
			{
				this.background.GetComponent<HelpBackgroundController> ().setSprite (1);
			}
			else
			{
				this.background.GetComponent<HelpBackgroundController> ().setSprite (0);
			}
			this.background.GetComponent<HelpBackgroundController> ().resize (this.backgroundRect,this.backgroundClickableSectionXRatio,this.backgroundClickableSectionYRatio);
		}

		this.background.GetComponent<SpriteRenderer>().enabled = b ; 
		BoxCollider[] components = this.background.GetComponents<BoxCollider>();
		for(int i = 0 ; i < components.Length ; i++){
			components[i].enabled = b ;
		}
	}

	public void setArrow(string orientation, Vector3 position)
	{
		this.arrowOrientation = orientation;
		this.endArrowPosition = position;
		this.toMoveArrow = true;
	}

	private void showArrow(bool b)
	{
		if (b) 
		{
			float range=1f;
			if(ApplicationDesignRules.isMobileScreen)
			{
				range=0.75f;
			}
			if (this.arrowOrientation == "left") 
			{
				this.endArrowPosition.x=this.endArrowPosition.x+ApplicationDesignRules.helpArrowWorldSize.x/2f;
				this.arrow.transform.localRotation=Quaternion.Euler(0f,0f,180f);
				this.startArrowPosition = new Vector3(this.endArrowPosition.x+range,this.endArrowPosition.y,this.endArrowPosition.z);
			} 
			else if (this.arrowOrientation == "right") 
			{
				this.endArrowPosition.x=this.endArrowPosition.x-ApplicationDesignRules.helpArrowWorldSize.x/2f;
				this.arrow.transform.localRotation=Quaternion.Euler(0f,0f,0f);
				this.startArrowPosition = new Vector3(this.endArrowPosition.x-range,this.endArrowPosition.y,this.endArrowPosition.z);
			} 
			else if (this.arrowOrientation == "up") 
			{
				this.endArrowPosition.y=this.endArrowPosition.y-ApplicationDesignRules.helpArrowWorldSize.x/2f;
				this.arrow.transform.localRotation=Quaternion.Euler(0f,0f,90f);
				this.startArrowPosition = new Vector3(this.endArrowPosition.x,this.endArrowPosition.y-range,this.endArrowPosition.z);
			} 
			else if (this.arrowOrientation == "down") 
			{
				this.endArrowPosition.y=this.endArrowPosition.y+ApplicationDesignRules.helpArrowWorldSize.x/2f;
				this.arrow.transform.localRotation=Quaternion.Euler(0f,0f,270f);
				this.startArrowPosition = new Vector3(this.endArrowPosition.x,this.endArrowPosition.y+range,this.endArrowPosition.z);
			}
			this.currentArrowPosition=this.startArrowPosition;
			this.isArrowMovingBack=false;

		} 
		this.arrow.GetComponent<SpriteRenderer>().enabled = b ;
	}

	private void drawArrow()
	{
		if(this.arrowOrientation=="left")
		{
			if(this.isArrowMovingBack)
			{
				this.currentArrowPosition.x=this.currentArrowPosition.x+Time.deltaTime*this.arrowSpeed;
				if(this.currentArrowPosition.x>=this.startArrowPosition.x)
				{
					this.isArrowMovingBack=false;
					this.currentArrowPosition=this.startArrowPosition;
				}
			}
			else
			{
				this.currentArrowPosition.x=this.currentArrowPosition.x-Time.deltaTime*this.arrowSpeed;
				if(this.currentArrowPosition.x<=this.endArrowPosition.x)
				{
					this.isArrowMovingBack=true;
					this.currentArrowPosition=this.endArrowPosition;
				}
			}
		}
		else if(this.arrowOrientation=="right")
		{
			if(this.isArrowMovingBack)
			{
				this.currentArrowPosition.x=this.currentArrowPosition.x-Time.deltaTime*this.arrowSpeed;
				if(this.currentArrowPosition.x<=this.startArrowPosition.x)
				{
					this.isArrowMovingBack=false;
					this.currentArrowPosition=this.startArrowPosition;
				}
			}
			else
			{
				this.currentArrowPosition.x=this.currentArrowPosition.x+Time.deltaTime*this.arrowSpeed;
				if(this.currentArrowPosition.x>=this.endArrowPosition.x)
				{
					this.isArrowMovingBack=true;
					this.currentArrowPosition=this.endArrowPosition;
				}
			}
		}
		else if(this.arrowOrientation=="up")
		{
			if(this.isArrowMovingBack)
			{
				this.currentArrowPosition.y=this.currentArrowPosition.y-Time.deltaTime*this.arrowSpeed;
				if(this.currentArrowPosition.y<=this.startArrowPosition.y)
				{
					this.isArrowMovingBack=false;
					this.currentArrowPosition=this.startArrowPosition;
				}
			}
			else
			{
				this.currentArrowPosition.y=this.currentArrowPosition.y+Time.deltaTime*this.arrowSpeed;
				if(this.currentArrowPosition.y>=this.endArrowPosition.y)
				{
					this.isArrowMovingBack=true;
					this.currentArrowPosition=this.endArrowPosition;
				}
			}
		}
		else if(this.arrowOrientation=="down")
		{
			if(this.isArrowMovingBack)
			{
				this.currentArrowPosition.y=this.currentArrowPosition.y+Time.deltaTime*this.arrowSpeed;
				if(this.currentArrowPosition.y>=this.startArrowPosition.y)
				{
					this.isArrowMovingBack=false;
					this.currentArrowPosition=this.startArrowPosition;
				}
			}
			else
			{
				this.currentArrowPosition.y=this.currentArrowPosition.y-Time.deltaTime*this.arrowSpeed;
				if(this.currentArrowPosition.y<=this.endArrowPosition.y)
				{
					this.isArrowMovingBack=true;
					this.currentArrowPosition=this.endArrowPosition;
				}
			}
		}
		this.arrow.transform.localPosition=this.currentArrowPosition;
	}
}