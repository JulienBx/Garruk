using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameView : MonoBehaviour
{
	public BottomZoneViewModel bottomZoneVM;
	public TopZoneViewModel topZoneVM ;
	public GameScreenViewModel gameScreenVM ;

	void Awake()
	{
		gameScreenVM = new GameScreenViewModel();
		gameScreenVM.recalculate();
		bottomZoneVM = new BottomZoneViewModel();
		topZoneVM = new TopZoneViewModel();
	}
	
	void Start()
	{	
	
	}

	void Update()
	{
//		if (Screen.width != widthScreen || Screen.height != heightScreen) 
//		{
//			if (displayedHex)
//			{
//				this.setSizes();
//				this.displayGrid();
//			}
//		}
	}

	void OnGUI()
	{
		GUILayout.BeginArea(gameScreenVM.bottomZoneRect);
		{
			GUILayout.BeginHorizontal(bottomZoneVM.backgroundStyle, GUILayout.Width(gameScreenVM.bottomZoneRect.width), GUILayout.Height(gameScreenVM.bottomZoneRect.height));
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Box(bottomZoneVM.userPicture, bottomZoneVM.imageStyle, GUILayout.Width(gameScreenVM.bottomZoneRect.width * 32 / 100), GUILayout.Height(gameScreenVM.bottomZoneRect.height * 80 / 100));
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				{
					GUILayout.Label(bottomZoneVM.userName, bottomZoneVM.nameTextStyle, GUILayout.Width(gameScreenVM.bottomZoneRect.width * 62 / 100), GUILayout.Height(gameScreenVM.bottomZoneRect.height * 50 / 100));
					if (bottomZoneVM.nbTurns != -1)
					{
						GUILayout.Label("Tour : " + bottomZoneVM.nbTurns, this.bottomZoneVM.turnsTextStyle, GUILayout.Width(gameScreenVM.bottomZoneRect.width * 62 / 100), GUILayout.Height(gameScreenVM.bottomZoneRect.height * 20 / 100));
					} else
					{
						GUILayout.Label(bottomZoneVM.message, bottomZoneVM.messageTextStyle, GUILayout.Width(gameScreenVM.bottomZoneRect.width * 62 / 100), GUILayout.Height(gameScreenVM.bottomZoneRect.height * 20 / 100));
					}
					GUILayout.Space(gameScreenVM.bottomZoneRect.height * 5 / 100);

					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (bottomZoneVM.displayStartButton)
						{
							if (GUILayout.Button("Start", bottomZoneVM.buttonTextStyle, GUILayout.Width(gameScreenVM.bottomZoneRect.width * 42 / 100), GUILayout.Height(gameScreenVM.bottomZoneRect.height * 20 / 100)))
							{
								GameController.instance.StartFight();
							}
							GUILayout.Space(gameScreenVM.bottomZoneRect.width * 2 / 100);
						}
						if (GUILayout.Button("Quit", bottomZoneVM.buttonTextStyle, GUILayout.Width(gameScreenVM.bottomZoneRect.width * 17 / 100), GUILayout.Height(gameScreenVM.bottomZoneRect.height * 20 / 100)))
						{
							PhotonNetwork.Disconnect();
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(gameScreenVM.bottomZoneRect.height * 5 / 100);
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();

		GUILayout.BeginArea(gameScreenVM.topZoneRect);
		{
			GUILayout.BeginHorizontal(topZoneVM.backgroundStyle, GUILayout.Width(gameScreenVM.topZoneRect.width), GUILayout.Height(gameScreenVM.topZoneRect.height));
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Box(topZoneVM.userPicture, topZoneVM.imageStyle, GUILayout.Width(gameScreenVM.topZoneRect.width * 32 / 100), GUILayout.Height(gameScreenVM.topZoneRect.height * 80 / 100));
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				{
					GUILayout.Label(topZoneVM.userName, topZoneVM.nameTextStyle, GUILayout.Width(gameScreenVM.topZoneRect.width * 62 / 100), GUILayout.Height(gameScreenVM.topZoneRect.height * 50 / 100));
					GUILayout.Space(gameScreenVM.topZoneRect.height * 5 / 100);

					if (topZoneVM.toDisplayRedStatus)
					{
						GUILayout.Label(topZoneVM.status, topZoneVM.redStatusTextStyle, GUILayout.Width(gameScreenVM.topZoneRect.width * 62 / 100), GUILayout.Height(gameScreenVM.topZoneRect.height * 40 / 100));
					} else if (topZoneVM.toDisplayGreenStatus)
					{
						GUILayout.Label(topZoneVM.status, topZoneVM.greenStatusTextStyle, GUILayout.Width(gameScreenVM.topZoneRect.width * 62 / 100), GUILayout.Height(gameScreenVM.topZoneRect.height * 40 / 100));
					}
					GUILayout.Space(gameScreenVM.topZoneRect.height * 5 / 100);
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
		if (gameScreenVM.hasAMessage)
		{
			GUILayout.BeginArea(gameScreenVM.centerMessageRect);
			{
				GUILayout.BeginHorizontal(gameScreenVM.centerMessageTextStyle);
				{
					GUILayout.Label(gameScreenVM.messageToDisplay, gameScreenVM.centerMessageTextStyle);
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndArea();
		}
		if (gameScreenVM.timer > 0)
		{
			GUILayout.BeginArea(gameScreenVM.rightMessageRect);
			{
				GUILayout.BeginHorizontal(gameScreenVM.rightMessageTextStyle);
				{
					GUILayout.Label(Mathf.Ceil(gameScreenVM.timer).ToString(), gameScreenVM.rightMessageTextStyle);
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndArea();
		}
	}

	public void changeCursor()
	{
		Cursor.SetCursor(this.gameScreenVM.cursor, new Vector2(0, 0), CursorMode.Auto);
	}

	public void SetCursorToDefault()
	{
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}
	
	void setSizes()
	{
//		heightScreen = Screen.height;
//		widthScreen = Screen.width;
//		
//		areaPlayer1 = new Rect(0, 0.87f * heightScreen, widthScreen, heightScreen);
//		areaPlayer2 = new Rect(0, 0, widthScreen, 0.13f * heightScreen);
//		
//		playerNameStyle.fixedHeight =       heightScreen * 13 / 100;
//		playerNameStyle.fontSize =          heightScreen * 3 / 100;
//		
//		myPlayerNameStyle.fixedHeight =     heightScreen * 13 / 100;
//		myPlayerNameStyle.fontSize =        heightScreen * 3 / 100;
//		
//		enAttenteStyle.fixedHeight =        heightScreen * 13 / 100;
//		enAttenteStyle.fontSize =           heightScreen * 2 / 100;
//		
//		message1Style.fixedHeight =         heightScreen * 13 / 100;
//		message1Style.fontSize =            heightScreen * 2 / 100;
//		message2Style.fontSize =            heightScreen * 2 / 100;
	}

	public void initGrid()
	{

//		int decalage;
//		int xmax = gridWidthInHexes;
//		int ymax = gridHeightInHexes;
//		
//		for (int x = -1 * xmax / 2 ; x <= 1 * xmax / 2; x++)
//		{
//			if ((xmax - x) % 2 == 0)
//			{
//				decalage = 0;
//			}
//			else
//			{
//				decalage = 1;
//			}
//			for (int y = -1 * ymax / 2; y <= 1 * ymax / 2 - decalage; y++)
//			{
//				int type = Mathf.RoundToInt (UnityEngine.Random.Range (1, 25));
//				if (type > 4)
//				{
//					type = 0;
//				}
//				photonView.RPC("AddTile", PhotonTargets.AllBuffered, x, y, type);
//			}
//		}
	}

	
	void displayGrid()
	{
//		int decalage;
//		Debug.Log ("je redimensionne");
//		CharacterScript gCard; 
//		Vector3 pos;
//		for (int i = 0 ; i < myCards.Count ; i++)
//		{
//			gCard = myCards[i].GetComponentInChildren<CharacterScript>();
//			if (gCard.x % 2 == 0)
//			{
//				decalage = 0;
//			}
//			else
//			{
//				decalage = 1;
//			}
//			if (gCard.y > 0)
//			{
//				if (GameController.instance.isFirstPlayer)
//				{
//					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3(gCard.x*scaleTile*0.71f-scaleTile*0.42f, (gCard.y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0));
//				}
//				else
//				{
//					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3((gCard.x+1)*scaleTile*0.71f-scaleTile*0.27f, (gCard.y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0));
//				}
//			}
//			else
//			{
//				if (GameController.instance.isFirstPlayer)
//				{
//					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3(gCard.x*scaleTile*0.71f-scaleTile*0.42f, (gCard.y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.2f, 0));
//				}
//				else
//				{
//					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3((gCard.x+1)*scaleTile*0.71f-scaleTile*0.27f, (gCard.y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.4f, 0));
//				}
//			}
//			gCard.setRectStats(pos.x, heightScreen-pos.y, heightScreen *12/100, heightScreen*4/100);
//		}
//		for (int i = 0 ; i < opponentCards.Count ; i++)
//		{
//			gCard = opponentCards[i].GetComponentInChildren<CharacterScript>();
//			if (gCard.x%2==0)
//			{
//				decalage = 0;
//			}
//			else
//			{
//				decalage = 1;
//			}
//			if (gCard.y>0)
//			{
//				if (GameController.instance.isFirstPlayer)
//				{
//					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3(gCard.x*scaleTile*0.71f-scaleTile*0.42f, (gCard.y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0));
//				}
//				else
//				{
//					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3((gCard.x+1)*scaleTile*0.71f-scaleTile*0.27f, (gCard.y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0));
//				}
//			}
//			else
//			{
//				if (GameController.instance.isFirstPlayer)
//				{
//					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3(gCard.x*scaleTile*0.71f-scaleTile*0.42f, (gCard.y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.2f, 0));
//				}
//				else
//				{
//					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3((gCard.x+1)*scaleTile*0.71f-scaleTile*0.27f, (gCard.y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.4f, 0));
//				}
//			}
//			gCard.setRectStats(pos.x, heightScreen-pos.y, heightScreen *12/100, heightScreen*4/100);
//		}
//		//setRectStats(float x, float y, float sizeX, float sizeY);
//		
	}
	
	public void ArrangeCards(bool player1, Deck deck)
	{
//		int y; 
//		if (player1)
//		{
//			y = -1*gridHeightInHexes/2;
//		}
//		else
//		{
//			y = gridHeightInHexes/2;
//		}
//		for (int x = 0 ; x < 5 ; x++){
//			int viewID = PhotonNetwork.AllocateViewID();
//			photonView.RPC("SpawnCard", PhotonTargets.AllBuffered, x-2, y, deck.Cards[x].ArtIndex, viewID, deck.Cards[x].Id);
//		}
	}
	
//	[RPC]
//	IEnumerator SpawnCard(int x, int y, int type, int viewID, int cardID)
//	{
//		int decalage ;
//		GameObject clone;
//		Vector3 pos ;
//		PlayingCardView playingCardV;
//		BoxCollider bCard ;
//		
//		if (x % 2 == 0)
//		{
//			decalage = 0;
//		}
//		else
//		{
//			decalage = 1;
//		}
//		
//		if ( y > 0)
//		{
//			clone = Instantiate(characters[type], new Vector3(x*scaleTile*0.71f, (y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0), Quaternion.identity) as GameObject;
//		}
//		else
//		{
//			clone = Instantiate(characters[type], new Vector3(x*scaleTile*0.71f, (y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.2f, 0), Quaternion.identity) as GameObject;
//		}
//		playingCardV = clone.GetComponentInChildren<PlayingCardView>();
//		bCard = clone.GetComponentInChildren<BoxCollider>();
//		PlayingCardController playingCardController = clone.GetComponent<PlayingCardController>();
//		playingCardV.setGnCard(playingCardV);
//		
//		if (y > 0)
//		{
//			clone.transform.localRotation =  Quaternion.Euler(90,180,0);
//			//clone.name = gnCard.card.Title + "-2";
//			if (!GameController.instance.isFirstPlayer){
//				//gCard.setStyles(myCharacterNameStyle, myCharacterLifeStyle, myLifeBarStyle, myAttackStyle, myMoveStyle, myQuicknessStyle, myStatsZoneStyle, attackIcon, quicknessIcon, moveIcon, mySkillInfoStyle);
//				myCards.Add(clone);
//				pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3((x+1)*scaleTile*0.71f-scaleTile*0.27f, (y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0));
//				playingCardController.isMine = true;
//				
//			}
//			else
//			{
//				//gCard.setStyles(characterNameStyle, characterLifeStyle, lifeBarStyle, attackStyle, moveStyle, quicknessStyle, statsZoneStyle, attackIcon, quicknessIcon, moveIcon, skillInfoStyle);
//				opponentCards.Add(clone);
//				pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3(x*scaleTile*0.71f-scaleTile*0.42f, (y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0));
//				playingCardController.isMine = false;
//			}
//		}
//		else
//		{
//			clone.transform.localRotation =  Quaternion.Euler(-90,0,0);
//			//clone.name = gnCard.card.Title + "-1";
//			if (GameController.instance.isFirstPlayer){
//				//gCard.setStyles(myCharacterNameStyle, myCharacterLifeStyle, myLifeBarStyle, myAttackStyle, myMoveStyle, myQuicknessStyle, myStatsZoneStyle, attackIcon, quicknessIcon, moveIcon, mySkillInfoStyle);
//				myCards.Add(clone);
//				pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3(x*scaleTile*0.71f-scaleTile*0.42f, (y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.2f, 0));
//				playingCardController.isMine = true;
//			}
//			else
//			{
//				//gCard.setStyles(characterNameStyle, characterLifeStyle, lifeBarStyle, attackStyle, moveStyle, quicknessStyle, statsZoneStyle, attackIcon, quicknessIcon, moveIcon, skillInfoStyle);
//				opponentCards.Add(clone);
//				pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3((x+1)*scaleTile*0.71f-scaleTile*0.27f, (y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.4f, 0));
//				playingCardController.isMine = false;
//			}
//		}
//		
//		yield return StartCoroutine(playingCardController.instance.retrieveCard(cardID));
//		
//		PhotonView nView;
//		nView = clone.GetComponentInChildren<PhotonView>();
//		nView.viewID = viewID;
//		clone.transform.localScale = new Vector3(1, 1, 1);
//		
//		//		if (gCard.photonView.isMine && GameBoard.instance.nbPlayer == 1 || !gCard.photonView.isMine && GameBoard.instance.nbPlayer == 2)
//		//		{
//		//			gnCard.ownerNumber = 1;
//		//			nbCardsPlayer1++;
//		//
//		//		}
//		//		else
//		//		{
//		//			gnCard.ownerNumber = 2;
//		//			nbCardsPlayer2++;
//		//		}
//		
//		
//		// JBU gCard.setRectStats(pos.x, heightScreen-pos.y, heightScreen *12/100, heightScreen*4/100);
//		// JBU gCard.x = x ;
//		// JBU gCard.y = y ;
//		// JBU gCard.showInformations();
//		clone.tag = "PlayableCard";
//		// JBU gnCard.ShowFace();
//		
//		//GameTimeLine.instance.GameCards.Add(gnCard);
//		//GameTimeLine.instance.SortCardsBySpeed();
//		//GameTimeLine.instance.removeBarLife();
//		//GameTimeLine.instance.Arrange();
//	}

//	[RPC]
//	public void addTile(int x, int y, int type)
//	{
//		int decalage ;
//		if ((this.gridWidthInHexes-x)%2==0){
//			decalage = 0;
//		}
//		else{
//			decalage = 1;
//		}
//		GameObject hex = (GameObject)Instantiate(Hex);
//		hex.name = "hex " + (x) + "-" + (y) ;
//		//hex.layer = GridLayerMask;
//		
//		hex.transform.localScale= new Vector3(scaleTile,scaleTile,scaleTile);
//		hex.transform.position = new Vector3(x*scaleTile*0.71f, (y+(decalage/2f))*scaleTile*0.81f, 0);
//		
//		var tb = (GameTile)hex.GetComponent("GameTile");
//		tb.tile = new Tile(x, y, type);
//		tb.ShowFace();
//		this.board.Add(tb.tile);
//		
//		//foreach(Tile tile in GameBoard.instance.board.Values){
//		//tile.FindNeighbours(GameBoard.instance.board, new Vector2(gridHeightInHexes, gridWidthInHexes));
//		//}
//		
//		displayedHex=true ;
//	}
}

