﻿using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class GameView : MonoBehaviour
{
	public static GameView instance;
	
	public GameObject tileModel ;
	public GameObject tileHandlerModel ;
	public GameObject verticalBorderModel;
	public GameObject horizontalBorderModel;
	public GameObject backgroundImageModel;
	public GameObject playingCardModel;
	public GameObject skillButtonModel;
	public string[] gameTexts;
	public GameObject TutorialObject;
	
	int boardWidth = 6;
	int boardHeight = 8;
	
	int nbCardsPerPlayer = 4 ;
	int nbFreeRowsAtBeginning = 2 ;
	
	GameObject[,] tiles ;
	GameObject[,] tileHandlers ;
	GameObject[] verticalBorders ;
	GameObject[] horizontalBorders ;
	GameObject[] playingCards ;
	GameObject[] skillButtons ;
	GameObject backgroundImage ;
	GameObject tutorial;
	
	int heightScreen = -1;
	int widthScreen = -1;
	
	float timer;
	int timerSeconds;
	
	float myLifePercentage = 100 ; 
	float hisLifePercentage = 100 ;
	
	AudioSource audioEndTurn;
	
	bool isBackgroundLoaded = false ;
	
	
	void Awake()
	{
		instance = this;
		
		this.tiles = new GameObject[this.boardWidth, this.boardHeight];
		this.tileHandlers = new GameObject[this.boardWidth, this.boardHeight];
		this.playingCards = new GameObject[2 * nbCardsPerPlayer];
		this.verticalBorders = new GameObject[this.boardWidth+1];
		this.horizontalBorders = new GameObject[this.boardHeight+1];
		this.timer = 0;
		
		this.audioEndTurn = GetComponent<AudioSource>();
	}
	
	void Update()
	{
		if (this.widthScreen!=-1){
			if (this.widthScreen!=-Screen.width){
				this.resize();
			}
		}
	}
	
	public void createBackground()
	{
		for (int i = 0; i < this.verticalBorders.Length; i++)
		{
			this.verticalBorders [i] = (GameObject)Instantiate(this.verticalBorderModel);
		}
		for (int i = 0; i < this.horizontalBorders.Length; i++)
		{
			this.horizontalBorders [i] = (GameObject)Instantiate(this.horizontalBorderModel);
		}
		
		GameObject go = GameObject.Find("toDisplay");
		foreach (Transform child in go.transform)
		{
			child.gameObject.SetActive(true);
		}
		
		this.isBackgroundLoaded = true ;
		this.resize();
	}
	
	public void createTile(int x, int y, int type, bool isFirstP)
	{
		this.tiles [x, y] = (GameObject)Instantiate(this.tileModel);
		this.tiles [x, y].name = "Tile " + (x) + "-" + (y);
		this.tiles [x, y].GetComponent<TileController>().initTileController(new Tile(x,y), type);
		Vector3 position ;
		
		if (isFirstP){
			position = new Vector3(-2.5f+x, -3.5f+y, 0);
		}
		else{
			position = new Vector3(2.5f-x, 3.5f-y, 0);
		}
		
		Vector3 scale = new Vector3(0.25f, 0.25f, 0.25f);
		this.tiles [x, y].GetComponent<TileController>().resize(position, scale);
		
		this.tileHandlers [x, y] = (GameObject)Instantiate(this.tileHandlerModel);
		this.tileHandlers [x, y].name = "TileHandler " + (x) + "-" + (y);
		this.tileHandlers [x, y].GetComponent<TileHandlerController>().initTileHandlerController(new Tile(x,y));
		
		tileHandlers [x, y].GetComponent<TileHandlerController>().resize(position, scale);
		tileHandlers [x, y].SetActive(false);
	}
	
	public void setInitialDestinations(bool isFirstP)
	{
		List<Tile> tileList = new List<Tile>();
		int debut = 0 ;
		if (!isFirstP){
			debut = this.boardHeight-this.nbFreeRowsAtBeginning;
		}
		for (int i = debut ; i < debut + this.nbFreeRowsAtBeginning; i++){
			for (int j = 0 ; j < this.boardWidth; j++){
				if (!tileHandlers[j,i].GetComponent<TileHandlerController>().isOccupied() && !tiles[j,i].GetComponent<TileController>().isRock()){
					tileList.Add(new Tile(j,i));
				}
			}
		}
		this.setDestinationTiles(tileList);
	}
	
	public void setDestinationTiles(List<Tile> tileList)
	{
		for (int i = 0 ; i < tileList.Count ; i++){
			this.tileHandlers[tileList[i].x, tileList[i].y].GetComponent<TileHandlerController>().changeType(1);
			this.tileHandlers[tileList[i].x, tileList[i].y].SetActive(true);
		}
	}
	
	public void createPlayingCard(Card c, bool isFirstP, bool isFirstPlayer)
	{
		int debut = 0 ;
		int hauteur = 0 ;
	
		if (!isFirstP){
			debut = this.nbCardsPerPlayer ;
			hauteur = this.boardHeight ;
		}
		this.playingCards [debut + c.deckOrder] = (GameObject)Instantiate(this.playingCardModel);
		if (isFirstP != isFirstPlayer)
		{
			this.playingCards [debut + c.deckOrder].GetComponentInChildren<PlayingCardController>().hide();
		} 
	
		this.playingCards [debut + c.deckOrder].GetComponentInChildren<PlayingCardController>().setCard(c, 3-c.deckOrder);
		this.playingCards [debut + c.deckOrder].GetComponentInChildren<PlayingCardController>().setIDCharacter(debut + c.deckOrder);
		this.playingCards [debut + c.deckOrder].GetComponentInChildren<PlayingCardController>().setTile(new Tile(c.deckOrder + 1, hauteur), tiles [c.deckOrder + 1, hauteur].GetComponent<TileController>().getPosition());
		this.playingCards [debut + c.deckOrder].GetComponentInChildren<PlayingCardController>().setIsMine(isFirstP==isFirstPlayer);
		
		this.tileHandlers [c.deckOrder + 1, hauteur].GetComponent<TileHandlerController>().setCharacterID(debut + c.deckOrder);
	}
	
	public void resize()
	{
		this.widthScreen = Screen.width ;
		this.heightScreen = Screen.height ;
		
		if (this.isBackgroundLoaded){
			this.resizeBackground();
		}
	}
	
	public void resizeBackground()
	{		
		Vector3 position;
		float realwidth = 10f*this.widthScreen/this.heightScreen;
		float tileScale = 8f / this.boardHeight;
		
		for (int i = 0; i < this.horizontalBorders.Length; i++)
		{
			position = new Vector3(0, -4f + tileScale * i, -1f);
			this.horizontalBorders [i].transform.localPosition = position;
			this.horizontalBorders [i].transform.localScale = new Vector3(1f,0.5f,1f);
		}
		
		for (int i = 0; i < this.verticalBorders.Length; i++)
		{
			position = new Vector3((-this.boardWidth/2f+i)*tileScale, 0f, -1f);
			this.verticalBorders [i].transform.localPosition = position;
			this.verticalBorders [i].transform.localScale = new Vector3(0.5f,1f,1f);
		}
		
		GameObject llbl = GameObject.Find("LLBLeft");
		llbl.transform.position = new Vector3(-realwidth/2f+0.25f, 4.5f, 0);
		
		GameObject llbr = GameObject.Find("LLBRight");
		llbr.transform.position = new Vector3(-1.2f, 4.5f, 0);
		
		GameObject llbc = GameObject.Find("LLBCenter");
		llbc.transform.position = new Vector3((llbl.transform.position.x+llbr.transform.position.x)/2f, 4.5f, 0);
		llbc.transform.localScale = new Vector3((-llbl.transform.position.x+llbr.transform.position.x-0.49f)/10f, 0.5f, 1f);
		
		GameObject leb = GameObject.Find("LLBRightEnd");
		leb.transform.position = new Vector3(-1.2f, 4.5f, 0);
		
		GameObject lcb = GameObject.Find("LLBLeftEnd");
		lcb.transform.position = new Vector3(llbr.transform.position.x-0.5f+this.myLifePercentage*(-llbr.transform.position.x+0.5f+(llbl.transform.position.x+0.1f))/100, 4.5f, 0);
		
		GameObject llbb = GameObject.Find("LLBBar");
		llbb.transform.position = new Vector3((leb.transform.position.x+lcb.transform.position.x)/2f, 4.5f, 0);
		llbb.transform.localScale = new Vector3((leb.transform.position.x-lcb.transform.position.x-0.49f)/10f, 0.5f, 0.5f);
		
		GameObject rlbl = GameObject.Find("RLBLeft");
		rlbl.transform.position = new Vector3(1.2f, 4.5f, 0);
		
		GameObject rlbr = GameObject.Find("RLBRight");
		rlbr.transform.position = new Vector3(realwidth/2f-0.25f, 4.5f, 0);
		
		GameObject rlbc = GameObject.Find("RLBCenter");
		rlbc.transform.position = new Vector3((rlbl.transform.position.x+rlbr.transform.position.x)/2f, 4.5f, 0);
		rlbc.transform.localScale = new Vector3((-rlbl.transform.position.x+rlbr.transform.position.x-0.49f)/10f, 0.5f, 0.5f);
		
		GameObject reb = GameObject.Find("RLBRightEnd");
		reb.transform.position = new Vector3(rlbl.transform.position.x+0.5f+this.hisLifePercentage*(-rlbl.transform.position.x-0.5f+(rlbr.transform.position.x-0.1f))/100, 4.5f, 0);
		
		GameObject rcb = GameObject.Find("RLBLeftEnd");
		rcb.transform.position = new Vector3(1.20f, 4.5f, 0);
		
		GameObject rlbb = GameObject.Find("RLBBar");
		rlbb.transform.position = new Vector3((reb.transform.position.x+rcb.transform.position.x)/2f, 4.5f, 0);
		rlbb.transform.localScale = new Vector3((reb.transform.position.x-rcb.transform.position.x-0.49f)/10f, 0.5f, 0.5f);	
		
		GameObject tempGO = GameObject.Find("MyPlayerName");
		tempGO.transform.position = new Vector3(-0.48f*realwidth,4f,1);
		tempGO.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
		tempGO.GetComponent<Renderer>().sortingLayerName = "Foreground" ;
		
		tempGO = GameObject.Find("HisPlayerName");
		tempGO.transform.position = new Vector3(0.48f*realwidth,4f,1);
		tempGO.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
		tempGO.GetComponent<Renderer>().sortingLayerName = "Foreground" ;
		
		if (EndSceneController.instance != null)
		{
			EndSceneController.instance.resize();
		}
		//if (GameController.instance.isTutorialLaunched)
		//{
		//	this.tutorial.GetComponent<GameTutorialController>().resize();
		//}
	}
	
	public void setMyPlayerName(string s){
		GameObject tempGO = GameObject.Find("MyPlayerName");
		tempGO.GetComponent<TextMesh>().text = s ;
	}
	
	public void setHisPlayerName(string s){
		GameObject tempGO = GameObject.Find("HisPlayerName");
		tempGO.GetComponent<TextMesh>().text = s ;
	}
	
	public Tile getRandomRock(int nbForbiddenRows){
		return new Tile(UnityEngine.Random.Range(0, this.boardWidth), UnityEngine.Random.Range(0, this.boardHeight - 2*nbForbiddenRows) + 1);
	}
	
	public int getBoardWidth(){
		return this.boardWidth;
	}
	
	public int getBoardHeight(){
		return this.boardHeight;
	}
}


