﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class EndSceneController : MonoBehaviour 
{

	public static EndSceneController instance;
	public GameObject cardObject;
	public GameObject darkBackgroundObject;
	public GUIStyle[] endSceneVMStyles;
	private GameObject[] cards;
	private EndSceneView view;

	private bool toUpdateCredits;
	private float updateSpeed;
	private float updateRatio;

	private int earnXp;
	private int earnCredits;
	private int xpDrawn;
	
	private GameObject darkBackground;
	
	void Awake()
	{
		instance = this;
		this.updateSpeed = 0.7f;
		this.updateRatio = 0;
		this.toUpdateCredits = false;
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
		this.view = Camera.main.gameObject.AddComponent <EndSceneView>();
		this.darkBackground = Instantiate(darkBackgroundObject) as GameObject;
		this.darkBackground.name = "darkBackground";
		this.initStyles ();
		this.resize ();
		this.initLabels (hasWon);
		this.createCards ();
		this.retrieveBonus (hasWon);
		StartCoroutine (drawCredits ());
	}
	public void initLabels(bool hasWon)
	{
		if(hasWon)
		{
			view.endSceneVM.title="BRAVO !";
		}
		else
		{
			view.endSceneVM.title="DOMMAGE !";
		}
	}
	public void createCards()
	{
		string name;
		Vector3 scale;
		Vector3 position;
		float tempF = 2*Camera.main.camera.orthographicSize*view.screenVM.widthScreen/view.screenVM.heightScreen;
		float width = 0.5f * tempF;
		float scaleCard = width/6f;
		this.cards=new GameObject[5];
		for (int i = 0; i < 5; i++)
		{
			name="Card" + i;
			scale = new Vector3(scaleCard,scaleCard,scaleCard);
			position = new Vector3(-width/2+(scaleCard/2)+i*(scaleCard+1f/4f*scaleCard), 0f, -9f); 
			this.cards [i] = Instantiate(this.cardObject) as GameObject;
			this.cards [i].AddComponent<CardGameController>();
			this.cards [i].GetComponent<CardController>().setGameObject(name,scale,position);
			this.cards [i].GetComponent<CardGameController>().setGameCard(GameController.instance.myDeck.Cards[i]);
		} 
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
		view.endSceneVM.creditsToAdd = earnCredits;
		int playerIndex = 0;
		if(!GameController.instance.isFirstPlayer)
		{
			playerIndex=1;
		}
		yield return StartCoroutine(GameController.instance.users[playerIndex].addMoney(earnCredits));
		view.endSceneVM.endCredits = GameController.instance.users[playerIndex].Money;
		view.endSceneVM.startCredits = view.endSceneVM.endCredits-view.endSceneVM.creditsToAdd;
		this.toUpdateCredits = true;
	}
	public IEnumerator drawExperience()
	{
		yield return StartCoroutine(GameController.instance.myDeck.addXpToDeck (earnXp));
		view.endSceneVM.collectionPoints = GameController.instance.myDeck.CollectionPoints;
		this.xpDrawn = 0;
		for(int i=0;i<this.cards.Length;i++)
		{
			this.cards [i].GetComponent<CardController>().animateExperience (GameController.instance.myDeck.Cards[i]);
		}
	}
	public void incrementXpDrawn()
	{
		this.xpDrawn++;
		if(xpDrawn==this.cards.Length)
		{
			view.endSceneVM.guiEnabled=true;
		}
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
		for (int i = 0; i < 5; i++) 
		{
			Destroy(this.cards[i]);
		}
	}
	public void resize()
	{
		if(this.view!=null)
		{
			view.screenVM.resize ();
			view.endSceneVM.resize (view.screenVM.heightScreen);
		}
		if(this.darkBackground!=null)
		{
			this.darkBackground.GetComponent<DarkBackgroundController> ().resize();
		}
		if(this.cards!=null)
		{
			for (int i=0;i<this.cards.Length;i++)
			{
				this.cards[i].GetComponent<CardController>().resize();
			}
		}
	}
	public void quitEndScene()
	{
		GameController.instance.disconnect ();
		Application.LoadLevel("EndGame");
	}
}

