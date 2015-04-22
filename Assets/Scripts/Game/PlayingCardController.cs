using UnityEngine;

public class PlayingCardController : MonoBehaviour
{
	private PlayingCardView playingCardView;
	public GUIStyle[] guiStylesMyCharacter ;
	public GUIStyle[] guiStylesHisCharacter ;
	public Texture2D[] icons ;
	private float scale ;
	public GameObject tile ;
	private Card card ;
	public int ID = -1 ;
	public bool isMovable ;

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

	void Awake()
	{
		this.playingCardView = gameObject.transform.parent.gameObject.AddComponent <PlayingCardView>();
		this.playingCardView.playingCardVM.scale = new Vector3(0, 0, 0);
		this.playingCardView.playingCardVM.attackIcon = icons[0];
		this.playingCardView.playingCardVM.moveIcon = icons[1];
		this.playingCardView.playingCardVM.quicknessIcon = icons[2];
		this.isMovable = true ;
	}

	public void setTile(GameObject t, bool toRotate, bool isPlayer1)
	{
		this.tile = t ;
		this.playingCardView.playingCardVM.position = this.tile.GetComponent<TileController>().tileView.tileVM.position;
		if (!toRotate)
		{
			this.playingCardView.playingCardVM.rotation =  Quaternion.Euler(-90,0,0);
		}
		else
		{
			this.playingCardView.playingCardVM.rotation =  Quaternion.Euler(90,180,0);

			//this.playingCardView.playingCardVM.ScreenPosition.y = Screen.height-this.playingCardView.playingCardVM.ScreenPosition.y;
		}
		this.playingCardView.playingCardVM.ScreenPosition = Camera.main.WorldToScreenPoint(this.playingCardView.playingCardVM.position);
		this.playingCardView.playingCardVM.scale = new Vector3(this.scale, this.scale, this.scale);
		this.playingCardView.replace();
	}

	public void changeTile(GameObject t)
	{
		this.tile = t ;
		this.playingCardView.playingCardVM.position = this.tile.GetComponent<TileController>().tileView.tileVM.position;
		this.playingCardView.playingCardVM.ScreenPosition = Camera.main.WorldToScreenPoint(this.playingCardView.playingCardVM.position);

		this.playingCardView.playingCardVM.ScreenPosition = Camera.main.WorldToScreenPoint(this.playingCardView.playingCardVM.position);
		this.playingCardView.playingCardVM.ScreenPosition.y = Screen.height-this.playingCardView.playingCardVM.ScreenPosition.y;
		this.playingCardView.playingCardVM.infoRect = new Rect(this.playingCardView.playingCardVM.ScreenPosition.x-scale*55f, this.playingCardView.playingCardVM.ScreenPosition.y, scale*110, scale*60);

		this.playingCardView.replace();
	}

	public void setStyles(bool isMyCharacter){
		if (isMyCharacter){
			this.playingCardView.playingCardVM.backgroundStyle = guiStylesMyCharacter[0];
			this.playingCardView.playingCardVM.nameTextStyle = guiStylesMyCharacter[1];
			this.playingCardView.playingCardVM.attackZoneTextStyle = guiStylesMyCharacter[2];
			this.playingCardView.playingCardVM.moveZoneTextStyle = guiStylesMyCharacter[3];
			this.playingCardView.playingCardVM.quicknessZoneTextStyle = guiStylesMyCharacter[4];
			this.playingCardView.playingCardVM.imageStyle = guiStylesMyCharacter[5];
			this.playingCardView.playingCardVM.lifeTextStyle = guiStylesMyCharacter[6];
		}
		else{
			this.playingCardView.playingCardVM.backgroundStyle = guiStylesHisCharacter[0];
			this.playingCardView.playingCardVM.nameTextStyle = guiStylesHisCharacter[1];
			this.playingCardView.playingCardVM.attackZoneTextStyle = guiStylesHisCharacter[2];
			this.playingCardView.playingCardVM.moveZoneTextStyle = guiStylesHisCharacter[3];
			this.playingCardView.playingCardVM.quicknessZoneTextStyle = guiStylesHisCharacter[4];
			this.playingCardView.playingCardVM.imageStyle = guiStylesHisCharacter[5];
			this.playingCardView.playingCardVM.lifeTextStyle = guiStylesHisCharacter[6];

		}
	}

	public void setCard(Card c){
		this.card = c ;
		this.playingCardView.playingCardVM.name = c.Title ;
		this.playingCardView.playingCardVM.attack = ""+c.Attack ;
		this.playingCardView.playingCardVM.move = ""+c.Move ;
		this.playingCardView.playingCardVM.quickness = ""+c.Speed ;
		this.playingCardView.playingCardVM.maxLife = c.Life ;
		this.playingCardView.playingCardVM.life = c.Life ;
	}

	public void resize(int h){
		this.scale = h*0.8f/1000f ;
		this.playingCardView.playingCardVM.scale = new Vector3(this.scale, this.scale, this.scale);

		this.playingCardView.playingCardVM.ScreenPosition = Camera.main.WorldToScreenPoint(this.playingCardView.playingCardVM.position);
		this.playingCardView.playingCardVM.ScreenPosition.y = Screen.height-this.playingCardView.playingCardVM.ScreenPosition.y;
		this.playingCardView.playingCardVM.infoRect = new Rect(this.playingCardView.playingCardVM.ScreenPosition.x-scale*55f, this.playingCardView.playingCardVM.ScreenPosition.y, scale*110, scale*60);
		this.playingCardView.playingCardVM.nameTextStyle.fontSize = h * 15 / 1000 ;
		this.playingCardView.playingCardVM.attackZoneTextStyle.fontSize = h * 15 / 1000 ;
		this.playingCardView.playingCardVM.moveZoneTextStyle.fontSize = h * 15 / 1000 ;
		this.playingCardView.playingCardVM.quicknessZoneTextStyle.fontSize = h * 15 / 1000 ;
		this.playingCardView.playingCardVM.lifeTextStyle.fontSize = h * 15 / 1000 ;
		this.playingCardView.replace();
	}

	public void setID(int i){
		this.ID = i ;
	}

	public void drag(){
		if (this.isMovable){
			GameController.instance.setCharacterDragged(this.ID);
		}
	}

	public void release(){
		if (this.isMovable){
			GameController.instance.dropCharacter();
		}
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
//	


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


