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

	// arrow gameobject

	private GameObject arrow;

	// scrolling gameobject

	private GameObject scrolling;

	// drag gameobject

	private GameObject drag;
	private GameObject dragCard0;
	private GameObject dragCard1;

	// miniCopanion gameobejct

	private GameObject minicompanion;

	public int sequenceId;

	// Companion settings

	private bool toShowCompanion;
	private string companionTextDisplayed;
	private bool toWriteCompanionText;
	private float companionTextTimer;
	private bool displayCompanionNextButton;
	private bool isCompanionOnLeftSide;
	private bool toSlideCompanion;
	private bool isSlidingCompanion;
	private Vector3 startCompanionSlidingPosition;
	private Vector3 endCompanionSlidingPosition;
	private float companionYPosition;

	// Background settings

	private bool toShowBackground;
	private bool isSquareBackground;
	private Rect backgroundRect;
	private float backgroundClickableSectionXRatio;
	private float backgroundClickableSectionYRatio;

	// FlashingBlock settings

	private bool toFlashBlock;
	private bool isFlashingBlock;
	private float flashingBlockTimer;
	private GameObject blockToFlash;
	private bool isFlashingBlockDark;

	// Arrow settings

	private string arrowOrientation;
	private Vector3 startArrowPosition;
	private Vector3 currentArrowPosition;
	private Vector3 endArrowPosition;
	private bool toMoveArrow;
	private bool isMovingArrow;
	private bool isArrowMovingBack;
	private float arrowTimer;
	private float arrowSpeed;

	// Scrolling settings

	private string scrollingOrientation;
	private Vector3 startScrollingPosition;
	private Vector3 currentScrollingPosition;
	private Vector3 endScrollingPosition;
	private bool toMoveScrolling;
	private bool isMovingScrolling;
	private float scrollingTimer;
	private float scrollingSpeed;

	// General settings

	private bool toDisplayHelp;



	void Update()
	{
		if(this.toDisplayHelp)
		{
			if(this.toWriteCompanionText)
			{
				this.drawCompanionText();
			}
			if (this.isSlidingCompanion) 
			{
				this.slideCompanion();
			}
			if (this.isFlashingBlock) 
			{
				this.drawFlashingBlock();
			}
			if(this.isMovingArrow)
			{
				this.drawArrow();
			}
			if(this.isMovingScrolling)
			{
				this.drawScrolling();
			}
		}
	}
	public void initialize()
	{
		instance = this;
		this.sequenceId=-1;
		this.arrowSpeed=2.5f;
		this.scrollingSpeed=2f;
		this.ressources = this.gameObject.GetComponent<HelpRessources> ();
		this.companion = this.gameObject.transform.FindChild ("Companion").gameObject;
		this.companionDialogBox = this.companion.transform.FindChild ("Dialog").gameObject;
		this.companionDialogTitle = this.companionDialogBox.transform.FindChild ("Title").gameObject;
		this.companionNextButton = this.companion.transform.FindChild ("NextButton").gameObject;
		this.companionNextButtonTitle = this.companionNextButton.transform.FindChild ("Title").gameObject;
		this.background=this.gameObject.transform.FindChild("Background").gameObject;
		this.arrow = this.gameObject.transform.FindChild ("Arrow").gameObject;
		this.scrolling = this.gameObject.transform.FindChild ("Scrolling").gameObject;
		this.drag = this.gameObject.transform.FindChild ("Drag").gameObject;
		this.dragCard0 = this.drag.transform.FindChild ("Card0").gameObject;
		this.dragCard1 = this.drag.transform.FindChild ("Card1").gameObject;
		this.minicompanion = this.gameObject.transform.FindChild ("miniCompanion").gameObject;
		this.companion.SetActive(false);
		this.background.SetActive(false);
		this.arrow.SetActive (false);
		this.scrolling.SetActive (false);
		this.drag.SetActive (false);
		this.minicompanion.SetActive (false);
	}
	public void resize()
	{
		this.gameObject.transform.position=ApplicationDesignRules.tutorialPosition;
		this.companion.transform.localScale=ApplicationDesignRules.companionScale;
		this.arrow.transform.localScale=ApplicationDesignRules.helpArrowScale;
		this.scrolling.transform.localScale=ApplicationDesignRules.helpScrollingScale;
		this.launchSequence ();
	}
	public void helpHandler()
	{
		this.startHelp();
	}
	public virtual void startHelp()
	{
		this.sequenceId = 0;
		this.toDisplayHelp=true;
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
		this.toSlideCompanion = false;
		this.toFlashBlock = false;
		this.isFlashingBlock = false;
		if (this.isFlashingBlockDark) 
		{
			this.blockToFlash.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f);
		}
		this.isFlashingBlockDark = false;
		this.toMoveArrow = false;
		this.isMovingArrow = false;
		this.toMoveScrolling = false;
		this.isMovingScrolling = false;
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
		this.showFlashingBlock ();
		this.showArrow();
		this.showScrolling();
	}
	public virtual void getSequenceSettings()
	{
	}

	public void quitHelp()
	{
		this.sequenceId = -1;
		this.resetSettings ();
		this.showSequence ();
		this.toDisplayHelp=false;
	}

	#region Companion Methods

	public void setCompanion(string textToDisplay, bool displayNextButton, bool isLeftSide, bool toSlideCompanion, float companionYPosition)
	{
		this.toShowCompanion=true;
		this.companionTextDisplayed=textToDisplay;
		this.displayCompanionNextButton=displayNextButton;
		this.isCompanionOnLeftSide=isLeftSide;
		this.toSlideCompanion = toSlideCompanion;
		this.companionYPosition = companionYPosition;
	}
	private void showCompanion()
	{
		if(this.toShowCompanion)
		{
			this.companion.SetActive(true);
			if (this.toSlideCompanion) 
			{
				
			}
			if(this.companionTextDisplayed!="")
			{
				this.companionDialogBox.SetActive(true);
				this.companionDialogTitle.SetActive(true);
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
				this.toWriteCompanionText=true;
			}
			else
			{
				this.companionDialogBox.SetActive(false);
				this.companionDialogTitle.SetActive(false);
			}
			this.companionNextButton.SetActive(this.displayCompanionNextButton);
			this.companionNextButton.GetComponent<HelpCompanionNextButtonController> ().reset ();
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
				this.isSlidingCompanion = true;
				this.companion.transform.localPosition = this.startCompanionSlidingPosition;
			} 
			else 
			{
				this.companion.transform.localPosition = this.endCompanionSlidingPosition;
			}
		}
		else
		{
			this.companion.SetActive(false);
		}
	}
	private void slideCompanion()
	{
		Vector3 companionCurrentPosition = this.companion.transform.localPosition;
		if (this.isCompanionOnLeftSide) 
		{
			companionCurrentPosition.x = companionCurrentPosition.x + 20f*Time.deltaTime;
			if (companionCurrentPosition.x >= this.endCompanionSlidingPosition.x) 
			{
				this.isSlidingCompanion = false;
				companionCurrentPosition.x = endCompanionSlidingPosition.x;
			}
		} 
		else 
		{
			companionCurrentPosition.x = companionCurrentPosition.x - 20f*Time.deltaTime;
			if (companionCurrentPosition.x <= this.endCompanionSlidingPosition.x) 
			{
				this.isSlidingCompanion = false;
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
			this.companionDialogTitle.GetComponent<TextMeshPro>().maxVisibleCharacters=this.companionDialogTitle.GetComponent<TextMeshPro>().maxVisibleCharacters+1;
			if(this.companionDialogTitle.GetComponent<TextMeshPro>().maxVisibleCharacters>=this.companionTextDisplayed.Length)
			{
				this.toWriteCompanionText=false;
			}
		}
	}

	#endregion

	#region Flashing block Methods

	public void setFlashingBlock(GameObject blockToFlash)
	{
		this.toFlashBlock = true;
		this.blockToFlash = blockToFlash;
	}
	private void showFlashingBlock()
	{
		if (this.toFlashBlock) 
		{
			this.background.SetActive (true);
			this.background.GetComponent<TutorialBackgroundController> ().setSprite (1);
			Vector3 gameObjectPosition = this.blockToFlash.GetComponent<NewBlockController> ().getOriginPosition ();
			Vector2 gameObjectSize=this.blockToFlash.GetComponent<NewBlockController> ().getSize ();
			if (ApplicationDesignRules.isMobileScreen) 
			{
				this.background.GetComponent<TutorialBackgroundController> ().resize (new Rect(0f,gameObjectPosition.y-ApplicationDesignRules.topBarWorldSize.y+0.2f,gameObjectSize.x-0.2f,gameObjectSize.y-0.2f),0f,0f);
			} 
			else 
			{
				this.background.GetComponent<TutorialBackgroundController> ().resize (new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			}
			this.isFlashingBlock = true;
		}
	}
	private void drawFlashingBlock()
	{
		this.flashingBlockTimer = this.flashingBlockTimer + Time.deltaTime;
		if (this.flashingBlockTimer > 0.5f) 
		{
			if (this.isFlashingBlockDark) 
			{
				this.blockToFlash.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f);
				this.isFlashingBlockDark = false;
			} 
			else 
			{
				this.blockToFlash.GetComponent<SpriteRenderer> ().color = new Color (0.5f, 0.5f, 0.5f);
				this.isFlashingBlockDark = true;
			}
			this.flashingBlockTimer = 0f;
		}
	}

	#endregion

	#region Background Methods

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

	#endregion

	#region Arrow Methods

	public void setArrow(string orientation, Vector3 position)
	{
		this.arrowOrientation = orientation;
		this.endArrowPosition = position;
		this.toMoveArrow = true;
	}
	private void showArrow()
	{
		if (this.toMoveArrow) 
		{
			this.arrow.SetActive(true);
			if (this.arrowOrientation == "left") 
			{
				this.endArrowPosition.x=this.endArrowPosition.x+ApplicationDesignRules.helpArrowWorldSize.x/2f;
				this.arrow.transform.localRotation=Quaternion.Euler(0f,0f,180f);
				this.startArrowPosition = new Vector3(this.endArrowPosition.x+1f,this.endArrowPosition.y,this.endArrowPosition.z);
			} 
			else if (this.arrowOrientation == "right") 
			{
				this.endArrowPosition.x=this.endArrowPosition.x-ApplicationDesignRules.helpArrowWorldSize.x/2f;
				this.arrow.transform.localRotation=Quaternion.Euler(0f,0f,0f);
				this.startArrowPosition = new Vector3(this.endArrowPosition.x-1f,this.endArrowPosition.y,this.endArrowPosition.z);
			} 
			else if (this.arrowOrientation == "up") 
			{
				this.endArrowPosition.y=this.endArrowPosition.y-ApplicationDesignRules.helpArrowWorldSize.x/2f;
				this.arrow.transform.localRotation=Quaternion.Euler(0f,0f,90f);
				this.startArrowPosition = new Vector3(this.endArrowPosition.x,this.endArrowPosition.y-1f,this.endArrowPosition.z);
			} 
			else if (this.arrowOrientation == "down") 
			{
				this.endArrowPosition.y=this.endArrowPosition.y+ApplicationDesignRules.helpArrowWorldSize.x/2f;
				this.arrow.transform.localRotation=Quaternion.Euler(0f,0f,270f);
				this.startArrowPosition = new Vector3(this.endArrowPosition.x,this.endArrowPosition.y+1f,this.endArrowPosition.z);
			}
			this.isMovingArrow = true;
			this.isArrowMovingBack=false;
			this.currentArrowPosition=this.startArrowPosition;
		} 
		else 
		{
			this.arrow.SetActive(false);
		}
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

	#endregion

	#region Scrolling Methods

	public void setScrolling(string orientation, Vector3 position)
	{
		this.scrollingOrientation=orientation;
		this.endScrollingPosition=position;
		this.toMoveScrolling = true;
	}
	private void showScrolling()
	{
		if(this.toMoveScrolling)
		{
			this.scrolling.SetActive(true);
			if (this.scrollingOrientation == "up") 
			{
				this.endScrollingPosition.y=this.endScrollingPosition.y+1f;
				this.scrolling.GetComponent<SpriteRenderer>().sprite=ressources.scrollingSprites[0];
				this.startScrollingPosition.y = this.endScrollingPosition.y-2f;
			} 
			else if (this.scrollingOrientation == "down") 
			{
				this.endScrollingPosition.y=this.endScrollingPosition.y-1f;
				this.scrolling.GetComponent<SpriteRenderer>().sprite=ressources.scrollingSprites[1];
				this.startScrollingPosition.y = this.endScrollingPosition.y+2f;
			}
			this.isMovingScrolling=true;
			this.currentScrollingPosition=this.startScrollingPosition;
		}
		else
		{
			this.scrolling.SetActive(false);
		}
	}
	private void drawScrolling()
	{
		if(this.scrollingOrientation=="up")
		{
			this.currentScrollingPosition.y=this.currentScrollingPosition.y+Time.deltaTime*this.scrollingSpeed;
			if(this.currentScrollingPosition.y>=this.endScrollingPosition.y)
			{
				this.currentScrollingPosition=this.startScrollingPosition;
			}
		}
		else if(this.scrollingOrientation=="down")
		{
			this.currentScrollingPosition.y=this.currentScrollingPosition.y-Time.deltaTime*this.scrollingSpeed;
			if(this.currentScrollingPosition.y<=this.endScrollingPosition.y)
			{
				this.currentScrollingPosition=this.startScrollingPosition;
			}
		}
		this.scrolling.transform.localPosition=this.currentScrollingPosition;
	}

	#endregion
}

