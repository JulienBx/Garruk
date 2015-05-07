using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayingCardController : MonoBehaviour
{
	private PlayingCardView playingCardView;
	public GUIStyle[] guiStylesMyCharacter ;
	public GUIStyle[] guiStylesHisCharacter ;
	public Texture2D[] icons ;
	private float scale ;
	public GameObject tile ;
	public Card card ;
	public int IDCharacter = -1 ;
	public int sortID = -1 ;
	public bool isMovable;
	public int damage = 0;
	public bool isDead ;
	public Texture2D[] pictures;
	public bool isSelected ;
	public bool isMoved ;
	public List<string> titlesSkill ;
	public List<string> descriptionsSkill ;
	public bool isMine;

	public List<StatModifier> statModifiers ;

	List<GameSkill> skills ;

	void Awake()
	{
		this.playingCardView = gameObject.AddComponent <PlayingCardView>();
		this.playingCardView.playingCardVM.attackIcon = icons[0];
		this.playingCardView.playingCardVM.moveIcon = icons[1];
		this.isMovable = true ;
		this.isDead = false ;
		this.isSelected = false ;
		this.isMoved = false ;
	}

	public void setStyles(bool isMyCharacter){
		isMine = isMyCharacter;
		if (isMyCharacter){
			this.playingCardView.playingCardVM.backgroundStyle = guiStylesMyCharacter[0];
			this.playingCardView.playingCardVM.nameTextStyle = guiStylesMyCharacter[1];
			this.playingCardView.playingCardVM.attackZoneTextStyle = guiStylesMyCharacter[2];
			this.playingCardView.playingCardVM.imageStyle = guiStylesMyCharacter[5];
			this.playingCardView.playingCardVM.lifeBarStyle = guiStylesMyCharacter[6];
			this.playingCardView.playingCardVM.backgroundBarStyle = guiStylesMyCharacter[7];
			this.playingCardView.playingCardVM.emptyButtonStyle = guiStylesMyCharacter[8];
			this.playingCardView.playingCardVM.skillTitleTextStyle = guiStylesMyCharacter[9];
			this.playingCardView.playingCardVM.skillDescriptionTextStyle = guiStylesMyCharacter[10];
			this.playingCardView.playingCardVM.QuicknessBarStyle = guiStylesMyCharacter[11];
			this.playingCardView.playingCardVM.buttonTextStyle = guiStylesMyCharacter[13];
		}
		else{
			this.playingCardView.playingCardVM.backgroundStyle = guiStylesHisCharacter[0];
			this.playingCardView.playingCardVM.nameTextStyle = guiStylesHisCharacter[1];
			this.playingCardView.playingCardVM.attackZoneTextStyle = guiStylesHisCharacter[2];
			this.playingCardView.playingCardVM.imageStyle = guiStylesHisCharacter[5];
			this.playingCardView.playingCardVM.lifeBarStyle = guiStylesHisCharacter[6];
			this.playingCardView.playingCardVM.backgroundBarStyle = guiStylesHisCharacter[7];
			this.playingCardView.playingCardVM.emptyButtonStyle = guiStylesHisCharacter[8];
			this.playingCardView.playingCardVM.skillTitleTextStyle = guiStylesHisCharacter[9];
			this.playingCardView.playingCardVM.skillDescriptionTextStyle = guiStylesHisCharacter[10];
			this.playingCardView.playingCardVM.QuicknessBarStyle = guiStylesHisCharacter[11];
		}
	}

	public void setCard(Card c){
		this.card = c ;
		this.playingCardView.playingCardVM.name = c.Title ;
		this.playingCardView.playingCardVM.attack = ""+c.Attack ;
		this.playingCardView.playingCardVM.move = ""+c.Move ;
		this.playingCardView.playingCardVM.maxLife = c.Life ;
		this.playingCardView.playingCardVM.life = c.Life ;
		this.playingCardView.playingCardVM.picture = this.pictures[c.ArtIndex];

		for (int i = 0 ; i < c.Skills.Count ; i++){
			this.playingCardView.playingCardVM.skillTitles.Add (c.Skills[i].Name);
			this.playingCardView.playingCardVM.skillDescriptions.Add (c.Skills[i].Description);
		}
	}

	public void setSkills(){
		this.skills = new List<GameSkill>();
		for (int i = 0 ; i < 4 ; i++){
			if (this.card.Skills.Count>i){
				switch(this.card.Skills[i].Id)
				{
				case 0:
					this.skills.Add(new DivisionSkill());
					break ;
				case 1:
					this.skills.Add(new Reflexe());
					break;
				case 8:
					this.skills.Add(new TirALarc());
					break;
				case 9:
					this.skills.Add(new Furtivite());
					break;
				case 10:
					this.skills.Add(new Assassinat());
					break;
				case 11:
					this.skills.Add(new AttaquePrecise());
					break;
				case 12:
					this.skills.Add(new AttaqueRapide());
					break;
				case 13:
					this.skills.Add(new PiegeALoups());
					break;
				case 15:
					this.skills.Add(new Espionnage());
					break;
				default:
					print ("Je ne connais pas le skill "+this.card.Skills[i].Id);
					break;
				}
			}
		}
	}

	public void resize(int h){
		this.scale = h*0.001f/1000f ;
		this.playingCardView.playingCardVM.scale = new Vector3(this.scale, this.scale, this.scale);
		if(this.isMine){
			if (this.isSelected){
				this.playingCardView.playingCardVM.ScreenPosition = new Vector3(Screen.width*((this.sortID-1)*0.13f+0.12f+0.21f),0.93f*h,0);
				this.playingCardView.playingCardVM.infoRect = new Rect(this.playingCardView.playingCardVM.ScreenPosition.x-0.12f*Screen.width, this.playingCardView.playingCardVM.ScreenPosition.y-0.07f*h, 0.24f*Screen.width, 0.14f*h);
				this.playingCardView.playingCardVM.isSelected = true ;
			}
			else if (this.isMoved){
				this.playingCardView.playingCardVM.ScreenPosition = new Vector3(Screen.width*((this.sortID-1)*0.13f+0.18f+0.21f),0.93f*h,0);
				this.playingCardView.playingCardVM.infoRect = new Rect(this.playingCardView.playingCardVM.ScreenPosition.x-0.06f*Screen.width, this.playingCardView.playingCardVM.ScreenPosition.y-0.07f*Screen.height, 0.12f*Screen.width, 0.14f*Screen.height);
			}
			else{
				this.playingCardView.playingCardVM.ScreenPosition = new Vector3(Screen.width*((this.sortID-1)*0.13f+0.06f+0.21f),0.93f*h,0);
				this.playingCardView.playingCardVM.infoRect = new Rect(this.playingCardView.playingCardVM.ScreenPosition.x-0.06f*Screen.width, this.playingCardView.playingCardVM.ScreenPosition.y-0.07f*Screen.height, 0.12f*Screen.width, 0.14f*Screen.height);
			}
			if (GameController.instance.isFirstPlayer){
				this.playingCardView.playingCardVM.position = Camera.main.ScreenToWorldPoint(this.playingCardView.playingCardVM.ScreenPosition);
				this.playingCardView.playingCardVM.position.y = -5f ;
			}
			else{
				this.playingCardView.playingCardVM.position = Camera.main.ScreenToWorldPoint(this.playingCardView.playingCardVM.ScreenPosition);
				this.playingCardView.playingCardVM.position.y = 5-this.playingCardView.playingCardVM.position.y;
				this.playingCardView.playingCardVM.position.y = 5f ;
			}
		}
		else{
			if (this.isSelected){
				this.playingCardView.playingCardVM.ScreenPosition = new Vector3(Screen.width*((this.sortID-1)*0.13f+0.15f),0.07f*h,0);
				this.playingCardView.playingCardVM.infoRect = new Rect(this.playingCardView.playingCardVM.ScreenPosition.x-0.12f*Screen.width, this.playingCardView.playingCardVM.ScreenPosition.y-0.07f*h, 0.24f*Screen.width, 0.14f*h);
				this.playingCardView.playingCardVM.isSelected = true ;
			}
			else if (this.isMoved){
				this.playingCardView.playingCardVM.ScreenPosition = new Vector3(Screen.width*((this.sortID-1)*0.10f),0.07f*h,0);
				this.playingCardView.playingCardVM.infoRect = new Rect(this.playingCardView.playingCardVM.ScreenPosition.x-0.06f*Screen.width, this.playingCardView.playingCardVM.ScreenPosition.y-0.07f*Screen.height, 0.12f*Screen.width, 0.14f*Screen.height);
			}
			else{
				this.playingCardView.playingCardVM.ScreenPosition = new Vector3(Screen.width*((this.sortID-1)*0.13f+0.06f+0.15f),0.07f*h,0);
				this.playingCardView.playingCardVM.infoRect = new Rect(this.playingCardView.playingCardVM.ScreenPosition.x-0.06f*Screen.width, this.playingCardView.playingCardVM.ScreenPosition.y-0.07f*Screen.height, 0.12f*Screen.width, 0.14f*Screen.height);
			}
			if (GameController.instance.isFirstPlayer){
				this.playingCardView.playingCardVM.position = Camera.main.ScreenToWorldPoint(this.playingCardView.playingCardVM.ScreenPosition);
				this.playingCardView.playingCardVM.position.y = 5f ;
			}
			else{
				this.playingCardView.playingCardVM.position = Camera.main.ScreenToWorldPoint(this.playingCardView.playingCardVM.ScreenPosition);
				this.playingCardView.playingCardVM.position.y = 5-this.playingCardView.playingCardVM.position.y;
				this.playingCardView.playingCardVM.position.y = -5f ;
			}
		}

		this.playingCardView.playingCardVM.nameTextStyle.fontSize = h * 18 / 1000 ;
		this.playingCardView.playingCardVM.attackZoneTextStyle.fontSize = h * 15 / 1000 ;
		this.playingCardView.playingCardVM.lifeBarStyle.fontSize = h * 15 / 1000 ;
		this.playingCardView.playingCardVM.QuicknessBarStyle.fontSize = h * 15 / 1000 ;
		this.playingCardView.playingCardVM.skillTitleTextStyle.fontSize = h * 16 / 1000 ;
		this.playingCardView.playingCardVM.skillDescriptionTextStyle.fontSize = h * 12 / 1000 ;
		this.playingCardView.playingCardVM.skillInfoRectWidth = 0.12f*Screen.width;
		
		this.playingCardView.replace();
	}

	public void resizeInfoRect(){
		if(this.isMine){
			if (this.isSelected){
				this.playingCardView.playingCardVM.ScreenPosition.x = Screen.width*((this.sortID-1)*0.13f+0.12f+0.21f);
				this.playingCardView.playingCardVM.infoRect.x = this.playingCardView.playingCardVM.ScreenPosition.x-0.12f*Screen.width;
				this.playingCardView.playingCardVM.infoRect.width = 0.24f*Screen.width;
				
				this.playingCardView.playingCardVM.isSelected = true ;
			}
			else if (this.isMoved){
				this.playingCardView.playingCardVM.ScreenPosition.x = Screen.width*((this.sortID-1)*0.13f+0.18f+0.21f);
				this.playingCardView.playingCardVM.infoRect.x = this.playingCardView.playingCardVM.ScreenPosition.x-0.06f*Screen.width;
				this.playingCardView.playingCardVM.infoRect.width = 0.12f*Screen.width;
			}
			else{
				this.playingCardView.playingCardVM.ScreenPosition.x = Screen.width*((this.sortID-1)*0.13f+0.06f+0.21f);
				this.playingCardView.playingCardVM.infoRect.x = this.playingCardView.playingCardVM.ScreenPosition.x-0.06f*Screen.width;
				this.playingCardView.playingCardVM.infoRect.width = 0.12f*Screen.width;
			}
			this.playingCardView.playingCardVM.position = Camera.main.ScreenToWorldPoint(this.playingCardView.playingCardVM.ScreenPosition);
			this.playingCardView.playingCardVM.position.y = -5f ;
		}
		else{
			if (this.isSelected){
				this.playingCardView.playingCardVM.ScreenPosition.x = Screen.width*((this.sortID-1)*0.13f+0.15f);
				this.playingCardView.playingCardVM.infoRect.x = this.playingCardView.playingCardVM.ScreenPosition.x-0.12f*Screen.width;
				this.playingCardView.playingCardVM.infoRect.width = 0.24f*Screen.width;
			}
			else if (this.isMoved){
				this.playingCardView.playingCardVM.ScreenPosition.x = Screen.width*((this.sortID-1)*0.13f+0.09f);
				this.playingCardView.playingCardVM.infoRect.x = this.playingCardView.playingCardVM.ScreenPosition.x-0.06f*Screen.width;
				this.playingCardView.playingCardVM.infoRect.width = 0.12f*Screen.width;
			}
			else{
				this.playingCardView.playingCardVM.ScreenPosition.x = Screen.width*((this.sortID-1)*0.13f+0.21f);
				this.playingCardView.playingCardVM.infoRect.x = this.playingCardView.playingCardVM.ScreenPosition.x-0.06f*Screen.width;
				this.playingCardView.playingCardVM.infoRect.width = 0.12f*Screen.width;
			}
			this.playingCardView.playingCardVM.position = Camera.main.ScreenToWorldPoint(this.playingCardView.playingCardVM.ScreenPosition);
			this.playingCardView.playingCardVM.position.y = 5f ;
		}

		this.playingCardView.playingCardVM.position.z = 0f ;
		this.playingCardView.replace();
	}

	public void setIDCharacter(int i){
		this.IDCharacter = i ;
	}

	public void setSortID(int i, int speed){
		this.sortID = i ;
		this.playingCardView.playingCardVM.quickness = speed ;
	}

	public void getDamage()
	{
		//GameController.instance.inflictDamage(ID);
	}

	public void updateAttack(){
		int attack = this.card.Attack ;
		int bonus = 0 ;
		for (int i = 0 ; i < this.statModifiers.Count ; i++){
			if (statModifiers[i].Stat == 0 && statModifiers[i].Type == 0){
				bonus += statModifiers[i].Amount ;
			}
		}
		this.playingCardView.playingCardVM.attack += attack+bonus ;
		if (bonus>0){
			//this.playingCardView.playingCardVM.attackZoneTextStyle = guiStylesMyCharacter[3];
		}
		else if (bonus>0){
			//this.playingCardView.playingCardVM.attackZoneTextStyle = guiStylesMyCharacter[4];
		}
		else{
			this.playingCardView.playingCardVM.attackZoneTextStyle = guiStylesMyCharacter[2];
		}
	}

	public void updateMove(){
		int move = this.card.Move ;
		int bonus = 0 ;
		for (int i = 0 ; i < this.statModifiers.Count ; i++){
			if (statModifiers[i].Stat == 1 && statModifiers[i].Type == 0){
				bonus += statModifiers[i].Amount ;
			}
		}
		this.playingCardView.playingCardVM.move += move+bonus ;
		if (bonus>0){
			//this.playingCardView.playingCardVM.moveZoneTextStyle = guiStylesMyCharacter[3];
		}
		else if (bonus>0){
			//this.playingCardView.playingCardVM.moveZoneTextStyle = guiStylesMyCharacter[4];
		}
		else{
			this.playingCardView.playingCardVM.attackZoneTextStyle = guiStylesMyCharacter[2];
		}
	}

	public void updateQuickness(){
		int speed = this.card.Speed ;
		int bonus = 0 ;
		for (int i = 0 ; i < this.statModifiers.Count ; i++){
			if (statModifiers[i].Stat == 2 && statModifiers[i].Type == 0){
				bonus += statModifiers[i].Amount ;
			}
		}
		this.playingCardView.playingCardVM.quickness += speed+bonus ;
		if (bonus>0){
			//this.playingCardView.playingCardVM.quicknessZoneTextStyle = guiStylesMyCharacter[3];
		}
		else if (bonus>0){
			//this.playingCardView.playingCardVM.quicknessZoneTextStyle = guiStylesMyCharacter[4];
		}
		else{
			this.playingCardView.playingCardVM.attackZoneTextStyle = guiStylesMyCharacter[2];
		}
	}

	public void hoverPlayingCard(){
		GameController.instance.hoverPlayingCard(this.IDCharacter);
	}

	public void clickPlayingCard(){
		GameController.instance.clickPlayingCard(this.IDCharacter);
	}

	public void displayHover(){
		if(this.isMine){
			this.playingCardView.playingCardVM.backgroundStyle = guiStylesMyCharacter[3];
		}
		else{
			this.playingCardView.playingCardVM.backgroundStyle = guiStylesHisCharacter[3];
		}
	}

	public void displayClick(){
		if(this.isMine){
			this.playingCardView.playingCardVM.backgroundStyle = guiStylesMyCharacter[4];
		}
		else{
			this.playingCardView.playingCardVM.backgroundStyle = guiStylesHisCharacter[4];
		}
	}

	public void displayPlaying(){
		if(this.isMine){
			this.playingCardView.playingCardVM.backgroundStyle = guiStylesMyCharacter[12];
			this.playingCardView.playingCardVM.isPlaying = true ;
		}
		else{
			this.playingCardView.playingCardVM.backgroundStyle = guiStylesHisCharacter[12];
		}
	}

	public void hideHover(){
		if(this.isMine){
			this.playingCardView.playingCardVM.backgroundStyle = guiStylesMyCharacter[0];
		}
		else{
			this.playingCardView.playingCardVM.backgroundStyle = guiStylesHisCharacter[0];
		}
	}

	public void hidePlaying(){
		if(this.isMine){
			this.playingCardView.playingCardVM.backgroundStyle = guiStylesMyCharacter[0];
			this.playingCardView.playingCardVM.isPlaying = false ;
		}
		else{
			this.playingCardView.playingCardVM.backgroundStyle = guiStylesHisCharacter[0];
		}
		this.isSelected = false ;
		this.resizeInfoRect();
		print ("Je hide dans playingCarsd");
	}

	public void pass(){
		GameController.instance.passHandler();
	}


//	void Start () {
//		animator = transform.parent.GetComponent<Animator> ();
//		bounds = renderer.bounds;
//	}
//	
//	public void setRectStats(float x, float y, float sizeX, float sizeY){
//		stats = new Rect(x, y, sizeX, sizeY);
//	}
//	
//	public void setGnCard(PlayingCardView playingCardV)
//	{
//		this.playingCardV = playingCardV;
//		toHide = false;
//	}
//	
//	void OnMouseExit()
//	{
//		isHovered = false ;
//		//		if (GameBoard.instance.isDragging)
//		//		{
//		//			if (!this.Equals(GameBoard.instance.CardSelected))
//		//			{
//		//				GameTile.instance.SetCursorToExchange();
//		//			} else
//		//			{
//		//				GameTile.instance.SetCursorToDrag();
//		//			}
//		//		}
//		//		
//		//		if (this.gameCard.card != null)
//		//		{
//		//			GameHoveredCard.instance.ChangeCard(this);
//		//		}
//	}
//	
//	void OnMouseDown() 
//	{
//		//		if (gameCard.photonView.isMine)
//		//		{
//		//			GameBoard.instance.CardSelected = this;
//		//			GameBoard.instance.isDragging = true;
//		//			GameTile.instance.SetCursorToDrag();
//		//		} 
//	}
//	
//	//			else
//	//			{
//	//				GameTile.InitIndexPathTile();
//	//				if (GameTimeLine.instance.PlayingCard.gameCard.card.Equals(gameCard.card) && !GamePlayingCard.instance.hasMoved) 
//	//				{
//	//					GameBoard.instance.CardSelected = this;
//	//					GameBoard.instance.isMoving = true;
//	//
//	//					RaycastHit hit;
//	//					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//	//					
//	//					if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << GameBoardGenerator.instance.GridLayerMask))
//	//					{
//	//						currentTile = hit.transform.gameObject.GetComponent<GameTile>();
//	//					}
//	//				}
//	//				if (GameTimeLine.instance.PlayingCard.gameCard.card.Equals(gameCard.card) && GamePlayingCard.instance.hasMoved) 
//	//				{
//	//					StartCoroutine(ChangeMessage("la carte s'est déjà déplacée pendant ce tour"));
//	//				}
//	//				Vector2 gridPosition = this.CalcGridPos();
//	//				if (currentTile != null)
//	//				{
//	//					currentTile.Passable = true;
//	//				}
//	//				//Tile tile = GameBoard.instance.board[new Point((int)gridPosition.x, (int)gridPosition.y)];
//	//				//colorAndMarkNeighboringTiles(tile.AllNeighbours, gameCard.Card.Move, Color.gray);
//	//			}                         
//	//
//	//		if (!GameTimeLine.instance.PlayingCard.Equals(this) && GamePlayingCard.instance.attemptToAttack && !GamePlayingCard.instance.hasAttacked) 
//	//		{
//	//			if (GameTimeLine.instance.PlayingCard.neighbors.Find(e => e.gameCard.card.Equals(this.gameCard.card)))
//	//			{
//	//				DiscoveryFeature.Life = true;
//	//				gameCard.photonView.RPC("GetDamage", PhotonTargets.AllBuffered, this.GetComponent<PhotonView>().viewID, GameTimeLine.instance.PlayingCard.gameCard.card.Attack);
//	//				GameTimeLine.instance.PlayingCard.GetComponent<GameNetworkCard>().DiscoveryFeature.Attack = true;
//	//				GamePlayingCard.instance.attemptToAttack = false;
//	//				GamePlayingCard.instance.hasAttacked = true;
//	//				GameTile.instance.SetCursorToDefault();
//	//				if (GamePlayingCard.instance.hasMoved && GamePlayingCard.instance.hasAttacked)
//	//				{
//	//					GamePlayingCard.instance.Pass();
//	//				}
//	//			}
//	//		}
//	//		if (GamePlayingCard.instance.attemptToCast && !GamePlayingCard.instance.hasAttacked)
//	//		{
//	//			gameCard.photonView.RPC("GetBuff", PhotonTargets.AllBuffered, this.GetComponent<PhotonView>().viewID, GamePlayingCard.instance.SkillCasted);
//	//			GamePlayingCard.instance.attemptToCast = false;
//	//			GamePlayingCard.instance.hasAttacked = true;
//	//			GameTile.instance.SetCursorToDefault();
//	//		}
//	//	}
//	//
//	//	void OnMouseExit()
//	//	{
//	//		if (GameBoard.instance.TimeOfPositionning)
//	//		{
//	//			if (gameCard.photonView.isMine)
//	//			{
//	//				if (GameBoard.instance.isDragging)
//	//				{
//	//					GameBoard.instance.CardHovered = null;
//	//				}
//	//			}
//	//		}
//	//		GameHoveredCard.instance.hide();
//	//	}
//	//	void OnMouseUp()
//	//	{
//	//		if (GameBoard.instance.TimeOfPositionning)
//	//		{
//	//			if (gameCard.photonView.isMine)
//	//			{
//	//				if (GameBoard.instance.isDragging)
//	//				{
//	//					if (!this.Equals(GameBoard.instance.CardHovered) && GameBoard.instance.CardHovered)
//	//					{
//	//						Vector3 temp = this.transform.position;
//	//						this.transform.position = GameBoard.instance.CardHovered.transform.position;
//	//						GameBoard.instance.CardHovered.transform.position = temp;
//	//					}
//	//					if (GameBoard.instance.CardHovered == null)
//	//					{
//	//						GameBoard.instance.droppedCard = true;
//	//					}
//	//					GameBoard.instance.isDragging = false;
//	//					GameTile.instance.SetCursorToDefault();
//	//				}
//	//			}
//	//		}
//	//		else
//	//		{
//	//			GameTile.RemovePassableTile();
//	//			
//	//			//this.FindNeighbors();
//	//			if (gameCard.photonView.isMine)
//	//			{
//	//				GameBoard.instance.isMoving = false;
//	//				GameBoard.instance.CardSelected = null;
//	//			}
//	//			if (GamePlayingCard.instance.attemptToMoveTo != null)
//	//			{
//	//				int nbTiles = CalcNbTiles(currentTile, GamePlayingCard.instance.attemptToMoveTo);
//	//				if (nbTiles == gameCard.card.GetMove())
//	//				{
//	//					DiscoveryFeature.Move = true;
//	//				}
//	//				else{
//	//					DiscoveryFeature.MoveMin = nbTiles;
//	//				}
//	//				GamePlayingCard.instance.attemptToMoveTo = null;
//	//				GamePlayingCard.instance.hasMoved = true;
//	//
//	//			}
//	//			if (GamePlayingCard.instance.hasMoved && GamePlayingCard.instance.hasAttacked)
//	//			{
//	//				GamePlayingCard.instance.Pass();
//	//			}
//	//			
//	//		}
//	//	}
//	
//	public void hideInformations(){
//		toHide=true;
//	}
//	
//	public void showInformations(){
//		toHide=false;
//	}
//	
//	public void toWalk(){
//		animator.SetBool("isWalking",true );
//	}
//	
//	public void stopWalking(){
//		animator.SetBool("isWalking",false );
//	}
//	
//	
//	//	void OnMouseDown() 
//	//	{
//	//		if (gameCard.photonView.isMine)
//	//		{
//	//			GameBoard.instance.CardSelected = this;
//	//			GameBoard.instance.isDragging = true;
//	//			GameTile.instance.SetCursorToDrag();
//	//		} 
//	//			else
//	//			{
//	//				GameTile.InitIndexPathTile();
//	//				if (GameTimeLine.instance.PlayingCard.gameCard.card.Equals(gameCard.card) && !GamePlayingCard.instance.hasMoved) 
//	//				{
//	//					GameBoard.instance.CardSelected = this;
//	//					GameBoard.instance.isMoving = true;
//	//
//	//					RaycastHit hit;
//	//					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//	//					
//	//					if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << GameBoardGenerator.instance.GridLayerMask))
//	//					{
//	//						currentTile = hit.transform.gameObject.GetComponent<GameTile>();
//	//					}
//	//				}
//	//				if (GameTimeLine.instance.PlayingCard.gameCard.card.Equals(gameCard.card) && GamePlayingCard.instance.hasMoved) 
//	//				{
//	//					StartCoroutine(ChangeMessage("la carte s'est déjà déplacée pendant ce tour"));
//	//				}
//	//				Vector2 gridPosition = this.CalcGridPos();
//	//				if (currentTile != null)
//	//				{
//	//					currentTile.Passable = true;
//	//				}
//	//				//Tile tile = GameBoard.instance.board[new Point((int)gridPosition.x, (int)gridPosition.y)];
//	//				//colorAndMarkNeighboringTiles(tile.AllNeighbours, gameCard.Card.Move, Color.gray);
//	//			}                         
//	//
//	//		if (!GameTimeLine.instance.PlayingCard.Equals(this) && GamePlayingCard.instance.attemptToAttack && !GamePlayingCard.instance.hasAttacked) 
//	//		{
//	//			if (GameTimeLine.instance.PlayingCard.neighbors.Find(e => e.gameCard.card.Equals(this.gameCard.card)))
//	//			{
//	//				DiscoveryFeature.Life = true;
//	//				gameCard.photonView.RPC("GetDamage", PhotonTargets.AllBuffered, this.GetComponent<PhotonView>().viewID, GameTimeLine.instance.PlayingCard.gameCard.card.Attack);
//	//				GameTimeLine.instance.PlayingCard.GetComponent<GameNetworkCard>().DiscoveryFeature.Attack = true;
//	//				GamePlayingCard.instance.attemptToAttack = false;
//	//				GamePlayingCard.instance.hasAttacked = true;
//	//				GameTile.instance.SetCursorToDefault();
//	//				if (GamePlayingCard.instance.hasMoved && GamePlayingCard.instance.hasAttacked)
//	//				{
//	//					GamePlayingCard.instance.Pass();
//	//				}
//	//			}
//	//		}
//	//		if (GamePlayingCard.instance.attemptToCast && !GamePlayingCard.instance.hasAttacked)
//	//		{
//	//			gameCard.photonView.RPC("GetBuff", PhotonTargets.AllBuffered, this.GetComponent<PhotonView>().viewID, GamePlayingCard.instance.SkillCasted);
//	//			GamePlayingCard.instance.attemptToCast = false;
//	//			GamePlayingCard.instance.hasAttacked = true;
//	//			GameTile.instance.SetCursorToDefault();
//	//		}
//	//	}
//	//
//	//	void OnMouseExit()
//	//	{
//	//		if (GameBoard.instance.TimeOfPositionning)
//	//		{
//	//			if (gameCard.photonView.isMine)
//	//			{
//	//				if (GameBoard.instance.isDragging)
//	//				{
//	//					GameBoard.instance.CardHovered = null;
//	//				}
//	//			}
//	//		}
//	//		GameHoveredCard.instance.hide();
//	//	}
//	//	void OnMouseUp()
//	//	{
//	//		if (GameBoard.instance.TimeOfPositionning)
//	//		{
//	//			if (gameCard.photonView.isMine)
//	//			{
//	//				if (GameBoard.instance.isDragging)
//	//				{
//	//					if (!this.Equals(GameBoard.instance.CardHovered) && GameBoard.instance.CardHovered)
//	//					{
//	//						Vector3 temp = this.transform.position;
//	//						this.transform.position = GameBoard.instance.CardHovered.transform.position;
//	//						GameBoard.instance.CardHovered.transform.position = temp;
//	//					}
//	//					if (GameBoard.instance.CardHovered == null)
//	//					{
//	//						GameBoard.instance.droppedCard = true;
//	//					}
//	//					GameBoard.instance.isDragging = false;
//	//					GameTile.instance.SetCursorToDefault();
//	//				}
//	//			}
//	//		}
//	//		else
//	//		{
//	//			GameTile.RemovePassableTile();
//	//			
//	//			//this.FindNeighbors();
//	//			if (gameCard.photonView.isMine)
//	//			{
//	//				GameBoard.instance.isMoving = false;
//	//				GameBoard.instance.CardSelected = null;
//	//			}
//	//			if (GamePlayingCard.instance.attemptToMoveTo != null)
//	//			{
//	//				int nbTiles = CalcNbTiles(currentTile, GamePlayingCard.instance.attemptToMoveTo);
//	//				if (nbTiles == gameCard.card.GetMove())
//	//				{
//	//					DiscoveryFeature.Move = true;
//	//				}
//	//				else{
//	//					DiscoveryFeature.MoveMin = nbTiles;
//	//				}
//	//				GamePlayingCard.instance.attemptToMoveTo = null;
//	//				GamePlayingCard.instance.hasMoved = true;
//	//
//	//			}
//	//			if (GamePlayingCard.instance.hasMoved && GamePlayingCard.instance.hasAttacked)
//	//			{
//	//				GamePlayingCard.instance.Pass();
//	//			}
//	//			
//	//		}
//	//	}

//	GUIStyle statsZoneStyle = new GUIStyle();
//	GUIStyle NamePoliceStyle = new GUIStyle();
//	GUIStyle LifePoliceStyle = new GUIStyle();
//	GUIStyle SpeedPoliceStyle = new GUIStyle();
//	GUIStyle AttackPoliceStyle = new GUIStyle();
//	GUIStyle MovePoliceStyle = new GUIStyle();
//	GUIStyle lifebarPoliceStyle = new GUIStyle();
//	GUIStyle skillInfoStyle = new GUIStyle();
//
//	Texture2D attackIcon ;
//	Texture2D quicknessIcon ;
//	Texture2D moveIcon ;
//	public int x ;
//	public int y ;
//
//	bool isFocused = false ;
//	public bool isHovered = false ;
//
//	float life ;
//
//	public int widthScreen = Screen.width; 
//	int heightScreen = Screen.height;
//	Bounds bounds;
//	bool isStyleSet = false ;
//
//	public Rect stats = new Rect(0,0,0,0); 
//
//	Animator animator;
//
//	public bool toHide=true;
//
//	// Use this for initialization
//
//	void Start () {
//		animator = transform.parent.GetComponent<Animator> ();
//		bounds = renderer.bounds;
//	}
//
//	public void setRectStats(float x, float y, float sizeX, float sizeY){
//		stats = new Rect(x, y, sizeX, sizeY);
//	}
//
//	public void setGnCard(GameNetworkCard gnc){
//		this.gnCard = gnc ;
//		toHide = false ;
//	}
//
//	
//	// Update is called once per frame
//	void Update () {
//		if (Screen.width != widthScreen || Screen.height != heightScreen) {
//			this.setStyles();
//		}
//	}
//
//	void OnGUI ()
//	{
//		if (!toHide){
//		if(gnCard.isLoaded){
//
//			bounds = renderer.bounds;
//			foreach(var r in GetComponentsInChildren<Renderer>())
//			{
//				bounds.Encapsulate(r.bounds);
//			}
//				
//			GUILayout.BeginArea (stats);
//			{
//				GUILayout.BeginHorizontal(statsZoneStyle, GUILayout.Width(stats.width));
//				{
//					GUILayout.Label("", lifebarPoliceStyle, GUILayout.Width(stats.width*gnCard.currentLife/this.gnCard.card.Life), GUILayout.Height(stats.height/2));
//				}
//				GUILayout.EndHorizontal();
//			}
//			GUILayout.EndArea ();
//
//			GUILayout.BeginArea (stats);
//				{
//					GUILayout.BeginVertical();
//					{
//					if (!isFocused){
//						if(GUILayout.Button (this.gnCard.card.Title,NamePoliceStyle)){
//								isFocused = true ;
//							}
//						}
//					else{
//						if (GUILayout.Button (gnCard.currentLife+"/"+this.gnCard.card.Life,LifePoliceStyle)){
//								isFocused = false ;
//						}
//					}
//					GUILayout.BeginHorizontal(statsZoneStyle);
//						{
//						GUILayout.FlexibleSpace();
//						GUILayout.BeginVertical();
//							{
//								GUILayout.FlexibleSpace();
//								GUILayout.Box (attackIcon, AttackPoliceStyle);
//								GUILayout.FlexibleSpace();
//							}
//						GUILayout.EndVertical();
//						GUILayout.FlexibleSpace();
//						GUILayout.Label (""+gnCard.currentAttack,MovePoliceStyle);
//						GUILayout.FlexibleSpace();
//						GUILayout.BeginVertical();
//							{
//								GUILayout.FlexibleSpace();
//								GUILayout.Box (quicknessIcon,AttackPoliceStyle);
//								GUILayout.FlexibleSpace();
//							}
//						GUILayout.EndVertical();
//						GUILayout.FlexibleSpace();
//						GUILayout.Label (""+gnCard.currentSpeed,MovePoliceStyle);
//						GUILayout.FlexibleSpace();
//						GUILayout.BeginVertical();
//							{
//								GUILayout.FlexibleSpace();
//								GUILayout.Box (moveIcon, AttackPoliceStyle);
//								GUILayout.FlexibleSpace();
//							}
//						GUILayout.EndVertical();
//						GUILayout.FlexibleSpace();
//						GUILayout.Label (""+gnCard.currentMove,MovePoliceStyle);
//						GUILayout.FlexibleSpace();
//						}
//						GUILayout.EndHorizontal();
//						
//					}
//					GUILayout.EndVertical();
//				}
//				GUILayout.EndArea ();
//
//			if (isHovered){
//				int j = 0 ;
//				GUI.depth=2;
//				for (int i = 0 ; i < 4 ; i++){
//					if (this.gnCard.card.Skills[i].IsActivated==1){
//						GUI.Label (new Rect(stats.x, stats.yMax+j*stats.height/2, stats.width, stats.height/2), this.gnCard.card.Skills[i].Level+"."+this.gnCard.card.Skills[i].Name, skillInfoStyle);
//						j++ ; 
//					}
//				}
//				GUI.depth=0;
//			}
//		}
//	}
//	}
//
//	public void setStyles(GUIStyle name, GUIStyle life, GUIStyle lifebar, GUIStyle attack, GUIStyle move, GUIStyle quickness, GUIStyle zone, Texture2D attackI, Texture2D quicknessI, Texture2D moveI, GUIStyle skillInfo) {
//		heightScreen = Screen.height;
//		widthScreen = Screen.width;
//		this.NamePoliceStyle = name;
//		this.SpeedPoliceStyle = quickness ;
//		this.AttackPoliceStyle = attack ;
//		this.MovePoliceStyle = move ;
//		this.LifePoliceStyle = life ;
//		this.lifebarPoliceStyle = lifebar;
//		this.statsZoneStyle = zone ;
//		this.NamePoliceStyle.fontSize = heightScreen*15/1000;
//		this.SpeedPoliceStyle.fontSize = heightScreen*15/1000;
//		this.SpeedPoliceStyle.fixedHeight = (int)heightScreen*2/100;
//		this.AttackPoliceStyle.fontSize = heightScreen*15/1000;
//		this.AttackPoliceStyle.fixedHeight = heightScreen/60;
//		this.AttackPoliceStyle.fixedWidth = heightScreen/60;
//		this.MovePoliceStyle.fontSize = heightScreen*15/1000;
//		this.LifePoliceStyle.fontSize = heightScreen*15/1000;
//		this.attackIcon = attackI;
//		this.skillInfoStyle = skillInfo;
//		this.skillInfoStyle.fontSize=heightScreen*15/1000;
//		this.moveIcon = moveI ;
//		this.quicknessIcon = quicknessI ;
//
//		isStyleSet = true ;
//	}
//
//	private void setStyles() {
//		if (isStyleSet){
//			heightScreen = Screen.height;
//			widthScreen = Screen.width;
//			this.NamePoliceStyle.fontSize = heightScreen*15/1000;
//			this.SpeedPoliceStyle.fontSize = heightScreen*15/1000;
//			this.SpeedPoliceStyle.fixedHeight = (int)heightScreen*2/100;
//			this.AttackPoliceStyle.fontSize = heightScreen*15/1000;
//			this.AttackPoliceStyle.fixedHeight = heightScreen/60;
//			this.AttackPoliceStyle.fixedWidth = heightScreen/60;
//			this.MovePoliceStyle.fontSize = heightScreen*15/1000;
//			this.LifePoliceStyle.fontSize = heightScreen*15/1000;
//			this.skillInfoStyle.fontSize=heightScreen*15/1000;
//		}
//	}
//
//	void OnMouseEnter()
//	{
//		isHovered = true ;
////		if (GameBoard.instance.isDragging)
////		{
////			if (!this.Equals(GameBoard.instance.CardSelected))
////			{
////				GameTile.instance.SetCursorToExchange();
////			} else
////			{
////				GameTile.instance.SetCursorToDrag();
////			}
////		}
////		
////		if (this.gameCard.card != null)
////		{
////			GameHoveredCard.instance.ChangeCard(this);
////		}
//	}
//
//	void OnMouseExit()
//	{
//		isHovered = false ;
//		//		if (GameBoard.instance.isDragging)
//		//		{
//		//			if (!this.Equals(GameBoard.instance.CardSelected))
//		//			{
//		//				GameTile.instance.SetCursorToExchange();
//		//			} else
//		//			{
//		//				GameTile.instance.SetCursorToDrag();
//		//			}
//		//		}
//		//		
//		//		if (this.gameCard.card != null)
//		//		{
//		//			GameHoveredCard.instance.ChangeCard(this);
//		//		}
//	}
//
//	void OnMouseDown() 
//	{
////		if (gameCard.photonView.isMine)
////		{
////			GameBoard.instance.CardSelected = this;
////			GameBoard.instance.isDragging = true;
////			GameTile.instance.SetCursorToDrag();
////		} 
//	}
//		
//	//			else
//	//			{
//	//				GameTile.InitIndexPathTile();
//	//				if (GameTimeLine.instance.PlayingCard.gameCard.card.Equals(gameCard.card) && !GamePlayingCard.instance.hasMoved) 
//	//				{
//	//					GameBoard.instance.CardSelected = this;
//	//					GameBoard.instance.isMoving = true;
//	//
//	//					RaycastHit hit;
//	//					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//	//					
//	//					if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << GameBoardGenerator.instance.GridLayerMask))
//	//					{
//	//						currentTile = hit.transform.gameObject.GetComponent<GameTile>();
//	//					}
//	//				}
//	//				if (GameTimeLine.instance.PlayingCard.gameCard.card.Equals(gameCard.card) && GamePlayingCard.instance.hasMoved) 
//	//				{
//	//					StartCoroutine(ChangeMessage("la carte s'est déjà déplacée pendant ce tour"));
//	//				}
//	//				Vector2 gridPosition = this.CalcGridPos();
//	//				if (currentTile != null)
//	//				{
//	//					currentTile.Passable = true;
//	//				}
//	//				//Tile tile = GameBoard.instance.board[new Point((int)gridPosition.x, (int)gridPosition.y)];
//	//				//colorAndMarkNeighboringTiles(tile.AllNeighbours, gameCard.Card.Move, Color.gray);
//	//			}                         
//	//
//	//		if (!GameTimeLine.instance.PlayingCard.Equals(this) && GamePlayingCard.instance.attemptToAttack && !GamePlayingCard.instance.hasAttacked) 
//	//		{
//	//			if (GameTimeLine.instance.PlayingCard.neighbors.Find(e => e.gameCard.card.Equals(this.gameCard.card)))
//	//			{
//	//				DiscoveryFeature.Life = true;
//	//				gameCard.photonView.RPC("GetDamage", PhotonTargets.AllBuffered, this.GetComponent<PhotonView>().viewID, GameTimeLine.instance.PlayingCard.gameCard.card.Attack);
//	//				GameTimeLine.instance.PlayingCard.GetComponent<GameNetworkCard>().DiscoveryFeature.Attack = true;
//	//				GamePlayingCard.instance.attemptToAttack = false;
//	//				GamePlayingCard.instance.hasAttacked = true;
//	//				GameTile.instance.SetCursorToDefault();
//	//				if (GamePlayingCard.instance.hasMoved && GamePlayingCard.instance.hasAttacked)
//	//				{
//	//					GamePlayingCard.instance.Pass();
//	//				}
//	//			}
//	//		}
//	//		if (GamePlayingCard.instance.attemptToCast && !GamePlayingCard.instance.hasAttacked)
//	//		{
//	//			gameCard.photonView.RPC("GetBuff", PhotonTargets.AllBuffered, this.GetComponent<PhotonView>().viewID, GamePlayingCard.instance.SkillCasted);
//	//			GamePlayingCard.instance.attemptToCast = false;
//	//			GamePlayingCard.instance.hasAttacked = true;
//	//			GameTile.instance.SetCursorToDefault();
//	//		}
//	//	}
//	//
//	//	void OnMouseExit()
//	//	{
//	//		if (GameBoard.instance.TimeOfPositionning)
//	//		{
//	//			if (gameCard.photonView.isMine)
//	//			{
//	//				if (GameBoard.instance.isDragging)
//	//				{
//	//					GameBoard.instance.CardHovered = null;
//	//				}
//	//			}
//	//		}
//	//		GameHoveredCard.instance.hide();
//	//	}
//	//	void OnMouseUp()
//	//	{
//	//		if (GameBoard.instance.TimeOfPositionning)
//	//		{
//	//			if (gameCard.photonView.isMine)
//	//			{
//	//				if (GameBoard.instance.isDragging)
//	//				{
//	//					if (!this.Equals(GameBoard.instance.CardHovered) && GameBoard.instance.CardHovered)
//	//					{
//	//						Vector3 temp = this.transform.position;
//	//						this.transform.position = GameBoard.instance.CardHovered.transform.position;
//	//						GameBoard.instance.CardHovered.transform.position = temp;
//	//					}
//	//					if (GameBoard.instance.CardHovered == null)
//	//					{
//	//						GameBoard.instance.droppedCard = true;
//	//					}
//	//					GameBoard.instance.isDragging = false;
//	//					GameTile.instance.SetCursorToDefault();
//	//				}
//	//			}
//	//		}
//	//		else
//	//		{
//	//			GameTile.RemovePassableTile();
//	//			
//	//			//this.FindNeighbors();
//	//			if (gameCard.photonView.isMine)
//	//			{
//	//				GameBoard.instance.isMoving = false;
//	//				GameBoard.instance.CardSelected = null;
//	//			}
//	//			if (GamePlayingCard.instance.attemptToMoveTo != null)
//	//			{
//	//				int nbTiles = CalcNbTiles(currentTile, GamePlayingCard.instance.attemptToMoveTo);
//	//				if (nbTiles == gameCard.card.GetMove())
//	//				{
//	//					DiscoveryFeature.Move = true;
//	//				}
//	//				else{
//	//					DiscoveryFeature.MoveMin = nbTiles;
//	//				}
//	//				GamePlayingCard.instance.attemptToMoveTo = null;
//	//				GamePlayingCard.instance.hasMoved = true;
//	//
//	//			}
//	//			if (GamePlayingCard.instance.hasMoved && GamePlayingCard.instance.hasAttacked)
//	//			{
//	//				GamePlayingCard.instance.Pass();
//	//			}
//	//			
//	//		}
//	//	}
//
//	public void hideInformations(){
//		toHide=true;
//	}
//
//	public void showInformations(){
//		toHide=false;
//	}
//
//	public void toWalk(){
//		animator.SetBool("isWalking",true );
//	}
//
//	public void stopWalking(){
//		animator.SetBool("isWalking",false );
//	}
}


