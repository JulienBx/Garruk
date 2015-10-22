using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class EndSceneController : MonoBehaviour 
{
	public static EndSceneController instance;

	public GameObject endGamePanelObject;
	public GameObject cardObject;
	public GameObject nextLevelPopUpObject;

	private GameObject endGamePanel;
	private GameObject nextLevelPopUp;
	private GameObject[] cards;

	private bool toUpdateCredits;
	private bool toStartExperienceUpdate;
	private bool areCreditsUpdated;

	private float updateSpeed;
	private float updateRatio;

	private int earnXp;
	private int earnCredits;

	private int endCredits;
	private int currentCredits;
	private int startCredits;

	private int xpDrawn;

	private User player;

	private int collectionPoints;
	private int collectionPointsRanking;
	private string newCardType;
	private IList<string> newSkills;

	private IList<int> idCardsToNextLevel;

	private bool isNextLevelPopUpDisplayed;
	

	void Awake()
	{
		instance = this;
		this.updateSpeed = 1.5f;
		this.updateRatio = 0;
		this.toUpdateCredits = false;
		this.toStartExperienceUpdate = false;
		this.areCreditsUpdated = false;
		this.player = new User ();
		this.newSkills = new List<string> ();
		this.idCardsToNextLevel = new List<int> ();
		this.newCardType = "";
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
				this.currentCredits=this.startCredits+(int)(this.updateRatio*this.earnCredits);
			}
			this.endGamePanel.transform.FindChild("Credits").GetComponent<TextMeshPro>().text="Vous avez gagné + "+ this.earnCredits+ " crédits ("+this.currentCredits+" crédits)";
		}
		if(this.areCreditsUpdated && this.toStartExperienceUpdate)
		{
			for(int i=0;i<this.cards.Length;i++)
			{
				this.cards[i].GetComponent<NewCardEndSceneController>().animateExperience();
			}
			this.toStartExperienceUpdate=false;
		}
	}
	public void displayEndScene(bool hasWon)
	{
		this.retrieveBonus (hasWon);
		this.player.Username = GameController.instance.getMyPlayerName ();
		this.endGamePanel = Instantiate(endGamePanelObject) as GameObject;
		this.endGamePanel.transform.FindChild ("Button").gameObject.SetActive (false); 
		this.endGamePanel.transform.position = new Vector3 (0f, 0f, -8f);
		this.cards=new GameObject[ApplicationModel.nbCardsByDeck];
		for(int i=0;i<ApplicationModel.nbCardsByDeck;i++)
		{
			cards[i]=Instantiate(cardObject) as GameObject;
			cards[i].transform.position=new Vector3(-4.5f+i*3f,0f,-8f);
			cards[i].AddComponent<NewCardEndSceneController>();
			cards[i].GetComponent<NewCardController>().c=GameController.instance.myDeck.Cards[i];
			cards[i].GetComponent<NewCardEndSceneController>().show();
			cards[i].GetComponent<NewCardEndSceneController>().changeLayer(11,"UIA");
			cards[i].GetComponent<NewCardEndSceneController>().setId(i);
			cards[i].transform.localScale=new Vector3(0.3108f,0.3108f,0.3108f);
		}

		if(hasWon)
		{
			this.endGamePanel.transform.FindChild("Title").GetComponent<TextMeshPro>().text="BRAVO !";
			ApplicationModel.hasWonLastGame=true;
		}
		else
		{
			this.endGamePanel.transform.FindChild("Title").GetComponent<TextMeshPro>().text="DOMMAGE !";
			ApplicationModel.hasWonLastGame=false;
		}

		StartCoroutine (this.drawCredits());
		StartCoroutine (this.addXp ());
	}
	
	private void retrieveBonus(bool hasWon)
	{
		switch(ApplicationModel.gameType)
		{
		case 0:
			if(hasWon)
			{
				this.earnXp=ApplicationModel.currentFriendlyGame.EarnXp_W;
				this.earnCredits=ApplicationModel.currentFriendlyGame.EarnCredits_W;
			}
			else
			{
				this.earnXp=ApplicationModel.currentFriendlyGame.EarnXp_L;
				this.earnCredits=ApplicationModel.currentFriendlyGame.EarnCredits_L;
			}
			break;
		case 1:
			if(hasWon)
			{
				this.earnXp=ApplicationModel.currentDivision.EarnXp_W;
				this.earnCredits=ApplicationModel.currentDivision.EarnCredits_W;
			}
			else
			{
				this.earnXp=ApplicationModel.currentDivision.EarnXp_L;
				this.earnCredits=ApplicationModel.currentDivision.EarnCredits_L;
			}
			break;
		case 2:
			if(hasWon)
			{
				this.earnXp=ApplicationModel.currentCup.EarnXp_W;
				this.earnCredits=ApplicationModel.currentCup.EarnCredits_W;
			}
			else
			{
				this.earnXp=ApplicationModel.currentCup.EarnXp_L;
				this.earnCredits=ApplicationModel.currentCup.EarnCredits_L;
			}
			break;
		}
	}
	public IEnumerator drawCredits()
	{
		yield return StartCoroutine(player.addMoney(earnCredits));
		this.endCredits = player.Money;
		this.startCredits = player.Money - this.earnCredits;
		this.toUpdateCredits = true;
	}
	public IEnumerator addXp()
	{
		yield return StartCoroutine(GameController.instance.myDeck.addXpToDeck (earnXp));
		this.collectionPoints = GameController.instance.myDeck.CollectionPoints;
		this.collectionPointsRanking = GameController.instance.myDeck.CollectionPointsRanking;
		if(GameController.instance.myDeck.NewSkills.Count>0)
		{
			for(int i=0;i<GameController.instance.myDeck.NewSkills.Count;i++)
			{
				this.newSkills.Add (GameController.instance.myDeck.NewSkills[i].Name);
			}
		}
		if(GameController.instance.myDeck.NewCardType!="")
		{
			this.newCardType=GameController.instance.myDeck.NewCardType;
		}
		this.toStartExperienceUpdate = true;
		this.xpDrawn = 0;
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
	public void quitEndSceneHandler()
	{
		//GameController.instance.disconnect ();
		ApplicationModel.launchEndGameSequence=true;
		if(ApplicationModel.gameType==1 || ApplicationModel.gameType==2)
		{
			Application.LoadLevel("NewLobby");
		}
		else
		{
			Application.LoadLevel("NewHomePage");
		}
	}
	public Vector3 getQuitButtonPosition()
	{
		return this.endGamePanel.transform.FindChild ("Button").gameObject.transform.position;
	}
	public void showEndButton()
	{
		this.endGamePanel.transform.FindChild("Button").gameObject.SetActive(true);
		if(GameView.instance.getIsTutorialLaunched())
		{
			TutorialObjectController.instance.actionIsDone();
		}
	}
	public void displayNextLevelPopUp(int indexCard)
	{
		this.nextLevelPopUp = Instantiate(this.nextLevelPopUpObject) as GameObject;
		this.nextLevelPopUp.transform.parent=this.gameObject.transform;
		this.nextLevelPopUp.transform.position = new Vector3 (gameObject.transform.position.x, 0, -2f);
		this.nextLevelPopUp.AddComponent<NextLevelPopUpControllerEndSceneGame> ();
		this.nextLevelPopUp.transform.GetComponent<NextLevelPopUpController> ().initialize (GameController.instance.myDeck.Cards[indexCard]);
		this.isNextLevelPopUpDisplayed=true;
	}
	public void hideNextLevelPopUp()
	{
		this.idCardsToNextLevel.RemoveAt(0);
		Destroy (this.nextLevelPopUp);
		this.isNextLevelPopUpDisplayed=false;
		if(this.idCardsToNextLevel.Count>0)
		{
			this.displayNextLevelPopUp(this.idCardsToNextLevel[0]);
		}
		else
		{
			this.showEndButton();
			for(int i=0;i<this.cards.Length;i++)
			{
				this.cards[i].GetComponent<NewCardEndSceneController>().endUpdatingCardToNextLevel();
			}
			if(this.collectionPoints>0)
			{
				this.endGamePanel.transform.FindChild("Credits").GetComponent<TextMeshPro>().text =this.endGamePanel.transform.FindChild("Credits").GetComponent<TextMeshPro>().text+"\n et "+this.collectionPoints+" points de collection (classement : "+this.collectionPointsRanking+")";
			}
			if(this.newCardType!="")
			{
				this.endGamePanel.transform.FindChild("NewCardType").GetComponent<TextMeshPro>().text="Bravo ! Vous venez d'acquérir la classe : "+this.newCardType;
			}
			if(this.newSkills.Count>0)
			{
				this.endGamePanel.transform.FindChild("Skills").GetComponent<TextMeshPro>().text="Vous débloquez : ";
				for(int i =0;i<this.newSkills.Count;i++)
				{
					this.endGamePanel.transform.FindChild("Skills").GetComponent<TextMeshPro>().text=this.endGamePanel.transform.FindChild("Skills").GetComponent<TextMeshPro>().text+"\n- "+this.newSkills[i];
				}
			}
		}
	}
	public IEnumerator upgradeCardAttribute(int attributeToUpgrade, int newPower, int newLevel)
	{
		GameView.instance.displayLoadingScreen ();
		yield return StartCoroutine (GameController.instance.myDeck.Cards[this.idCardsToNextLevel[0]].upgradeCardAttribute (attributeToUpgrade, newPower, newLevel));
		if(GameController.instance.myDeck.Cards[this.idCardsToNextLevel[0]].Error=="")
		{
			this.collectionPoints=this.collectionPoints+GameController.instance.myDeck.Cards[this.idCardsToNextLevel[0]].CollectionPoints;
			this.collectionPointsRanking=GameController.instance.myDeck.Cards[this.idCardsToNextLevel[0]].CollectionPointsRanking;
			if(GameController.instance.myDeck.Cards[this.idCardsToNextLevel[0]].IdCardTypeUnlocked!=-1)
			{
				this.newCardType=GameController.instance.myDeck.Cards[this.idCardsToNextLevel[0]].TitleCardTypeUnlocked;
			}
			if(GameController.instance.myDeck.Cards[this.idCardsToNextLevel[0]].NewSkills.Count>0)
			{
				for(int i=0;i<GameController.instance.myDeck.Cards[this.idCardsToNextLevel[0]].NewSkills.Count;i++)
				{
					this.newSkills.Add (GameController.instance.myDeck.Cards[this.idCardsToNextLevel[0]].NewSkills[i].Name);
				}
			}
			this.hideNextLevelPopUp();
		}
		else
		{
			this.cards[this.idCardsToNextLevel[0]].GetComponent<NewCardController>().displayErrorPopUp();
		}
		GameView.instance.hideLoadingScreen ();
	}
}

