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
	private GameObject dragHelp;
	private GameObject scrollUpHelp;
	private GameObject scrollDownHelp;

	private Rect backgroundRect;
	private float popUpHalfHeight;
	private float popUpHalfWidth;

	private bool isTutorialLaunched;
	private bool isTutorialDisplayed;
	private bool isHelpLaunched;
	private bool isScrolling;

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
				Vector3 arrowPosition = gameObject.transform.FindChild("Arrow").localPosition;
				arrowPosition.x=this.startTranslation+this.currentTranslation;
				gameObject.transform.FindChild("Arrow").localPosition=arrowPosition;
			}
			else
			{
				Vector3 arrowPosition = gameObject.transform.FindChild("Arrow").localPosition;
				arrowPosition.y=this.startTranslation+this.currentTranslation;
				gameObject.transform.FindChild("Arrow").localPosition=arrowPosition;
			}
		}
		if(isScrolling)
		{
			if(isTutorialLaunched)
			{
				this.scrollingExceptions();
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
		this.popUp = gameObject.transform.FindChild ("PopUp").gameObject;
		this.popUpTitle = this.popUp.transform.FindChild ("Title").gameObject;
		this.popUpDescription = this.popUp.transform.FindChild ("Description").gameObject;
		this.popUpNextButton= this.popUp.transform.FindChild ("NextButton").gameObject;
		this.dragHelp = gameObject.transform.FindChild ("DragHelp").gameObject;
		this.scrollDownHelp = gameObject.transform.FindChild("ScrollDownHelp").gameObject;
		this.scrollUpHelp=gameObject.transform.FindChild("ScrollUpHelp").gameObject;
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
		this.exitButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Quitter le tutoriel";
		MenuController.instance.setIsUserBusy (true);
		this.launchSequence (getStartSequenceId(tutorialStep));
		if(!isDisplayed)
		{
			MenuController.instance.setFlashingHelp(true);
		}
	}
	public void startHelp()
	{
		this.isHelpLaunched = true;
		this.exitButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Quitter l'aide";
		MenuController.instance.setIsUserBusy (true);
		this.launchHelpSequence(0);
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
	public void displayDragHelp(bool value, bool isHorizontal)
	{
		this.dragHelp.SetActive (value);
		if(isHorizontal)
		{
			this.dragHelp.GetComponent<DragHelpController>().setHorizontalTranslation();
		}
		else
		{
			this.dragHelp.GetComponent<DragHelpController>().setVerticalTranslation();
		}
	}
	public void displayScrollUpHelp(bool value)
	{
		this.scrollUpHelp.SetActive(value);
	}
	public void displayScrollDownHelp(bool value)
	{
		this.scrollDownHelp.SetActive(value);
	}
	public void displayBackground(bool value)
	{
		if(!this.isTutorialDisplayed && !this.isHelpLaunched)
		{
			value=false;
		}
		this.background.SetActive (value);
		this.background.GetComponent<TutorialBackgroundController> ().setSprite (0);
	}
	public void displaySquareBackground(bool value)
	{
		if(!this.isTutorialDisplayed && !this.isHelpLaunched)
		{
			value=false;
		}
		this.background.SetActive (value);
		this.background.GetComponent<TutorialBackgroundController> ().setSprite (1);
	}
	public void displayExitButton(bool value)
	{
		if(!this.isTutorialDisplayed&& !this.isHelpLaunched)
		{
			value=false;
		}
		this.exitButton.SetActive (value);
		this.exitButton.GetComponent<TutorialObjectQuitButtonController>().reset();
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
	public void setIsScrolling(bool value)
	{
		if(ApplicationDesignRules.isMobileScreen)
		{	
			this.isScrolling=value;
		}
		else
		{
			this.isScrolling=false;
		}
	}
	public bool getIsMoving()
	{
		return move;
	}
	public bool getIsScrolling()
	{
		return isScrolling;
	}
	public void resizeDragHelp(Vector3 position)
	{
		this.dragHelp.transform.localPosition = position;
	}
	public void resizeScrollUpHelp(Vector3 position)
	{
		this.scrollUpHelp.transform.localPosition=position;
	}
	public void resizeScrollDownHelp(Vector3 position)
	{	
		this.scrollDownHelp.transform.localPosition=position;
	}
	public void resizeArrow(Vector3 position)
	{
		this.arrow.transform.localPosition = position;
	}
	public void resizePopUp(Vector3 position)
	{
		if(Mathf.Abs(ApplicationDesignRules.worldWidth/2f)-Mathf.Abs(position.x)<this.popUpHalfWidth)
		{
			if(position.x>0)
			{
				position.x=ApplicationDesignRules.worldWidth/2f-this.popUpHalfWidth;
			}
			else
			{
				position.x=-ApplicationDesignRules.worldWidth/2f+this.popUpHalfWidth;
			}

		}
		this.popUp.transform.localPosition = position;
	}
	public void displayNextButton(bool value)
	{
		if(!this.isTutorialDisplayed&& !this.isHelpLaunched)
		{
			value=false;
		}
		this.popUpNextButton.SetActive (value);
	}
	public void displayPopUp(int value)
	{
		if(!this.isTutorialDisplayed&& !this.isHelpLaunched)
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
				this.popUpHalfWidth=2.75f;
			}
			else if(value==1)
			{
				this.gameObject.transform.FindChild ("PopUp").gameObject.SetActive(true);
				this.gameObject.transform.FindChild ("PopUpSmall").gameObject.SetActive(false);
				this.gameObject.transform.FindChild ("PopUpLarge").gameObject.SetActive(false);
				this.popUp=this.gameObject.transform.FindChild("PopUp").gameObject;
				this.popUpHalfHeight=3.375f;
				this.popUpHalfWidth=2.75f;
			}
			else if(value==2)
			{
				this.gameObject.transform.FindChild ("PopUp").gameObject.SetActive(false);
				this.gameObject.transform.FindChild ("PopUpSmall").gameObject.SetActive(false);
				this.gameObject.transform.FindChild ("PopUpLarge").gameObject.SetActive(true);
				this.popUp=this.gameObject.transform.FindChild("PopUpLarge").gameObject;
				this.popUpHalfHeight=4f;
				this.popUpHalfWidth=2.75f;

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
		if(this.isTutorialLaunched)
		{
			this.actionIsDone ();
		}
		else if(this.isHelpLaunched)
		{
			this.launchHelpSequence(this.sequenceID+1);
		}
	}

	#region TUTORIAL SEQUENCES

	public virtual void launchSequence(int sequenceID)
	{
		Vector3 gameObjectPosition = new Vector3 ();
		this.sequenceID = sequenceID;
		switch(this.sequenceID)
		{
		case 100: // Texte pour indiquer au joueur qu'il doit se rendre sur my Game pour créer une équipe
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.displayNextButton(false);
				this.setPopUpTitle("Une équipe, vite !");
				this.setPopUpDescription("Avant de pouvoir aller plus loin, il vous faut créer une équipe de combattants prete à vous défendre dans cet environnement hostile");
				this.displayBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition = MenuController.instance.getButtonPosition(1);
			if(ApplicationDesignRules.isMobileScreen)
			{
				this.setDownArrow();
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,1.5f,1.5f),0.6f,0.6f);
				this.drawDownArrow();
			}
			else
			{
				this.setUpArrow();
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2.5f,0.75f),0.8f,0.8f);
				this.drawUpArrow();
			}
			break;
		case 101: // Texte pour indiquer au joueur qu'il doit cliquer sur "Jouer" pour faire un premier match
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.displayNextButton(false);
				this.setPopUpTitle("Premier combat");
				this.setPopUpDescription("Il est temps de participer à votre premier combat, votre adversaire vous attend dans l'arène");
				this.displayBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition = MenuController.instance.getButtonPosition(5);
			if(ApplicationDesignRules.isMobileScreen)
			{
				this.setDownArrow();
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,1.5f,1.5f),0.6f,0.6f);
				this.drawDownArrow();
			}
			else
			{
				this.setUpArrow();
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2.5f,0.75f),0.8f,0.8f);
				this.drawUpArrow();
			}
			break;
		case 102: // Pas de texte, séquence pour cliquer sur le bouton match amical
			if(!isResizing)
			{
				this.displayPopUp(-1);
				this.setUpArrow();
				this.displayNextButton(false);
				this.displayBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition = PlayPopUpController.instance.getFriendlyGameButtonPosition();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,5f,1f),0.8f,0.8f);
			this.drawUpArrow();
			break;
		}
	}
	public virtual void scrollingExceptions()
	{
	}

	#endregion

	#region HELP SEQUENCES

	public virtual void launchHelpSequence(int sequenceID)
	{
		Vector3 gameObjectPosition = new Vector3 ();
		Vector3 gameObjectPosition2 = new Vector3 ();
		Vector2 gameObjectSize = new Vector2 ();
		this.sequenceID = sequenceID;
		switch(this.sequenceID)
		{
		case 100: // Description du haut de la carte, nom, compétence passive, xp
			if(!isResizing)
			{
				this.setUpArrow();
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Classe d'unité");
				this.setPopUpDescription("Les cristaliens se divisent en dix classes d'unités, chacune possédant ses propres compétences. L'unité prend de base le nom de sa classe et peut etre renommée par son colon.\n\nLa première compétence de l'unité est sa compétence passive, lui conférant des bonus permanents. Les compétences passives permettent de distinguer différents types d'unités au sein d'une meme classe.\n\nEnfin l'expérience de l'unité lui permet d'acquérir de nouvelles compétences et de faire progresser ses caractéristiques");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=getCardFocused().transform.FindChild("Name").position;
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y-0.6f,5.35f,2f),0f,0f);
			this.drawUpArrow();
			break;
		case 101: // Description des compétences de la carte
			if(!isResizing)
			{
				this.setDownArrow();
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Compétences");
				this.setPopUpDescription("Chaque Cristalien a développé des compétences uniques au contact du Cristal (plus de 150 découvertes à ce jour). Chaque cristalien peut posséder 3 compétences en plus de sa compétence passive.");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=getCardFocused().transform.FindChild("Skill2").position;
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,5.35f,3f),0f,0f);
			this.drawDownArrow();
			break;
		case 102: // Description des caractèristiques de la carte "vie, attaque"
			if(!isResizing)
			{
				this.setDownArrow();
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Caractéristiques");
				this.setPopUpDescription("Les caractéristiques déterminent la force et la santé de l'unité. Dépendantes de la classe, elles peuvent etre améliorées avec l'expérience. La santé se régènère à la fin de chaque combat, les blessures létales ayant été abolies il y a quelques années");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=getCardFocused().transform.FindChild("Life").position;
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,5.35f,1f),0f,0f);
			this.drawDownArrow();
			break;
		case 103: // Demande à l'utilisateur de sélectionner des cartes
			this.endHelp();
			break;
		}
	}

	#endregion

	public void resize()
	{
		this.isResizing = true;
		this.exitButton.transform.localScale=ApplicationDesignRules.button62Scale;
		if(ApplicationDesignRules.isMobileScreen)
		{
			this.exitButton.transform.localPosition=new Vector3(ApplicationDesignRules.worldWidth/2f-ApplicationDesignRules.blockHorizontalSpacing-ApplicationDesignRules.button62WorldSize.x/2f, ApplicationDesignRules.worldHeight/2f-ApplicationDesignRules.topBarWorldSize.y/2f+0.05f,-9.5f);
		}
		else
		{
			this.exitButton.transform.localPosition=new Vector3(ApplicationDesignRules.worldWidth/2f-ApplicationDesignRules.blockHorizontalSpacing-ApplicationDesignRules.button62WorldSize.x/2f, -ApplicationDesignRules.worldHeight/2f+ApplicationDesignRules.buttonVerticalSpacing+ApplicationDesignRules.button62WorldSize.y/2f+ApplicationDesignRules.downMargin,-9.5f);
		}
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
		Vector3 popUpPosition = this.arrow.transform.localPosition;
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
		Vector3 popUpPosition = this.arrow.transform.localPosition;
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
		Vector3 popUpPosition = this.arrow.transform.localPosition;
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
		Vector3 popUpPosition = this.arrow.transform.localPosition;
		popUpPosition.x = this.startTranslation + 4f;
		this.resizePopUp (popUpPosition);
		this.move=true;
		this.currentTranslation=0f;
		this.moveBack=false;
	}
	public void adjustUpArrowY(float correction)
	{
		Vector3 arrowLocalPosition = this.arrow.transform.localPosition;
		arrowLocalPosition.y=correction+0.3f-backgroundRect.height/2f;
		this.arrow.transform.localPosition=arrowLocalPosition;
	}
	public void adjustDownArrowY(float correction)
	{
		Vector3 arrowLocalPosition = this.arrow.transform.localPosition;
		arrowLocalPosition.y=correction+0.3f+backgroundRect.height/2f;
		this.arrow.transform.localPosition=arrowLocalPosition;
	}
	public void adjustLeftArrowY(float correction)
	{
		Vector3 arrowLocalPosition = this.arrow.transform.localPosition;
		arrowLocalPosition.y=correction;
		this.arrow.transform.localPosition=arrowLocalPosition;
	}
	public void adjustRightArrowY(float correction)
	{
		Vector3 arrowLocalPosition = this.arrow.transform.localPosition;
		arrowLocalPosition.y=correction;
		this.arrow.transform.localPosition=arrowLocalPosition;
	}
	public void adjustBackgroundY(float correction)
	{
		Vector3 backgroundLocalPosition = this.background.transform.localPosition;
		backgroundLocalPosition.y=correction;
		this.background.transform.localPosition=backgroundLocalPosition;
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
		switch(this.sequenceID)
		{
		case 101:
			if(MenuController.instance.getIsPlayPopUpDisplayed())
			{
				this.sequenceID=102;
				this.launchSequence(this.sequenceID);
			}
			break;
		case 102:
			if(!MenuController.instance.getIsPlayPopUpDisplayed())
			{
				this.sequenceID=101;
				this.launchSequence(this.sequenceID);
			}
			break;
		}
	}
	public int getSequenceID()
	{
		return this.sequenceID;
	}
	public bool canAccess()
	{
		int sequenceId=-1;
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
	public bool canAccess(int sequenceId)
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
	public void helpClicked()
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
			this.startHelp();
		}
	}
	public void displayCantAccessPopUp ()
	{
		MenuController.instance.displayErrorPopUp ("Vous êtes encore en apprentissage !  \n Attendez d'avoir disputé votre premier match \npour pouvoir ensuite réaliser cette action");
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
			if(ApplicationModel.hasDeck)
			{
				return 101;
			}
			else
			{
				return 100;
			}
			break;
		}
		return 0;
	}
	public void quitButtonHandler()
	{
		if(this.isTutorialLaunched)
		{
			this.hideTutorial ();
		}
		else if(this.isHelpLaunched)
		{
			this.endHelp();
		}
	}
	public IEnumerator endTutorial()
	{
		MenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine(this.player.setTutorialStep(-1));
		this.isTutorialLaunched = false;
		this.disableTutorial ();
		MenuController.instance.hideLoadingScreen ();
	}
	public void disableTutorial()
	{
		this.displayBackground (false);
		this.displayPopUp (-1);
		this.displayArrow (false);
		this.displayDragHelp (false,false);
		this.displayExitButton (false);
	}
	public virtual void endHelp()
	{
		this.isHelpLaunched = false;
		MenuController.instance.setIsUserBusy (false);
		this.disableTutorial ();
	}
	public virtual GameObject getCardFocused()
	{
		return new GameObject ();
	}
	public bool launchTutorialGame()
	{
		if(this.isTutorialLaunched && this.sequenceID==102)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}

