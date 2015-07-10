using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using TMPro ;

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
	GameObject clickedRPC ;
	GameObject hoveredRPC ;
	GameObject tutorial;
	
	int heightScreen = -1;
	int widthScreen = -1;
	float realwidth = -1;
	
	float timer;
	int timerSeconds;
	
	float myLifePercentage = 100 ; 
	float hisLifePercentage = 100 ;
	
	AudioSource audioEndTurn;
	
	bool isBackgroundLoaded = false ;
	
	float animationTime = 0.2f ;
	
	bool isDisplayedClickedRPC = false ;
	int statusClickedPC = 0;
	Vector3 clickedPCPosition ;

	float timerClickedRPC ;
	
	bool isDisplayedHoveredRPC = false ;
	int statusHoveredPC = 0;
	Vector3 hoveredPCPosition ;
	
	float timerHoveredRPC ;
	
	int currentHoveredCard = -1;
	
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
			if (this.widthScreen!=Screen.width){
				this.resize();
			}
		}
		
		if (statusHoveredPC==1){
			this.timerHoveredRPC += Time.deltaTime;
			this.hoveredPCPosition.x = (0.5f*this.realwidth+5f)-(Mathf.Min(1,this.timerHoveredRPC/this.animationTime))*(0.5f*realwidth-3.25f);
			this.hoveredRPC.transform.position = this.hoveredPCPosition ;
			if (timerHoveredRPC>animationTime){
				statusHoveredPC = 0 ;
				this.isDisplayedHoveredRPC = true ;
			}
		}
		else if (statusHoveredPC<0){
			this.timerHoveredRPC += Time.deltaTime;
			this.hoveredPCPosition.x = (8.25f)+(Mathf.Min(1,this.timerHoveredRPC/this.animationTime))*(0.5f*realwidth-3.25f);
			this.hoveredRPC.transform.position = this.hoveredPCPosition ;
			if (timerHoveredRPC>animationTime){
				if (statusHoveredPC==-2){
					this.loadHoveredPC();
					statusHoveredPC = 1 ;
				}
				else{
					statusHoveredPC = 0 ;
				}
				this.isDisplayedHoveredRPC = false ;
				this.timerHoveredRPC=0;
			}
		}
		
		if (statusClickedPC==1){
			this.timerClickedRPC += Time.deltaTime;
			this.clickedPCPosition.x = (-0.5f*this.realwidth-5f)+(Mathf.Min(1,this.timerClickedRPC/this.animationTime))*(0.5f*realwidth-3.25f);
			this.clickedRPC.transform.position = this.clickedPCPosition ;
			if (timerClickedRPC>animationTime){
				statusClickedPC = 0 ;
				this.isDisplayedClickedRPC = true ;
			}
		}
		else if (statusClickedPC<0){
			this.timerClickedRPC += Time.deltaTime;
			this.clickedPCPosition.x = (-8.25f)-(Mathf.Min(1,this.timerClickedRPC/this.animationTime))*(0.5f*realwidth-3.25f);
			this.clickedRPC.transform.position = this.clickedPCPosition ;
			if (timerClickedRPC>animationTime){
				if (statusClickedPC==-2){
					this.loadClickedPC();
					statusClickedPC = 1 ;
					this.timerClickedRPC = 0 ;
				}
				else{
					statusClickedPC = 0 ;
				}
				this.isDisplayedClickedRPC = false ;
			}
		}
	}
	
	public void loadHoveredPC(){
		GameObject.Find("Title").GetComponent<TextMesh>().text = this.playingCards[this.currentHoveredCard].GetComponent<PlayingCardController>().getCard().Title;
		GameObject.Find("TextMove").GetComponent<TextMesh>().text = this.playingCards[this.currentHoveredCard].GetComponent<PlayingCardController>().getCard().GetMoveString();
		GameObject.Find("TextLife").GetComponent<TextMesh>().text = this.playingCards[this.currentHoveredCard].GetComponent<PlayingCardController>().getCard().GetLifeString();
		GameObject.Find("TextAttack").GetComponent<TextMesh>().text = this.playingCards[this.currentHoveredCard].GetComponent<PlayingCardController>().getCard().GetAttackString();
	
		List<Skill> skills = this.playingCards[this.currentHoveredCard].GetComponent<PlayingCardController>().getCard().getSkills();
		for (int i = 0 ; i < skills.Count ; i++){
			GameObject.Find("Skill"+i+"Title").GetComponent<TextMesh>().text = skills[i].Name;
			GameObject.Find(("Skill"+i+"Description")).GetComponent<TextMesh>().text = skills[i].Description;
		}
	}
	
	public void loadClickedPC(){
		int currentPlayingCard = GameController.instance.getCurrentPlayingCard();
		
		GameObject.Find("Title2").GetComponent<TextMesh>().text = this.playingCards[currentPlayingCard].GetComponent<PlayingCardController>().getCard().Title;
		GameObject.Find("TextMove2").GetComponent<TextMesh>().text = this.playingCards[currentPlayingCard].GetComponent<PlayingCardController>().getCard().GetMoveString();
		GameObject.Find("TextLife2").GetComponent<TextMesh>().text = this.playingCards[currentPlayingCard].GetComponent<PlayingCardController>().getCard().GetLifeString();
		GameObject.Find("TextAttack2").GetComponent<TextMesh>().text = this.playingCards[currentPlayingCard].GetComponent<PlayingCardController>().getCard().GetAttackString();
		
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
		
		position.z = -1 ;
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
				if (tiles[j,i].GetComponent<TileController>().canBeDestination()){
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
	
	public void displayClickedPC(int c){
		if (!this.isDisplayedClickedRPC){
			this.loadClickedPC();
			statusClickedPC = 1 ;
			this.timerClickedRPC = 0 ;
		}
		else{
			statusClickedPC = -2 ;
			this.timerClickedRPC = 0 ;
		}
	}
	
	public void displayHoveredPC(int c){
		
		if (!this.isDisplayedHoveredRPC){
			this.loadHoveredPC();
			statusHoveredPC = 1 ;
			this.timerClickedRPC = 0 ;
		}
		else{
			statusHoveredPC = -2 ;
			this.timerHoveredRPC = 0 ;
		}
	}
	
	public void hideHoveredPC(int c){
		
		if (this.isDisplayedHoveredRPC){	
			statusHoveredPC = -1 ;
			this.timerHoveredRPC = 0 ;
		}
	}
	
	public void removeDestinations()
	{
		for (int i = 0; i < this.boardWidth; i++)
		{
			for (int j = 0; j < this.boardHeight; j++)
			{
				this.tileHandlers [i, j].SetActive(false);
			}
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
		
		this.tiles [c.deckOrder + 1, hauteur].GetComponent<TileController>().setCharacterID(debut + c.deckOrder);
	}
	
	public void movePlayingCard(int x, int y, int c)
	{
		Tile t = this.playingCards [c].GetComponentInChildren<PlayingCardController>().getTile();
		this.tiles[t.x, t.y].GetComponentInChildren<TileController>().setCharacterID(-1);
		this.playingCards [c].GetComponentInChildren<PlayingCardController>().setTile(new Tile(x,y), this.tiles[x,y].GetComponentInChildren<TileController>().getPosition());
		this.tiles[x, y].GetComponentInChildren<TileController>().setCharacterID(c);
	}
	
	public void resize()
	{
		print ("Je resize");
		this.widthScreen = Screen.width ;
		this.heightScreen = Screen.height ;
		
		if (this.isBackgroundLoaded){
			this.resizeBackground();
		}
	}
	
	public void resizeBackground()
	{		
		Vector3 position;
		this.realwidth = 10f*this.widthScreen/this.heightScreen;
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
		llbl.transform.position = new Vector3(-this.realwidth/2f+0.25f, 4.5f, 0);
		
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
		rlbr.transform.position = new Vector3(this.realwidth/2f-0.25f, 4.5f, 0);
		
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
		tempGO.transform.position = new Vector3(-0.48f*this.realwidth,4f,1);
		tempGO.GetComponent<Renderer>().sortingLayerName = "UI" ;
		
		tempGO = GameObject.Find("HisPlayerName");
		tempGO.transform.position = new Vector3(0.48f*this.realwidth,4f,1);
		tempGO.GetComponent<Renderer>().sortingLayerName = "UI" ;
		
		this.clickedRPC = GameObject.Find("ClickedPlayingCard");
		this.clickedPCPosition = new Vector3(-0.50f*this.realwidth-10f,-1f,1);
		this.clickedRPC.transform.position = this.clickedPCPosition;
		
		this.hoveredRPC = GameObject.Find("HoveredPlayingCard");
		this.hoveredPCPosition = new Vector3(0.50f*this.realwidth+10f,-1f,1);
		this.hoveredRPC.transform.position = this.hoveredPCPosition;
		
		GameObject.Find("IconMove").transform.localPosition = new Vector3((0.25f*(this.realwidth/2f-3f))-5.2f,-0.4f,0f);
		GameObject.Find("IconLife").transform.localPosition = new Vector3((0.5f*(this.realwidth/2f-3f))-5.2f,-0.4f,0f);
		GameObject.Find("IconAttack").transform.localPosition = new Vector3((0.75f*(this.realwidth/2f-3f))-5.2f,-0.4f,0f);
		
		tempGO = GameObject.Find("Title");
		tempGO.transform.localPosition = new Vector3((0.55f*(this.realwidth/2f-3f))-5f,0.42f,0f);
		tempGO.GetComponent<Renderer>().sortingLayerName = "UI" ;
		tempGO = GameObject.Find("TextMove");
		tempGO.transform.localPosition = new Vector3((0.25f*(this.realwidth/2f-3f))-5f,-0.4f,0f);
		tempGO.GetComponent<Renderer>().sortingLayerName = "UI" ;
		tempGO = GameObject.Find("TextLife");
		tempGO.transform.localPosition = new Vector3((0.5f*(this.realwidth/2f-3f))-5f,-0.4f,0f);
		tempGO.GetComponent<Renderer>().sortingLayerName = "UI" ;
		tempGO = GameObject.Find("TextAttack");
		tempGO.transform.localPosition = new Vector3((0.75f*(this.realwidth/2f-3f))-5f,-0.4f,0f);
		tempGO.GetComponent<Renderer>().sortingLayerName = "UI" ;
		
		GameObject.Find("IconMove2").transform.localPosition = new Vector3((0.80f*(this.realwidth/2f-3f))-4.8f,-0.4f,0f);
		GameObject.Find("IconLife2").transform.localPosition = new Vector3((0.55f*(this.realwidth/2f-3f))-4.8f,-0.4f,0f);
		GameObject.Find("IconAttack2").transform.localPosition = new Vector3((0.30f*(this.realwidth/2f-3f))-4.8f,-0.4f,0f);
		
		tempGO = GameObject.Find("Title2");
		tempGO.transform.localPosition = new Vector3((0.5f*(this.realwidth/2f-3f))-5f,0.42f,0f);
		tempGO.GetComponent<Renderer>().sortingLayerName = "UI" ;
		tempGO = GameObject.Find("TextMove2");
		tempGO.transform.localPosition = new Vector3((0.80f*(this.realwidth/2f-3f))-5f,-0.4f,0f);
		tempGO.GetComponent<Renderer>().sortingLayerName = "UI" ;
		tempGO = GameObject.Find("TextLife2");
		tempGO.transform.localPosition = new Vector3((0.55f*(this.realwidth/2f-3f))-5f,-0.4f,0f);
		tempGO.GetComponent<Renderer>().sortingLayerName = "UI" ;
		tempGO = GameObject.Find("TextAttack2");
		tempGO.transform.localPosition = new Vector3((0.30f*(this.realwidth/2f-3f))-5f,-0.4f,0f);
		tempGO.GetComponent<Renderer>().sortingLayerName = "UI" ;
		
		tempGO = GameObject.Find("Skill0Title");
		tempGO.transform.localPosition = new Vector3((0.05f*(this.realwidth/2f-3f))-5f,2.7f,0f);
		tempGO.GetComponent<Renderer>().sortingLayerName = "UI" ;
		tempGO = GameObject.Find("Skill0Description");
		tempGO.transform.localPosition = new Vector3((0.07f*(this.realwidth/2f-3f))-5f,2.0f,0f);
		tempGO.GetComponent<Renderer>().sortingLayerName = "UI" ;
		tempGO = GameObject.Find("Skill1Title");
		tempGO.transform.localPosition = new Vector3((0.09f*(this.realwidth/2f-3f))-5f,1.4f,0f);
		tempGO.GetComponent<Renderer>().sortingLayerName = "UI" ;
		tempGO = GameObject.Find("Skill1Description");
		tempGO.transform.localPosition = new Vector3((0.11f*(this.realwidth/2f-3f))-5f,0.7f,0f);
		tempGO.GetComponent<Renderer>().sortingLayerName = "UI" ;
		tempGO = GameObject.Find("Skill2Title");
		tempGO.transform.localPosition = new Vector3((0.13f*(this.realwidth/2f-3f))-5f,0.1f,0f);
		tempGO.GetComponent<Renderer>().sortingLayerName = "UI" ;
		tempGO = GameObject.Find("Skill2Description");
		tempGO.transform.localPosition = new Vector3((0.15f*(this.realwidth/2f-3f))-5f,-0.5f,0f);
		tempGO.GetComponent<Renderer>().sortingLayerName = "UI" ;
		tempGO = GameObject.Find("Skill3Title");
		tempGO.transform.localPosition = new Vector3((0.17f*(this.realwidth/2f-3f))-5f,-1.2f,0f);
		tempGO.GetComponent<Renderer>().sortingLayerName = "UI" ;
		tempGO = GameObject.Find("Skill3Description");
		tempGO.transform.localPosition = new Vector3((0.19f*(this.realwidth/2f-3f))-5f,-1.8f,0f);
		tempGO.GetComponent<Renderer>().sortingLayerName = "UI" ;
		
		if (EndSceneController.instance != null)
		{
			EndSceneController.instance.resize();
		}
		//if (GameController.instance.isTutorialLaunched)
		//{
		//	this.tutorial.GetComponent<GameTutorialController>().resize();
		//}
	}
	
	public void hoverTile(int c, Tile t){
		this.currentHoveredCard = c ;
		int currentPlayingCard = GameController.instance.getCurrentPlayingCard();
		if (currentPlayingCard!=this.currentHoveredCard){
			this.displayHoveredPC(c);
		}
		GameObject tempGO = GameObject.Find("Hover");
		Vector3 pos = tiles[t.x, t.y].GetComponent<TileController>().getPosition();
		pos.z = -1 ;
		tempGO.transform.position = pos ;
	}
	
	public void hoverTile(Tile t){
		GameObject tempGO = GameObject.Find("Hover");
		Vector3 pos = tiles[t.x, t.y].GetComponent<TileController>().getPosition();
		pos.z = -1 ;
		tempGO.transform.position = pos ;
	}
	
	public void clickTile(Tile t){
		GameObject tempGO = GameObject.Find("Click");
		Vector3 pos = tiles[t.x, t.y].GetComponent<TileController>().getPosition();
		pos.z = -1 ;
		tempGO.transform.position = pos ;
	}
	
	public void setMyPlayerName(string s){
		GameObject tempGO = GameObject.Find("MyPlayerName");
		tempGO.GetComponent<TextMeshPro>().text = s ;
	}
	
	public void setHisPlayerName(string s){
		GameObject tempGO = GameObject.Find("HisPlayerName");
		tempGO.GetComponent<TextMeshPro>().text = s ;
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


