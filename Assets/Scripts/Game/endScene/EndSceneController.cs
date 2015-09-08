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
	//public GameObject darkBackgroundObject;
	public GUIStyle[] endSceneVMStyles;

	private GameObject endGamePanel;
	private GameObject[] cards;
	private EndSceneView view;

	private bool toUpdateCredits;
	private float updateSpeed;
	private float updateRatio;

	private int earnXp;
	private int earnCredits;
	private int credits;

	private int xpDrawn;

	private bool hasWon;
	
	//private GameObject darkBackground;
	
	void Awake()
	{
		instance = this;
		this.updateSpeed = 0.7f;
		this.updateRatio = 0;
		this.toUpdateCredits = false;
		this.credits = 0;
	}
	void Update () 
	{
		if (this.toUpdateCredits)
		{
			this.updateRatio = this.updateRatio + this.updateSpeed * Time.deltaTime;
			if(this.updateRatio>=1)
			{
				this.endUpdateCredits();
			}
			else
			{
				view.endSceneVM.credits=view.endSceneVM.startCredits+(int)(this.updateRatio*view.endSceneVM.creditsToAdd);
			}
		}
	}
	public void endUpdateCredits()
	{
		view.endSceneVM.credits=view.endSceneVM.endCredits;
		this.toUpdateCredits=false;
		StartCoroutine(this.drawExperience());
	}
	public void displayEndScene(bool hasWon)
	{
		this.retrieveBonus (hasWon);
		this.endGamePanel = Instantiate(endGamePanelObject) as GameObject;
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
		}
		else
		{
			this.endGamePanel.transform.FindChild("Title").GetComponent<TextMeshPro>().text="DOMMAGE !";
		}

		//this.endGamePanel.transform.FindChild("Credits").GetComponent<TextMeshPro>().text="Vous gagnez "+this.credits 

		//this.darkBackground.name = "darkBackground";
		//this.initStyles ();
		//this.resize ();
		//this.initLabels (hasWon);
		//this.createCards ();

		//StartCoroutine (drawCredits ());
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
//	public IEnumerator drawCredits()
//	{
//		view.endSceneVM.creditsToAdd = earnCredits;
//		int playerIndex = 0;
////		if(!GameController.instance.isFirstPlayer)
////		{
////			playerIndex=1;
////		}
////		yield return StartCoroutine(GameController.instance.users[playerIndex].addMoney(earnCredits));
////		view.endSceneVM.endCredits = GameController.instance.users[playerIndex].Money;
//		view.endSceneVM.startCredits = view.endSceneVM.endCredits-view.endSceneVM.creditsToAdd;
//		this.toUpdateCredits = true;
//	}
	public IEnumerator drawExperience()
	{
//		yield return StartCoroutine(GameController.instance.myDeck.addXpToDeck (earnXp));
//		view.endSceneVM.collectionPoints = GameController.instance.myDeck.CollectionPoints;
//		view.endSceneVM.collectionPointsRanking = GameController.instance.myDeck.CollectionPointsRanking;
//		if(GameController.instance.myDeck.NewSkills.Count>0)
//		{
//			for(int i=0;i<GameController.instance.myDeck.NewSkills.Count;i++)
//			{
//				view.endSceneVM.newSkills.Add (GameController.instance.myDeck.NewSkills[i].Name);
//			}
//		}
//		if(GameController.instance.myDeck.NewCardType!="")
//		{
//			view.endSceneVM.newCardType=GameController.instance.myDeck.NewCardType;
//		}
//		this.xpDrawn = 0;
//		for(int i=0;i<this.cards.Length;i++)
//		{
//			this.cards [i].GetComponent<CardController>().animateExperience (GameController.instance.myDeck.Cards[i]);
//		}
		yield return 0;
	}
	public void incrementXpDrawn()
	{
		this.xpDrawn++;
		if(xpDrawn==this.cards.Length)
		{
			if(GameController.instance.getIsTutorialLaunched())
			{
				GameController.instance.callTutorial();
			}
			else
			{
				this.setGUI(true);
			}
		}
	}
	public void setGUI(bool value)
	{
		view.endSceneVM.guiEnabled=value;
	}
	public void initStyles()
	{
		view.endSceneVM.styles=new GUIStyle[this.endSceneVMStyles.Length];
		for(int i=0;i<this.endSceneVMStyles.Length;i++)
		{
			view.endSceneVM.styles[i]=this.endSceneVMStyles[i];
		}
		view.endSceneVM.initStyles();
	}
	public void clearCards()
	{
		for (int i = 0; i < ApplicationModel.nbCardsByDeck; i++) 
		{
			Destroy(this.cards[i]);
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
			if(hasWon)
			{
				ApplicationModel.hasWonLastGame=true;
			}
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

