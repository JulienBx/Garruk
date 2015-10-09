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
	public Sprite[] sprites;
	public Sprite[] skillSprites;
	public GameObject loadingScreenObject;
	
	bool isDisplayedItsDestinations = false ;
	
	int boardWidth = 6;
	int boardHeight = 8;
	
	int nbCardsPerPlayer = 4 ;
	int nbFreeRowsAtBeginning = 2 ;
	
	int turnTime = 30 ;
	
	GameObject[,] tiles ;
	GameObject[,] tileHandlers ;
	GameObject[] verticalBorders ;
	GameObject[] horizontalBorders ;
	List<GameObject> playingCards ;
	GameObject[] skillButtons ;
	GameObject clickedRPC ;
	GameObject myHoveredRPC ;
	GameObject hisHoveredRPC ;
	GameObject skillRPC ;
	GameObject tutorial;
	GameObject skillPopUp;
	GameObject popUp;
	GameObject popUpText;
	GameObject popUpTitle;
	GameObject opaque;
	GameObject actionButtons;
	
	GameObject timerGO;
	GameObject loadingScreen;

	int heightScreen = -1;
	int widthScreen = -1;
	float realwidth = -1;
	
	float timerTurn;
	int timerSeconds ;
	
	AudioSource audioEndTurn;
	
	bool isBackgroundLoaded = false ;
	
	float animationTime = 0.2f ;
	
	float timerRightSide ;
	
	bool isDisplayedSkill = false ;
	int statusSkill = 0;
	Vector3 skillPosition ;
	
	float timerSkill ;
	
	Skill currentHoveredSkill = new Skill(-1) ;

	bool toDisplayPopUp = false ;
	
	public bool isTargeting = false ;
	List<Tile> targets ;
	float timerTargeting ;
	float targetingTime = 0.5f ;
	bool isTargetingHaloOn = false ;
	
	int currentTargetingTileHandler = -1;
	
	int nextMove ; 
	
	bool toDisplayDeadHalos = false ;
	List<int> displayedDeads ;
	List<float> displayedDeadsTimer ;
	
	bool toDisplaySE = false ;
	List<int> displayedSE ;
	List<float> displayedSETimer ;
	
	List<Tile> destinations ;

	bool isTutorialLaunched = false;
	bool isLoadingScreenDisplayed;
		
	void Awake()
	{
		instance = this;

		this.displayLoadingScreen ();
		this.tiles = new GameObject[this.boardWidth, this.boardHeight];
		this.tileHandlers = new GameObject[this.boardWidth, this.boardHeight];
		this.playingCards = new List<GameObject>();
		this.verticalBorders = new GameObject[this.boardWidth+1];
		this.horizontalBorders = new GameObject[this.boardHeight+1];
		this.skillButtons = new GameObject[5];
		this.skillButtons[0] = GameObject.Find("AttackButton");
		this.skillButtons[1] = GameObject.Find("SkillButton0");
		this.skillButtons[2] = GameObject.Find("SkillButton1");
		this.skillButtons[3] = GameObject.Find("SkillButton2");
		this.skillButtons[4] = GameObject.Find("PassButton");
		this.opaque = GameObject.Find("Opaque");
		this.opaque.GetComponent<SpriteRenderer>().enabled = false ;
		this.actionButtons = GameObject.Find("ActionButtons");
		this.displaySkills(false);
		this.clickedRPC = GameObject.Find("ClickedPlayingCard");
		this.myHoveredRPC = GameObject.Find("MyHoveredPlayingCard");
		this.hisHoveredRPC = GameObject.Find("HisHoveredPlayingCard");
		this.skillRPC = GameObject.Find("SkillDescription");
		this.popUp = GameObject.Find("PopUp");
		this.popUpText = GameObject.Find("PopUpText");
		this.skillPopUp = GameObject.Find("SkillPopUp");
		this.skillPopUp.GetComponent<SkillPopUpController>().hide ();
		this.popUpTitle = GameObject.Find("PopUpTitle");
		this.timerGO = GameObject.Find("Timer");
		this.targets = new List<Tile>();
		this.displayedDeads = new List<int>();
		this.displayedDeadsTimer = new List<float>();
		this.displayedSE = new List<int>();
		this.displayedSETimer = new List<float>();
		
		this.audioEndTurn = GetComponent<AudioSource>();

		this.setMyPlayerName(ApplicationModel.myPlayerName);
		this.setHisPlayerName(ApplicationModel.hisPlayerName);
		
		GameController.instance.launchGC();
		
		this.createBackground();
	}
	
	void Update()
	{
		if (this.widthScreen!=-1){
			if (this.widthScreen!=Screen.width){
				this.resize();
			}
		}
		
		if(GameController.instance.getCurrentPlayingCard()!=-1){
			this.playingCards[GameController.instance.getCurrentPlayingCard()].GetComponent<PlayingCardController>().addTime(Time.deltaTime);
		}
		
		if(this.skillPopUp.GetComponent<SkillPopUpController>().getTimeToDisplay()!=0){
			this.skillPopUp.GetComponent<SkillPopUpController>().addTime(Time.deltaTime);
		}
		
		if(timerTurn>0){
			this.timerTurn += Time.deltaTime; 
			if (this.timerTurn<=0 && this.getIsMine(GameController.instance.getCurrentPlayingCard())){
				GameController.instance.resolvePass();
			}
			else{
				if(this.timerSeconds != Mathf.Min(Mathf.FloorToInt(this.timerTurn))){
					this.timerSeconds = Mathf.Min(Mathf.FloorToInt(this.timerTurn));
					if(this.timerSeconds>9){
						this.timerGO.GetComponent<TextMeshPro>().text = this.timerSeconds.ToString();
					}
					else{
						this.timerGO.GetComponent<TextMeshPro>().text = "0"+this.timerSeconds.ToString();
					}
				}
			}
		}
		
		if (this.getMyHoveredCardController().getStatus()!=0){
			this.getMyHoveredCardController().addTime(Time.deltaTime);
		}
		
		if (this.getHisHoveredCardController().getStatus()!=0){
			this.getHisHoveredCardController().addTime(Time.deltaTime);
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
		
		if (this.isTargeting){
			this.timerTargeting += Time.deltaTime;
			Tile t ;
			
			if (this.timerTargeting>this.targetingTime){
				if(this.isTargetingHaloOn){
					for (int i = 0 ; i < this.targets.Count ; i++){	
						t = this.targets[i];
						if (!this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().getIsHovered()){
							this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().disable() ;
						}
					}
				}
				else{
					for (int i = 0 ; i < this.targets.Count ; i++){	
						t = this.targets[i];
						this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().enable() ;
					}
				}
				this.isTargetingHaloOn = !this.isTargetingHaloOn;
				this.timerTargeting=0;
			}
		}
		
		if(this.toDisplayDeadHalos){
			for (int i = this.displayedDeads.Count-1 ; i >= 0 ; i--){
				this.displayedDeadsTimer[i] -= Time.deltaTime;
				if (this.displayedDeadsTimer[i]<=0f){
					Tile t = this.getPlayingCardTile(this.displayedDeads[i]);
					this.disappear(this.displayedDeads[i]);
					this.tileHandlers[t.x, t.y].SetActive(false);
					this.displayedDeads.RemoveAt(i);
					this.displayedDeadsTimer.RemoveAt(i);
					if(!this.hasMoved(GameController.instance.getCurrentPlayingCard())){
						this.removeDestinations();
						if(this.getIsMine(GameController.instance.getCurrentPlayingCard())){
							this.setDestinations(GameController.instance.getCurrentPlayingCard(), true);
						}
						else{
							this.setHisDestinations(GameController.instance.getCurrentPlayingCard(), true);
						}
					}
				}
			}
			if (this.displayedDeads.Count==0){
				this.toDisplayDeadHalos = false ;
			}
		}
		
		if(this.toDisplaySE){
			for (int i = this.displayedSE.Count-1 ; i >= 0 ; i--){
				this.displayedSETimer[i] -= Time.deltaTime;
				if (this.displayedSETimer[i]<=0f){
					Tile t = this.getPlayingCardTile(this.displayedSE[i]);
					this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().moveBack();
					this.tileHandlers[t.x, t.y].SetActive(false);
					this.displayedSE.RemoveAt(i);
					this.displayedSETimer.RemoveAt(i);
					if(!this.hasMoved(GameController.instance.getCurrentPlayingCard())){
						this.removeDestinations();
						
						if(this.getIsMine(GameController.instance.getCurrentPlayingCard())){
							this.setDestinations(GameController.instance.getCurrentPlayingCard(), true);
						}
						else{
							this.setHisDestinations(GameController.instance.getCurrentPlayingCard(), true);
						}
					}
				}
			}
			if (this.displayedSE.Count==0){
				this.toDisplaySE = false ;
			}
		}
	}
	
	public void setSkillPopUp(string texte, Card launcher, List<Card> receivers, List<string> textsReceivers){
		this.skillPopUp.GetComponent<SkillPopUpController>().setPopUp(texte, launcher, receivers, textsReceivers);
		this.skillPopUp.GetComponent<SkillPopUpController>().setTimeToDisplay(3f);
	}
	
	public void displaySkills(bool b){
		this.skillButtons[0].SetActive(b);
		this.skillButtons[1].SetActive(b);
		this.skillButtons[2].SetActive(b);
		this.skillButtons[3].SetActive(b);
		this.skillButtons[4].SetActive(b);
	}
	
	public void showSkills(){
		this.opaque.GetComponent<SpriteRenderer>().enabled = false ;
		this.skillButtons[0].GetComponent<BoxCollider>().enabled = true;
		this.skillButtons[1].GetComponent<BoxCollider>().enabled = true;
		this.skillButtons[2].GetComponent<BoxCollider>().enabled = true;
		this.skillButtons[3].GetComponent<BoxCollider>().enabled = true;
		this.skillButtons[4].GetComponent<BoxCollider>().enabled = true;
		this.actionButtons.GetComponent<TextMeshPro>().text = "" ;
	}
	
	public void overloadSkills(string s){
		this.opaque.GetComponent<SpriteRenderer>().enabled = true ;
		this.skillButtons[0].GetComponent<BoxCollider>().enabled = false;
		this.skillButtons[1].GetComponent<BoxCollider>().enabled = false;
		this.skillButtons[2].GetComponent<BoxCollider>().enabled = false;
		this.skillButtons[3].GetComponent<BoxCollider>().enabled = false;
		this.skillButtons[4].GetComponent<BoxCollider>().enabled = false;
		this.actionButtons.GetComponent<TextMeshPro>().text = s ;
	}
	
	public void changeOverloadText(string s){
		this.actionButtons.GetComponent<TextMeshPro>().text = s ;
	}
	
	public void unSelectPC(int p){
		this.playingCards[p].GetComponent<PlayingCardController>().resetTimer();
	}
	
	public void unClickPC(int p){
		Tile t = this.getPlayingCardTile(p);
		this.tileHandlers[t.x,t.y].SetActive(false);
	}
	
	public MyHoveredCardController getMyHoveredCardController(){
		return this.myHoveredRPC.GetComponent<MyHoveredCardController>();
	}
	
	public HisHoveredCardController getHisHoveredCardController(){
		return this.hisHoveredRPC.GetComponent<HisHoveredCardController>();
	}
	
	public void initTimer(){
		this.timerTurn = this.turnTime ;
		this.timerSeconds = Mathf.FloorToInt(this.timerTurn) ;
		this.timerGO.GetComponent<TextMeshPro>().text = this.timerSeconds.ToString();
	}
	
	public void selectSkill(int id){
//		this.unSelectSkill();
//		GameObject.Find ("Skill"+id+"Title").GetComponent<TextMeshPro>().color = Color.yellow;
//		GameObject.Find ("Skill"+id+"Description").GetComponent<TextMeshPro>().color = Color.yellow;
	}
	
	public void unSelectSkill(){
//		for(int i = 0 ; i < 4 ; i++){
//			GameObject.Find ("Skill"+i+"Title").GetComponent<TextMeshPro>().color = Color.white;
//			GameObject.Find ("Skill"+i+"Description").GetComponent<TextMeshPro>().color = Color.white;
//		}
	}
	
	public Sprite getSprite(int i){
		return this.sprites[i];
	}
	
	public void loadHisHoveredPC(int c){
		Card card = this.playingCards[c].GetComponent<PlayingCardController>().getCard();
		List<Skill> skills = this.playingCards[c].GetComponent<PlayingCardController>().getCard().getSkills();
		GameObject.Find("Title").GetComponent<TextMeshPro>().text = card.Title;
		GameObject.Find("HisSpecialite").GetComponent<TextMeshPro>().text = skills[0].Name;
		GameObject.Find("HisSpecialiteDescription").GetComponent<TextMeshPro>().text = skills[0].Description;
		
		this.hisHoveredRPC.GetComponent<SpriteRenderer>().sprite = this.sprites[card.ArtIndex];
		
		for (int i = 1 ; i < skills.Count ; i++){
			GameObject.Find("Skill"+(i-1)+"Title").GetComponent<TextMeshPro>().text = skills[i].Name;
			GameObject.Find(("Skill"+(i-1)+"Description")).GetComponent<TextMeshPro>().text = skills[i].Description;
		}
		for (int i = skills.Count ; i < 4 ; i++){
			GameObject.Find("Skill"+(i-1)+"Title").GetComponent<TextMeshPro>().text = "";
			GameObject.Find(("Skill"+(i-1)+"Description")).GetComponent<TextMeshPro>().text = "";
		}
	}
	
	public void loadClickedPC(){
		int currentPlayingCard = GameController.instance.getCurrentPlayingCard();
		Card c = this.playingCards[currentPlayingCard].GetComponent<PlayingCardController>().getCard();
		
		this.skillButtons[0].SetActive(true);
		this.skillButtons[0].GetComponent<SkillButtonController>().setSkill(c.GetAttackSkill(), this.skillSprites[this.skillSprites.Length-4]);
		this.skillButtons[4].SetActive(true);
		this.skillButtons[4].GetComponent<SkillButtonController>().setSkill(new Skill("Fin du tour","Termine son tour et passe la main au personnage suivant",1), this.skillSprites[this.skillSprites.Length-3]);
		
		int count = c.Skills.Count ;
		
		for (int i = 1 ; i < 4 ; i++){
			this.skillButtons[i].SetActive(true);
			if (i<count){
				this.skillButtons[i].GetComponent<SkillButtonController>().setSkill(c.Skills[i], this.skillSprites[c.Skills[i].Id]);
				GameObject.Find ("Description"+i).GetComponent<TextMeshPro>().text = c.Skills[i].Name;
			}
			else{
				if(i==1){
					this.skillButtons[i].GetComponent<SkillButtonController>().setSkill(new Skill("Non disponible","Niveau 4 requis pour débloquer cette compétence",-99), this.skillSprites[this.skillSprites.Length-2]);
					GameObject.Find ("Description"+i).GetComponent<TextMeshPro>().text = "?";
				}
				else{
					this.skillButtons[i].GetComponent<SkillButtonController>().setSkill(new Skill("Non disponible","Niveau 8 requis pour débloquer cette compétence",-99), this.skillSprites[this.skillSprites.Length-1]);
					GameObject.Find ("Description"+i).GetComponent<TextMeshPro>().text = "?";
				}
			}
		}
	}
	
	public void loadSkill(){
		int currentPlayingCard = GameController.instance.getCurrentPlayingCard();
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
		this.destinations = new List<Tile>();
		int debut = 0 ;
		if (!isFirstP){
			debut = this.boardHeight-this.nbFreeRowsAtBeginning;
		}
		for (int i = debut ; i < debut + this.nbFreeRowsAtBeginning; i++){
			for (int j = 0 ; j < this.boardWidth; j++){
				if (tiles[j,i].GetComponent<TileController>().canBeDestination()){
					this.destinations.Add(new Tile(j,i));
				}
			}
		}
		this.displayDestinations(true);
	}
	
	public void displayPopUp(string s, Vector3 position, string t){
		this.popUpText.GetComponent<TextMeshPro>().text = s ;
		this.popUpTitle.GetComponent<TextMeshPro>().text = t ;
		
		this.popUp.transform.position = position;
		
		this.toDisplayPopUp = true ;
	}
	
	public void hidePopUp(){
		this.popUp.transform.position = new Vector3(0, -10, 0);
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
	
	public void removeDestinations()
	{
		for (int i = 0; i < this.boardWidth; i++)
		{
			for (int j = 0; j < this.boardHeight; j++)
			{
				if(this.tileHandlers [i, j].GetComponent<TileHandlerController>().getTypeNumber()==1 || this.tileHandlers [i, j].GetComponent<TileHandlerController>().getTypeNumber()==9){
					this.tileHandlers [i, j].SetActive(false);
				}
			}
		}
	}
	
	public void createPlayingCard(Card c, bool isFirstP, bool isFirstPlayer)
	{
		int debut = 0 ;
		int hauteur = 0 ;

		if (!isFirstP){
			hauteur = this.boardHeight-1 ;
		}
		this.playingCards.Add((GameObject)Instantiate(this.playingCardModel));
		int index = this.playingCards.Count-1;
		if (isFirstP != isFirstPlayer)
		{
			this.playingCards [index].GetComponentInChildren<PlayingCardController>().hide();
		} 
		
		this.playingCards [index].GetComponentInChildren<PlayingCardController>().setCard(c);
		this.playingCards [index].GetComponentInChildren<PlayingCardController>().setIsMine(isFirstP==isFirstPlayer);
		this.playingCards [index].GetComponentInChildren<PlayingCardController>().setIDCharacter(index);
		this.playingCards [index].name = "Card"+(index);
		this.playingCards [index].transform.FindChild("AttackZone").GetComponent<AttackPictoController>().setIDCard(index);
		this.playingCards [index].transform.FindChild("LifeBar").transform.FindChild("PV").GetComponent<PVPictoController>().setIDCard(index);
		this.playingCards [index].transform.FindChild("PictoTR").GetComponent<TRPictoController>().setIDCard(index);
		this.playingCards [index].GetComponent<PlayingCardController>().showTR(false);
		
		if (isFirstP){
			this.playingCards [index].GetComponentInChildren<PlayingCardController>().setTile(new Tile(c.deckOrder + 1, hauteur), tiles [c.deckOrder + 1, hauteur].GetComponent<TileController>().getPosition());
			this.tiles [c.deckOrder + 1, hauteur].GetComponent<TileController>().setCharacterID(index);
		}
		else{
			this.playingCards [index].GetComponentInChildren<PlayingCardController>().setTile(new Tile(4-c.deckOrder, hauteur), tiles [4-c.deckOrder, hauteur].GetComponent<TileController>().getPosition());
			this.tiles [4-c.deckOrder, hauteur].GetComponent<TileController>().setCharacterID(index);
		}
		
		if(this.getCard(index).isPacifiste()){
			int amount = this.getCard(index).getPassiveManacost();
			int amountLife = Mathf.CeilToInt(this.getCard(index).GetLife()*amount / 100f);
			int amountAttack = Mathf.CeilToInt(amount * this.getCard(index).GetAttack() / 100f);
			
			this.getCard(index).addModifier(1*amountLife, ModifierType.Type_BonusMalus, ModifierStat.Stat_Life, -1, 31, "PACIFISTE", "+"+amountLife+" PV. Permanent.", "");
			this.getCard(index).addModifier(-1*amountAttack, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, -1, 24, "PACIFISTE", "-"+amountAttack+" ATK. Permanent.", "");
			this.playingCards [index].GetComponentInChildren<PlayingCardController>().show(false);
		}
		else if(this.getCard(index).isAguerri()){
			int amount = this.getCard(index).getPassiveManacost();
			int amountAttack = Mathf.CeilToInt(this.getCard(index).GetAttack()*amount / 100f);
			
			this.getCard(index).addModifier(amountAttack, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, -1, 25, "AGUERRI", "+"+amountAttack+" ATK. Permanent.", "");
			this.playingCards [index].GetComponentInChildren<PlayingCardController>().show(false);
		}
		else if(this.getCard(index).isRapide()){
			int amount = this.getCard(index).getPassiveManacost();
			int amountMove = Mathf.CeilToInt(this.getCard(index).GetMove()*amount / 100f);
			int amountAttack = Mathf.CeilToInt(this.getCard(index).GetAttack()*amount / 100f);
			
			this.getCard(index).addModifier(-1*amountAttack, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, -1, 26, "RAPIDE", "-"+amountAttack+" ATK. Permanent.", "");
			this.getCard(index).addModifier(amountMove, ModifierType.Type_BonusMalus, ModifierStat.Stat_Move, -1, 13, "RAPIDE", "+"+amountMove+" ATK. Permanent.", "");
			
			this.playingCards [index].GetComponentInChildren<PlayingCardController>().show(false);
		}
		else if(this.getCard(index).isRobuste()){
			int amount = this.getCard(index).getPassiveManacost();
			int amountMove = Mathf.CeilToInt(this.getCard(index).GetMove()*amount / 100f);
			int amountAttack = Mathf.CeilToInt(this.getCard(index).GetAttack()*amount / 100f);
			
			this.getCard(index).addModifier(amountAttack, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, -1, 27, "RAPIDE", "+"+amountAttack+" ATK. Permanent.", "Permanent");
			this.getCard(index).addModifier(-amountMove, ModifierType.Type_BonusMalus, ModifierStat.Stat_Move, -1, 14, "RAPIDE", "-"+amountMove+" MOV. Permanent.", "");
			
			this.playingCards [index].GetComponentInChildren<PlayingCardController>().show(false);
		}
		else if(this.getCard(index).isPiegeur()){
			if (isFirstP == isFirstPlayer)
			{
				print ("Je mets des pièges");
				int randomNumber ;
				List<Tile> tiles = this.getFreeCenterTiles();
				
				if(tiles.Count>0){
					randomNumber = UnityEngine.Random.Range(0,tiles.Count);
					GameController.instance.addElectroPiege(tiles[randomNumber].x, tiles[randomNumber].y, this.getCard(index).getPassiveManacost(), GameController.instance.getIsFirstPlayer());
					tiles.RemoveAt(randomNumber);
				}
				
				if(tiles.Count>0){
					randomNumber = UnityEngine.Random.Range(0,tiles.Count);
					GameController.instance.addElectroPiege(tiles[randomNumber].x, tiles[randomNumber].y, this.getCard(index).getPassiveManacost(), GameController.instance.getIsFirstPlayer());
				}
			}
		}
		else if(this.getCard(index).isAgile()){
			int amount = this.getCard(index).getPassiveManacost();
			this.getCard(index).addModifier(amount, ModifierType.Type_EsquivePercentage, ModifierStat.Stat_No, -1, 1, "AGILITE", "Peut esquiver les dégats, Probabilité : "+amount+"%. Permanent.", "Permanent");
			this.playingCards [index].GetComponentInChildren<PlayingCardController>().show(false);
		}
	}
	
	public List<Tile> getFreeCenterTiles(){
		List<Tile> freeCenterTiles = new List<Tile>();
		for(int x = 0 ; x < this.boardWidth ; x++){
			for(int y = 2 ; y < this.boardHeight-2 ; y++){
				if(this.tiles[x,y].GetComponent<TileController>().getTileType()==0 && this.tiles[x,y].GetComponent<TileController>().getTrapId()==-1){
					freeCenterTiles.Add (new Tile(x,y));
				}
			}
		}
		return freeCenterTiles;
	}
	
	public void showTR(int i)
	{
		this.playingCards [i].GetComponent<PlayingCardController>().showTR(true);
	}
	
	public void movePlayingCard(int x, int y, int c)
	{
		Tile t = this.playingCards [c].GetComponentInChildren<PlayingCardController>().getTile();
		this.tiles[t.x, t.y].GetComponentInChildren<TileController>().setCharacterID(-1);
		this.playingCards [c].GetComponentInChildren<PlayingCardController>().setTile(new Tile(x,y), this.tiles[x,y].GetComponentInChildren<TileController>().getPosition());
		this.tiles[x, y].GetComponentInChildren<TileController>().setCharacterID(c);
		
		if(this.getIsMine(c)){
			this.tiles[x, y].GetComponentInChildren<TileController>().checkTrap(c);
		}
		
		if (GameController.instance.hasGameStarted() && this.getIsMine(GameController.instance.getCurrentPlayingCard())){
			this.checkSkillsLaunchability();
		}
		this.moveCard(c, true);
		print ("Je mets move à true pour "+c);
	}
	
	public void hideTrap(int x, int y){
		this.tiles [x, y].GetComponent<TileController>().removeTrap();
	}
	
	public void checkSkillsLaunchability(){
		List<Skill> skills = this.getCard(GameController.instance.getCurrentPlayingCard()).getSkills();
		this.skillButtons[0].GetComponentInChildren<SkillButtonController>().checkLaunchability();
		for (int i = 0 ; i < skills.Count ; i++){
			this.skillButtons[1+i].GetComponentInChildren<SkillButtonController>().checkLaunchability();
		}
	}
	
	public void resize()
	{
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
			this.playingCards [i].GetComponentInChildren<PlayingCardController>().display(false);
		}
	}
	
	public void updateMyLifeBar(){
		GameObject llbr = GameObject.Find("LLBRight");
		llbr.transform.position = new Vector3(-1.2f, 4.5f, 0);
		
		GameObject leb = GameObject.Find("LLBRightEnd");
		leb.transform.position = new Vector3(-1.2f, 4.5f, 0);
		
		GameObject llbl = GameObject.Find("LLBLeft");
		llbl.transform.position = new Vector3(-this.realwidth/2f+0.25f, 4.5f, 0);
		
		GameObject lcb = GameObject.Find("LLBLeftEnd");
		lcb.transform.position = new Vector3(llbr.transform.position.x-0.5f+(this.getPercentageLifeMyPlayer())*(-llbr.transform.position.x+0.5f+(llbl.transform.position.x+0.1f))/100, 4.5f, 0);
		
		GameObject llbb = GameObject.Find("LLBBar");
		llbb.transform.position = new Vector3((leb.transform.position.x+lcb.transform.position.x)/2f, 4.5f, 0);
		llbb.transform.localScale = new Vector3((leb.transform.position.x-lcb.transform.position.x-0.49f)/10f, 0.5f, 0.5f);
	}
	
	public void updateHisLifeBar(){
		GameObject rlbl = GameObject.Find("RLBLeft");
		rlbl.transform.position = new Vector3(1.2f, 4.5f, 0);
		
		GameObject rlbr = GameObject.Find("RLBRight");
		rlbr.transform.position = new Vector3(this.realwidth/2f-0.25f, 4.5f, 0);
		
		GameObject rlbc = GameObject.Find("RLBCenter");
		rlbc.transform.position = new Vector3((rlbl.transform.position.x+rlbr.transform.position.x)/2f, 4.5f, 0);
		rlbc.transform.localScale = new Vector3((-rlbl.transform.position.x+rlbr.transform.position.x-0.49f)/10f, 0.5f, 0.5f);
		
		GameObject reb = GameObject.Find("RLBRightEnd");
		reb.transform.position = new Vector3(rlbl.transform.position.x+0.5f+(this.getPercentageLifeHisPlayer())*(-rlbl.transform.position.x-0.5f+(rlbr.transform.position.x-0.1f))/100, 4.5f, 0);
		
		GameObject rcb = GameObject.Find("RLBLeftEnd");
		rcb.transform.position = new Vector3(1.20f, 4.5f, 0);
		
		GameObject rlbb = GameObject.Find("RLBBar");
		rlbb.transform.position = new Vector3((reb.transform.position.x+rcb.transform.position.x)/2f, 4.5f, 0);
		rlbb.transform.localScale = new Vector3((reb.transform.position.x-rcb.transform.position.x-0.49f)/10f, 0.5f, 0.5f);	
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
		lcb.transform.position = new Vector3(llbr.transform.position.x-0.5f+100f*(-llbr.transform.position.x+0.5f+(llbl.transform.position.x+0.1f))/100, 4.5f, 0);
		
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
		reb.transform.position = new Vector3(rlbl.transform.position.x+0.5f+100f*(-rlbl.transform.position.x-0.5f+(rlbr.transform.position.x-0.1f))/100, 4.5f, 0);
		
		GameObject rcb = GameObject.Find("RLBLeftEnd");
		rcb.transform.position = new Vector3(1.20f, 4.5f, 0);
		
		GameObject rlbb = GameObject.Find("RLBBar");
		rlbb.transform.position = new Vector3((reb.transform.position.x+rcb.transform.position.x)/2f, 4.5f, 0);
		rlbb.transform.localScale = new Vector3((reb.transform.position.x-rcb.transform.position.x-0.49f)/10f, 0.5f, 0.5f);
		
		this.getMyHoveredCardController().resize(realwidth, tileScale);
		this.getHisHoveredCardController().resize(realwidth, tileScale);
	}
	
	public void hoverTile(int c, Tile t, bool toCheckSkills){
		
		int currentPlayingCard = GameController.instance.getCurrentPlayingCard();
		
		if (this.isTargeting && this.currentTargetingTileHandler!=-1 && this.currentTargetingTileHandler!=c){	
			Tile tile = this.getPlayingCardTile(currentTargetingTileHandler);
			this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().changeType(6);
			this.tileHandlers[tile.x, tile.y].GetComponentInChildren<TextMeshPro>().text = "";
			this.currentTargetingTileHandler = -1 ;
		}
		
		if(c!=-1){
			if(GameController.instance.getCurrentPlayingCard()!=-1){		
				if (GameController.instance.getCurrentPlayingCard()!=c){
					this.clearDestinations();
					print("Je clear");
					
					this.isDisplayedItsDestinations = true ;
					if(!this.hasMoved(GameController.instance.getCurrentPlayingCard())){
						if (this.getIsMine(c)){
							print("Je set my destinations en false");
							this.setDestinations(c, false);
						}
						else{
							print("Je set his destinations en false");
							this.setHisDestinations(c, false);
						}
					}
				}
				else{
					if(this.isDisplayedItsDestinations){
						this.clearDestinations();
						if(!this.hasMoved(GameController.instance.getCurrentPlayingCard())){
							if (this.getIsMine(GameController.instance.getCurrentPlayingCard())){
						
								this.setDestinations(GameController.instance.getCurrentPlayingCard(), true);
							}
							else{
								this.setHisDestinations(GameController.instance.getCurrentPlayingCard(), true);
							}
						}
						this.isDisplayedItsDestinations=false;
					}
				}
			}
			if (this.getIsMine(c)){
				if(this.getMyHoveredCardController().getIsDisplayed()){
					if(c!=this.getMyHoveredCardController().getCurrentCharacter()){
						if(this.getMyHoveredCardController().getStatus()==-1){
							
						}
						else{
							this.getMyHoveredCardController().hide();
						}
						this.getMyHoveredCardController().setNextDisplayedCharacter(c) ;
					}
					else{
						if(this.getMyHoveredCardController().getStatus()==-1){
							this.getMyHoveredCardController().reverse(1);
							this.getMyHoveredCardController().setNextDisplayedCharacter(-1) ;
						}
					}
				}
				else{
					if(this.getMyHoveredCardController().getStatus()==1){
						if(c!=this.getMyHoveredCardController().getCurrentCharacter()){
							this.getMyHoveredCardController().reverse(-1);
							this.getMyHoveredCardController().setNextDisplayedCharacter(c) ;
						}
					}
					else{
						this.getMyHoveredCardController().setNextDisplayedCharacter(c) ;
						this.getMyHoveredCardController().launchNextMove();
					}
				}
			}
			else{
				if(this.getHisHoveredCardController().getIsDisplayed()){
					if(c!=this.getHisHoveredCardController().getCurrentCharacter()){
						if(this.getHisHoveredCardController().getStatus()==-1){
							
						}
						else{
							this.getHisHoveredCardController().hide();
						}
						this.getHisHoveredCardController().setNextDisplayedCharacter(c) ;
					}
					else{
						if(this.getHisHoveredCardController().getStatus()==-1){
							this.getHisHoveredCardController().reverse(1);
							this.getHisHoveredCardController().setNextDisplayedCharacter(-1);
						}
					}
				}
				else{
					if(this.getHisHoveredCardController().getStatus()==1){
						if(c!=this.getHisHoveredCardController().getCurrentCharacter()){
							this.getHisHoveredCardController().reverse(-1);
							this.getHisHoveredCardController().setNextDisplayedCharacter(c) ;
						}
					}
					else{
						this.getHisHoveredCardController().setNextDisplayedCharacter(c) ;
						this.getHisHoveredCardController().launchNextMove();
					}
				}
			}
		}
		else{
			if(GameController.instance.getCurrentPlayingCard()!=-1){
				this.clearDestinations();
				print("Je clear");
				if(!this.hasMoved(GameController.instance.getCurrentPlayingCard())){
					if (this.getIsMine(GameController.instance.getCurrentPlayingCard())){
						print("Je set my destinations en true");
						this.setDestinations(GameController.instance.getCurrentPlayingCard(), true);
					}
					else{
						print("Je set his destinations en true");
						this.setHisDestinations(GameController.instance.getCurrentPlayingCard(), true);
					}
				}
				else{
					print("Le joueur a bougé");
				}
				this.isDisplayedItsDestinations=false;
			}
			if(currentPlayingCard!=-1){
				if(this.getIsMine(currentPlayingCard)){
					if(this.getMyHoveredCardController().getIsDisplayed()){
						if(this.getMyHoveredCardController().getStatus()==-1){
							this.getMyHoveredCardController().reverse(1);
							this.getMyHoveredCardController().setNextDisplayedCharacter(currentPlayingCard) ;
					 	}
						else if (this.getMyHoveredCardController().getCurrentCharacter()!=currentPlayingCard){
							if(this.getMyHoveredCardController().getStatus()==1){
								this.getMyHoveredCardController().reverse(-1);
								this.getMyHoveredCardController().setNextDisplayedCharacter(currentPlayingCard) ;
							}
							else{
								this.getMyHoveredCardController().setNextDisplayedCharacter(currentPlayingCard) ;
								this.getMyHoveredCardController().launchNextMove();
							}
					 	}
					}
					else{
						if(this.getMyHoveredCardController().getStatus()==1){
							if(this.getMyHoveredCardController().getCurrentCharacter()!=currentPlayingCard){
								this.getMyHoveredCardController().reverse(-1);
								this.getMyHoveredCardController().setNextDisplayedCharacter(currentPlayingCard) ;
							}
						}
						else{
							this.getMyHoveredCardController().setNextDisplayedCharacter(currentPlayingCard) ;
							this.getMyHoveredCardController().launchNextMove();
						}
					}
					if(!this.getHisHoveredCardController().getIsDisplayed()){
						if(this.getHisHoveredCardController().getStatus()==1){
							this.getHisHoveredCardController().reverse(-1);
							this.getHisHoveredCardController().setNextDisplayedCharacter(-1);
						}
					}
					else{
						this.getHisHoveredCardController().setNextDisplayedCharacter(-1);
						this.getHisHoveredCardController().hide();
					}
				}
				else{
					if(this.getHisHoveredCardController().getIsDisplayed()){
						if (this.getHisHoveredCardController().getStatus()==1){
							this.getHisHoveredCardController().reverse(1);
							this.getHisHoveredCardController().setNextDisplayedCharacter(-1);
						}
						else{
							if(this.getHisHoveredCardController().getStatus()==-1){
								this.getHisHoveredCardController().setNextDisplayedCharacter(currentPlayingCard) ;
							}
							else{
								this.getHisHoveredCardController().hide();
								this.getHisHoveredCardController().setNextDisplayedCharacter(currentPlayingCard) ;
							}
						}
					}
					else{
						if(this.getHisHoveredCardController().getStatus()==1){
							if(this.getHisHoveredCardController().getCurrentCharacter()==currentPlayingCard){
							
							}
							else{
								this.getHisHoveredCardController().reverse(-1);
								this.getHisHoveredCardController().setNextDisplayedCharacter(currentPlayingCard) ;
							}
						}
						else{
							this.getHisHoveredCardController().setNextDisplayedCharacter(currentPlayingCard);
							this.getHisHoveredCardController().launchNextMove();
						}
					}
				}
			}
			else{
				int clickedCard = GameController.instance.getClickedCard();
				if (clickedCard!=-1){
					if (this.getMyHoveredCardController().getIsDisplayed()){
						if(this.getMyHoveredCardController().getCurrentCharacter()!=clickedCard){
							if(this.getMyHoveredCardController().getStatus()==0){
								this.getMyHoveredCardController().hide();
							}
							this.getMyHoveredCardController().setNextDisplayedCharacter(clickedCard);
						}
					}
					else{
						if(this.getMyHoveredCardController().getStatus()==1){
							this.getMyHoveredCardController().reverse(-1);
							this.getMyHoveredCardController().setNextDisplayedCharacter(clickedCard);
						}
						else{
							this.getMyHoveredCardController().setNextDisplayedCharacter(clickedCard);
							this.getMyHoveredCardController().launchNextMove();
						}
					}
				}
				else{
					if (this.getMyHoveredCardController().getIsDisplayed()){
						if(this.getMyHoveredCardController().getStatus()==0){
							this.getMyHoveredCardController().hide();
						}
						this.getMyHoveredCardController().setNextDisplayedCharacter(-1);
					}
					else{
						if(this.getMyHoveredCardController().getStatus()==1){
							this.getMyHoveredCardController().reverse(-1);
							this.getMyHoveredCardController().setNextDisplayedCharacter(-1);
						}
					}
				}
			}
		}
		
		GameObject tempGO = GameObject.Find("Hover");
		Vector3 pos = tiles[t.x, t.y].GetComponent<TileController>().getPosition();
		pos.z = -1 ;
		tempGO.transform.position = pos ;
	}
	
	public void changePlayingCard(int c){
		Tile t = this.getPlayingCardTile(c);
		if(this.getIsMine(c)){
			this.loadClickedPC();
		}
		this.playingCards[c].GetComponent<PlayingCardController>().moveForward();
		this.hoverTile(c, t, false);
	}
	
	public void changeClickedCard(int c){
		Tile t = this.getPlayingCardTile(c);
		this.tileHandlers[t.x,t.y].SetActive(true);
		this.tileHandlers[t.x,t.y].GetComponent<TileHandlerController>().setCharacterID(c);
		this.tileHandlers[t.x,t.y].GetComponent<TileHandlerController>().changeType(7);
	}
	
	public void loadPlayInformation(){
		this.currentHoveredSkill = new Skill("C'est votre tour !", "Choisissez l'action à effectuer par votre héros ", 1) ;
		GameObject.Find("AdditionnalInfo").GetComponent<TextMeshPro>().text = "";
		this.loadSkill();
		this.statusSkill = 1 ;
	}
	
	public void hoverTileHandler(int c, Tile t){
		
		this.hoverTile(c,t,true);
		
		if (this.currentTargetingTileHandler !=c && this.currentTargetingTileHandler!=-1){
			this.tileHandlers[this.getPlayingCardTile(currentTargetingTileHandler).x, this.getPlayingCardTile(currentTargetingTileHandler).y].GetComponent<TileHandlerController>().changeType(6);
			this.tileHandlers[this.getPlayingCardTile(currentTargetingTileHandler).x,this.getPlayingCardTile(currentTargetingTileHandler).y].GetComponentInChildren<TextMeshPro>().text = "";
		}
		this.tileHandlers[t.x,t.y].GetComponent<TileHandlerController>().changeType(2);
		if(c==-1){
			this.tileHandlers[t.x,t.y].GetComponentInChildren<TextMeshPro>().text = GameSkills.instance.getCurrentGameSkill().getTargetText(-1, new Card());
		}
		else{
			this.tileHandlers[t.x,t.y].GetComponentInChildren<TextMeshPro>().text = GameSkills.instance.getCurrentGameSkill().getTargetText(c, this.getCard(c));
		}
		
		this.currentTargetingTileHandler = c ;
	}
	
	public int getNbPlayingCards(){
		return this.playingCards.Count;
	}
	
	public Card getCard(int i){
		return this.playingCards[i].GetComponent<PlayingCardController>().getCard();
	}
	
	public Tile getPlayingCardTile(int i){
		return this.playingCards[i].GetComponent<PlayingCardController>().getTile();
	}
	
	public Tile getTile(int x, int y){
		return this.tiles[x,y].GetComponent<TileController>().getTile();
	}
	
	public int getTileCharacterID(int x, int y){
		return this.tiles[x,y].GetComponent<TileController>().getCharacterID();
	}
	
	public bool hasPlayed(int i){
		return this.playingCards[i].GetComponent<PlayingCardController>().getHasPlayed();
	}
	
	public bool hasMoved(int i){
		return this.playingCards[i].GetComponent<PlayingCardController>().getHasMoved();
	}
	
	public bool getIsMine(int i){
		return this.playingCards[i].GetComponent<PlayingCardController>().getIsMine();
	}
	
	public void playCard(int i, bool b){
		this.playingCards[i].GetComponent<PlayingCardController>().play(b);
	}
	
	public void checkModifyers(int i){
		this.playingCards[i].GetComponent<PlayingCardController>().checkModyfiers();
	}
	
	public void moveCard(int i, bool b){
		this.playingCards[i].GetComponent<PlayingCardController>().move(b);
	}
	
	public void setDestinations(int i, bool b){
		bool[,] hasBeenPassages = new bool[this.boardWidth, this.boardHeight];
		bool[,] isDestination = new bool[this.boardWidth, this.boardHeight];
		
		for(int l = 0 ; l < this.boardWidth ; l++){
			for(int k = 0 ; k < this.boardHeight ; k++){
				hasBeenPassages[l,k]=false;
				isDestination[l,k]=false;
			}
		}
		
		this.destinations = new List<Tile>();
		List<Tile> baseTiles = new List<Tile>();
		List<Tile> tempTiles = new List<Tile>();
		List<Tile> tempNeighbours ;
		baseTiles.Add(this.getPlayingCardTile(i));
		int move = this.getCard(i).GetMove();
		
		int j = 0 ;
		while (j < move){
			tempTiles = new List<Tile>();
			
			for(int k = 0 ; k < baseTiles.Count ; k++){
				tempNeighbours = this.getDestinationImmediateNeighbours(baseTiles[k]);
				for(int l = 0 ; l < tempNeighbours.Count ; l++){
					
					if(!hasBeenPassages[tempNeighbours[l].x, tempNeighbours[l].y]){
						tempTiles.Add(tempNeighbours[l]);
						hasBeenPassages[tempNeighbours[l].x, tempNeighbours[l].y]=true;
					}
					if(this.tiles[tempNeighbours[l].x, tempNeighbours[l].y].GetComponent<TileController>().getCharacterID()==-1){
						if(!isDestination[tempNeighbours[l].x, tempNeighbours[l].y]){
							destinations.Add(tempNeighbours[l]);
							isDestination[tempNeighbours[l].x, tempNeighbours[l].y]=true;
						}
					}
				}	
			}
			baseTiles = new List<Tile>();
			for(int l = 0 ; l < tempTiles.Count ; l++){
				baseTiles.Add(tempTiles[l]);
			}
			j++;
		}
		this.displayDestinations(b);
	}
	
	public void setHisDestinations(int i, bool b){
		bool[,] hasBeenPassages = new bool[this.boardWidth, this.boardHeight];
		bool[,] isDestination = new bool[this.boardWidth, this.boardHeight];
		
		for(int l = 0 ; l < this.boardWidth ; l++){
			for(int k = 0 ; k < this.boardHeight ; k++){
				hasBeenPassages[l,k]=false;
				isDestination[l,k]=false;
			}
		}
		
		this.destinations = new List<Tile>();
		List<Tile> baseTiles = new List<Tile>();
		List<Tile> tempTiles = new List<Tile>();
		List<Tile> tempNeighbours ;
		baseTiles.Add(this.getPlayingCardTile(i));
		int move = this.getCard(i).GetMove();
		
		int j = 0 ;
		while (j < move){
			tempTiles = new List<Tile>();
			
			for(int k = 0 ; k < baseTiles.Count ; k++){
				tempNeighbours = this.getDestinationHisImmediateNeighbours(baseTiles[k]);
				for(int l = 0 ; l < tempNeighbours.Count ; l++){
					if(!hasBeenPassages[tempNeighbours[l].x, tempNeighbours[l].y]){
						tempTiles.Add(tempNeighbours[l]);
						hasBeenPassages[tempNeighbours[l].x, tempNeighbours[l].y]=true;
					}
					if(this.tiles[tempNeighbours[l].x, tempNeighbours[l].y].GetComponent<TileController>().getCharacterID()==-1){
						if(!isDestination[tempNeighbours[l].x, tempNeighbours[l].y]){
							destinations.Add(tempNeighbours[l]);
							isDestination[tempNeighbours[l].x, tempNeighbours[l].y]=true;
						}
					}
				}	
			}
			baseTiles = new List<Tile>();
			for(int l = 0 ; l < tempTiles.Count ; l++){
				baseTiles.Add(tempTiles[l]);
			}
			j++;
		}
		this.displayHisDestinations(b);
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
	
	public List<Tile> getMyPlayingCardsTiles(){
		List<Tile> tiles = new List<Tile>();
		for (int i = 0 ; i < this.playingCards.Count ; i++){
			if (this.getIsMine(i)){
				tiles.Add(this.getPlayingCardTile(i));
			}
		}
		return tiles;
	}
	
	public void clearDestinations(){
		for (int i = 0 ; i < GameView.instance.boardWidth ; i++){
			for (int j = 0 ; j < GameView.instance.boardHeight ; j++){
				if(this.tileHandlers[i,j].activeSelf){
					if(this.tileHandlers[i,j].GetComponent<TileHandlerController>().getTypeNumber()!=6){
						this.tileHandlers[i, j].GetComponent<TileHandlerController>().changeType(0);
						this.tileHandlers[i, j].GetComponent<TileHandlerController>().setText("");
						this.tileHandlers[i, j].SetActive(false);
					}
				}
			}
		}
	}
	
	public void displayDestinations(bool b)
	{
		int i = 10;
		if(b){
			i = 1;
		}
		
		foreach (Tile t in destinations)
		{
			this.tileHandlers[t.x, t.y].SetActive(true);
			this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().changeType(i);
			this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().enable();
		}
	}
	
	public void displayHisDestinations(bool b)
	{
		int i = 10;
		if(b){
			i = 9;
		}
		
		foreach (Tile t in destinations)
		{
			this.tileHandlers[t.x, t.y].SetActive(true);
			this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().changeType(i);
			this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().enable();
		}
	}
	
	public void hideStartButton(){
		GameObject.Find("StartButton").SetActive(false);
	}
	
	public void showStartButton(){
		GameObject go = GameObject.Find("SB");
		foreach (Transform child in go.transform)
		{
			child.gameObject.SetActive(true);
		}
	}
	
	public void displayAdjacentOpponentsTargets()
	{
		Tile tile ;
		this.targets = new List<Tile>();
		List<Tile> neighbourTiles = this.getOpponentImmediateNeighbours(this.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()));
		int playerID;
		foreach (Tile t in neighbourTiles)
		{
			playerID = this.tiles [t.x, t.y].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getIsMine(playerID))
				{
					tile = this.getPlayingCardTile(playerID);
					this.targets.Add(tile);
					this.tileHandlers[tile.x, tile.y].SetActive(true);
					this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().changeType(6);
					this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setText("");
					this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setCharacterID(playerID);
				}
			}
		}
		this.timerTargeting = 0 ;
		this.currentTargetingTileHandler = -1;
		this.isTargeting = true ;
	}
	
	public void display1TileAwayOpponentsTargets()
	{
		int playerID;
		Tile tile = this.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()) ;
		if(tile.x>1){
			playerID = this.tiles [tile.x-2, tile.y].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if(!this.getIsMine(this.getTileCharacterID(tile.x-2, tile.y))){
					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getIsMine(playerID))
					{
						tile = this.getPlayingCardTile(playerID);
						this.targets.Add(tile);
						this.tileHandlers[tile.x, tile.y].SetActive(true);
						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().changeType(6);
						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setText("");
						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setCharacterID(playerID);
					}
				}
			}
		}
		if(tile.x<this.boardWidth-2){
			playerID = this.tiles [tile.x+2, tile.y].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if(!this.getIsMine(this.getTileCharacterID(tile.x+2, tile.y))){
					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getIsMine(playerID))
					{
						tile = this.getPlayingCardTile(playerID);
						this.targets.Add(tile);
						this.tileHandlers[tile.x, tile.y].SetActive(true);
						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().changeType(6);
						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setText("");
						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setCharacterID(playerID);
					}
				}
			}
		}
		if(tile.y>1){
			playerID = this.tiles [tile.x, tile.y-2].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if(!this.getIsMine(this.getTileCharacterID(tile.x, tile.y-2))){
					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getIsMine(playerID))
					{
						tile = this.getPlayingCardTile(playerID);
						this.targets.Add(tile);
						this.tileHandlers[tile.x, tile.y].SetActive(true);
						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().changeType(6);
						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setText("");
						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setCharacterID(playerID);
					}
				}
			}
		}
		if(tile.y<this.boardHeight-2){
			playerID = this.tiles [tile.x, tile.y+2].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if(!this.getIsMine(this.getTileCharacterID(tile.x, tile.y+2))){
					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getIsMine(playerID))
					{
						tile = this.getPlayingCardTile(playerID);
						this.targets.Add(tile);
						this.tileHandlers[tile.x, tile.y].SetActive(true);
						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().changeType(6);
						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setText("");
						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setCharacterID(playerID);
					}
				}
			}
		}
		
		this.timerTargeting = 0 ;
		this.currentTargetingTileHandler = -1;
		this.isTargeting = true ;
	}
	
	public string canLaunch1TileAwayOpponents()
	{
		string isLaunchable = "Aucun ennemi à portée de lance";
		
		int playerID;
		Tile tile = this.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()) ;
		
		if(tile.x>1){
			playerID = this.tiles [tile.x-2, tile.y].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if(!this.getIsMine(this.getTileCharacterID(tile.x-2, tile.y))){
					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getIsMine(playerID))
					{
						isLaunchable="";
					}
				}
			}
		}
		if(tile.x<this.boardWidth-2){
			playerID = this.tiles [tile.x+2, tile.y].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if(!this.getIsMine(this.getTileCharacterID(tile.x+2, tile.y))){
					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getIsMine(playerID))
					{
						isLaunchable="";
					}
				}
			}
		}
		if(tile.y>1){
			playerID = this.tiles [tile.x, tile.y-2].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if(!this.getIsMine(this.getTileCharacterID(tile.x, tile.y-2))){
					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getIsMine(playerID))
					{
						isLaunchable="";
					}
				}
			}
		}
		if(tile.y<this.boardHeight-2){
			playerID = this.tiles [tile.x, tile.y+2].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if(!this.getIsMine(this.getTileCharacterID(tile.x, tile.y+2))){
					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getIsMine(playerID))
					{
						isLaunchable="";
					}
				}
			}
		}
		
		return isLaunchable;
	}
	
	public void displayAdjacentAllyTargets()
	{
		Tile tile ;
		this.targets = new List<Tile>();
		List<Tile> neighbourTiles = this.getAllyImmediateNeighbours(this.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()));
		int playerID;
		foreach (Tile t in neighbourTiles)
		{
			playerID = this.tiles [t.x, t.y].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && this.getIsMine(playerID))
				{
					tile = this.getPlayingCardTile(playerID);
					this.targets.Add(tile);
					this.tileHandlers[tile.x, tile.y].SetActive(true);
					this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().changeType(6);
					this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setText("");
					this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setCharacterID(playerID);
				}
			}
		}
		this.timerTargeting = 0 ;
		this.currentTargetingTileHandler = -1;
		this.isTargeting = true ;
	}
	
	public void displayAdjacentTileTargets()
	{
		List<Tile> neighbourTiles = this.getFreeImmediateNeighbours(this.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()));
		this.targets = new List<Tile>();
		
		foreach (Tile t in neighbourTiles){
			this.targets.Add(t);
			this.tileHandlers[t.x, t.y].SetActive(true);
			this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().changeType(6);
			this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().setText("");
			this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().setCharacterID(-1);
		}
		this.isTargeting = true ;
	}
	
	public void displayAllButMeModifiersTargets()
	{
		PlayingCardController pcc;
		this.targets = new List<Tile>();
		Tile tile ;
		
		for (int i = 0; i < this.playingCards.Count; i++)
		{
			pcc = this.getPCC(i);
			if (pcc.getCard().hasModifiers() && pcc.canBeTargeted() && i != GameController.instance.getCurrentPlayingCard())
			{
				tile = this.getPlayingCardTile(i);
				this.targets.Add(tile);
				this.tileHandlers[tile.x, tile.y].SetActive(true);
				this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().changeType(6);
				this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setText("");
				this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setCharacterID(i);
			}
		}		
	}
	
	public void displayOpponentsTargets()
	{
		PlayingCardController pcc;
		Tile tile ;
		this.targets = new List<Tile>();
		
		for (int i = 0; i < this.playingCards.Count; i++)
		{
			pcc = this.getPCC(i);
			if (!this.getIsMine(i) && pcc.canBeTargeted())
			{
				tile = this.getPlayingCardTile(i);
				this.targets.Add(tile);
				this.tileHandlers[tile.x, tile.y].SetActive(true);
				this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().changeType(6);
				this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setText("");
				this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setCharacterID(i);
			}
		}
		this.timerTargeting = 0 ;
		this.currentTargetingTileHandler = -1;
		this.isTargeting = true ;	
	}
	
	public void displayAllysButMeTargets()
	{
		PlayingCardController pcc;
		Tile tile ;
		this.targets = new List<Tile>();
		
		for (int i = 0; i < this.playingCards.Count; i++)
		{
			pcc = this.getPCC(i);
			if (this.getIsMine(i) && pcc.canBeTargeted() &&  i != GameController.instance.getCurrentPlayingCard())
			{
				tile = this.getPlayingCardTile(i);
				this.targets.Add(tile);
				this.tileHandlers[tile.x, tile.y].SetActive(true);
				this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().changeType(6);
				this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setText("");
				this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setCharacterID(i);
			}
		}
		this.timerTargeting = 0 ;
		this.currentTargetingTileHandler = -1;
		this.isTargeting = true ;	
	}
	
	public PlayingCardController getPCC(int i){
		return this.playingCards[i].GetComponent<PlayingCardController>();
	}
	
	public void hideTargets(){
		this.isTargeting = false ;
		this.currentTargetingTileHandler = -1;
		for (int i = 0 ; i < this.targets.Count ; i++){
			this.tileHandlers[this.targets[i].x, this.targets[i].y].SetActive(false);
		}
		if(!hasMoved(GameController.instance.getCurrentPlayingCard())){
			this.displayDestinations(true);
		}
	}
	
	public string canLaunchAdjacentOpponents()
	{
		string isLaunchable = "Aucun ennemi à proximité";
		
		List<Tile> neighbourTiles = this.getOpponentImmediateNeighbours(this.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()));
		int playerID;
		foreach (Tile t in neighbourTiles)
		{
			playerID = this.tiles [t.x, t.y].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getIsMine(playerID))
				{
					isLaunchable = "";
				}
			}
		}
		return isLaunchable;
	}
	
	public string canLaunchAdjacentAllys()
	{
		string isLaunchable = "Aucun allié à proximité";
		
		List<Tile> neighbourTiles = this.getAllyImmediateNeighbours(this.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()));
		int playerID;
		foreach (Tile t in neighbourTiles)
		{
			playerID = this.tiles [t.x, t.y].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && this.getIsMine(playerID))
				{
					isLaunchable = "";
				}
			}
		}
		return isLaunchable;
	}
	
	public string canLaunchAdjacentTileTargets()
	{
		string isLaunchable = "Aucun terrain ne peut etre ciblé";
		
		List<Tile> neighbourTiles = this.getFreeImmediateNeighbours(this.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()));
		this.targets = new List<Tile>();
		
		if(neighbourTiles.Count>0){
			isLaunchable = "";
		}
			
		return isLaunchable;
	}
	
	public string canLaunchAllButMeModifiersTargets()
	{
		string isLaunchable = "Aucun personnage ne peut etre ciblé";
		
		PlayingCardController pcc;
		this.targets = new List<Tile>();
		Tile tile ;
		
		for (int i = 0; i < this.playingCards.Count; i++)
		{
			pcc = this.getPCC(i);
			if (pcc.getCard().hasModifiers() && pcc.canBeTargeted() && i != GameController.instance.getCurrentPlayingCard())
			{
				isLaunchable = "";
			}
		}		
		return isLaunchable ;
	}
	
	public string canLaunchOpponentsTargets()
	{
		string isLaunchable = "Aucun ennemi ne peut etre atteint";
		
		PlayingCardController pcc;
		
		for (int i = 0; i < this.playingCards.Count; i++)
		{
			pcc = this.getPCC(i);
			if (!this.getIsMine(i) && pcc.canBeTargeted())
			{
				isLaunchable = "";
			}
		}
		return isLaunchable;
	}
	
	public string canLaunchAllysButMeTargets()
	{
		string isLaunchable = "Aucun ennemi ne peut etre atteint";
		
		PlayingCardController pcc;
		
		for (int i = 0; i < this.playingCards.Count; i++)
		{
			pcc = this.getPCC(i);
			if (this.getIsMine(i) && pcc.canBeTargeted() && i != GameController.instance.getCurrentPlayingCard())
			{
				isLaunchable = "";
			}
		}
		return isLaunchable;
	}
	
	public string canLaunchAnyone()
	{
		string isLaunchable = "Aucun ennemi ne peut etre atteint";
		
		PlayingCardController pcc;
		
		for (int i = 0; i < this.playingCards.Count; i++)
		{
			pcc = this.getPCC(i);
			if (pcc.canBeTargeted() && i != GameController.instance.getCurrentPlayingCard())
			{
				isLaunchable = "";
			}
		}
		return isLaunchable;
	}
	
	public void setModifier(Tile tile, int amount, ModifierType type, ModifierStat stat, int duration, int idIcon, string t, string d, string a, bool b){
		this.tiles [tile.x, tile.y].GetComponent<TileController>().getTile().setModifier(amount, type, stat, duration, idIcon, t, d, a, b);
		this.tiles [tile.x, tile.y].GetComponent<TileController>().changeTrapId(idIcon);
		this.tiles [tile.x, tile.y].GetComponent<TileController>().show();
	}
	
	public void show(int target, bool showTR){
		this.playingCards[target].GetComponent<PlayingCardController>().show(showTR);
	}
	
	public void kill(int target){
		
		this.playingCards[target].GetComponent<PlayingCardController>().kill();
		GameController.instance.killHandle (target);
		
		Tile t = this.getPlayingCardTile(target);
		this.emptyTile(t.x, t.y);
		
		this.tileHandlers[t.x, t.y].SetActive(true);
		this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().setText("");
		this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().changeType(3);
		this.displayedDeads.Add(target);
		this.displayedDeadsTimer.Add(1);
		
		if(this.getCard(target).isLeader()){
			bool isMine = this.getIsMine(target);
			for (int i = 0 ; i < this.playingCards.Count ; i++){
				if(i!=target && this.getIsMine(i)==isMine){
					this.getCard(i).removeLeaderEffect();
					this.show(i,true);
				}
			}
			
		}	
		
		this.toDisplayDeadHalos = true ;
	}
	
	public void emptyTile(int x, int y)
	{
		this.tiles [x, y].GetComponent<TileController>().setCharacterID(-1);
	}
	
	public void disappear(int target){
		this.playingCards[target].GetComponent<PlayingCardController>().disappear();		
	}
	
	public void displaySkillEffect(int target, string text, int type){
		Tile t = this.getPlayingCardTile(target);
		this.tileHandlers[t.x, t.y].SetActive(true);
		this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().enable();
		this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().changeType(type);
		this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().setText(text);
		
		this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().moveForward();
		this.displayedSE.Add(target);
		this.displayedSETimer.Add(2);
		
		this.toDisplaySE = true ;
	}
	
	public List<Tile> getFreeImmediateNeighbours(Tile t){
		List<Tile> freeNeighbours = new List<Tile>();
		List<Tile> neighbours = t.getImmediateNeighbourTiles();
		for (int i = 0 ; i < neighbours.Count ; i++){
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getTileType()==0 && this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()==-1){
				freeNeighbours.Add(neighbours[i]);
			}
		}
		return freeNeighbours ;
	}
	
	public List<Tile> getDestinationImmediateNeighbours(Tile t){
		bool b ; 
		List<Tile> freeNeighbours = new List<Tile>();
		List<Tile> neighbours = t.getImmediateNeighbourTiles();
		for (int i = 0 ; i < neighbours.Count ; i++){
			b = false ;
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()!=-1){
				if(this.getIsMine(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID())){
					b = true ;
				}
			}
			else{
				b = true ; 
			}
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getTileType()==0 && b){
				freeNeighbours.Add(neighbours[i]);
			}
		}
		return freeNeighbours ;
	}
	
	public List<Tile> getDestinationHisImmediateNeighbours(Tile t){
		bool b ; 
		List<Tile> freeNeighbours = new List<Tile>();
		List<Tile> neighbours = t.getImmediateNeighbourTiles();
		for (int i = 0 ; i < neighbours.Count ; i++){
			b = false ;
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()!=-1){
				if(!this.getIsMine(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID())){
					b = true ;
				}
			}
			else{
				b = true ; 
			}
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getTileType()==0 && b){
				freeNeighbours.Add(neighbours[i]);
			}
		}
		return freeNeighbours ;
	}
	
	public List<Tile> getOpponentImmediateNeighbours(Tile t){
		List<Tile> freeNeighbours = new List<Tile>();
		List<Tile> neighbours = t.getImmediateNeighbourTiles();
		for (int i = 0 ; i < neighbours.Count ; i++){
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()!=-1){
				if(!this.getIsMine(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID())){
					freeNeighbours.Add(neighbours[i]);
				}
			}
		}
		return freeNeighbours ;
	}
	
	public List<Tile> getCharacterImmediateNeighbours(Tile t){
		List<Tile> freeNeighbours = new List<Tile>();
		List<Tile> neighbours = t.getImmediateNeighbourTiles();
		for (int i = 0 ; i < neighbours.Count ; i++){
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()!=-1 && this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()!=GameController.instance.getCurrentPlayingCard()){
				freeNeighbours.Add(neighbours[i]);
			}
		}
		return freeNeighbours ;
	}
	
	public List<Tile> getAllyImmediateNeighbours(Tile t){
		List<Tile> freeNeighbours = new List<Tile>();
		List<Tile> neighbours = t.getImmediateNeighbourTiles();
		for (int i = 0 ; i < neighbours.Count ; i++){
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()!=-1){
				if(this.getIsMine(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID())){
					freeNeighbours.Add(neighbours[i]);
				}
			}
		}
		return freeNeighbours ;
	}
	
	public void removeClickedCard(int c){
		if(c!=-1){
			Tile t = this.getPlayingCardTile(c);
			this.tileHandlers[t.x, t.y].SetActive(false);
		}
	}
	
	public List<int> getAllys(){
		List<int> allys = new List<int>();
		int CPC = GameController.instance.getCurrentPlayingCard();
		for(int i = 0 ; i < this.playingCards.Count;i++){
			if(i!=CPC && !GameView.instance.isDead(i) && GameView.instance.getIsMine(i)){
				allys.Add(i);
			}
		}
		return allys;
	}
	
	public List<int> getOpponents(){
		List<int> allys = new List<int>();
		for(int i = 0 ; i < this.playingCards.Count;i++){
			if(!GameView.instance.isDead(i) && !GameView.instance.getIsMine(i)){
				allys.Add(i);
			}
		}
		return allys;
	}
	
	public List<int> getEveryone(){
		List<int> everyone = new List<int>();
		for(int i = 0 ; i < this.playingCards.Count;i++){
			if(!GameView.instance.isDead(i)){
				everyone.Add(i);
			}
		}
		return everyone;
	}
	
	public List<int> getEveryoneButMe(){
		List<int> everyone = new List<int>();
		for(int i = 0 ; i < this.playingCards.Count;i++){
			if(!GameView.instance.isDead(i) && GameController.instance.getCurrentPlayingCard()!=i){
				everyone.Add(i);
			}
		}
		return everyone;
	}
	
	public int countAlive(){
		int compteur = 0 ;
		for (int i = 0 ; i < this.playingCards.Count ; i++){
			if (!this.isDead(i)){
				compteur++;
			}
		}
		return compteur ;
	}
	
	public void checkPassiveSkills(bool mine){
		bool isFoundLeader = false ;
		for (int i = 0 ; i < this.playingCards.Count ; i++){
			if (this.getIsMine(i)==mine){
				if(this.getCard(i).isLeader() && !isFoundLeader){
					isFoundLeader = true ;
					int amount = this.getCard(i).getPassiveManacost();
					for (int j = 0 ; j < this.playingCards.Count ; j++){
						if(i!=j){
							if (this.getIsMine(j)==mine){
								int amountLife = Mathf.CeilToInt(amount*this.getCard(j).GetTotalLife()/100f);
								int amountAttack = Mathf.CeilToInt(amount*this.getCard(j).GetAttack()/100f);
								this.getCard(j).addModifier(amountLife, ModifierType.Type_BonusMalus, ModifierStat.Stat_Life, -1, 17, "LEADER ACTIF", "+"+amountLife+"PV. Tant que le leader est en vie.", "");
								this.getCard(j).addModifier(amountAttack, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, -1, 28, "LEADER ACTIF", "+"+amountAttack+"ATK. Tant que le leader est en vie.", "");
								this.show (j, false);
							}
						}
					}
				}
			}
		}
	}
	public void launchTuto(){
		this.tutorial = Instantiate(this.TutorialObject) as GameObject;
		this.tutorial.AddComponent<GameTutorialController>();
		StartCoroutine(this.tutorial.GetComponent<GameTutorialController>().launchSequence(0));
	}
	public bool getIsTutorialLaunched()
	{
		return isTutorialLaunched;
	}
	public void setIsTutorialLaunched(bool b)
	{
		this.isTutorialLaunched = b ;
	}
	public Vector3 getTilesPosition(int x, int y)
	{
		return this.tiles [x, y].transform.position;
	} 
	public Vector3 getPlayingCardsPosition(int index)
	{
		return this.playingCards [index].transform.position;
	}
	public Vector3 getPlayingCardsAttackZonePosition(int index)
	{
		return this.playingCards [index].transform.FindChild("AttackZone").position;
	}
	public Vector3 getPlayingCardsQuicknessZonePosition(int index)
	{
		return this.playingCards [index].transform.FindChild("WaitTime").position;
	}
	public Vector3 getPlayingCardsLifeZonePosition(int index)
	{
		return this.playingCards [index].transform.FindChild("LifeBar").FindChild("PVValue").position;
	}
	public Vector3 getMyHoveredRPCPosition()
	{
		return this.myHoveredRPC.transform.position;
	}
	public Vector3 getHisHoveredRPCPosition()
	{
		return this.hisHoveredRPC.transform.position;
	}
	public Vector3 getStartButtonPosition()
	{
		return GameObject.Find ("StartButton").transform.position;
	}
	public Vector3 getTimerGoPosition()
	{
		return this.timerGO.transform.position;
	}
	public Vector3 getSkillButtonPosition(int id)
	{
		return GameObject.Find ("ActionButtons").gameObject.transform.FindChild ("SkillButton"+id).transform.position;
	}
	public Vector3 getAttackButtonPosition()
	{
		return GameObject.Find ("ActionButtons").gameObject.transform.FindChild ("AttackButton").transform.position;
	}
	public Vector3 getPassButtonPosition()
	{
		return GameObject.Find ("ActionButtons").gameObject.transform.FindChild ("PassButton").transform.position;
	}
	
	public bool areAllMyPlayersDead(){
		bool areMyPlayersDead = true ;
		for (int i = 0 ; i < this.playingCards.Count ; i++){
			if (this.getIsMine(i)){
				if (!this.isDead(i)){
					areMyPlayersDead = false ;
				}
				else{
					print ("Dead "+i);
				}
			}
		}
		return areMyPlayersDead ;
	}
	
	public int attackClosestEnnemy(){
		bool[,] hasBeenPassages = new bool[this.boardWidth, this.boardHeight];
		bool hasFoundEnnemy = false ;
		int idEnnemyToAttack = -1 ;
		Tile idPlaceToMoveTo = new Tile(-1,-1) ;
		for(int l = 0 ; l < this.boardWidth ; l++){
			for(int k = 0 ; k < this.boardHeight ; k++){
				hasBeenPassages[l,k]=false;
			}
		}
		
		int move = this.getCard(GameController.instance.getCurrentPlayingCard()).GetMove();
		
		this.destinations = new List<Tile>();
		List<Tile> baseTiles = new List<Tile>();
		List<Tile> tempTiles = new List<Tile>();
		List<Tile> tempNeighbours = new List<Tile>();
		baseTiles.Add(this.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()));
		
		int j = 0 ;
		while (!hasFoundEnnemy && j<move){
			tempTiles = new List<Tile>();
			
			for(int k = 0 ; k < baseTiles.Count ; k++){
				tempNeighbours = this.getCharacterImmediateNeighbours(baseTiles[k]);
				if(tempNeighbours.Count>0){
					idEnnemyToAttack = GameView.instance.getTileCharacterID(tempNeighbours[0].x, tempNeighbours[0].y);
					idPlaceToMoveTo = baseTiles[k];
					hasFoundEnnemy = true ;
				}
				else{
					tempNeighbours = this.getDestinationImmediateNeighbours(baseTiles[k]);
					for(int l = 0 ; l < tempNeighbours.Count ; l++){
						if(!hasBeenPassages[tempNeighbours[l].x, tempNeighbours[l].y]){
							tempTiles.Add(tempNeighbours[l]);
							hasBeenPassages[tempNeighbours[l].x, tempNeighbours[l].y]=true;
						}
					}
				}
			}
			baseTiles = new List<Tile>();
			for(int l = 0 ; l < tempTiles.Count ; l++){
				baseTiles.Add(tempTiles[l]);
			}
			j++;
		}
		
		if(hasFoundEnnemy==false){
			GameController.instance.moveToDestination(baseTiles[0]);
		}
		else{
			GameController.instance.moveToDestination(idPlaceToMoveTo);
		}
		
		return idEnnemyToAttack;
	}
	
	public float getPercentageLifeMyPlayer(){
		float life = 0; 
		float totalLife = 0;
		for(int i = 0 ; i < this.playingCards.Count ; i++){
			if(this.getIsMine(i)){
				totalLife += this.getCard(i).GetTotalLife();
				if(!this.isDead(i)){
					life += this.getCard(i).GetLife();
				}
			}
		}
		return (100f*(life/totalLife));
	}
	
	public float getPercentageLifeHisPlayer(){
		float life = 0; 
		float totalLife = 0;
		for(int i = 0 ; i < this.playingCards.Count ; i++){
			if(!this.getIsMine(i)){
				totalLife += this.getCard(i).GetTotalLife();
				if(!this.isDead(i)){
					life += this.getCard(i).GetLife();
				}
			}
		}
		return (100f*(life/totalLife));
	}
	public void displayLoadingScreen()
	{
		if(!isLoadingScreenDisplayed)
		{
			this.loadingScreen=Instantiate(this.loadingScreenObject) as GameObject;
			this.isLoadingScreenDisplayed=true;
		}
	}
	public void hideLoadingScreen()
	{
		if(isLoadingScreenDisplayed)
		{
			Destroy (this.loadingScreen);
			this.isLoadingScreenDisplayed=false;
		}
	}
	
}


