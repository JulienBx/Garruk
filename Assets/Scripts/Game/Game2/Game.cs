using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
	public static Game instance;
	public Board board ;
	public Gamecards gamecards ;

	public GameObject verticalBorderModel;
	public GameObject horizontalBorderModel;
	public GameObject tileModel;
	public GameObject cardModel;

	public Sprite[] skillSprites;
	public Sprite[] bigCharacterSprites;
	public Sprite[] animSprites;

	GameObject myHoveredCard ;
	GameObject hisHoveredCard ;
	GameObject startButton ;
	GameObject interlude ;
	GameObject passButton ;
	GameObject[] skillsButton ;
	GameObject popUpChoice ;
	GameObject timeline ;
	GameObject timer ;
	GameObject frontTimer ;
	GameObject popUpValidation ;
	GameObject forfeitButton ;
	GameObject myPlayerName ;
	GameObject hisPlayerName ;
	GameObject cancelButton ;

	bool ignoreNextMoveOn;

	SkillsM skills ;

	bool mobile ;
	bool firstPlayer ;
	bool ia;

	Intelligence intelligence ;
	int currentCardID ;
	int draggingCardID ;
	int draggingSBID ;
	float tileScale;
	int nbPlayersReadyToStart;

	List<int> cardsToPlay;

	int lastToPlay;
	int indexPlayingCard;
	int indexMeteores;

	string URLStat = ApplicationModel.host + "updateResult.php";
	bool SE ;
	bool toLaunchNextTurn;

	private string URLInitiliazeGame = ApplicationModel.host + "initialize_game.php";

	void Awake(){
		instance = this;
		this.currentCardID = -1;
		this.draggingCardID = -1;
		this.draggingSBID = -1;
		this.nbPlayersReadyToStart = 0;
		this.indexPlayingCard = -1;
		this.lastToPlay = -10;
		this.indexMeteores = 1;
		this.ignoreNextMoveOn = false ;
		this.skills = new SkillsM();
		this.firstPlayer = false ;
		this.cardsToPlay = new List<int>();
		this.ia = false ;
		GameObject.Find("PhotonController").GetComponent<PhotonController>().isOk = false ;

		this.board = new Board();
		this.gamecards = new Gamecards();
		SoundController.instance.playMusic(new int[]{4,5,6});

		this.myHoveredCard = GameObject.Find("MyHoveredPlayingCard");
		this.hisHoveredCard = GameObject.Find("HisHoveredPlayingCard");
		this.startButton = GameObject.Find("SB");
		this.interlude = GameObject.Find("Interlude");
		this.passButton = GameObject.Find("PassButton");
		this.skillsButton = new GameObject[4];
		this.skillsButton[0] = GameObject.Find("SkillButton1");
		this.skillsButton[1] = GameObject.Find("SkillButton2");
		this.skillsButton[2] = GameObject.Find("SkillButton3");
		this.skillsButton[3] = GameObject.Find("SkillButton4");
		this.popUpChoice = GameObject.Find("PopUpChoice");
		this.timeline = GameObject.Find("Timeline");
		this.timer = GameObject.Find("Timer");
		this.frontTimer = GameObject.Find("FrontTimer");
		this.popUpValidation = GameObject.Find("PopUpValidation");
		this.forfeitButton = GameObject.Find("ForfeitButton");
		this.myPlayerName = GameObject.Find("MyPlayerName");
		this.hisPlayerName = GameObject.Find("HisPlayerName");
		this.cancelButton = GameObject.Find("CancelButton");

		PhotonC.instance.findRoom();
		GameObject.Find("Logo").GetComponent<SpriteRenderer>().enabled = false;
	}

	void Update(){
		this.SE = false ;
		if(PhotonC.instance.isWaiting){
			PhotonC.instance.addTime(Time.deltaTime);
		}

		if(GameRPC.instance.isFailing()){
			if(!PhotonC.instance.isReconnecting()){
				GameRPC.instance.addTime(Time.deltaTime);
			}
		}

		if(this.getInterlude().isDisplaying()){
			this.getInterlude().addTime(Time.deltaTime);
		}

		if(this.getMyHoveredCard().isClignoting()){
			this.getMyHoveredCard().addTimeClignoting(Time.deltaTime);
		}

		if(this.getMyHoveredCard().isMoving()){
			this.getMyHoveredCard().addTime(Time.deltaTime);
		}

		if(this.getMyHoveredCard().isMoving()){
			this.getMyHoveredCard().addTime(Time.deltaTime);
		}

		if(this.getMyHoveredCard().isMoving()){
			this.getMyHoveredCard().addTime(Time.deltaTime);
		}

		if(this.getHisHoveredCard().isMoving()){
			this.getHisHoveredCard().addTime(Time.deltaTime);
		}

		if(this.getHisHoveredCard().isClignoting()){
			this.getHisHoveredCard().addTimeClignoting(Time.deltaTime);
		}

		if(this.gamecards.isLoaded()){
			for (int i = 0 ; i < 8 ; i++){
				if(this.gamecards.getCardC(i).isClignoting()){
					this.gamecards.getCardC(i).addClignoteTime(Time.deltaTime);
				}
				if(this.gamecards.getCardC(i).isMoving()){
					this.gamecards.getCardC(i).addMoveTime(Time.deltaTime);
				}
				if(this.gamecards.getCardC(i).isMeasuringPush()){
					this.gamecards.getCardC(i).addPushTime(Time.deltaTime);
				}
				if(this.gamecards.getCardC(i).isUpgradingAttack()){
					this.gamecards.getCardC(i).addTimeAttack(Time.deltaTime);
				}
				if(this.gamecards.getCardC(i).isUpgradingLife()){
					this.gamecards.getCardC(i).addTimeLife(Time.deltaTime);
				}
				if(this.gamecards.getCardC(i).isSkillEffect()){
					this.gamecards.getCardC(i).addSETime(Time.deltaTime);
					SE = true ;
				}
			}
		}

		if(this.getBoard().isLoaded()){
			for (int i = 0 ; i < this.getBoard().getBoardWidth() ; i++){
				for (int j = 0 ; j < this.getBoard().getBoardHeight() ; j++){
					if(this.getBoard().getTileC(i,j).isSkillEffect()){
						this.getBoard().getTileC(i,j).addSETime(Time.deltaTime);
						SE = true ;
					}
					if(this.getBoard().getTileC(i,j).isTarget()){
						this.getBoard().getTileC(i,j).addTargetTime(Time.deltaTime);
					}
				}
			}
		}

		if(this.draggingCardID!=-1){
			Vector3 mousePos = Input.mousePosition;
			this.getCards().getCardC(draggingCardID).setPosition(Camera.main.ScreenToWorldPoint(mousePos));
		}

		if(this.draggingSBID!=-1){
			Vector3 mousePos = Input.mousePosition;
			this.getSkillButton(this.draggingSBID).setPosition(Camera.main.ScreenToWorldPoint(mousePos));
		}

		if(this.ia){
			if(this.intelligence.isStarting()){
				this.intelligence.addStartTime(Time.deltaTime);
			}
		}

		if(this.getTimer().isVisible()){
			this.getTimer().addTime(Time.deltaTime);
		}

		if(this.getTimer().isVisibleFront()){
			this.getTimer().addFrontTime(Time.deltaTime);
		}

		for (int i = 0 ; i < 4 ; i++){
			if(this.getSkillButton(i).isMoving()){
				this.getSkillButton(i).addMoveTime(Time.deltaTime);
			}
		}

		if(!SE){
			//print("Plus de Skill");
			if(this.toLaunchNextTurn){
				this.toLaunchNextTurn=false;
				this.launchNextTurn();
			}
			else{
				this.getPassButton().white();
			}
		}
		else{
			this.getPassButton().grey();
		}
	}

	public int getCurrentCardID(){
		return this.currentCardID;
	}

	public float getTileScale(){
		return this.tileScale;
	}

	public CardC getCurrentCard(){
		return this.gamecards.getCardC(this.currentCardID);
	}

	public bool getIgnoreNextMoveOn(){
		return this.ignoreNextMoveOn;
	}

	public void setIgnoreNextMoveOn(bool b){
		this.ignoreNextMoveOn = b;
	}

	public Sprite getSkillSprite(int i){
		return this.skillSprites[i];
	}

	public Sprite getAnimSprite(int i){
		return this.animSprites[i];
	}

	public Sprite getBigCharacterSprite(int i){
		return this.bigCharacterSprites[i];
	}

	public void setMyName(string s){
		this.myPlayerName.GetComponent<TextMeshPro>().text = s;
	}

	public Board getBoard(){
		return this.board ;
	}

	public void setHisName(string s){
		this.hisPlayerName.GetComponent<TextMeshPro>().text = s;
	}

	public void createBackground(){
		print("createBack "+this.getBoard().getBoardWidth());
		for (int i = 0; i < this.board.getBoardWidth()+1; i++)
		{
			this.board.addVerticalBorder(i, (GameObject)Instantiate(verticalBorderModel));
		}
		for (int i = 0; i < this.board.getBoardHeight()+1; i++)
		{
			this.board.addHorizontalBorder(i, (GameObject)Instantiate(horizontalBorderModel));
		}

		if(this.isFirstPlayer()){
			this.createTiles();
		}
	}

	public void createCards(){
		print("CardsCreation");
		int characterID;
		int power ;

		if(this.ia){
			this.gamecards.GenerateIADeck();
		}
			
		int card0Index = ApplicationModel.player.MyCards.getCardIndex(ApplicationModel.player.MyDecks.getDeck (ApplicationModel.player.SelectedDeckIndex).cards [0].Id);
		int card1Index = ApplicationModel.player.MyCards.getCardIndex(ApplicationModel.player.MyDecks.getDeck (ApplicationModel.player.SelectedDeckIndex).cards [1].Id);
		int card2Index = ApplicationModel.player.MyCards.getCardIndex(ApplicationModel.player.MyDecks.getDeck (ApplicationModel.player.SelectedDeckIndex).cards [2].Id);
		int card3Index = ApplicationModel.player.MyCards.getCardIndex(ApplicationModel.player.MyDecks.getDeck (ApplicationModel.player.SelectedDeckIndex).cards [3].Id);

		int card0DeckOrder = ApplicationModel.player.MyDecks.getDeck (ApplicationModel.player.SelectedDeckIndex).cards [0].deckOrder;
		int card1DeckOrder = ApplicationModel.player.MyDecks.getDeck (ApplicationModel.player.SelectedDeckIndex).cards [1].deckOrder;
		int card2DeckOrder = ApplicationModel.player.MyDecks.getDeck (ApplicationModel.player.SelectedDeckIndex).cards [2].deckOrder;
		int card3DeckOrder = ApplicationModel.player.MyDecks.getDeck (ApplicationModel.player.SelectedDeckIndex).cards [3].deckOrder;

		if (this.isFirstPlayer()){
			this.gamecards.createPlayingCard(card0DeckOrder, ApplicationModel.player.MyCards.getCardM(card0Index), true, (GameObject)Instantiate(cardModel),card0DeckOrder);
			this.gamecards.createPlayingCard(card1DeckOrder, ApplicationModel.player.MyCards.getCardM(card1Index), true, (GameObject)Instantiate(cardModel),card1DeckOrder);
			this.gamecards.createPlayingCard(card2DeckOrder, ApplicationModel.player.MyCards.getCardM(card2Index), true, (GameObject)Instantiate(cardModel),card2DeckOrder);
			this.gamecards.createPlayingCard(card3DeckOrder, ApplicationModel.player.MyCards.getCardM(card3Index), true, (GameObject)Instantiate(cardModel),card3DeckOrder);
			this.gamecards.createPlayingCard(ApplicationModel.opponentDeck.getCardM(0).getDeckOrder()+4, ApplicationModel.opponentDeck.getCardM(0), false, (GameObject)Instantiate(cardModel),ApplicationModel.opponentDeck.getCardM(0).getDeckOrder());
			this.gamecards.createPlayingCard(ApplicationModel.opponentDeck.getCardM(1).getDeckOrder()+4, ApplicationModel.opponentDeck.getCardM(1), false, (GameObject)Instantiate(cardModel),ApplicationModel.opponentDeck.getCardM(1).getDeckOrder());
			this.gamecards.createPlayingCard(ApplicationModel.opponentDeck.getCardM(2).getDeckOrder()+4, ApplicationModel.opponentDeck.getCardM(2), false, (GameObject)Instantiate(cardModel),ApplicationModel.opponentDeck.getCardM(2).getDeckOrder());
			this.gamecards.createPlayingCard(ApplicationModel.opponentDeck.getCardM(3).getDeckOrder()+4, ApplicationModel.opponentDeck.getCardM(3), false, (GameObject)Instantiate(cardModel),ApplicationModel.opponentDeck.getCardM(3).getDeckOrder());
		}
		else{
			this.gamecards.createPlayingCard(card0DeckOrder+4, ApplicationModel.player.MyCards.getCardM(card0Index), true, (GameObject)Instantiate(cardModel),card0DeckOrder);
			this.gamecards.createPlayingCard(card1DeckOrder+4, ApplicationModel.player.MyCards.getCardM(card1Index), true, (GameObject)Instantiate(cardModel),card1DeckOrder);
			this.gamecards.createPlayingCard(card2DeckOrder+4, ApplicationModel.player.MyCards.getCardM(card2Index), true, (GameObject)Instantiate(cardModel),card2DeckOrder);
			this.gamecards.createPlayingCard(card3DeckOrder+4, ApplicationModel.player.MyCards.getCardM(card3Index), true, (GameObject)Instantiate(cardModel),card3DeckOrder);
			this.gamecards.createPlayingCard(ApplicationModel.opponentDeck.getCardM(0).getDeckOrder(), ApplicationModel.opponentDeck.getCardM(0), false, (GameObject)Instantiate(cardModel),ApplicationModel.opponentDeck.getCardM(0).getDeckOrder());
			this.gamecards.createPlayingCard(ApplicationModel.opponentDeck.getCardM(1).getDeckOrder(), ApplicationModel.opponentDeck.getCardM(1), false, (GameObject)Instantiate(cardModel),ApplicationModel.opponentDeck.getCardM(1).getDeckOrder());
			this.gamecards.createPlayingCard(ApplicationModel.opponentDeck.getCardM(2).getDeckOrder(), ApplicationModel.opponentDeck.getCardM(2), false, (GameObject)Instantiate(cardModel),ApplicationModel.opponentDeck.getCardM(2).getDeckOrder());
			this.gamecards.createPlayingCard(ApplicationModel.opponentDeck.getCardM(3).getDeckOrder(), ApplicationModel.opponentDeck.getCardM(3), false, (GameObject)Instantiate(cardModel),ApplicationModel.opponentDeck.getCardM(3).getDeckOrder());
		}

		if(this.ia || this.isTutorial()){
			this.intelligence.placeCards();
		}

		for(int i = 0 ; i < this.getCards().getNumberOfCards() ; i++){
			if(this.getCards().getCardC(i).getCardM().getCharacterType()==70){
				int bonusBouclier = this.getCards().getCardC(i).getCardM().getSkill(0).Power*4;
				this.getCards().getCardC(i).addShieldModifyer(new ModifyerM(bonusBouclier, 1, "", "",-1));
			}
			else if(this.getCards().getCardC(i).getCardM().getCharacterType()==35){
				this.getCards().getCardC(i).addStateModifyer(new ModifyerM(1,9,WordingGame.getText(126, new List<int>{(50-5*this.getCards().getCardC(i).getCardM().getSkill(0).Power)}),WordingSkills.getName(34),-1));
			}
			else if(this.getCards().getCardC(i).getCardM().getCharacterType()==32){
				this.getCards().getCardC(i).addShieldModifyer(new ModifyerM(this.getCards().getCardC(i).getCardM().getSkill(0).Power*8+20,12,WordingGame.getText(87, new List<int>{(this.getCards().getCardC(i).getCardM().getSkill(0).Power*8+20)})+". "+WordingGame.getText(100),WordingSkills.getName(32),1));
			}
			else if(this.getCards().getCardC(i).getCardM().getCharacterType()==35){
				this.getCards().getCardC(i).addMoveModifyer(new ModifyerM(-2,8,"","",-1));
				this.getCards().getCardC(i).addStateModifyer(new ModifyerM(20+this.getCards().getCardC(i).getCardM().getSkill(0).Power,8,WordingGame.getText(123, new List<int>{20+this.getCards().getCardC(i).getCardM().getSkill(0).Power}),WordingSkills.getName(35),-1));
			}
			else if(this.getCards().getCardC(i).getCardM().getCharacterType()==73){
				List<TileM> neighbours = Game.instance.getBoard().getTileNeighbours(this.getCards().getCardC(i).getTileM());
				for (int j = 0 ; j < neighbours.Count ;j++){
					characterID = Game.instance.getBoard().getTileC(neighbours[j]).getCharacterID();
					if(characterID!=-1){
						if(this.getCards().getCardC(i).getCardM().isMine()!=this.getCards().getCardC(characterID).getCardM().isMine()){
							Game.instance.getCards().getCardC(characterID).addAttackModifyer(new ModifyerM(-2-Game.instance.getCards().getCardC(i).getCardM().getSkill(0).Power,3,WordingGame.getText(83, new List<int>{2+Game.instance.getCards().getCardC(i).getCardM().getSkill(0).Power}),"Paladin",-1));
						}
					}
				}
			}
			else if(this.getCards().getCardC(i).getCardM().getCharacterType()==111){
				List<TileM> neighbours = Game.instance.getBoard().getTileNeighbours(this.getCards().getCardC(i).getTileM());
				for (int j = 0 ; j < neighbours.Count ;j++){
					characterID = Game.instance.getBoard().getTileC(neighbours[j]).getCharacterID();
					if(characterID!=-1){
						if(this.getCards().getCardC(i).getCardM().isMine()||this.currentCardID!=-1){
							Game.instance.getCards().getCardC(characterID).addShieldModifyer(new ModifyerM(4*Game.instance.getCards().getCardC(i).getCardM().getSkill(0).Power,16, "",WordingSkills.getName(111),-1));
						}
					}
				}
			}
			else if(this.getCards().getCardC(i).getCardM().getCharacterType()==76){
				Game.instance.getCards().getCardC(i).addDamageModifyer(new ModifyerM(Mathf.RoundToInt(Game.instance.getCards().getCardC(i).getLife()/2f), -1, "", "",-1));
				power = this.getCards().getCardC(i).getCardM().getSkill(0).Power;
				for (int j = 0 ; j < this.getCards().getNumberOfCards() ;j++){
					if(i!=j){
						if(this.getCards().getCardC(j).getCardM().isMine()==this.getCards().getCardC(i).getCardM().isMine()){
							Game.instance.getCards().getCardC(j).addAttackModifyer(new ModifyerM(power,4, "","Leader",-1));
							Game.instance.getCards().getCardC(j).addLifeModifyer(new ModifyerM(2+power,4, "","Leader",-1));
						}
					}
				}
			}
			else if(this.getCards().getCardC(i).getCardM().getCharacterType()==64){
				if(Game.instance.isFirstPlayer()){
					List<TileM> tiles = this.getBoard().get4RandomEmptyCenterTile();
					for(int j = 0 ; j < tiles.Count ; j++){
						Game.instance.getBoard().getTileC(tiles[j].x, tiles[j].y).setTrap(1,this.getCards().getCardC(i).getCardM().getSkill(0).Power, this.getCards().getCardC(i).getCardM().isMine());
						Game.instance.launchCorou("SetTrapRPC", tiles[j].x, tiles[j].y, 1, this.getCards().getCardC(i).getCardM().getSkill(0).Power, this.getCards().getCardC(i).getCardM().isMine());
					}
				}
			}
			else if(this.getCards().getCardC(i).getCardM().getCharacterType()==66){
				Game.instance.getCards().getCardC(i).addEsquiveModifyer(new ModifyerM(10+4*Game.instance.getCards().getCardC(i).getCardM().getSkill(0).Power,7,"","",-1));
			}
		}

		this.resize();
	}

	void createTiles()
	{
		print("CREATETILES");
		bool isRock = false;
		List<TileM> rocks = new List<TileM>();
		TileM t = new TileM(0,0) ;

		if(ApplicationModel.player.ToLaunchGameTutorial){
			rocks.Add(new TileM(2,2));
			rocks.Add(new TileM(0,2));
			rocks.Add(new TileM(5,4));
		}
		else{
			int nbRocksToAdd = UnityEngine.Random.Range(3, 6);
			int compteurRocks = 0;
			bool isOk = true;
			while (compteurRocks<nbRocksToAdd){
				isOk = false;
				while (!isOk)
				{
					t = new TileM();
					t.randomize(this.board.getBoardWidth(), this.board.getBoardHeight());
					isOk = true;
					for (int a = 0; a < rocks.Count && isOk; a++){
						if (rocks [a].x == t.x && rocks [a].y == t.y)
						{
							isOk = false;
						}
					}
				}
				rocks.Add(t);
				compteurRocks++;
			}
		}
		
		for (int x = 0; x < this.board.getBoardWidth(); x++){
			for (int y = 0; y < this.board.getBoardHeight(); y++){
				isRock = false;
				for (int z = 0; z < rocks.Count && !isRock; z++){
					if (rocks [z].x == x && rocks [z].y == y){
						isRock = true;
					}
				}
				if(this.ia || this.isTutorial()){
					this.createTile(x, y, isRock);
				}
				else{
					StartCoroutine(GameRPC.instance.launchRPC("createTileRPC", x, y, isRock));
				}
			}
		}
	}

	public CancelButtonC getCancelButton(){
		return this.cancelButton.GetComponent<CancelButtonC>();
	}

	public StartButtonC getStartButton(){
		return this.startButton.GetComponent<StartButtonC>();
	}

	public TimerC getTimer(){
		return this.timer.GetComponent<TimerC>();
	}

	public ForfeitC getForfeitButton(){
		return this.forfeitButton.GetComponent<ForfeitC>();
	}

	public LeftSlider getMyHoveredCard(){
		return this.myHoveredCard.GetComponent<LeftSlider>();
	}

	public RightSlider getHisHoveredCard(){
		return this.hisHoveredCard.GetComponent<RightSlider>();
	}

	public PassButtonC getPassButton(){
		return this.passButton.GetComponent<PassButtonC>();
	}

	public SkillButtonC getSkillButton(int i){
		return this.skillsButton[i].GetComponent<SkillButtonC>();
	}

	public TimelineC getTimeline(){
		return this.timeline.GetComponent<TimelineC>();
	}

	public InterludeC getInterlude(){
		return this.interlude.GetComponent<InterludeC>();
	}

	public Gamecards getCards(){
		return this.gamecards;
	}

	public SkillsM getSkills(){
		return this.skills;
	}

	public bool isFirstPlayer(){
		return this.firstPlayer;
	}

	public bool isIA(){
		return this.ia;
	}

	public void setFirstPlayer(bool b){
		this.firstPlayer = b;
	}

	public void setIA(bool b){
		if(b){
			this.intelligence = new Intelligence();
		}
		this.ia = b;
	}

	public bool isTutorial(){
		return ApplicationModel.player.ToLaunchGameTutorial;
	}

	public void resize(){
		print("Resizing");

		int width = Screen.width ;
		int height = Screen.height ;

		float realwidth = 10f*width/height;

		this.getMyHoveredCard().putToStartPosition(realwidth);
		this.getHisHoveredCard().putToStartPosition(realwidth);
		this.mobile = (width<height);

		this.getMyHoveredCard().activateCollider(this.mobile);
		this.getHisHoveredCard().activateCollider(this.mobile);
		float TileScale = Mathf.Min (realwidth/6.05f, 8f/this.board.getBoardHeight());
		this.tileScale = TileScale ;

		for (int i = 0; i < this.board.getBoardHeight()+1; i++){
			this.board.sizeHorizontalBorder(i, new Vector3(0,(-4*tileScale)+tileScale*i,-1f), new Vector3(1f,0.5f,1f));
		}
		
		for (int i = 0; i < this.board.getBoardWidth()+1; i++){
			this.board.sizeVerticalBorder(i, new Vector3((-this.board.getBoardWidth()/2f+i)*tileScale, 0f, -1f), new Vector3(0.5f,tileScale,1f));
		}

		if(this.mobile){
			this.getForfeitButton().size(new Vector3(-realwidth/2f+0.25f, 4.75f, 0f), new Vector3(0.4f, 0.4f, 0.4f));
			GameObject.Find("MyPlayerName").GetComponent<MeshRenderer>().enabled = false;
			GameObject.Find("HisPlayerName").GetComponent<MeshRenderer>().enabled = false;
		}
		else{
			this.getForfeitButton().size(new Vector3(-realwidth/2f+0.5f, 4.5f, 0f), new Vector3(0.8f, 0.8f, 0.8f));
			GameObject.Find("MyPlayerName").transform.localPosition = new Vector3(-realwidth/2f+1f, 4.5f, 0f);
			GameObject.Find("HisPlayerName").transform.localPosition = new Vector3(realwidth/2f, 4.5f, 0f);
		}

		this.getTimer().size(new Vector3(0f, 4*tileScale+0.1f, 0f));

		GameObject.Find("Logo").transform.position = new Vector3(0f, 2.5f+2*tileScale, 0f);
		this.getStartButton().size(new Vector3(0f, -2.5f-2*tileScale, 0f));

		this.getPassButton().size(new Vector3(Mathf.Min(2.2f, 0.5f*realwidth-0.8f), -2.4f-2*tileScale, 0f));
		this.getSkillButton(0).size(new Vector3(-2.5f*tileScale, -2.4f-2*tileScale, 0f));
		this.getSkillButton(1).size(new Vector3(-1.5f*tileScale, -2.4f-2*tileScale, 0f));
		this.getSkillButton(2).size(new Vector3(-0.5f*tileScale, -2.4f-2*tileScale, 0f));
		this.getSkillButton(3).size(new Vector3(0.5f*tileScale, -2.4f-2*tileScale, 0f));

		this.getCancelButton().size(new Vector3(0f, -2.4f-2*tileScale, 0f));
		this.getInterlude().size(realwidth);

		for(int i = 0 ; i < this.board.getBoardWidth() ; i++){
			for(int j = 0 ; j < this.board.getBoardHeight() ; j++){
				this.board.getTileC(i,j).size(tileScale);
			}
		}

		for(int i = 0 ; i < this.board.getBoardWidth() ; i++){
			for(int j = 0 ; j < this.board.getBoardHeight() ; j++){
				this.board.getTileC(i,j).size(tileScale);
			}
		}

		for(int i = 0 ; i < this.gamecards.getNumberOfCards() ; i++){
			this.gamecards.getCardC(i).resize(tileScale);
		}

		PhotonC.instance.hideLoadingScreen();
		this.launchPreGame();
	}

	public void launchPreGame(){
		if(this.ia){
			this.intelligence.launch();
		}
		this.setInitialDestinations();
		this.getTimer().setTimer(30);
		this.getStartButton().show(true);
		this.getForfeitButton().show(true);
	}

	public void createTile(int x, int y, bool type)
	{
		this.board.addTile(x,y,type,(GameObject)Instantiate(this.tileModel));
		if(x==this.getBoard().getBoardWidth()-1 && y==this.getBoard().getBoardHeight()-1){
			if(this.ia || this.isTutorial()){
				StartCoroutine(this.initializeGame(this.isFirstPlayer(), true, ApplicationModel.player.Id, ApplicationModel.player.RankingPoints, ApplicationModel.player.SelectedDeckId));
			}
			else{
				print("J'initialise");
				StartCoroutine(GameRPC.instance.launchRPC("sendMyDeckIDRPC", ApplicationModel.player.SelectedDeckId, ApplicationModel.player.Id, ApplicationModel.player.RankingPoints));
			}
		}
	}

	public void setInitialDestinations(){
		if(this.firstPlayer){
			for(int i = 0 ; i < this.board.getBoardWidth() ; i++){
				for(int j = 0 ; j < 2 ; j++){
					if(this.board.getTileC(i,j).isEmpty()){
						this.board.getTileC(i,j).displayDestination(0);
					}
					else{
						this.board.getTileC(i,j).showDestination(false);
					}
				}
			}
		}
		else{
			for(int i = 0 ; i < this.board.getBoardWidth() ; i++){
				for(int j = 6 ; j < 8 ; j++){
					if(this.board.getTileC(i,j).isEmpty()){
						this.board.getTileC(i,j).displayDestination(0);
					}
					else{
						this.board.getTileC(i,j).showDestination(false);
					}
				}
			}
		}
	}

	public void hoverMyCard(int i){
		this.getMyHoveredCard().setNextCard(i);
		if(this.currentCardID!=-1){
			this.displayDestinations(i);
		}
	}

	public void hoverHisCard(int i){
		this.getHisHoveredCard().setNextCard(i);
		if(this.currentCardID!=-1){
			this.displayDestinations(i);
		}
	}

	public void hoverTile(){
		if(this.currentCardID==-1){
			this.getMyHoveredCard().setNextCard(-1);
			this.getHisHoveredCard().setNextCard(-1);
		}
		else{
			this.displayDestinations(this.currentCardID);
			if(this.getCurrentCard().getCardM().isMine()){
				this.getMyHoveredCard().setNextCard(this.currentCardID);
				this.getHisHoveredCard().setNextCard(-1);
			}
			else{
				this.getHisHoveredCard().setNextCard(this.currentCardID);
				this.getMyHoveredCard().setNextCard(-1);
			}
		}
	}

	public bool isMobile(){
		return this.mobile;
	}

	public void failToReconnect(){
		print("Pas réussi à me reconnecter...");
		SceneManager.LoadScene("Home");
	}

	public void hitCard(int i){
		if(this.currentCardID==-1){
			this.startDragging(i);
		}
		else if(this.getCurrentCard().getCardM().isMine() && i==this.currentCardID && this.getCurrentCard().canMove()){
			this.startDragging(i);
		}
	}

	public void startDragging(int i){
		this.draggingCardID = i ;
		this.gamecards.getCardC(i).moveForward();
		this.gamecards.getCardC(i).showCollider(false);
		this.gamecards.getCardC(i).showHover(false);
		this.gamecards.getCardC(i).displayBackTile(false);
		this.getBoard().getTileC(this.getCards().getCardC(i).getTileM()).free();
	}

	public int getDraggingCardID(){
		return this.draggingCardID;
	}

	public int getDraggingSBID(){
		return this.draggingSBID;
	}

	public void dropOnTile(int x, int y){
		if(this.getBoard().getTileC(x,y).getDestination()==0){
			if(currentCardID!=-1){
				if(this.draggingCardID==this.getCurrentCardID()){
					this.getCurrentCard().move(true);
				}
				this.displayDestinations(this.draggingCardID);
			}

			if(this.ia || this.isTutorial()){
				this.moveOn(x,y,draggingCardID);
			}
			else{
				if(this.draggingCardID==this.currentCardID){
					this.moveOn(x,y,draggingCardID);
					this.setIgnoreNextMoveOn(true);
				}

				StartCoroutine(GameRPC.instance.launchRPC("moveOnRPC",x,y,draggingCardID));
			}
		}
		else{
			if(this.getBoard().getTileC(x,y).getCharacterID()==-1){
				this.getBoard().getTileC(x,y).displaySkillEffect(WordingGame.getText(68),2);
			}
			else{
				if(this.draggingCardID==this.getBoard().getTileC(x,y).getCharacterID()){
					//this.getBoard().getTileC(x,y).displaySkillEffect(WordingGame.getText(71),2);
				}
				else{
					this.getBoard().getTileC(x,y).displaySkillEffect(WordingGame.getText(69),2);
				}
			}
			this.dropOutsideBoard();
		}
		this.draggingCardID = -1;
	}

	public void dropSBOnTile(int x, int y){
		if(this.getBoard().getTileC(x,y).isTarget()){
			this.getCurrentCard().play(true);
			this.getSkillButton(draggingSBID).getSkillC().resolve(x,y,this.getSkillButton(draggingSBID).getSkill());
		}
		else{
			if(this.getBoard().getTileC(x,y).getCharacterID()==-1){
				this.getBoard().getTileC(x,y).displaySkillEffect(WordingGame.getText(73),2);
			}
			else{
				this.getBoard().getTileC(x,y).displaySkillEffect(WordingGame.getText(74),2);
			}
		}
		this.dropSBOutsideBoard();
	}

	public void moveOn(int x, int y, int i){
		if(this.currentCardID==-1){
			if(Game.instance.isIA() || Game.instance.isTutorial()){
				if(Game.instance.getCards().getCardC(i).getCardM().isMine()){
					this.gamecards.getCardC(i).startMove(this.gamecards.getCardC(i).getPosition(), this.getBoard().getTileC(x,y).getPosition());
				}
			}
			else{
				this.gamecards.getCardC(i).startMove(this.gamecards.getCardC(i).getPosition(), this.getBoard().getTileC(x,y).getPosition());
			}
		}
		else{
			this.gamecards.getCardC(i).startMove(this.gamecards.getCardC(i).getPosition(), this.getBoard().getTileC(x,y).getPosition());
		}

		this.updateBoard(x,y,i);
	}

	public void updateBoard(int x, int y, int i){
		int characterID ;
		int found ;
		int foundProtecteur;
		if(this.currentCardID!=-1){
			if(i==this.getCurrentCardID()){
				this.getCurrentCard().move(true);
			}
		}

		this.getBoard().getTileC(this.gamecards.getCardC(i).getTileM()).setCharacterID(-1);
		this.getBoard().getTileC(this.gamecards.getCardC(i).getTileM()).showCollider(true);

		if(this.currentCardID!=-1){
			if(Game.instance.getCards().getCardC(i).getCardM().getCharacterType()==140){
				if(Game.instance.getCards().getCardC(i).getCardM().isMine()){
					if(UnityEngine.Random.Range(1,101)<Game.instance.getCards().getCardC(i).getCardM().getSkill(0).Power*5+30){
						if(this.ia || this.isTutorial()){
							this.getBoard().createRock(this.gamecards.getCardC(i).getTileM().x, this.gamecards.getCardC(i).getTileM().y);
						}
						else{
							Game.instance.launchCorou("createRockRPC", this.gamecards.getCardC(i).getTileM().x, this.gamecards.getCardC(i).getTileM().y);
						}
					}
				}
			}
		}

		if(Game.instance.getCards().getCardC(i).getCardM().getCharacterType()==73){
			List<TileM> neighbours = Game.instance.getBoard().getTileNeighbours(this.gamecards.getCardC(i).getTileM());
			for (int j = 0 ; j < neighbours.Count ;j++){
				characterID = Game.instance.getBoard().getTileC(neighbours[j]).getCharacterID();
				if(characterID!=-1){
					if(this.getCards().getCardC(i).getCardM().isMine()!=this.getCards().getCardC(characterID).getCardM().isMine()){
						Game.instance.getCards().getCardC(characterID).removePaladinModifyer();
						Game.instance.getCards().getCardC(characterID).displaySkillEffect(WordingGame.getText(105),0);
					}
				}
			}
		}
		else if(Game.instance.getCards().getCardC(i).getCardM().getCharacterType()==111){
			//Debug.Log("Je suis protecteur");
			List<TileM> neighbours = Game.instance.getBoard().getTileNeighbours(this.gamecards.getCardC(i).getTileM());
			for (int j = 0 ; j < neighbours.Count ;j++){
				characterID = Game.instance.getBoard().getTileC(neighbours[j]).getCharacterID();
				if(characterID!=-1){
					Game.instance.getCards().getCardC(characterID).removeProtecteurModifyer();
					if(this.getCards().getCardC(characterID).getCardM().isMine()||this.currentCardID!=-1){
						Game.instance.getCards().getCardC(characterID).displaySkillEffect(WordingGame.getText(131),0);
					}
				}
			}
		}
		
		this.gamecards.getCardC(i).setTile(new TileM(x,y));
		if(this.currentCardID!=-1 || this.gamecards.getCardC(i).getCardM().isMine()){
			this.gamecards.getCardC(i).showCollider(true);
			this.getBoard().getTileC(x,y).showCollider(false);
		}

		if(Game.instance.getCards().getCardC(i).getCardM().getCharacterType()==73){
			List<TileM> neighbours = Game.instance.getBoard().getTileNeighbours(this.gamecards.getCardC(i).getTileM());
			for (int j = 0 ; j < neighbours.Count ;j++){
				characterID = Game.instance.getBoard().getTileC(neighbours[j]).getCharacterID();
				if(characterID!=-1){
					if(this.getCards().getCardC(i).getCardM().isMine()!=this.getCards().getCardC(characterID).getCardM().isMine()){
						Game.instance.getCards().getCardC(characterID).addAttackModifyer(new ModifyerM(-2-Game.instance.getCards().getCardC(i).getCardM().getSkill(0).Power,3,WordingGame.getText(83, new List<int>{2+Game.instance.getCards().getCardC(i).getCardM().getSkill(0).Power}),"Paladin",-1));
						Game.instance.getCards().getCardC(characterID).displaySkillEffect(WordingGame.getText(106)+"\n"+WordingGame.getText(83, new List<int>{-2-Game.instance.getCards().getCardC(i).getCardM().getSkill(0).Power}),2);
					}
				}
			}
		}
		else if(Game.instance.getCards().getCardC(i).getCardM().getCharacterType()==111){
			List<TileM> neighbours = Game.instance.getBoard().getTileNeighbours(this.gamecards.getCardC(i).getTileM());
			for (int j = 0 ; j < neighbours.Count ;j++){
				characterID = Game.instance.getBoard().getTileC(neighbours[j]).getCharacterID();
				if(characterID!=-1){
					Game.instance.getCards().getCardC(characterID).addShieldModifyer(new ModifyerM(4*Game.instance.getCards().getCardC(i).getCardM().getSkill(0).Power,16,WordingGame.getText(87, new List<int>{4*Game.instance.getCards().getCardC(i).getCardM().getSkill(0).Power}),"Paladin",-1));
					if(this.getCards().getCardC(characterID).getCardM().isMine()||this.currentCardID!=-1){
						Game.instance.getCards().getCardC(characterID).displaySkillEffect(WordingGame.getText(87, new List<int>{4*Game.instance.getCards().getCardC(i).getCardM().getSkill(0).Power}),0);
					}
				}
			}
		}
		else{
			List<TileM> neighbours = Game.instance.getBoard().getTileNeighbours(this.gamecards.getCardC(i).getTileM());
			found = -1 ;
			foundProtecteur = -1;
			for (int j = 0 ; j < neighbours.Count ;j++){
				characterID = Game.instance.getBoard().getTileC(neighbours[j]).getCharacterID();
				if(characterID!=-1){
					if(this.getCards().getCardC(characterID).getCardM().getCharacterType()==73){
						if(this.getCards().getCardC(i).getCardM().isMine()!=this.getCards().getCardC(characterID).getCardM().isMine()){
							found = characterID;
						}
					}
					else if(this.getCards().getCardC(characterID).getCardM().getCharacterType()==111)
					{
						foundProtecteur = characterID;
					}
				}
			}
			if(found!=-1){
				if(!this.gamecards.getCardC(i).hasPaladinModifyer()){
					Game.instance.getCards().getCardC(i).addAttackModifyer(new ModifyerM(-2-Game.instance.getCards().getCardC(found).getCardM().getSkill(0).Power,3,WordingGame.getText(83, new List<int>{2+Game.instance.getCards().getCardC(found).getCardM().getSkill(0).Power}),"Paladin",-1));
					Game.instance.getCards().getCardC(i).displaySkillEffect(WordingGame.getText(106)+"\n"+WordingGame.getText(83, new List<int>{-2-Game.instance.getCards().getCardC(found).getCardM().getSkill(0).Power}),2);
				}
			}
			else{
				if(this.gamecards.getCardC(i).hasPaladinModifyer()){
					Game.instance.getCards().getCardC(i).removePaladinModifyer();
					Game.instance.getCards().getCardC(i).displaySkillEffect(WordingGame.getText(105),0);
				}
			}

			if(foundProtecteur!=-1){
				if(!this.gamecards.getCardC(i).hasProtecteurModifyer()){
					Game.instance.getCards().getCardC(i).addShieldModifyer(new ModifyerM(4*Game.instance.getCards().getCardC(foundProtecteur).getCardM().getSkill(0).Power,16,WordingGame.getText(87, new List<int>{4*Game.instance.getCards().getCardC(i).getCardM().getSkill(0).Power}),"Paladin",-1));
					Game.instance.getCards().getCardC(i).displaySkillEffect(WordingGame.getText(87, new List<int>{4*Game.instance.getCards().getCardC(foundProtecteur).getCardM().getSkill(0).Power}),0);
				}
			}
			else{
				if(this.gamecards.getCardC(i).hasProtecteurModifyer()){
					Game.instance.getCards().getCardC(i).removeProtecteurModifyer();
					Game.instance.getCards().getCardC(i).displaySkillEffect(WordingGame.getText(131),0);
				}
			}
		}

		this.getBoard().getTileC(x,y).BruteStopSE();
		this.getBoard().getTileC(x,y).setCharacterID(i);

		if(this.currentCardID!=-1){
			if(this.getCurrentCard().getCardM().isMine()){
				this.actuController(false);
			}
			this.loadDestinations();
		}
	}

	public void dropOutsideBoard(){
		if(this.currentCardID==-1){

		}
		else{
			this.getBoard().getTileC(this.gamecards.getCardC(this.draggingCardID).getTileM()).showDestination(false);
		}
		this.gamecards.getCardC(this.draggingCardID).startMove(this.gamecards.getCardC(this.draggingCardID).getPosition(), this.getBoard().getTileC(this.gamecards.getCardC(this.draggingCardID).getTileM()).getPosition());
		this.draggingCardID = -1;
	}

	public void dropSBOutsideBoard(){
		this.getSkillButton(this.draggingSBID).showDescription(false);
		this.getSkillButton(this.draggingSBID).startMove(this.getSkillButton(this.draggingSBID).getPosition(), this.getSkillButton(this.draggingSBID).getInitialPosition());
		this.getBoard().stopTargets(this.getCurrentSkillButtonC().getTargets());
		this.draggingSBID = -1;
		this.actuController(true);
	}

	public void endMove(){
		if(this.currentCardID==-1){
			this.setInitialDestinations();
		}
		else{
			if(this.getCurrentCard().getCardM().isMine()){
				this.actuController(true);
			}
		}
	}

	public void pushStartButton(){
		if(this.ia || this.isTutorial()){
			this.addStartGame(true);
		}
		else{
			StartCoroutine(GameRPC.instance.launchRPC("startGameRPC"));
		}
		this.getStartButton().setText(WordingGame.getText(70));
		this.getStartButton().hideButton();
	}

	public void addStartGame(bool b){
		this.nbPlayersReadyToStart++;
		if(nbPlayersReadyToStart==2){
			this.startGame(!b);
		}
	}

	public void deleteDestinations(){
		for(int i = 0 ; i < this.getBoard().getBoardWidth() ; i++){
			for(int j = 0 ; j < this.getBoard().getBoardHeight() ; j++){
				this.getBoard().getTileC(i,j).showDestination(false);
			}
		}
	}

	public void startGame(bool b){
		this.getStartButton().show(false);
		this.showOpponentCards();
		if(b){
			this.cardsToPlay.Add(0);
			this.cardsToPlay.Add(4);
			this.cardsToPlay.Add(1);
			this.cardsToPlay.Add(5);
			this.cardsToPlay.Add(2);
			this.cardsToPlay.Add(6);
			this.cardsToPlay.Add(3);
			this.cardsToPlay.Add(7);
		}
		else{
			this.cardsToPlay.Add(4);
			this.cardsToPlay.Add(0);
			this.cardsToPlay.Add(5);
			this.cardsToPlay.Add(1);
			this.cardsToPlay.Add(6);
			this.cardsToPlay.Add(2);
			this.cardsToPlay.Add(7);
			this.cardsToPlay.Add(3);
		}
		this.getTimeline().show(true);
		this.firstTurn();
	}

	public void showOpponentCards(){
		if(this.firstPlayer){
			this.gamecards.getCardC(4).show(true);
			this.gamecards.getCardC(5).show(true);
			this.gamecards.getCardC(6).show(true);
			this.gamecards.getCardC(7).show(true);
		}
		else{
			this.gamecards.getCardC(0).show(true);
			this.gamecards.getCardC(1).show(true);
			this.gamecards.getCardC(2).show(true);
			this.gamecards.getCardC(3).show(true);
		}
	}

	public void firstTurn(){
		this.indexPlayingCard = 0 ;
		this.giveHandTo(this.cardsToPlay[0]);
	}

	public void handleBeginningTurnEffects(){
		print("BEGINNINGTURNEFFECTS");
		int playerID;
		int tempInt;
		if(this.getCurrentCard().getCardM().getCharacterType()==69){
			this.getSkills().skills[69].resolve(this.getCurrentCard().getCardM().getSkill(0), this.getCurrentCardID());
		}
		else if(this.getCurrentCard().getCardM().getCharacterType()==138){
			if(this.indexMeteores==3){
				this.getCurrentCard().godTransform();
			}
		}
		else if(this.getCurrentCard().getCardM().getCharacterType()==141){
			int soin = Mathf.Min(this.getCurrentCard().getTotalLife()-this.getCurrentCard().getLife(), 10+2*this.getCurrentCard().getCardM().getSkill(0).Power);
			this.getCurrentCard().addDamageModifyer(new ModifyerM(-1*soin, -1, "", "",-1));
			this.getCurrentCard().displaySkillEffect(WordingGame.getText(11, new List<int>{soin}),0);
		}
		else if(this.getCurrentCard().getCardM().getCharacterType()==110){
			if(this.getCurrentCard().getCardM().isMine()){
				List<TileM> neighbours = this.getBoard().getTileNeighbours(this.getCurrentCard().getTileM());
				for(int i = 0 ; i < neighbours.Count; i++){
					playerID = this.getBoard().getTileC(neighbours[i]).getCharacterID()	;
					if(playerID>=0){
						if(!Game.instance.getCards().getCardC(playerID).getCardM().isMine()){
							if(UnityEngine.Random.Range(1,101)<=this.getCurrentCard().getCardM().getSkill(0).Power*4+10){
								if(Game.instance.isIA() || Game.instance.isTutorial()){
									Game.instance.getSkills().skills[110].effects(playerID, 0);
								}
								else{
									Game.instance.launchCorou("EffectsSkillRPC", 110, playerID, 0);
								}
							}
						}
					}
				}
			}
		}
		else if(this.getCurrentCard().getCardM().getCharacterType()==113){
			if(this.getCurrentCard().getCardM().isMine()){
				List<TileM> neighbours = this.getBoard().getTileNeighbours(this.getCurrentCard().getTileM());
				for(int i = 0 ; i < neighbours.Count; i++){
					playerID = this.getBoard().getTileC(neighbours[i]).getCharacterID()	;
					if(playerID>=0){
						if(UnityEngine.Random.Range(1,101)<=this.getCurrentCard().getCardM().getSkill(0).Power*5+50){
							if(Game.instance.isIA() || Game.instance.isTutorial()){
								Game.instance.getSkills().skills[113].effects(playerID, 0);
							}
							else{
								Game.instance.launchCorou("EffectsSkillRPC", 113, playerID, 0);
							}
						}
					}
				}
			}
		}
		else if(this.getCurrentCard().getCardM().getCharacterType()==68){
			this.getSkills().skills[68].resolve(this.getCurrentCard().getCardM().getSkill(0), this.getCurrentCardID());
		}
		else if(this.getCurrentCard().getCardM().getCharacterType()==75){
			List<TileM> neighbours = this.getBoard().getTileNeighbours(this.getCurrentCard().getTileM());
			for(int i = 0 ; i < neighbours.Count; i++){
				playerID = this.getBoard().getTileC(neighbours[i]).getCharacterID()	;
				if(playerID!=-1){
					if(this.getCards().getCardC(playerID).getCardM().isMine()==this.getCurrentCard().getCardM().isMine()){
						if(this.getCards().getCardC(playerID).getLife()!=this.getCards().getCardC(playerID).getTotalLife()){
							tempInt = this.getCurrentCard().getCardM().getSkill(0).Power+5;
							this.getCards().getCardC(playerID).addDamageModifyer(new ModifyerM(-1*tempInt, -1, "", "",-1));
							this.getCards().getCardC(playerID).displaySkillEffect(WordingGame.getText(11, new List<int>{tempInt}),0);
						}
					}
				}
			}
		}
		else if(this.getCurrentCard().getCardM().getCharacterType()==67){
			if(this.getCurrentCard().getCardM().isMine()){
				List<int> opponents = this.getTargetableOpponents(true);
				int random = opponents[UnityEngine.Random.Range(0,opponents.Count)];

				if(Game.instance.isIA() || Game.instance.isTutorial()){
					Game.instance.getSkills().skills[67].effects(random, this.getCurrentCard().getCardM().getSkill(0).Power);
				}
				else{
					Game.instance.launchCorou("EffectsSkillRPC", 67, random, this.getCurrentCard().getCardM().getSkill(0).Power);
				}

				if(UnityEngine.Random.Range(0,101)<=50){
					opponents.Remove(random);
					if(opponents.Count>0){
						random = opponents[UnityEngine.Random.Range(0,opponents.Count)];

						if(Game.instance.isIA() || Game.instance.isTutorial()){
							Game.instance.getSkills().skills[67].effects(random, this.getCurrentCard().getCardM().getSkill(0).Power);
						}
						else{
							Game.instance.launchCorou("EffectsSkillRPC", 67, random, this.getCurrentCard().getCardM().getSkill(0).Power);
						}
					}
				}
			}
		}
	}

	public void launchNextTurn(){
		if(this.getCurrentCard().isFatality()){
			int degats = this.getCurrentCard().getLife();
			this.getCurrentCard().displayAnim(1);
			this.getCurrentCard().addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
		}
		else{
			this.getCurrentCard().stopClignote();
			if(this.getCurrentCard().getCardM().isMine()){
				this.getMyHoveredCard().stopClignoting();
			}
			else{
				this.getHisHoveredCard().stopClignoting();
			}
			this.getCurrentCard().checkModifyers();
			if(!this.getCurrentCard().isDead()){
				this.getCurrentCard().displayBackTile(true);
			}
			this.getCurrentCard().play(false);
			this.getCurrentCard().move(false);
			this.indexPlayingCard++;
			bool toLaunchMeteors = false;
			if(this.indexPlayingCard>=this.getCards().getNumberOfCards()){
				this.indexPlayingCard = 0 ;
				toLaunchMeteors = true;
			}
			while(this.getCards().getCardC(this.cardsToPlay[indexPlayingCard]).isDead()){
				this.indexPlayingCard++;
				if(this.indexPlayingCard>=this.getCards().getNumberOfCards()){
					this.indexPlayingCard = 0 ;
					toLaunchMeteors = true;
				}
			}

			if(!toLaunchMeteors){
				this.giveHandTo(this.cardsToPlay[this.indexPlayingCard]);
			}
			else{
				StartCoroutine(this.launchMeteors());
			}
		}

	}

	public void meteorHitTile(TileM t, int amount){
		if(this.getBoard().getTileC(t).getCharacterID()!=-1){
				this.getCards().getCardC(this.getBoard().getTileC(t).getCharacterID()).meteorEffect(amount);
		}
		else{
			this.getBoard().getTileC(t).displayAnim(0);
		}
	}

	public IEnumerator launchMeteors(){
		GameObject.Find("GameBackground").transform.GetComponent<GameBackgroundController>().launchMeteors();
		yield return new WaitForSeconds(1f);

		int amount = 5 ;
		SoundController.instance.playSound(25);

		for(int i = 0 ; i < this.getBoard().getBoardWidth() ; i++){
			this.meteorHitTile(new TileM(i,0), amount*this.indexMeteores);
			this.meteorHitTile(new TileM(i,this.getBoard().getBoardHeight()-1), amount*this.indexMeteores);
		}

		if(this.indexMeteores>=2){
			for(int i = 0 ; i < this.getBoard().getBoardWidth() ; i++){
				this.meteorHitTile(new TileM(i,1), amount*(this.indexMeteores-1));
				this.meteorHitTile(new TileM(i,this.getBoard().getBoardHeight()-2), amount*(this.indexMeteores-1));
			}
		}

		if(this.indexMeteores>=3){
			for(int i = 0 ; i < this.getBoard().getBoardWidth() ; i++){
				this.meteorHitTile(new TileM(i,2), amount*(this.indexMeteores-2));
				this.meteorHitTile(new TileM(i,this.getBoard().getBoardHeight()-3), amount*(this.indexMeteores-2));
			}
		}

		if(this.indexMeteores>=4){
			for(int i = 0 ; i < this.getBoard().getBoardWidth() ; i++){
				this.meteorHitTile(new TileM(i,3), amount*(this.indexMeteores-3));
				this.meteorHitTile(new TileM(i,this.getBoard().getBoardHeight()-4), amount*(this.indexMeteores-3));
			}
		}

		this.indexMeteores++;
		yield return new WaitForSeconds(3f);
		while(this.getCards().getCardC(this.cardsToPlay[indexPlayingCard]).isDead()){
			this.indexPlayingCard++;
			if(this.indexPlayingCard>=this.getCards().getNumberOfCards()){
				this.indexPlayingCard = 0 ;
			}
		}

		this.giveHandTo(this.cardsToPlay[this.indexPlayingCard]);
	}

	public void updateTimeline(){
		List<int> liste = new List<int>();
		liste.Add(this.lastToPlay);
		int tempInt = this.indexPlayingCard;
		int tempIndex = this.indexMeteores;

		while(liste.Count<8){
			if(!this.getCards().getCardC(this.cardsToPlay[tempInt]).isDead()){
				liste.Add(this.cardsToPlay[tempInt]);
			}
			if(liste.Count<8){
				tempInt++;
				if(tempInt==this.getCards().getNumberOfCards()){
					tempInt = 0;
					liste.Add(Mathf.Max(-4,-1*tempIndex));
					tempIndex++;
				}
			}
		}
		this.getTimeline().changeFaces(liste);
	}

	public void giveHandTo(int i){
		print("AU TOUR DE "+i);
		if(this.getCards().getCardC(i).getCardM().isMine()){
			this.getInterlude().launchType(0);
		}
		else{
			this.getInterlude().launchType(1);
		}
	
		this.lastToPlay = this.currentCardID;
		this.currentCardID = i;
		this.updateTimeline();
		this.getCards().getCardC(this.currentCardID).startClignote();
			
		if(this.getCards().getCardC(this.currentCardID).getCardM().isMine()){
			this.getMyHoveredCard().setNextCard(this.currentCardID);
			if(this.getCurrentCard().isParalized()){
				this.getCurrentCard().play(true);
			}
			this.loadController();
		}
		else{
			this.getHisHoveredCard().setNextCard(this.currentCardID);
			this.displayController(false);
		}

		this.loadDestinations();
		this.displayDestinations();

	}

	public void startActions(){
		this.getTimer().setTimer(30);
		this.handleBeginningTurnEffects();

		if(this.ia){
			if(!this.getCards().getCardC(this.currentCardID).getCardM().isMine()){
				StartCoroutine(this.intelligence.play());
			}
		}
	}

	public void loadController(){
		CardC c = this.getCurrentCard();
		this.getSkillButton(0).setCard(c);
		for(int i = 1 ; i < 4 ; i++){
			if (c.getCardM().getSkill(i).IsActivated==1){
				this.getSkillButton(i).setCard(c);
			}
		}

		this.actuController(false);

		this.getSkillButton(0).show(true);
		for(int i = 1 ; i < 4 ; i++){
			if (c.getCardM().getSkill(i).IsActivated==1){
				this.getSkillButton(i).show(true);
			}
		}
		this.getPassButton().setLaunchable(true);
		this.getPassButton().show(true);
	}

	public void setSE(bool b){
		this.SE = b ;
	}

	public void setLaunchNextTurn(bool b){
		this.toLaunchNextTurn = b ;
	}

	public bool getSE(){
		return this.SE ;
	}

	public void actuController(bool checkEndTurn){
		CardC c = this.getCurrentCard();
		if(this.getCurrentCard().hasPlayed()){
			this.getSkillButton(0).forbid();
			for(int i = 1 ; i < 4 ; i++){
				if (c.getCardM().getSkill(i).IsActivated==1){
					this.getSkillButton(i).forbid();
				}
			}
			if(checkEndTurn){
				if(this.getCurrentCard().hasMoved()){
					if(Game.instance.isIA() || Game.instance.isTutorial()){
						Game.instance.setLaunchNextTurn(true);
					}
					else{
						StartCoroutine(GameRPC.instance.launchRPC("toLaunchNextTurnRPC"));
					}
				}
			}
		}
		else{
			this.getSkillButton(0).update();
			for(int i = 1 ; i < 4 ; i++){
				if (c.getCardM().getSkill(i).IsActivated==1){
					this.getSkillButton(i).update();
				}
			}	
		}

		if(SE){
			this.getPassButton().grey();
		}
		else{
			this.getPassButton().white();
		}
	}

	public void displayDestinations(){
		TileM tile = this.board.getMouseTile();
		int characterID;

		if(tile.x>=0 && tile.x<Game.instance.getBoard().getBoardWidth() && tile.y>=0 && tile.y<Game.instance.getBoard().getBoardHeight()){
			characterID = this.getBoard().getTileC(tile).getCharacterID();
		}
		else{
			characterID = -1;
		}

		if(characterID==-1){
			this.displayDestinations(this.currentCardID);
		}
		else{
			this.displayDestinations(characterID);
		}
	}

	public void displayDestinations(int c){
		this.deleteDestinations();
		int type = 0 ;
		if(c==this.currentCardID && !this.getCurrentCard().hasMoved()){
			if(this.getCurrentCard().getCardM().isMine()){
				type = 0;
			}
			else{
				type = 1 ;
			}
		}
		else{
			type = 2 ;
		}
		for (int i = 0 ; i < this.getBoard().getBoardWidth();i++){
			for (int j = 0 ; j < this.getBoard().getBoardHeight();j++){
				if(this.gamecards.getCardC(c).canMoveOn(i,j)){
					this.getBoard().getTileC(i,j).displayDestination(type);
				}
				else{
					this.getBoard().getTileC(i,j).showDestination(false);
				}
			}
		}
	}

	public void loadDestinations(){
		for (int i = 0 ; i < this.gamecards.getNumberOfCards() ; i++){
			if(!this.gamecards.getCardC(i).isDead()){
				this.gamecards.getCardC(i).setDestinations(this.getDestinations(i));
			}
		}
		this.displayDestinations();
	}

	public bool[,] getDestinations(int i){
		int boardWidth = this.getBoard().getBoardWidth();
		int boardHeight = this.getBoard().getBoardHeight();

		bool[,] hasBeenPassages = new bool[boardWidth, boardHeight];
		bool[,] isDestination = new bool[boardWidth, boardHeight];
		for(int l = 0 ; l < boardWidth ; l++){
			for(int k = 0 ; k < boardHeight ; k++){
				hasBeenPassages[l,k]=false;
				isDestination[l,k]=false;
			}
		}
		List<TileM> baseTiles = new List<TileM>();
		List<TileM> tempTiles = new List<TileM>();
		List<TileM> tempNeighbours ;
		baseTiles.Add(this.gamecards.getCardC(i).getTileM());
		int move = this.gamecards.getCardC(i).getMove();
		
		int j = 0 ;
		bool mine = this.gamecards.getCardC(i).getCardM().isMine()	;
		while (j < move){
			tempTiles = new List<TileM>();
			for(int k = 0 ; k < baseTiles.Count ; k++){
				tempNeighbours = this.getBoard().getTileNeighbours(baseTiles[k]);
				for(int l = 0 ; l < tempNeighbours.Count ; l++){
					if(!hasBeenPassages[tempNeighbours[l].x, tempNeighbours[l].y]){
						if(this.getBoard().getTileC(tempNeighbours[l].x, tempNeighbours[l].y).canPassOver(mine)){
							tempTiles.Add(tempNeighbours[l]);
							hasBeenPassages[tempNeighbours[l].x, tempNeighbours[l].y]=true;
						}
					}
					if(this.getBoard().getTileC(tempNeighbours[l].x, tempNeighbours[l].y).isEmpty()){
						if(!isDestination[tempNeighbours[l].x, tempNeighbours[l].y]){
							isDestination[tempNeighbours[l].x, tempNeighbours[l].y]=true;
						}
					}
				}	
			}
			baseTiles = new List<TileM>();
			for(int l = 0 ; l < tempTiles.Count ; l++){
				baseTiles.Add(tempTiles[l]);
			}
			j++;
		}

		return isDestination;
	}

	public void displayController(bool b){
		this.getSkillButton(0).show(b);
		this.getSkillButton(1).show(b);
		this.getSkillButton(2).show(b);
		this.getSkillButton(3).show(b);
		this.getPassButton().show(b);
	}

	public void timeOver(){
		if(this.currentCardID==-1){
			this.pushStartButton();
		}
		else{
			if(this.getCurrentCard().getCardM().isMine()){
				
			}
			else{

			}
		}
	}

	public void hitSkillButton(int id){
		this.draggingSBID = id;

		this.getSkillButton(id).showTitle(false);
		this.getSkillButton(id).showCollider(false);
		this.getSkillButton(id).red();
	}

	public void hitPassButton(){
		print("Je passe");
		if(!this.areUnitsDead(true) && !this.areUnitsDead(false)){
			this.displayController(false);
			if(this.ia || this.isTutorial()){
				this.setLaunchNextTurn(true);
			}
			else{
				StartCoroutine(GameRPC.instance.launchRPC("toLaunchNextTurnRPC"));
			}
		}
	}

	public SkillButtonC getCurrentSkillButtonC(){
		return this.getSkillButton(this.draggingSBID);
	}

	public bool areUnitsDead(bool b){
		bool areTheyDead = true ;

		for (int i = 0 ; i < 8 ; i++){
			if (this.getCards().getCardC(i).getCardM().isMine()==b){
				if (!this.getCards().getCardC(i).isDead()){
					areTheyDead = false ;
				}
			}
		}
		if(areTheyDead){
			if(this.ia || this.isTutorial()){
				StartCoroutine(quitGameHandler(b));
			}
			else{
				StartCoroutine(GameRPC.instance.launchRPC("LostRPC",b));
			}
		}

		return areTheyDead;
	}

	public IEnumerator quitGameHandler(bool hasFirstPlayerLost)
	{		
		//print("LOST");
		this.getHisHoveredCard().moveCharacterBackward();
		this.getMyHoveredCard().moveCharacterBackward();
		int type = 2;
		if(hasFirstPlayerLost==this.firstPlayer){
			type = 3 ;
		}
		this.getInterlude().launchType(type);

		if(this.isTutorial())
		{
            if(hasFirstPlayerLost==this.firstPlayer)
			{
                ApplicationModel.player.HasWonLastGame=false;
				ApplicationModel.player.setTutorialStep (3);
			}
			else
			{
                ApplicationModel.player.HasWonLastGame=true;
				ApplicationModel.player.setTutorialStep (2);
			}
		}
		else
		{
            if(hasFirstPlayerLost==this.firstPlayer)
            {
                ApplicationModel.player.HasWonLastGame=false;
                ApplicationModel.player.PercentageLooser=Game.instance.getPercentageTotalDamages();
            }
            else
            {
                ApplicationModel.player.HasWonLastGame=true;
            }
			yield return (StartCoroutine(this.sendStat(ApplicationModel.player.PercentageLooser,ApplicationModel.currentGameId,ApplicationModel.player.HasWonLastGame,this.firstPlayer)));
		}
		yield break;
	}

	public int getPercentageTotalDamages(){
		int damages = 0;
		int total = 0;
		for(int i = 0 ; i < this.getCards().getNumberOfCards(); i++){
			if(!this.getCards().getCardC(i).getCardM().isMine()){
				damages+=Mathf.Max(0,(this.getCards().getCardC(i).getCardM().getLife()-this.getCards().getCardC(i).getLife()));
				total+=this.getCards().getCardC(i).getCardM().getLife();
			}
		}
		return Mathf.FloorToInt(100*damages/total);
	}

	public IEnumerator sendStat(int percentageTotalDamages, int currentGameid, bool hasWon, bool isFirstPlayer)
	{
		//Debug.Log ("Damages :" + percentageTotalDamages + "// CurentGameId :" + currentGameid.ToString () + "// HasWon :" + System.Convert.ToInt32 (hasWon).ToString () + "// isFirstPlayer :" + System.Convert.ToInt32 (isFirstPlayer).ToString ());
		int hasWonInt = 0;
        if(hasWon)
        {
            hasWonInt=1;
        }
        int isFirstPlayerInt =0;
        if(isFirstPlayer)
        {
            isFirstPlayerInt=1;
        }
		
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
        form.AddField("myform_percentagelooser",percentageTotalDamages);
        form.AddField("myform_currentgameid",currentGameid);
        form.AddField("myform_haswon",hasWonInt);
        form.AddField("myform_isfirstplayer",isFirstPlayerInt);

        ServerController.instance.setRequest(this.URLStat, form);
        yield return ServerController.instance.StartCoroutine("executeRequest");

        if(ServerController.instance.getError()!="")
        {
            Debug.Log(ServerController.instance.getError());
            //ServerController.instance.lostConnection();
        }
	}

	public void hitForfeit(){
		if(this.ia || this.isTutorial()){
			StartCoroutine(quitGameHandler(true));
		}
		else{
			StartCoroutine(GameRPC.instance.launchRPC("LostRPC",true));
		}
	}

	public void updateModifyers(int i){
		if(this.getCards().getCardC(i).getCardM().isMine() || Game.instance.getCurrentCardID()!=-1){
			this.getCards().getCardC(i).showIcons(true);
		}
		if(this.getMyHoveredCard().getCurrentCard()!=-1){
			if(this.getMyHoveredCard().getCurrentCard()==i){
				this.getMyHoveredCard().setCard(i);
			}
		}
		if(this.getHisHoveredCard().getCurrentCard()!=-1){
			if(this.getHisHoveredCard().getCurrentCard()==i){
				this.getHisHoveredCard().setCard(i);
			}
		}
		if(this.getCurrentCardID()!=-1){
			if(this.getCurrentCard().getCardM().isMine()){
				this.actuController(false);
			}
		}
	}

	public int getIndexMeteores(){
		return this.indexMeteores;
	}

	public void initialize2PGame(bool isFirstP, int idDeck, int idPlayer, int rankingPoints){
		if(idPlayer!=ApplicationModel.player.Id){
			StartCoroutine(this.initializeGame(isFirstP, false, idPlayer, rankingPoints, idDeck));
		}
	}

	private IEnumerator initializeGame(bool isFirstPlayer, bool isIAGame, int hisId, int hisRankingPoints, int hisDeckId)
    {
    	string isFirstPlayerString="1";
    	if(!isFirstPlayer)
    	{
    		isFirstPlayerString="0";
    	}
		string isIAGameString="1";
    	if(!isIAGame)
    	{
    		isIAGameString="0";
    	}
        WWWForm form = new WWWForm();                                       
        form.AddField("myform_hash", ApplicationModel.hash);                
		form.AddField("myform_isfirstplayer", isFirstPlayerString);
		form.AddField("myform_isiagame", isIAGameString);        
		form.AddField("myform_myid", ApplicationModel.player.Id.ToString());     
		form.AddField("myform_hisid", hisId.ToString());     
		form.AddField("myform_myrankingpoints",ApplicationModel.player.RankingPoints.ToString());     
		form.AddField("myform_hisrankingpoints", hisRankingPoints.ToString());  
		form.AddField("myform_gametype", ApplicationModel.player.ChosenGameType.ToString());    
		//form.AddField("myform_mydeck", ApplicationModel.player.SelectedDeckId.ToString());     
		form.AddField("myform_hisdeck", hisDeckId.ToString()); 

        ServerController.instance.setRequest(URLInitiliazeGame, form);
        yield return ServerController.instance.StartCoroutine("executeRequest");
        
        if(ServerController.instance.getError()=="")
        {
			string result = ServerController.instance.getResult();
			string[] data=result.Split(new string[] { "#END#" }, System.StringSplitOptions.None);

			//ApplicationModel.player.MyDeck=new Deck();

			string[] myDeckData = data[0].Split(new string[] { "#CARD#" }, System.StringSplitOptions.None);
			for(int i = 0 ; i < myDeckData.Length ; i++){
				//ApplicationModel.player.MyDeck.cards.Add(new Card());
				//ApplicationModel.player.MyDeck.cards[i].parseCard(myDeckData[i]);
				//ApplicationModel.player.MyDeck.cards[i].deckOrder=i;
			}

			if(!isIAGame)
			{
				ApplicationModel.opponentDeck=new Deck();

				string[] hisDeckData = data[1].Split(new string[] { "#CARD#" }, System.StringSplitOptions.None);
				for(int i = 0 ; i < hisDeckData.Length ; i++){
					ApplicationModel.opponentDeck.cards.Add(new Card());
					ApplicationModel.opponentDeck.cards[i].parseCard(hisDeckData[i]);
					ApplicationModel.opponentDeck.cards[i].deckOrder=i;
				}
			}
			else
			{
				string[] iAData = data[1].Split(new string[] { "#IAINFO#" }, System.StringSplitOptions.None);
				ApplicationModel.hisPlayerID=System.Convert.ToInt32(iAData[0]);
				ApplicationModel.hisPlayerName=iAData[1];
				ApplicationModel.hisRankingPoints=System.Convert.ToInt32(iAData[2]);
			}

			if(isFirstPlayer)
			{
				ApplicationModel.currentGameId=System.Convert.ToInt32(data[2]);
			}
        }
        else
        {
            Debug.Log(ServerController.instance.getError());
            //ServerController.instance.lostConnection();
        }

        this.createCards();
    }

	public void launchCorou(string s, int a, int b, int c){

		StartCoroutine(GameRPC.instance.launchRPC(s, a, b, c));
    }

	public void launchCorou(string s, int a, int b, int c, int d){
		StartCoroutine(GameRPC.instance.launchRPC(s, a, b, c, d));
    }

	public void launchCorou(string s, int a, int b, int c, int d, int e){
		StartCoroutine(GameRPC.instance.launchRPC(s, a, b, c, d, e));
    }

	public void launchCorou(string s, int a, int b, int c, int d, bool boo){
		StartCoroutine(GameRPC.instance.launchRPC(s, a, b, c, d, boo));
    }

    public void launchCorou(string s, int a, int b){
		StartCoroutine(GameRPC.instance.launchRPC(s, a, b));
    }

	public void launchCorou(string s, int a){
		StartCoroutine(GameRPC.instance.launchRPC(s, a));
    }

    public List<int> getTargetableOpponents(bool isMine){
    	List<int> opponents = new List<int>();
    	for(int i = 0 ; i < this.getCards().getNumberOfCards() ;i++){
			if(this.getCards().getCardC(i).getCardM().isMine()!=isMine){
	    		if(!this.getCards().getCardC(i).isDead()){
					if(this.getCards().getCardC(i).canBeTargeted()){
						opponents.Add(i);
					}
	    		}
	    	}
    	}
    	return opponents;
    }

	public List<int> getTargetableAllys(bool isMine){
    	List<int> opponents = new List<int>();
    	for(int i = 0 ; i < this.getCards().getNumberOfCards() ;i++){
    		if(i!=this.getCurrentCardID()){
				if(this.getCards().getCardC(i).getCardM().isMine()==isMine){
		    		if(!this.getCards().getCardC(i).isDead()){
						if(this.getCards().getCardC(i).canBeTargeted()){
							opponents.Add(i);
						}
		    		}
		    	}
	    	}
    	}
    	return opponents;
    }

	public List<int> getTargetableAnyone(){
    	List<int> opponents = new List<int>();
    	for(int i = 0 ; i < this.getCards().getNumberOfCards() ;i++){
			if(!this.getCards().getCardC(i).isDead()){
				if(this.getCards().getCardC(i).canBeTargeted()){
					opponents.Add(i);
				}
    		}
    	}
    	return opponents;
    }

    public void setNextPlayer(int j){
    	for(int i = this.getCards().getNumberOfCards()-1 ; i>=0 ; i--){
			if(this.cardsToPlay[indexPlayingCard]==j){
				this.cardsToPlay.Remove(j);
				i=-1;
			}
    	}

		this.cardsToPlay.Insert(this.indexPlayingCard+1, j);
		this.updateTimeline();
    }
}