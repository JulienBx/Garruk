using UnityEngine;
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
	
	private GameObject darkBackground;
	
	void Start()
	{
		instance = this;
		this.updateSpeed = 0.5f;
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
				view.endSceneVM.credits=view.endSceneVM.endCredits;
				this.toUpdateCredits=false;
			}
			else
			{
				view.endSceneVM.credits=view.endSceneVM.startCredits+(int)(this.updateRatio*view.endSceneVM.creditsToAdd);
			}
		}
	}
	public void displayEndScene(bool hasWon)
	{
		this.view = Camera.main.gameObject.AddComponent <EndSceneView>();
		this.darkBackground = Instantiate(darkBackgroundObject) as GameObject;
		this.darkBackground.name = "darkBackground";
		this.resize ();
		this.initStyles ();
		this.createCards ();
		StartCoroutine(this.updateModels (hasWon));
		this.animateExperience ();
		this.animateCredits ();
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
	private IEnumerator updateModels(bool hasWon)
	{
		int earnXp=0;
		int earnCredits=0;
		switch(ApplicationModel.gameType)
		{
		case 0:
			if(hasWon)
			{
				earnXp=ApplicationModel.currentFriendlyGame.EarnXp_W;
				earnCredits=ApplicationModel.currentFriendlyGame.EarnCredits_W;
			}
			else
			{
				earnXp=ApplicationModel.currentFriendlyGame.EarnXp_L;
				earnCredits=ApplicationModel.currentFriendlyGame.EarnCredits_L;
			}
			break;
		case 1:
			if(hasWon)
			{
				earnXp=ApplicationModel.currentDivision.EarnXp_W;
				earnCredits=ApplicationModel.currentDivision.EarnCredits_W;
			}
			else
			{
				earnXp=ApplicationModel.currentDivision.EarnXp_L;
				earnCredits=ApplicationModel.currentDivision.EarnCredits_L;
			}
			break;
		case 2:
			if(hasWon)
			{
				earnXp=ApplicationModel.currentCup.EarnXp_W;
				earnCredits=ApplicationModel.currentCup.EarnCredits_W;
			}
			else
			{
				earnXp=ApplicationModel.currentCup.EarnXp_L;
				earnCredits=ApplicationModel.currentCup.EarnCredits_L;
			}
			break;
		}
		view.endSceneVM.creditsToAdd = earnCredits;
		StartCoroutine (updateDeck (earnXp));
		StartCoroutine (updateUser (earnCredits));
		yield break;
	}
	public IEnumerator updateDeck(int earnXp)
	{
		yield return StartCoroutine(GameController.instance.myDeck.updateXpCards (earnXp));
	}
	public IEnumerator updateUser(int earnCredits)
	{
		int playerIndex = 0;
		if(!GameController.instance.isFirstPlayer)
		{
			playerIndex=1;
		}
		yield return StartCoroutine(GameController.instance.users[playerIndex].addMoney(earnCredits));
	}
	public void animateExperience()
	{
		for (int i=0;i<5;i++)
		{
			this.cards [i].GetComponent<CardController>().animateExperience (GameController.instance.myDeck.Cards[i]);
		}
	}
	public void animateCredits()
	{
		int playerIndex = 0;
		if(!GameController.instance.isFirstPlayer)
		{
			playerIndex=1;
		}
		view.endSceneVM.endCredits = GameController.instance.users [playerIndex].Money;
		view.endSceneVM.startCredits = view.endSceneVM.endCredits - view.endSceneVM.creditsToAdd;
		this.toUpdateCredits = true;
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
	public void resize()
	{
		view.screenVM.resize ();
		view.endSceneVM.resize (view.screenVM.heightScreen);
		this.darkBackground.GetComponent<DarkBackgroundController> ().resize();
	}
	public void quitEndScene()
	{
		GameController.instance.disconnect ();
		Application.LoadLevel("EndGame");
	}
}

