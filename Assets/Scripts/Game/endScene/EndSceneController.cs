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

	private GameObject endGamePanel;
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

	//private bool hasWon;
	private User player;

	private int collectionPoints;
	private int collectionPointsRanking;
	private string newCardType;
	public IList<string> newSkills;
	



	//private GameObject darkBackground;
	
	void Awake()
	{
		instance = this;
		this.updateSpeed = 0.7f;
		this.updateRatio = 0;
		this.toUpdateCredits = false;
		this.toStartExperienceUpdate = false;
		this.areCreditsUpdated = false;
		this.player = new User ();
		this.newSkills = new List<string> ();

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
			this.cards[this.xpDrawn].GetComponent<NewCardEndSceneController>().animateExperience();
			this.toStartExperienceUpdate=false;
		}
	}
	public void displayEndScene(bool hasWon)
	{
		this.retrieveBonus (hasWon);
		this.player.Username = GameController.instance.getMyPlayerName ();
		this.endGamePanel = Instantiate(endGamePanelObject) as GameObject;
		this.endGamePanel.transform.FindChild ("Button").gameObject.SetActive (true); // A remodifier
		this.endGamePanel.transform.position = new Vector3 (0f, 0f, -8f);
		this.cards=new GameObject[ApplicationModel.nbCardsByDeck];
		for(int i=0;i<ApplicationModel.nbCardsByDeck;i++)
		{
			cards[i]=Instantiate(cardObject) as GameObject;
			cards[i].transform.localScale=new Vector3(1.4f,1.4f,1.4f);
			cards[i].transform.position=new Vector3(-4.5f+i*3f,0f,-9f);
			cards[i].AddComponent<NewCardEndSceneController>();
			cards[i].GetComponent<NewCardController>().c=GameController.instance.myDeck.Cards[i];
			cards[i].GetComponent<NewCardEndSceneController>().show();
			cards[i].GetComponent<NewCardController>().changeLayer(11,"UIA");
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
		case 0: case 3: // ENLEVER 3 
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
	public void incrementXpDrawn()
	{
		this.xpDrawn++;
		if(xpDrawn==this.cards.Length)
		{
			this.endGamePanel.transform.FindChild("Button").gameObject.SetActive(true);
		}
		else
		{
			this.cards[this.xpDrawn].GetComponent<NewCardEndSceneController>().animateExperience();
		}
	}
	public void quitEndSceneHandler()
	{
		GameController.instance.disconnect ();
		if(GameController.instance.getIsTutorialLaunched())
		{
			//StartCoroutine(GameController.instance.endTutorial());
		}
		else
		{
			if(ApplicationModel.gameType==3) // A MODIFIER APRES
			{
				ApplicationModel.launchEndGameSequence=true;
				Application.LoadLevel("NewHomePage");
			}
			else
			{
				Application.LoadLevel("EndGame");
			}
		}
	}
}

