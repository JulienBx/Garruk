using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;


public class CupLobbyController : MonoBehaviour
{
	public static CupLobbyController instance;
	private CupLobbyModel model;
	private CupLobbyView view;
	
	public string[] roundsName;
	public GUIStyle[] screenVMStyle;
	public GUIStyle[] boardVMStyle;
	public GUIStyle[] resultsVMStyle;
	public GUIStyle[] cupLobbyVMStyle;
	public GUIStyle[] opponnentVMStyle;
	public GUIStyle[] competInfosVMStyle;
	private bool isTutorialLaunched;
	private GameObject tutorial;
	
	public GameObject MenuObject;
	public GameObject TutorialObject;
	
	void Start()
	{
		instance = this;
		this.model = new CupLobbyModel ();
		this.view = Camera.main.gameObject.AddComponent <CupLobbyView>();
		this.MenuObject = Instantiate(this.MenuObject) as GameObject;
		StartCoroutine (this.initialization());
	}
	private IEnumerator initialization()
	{
		yield return StartCoroutine(model.getCupLobbyData());
		this.initStyles ();
		this.initVM ();
		this.initializeRounds ();
		this.resize ();
		if(!model.player.CupLobbyTutorial)
		{
			this.tutorial = Instantiate(this.TutorialObject) as GameObject;
			MenuObject.GetComponent<MenuController>().setTutorialLaunched(true);
			this.tutorial.GetComponent<TutorialObjectController>().launchSequence(1200);
			this.isTutorialLaunched=true;
		}
	}
	private void initVM()
	{
		view.boardVM.name = model.currentCup.Name;
		view.boardVM.nbRounds = model.currentCup.NbRounds;

		for(int i=0;i<model.results.Count;i++)
		{
			view.resultsVM.resultsStyles.Add (new GUIStyle());
			view.resultsVM.focusButtonStyles.Add(new GUIStyle());
			view.resultsVM.resultsLabel.Add (model.results[i].Date.ToString("dd/MM/yyyy"));
			if(i==0)
			{
				view.resultsVM.focusButtonStyles[i]=view.resultsVM.selectedFocusButtonStyle;
			}
			else
			{
				view.resultsVM.focusButtonStyles[i]=view.resultsVM.focusButtonStyle;
			}
			if(model.results[i].HasWon)
			{
				view.resultsVM.resultsStyles[i]=view.resultsVM.wonLabelStyle;
				view.resultsVM.resultsLabel[i]=view.resultsVM.resultsLabel[i]+" Victoire";
			}
			else
			{
				view.resultsVM.resultsStyles[i]=view.resultsVM.defeatLabelStyle;
				view.resultsVM.resultsLabel[i]=view.resultsVM.resultsLabel[i]+" Défaite";
			}
		}
		if(model.results.Count>0)
		{
			view.opponentVM.username = model.results [0].Opponent.Username;
			view.opponentVM.totalNbWins = model.results [0].Opponent.TotalNbWins;
			view.opponentVM.totalNbLooses = model.results [0].Opponent.TotalNbLooses;
			view.opponentVM.ranking = model.results [0].Opponent.Ranking;
			view.opponentVM.rankingPoints = model.results [0].Opponent.RankingPoints;
			view.opponentVM.division = model.results [0].Opponent.Division;
			view.opponentVM.profilePictureStyle.normal.background = model.results [0].Opponent.texture;
			StartCoroutine (model.results [0].Opponent.setProfilePicture ());
		}
		view.competInfosVM.nbRounds = model.currentCup.NbRounds;
		view.competInfosVM.cupPrize = model.currentCup.CupPrize;
		view.competInfosVM.competitionPictureStyle.normal.background = model.currentCup.texture;
		StartCoroutine (model.currentCup.setPicture ());
	}
	public void displayOpponent(int chosenOpponent)
	{
		view.opponentVM.username = model.results [chosenOpponent].Opponent.Username;
		view.opponentVM.totalNbWins = model.results [chosenOpponent].Opponent.TotalNbWins;
		view.opponentVM.totalNbLooses = model.results [chosenOpponent].Opponent.TotalNbLooses;
		view.opponentVM.ranking = model.results [chosenOpponent].Opponent.Ranking;
		view.opponentVM.rankingPoints = model.results [chosenOpponent].Opponent.RankingPoints;
		view.opponentVM.division = model.results [chosenOpponent].Opponent.Division;
		view.opponentVM.profilePictureStyle.normal.background = model.results [chosenOpponent].Opponent.texture;
		StartCoroutine (model.results [chosenOpponent].Opponent.setProfilePicture ());
		view.resultsVM.focusButtonStyles[chosenOpponent]=view.resultsVM.selectedFocusButtonStyle;
		view.resultsVM.focusButtonStyles[view.resultsVM.chosenResult]=view.resultsVM.focusButtonStyle;
		view.resultsVM.chosenResult = chosenOpponent;
	}
	private void initializeRounds()
	{
		for (int i=0; i<model.currentCup.NbRounds;i++)
		{
			view.boardVM.roundsStyle.Add(new GUIStyle());
			view.boardVM.roundsName.Add (this.roundsName[i]);
			if(i>=model.currentCup.NbRounds-model.results.Count)
			{
				view.boardVM.roundsStyle[i]=view.boardVM.winRoundStyle;
			}
			else
			{
				view.boardVM.roundsStyle[i]=view.boardVM.notPlayedRoundStyle;
			}
		}
	}
	private void initStyles()
	{
		view.screenVM.styles=new GUIStyle[this.screenVMStyle.Length];
		for(int i=0;i<this.screenVMStyle.Length;i++)
		{
			view.screenVM.styles[i]=this.screenVMStyle[i];
		}
		view.screenVM.initStyles();
		view.cupLobbyVM.styles=new GUIStyle[this.cupLobbyVMStyle.Length];
		for(int i=0;i<this.cupLobbyVMStyle.Length;i++)
		{
			view.cupLobbyVM.styles[i]=this.cupLobbyVMStyle[i];
		}
		view.cupLobbyVM.initStyles();
		view.resultsVM.styles=new GUIStyle[this.resultsVMStyle.Length];
		for(int i=0;i<this.resultsVMStyle.Length;i++)
		{
			view.resultsVM.styles[i]=this.resultsVMStyle[i];
		}
		view.resultsVM.initStyles();
		view.opponentVM.styles=new GUIStyle[this.opponnentVMStyle.Length];
		for(int i=0;i<this.opponnentVMStyle.Length;i++)
		{
			view.opponentVM.styles[i]=this.opponnentVMStyle[i];
		}
		view.opponentVM.initStyles();
		view.boardVM.styles=new GUIStyle[this.boardVMStyle.Length];
		for(int i=0;i<this.boardVMStyle.Length;i++)
		{
			view.boardVM.styles[i]=this.boardVMStyle[i];
		}
		view.boardVM.initStyles();
		view.competInfosVM.styles=new GUIStyle[this.competInfosVMStyle.Length];
		for(int i=0;i<this.competInfosVMStyle.Length;i++)
		{
			view.competInfosVM.styles[i]=this.competInfosVMStyle[i];
		}
		view.competInfosVM.initStyles();
	}
	public void resize()
	{
		view.screenVM.resize ();
		view.cupLobbyVM.resize (view.screenVM.heightScreen);
		view.resultsVM.resize (view.screenVM.heightScreen);
		view.opponentVM.resize (view.screenVM.heightScreen);
		view.resultsVM.resize (view.screenVM.heightScreen);
		view.boardVM.resize (view.screenVM.heightScreen);
		view.competInfosVM.resize (view.screenVM.heightScreen);
		this.resizeRounds ();
		if(this.isTutorialLaunched)
		{
			this.tutorial.GetComponent<TutorialObjectController>().resize();
		}
	}
	private void resizeRounds()
	{
		for(int i=0;i<model.currentCup.NbRounds;i++)
		{
			view.boardVM.roundsStyle[i].fixedHeight=(int)view.screenVM.blockTopLeftHeight*50/(100*model.currentCup.NbRounds);
			view.boardVM.roundsStyle[i].fontSize=(int)view.boardVM.roundsStyle[i].fixedHeight*60/100;
		}
	}
	public void joinCupGame()
	{
		ApplicationModel.gameType = 2; // 1 pour Official
		Application.LoadLevel("Game");
	}
	public void quitCupLobby()
	{
		Application.LoadLevel("Lobby");
	}
	public void setButtonsGui(bool value)
	{
		view.cupLobbyVM.buttonsEnabled =value;
	}
	public IEnumerator endTutorial()
	{
		yield return StartCoroutine (model.player.setCupLobbyTutorial(true));
		MenuController.instance.setButtonsGui (true);
		Destroy (this.tutorial);
		this.isTutorialLaunched = false;
		MenuController.instance.isTutorialLaunched = false;
		this.setButtonsGui (true);
	}
}