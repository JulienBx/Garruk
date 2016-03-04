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

	private GameObject backOfficeController;
	private GameObject nextLevelPopUp;
	private GameObject[] cards;
	private GameObject title;
	private GameObject credits;
	private GameObject button;

	private GameObject mainCamera;
	private GameObject sceneCamera;

	private bool toUpdateCredits;
	private bool toStartExperienceUpdate;
	private bool areCreditsUpdated;

	private float updateSpeed;
	private float updateRatio;

	private int endCredits;
	private int currentCredits;
	private int startCredits;

	private int xpDrawn;

	private int collectionPointsEarned;
	private int newCollectionRanking;
	private int idCardTypeUnlocked;
	private int bonus;
	private int xpWon;
	private string titleCardTypeUnlocked;
	private List<Skill> skillsUnlocked;

	private IList<int> idCardsToNextLevel;

	private bool isNextLevelPopUpDisplayed;
	private bool hasFinishedCardUpgrade;

	private string URLGetMyGameData = ApplicationModel.host + "get_end_game_data.php";
	private string urlUpgradeCardAttribute = ApplicationModel.host + "upgrade_card_attribute.php";

	void Awake()
	{
		instance = this;
		this.updateSpeed = 1.5f;
		this.updateRatio = 0;
		this.toUpdateCredits = false;
		this.toStartExperienceUpdate = false;
		this.areCreditsUpdated = false;
		this.idCardsToNextLevel = new List<int> ();
		this.initializeScene ();
		this.initializeBackOffice();
		SoundController.instance.playMusic(new int[]{1,2});
		StartCoroutine (this.initialization ());
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
			this.cards[i] = GameObject.Find("Card"+i);
			this.cards[i].AddComponent<NewCardEndGameController>();
			this.cards[i].GetComponent<NewCardEndGameController>().c = ApplicationModel.player.MyDeck.cards[i];
			this.cards[i].GetComponent<NewCardEndGameController>().show();
			this.cards[i].GetComponent<NewCardEndGameController>().setId(i);
		}
	}
	public IEnumerator initialization()
	{
		this.resize();
		if(ApplicationModel.player.ChosenGameType>3)
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
			yield break;
		}
		else if(!ApplicationModel.player.HasWonLastGame && ApplicationModel.player.PercentageLooser==0)
		{
			this.credits.GetComponent<TextMeshPro>().text=WordingEndGame.getReference(2);
			this.showEndButton();
			BackOfficeController.instance.hideLoadingScreen();
			yield break;
		}
		else
		{
			yield return StartCoroutine(this.initializeEndGame());
			this.endCredits = ApplicationModel.player.Money;
			this.startCredits = ApplicationModel.player.Money - this.bonus;
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
			}
			else
			{
				this.showEndButton();	
			}
		}
	}
	public void quitEndGameHandler()
	{
		ApplicationModel.player.ToLaunchEndGameSequence=true;
		if(ApplicationModel.player.ChosenGameType==1 || ApplicationModel.player.ChosenGameType==2)
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
		this.nextLevelPopUp = Instantiate(this.nextLevelPopUpObject) as GameObject;
		this.nextLevelPopUp.transform.position = new Vector3 (0, 0, -2f);
		this.nextLevelPopUp.AddComponent<NextLevelPopUpControllerEndGame> ();
		this.nextLevelPopUp.transform.GetComponent<NextLevelPopUpController> ().initialize (ApplicationModel.player.MyDeck.getCard(indexCard));
		this.isNextLevelPopUpDisplayed=true;
	}
	public void hideNextLevelPopUp()
	{
		Destroy (this.nextLevelPopUp);
		this.isNextLevelPopUpDisplayed=false;
	}
	public void switchToNextLevelPopUp()
	{
		BackOfficeController.instance.hideLoadingScreen();
		this.hasFinishedCardUpgrade=false;
		this.idCardsToNextLevel.RemoveAt(0);
		if(this.idCardsToNextLevel.Count>0)
		{
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
			if(this.collectionPointsEarned>0)
			{
				BackOfficeController.instance.displayCollectionPointsPopUp(this.collectionPointsEarned,this.newCollectionRanking);
			}
			if(this.skillsUnlocked.Count>0)
			{
				BackOfficeController.instance.displayNewSkillsPopUps(this.skillsUnlocked);
			}
			if(this.idCardTypeUnlocked!=-1)
			{
				BackOfficeController.instance.displayNewCardTypePopUp(this.idCardTypeUnlocked);
			}
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
	}
	public void upgradeCardAttributeHandler(int attributeToUpgrade, int newPower, int newLevel)
	{
		BackOfficeController.instance.displayLoadingScreen();
		StartCoroutine(this.launchUpgradeCardAttribute(attributeToUpgrade,newPower,newLevel));
	}
	public IEnumerator launchUpgradeCardAttribute(int attributeToUpgrade, int newPower, int newLevel)
	{
		yield return StartCoroutine(this.upgradeCardAttribute(attributeToUpgrade, newPower, newLevel));
		this.hasFinishedCardUpgrade=true;
	}
	public IEnumerator upgradeCardAttribute(int attributeToUpgrade, int newPower, int newLevel)
	{
		WWWForm form = new WWWForm(); 								
		form.AddField("myform_hash", ApplicationModel.hash); 		
		form.AddField("myform_idcard", ApplicationModel.player.MyDeck.cards[this.idCardsToNextLevel[0]].Id.ToString());
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField ("myform_attribute", attributeToUpgrade);
		form.AddField ("myform_newpower", newPower);
		form.AddField ("myform_newlevel", newLevel);
		
		WWW w = new WWW(urlUpgradeCardAttribute, form); 								
		yield return w; 											
		
		if (w.error == null)
		{
			if (!w.text.Contains("#ERROR#"))
			{
				string [] cardData = w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
				string [] experienceData = cardData[0].Split(new string[] {"#EXPERIENCEDATA#"},System.StringSplitOptions.None);
				ApplicationModel.player.MyDeck.cards[this.idCardsToNextLevel[0]].parseCard(experienceData[0]);
				this.titleCardTypeUnlocked=experienceData[1];
				this.idCardTypeUnlocked=System.Convert.ToInt32(experienceData[2]);
				this.cards[this.idCardsToNextLevel[0]].GetComponent<NewCardController>().caracteristicUpgraded=System.Convert.ToInt32(experienceData[3]);
				this.cards[this.idCardsToNextLevel[0]].GetComponent<NewCardController>().caracteristicIncrease=System.Convert.ToInt32(experienceData[4]);
				this.collectionPointsEarned=this.collectionPointsEarned+ System.Convert.ToInt32(cardData [1]);
				this.newCollectionRanking=System.Convert.ToInt32(cardData[2]);
			}
		}
	}
	public IEnumerator initializeEndGame()
	{
		this.skillsUnlocked = new List<Skill> ();
		this.titleCardTypeUnlocked = "";
		string idCards = "";
		for (int i=0; i<4; i++)
		{
			idCards = idCards + ApplicationModel.player.MyDeck.cards [i].Id.ToString() + "SEPARATOR";
		}
		string hasWon ="";
		if(ApplicationModel.player.HasWonLastGame)
		{
			hasWon="1";
		}
		else
		{
			hasWon="0";
		}
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField("myform_haswon", hasWon);
		form.AddField("myform_gametype", ApplicationModel.player.ChosenGameType.ToString());
		form.AddField("myform_percentagelooser", ApplicationModel.player.PercentageLooser.ToString());
		form.AddField("myform_idcard", idCards); 
		
		WWW w = new WWW(URLGetMyGameData, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		
		if (w.error != null)
		{
			Debug.Log(w.error); 											// donne l'erreur eventuelle
		}
		else if(w.text.Contains("#ERROR#"))
		{
			string[] errors = w.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
			Debug.Log (errors[1]);
		}	
		else
		{
			string [] data = w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			for(int i=0;i<4;i++)
			{
				string [] experienceData = data[i].Split(new string[] {"#EXPERIENCEDATA#"},System.StringSplitOptions.None);
				ApplicationModel.player.MyDeck.cards[i].parseCard(experienceData[0]);
				ApplicationModel.player.MyDeck.cards[i].GetNewSkill=System.Convert.ToBoolean(System.Convert.ToInt32(experienceData[1]));
				//this.Cards[i].NewSkills=new List<Skill>();
				if(ApplicationModel.player.MyDeck.cards[i].GetNewSkill)
				{
					for(int j=0;j<ApplicationModel.player.MyDeck.cards[i].Skills.Count;j++)
					{
						if(ApplicationModel.player.MyDeck.cards[i].Skills[ApplicationModel.player.MyDeck.cards[i].Skills.Count-j-1].IsActivated==1)
						{
							if(System.Convert.ToBoolean(System.Convert.ToInt32(experienceData[2])))
							{
								this.skillsUnlocked.Add (ApplicationModel.player.MyDeck.cards[i].Skills[ApplicationModel.player.MyDeck.cards[i].Skills.Count-j-1]);
								ApplicationModel.player.MyDeck.cards[i].Skills[ApplicationModel.player.MyDeck.cards[i].Skills.Count-j-1].IsNew=true;
							}
							break;
						}
					}
				}
			}
			this.collectionPointsEarned=System.Convert.ToInt32(data[4]);
			this.newCollectionRanking=System.Convert.ToInt32(data[5]);
			this.xpWon=System.Convert.ToInt32(data[6]);
			ApplicationModel.player.Money=System.Convert.ToInt32(data[7]);
			this.bonus=System.Convert.ToInt32(data[8]);
		}
	}
	public void returnPressed()
	{
	}
	public void escapePressed()
	{
	}
}

