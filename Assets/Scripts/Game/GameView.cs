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
	public Sprite[] volets ;
	
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
	GameObject skillRPC ;
	GameObject tutorial;
	GameObject popUp;
	GameObject popUpText;
	
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
	
	bool isDisplayedSkill = false ;
	int statusSkill = 0;
	Vector3 skillPosition ;
	
	float timerSkill ;
	
	int currentHoveredCard = -1;
	Skill currentHoveredSkill = new Skill(-1) ;
	
	float timerPopUp ;
	float popUpTime = 3 ;
	bool toDisplayPopUp = false ;
	
	void Awake()
	{
		instance = this;
		
		this.tiles = new GameObject[this.boardWidth, this.boardHeight];
		this.tileHandlers = new GameObject[this.boardWidth, this.boardHeight];
		this.playingCards = new GameObject[2 * nbCardsPerPlayer];
		this.verticalBorders = new GameObject[this.boardWidth+1];
		this.horizontalBorders = new GameObject[this.boardHeight+1];
		this.skillButtons = new GameObject[6];
		this.skillButtons[0] = GameObject.Find("AttackButton");
		this.skillButtons[1] = GameObject.Find("SkillButton0");
		this.skillButtons[2] = GameObject.Find("SkillButton1");
		this.skillButtons[3] = GameObject.Find("SkillButton2");
		this.skillButtons[4] = GameObject.Find("SkillButton3");
		this.skillButtons[5] = GameObject.Find("PassButton");
		this.clickedRPC = GameObject.Find("ClickedPlayingCard");
		this.hoveredRPC = GameObject.Find("HoveredPlayingCard");
		this.skillRPC = GameObject.Find("SkillDescription");
		this.popUp = GameObject.Find("PopUp");
		this.popUpText = GameObject.Find("PopUpText");
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
			this.hoveredPCPosition.x = (0.5f*this.realwidth+5.25f)-(Mathf.Min(1,this.timerHoveredRPC/this.animationTime))*(0.5f*realwidth-3f);
			this.hoveredRPC.transform.position = this.hoveredPCPosition ;
			if (timerHoveredRPC>animationTime){
				statusHoveredPC = 0 ;
				this.isDisplayedHoveredRPC = true ;
			}
		}
		else if (statusHoveredPC<0){
			this.timerHoveredRPC += Time.deltaTime;
			this.hoveredPCPosition.x = (8.25f)+(Mathf.Min(1,this.timerHoveredRPC/this.animationTime))*(0.5f*realwidth-3f);
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
		
		if (statusSkill==1){
			this.timerSkill += Time.deltaTime;
			this.skillPosition.x = (5f+(realwidth/2f)-8.25f)-(Mathf.Min(1,this.timerSkill/this.animationTime))*(5f+(realwidth/2f)-8.25f);
			this.skillRPC.transform.localPosition = this.skillPosition ;
			if (timerSkill>animationTime){
				statusSkill = 0 ;
				this.isDisplayedSkill = true ;
			}
		}
		else if (statusSkill<0){
			this.timerSkill += Time.deltaTime;
			this.skillPosition.x = (Mathf.Min(1,this.timerSkill/this.animationTime))*(5f+(realwidth/2f)-8.25f);
			this.skillRPC.transform.localPosition = this.skillPosition ;
			if (timerSkill>animationTime){
				if (statusSkill==-2){
					this.loadSkill();
					statusSkill = 1 ;
					this.timerSkill = 0 ;
				}
				else{
					statusSkill = 0 ;
				}
				this.isDisplayedSkill = false ;
			}
		}
		
		if (this.toDisplayPopUp){
			this.timerPopUp += Time.deltaTime;
			if (this.timerPopUp>this.popUpTime){
				this.popUp.transform.position = new Vector3(0f, -10f, 0f);
				this.toDisplayPopUp = false ;
			}
		}
	}
	
	public void loadHoveredPC(){
		if(this.playingCards[this.currentHoveredCard].GetComponent<PlayingCardController>().getIsMine()){
			GameObject.Find("MainDescription").GetComponent<SpriteRenderer>().sprite=this.volets[1];
			GameObject.Find("SkillsDescription").GetComponent<SpriteRenderer>().sprite=this.volets[3];
		}
		else{
			GameObject.Find("MainDescription").GetComponent<SpriteRenderer>().sprite=this.volets[0];
			GameObject.Find("SkillsDescription").GetComponent<SpriteRenderer>().sprite=this.volets[2];
		}
		
		GameObject.Find("Title").GetComponent<TextMeshPro>().text = this.playingCards[this.currentHoveredCard].GetComponent<PlayingCardController>().getCard().Title;
		GameObject.Find("TextMove").GetComponent<TextMeshPro>().text = this.playingCards[this.currentHoveredCard].GetComponent<PlayingCardController>().getCard().GetMoveString();
		GameObject.Find("TextLife").GetComponent<TextMeshPro>().text = this.playingCards[this.currentHoveredCard].GetComponent<PlayingCardController>().getCard().GetLifeString();
		GameObject.Find("TextAttack").GetComponent<TextMeshPro>().text = this.playingCards[this.currentHoveredCard].GetComponent<PlayingCardController>().getCard().GetAttackString();
	
		List<Skill> skills = this.playingCards[this.currentHoveredCard].GetComponent<PlayingCardController>().getCard().getSkills();
		for (int i = 0 ; i < skills.Count ; i++){
			GameObject.Find("Skill"+i+"Title").GetComponent<TextMeshPro>().text = skills[i].Name;
			GameObject.Find(("Skill"+i+"Description")).GetComponent<TextMeshPro>().text = skills[i].Description;
		}
		for (int i = skills.Count ; i < 4 ; i++){
			GameObject.Find("Skill"+i+"Title").GetComponent<TextMeshPro>().text = "";
			GameObject.Find(("Skill"+i+"Description")).GetComponent<TextMeshPro>().text = "";
		}
	}
	
	public void loadClickedPC(){
		int currentPlayingCard = GameController.instance.getCurrentPlayingCard();
		if(this.playingCards[currentPlayingCard].GetComponent<PlayingCardController>().getIsMine()){
			GameObject.Find("MainDescription2").GetComponent<SpriteRenderer>().sprite=this.volets[5];
			GameObject.Find("SkillDescription").GetComponent<SpriteRenderer>().sprite=this.volets[7];
		}
		else{
			GameObject.Find("MainDescription2").GetComponent<SpriteRenderer>().sprite=this.volets[4];
			GameObject.Find("SkillDescription").GetComponent<SpriteRenderer>().sprite=this.volets[6];
		}
		Card c = this.playingCards[currentPlayingCard].GetComponent<PlayingCardController>().getCard();
		
		GameObject.Find("Title2").GetComponent<TextMeshPro>().text = c.Title;
		GameObject.Find("TextMove2").GetComponent<TextMeshPro>().text = c.GetMoveString();
		GameObject.Find("TextLife2").GetComponent<TextMeshPro>().text = c.GetLifeString();
		GameObject.Find("TextAttack2").GetComponent<TextMeshPro>().text = c.GetAttackString();
		
		this.skillButtons[0].GetComponent<SkillButtonController>().setSkill(c.GetAttackSkill());
		this.skillButtons[5].GetComponent<SkillButtonController>().setSkill(new Skill("Fin du tour","Termine son tour et passe la main au personnage suivant"));
		
		for (int i = 0 ; i < c.Skills.Count ; i++){
			this.skillButtons[1+i].SetActive(true);
			this.skillButtons[1+i].GetComponent<SkillButtonController>().setSkill(c.Skills[i]);
		}
		for (int i = c.Skills.Count ; i < 4 ; i++){
			this.skillButtons[1+i].SetActive(false);
		}
		
	}
	
	public void resetSkill(){
		this.skillPosition.x = (5f+(realwidth/2f)-8.25f);
		this.skillRPC.transform.localPosition = this.skillPosition;
		this.isDisplayedSkill = false ;
	}
	
	public void loadSkill(){
		int currentPlayingCard = GameController.instance.getCurrentPlayingCard();
		print (this.currentHoveredSkill.Name);
		GameObject.Find("TitleSD").GetComponent<TextMeshPro>().text = this.currentHoveredSkill.Name;
		GameObject.Find("DescriptionSD").GetComponent<TextMeshPro>().text = this.currentHoveredSkill.Description;
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
	
	public void displayPopUp(string s){
		this.popUpText.GetComponent<TextMeshPro>().text = s ;
		this.popUp.transform.position = new Vector3(0f, 0f, 0f);
		
		this.timerPopUp = 0 ;
		this.toDisplayPopUp = true ;
		
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
	
	public void displaySkill(){
		
		if (!this.isDisplayedSkill){
			this.loadSkill();
			statusSkill = 1 ;
			this.timerSkill= 0 ;
		}
		else{
			statusSkill = -2 ;
			this.timerSkill = 0 ;
		}
	}
	
	public void hideHoveredPC(int c){
		
		if (this.isDisplayedHoveredRPC){	
			statusHoveredPC = -1 ;
			this.timerHoveredRPC = 0 ;
		}
		else if(this.statusHoveredPC==1){
			statusHoveredPC = -1 ;
			this.timerHoveredRPC = this.animationTime-timerHoveredRPC ;
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
			hauteur = this.boardHeight-1 ;
		}
		this.playingCards [debut + c.deckOrder] = (GameObject)Instantiate(this.playingCardModel);
		if (isFirstP != isFirstPlayer)
		{
			this.playingCards [debut + c.deckOrder].GetComponentInChildren<PlayingCardController>().hide();
		} 
		this.playingCards [debut + c.deckOrder].GetComponentInChildren<PlayingCardController>().setCard(c, 3-c.deckOrder);
		this.playingCards [debut + c.deckOrder].GetComponentInChildren<PlayingCardController>().setIDCharacter(debut + c.deckOrder);
		if (isFirstP){
			this.playingCards [debut + c.deckOrder].GetComponentInChildren<PlayingCardController>().setTile(new Tile(c.deckOrder + 1, hauteur), tiles [c.deckOrder + 1, hauteur].GetComponent<TileController>().getPosition());
		}
		else{
			this.playingCards [debut + c.deckOrder].GetComponentInChildren<PlayingCardController>().setTile(new Tile(4-c.deckOrder, hauteur), tiles [4-c.deckOrder, hauteur].GetComponent<TileController>().getPosition());
		}
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
	
	public void displayOpponentCards(bool isFirstP){
		int debut = 0;
		if (isFirstP)
		{
			debut = this.nbCardsPerPlayer;
		}
				
		for (int i = debut; i < debut+this.nbCardsPerPlayer; i++)
		{
			this.playingCards [i].GetComponentInChildren<PlayingCardController>().display();
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
		
		GameObject tempGO = GameObject.Find("MyPlayerName");
		position = tempGO.transform.position ;
		position.x = -0.48f*this.realwidth;
		tempGO.transform.position = position;
		tempGO.GetComponent<TextContainer>().width = 0.48f*this.realwidth-3f ;
		
		tempGO = GameObject.Find("HisPlayerName");
		position = tempGO.transform.position ;
		position.x = 0.48f*this.realwidth;
		tempGO.transform.position = position;
		tempGO.GetComponent<TextContainer>().width = 0.48f*this.realwidth-3f ;
		
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
		
		this.clickedPCPosition = new Vector3(-0.50f*this.realwidth-5f,-1f,1);
		this.clickedRPC.transform.position = this.clickedPCPosition;
		
		this.hoveredPCPosition = new Vector3(0.50f*this.realwidth+5f,-1f,1);
		this.hoveredRPC.transform.position = this.hoveredPCPosition;
		
		this.skillPosition = this.skillRPC.transform.localPosition;
		this.skillPosition.x = 5f+(realwidth/2f)-8.25f;
		this.skillRPC.transform.localPosition = this.skillPosition;
		
		tempGO = GameObject.Find("Title");
		position = tempGO.transform.localPosition ;
		position.x = (realwidth/2f)-8.25f-(realwidth/2f-4.25f)/2f;
		tempGO.transform.localPosition = position;
		tempGO.GetComponent<TextContainer>().width = (realwidth/2f-4.25f) ;
		
		tempGO = GameObject.Find("IconMove");
		position = tempGO.transform.localPosition ;
		position.x = (realwidth/2f)-8.45f-0.80f*(realwidth/2f-3.75f);
		tempGO.transform.localPosition = position;
		
		tempGO = GameObject.Find("IconLife");
		position = tempGO.transform.localPosition ;
		position.x = (realwidth/2f)-8.45f-0.5f*(realwidth/2f-3.75f);
		tempGO.transform.localPosition = position;
		
		tempGO = GameObject.Find("IconAttack");
		position = tempGO.transform.localPosition ;
		position.x = (realwidth/2f)-8.45f-0.20f*(realwidth/2f-3.75f);
		tempGO.transform.localPosition = position;
		
		tempGO = GameObject.Find("TextMove");
		position = tempGO.transform.localPosition ;
		position.x = (realwidth/2f)-8.25f-0.80f*(realwidth/2f-3.75f);
		tempGO.transform.localPosition = position;
		
		tempGO = GameObject.Find("TextLife");
		position = tempGO.transform.localPosition ;
		position.x = (realwidth/2f)-8.25f-0.5f*(realwidth/2f-3.75f);
		tempGO.transform.localPosition = position;
		
		tempGO = GameObject.Find("TextAttack");
		position = tempGO.transform.localPosition ;
		position.x = (realwidth/2f)-8.25f-0.20f*(realwidth/2f-3.75f);
		tempGO.transform.localPosition = position;
		
		tempGO = GameObject.Find("Skill0Title");
		tempGO.GetComponent<TextContainer>().width = (realwidth/2f-3.65f) ;
		tempGO = GameObject.Find("Skill0Description");
		tempGO.GetComponent<TextContainer>().width = (realwidth/2f-3.75f) ;
		tempGO = GameObject.Find("Skill1Title");
		tempGO.GetComponent<TextContainer>().width = (realwidth/2f-3.85f) ;
		tempGO = GameObject.Find("Skill1Description");
		tempGO.GetComponent<TextContainer>().width = (realwidth/2f-3.95f) ;
		tempGO = GameObject.Find("Skill2Title");
		tempGO.GetComponent<TextContainer>().width = (realwidth/2f-4.05f) ;
		tempGO = GameObject.Find("Skill2Description");
		tempGO.GetComponent<TextContainer>().width = (realwidth/2f-4.15f) ;
		tempGO = GameObject.Find("Skill3Title");
		tempGO.GetComponent<TextContainer>().width = (realwidth/2f-4.25f) ;
		tempGO = GameObject.Find("Skill3Description");
		tempGO.GetComponent<TextContainer>().width = (realwidth/2f-4.35f) ;
		
		tempGO = GameObject.Find("Title2");
		position = tempGO.transform.localPosition ;
		position.x = (realwidth/2f)-8.25f-(realwidth/2f-3.75f)/2f;
		tempGO.transform.localPosition = position;
		tempGO.GetComponent<TextContainer>().width = (realwidth/2f-3.75f) ;
		
		tempGO = GameObject.Find("IconMove2");
		position = tempGO.transform.localPosition ;
		position.x = (realwidth/2f)-8.05f-0.20f*(realwidth/2f-4.25f);
		tempGO.transform.localPosition = position;
		
		tempGO = GameObject.Find("IconLife2");
		position = tempGO.transform.localPosition ;
		position.x = (realwidth/2f)-8.05f-0.5f*(realwidth/2f-4.25f);
		tempGO.transform.localPosition = position;
		
		tempGO = GameObject.Find("IconAttack2");
		position = tempGO.transform.localPosition ;
		position.x = (realwidth/2f)-8.05f-0.80f*(realwidth/2f-4.25f);
		tempGO.transform.localPosition = position;
		
		tempGO = GameObject.Find("TextMove2");
		position = tempGO.transform.localPosition ;
		position.x = (realwidth/2f)-8.25f-0.20f*(realwidth/2f-4.25f);
		tempGO.transform.localPosition = position;
		
		tempGO = GameObject.Find("TextLife2");
		position = tempGO.transform.localPosition ;
		position.x = (realwidth/2f)-8.25f-0.5f*(realwidth/2f-4.25f);
		tempGO.transform.localPosition = position;
		
		tempGO = GameObject.Find("TextAttack2");
		position = tempGO.transform.localPosition ;
		position.x = (realwidth/2f)-8.25f-0.80f*(realwidth/2f-4.25f);
		tempGO.transform.localPosition = position;
				
		this.skillButtons[0].transform.localPosition = new Vector3((0.90f*(this.realwidth/2f-3f))-5.2f, 0f,0f);
		this.skillButtons[1].transform.localPosition = new Vector3((0.74f*(this.realwidth/2f-3f))-5.2f, 0f, 0f);
		this.skillButtons[2].transform.localPosition = new Vector3((0.58f*(this.realwidth/2f-3f))-5.2f, 0f, 0f);
		this.skillButtons[3].transform.localPosition = new Vector3((0.42f*(this.realwidth/2f-3f))-5.2f, 0f, 0f);
		this.skillButtons[4].transform.localPosition = new Vector3((0.26f*(this.realwidth/2f-3f))-5.2f, 0f, 0f);
		this.skillButtons[5].transform.localPosition = new Vector3((0.10f*(this.realwidth/2f-3f))-5.2f, 0f, 0f);
		
		tempGO = GameObject.Find("IconSD");
		position = tempGO.transform.localPosition ;
		position.x = (realwidth/2f)-8.75f;
		tempGO.transform.localPosition = position;
		
		tempGO = GameObject.Find("TitleSD");
		position = tempGO.transform.localPosition ;
		position.x = (realwidth/2f)-8.55f-(realwidth/2f-3.75f)/2f;
		tempGO.transform.localPosition = position;
		tempGO.GetComponent<TextContainer>().width = (realwidth/2f-5.25f) ;
		
		tempGO = GameObject.Find("DescriptionSD");
		position = tempGO.transform.localPosition ;
		position.x = (realwidth/2f)-8.55f-(realwidth/2f-3.75f)/2f;
		tempGO.transform.localPosition = position;
		tempGO.GetComponent<TextContainer>().width = (realwidth/2f-5.25f) ;
		
//		if (EndSceneController.instance != null)
//		{
//			EndSceneController.instance.resize();
//		}
		//if (GameController.instance.isTutorialLaunched)
		//{
		//	this.tutorial.GetComponent<GameTutorialController>().resize();
		//}
	}
	
	public void hoverTile(int c, Tile t){
		if (c!=this.currentHoveredCard){
			int currentPlayingCard = GameController.instance.getCurrentPlayingCard();
			if (currentPlayingCard!=c){
				this.currentHoveredCard = c ;
				this.displayHoveredPC(c);
			}
		}
		GameObject tempGO = GameObject.Find("Hover");
		Vector3 pos = tiles[t.x, t.y].GetComponent<TileController>().getPosition();
		pos.z = -1 ;
		tempGO.transform.position = pos ;
	}
	
	public void hoverSkill(Skill s){
		if (s.Id!=this.currentHoveredSkill.Id){
			this.currentHoveredSkill = s ;
			this.displaySkill();
		}	
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
	
	public int getNbPlayingCards(){
		return this.playingCards.Length;
	}
	
	public Card getCard(int i){
		return this.playingCards[i].GetComponent<PlayingCardController>().getCard();
	}
	
	public Tile getPlayingCardTile(int i){
		return this.playingCards[i].GetComponent<PlayingCardController>().getTile();
	}
	
	public bool hasPlayed(int i){
		return this.playingCards[i].GetComponent<PlayingCardController>().getHasPlayed();
	}
	
	public bool getIsMine(int i){
		return this.playingCards[i].GetComponent<PlayingCardController>().getIsMine();
	}
	
	public void playCard(int i, bool b){
		this.playingCards[i].GetComponent<PlayingCardController>().play(b);
	}
	
	public void moveCard(int i, bool b){
		this.playingCards[i].GetComponent<PlayingCardController>().move(b);
	}
	
	public void setNeighbours(int i, bool isFirstP){
		this.playingCards [i].GetComponentInChildren<PlayingCardController>().getTile().setNeighbours(this.getCharacterTilesArray(isFirstP), this.playingCards [i].GetComponentInChildren<PlayingCardController>().getCard().GetMove());
	}
	
	public bool isDead(int i){
		return this.playingCards[i].GetComponent<PlayingCardController>().getIsDead();
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
	
	public int[,] getCharacterTilesArray(bool isFirstP)
	{		
		int[,] characterTiles = new int[this.boardWidth, this.boardHeight];
		
		for (int i = 0; i < this.boardWidth; i ++)
		{
			for (int j = 0; j < this.boardHeight; j ++)
			{
				if (this.tiles[i, j].GetComponent<TileController>().getTileType() == 1)
				{
					characterTiles [i, j] = 9;
				} 
				else
				{
					characterTiles [i, j] = -1;
				}	
			}
		}
		int debut;
		int fin;
		if (isFirstP)
		{
			debut = this.nbCardsPerPlayer;
			fin = playingCards.Length;
		} else
		{
			debut = 0;
			fin = this.nbCardsPerPlayer;
		}
		for (int i = debut; i < fin; i++)
		{
			if (!this.isDead(i))
			{
				Tile tiletemp = this.playingCards [i].GetComponentInChildren<PlayingCardController>().getTile();
				characterTiles [tiletemp.x, tiletemp.y] = 8;
			}
		}
		
		return characterTiles;
	}
	
	public void setDestinations(int idPlayer)
	{
		List<Tile> nt = this.playingCards [idPlayer].GetComponentInChildren<PlayingCardController>().getTile().neighbours.tiles;
		foreach (Tile t in nt)
		{
//			if (!this.isTutorialLaunched)
//			{
//				this.tiles [t.x, t.y].GetComponentInChildren<TileController>().setDestination(true);
//			} else
//			{
			this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().changeType(1);
			this.tileHandlers[t.x, t.y].SetActive(true);
			//			}
		}
	}
}


