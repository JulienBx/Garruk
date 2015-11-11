using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class TutorialObjectController : MonoBehaviour 
{
	public static TutorialObjectController instance;
	private User player;
	public int sequenceID;
	private float startTranslation;
	private float currentTranslation;
	private float speed;
	public float arrowWidth;
	public float arrowHeight;
	public float arrowX;
	public float arrowY;
	public float popUpWidth;
	public float popUpHeight;
	public float popUpX;
	public float popUpY;
	public Vector2 GOPosition;
	public Vector2 GOSize;
	private bool move;
	private bool moveForward;
	private bool moveBack;
	private bool moveHorizontal;
	private bool inversedMove;
	public bool isResizing;

	private GameObject arrow;
	private GameObject popUpTitle;
	private GameObject popUpDescription;
	private GameObject background;
	private GameObject popUpNextButton;
	private GameObject popUp;
	private GameObject exitButton;

	private Rect backgroundRect;
	private float popUpHalfHeight;

	private bool isTutorialLaunched;
	private bool isTutorialDisplayed;
	private bool isHelpLaunched;

	void Update()
	{
		if(this.move)
		{
			if(!moveBack)
			{
				this.currentTranslation=this.currentTranslation+Time.deltaTime*this.speed;
				if(this.currentTranslation>0.5f)
				{
					this.currentTranslation=0.5f;
					this.moveForward=false;
					this.moveBack=true;
				}
			}
			else
			{
				this.currentTranslation=this.currentTranslation-Time.deltaTime*this.speed;
				if(this.currentTranslation<0f)
				{
					this.currentTranslation=0f;
					this.moveForward=true;
					this.moveBack=false;
				}
			}
			if(moveHorizontal)
			{
				Vector3 arrowPosition = gameObject.transform.FindChild("Arrow").position;
				arrowPosition.x=this.startTranslation+this.currentTranslation;
				gameObject.transform.FindChild("Arrow").position=arrowPosition;
			}
			else
			{
				Vector3 arrowPosition = gameObject.transform.FindChild("Arrow").position;
				arrowPosition.y=this.startTranslation+this.currentTranslation;
				gameObject.transform.FindChild("Arrow").position=arrowPosition;
			}
		}
	}
	void Awake () 
	{
		instance = this;
		this.player = new User ();
		this.isResizing = false;
		this.sequenceID = -1;
		this.speed = 2f;
		this.arrow = gameObject.transform.FindChild ("Arrow").gameObject;
		this.background = gameObject.transform.FindChild ("Background").gameObject;
		this.exitButton = gameObject.transform.FindChild ("ExitButton").gameObject;
		this.exitButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Quitter le tutoriel";
		this.popUp = gameObject.transform.FindChild ("PopUp").gameObject;
		this.popUpTitle = this.popUp.transform.FindChild ("Title").gameObject;
		this.popUpDescription = this.popUp.transform.FindChild ("Description").gameObject;
		this.popUpNextButton= this.popUp.transform.FindChild ("NextButton").gameObject;
	}
	void Start () 
	{	
		this.endInitialization ();
	}
	public virtual void endInitialization()
	{
	}
	public virtual void startTutorial(int tutorialStep, bool isDisplayed)
	{
		this.isTutorialLaunched = true;
		this.isTutorialDisplayed = isDisplayed;
		if(!isDisplayed)
		{
			MenuController.instance.setFlashingHelp(true);
		}
	}
	public bool getIsTutorialLaunched()
	{
		return this.isTutorialLaunched;
	}
	public bool getIsTutorialDisplayed()
	{
		return this.isTutorialDisplayed;
	}
	public void setTutorialStep(int id)
	{
		StartCoroutine (this.player.setTutorialStep (id));
	}
	public void displayBackground(bool value)
	{
		if(!this.isTutorialDisplayed)
		{
			value=false;
		}
		this.background.SetActive (value);
	}
	public void displayExitButton(bool value)
	{
		if(!this.isTutorialDisplayed)
		{
			value=false;
		}
		this.exitButton.SetActive (value);
	}
	public void resizeBackground(Rect rect, float clickableSectionXRatio, float clickableSectionYRatio)
	{
		this.background.GetComponent<TutorialBackgroundController> ().resize (rect,clickableSectionXRatio,clickableSectionYRatio);
		this.backgroundRect = rect;
	}
	public void setPopUpTitle(string title)
	{
		this.popUpTitle.GetComponent<TextMeshPro> ().text = title;
	}
	public void setPopUpDescription(string description)
	{
		this.popUpDescription.GetComponent<TextMeshPro> ().text = description;
	}
	public void displayArrow(bool value)
	{
		this.arrow.SetActive (value);
		this.move = false;
	}
	public void resizeArrow(Vector3 position)
	{
		this.arrow.transform.position = position;
	}
	public void resizePopUp(Vector3 position)
	{
		this.popUp.transform.position = position;
	}
	public void displayNextButton(bool value)
	{
		if(!this.isTutorialDisplayed)
		{
			value=false;
		}
		this.popUpNextButton.SetActive (value);
	}
	public void displayPopUp(int value)
	{
		if(!this.isTutorialDisplayed)
		{
			value=-1;
		}
		if(value==-1)
		{
			this.gameObject.transform.FindChild ("PopUp").gameObject.SetActive(false);
			this.gameObject.transform.FindChild ("PopUpSmall").gameObject.SetActive(false);
			this.gameObject.transform.FindChild ("PopUpLarge").gameObject.SetActive(false);
		}
		else
		{
			if(value==0)
			{
				this.gameObject.transform.FindChild ("PopUp").gameObject.SetActive(false);
				this.gameObject.transform.FindChild ("PopUpSmall").gameObject.SetActive(true);
				this.gameObject.transform.FindChild ("PopUpLarge").gameObject.SetActive(false);
				this.popUp=this.gameObject.transform.FindChild("PopUpSmall").gameObject;
				this.popUpHalfHeight=2.75f;
			}
			else if(value==1)
			{
				this.gameObject.transform.FindChild ("PopUp").gameObject.SetActive(true);
				this.gameObject.transform.FindChild ("PopUpSmall").gameObject.SetActive(false);
				this.gameObject.transform.FindChild ("PopUpLarge").gameObject.SetActive(false);
				this.popUp=this.gameObject.transform.FindChild("PopUp").gameObject;
				this.popUpHalfHeight=3.375f;
			}
			else if(value==2)
			{
				this.gameObject.transform.FindChild ("PopUp").gameObject.SetActive(false);
				this.gameObject.transform.FindChild ("PopUpSmall").gameObject.SetActive(false);
				this.gameObject.transform.FindChild ("PopUpLarge").gameObject.SetActive(true);
				this.popUp=this.gameObject.transform.FindChild("PopUpLarge").gameObject;
				this.popUpHalfHeight=4f;

			}
			this.popUpTitle = this.popUp.transform.FindChild ("Title").gameObject;
			this.popUpDescription = this.popUp.transform.FindChild ("Description").gameObject;
			this.popUpNextButton= this.popUp.transform.FindChild ("NextButton").gameObject;
			this.popUpNextButton.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
			this.popUpNextButton.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
		}

	}
	public void nextStepHandler()
	{
		this.actionIsDone ();
	}
	public virtual void launchSequence(int sequenceID)
	{
		Vector3 gameObjectPosition = new Vector3 ();
		this.sequenceID = sequenceID;
		switch(this.sequenceID)
		{
		case 100:
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(false);
				this.setPopUpTitle("Allons créer une équipe");
				this.setPopUpDescription("A compléter");
				this.displayBackground(true);
				this.displayExitButton(true);
				
			}
			gameObjectPosition = MenuController.instance.getButtonPosition(1);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2.5f,0.75f),0.8f,0.8f);
			this.drawUpArrow();
			break;
		}
	}
	public void resize()
	{
		this.isResizing = true;
		Vector2 exitButtonSize = new Vector2(425f,105f);
		float exitButtonScale = 0.49f;
		Vector2 exitButtonWorldSize = (exitButtonSize / ApplicationDesignRules.pixelPerUnit) * exitButtonScale;
		this.exitButton.transform.position=new Vector3(ApplicationDesignRules.worldWidth/2f-0.3f-exitButtonWorldSize.x/2f, -ApplicationDesignRules.worldHeight/2f+0.3f+exitButtonWorldSize.y/2f,-9.5f);
		if(this.isTutorialLaunched)
		{
			this.launchSequence(sequenceID);
		}
		this.isResizing = false;
	}
	public void setUpArrow()
	{
		this.displayArrow (true);
		this.moveHorizontal=false;
		this.arrow.transform.localRotation= Quaternion.Euler(0.0f, 0.0f, 90.0f);
	}
	public void setDownArrow()
	{
		this.displayArrow (true);
		this.moveHorizontal=false;
		this.arrow.transform.localRotation= Quaternion.Euler(0.0f, 0.0f, -90.0f);
	}
	public void setRightArrow()
	{
		this.displayArrow (true);
		this.moveHorizontal=true;
		this.arrow.transform.localRotation= Quaternion.Euler(0.0f, 0.0f, 0.0f);
	}
	public void setLeftArrow()
	{
		this.displayArrow (true);
		this.moveHorizontal=true;
		this.arrow.transform.localRotation= Quaternion.Euler(0.0f, 0.0f, 180.0f);
	}
	public void drawUpArrow()
	{
		this.resizeArrow(new Vector3(backgroundRect.x,backgroundRect.y+0.3f-backgroundRect.height/2f,-9.5f));
		this.startTranslation=backgroundRect.y-0.8f-backgroundRect.height/2f;
		Vector3 popUpPosition = this.arrow.transform.position;
		popUpPosition.y = this.startTranslation +0.5f- this.popUpHalfHeight;
		this.resizePopUp (popUpPosition);
		this.move=true;
		this.currentTranslation=0f;
		this.moveBack=false;
	}
	public void drawDownArrow()
	{
		this.resizeArrow(new Vector3(backgroundRect.x,backgroundRect.y+0.3f+backgroundRect.height/2f,-9.5f));
		this.startTranslation=backgroundRect.y+0.3f+backgroundRect.height/2f;
		Vector3 popUpPosition = this.arrow.transform.position;
		popUpPosition.y = this.startTranslation + this.popUpHalfHeight;
		this.resizePopUp (popUpPosition);
		this.move=true;
		this.currentTranslation=0f;
		this.moveBack=false;
	}
	public void drawRightArrow()
	{
		this.resizeArrow(new Vector3(backgroundRect.x-0.3f-backgroundRect.width/2f,backgroundRect.y,-9.5f));
		this.startTranslation=backgroundRect.x-0.8f-backgroundRect.width/2f;
		Vector3 popUpPosition = this.arrow.transform.position;
		popUpPosition.x = this.startTranslation+0.5f - 4f;
		this.resizePopUp (popUpPosition);
		this.move=true;
		this.currentTranslation=0f;
		this.moveBack=false;
	}
	public void drawLeftArrow()
	{
		this.resizeArrow(new Vector3(backgroundRect.x+0.3f+backgroundRect.width/2f,backgroundRect.y,-9.5f));
		this.startTranslation=backgroundRect.x+0.3f+backgroundRect.width/2f;
		Vector3 popUpPosition = this.arrow.transform.position;
		popUpPosition.x = this.startTranslation + 4f;
		this.resizePopUp (popUpPosition);
		this.move=true;
		this.currentTranslation=0f;
		this.moveBack=false;
	}
	public void tutorialTrackPoint()
	{
		if(this.isTutorialLaunched)
		{
			this.actionIsDone();
		}
	}
	public virtual void actionIsDone()
	{
	}
	public int getSequenceID()
	{
		return this.sequenceID;
	}
	public bool canAccess(int sequenceId=-1)
	{
		if(this.isTutorialLaunched)
		{
			if(this.sequenceID==sequenceId)
			{
				return true;
			}
			else
			{
				this.displayCantAccessPopUp();
				return false;
			}
		}
		else
		{
			return true;
		}
	}
	public virtual void helpClicked()
	{
		if(this.isTutorialLaunched)
		{
			if(!this.isTutorialDisplayed)
			{
				MenuController.instance.setFlashingHelp(false);
				this.isTutorialDisplayed=true;
				this.launchSequence(this.sequenceID);
				StartCoroutine (player.setDisplayTutorial (true));
			}
		}
		else
		{
		}
	}
	public void displayCantAccessPopUp ()
	{
		MenuController.instance.displayErrorPopUp ("Vous êtes encore en apprentissage !  \n Attendez d'avoir disputé votre premier match pour pouvoir ensuite réaliser cette action");
	}
	public virtual void hideTutorial()
	{
		MenuController.instance.setFlashingHelp(true);
		this.isTutorialDisplayed = false;
		this.exitButton.SetActive (false);
		this.launchSequence (this.sequenceID);
		StartCoroutine (player.setDisplayTutorial (false));
	}
	public virtual int getStartSequenceId(int tutorialStep)
	{
		switch(tutorialStep)
		{
		case 2:
			return 100; 
			break;
		}
		return 0;
	}
	public void quitButtonHandler()
	{
		this.hideTutorial ();
	}
}

