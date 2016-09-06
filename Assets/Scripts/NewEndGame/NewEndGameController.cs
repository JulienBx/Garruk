using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class NewEndGameController : MonoBehaviour 
{
	public static NewEndGameController instance;

	public GameObject nextLevelPopUpObject;
	public GameObject cardObject;

	private GameObject backOfficeController;
	private GameObject serverController;
	private GameObject help;
	private GameObject nextLevelPopUp;
	private GameObject[] cards;
	private GameObject title;
	private GameObject credits;
	private GameObject button;

	private GameObject mainCamera;
	private GameObject sceneCamera;
	private GameObject helpCamera;

	private bool toUpdateCredits;
	private bool areCreditsUpdated;

	private float updateSpeed;
	private float updateRatio;

	private int endCredits;
	private int currentCredits;
	private int startCredits;

	private int xpDrawn;

	//private int collectionPointsEarned;
	//private int newCollectionRanking;
	private int bonus;
	private int xpWon;
	//private List<Skill> skillsUnlocked;

	private IList<int> idCardsToNextLevel;

	private bool hasFinishedCardUpgrade;

	private string URLGetMyGameData = ApplicationModel.host + "get_end_game_data.php";
	private string urlUpgradeCardAttribute = ApplicationModel.host + "upgrade_card_attribute.php";

	void Awake()
	{
		instance = this;
		SoundController.instance.playMusic(new int[]{3});
        
        ApplicationModel.player.HasLostConnectionDuringGame=false;
		this.updateSpeed = 1.5f;
		this.updateRatio = 0;
		this.toUpdateCredits = false;
		this.areCreditsUpdated = false;
		this.idCardsToNextLevel = new List<int> ();
		this.initializeBackOffice();
		this.initializeScene ();
		this.initializeHelp();
		this.initialization ();
	}
	void Update () 
	{
		if (this.toUpdateCredits)
		{
			this.updateRatio = this.updateRatio + this.updateSpeed * Time.deltaTime;
			if(this.updateRatio>=1)
			{
				this.currentCredits=this.endCredits;
				this.toUpdateCredits=false;
				this.areCreditsUpdated=true;
			}
			else
			{
				this.currentCredits=this.startCredits+(int)(this.updateRatio*this.bonus);
			}
			string creditsText = "";
			if(ApplicationModel.player.HasWonLastGame)
			{
				creditsText=WordingEndGame.getReference(4);
			}
			else
			{
				creditsText=WordingEndGame.getReference(10);
			}
			this.credits.GetComponent<TextMeshPro>().text=creditsText+ this.bonus.ToString()+ WordingEndGame.getReference(5)+this.currentCredits.ToString()+WordingEndGame.getReference(6);
			this.credits.GetComponent<TextMeshPro>().text+=WordingEndGame.getReference(7)+this.xpWon.ToString()+WordingEndGame.getReference(8);
		}
		if(this.areCreditsUpdated)
		{
			this.areCreditsUpdated=false;
			for(int i=0;i<this.cards.Length;i++)
			{
				this.cards[i].GetComponent<NewCardEndGameController>().animateExperience();
			}
		}
		if(this.hasFinishedCardUpgrade)
		{
			this.switchToNextLevelPopUp();
		}
	}
	private void initializeBackOffice()
	{
		this.backOfficeController = GameObject.Find ("BackOfficeController");
		this.backOfficeController.AddComponent<BackOfficeEndGameController>();
		this.backOfficeController.GetComponent<BackOfficeEndGameController>().initialize();
	}
	private void initializeHelp()
	{
		this.help = GameObject.Find ("HelpController");
		this.help.AddComponent<EndGameHelpController>();
		this.help.GetComponent<EndGameHelpController>().initialize();
		BackOfficeController.instance.setIsHelpLoaded(true);
	}
	public void initializeScene()
	{
		this.mainCamera = gameObject;
		this.sceneCamera = GameObject.Find ("sceneCamera");
		this.title = GameObject.Find ("MainTitle");
		this.title.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.title.GetComponent<TextMeshPro>().text=WordingEndGame.getReference(1);
		this.credits = GameObject.Find ("Credits");
		this.credits.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.button=GameObject.Find("Button");
		this.button.AddComponent<NewEndGameQuitButtonController>();
		this.button.transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingEndGame.getReference(0);
		this.button.SetActive(false);
		this.cards=new GameObject[4];
		for(int i=0;i<4;i++)
		{
			this.cards[i] = Instantiate (this.cardObject) as GameObject;
			this.cards[i].AddComponent<NewCardEndGameController>();
			this.cards[i].GetComponent<NewCardEndGameController>().c = ApplicationModel.player.MyDeck.cards[i];
			this.cards[i].GetComponent<NewCardEndGameController>().show();
			this.cards[i].GetComponent<NewCardEndGameController>().setId(i);
		}
		this.helpCamera = GameObject.Find ("HelpCamera");
	}
	public void initialization()
	{
		this.resize();
		if(ApplicationModel.player.ChosenGameType>20)
		{
			if(ApplicationModel.player.HasWonLastGame)
			{
				this.credits.GetComponent<TextMeshPro>().text=WordingEndGame.getReference(3);
			}
			else
			{
				this.credits.GetComponent<TextMeshPro>().text=WordingEndGame.getReference(9);
			}
			this.showEndButton();
			BackOfficeController.instance.hideLoadingScreen();
		}
		else if(!ApplicationModel.player.HasWonLastGame && ApplicationModel.player.PercentageLooser==0)
		{
			this.credits.GetComponent<TextMeshPro>().text=WordingEndGame.getReference(2);
			this.showEndButton();
			BackOfficeController.instance.hideLoadingScreen();
		}
		else
		{
			this.retrieveBonus ();
			this.updateCards ();
			this.endCredits = ApplicationModel.player.Money+this.bonus;
			this.startCredits = ApplicationModel.player.Money;
			StartCoroutine(ApplicationModel.player.payMoney (-this.bonus));
			if (!ApplicationModel.player.IsOnline) {
				ApplicationModel.player.moneyToSync += this.bonus;
			}
			this.toUpdateCredits = true;
		}
		BackOfficeController.instance.hideLoadingScreen();
	}
	public void incrementXpDrawn(bool hasChangedLevel, int id)
	{
		this.xpDrawn++;
		if(hasChangedLevel)
		{
			this.idCardsToNextLevel.Add (id);
		}
		if(xpDrawn==this.cards.Length)
		{
			if(this.idCardsToNextLevel.Count>0)
			{
				this.displayNextLevelPopUp(this.idCardsToNextLevel[0]);
				if(!ApplicationModel.player.NextLevelTutorial)
				{
					HelpController.instance.startHelp();
				}
			}
			else
			{
				this.showEndButton();	
			}
		}
	}
	public void quitEndGameHandler()
	{
		SoundController.instance.playSound(9);
		ApplicationModel.player.ToLaunchEndGameSequence=true;
		SoundController.instance.playMusic(new int[]{0,1,2});

		if(ApplicationModel.player.ChosenGameType>10 && ApplicationModel.player.ChosenGameType<21)
		{
			Application.LoadLevel("NewLobby");
		}
		else
		{
			Application.LoadLevel("NewHomePage");
		}
	}
	public void showEndButton()
	{
		this.button.SetActive(true);
	}
	public void displayNextLevelPopUp(int indexCard)
	{
		SoundController.instance.playSound(3);
		this.nextLevelPopUp = Instantiate(this.nextLevelPopUpObject) as GameObject;
		this.nextLevelPopUp.transform.position = new Vector3 (0, 0, -2f);
		this.nextLevelPopUp.AddComponent<NextLevelPopUpControllerEndGame> ();
		this.nextLevelPopUp.transform.GetComponent<NextLevelPopUpController> ().initialize (ApplicationModel.player.MyDeck.getCard(indexCard));
	}
	public void hideNextLevelPopUp()
	{
		Destroy (this.nextLevelPopUp);
	}
	public void switchToNextLevelPopUp()
	{
		BackOfficeController.instance.hideLoadingScreen();
		this.hasFinishedCardUpgrade=false;
		this.idCardsToNextLevel.RemoveAt(0);
		if(this.idCardsToNextLevel.Count>0)
		{
			SoundController.instance.playSound(3);
			this.nextLevelPopUp.transform.GetComponent<NextLevelPopUpController> ().initialize (ApplicationModel.player.MyDeck.getCard(this.idCardsToNextLevel[0]));
		}
		else
		{
			this.hideNextLevelPopUp();
			this.showEndButton();
			for(int i=0;i<this.cards.Length;i++)
			{
				this.cards[i].GetComponent<NewCardEndGameController>().show();
				this.cards[i].GetComponent<NewCardEndGameController>().endUpdatingCardToNextLevel();
			}
            Cards cards=new Cards();
            for(int i=0;i<ApplicationModel.player.MyDeck.cards.Count;i++)
            {
                cards.add();
                cards.cards[i]=ApplicationModel.player.MyDeck.cards[i];
				int index = ApplicationModel.player.MyCards.getCardIndex (ApplicationModel.player.MyDeck.cards [i].Id);
				if (index != -1) 
				{
					ApplicationModel.player.MyCards.cards[index] = ApplicationModel.player.MyDeck.cards [i];
				}
            }
            ApplicationModel.player.updateMyCollection(cards);
			ApplicationModel.Save();
		}
	}
	public void resize()
	{
		this.mainCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.mainCamera.transform.position = ApplicationDesignRules.mainCameraPosition;
		this.sceneCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.sceneCamera.transform.position = new Vector3(0f,0f,-10f);
		this.title.GetComponent<TextContainer>().width=ApplicationDesignRules.worldWidth-2f*ApplicationDesignRules.blockHorizontalSpacing;
		this.credits.GetComponent<TextContainer>().width=ApplicationDesignRules.worldWidth-2f*ApplicationDesignRules.blockHorizontalSpacing;
		float gapBetweenCards = (ApplicationDesignRules.worldWidth-2f*ApplicationDesignRules.blockHorizontalSpacing-4f*ApplicationDesignRules.cardWorldSize.x)/3f;

		if(gapBetweenCards>0.5f)
		{
			gapBetweenCards=0.5f;
		}

		for(int i=0;i<ApplicationModel.nbCardsByDeck;i++)
		{
			cards[i].transform.position=new Vector3((i-1.5f)*(gapBetweenCards+ApplicationDesignRules.cardWorldSize.x),-1f,0f);
			cards[i].transform.localScale=ApplicationDesignRules.cardScale;
		}
		this.helpCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.helpCamera.transform.position = ApplicationDesignRules.helpCameraPositiion;
		this.helpCamera.GetComponent<Camera> ().rect = new Rect (0f, 0f, 1f, 1f);
		HelpController.instance.resize ();
	}
	public void upgradeCardAttributeHandler(int attributeToUpgrade, int newPower, int newLevel)
	{
		BackOfficeController.instance.displayLoadingScreen();
		StartCoroutine(this.launchUpgradeCardAttribute(attributeToUpgrade,newPower,newLevel));
	}
	public IEnumerator launchUpgradeCardAttribute(int attributeToUpgrade, int newPower, int newLevel)
	{
		ApplicationModel.player.MyDeck.cards[this.idCardsToNextLevel[0]].updateCardAttribute (attributeToUpgrade, newPower, newLevel);
		ApplicationModel.player.MyDeck.cards[this.idCardsToNextLevel[0]].setString ();
		yield return StartCoroutine(ApplicationModel.player.MyDeck.cards[this.idCardsToNextLevel[0]].syncCard ());
		if (ApplicationModel.player.MyDeck.cards[this.idCardsToNextLevel[0]].Error != "") 
		{
			Debug.Log (ApplicationModel.player.MyDeck.cards[this.idCardsToNextLevel[0]].Error);
			ApplicationModel.player.MyDeck.cards[this.idCardsToNextLevel[0]].Error = "";
		}
		this.hasFinishedCardUpgrade=true;
	}
	public void updateCards()
	{
		for (int i=0; i<4; i++)
		{
			ApplicationModel.player.MyDeck.cards [i].updateCardXp(false,this.xpWon);
		}
	}
	public void returnPressed()
	{
	}
	public void escapePressed()
	{
	}
	public void retrieveBonus()
	{
		if (ApplicationModel.player.ChosenGameType > 10) 
		{
			if (ApplicationModel.player.HasWonLastGame) 
			{
				this.bonus = ApplicationModel.divisions.getDivision (ApplicationModel.player.Division).earnCredits_W;
				this.xpWon = ApplicationModel.divisions.getDivision (ApplicationModel.player.Division).earnXp_W;
			} 
			else 
			{
				this.bonus = Mathf.RoundToInt (ApplicationModel.divisions.getDivision (ApplicationModel.player.Division).earnCredits_L * (ApplicationModel.player.PercentageLooser * 0.01f));
				this.xpWon = Mathf.RoundToInt (ApplicationModel.divisions.getDivision (ApplicationModel.player.Division).earnXp_L * (ApplicationModel.player.PercentageLooser * 0.01f));
			}
		} 
		else if (ApplicationModel.player.ChosenGameType > 0) 
		{
			if (ApplicationModel.player.HasWonLastGame) 
			{
				this.bonus = ApplicationModel.trainingGameEarnCredits_W;
				this.xpWon = ApplicationModel.trainingGameEarnXp_W;
			} 
			else 
			{
				this.bonus = Mathf.RoundToInt (ApplicationModel.trainingGameEarnCredits_L * (ApplicationModel.player.PercentageLooser * 0.01f));
				this.xpWon = Mathf.RoundToInt (ApplicationModel.trainingGameEarnXp_L * (ApplicationModel.player.PercentageLooser * 0.01f));
			}
		} 
		else 
		{
			if (ApplicationModel.player.HasWonLastGame) 
			{
				this.bonus = ApplicationModel.friendlyGameEarnCredits_W;
				this.xpWon = ApplicationModel.friendlyGameEarnXp_W;
			}
			else 
			{
				this.bonus = Mathf.RoundToInt (ApplicationModel.friendlyGameEarnCredits_L * (ApplicationModel.player.PercentageLooser * 0.01f));
				this.xpWon = Mathf.RoundToInt (ApplicationModel.friendlyGameEarnXp_L * (ApplicationModel.player.PercentageLooser * 0.01f));
			}
		}
	}
}

