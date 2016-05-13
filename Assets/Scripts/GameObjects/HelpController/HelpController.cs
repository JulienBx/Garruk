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

	private GameObject dragging;
	private GameObject draggingCard0;
	private GameObject draggingCard1;

	// miniCompanion gameobejct

	private GameObject miniCompanion;

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
	private bool flashWithBackground;

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

	// Dragging settings

	private string draggingOrientation;
	private Vector3 currentDraggingPosition;
	private Vector3 startDraggingPosition;
	private Vector3 endDraggingPosition;
	private bool toMoveDragging;
	private bool isMovingDragging;
	private float draggingTimer;
	private float draggingSpeed;

	// Mini companion settings

	private bool toShowMiniCompanion;
	private float miniCompanionYPosition;
	private bool isFlashingMiniCompanion;
	private bool isMiniCompanionOnHoveredState;
	private float miniCompanionTimer;
	private float miniCompanionSpeed;

	// General settings

	private bool toDisplayHelpController;
	private bool isTutorial;
	public bool isMiniCompanionClicked;
	private bool canSwipe;
	private bool canScroll;
	private bool toDetectScrolling;

	void Update()
	{
		if(this.toDisplayHelpController)
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
			if (this.isMovingDragging) 
			{
				this.drawDragging ();
			}
			if (this.isFlashingMiniCompanion) 
			{
				this.drawMiniCompanion ();
			}
			if(this.toDetectScrolling && this.getIsScrolling())
			{
				this.launchTutorialSequence();
			}
		}
	}
	public void initialize()
	{
		instance = this;
		this.sequenceId=-1;
		this.arrowSpeed=2.5f;
		this.scrollingSpeed=2f;
		this.draggingSpeed = 2f;
		this.ressources = this.gameObject.GetComponent<HelpRessources> ();
		this.companion = this.gameObject.transform.FindChild ("Companion").gameObject;
		this.companionDialogBox = this.companion.transform.FindChild ("Dialog").gameObject;
		this.companionDialogTitle = this.companionDialogBox.transform.FindChild ("Title").gameObject;
		this.companionNextButton = this.companion.transform.FindChild ("NextButton").gameObject;
		this.companionNextButtonTitle = this.companionNextButton.transform.FindChild ("Title").gameObject;
		this.background=this.gameObject.transform.FindChild("Background").gameObject;
		this.arrow = this.gameObject.transform.FindChild ("Arrow").gameObject;
		this.scrolling = this.gameObject.transform.FindChild ("Scrolling").gameObject;
		this.dragging = this.gameObject.transform.FindChild ("Drag").gameObject;
		this.draggingCard0 = this.dragging.transform.FindChild ("DragCard0").gameObject;
		this.draggingCard1 = this.dragging.transform.FindChild ("DragCard1").gameObject;
		this.miniCompanion = this.gameObject.transform.FindChild ("miniCompanion").gameObject;
		this.companion.SetActive(false);
		this.background.SetActive(false);
		this.arrow.SetActive (false);
		this.scrolling.SetActive (false);
		this.dragging.SetActive (false);
		this.miniCompanion.SetActive (false);
	}
	public void resize()
	{
		this.gameObject.transform.position=ApplicationDesignRules.helpPosition;
		this.companion.transform.localScale=ApplicationDesignRules.companionScale;
		this.arrow.transform.localScale=ApplicationDesignRules.helpArrowScale;
		this.scrolling.transform.localScale=ApplicationDesignRules.helpScrollingScale;
		this.dragging.transform.localScale = ApplicationDesignRules.helpDraggingScale;
		this.miniCompanion.transform.localScale = ApplicationDesignRules.miniCompanionScale;
		if (!this.isTutorial) 
		{
			this.launchHelpSequence ();
		} 
		else 
		{
			this.launchTutorialSequence ();
		}
	}
	public void freeze()
	{
		if(this.toDisplayHelpController)
		{	
			this.resetSettings();
			this.showSequence();
		}
	}
	public void reload()
	{
		if(this.toDisplayHelpController)
		{
			if(this.isTutorial)
			{
				this.launchTutorialSequence();
			}
			else
			{
				this.launchHelpSequence();
			}
		}
	}
	public void helpHandler()
	{
		this.startHelp();
	}
	public void startHelp()
	{
		this.sequenceId = -1;
		this.getHelpNextAction ();
		this.toDisplayHelpController=true;
		this.isTutorial = false;
		ApplicationModel.player.IsBusy=true;
	}
	public void startTutorial()
	{
		this.sequenceId = -1;
		this.getTutorialNextAction ();
		this.toDisplayHelpController = true;
		this.isTutorial = true;
		ApplicationModel.player.IsBusy=true;
	}
	public void companionNextButtonHandler()
	{
		SoundController.instance.playSound(8);
		if (!this.isTutorial) 
		{
			this.getHelpNextAction ();
		} 
		else 
		{
			this.getTutorialNextAction ();
		}
	}
	public virtual void miniCompanionHandler()
	{
		this.isMiniCompanionClicked = true;
		if (!this.isTutorial) 
		{
			this.getHelpNextAction ();
		} 
		else 
		{
			this.getTutorialNextAction ();
		}
	}
	public virtual void getHelpNextAction()
	{
		if(this.sequenceId<103)
		{
			this.sequenceId++;
			this.launchHelpSequence();
		}
		else if(this.sequenceId==103)
		{
			this.quitHelp ();
		}
		else if(this.sequenceId==200)
		{
			this.quitHelp ();
		}
		else if(this.sequenceId==300)
		{
			this.quitHelp ();
			StartCoroutine(ApplicationModel.player.setNextLevelTutorial(true));
		}
        else if(this.sequenceId==400)
        {
            this.quitHelp ();
        }
	}
	public virtual void getTutorialNextAction()
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
		this.isMovingDragging = false;
		this.toMoveDragging = false;
		this.toShowMiniCompanion = false;
		this.isFlashingMiniCompanion = false;
		this.canSwipe = false;
		this.canScroll = false;
		this.toDetectScrolling=false;
		this.flashWithBackground=false;
	}
	public void launchHelpSequence()
	{
		this.resetSettings();
		if (ApplicationDesignRules.isMobileScreen) 
		{
			this.getMobileHelpSequenceSettings ();
		} 
		else 
		{
			this.getDesktopHelpSequenceSettings ();
		}
		this.showSequence();
	}
	public void launchTutorialSequence()
	{
		this.resetSettings();
		if (ApplicationDesignRules.isMobileScreen) 
		{
			this.getMobileTutorialSequenceSettings ();
		} 
		else 
		{
			this.getDesktopTutorialSequenceSettings ();
		}
		this.showSequence();
	}
	private void showSequence()
	{
		this.showCompanion();
		this.showBackground();
		this.showFlashingBlock ();
		this.showArrow();
		this.showScrolling();
		this.showDragging ();
		this.showMiniCompanion ();
	}
	public virtual void getDesktopHelpSequenceSettings()
	{
		switch (this.sequenceId) 
		{
		case 100:
			this.setBackground (true,new Rect(this.getFocusedCardPosition().x,-ApplicationDesignRules.upMargin/2f+this.getFocusedCardPosition().y,5.4f,7.7f),0f,0f);
			this.setArrow("up",new Vector3(-ApplicationDesignRules.focusedCardPosition.x+this.getFocusedCard().GetComponent<NewFocusedCardController>().getCardTypePosition().x,-0.3f-ApplicationDesignRules.upMargin/2f-ApplicationDesignRules.focusedCardPosition.y+this.getFocusedCard().GetComponent<NewFocusedCardController>().getCardTypePosition().y,this.getFocusedCard().GetComponent<NewFocusedCardController>().getCardTypePosition().z));
			this.setCompanion (WordingHelp.getHelpContent (0), true, false, true, 0f);
			break;
		case 101:
			this.setBackground (true,new Rect(this.getFocusedCardPosition().x,-ApplicationDesignRules.upMargin/2f+this.getFocusedCardPosition().y,5.4f,7.7f),0f,0f);
			this.setArrow("right",new Vector3(-ApplicationDesignRules.focusedCardPosition.x+this.getFocusedCard().GetComponent<NewFocusedCardController>().getExperienceLevelPosition().x,-ApplicationDesignRules.upMargin/2f-ApplicationDesignRules.focusedCardPosition.y+this.getFocusedCard().GetComponent<NewFocusedCardController>().getExperienceLevelPosition().y,this.getFocusedCard().GetComponent<NewFocusedCardController>().getExperienceLevelPosition().z));
			this.setCompanion (WordingHelp.getHelpContent (1), true, false, false, 0f);
			break;
		case 102:
			this.setBackground (true,new Rect(this.getFocusedCardPosition().x,-ApplicationDesignRules.upMargin/2f+this.getFocusedCardPosition().y,5.4f,7.7f),0f,0f);
			this.setArrow("left",new Vector3(0.3f-ApplicationDesignRules.focusedCardPosition.x+this.getFocusedCard().GetComponent<NewFocusedCardController>().getLifePosition().x,-ApplicationDesignRules.upMargin/2f-ApplicationDesignRules.focusedCardPosition.y+this.getFocusedCard().GetComponent<NewFocusedCardController>().getLifePosition().y,this.getFocusedCard().GetComponent<NewFocusedCardController>().getLifePosition().z));
			this.setCompanion (WordingHelp.getHelpContent (2), true, false, false, 0f);
			break;
		case 103:
			int skillsIndex = this.getFocusedCard().GetComponent<NewFocusedCardController>().GetSkillsNumber()-1;
			this.setBackground (true,new Rect(this.getFocusedCardPosition().x,-ApplicationDesignRules.upMargin/2f+this.getFocusedCardPosition().y,5.4f,7.7f),0f,0f);
			this.setArrow("down",new Vector3(-ApplicationDesignRules.focusedCardPosition.x+this.getFocusedCard().GetComponent<NewFocusedCardController>().getSkillPosition(skillsIndex).x,0.4f-ApplicationDesignRules.upMargin/2f-ApplicationDesignRules.focusedCardPosition.y+this.getFocusedCard().GetComponent<NewFocusedCardController>().getSkillPosition(skillsIndex).y,this.getFocusedCard().GetComponent<NewFocusedCardController>().getSkillPosition(skillsIndex).z));
			this.setCompanion (WordingHelp.getHelpContent (3), true, false, false, 0f);
			break;
		case 200:
			this.setBackground (true,new Rect(this.getFocusedCardPosition().x,-ApplicationDesignRules.upMargin/2f+this.getFocusedCardPosition().y,5.4f,7.7f),0f,0f);
			this.setCompanion (WordingHelp.getHelpContent (4), true, false, true, 0f);
			break;
		case 300:
			this.setBackground (true,new Rect(this.getFocusedCardPosition().x,-ApplicationDesignRules.upMargin/2f+this.getFocusedCardPosition().y,5.4f,7.7f),0f,0f);
			this.setCompanion (WordingHelp.getHelpContent (5), true, false, true, 0f);
			break;
        case 400:
            this.setBackground (true,new Rect(this.getFocusedSkillPosition().x,-ApplicationDesignRules.upMargin/2f+this.getFocusedSkillPosition().y,5.4f,7.7f),0f,0f);
            this.setCompanion (WordingHelp.getHelpContent (4), true, false, true, 0f);
            break;
		}
	}
	public virtual void getMobileHelpSequenceSettings()
	{
		switch (this.sequenceId) 
		{
		case 100:
			this.setBackground (true,new Rect(this.getFocusedCardPosition().x,this.getFocusedCardPosition().y,4.2f,5.8f),0f,0f);
			this.setArrow("up",new Vector3(-ApplicationDesignRules.focusedCardPosition.x+this.getFocusedCard().GetComponent<NewFocusedCardController>().getCardTypePosition().x,-0.3f-ApplicationDesignRules.focusedCardPosition.y+this.getFocusedCard().GetComponent<NewFocusedCardController>().getCardTypePosition().y,this.getFocusedCard().GetComponent<NewFocusedCardController>().getCardTypePosition().z));
			this.setCompanion (WordingHelp.getHelpContent (0), true, false, true, 0f);
			break;
		case 101:
			this.setBackground (true,new Rect(this.getFocusedCardPosition().x,this.getFocusedCardPosition().y,4.2f,5.8f),0f,0f);
			this.setArrow("right",new Vector3(-ApplicationDesignRules.focusedCardPosition.x+this.getFocusedCard().GetComponent<NewFocusedCardController>().getExperienceLevelPosition().x,-ApplicationDesignRules.focusedCardPosition.y+this.getFocusedCard().GetComponent<NewFocusedCardController>().getExperienceLevelPosition().y,this.getFocusedCard().GetComponent<NewFocusedCardController>().getExperienceLevelPosition().z));
			this.setCompanion (WordingHelp.getHelpContent (1), true, false, false, 0f);
			break;
		case 102:
			this.setBackground (true,new Rect(this.getFocusedCardPosition().x,this.getFocusedCardPosition().y,4.2f,5.8f),0f,0f);
			this.setArrow("left",new Vector3(0.3f-ApplicationDesignRules.focusedCardPosition.x+this.getFocusedCard().GetComponent<NewFocusedCardController>().getLifePosition().x,-ApplicationDesignRules.focusedCardPosition.y+this.getFocusedCard().GetComponent<NewFocusedCardController>().getLifePosition().y,this.getFocusedCard().GetComponent<NewFocusedCardController>().getLifePosition().z));
			this.setCompanion (WordingHelp.getHelpContent (2), true, false, false, 0f);
			break;
		case 103:
			int skillsIndex = this.getFocusedCard().GetComponent<NewFocusedCardController>().GetSkillsNumber()-1;
			this.setBackground (true,new Rect(this.getFocusedCardPosition().x,this.getFocusedCardPosition().y,4.2f,5.8f),0f,0f);
			this.setArrow("down",new Vector3(-ApplicationDesignRules.focusedCardPosition.x+this.getFocusedCard().GetComponent<NewFocusedCardController>().getSkillPosition(skillsIndex).x,0.4f-ApplicationDesignRules.focusedCardPosition.y+this.getFocusedCard().GetComponent<NewFocusedCardController>().getSkillPosition(skillsIndex).y,this.getFocusedCard().GetComponent<NewFocusedCardController>().getSkillPosition(skillsIndex).z));
			this.setCompanion (WordingHelp.getHelpContent (3), true, false, false, 5.5f);
			break;
		case 200:
			this.setBackground (true,new Rect(this.getFocusedCardPosition().x,this.getFocusedCardPosition().y,4.2f,5.8f),0f,0f);
			this.setCompanion (WordingHelp.getHelpContent (4), true, false, true, 0f);
			break;
		case 300:
			this.setBackground (true,new Rect(this.getFocusedCardPosition().x,this.getFocusedCardPosition().y,4.2f,5.8f),0f,0f);
			this.setCompanion (WordingHelp.getHelpContent (5), true, false, true, 0f);
			break;
        case 400:
            this.setBackground (true,new Rect(this.getFocusedSkillPosition().x,this.getFocusedSkillPosition().y,4.2f,5.8f),0f,0f);
            this.setCompanion (WordingHelp.getHelpContent (4), true, false, true, 0f);
            break;
		}
	}
	public virtual void getDesktopTutorialSequenceSettings()
	{
	}
	public virtual void getMobileTutorialSequenceSettings()
	{
	}
	public void quitHelp()
	{
		this.sequenceId = -1;
		this.resetSettings ();
		this.showSequence ();
		this.toDisplayHelpController=false;
		ApplicationModel.player.IsBusy=false;
	}
	public void quitTutorial()
	{
		this.sequenceId = -1;
		this.resetSettings ();
		this.showSequence ();
		this.toDisplayHelpController=false;
		this.isTutorial=false;
		ApplicationModel.player.IsBusy=false;
	}
	public void tutorialTrackPoint()
	{
		if(this.isTutorial)
		{
			this.getTutorialNextAction ();
		}
	}
	public virtual bool getIsScrolling()
	{
		return false;
	}

	#region General Settings Methods

	public void setCanSwipe()
	{
		this.canSwipe = true;
	}
	public void setCanScroll()
	{
		this.canScroll = true;
		this.toDetectScrolling=true;
	}
	public bool getCanSwipe()
	{
		if(this.toDisplayHelpController	)
		{
			return canSwipe;
		}
		else
		{
			return true;
		}
	}
	public bool getCanScroll()
	{
		if(this.toDisplayHelpController	)
		{
			return canScroll;
		}
		else
		{
			return true;
		}
	}
	public bool getIsTutorialLaunched()
	{
		return this.isTutorial;
	}
	public bool canAccess()
	{
		if(this.isTutorial)
		{
			this.displayCantAccessPopUp();
			return false;
		}
		else
		{
			return true;
		}
	}
	public void displayCantAccessPopUp ()
	{
		BackOfficeController.instance.displayErrorPopUp (WordingHelp.getReference(2));
	}
	public virtual GameObject getFocusedCard()
	{
		return new GameObject();
	}
	public virtual Vector3 getFocusedCardPosition()
	{
		return new Vector3();
	}
    public virtual Vector3 getFocusedSkillPosition()
    {
        return new Vector3();
    }
	#endregion

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
                float dialogHeight;
				if (this.companionTextDisplayed.Length < 100) 
				{
					this.companionDialogBox.GetComponent<SpriteRenderer> ().sprite = ressources.dialogs [0];
					dialogBoxPosition.y=2.4f;
					dialogTitlePosition.y=0.75f;
                    dialogHeight=1.52f;
				} 
				else if (companionTextDisplayed.Length< 300) 
				{
					this.companionDialogBox.GetComponent<SpriteRenderer> ().sprite = ressources.dialogs [1];
					dialogBoxPosition.y=3f;
					dialogTitlePosition.y=1.3f;
                    dialogHeight=2.57f;
				} 
				else 
				{
					this.companionDialogBox.GetComponent<SpriteRenderer> ().sprite = ressources.dialogs [2];
					dialogBoxPosition.y=3.55f;
					dialogTitlePosition.y=1.8f;
                    dialogHeight=3.61f;
				}
				this.companionDialogBox.transform.localPosition=dialogBoxPosition;
				this.companionDialogTitle.transform.localPosition = dialogTitlePosition;
				this.companionDialogTitle.GetComponent<TextMeshPro>().text=this.companionTextDisplayed;
				this.companionDialogTitle.GetComponent<TextMeshPro>().maxVisibleCharacters=0;
                this.companionDialogTitle.GetComponent<TextContainer>().height=dialogHeight;
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

	public void setFlashingBlock(GameObject blockToFlash, bool flashWithBackground)
	{
		this.toFlashBlock = true;
		this.blockToFlash = blockToFlash;
		this.flashWithBackground=flashWithBackground;
	}
	private void showFlashingBlock()
	{
		if (this.toFlashBlock) 
		{
			if(this.flashWithBackground)
			{
				this.background.SetActive (true);
				this.background.GetComponent<HelpBackgroundController> ().setSprite (1);
				Vector3 gameObjectPosition = this.blockToFlash.GetComponent<NewBlockController> ().getOriginPosition ();
				Vector2 gameObjectSize=this.blockToFlash.GetComponent<NewBlockController> ().getSize ();

				if(gameObjectSize.y>10f)
				{
					gameObjectPosition.y=gameObjectPosition.y+(gameObjectSize.y-10f)/2f;
					gameObjectSize.y=10f;
				}

				if (ApplicationDesignRules.isMobileScreen) 
				{
					this.background.GetComponent<HelpBackgroundController> ().resize (new Rect(0f,gameObjectPosition.y-ApplicationDesignRules.topBarWorldSize.y+0.2f,gameObjectSize.x,gameObjectSize.y),0f,0f);
				} 
				else 
				{
					this.background.GetComponent<HelpBackgroundController> ().resize (new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x,gameObjectSize.y),0f,0f);
				}
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
				this.background.GetComponent<HelpBackgroundController> ().setSprite (1);
			}
			else
			{
				this.background.GetComponent<HelpBackgroundController> ().setSprite (0);
			}
			this.background.GetComponent<HelpBackgroundController> ().resize (this.backgroundRect,this.backgroundClickableSectionXRatio,this.backgroundClickableSectionYRatio);
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
				if((this.currentArrowPosition.x>this.startArrowPosition.x || this.currentArrowPosition.x<this.endArrowPosition.x))
				{
					this.currentArrowPosition=this.startArrowPosition;
					this.isArrowMovingBack=false;
				}
				else
				{
					this.currentArrowPosition.y=this.startArrowPosition.y;
				}
			} 
			else if (this.arrowOrientation == "right") 
			{
				this.endArrowPosition.x=this.endArrowPosition.x-ApplicationDesignRules.helpArrowWorldSize.x/2f;
				this.arrow.transform.localRotation=Quaternion.Euler(0f,0f,0f);
				this.startArrowPosition = new Vector3(this.endArrowPosition.x-range,this.endArrowPosition.y,this.endArrowPosition.z);
				if((this.currentArrowPosition.x<this.startArrowPosition.x || this.currentArrowPosition.x>this.endArrowPosition.x))
				{
					this.currentArrowPosition=this.startArrowPosition;
					this.isArrowMovingBack=false;
				}
				else
				{
					this.currentArrowPosition.y=this.startArrowPosition.y;
				}
			} 
			else if (this.arrowOrientation == "up") 
			{
				this.endArrowPosition.y=this.endArrowPosition.y-ApplicationDesignRules.helpArrowWorldSize.x/2f;
				this.arrow.transform.localRotation=Quaternion.Euler(0f,0f,90f);
				this.startArrowPosition = new Vector3(this.endArrowPosition.x,this.endArrowPosition.y-range,this.endArrowPosition.z);
				if((this.currentArrowPosition.y<this.startArrowPosition.y || this.currentArrowPosition.y>this.endArrowPosition.y))
				{
					this.currentArrowPosition=this.startArrowPosition;
					this.isArrowMovingBack=false;
				}
				else
				{
					this.currentArrowPosition.x=this.startArrowPosition.x;
				}
			} 
			else if (this.arrowOrientation == "down") 
			{
				this.endArrowPosition.y=this.endArrowPosition.y+ApplicationDesignRules.helpArrowWorldSize.x/2f;
				this.arrow.transform.localRotation=Quaternion.Euler(0f,0f,270f);
				this.startArrowPosition = new Vector3(this.endArrowPosition.x,this.endArrowPosition.y+range,this.endArrowPosition.z);
				if((this.currentArrowPosition.y<this.startArrowPosition.y || this.currentArrowPosition.y>this.endArrowPosition.y))
				{
					this.currentArrowPosition=this.startArrowPosition;
					this.isArrowMovingBack=false;
				}
				else
				{
					this.currentArrowPosition.x=this.startArrowPosition.x;
				}
			}
			this.isMovingArrow = true;

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
				if((this.currentScrollingPosition.y<this.startScrollingPosition.y || this.currentScrollingPosition.y>this.endDraggingPosition.y))
				{
					this.currentScrollingPosition=this.startScrollingPosition;
				}
				else
				{
					this.currentScrollingPosition.x=this.startScrollingPosition.x;
				}
			} 
			else if (this.scrollingOrientation == "down") 
			{
				this.endScrollingPosition.y=this.endScrollingPosition.y-1f;
				this.scrolling.GetComponent<SpriteRenderer>().sprite=ressources.scrollingSprites[1];
				this.startScrollingPosition.y = this.endScrollingPosition.y+2f;
				if((this.currentScrollingPosition.y>this.startScrollingPosition.y || this.currentScrollingPosition.y<this.endDraggingPosition.y))
				{
					this.currentScrollingPosition=this.startScrollingPosition;
				}
				else
				{
					this.currentScrollingPosition.x=this.startScrollingPosition.x;
				}
			}
			this.isMovingScrolling=true;
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

	#region Dragging Methods

	public void setDragging(string orientation, Vector3 position)
	{
		this.toMoveDragging = true;
		this.endDraggingPosition = position;
		this.draggingOrientation = orientation;
	}
	private void showDragging()
	{
		if(this.toMoveDragging)
		{
			this.dragging.SetActive(true);
			this.dragging.transform.localPosition = this.endDraggingPosition;
			if (this.draggingOrientation == "left") 
			{
				this.startDraggingPosition = new Vector3 (1f, 0f,0f);
				this.endDraggingPosition = new Vector3 (-1f, 0f,0f);
			} 
			else if (this.draggingOrientation == "right") 
			{
				this.startDraggingPosition = new Vector3 (-1f, 0f,0f);
				this.endDraggingPosition = new Vector3 (1f, 0f,0f);
			} 
			else if (this.draggingOrientation == "up") 
			{
				this.startDraggingPosition = new Vector3 (0f, -1f,0f);
				this.endDraggingPosition = new Vector3 (0f, 1f,0f);
			} 
			else if (this.draggingOrientation == "down") 
			{
				this.startDraggingPosition = new Vector3 (0f, 1f,0f);
				this.endDraggingPosition = new Vector3 (0f, -1f,0f);
			} 
			this.isMovingDragging=true;
			this.draggingCard0.transform.localPosition = this.startDraggingPosition;
			this.draggingCard1.transform.localPosition = this.endDraggingPosition;
			this.currentDraggingPosition = this.startDraggingPosition;
		}
		else
		{
			this.dragging.SetActive(false);
		}
	}
	private void drawDragging()
	{
		if(this.draggingOrientation=="left")
		{
			this.currentDraggingPosition.x=this.currentDraggingPosition.x-Time.deltaTime*this.draggingSpeed;
			if(this.currentDraggingPosition.x<=this.endDraggingPosition.x)
			{
				this.currentDraggingPosition=this.startDraggingPosition;
			}
		}
		else if(this.draggingOrientation=="right")
		{
			this.currentDraggingPosition.x=this.currentDraggingPosition.x+Time.deltaTime*this.draggingSpeed;
			if(this.currentDraggingPosition.x>=this.endDraggingPosition.x)
			{
				this.currentDraggingPosition=this.startDraggingPosition;
			}
		}
		else if(this.draggingOrientation=="up")
		{
			this.currentDraggingPosition.y=this.currentDraggingPosition.y+Time.deltaTime*this.draggingSpeed;
			if(this.currentDraggingPosition.y>=this.endDraggingPosition.y)
			{
				this.currentDraggingPosition=this.startDraggingPosition;
			}
		}
		else if(this.draggingOrientation=="down")
		{
			this.currentDraggingPosition.y=this.currentDraggingPosition.y-Time.deltaTime*this.draggingSpeed;
			if(this.currentDraggingPosition.y<=this.endDraggingPosition.y)
			{
				this.currentDraggingPosition=this.startDraggingPosition;
			}
		}
		this.draggingCard0.transform.localPosition=this.currentDraggingPosition;
	}

	#endregion

	#region miniCompanion Methods

	public void setMiniCompanion(bool isMiniCompanionOnLeftSide, float miniCompanionPositionY)
	{
		this.toShowMiniCompanion = true;
		this.miniCompanionYPosition = miniCompanionPositionY;
	}
	private void showMiniCompanion()
	{
		if(this.toShowMiniCompanion)
		{
			this.miniCompanion.SetActive(true);
			this.miniCompanion.GetComponent<HelpMiniCompanionController> ().reset ();
			if(this.isCompanionOnLeftSide)
			{
				this.miniCompanion.transform.rotation=Quaternion.Euler(0,0,0);

				if(ApplicationDesignRules.isMobileScreen)
				{
					this.miniCompanion.transform.localPosition=new Vector3(0.2f-ApplicationDesignRules.worldWidth/2f+ApplicationDesignRules.leftMargin+ApplicationDesignRules.miniCompanionWorldSize.x/2f,this.miniCompanionYPosition+0.2f-ApplicationDesignRules.worldHeight/2f+ApplicationDesignRules.miniCompanionWorldSize.y/2f,-9.5f);
				}
				else
				{
					this.miniCompanion.transform.localPosition=new Vector3(0.2f-ApplicationDesignRules.worldWidth/2f+ApplicationDesignRules.leftMargin+ApplicationDesignRules.miniCompanionWorldSize.x/2f,this.miniCompanionYPosition+0.2f-ApplicationDesignRules.worldHeight/2f+ApplicationDesignRules.downMargin+ApplicationDesignRules.miniCompanionWorldSize.y/2f,-9.5f);
				}
			}
			else
			{
				this.miniCompanion.transform.rotation=Quaternion.Euler(0,180,0);
				if(ApplicationDesignRules.isMobileScreen)
				{
					this.miniCompanion.transform.localPosition=new Vector3(-0.2f+ApplicationDesignRules.worldWidth/2f-ApplicationDesignRules.rightMargin-ApplicationDesignRules.miniCompanionWorldSize.x/2f,this.miniCompanionYPosition+0.2f-ApplicationDesignRules.worldHeight/2f+ApplicationDesignRules.miniCompanionWorldSize.y/2f,-9.5f);
				}
				else
				{
					this.miniCompanion.transform.localPosition=new Vector3(-0.2f+ApplicationDesignRules.worldWidth/2f-ApplicationDesignRules.rightMargin-ApplicationDesignRules.miniCompanionWorldSize.x/2f,this.miniCompanionYPosition+0.2f-ApplicationDesignRules.worldHeight/2f+ApplicationDesignRules.downMargin+ApplicationDesignRules.miniCompanionWorldSize.y/2f,-9.5f);
				}
			}
			this.isFlashingMiniCompanion = true;
			this.isMiniCompanionOnHoveredState = false;
		}
		else
		{
			this.miniCompanion.SetActive(false);
		}
	}
	private void drawMiniCompanion()
	{
		this.miniCompanionTimer = this.miniCompanionTimer + Time.deltaTime;
		if (this.miniCompanionTimer > 0.5f) 
		{
			if (this.isMiniCompanionOnHoveredState) 
			{
				this.isMiniCompanionOnHoveredState = false;
				if (!this.miniCompanion.GetComponent<HelpMiniCompanionController> ().getIsHovered ()) 
				{
					this.miniCompanion.GetComponent<HelpMiniCompanionController> ().setInitialState ();
				}	
			} 
			else 
			{
				this.isMiniCompanionOnHoveredState = true;
				if (!this.miniCompanion.GetComponent<HelpMiniCompanionController> ().getIsHovered ()) 
				{
					this.miniCompanion.GetComponent<HelpMiniCompanionController> ().setHoveredState ();
				}	
			}
			this.miniCompanionTimer = 0f;
		}
	}

	#endregion
}

