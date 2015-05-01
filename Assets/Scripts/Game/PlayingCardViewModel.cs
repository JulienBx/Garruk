using UnityEngine;


public class PlayingCardViewModel
{
	public Vector3 position ;
	public Vector3 ScreenPosition ;
	public Quaternion rotation ;
	public Vector3 scale ;
	public Rect infoRect ;

	public Texture2D attackIcon ;
	public Texture2D moveIcon ;
	public Texture2D quicknessIcon ;
	public Texture2D picture ;

	public GUIStyle backgroundStyle;
	public GUIStyle nameTextStyle;
	public GUIStyle attackZoneTextStyle;
	public GUIStyle moveZoneTextStyle;
	public GUIStyle quicknessZoneTextStyle;
	public GUIStyle imageStyle;
	public GUIStyle lifeTextStyle;
	public GUIStyle backgroundLifeBar;

	public string name ;
	public string attack ;
	public string move ;
	public int quickness ;
	public int life ;
	public int maxLife ;

	public PlayingCardViewModel ()
	{
		this.position = new Vector3(0,0,0);
		this.ScreenPosition = new Vector3(0,0,0);
		this.rotation = Quaternion.Euler(0,0,0);
		this.scale = new Vector3(0,0,0);
		this.infoRect = new Rect(0,0,0,0);

	 	this.name = "";
		this.attack = "";
		this.move = "";
		this.quickness = 0;
		this.life = 0;
		this.maxLife = 0;

		this.backgroundStyle = new GUIStyle();
		this.nameTextStyle = new GUIStyle();
		this.attackZoneTextStyle = new GUIStyle();
		this.moveZoneTextStyle = new GUIStyle();
		this.quicknessZoneTextStyle = new GUIStyle();
		this.imageStyle = new GUIStyle();
		this.lifeTextStyle = new GUIStyle();
		this.backgroundLifeBar = new GUIStyle();

		this.attackIcon = null ;
		this.moveIcon = null ;
		this.quicknessIcon = null ;
	}

		//      int decalage ;
		//		GameObject clone;
		//		Vector3 pos ;
		//		CharacterScript gCard ;
		//		BoxCollider bCard ;
		//
		//		if (x%2==0){
		//			decalage = 0;
		//		}
		//		else{
		//			decalage = 1;
		//		}
		//		
		//		if (y>0){
		//			clone = Instantiate(characters[type], new Vector3(x*scaleTile*0.71f, (y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0), Quaternion.identity) as GameObject;
		//		}
		//		else{
		//			clone = Instantiate(characters[type], new Vector3(x*scaleTile*0.71f, (y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.2f, 0), Quaternion.identity) as GameObject;
		//		}
		//		gCard = clone.GetComponentInChildren<CharacterScript>();
		//		bCard = clone.GetComponentInChildren<BoxCollider>();
		//		GameNetworkCard gnCard = clone.GetComponent<GameNetworkCard>();
		//		gCard.setGnCard(gnCard);
		//		
		//		if (y>0){
		//			clone.transform.localRotation =  Quaternion.Euler(90,180,0);
		//			//clone.name = gnCard.card.Title + "-2";
		//			if (!GameController.instance.isFirstPlayer){
		//				//gCard.setStyles(myCharacterNameStyle, myCharacterLifeStyle, myLifeBarStyle, myAttackStyle, myMoveStyle, myQuicknessStyle, myStatsZoneStyle, attackIcon, quicknessIcon, moveIcon, mySkillInfoStyle);
		//				mesCartes.Add(clone);
		//				pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3((x+1)*scaleTile*0.71f-scaleTile*0.27f, (y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0));
		//				gnCard.isMine = true;
		//
		//			}
		//			else{
		//				//gCard.setStyles(characterNameStyle, characterLifeStyle, lifeBarStyle, attackStyle, moveStyle, quicknessStyle, statsZoneStyle, attackIcon, quicknessIcon, moveIcon, skillInfoStyle);
		//				sesCartes.Add(clone);
		//				pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3(x*scaleTile*0.71f-scaleTile*0.42f, (y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0));
		//				gnCard.isMine = false;
		//			}
		//		}
		//		else{
		//			clone.transform.localRotation =  Quaternion.Euler(-90,0,0);
		//			//clone.name = gnCard.card.Title + "-1";
		//			if (GameController.instance.isFirstPlayer){
		//				//gCard.setStyles(myCharacterNameStyle, myCharacterLifeStyle, myLifeBarStyle, myAttackStyle, myMoveStyle, myQuicknessStyle, myStatsZoneStyle, attackIcon, quicknessIcon, moveIcon, mySkillInfoStyle);
		//				mesCartes.Add(clone);
		//				pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3(x*scaleTile*0.71f-scaleTile*0.42f, (y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.2f, 0));
		//				gnCard.isMine = true;
		//			}
		//			else{
		//				//gCard.setStyles(characterNameStyle, characterLifeStyle, lifeBarStyle, attackStyle, moveStyle, quicknessStyle, statsZoneStyle, attackIcon, quicknessIcon, moveIcon, skillInfoStyle);
		//				sesCartes.Add(clone);
		//				pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3((x+1)*scaleTile*0.71f-scaleTile*0.27f, (y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.4f, 0));
		//				gnCard.isMine = false;
		//			}
		//		}
		//
		//		yield return StartCoroutine(gnCard.RetrieveCard(cardID));
		//
		//		PhotonView nView;
		//		nView = clone.GetComponentInChildren<PhotonView>();
		//		nView.viewID = viewID;
		//		clone.transform.localScale = new Vector3(1, 1, 1);
		//
		////		if (gCard.photonView.isMine && GameBoard.instance.nbPlayer == 1 || !gCard.photonView.isMine && GameBoard.instance.nbPlayer == 2)
		////		{
		////			gnCard.ownerNumber = 1;
		////			nbCardsPlayer1++;
		////
		////		}
		////		else
		////		{
		////			gnCard.ownerNumber = 2;
		////			nbCardsPlayer2++;
		////		}
		//		
		//
		//		gCard.setRectStats(pos.x, heightScreen-pos.y, heightScreen *12/100, heightScreen*4/100);
		//		gCard.x = x ;
		//		gCard.y = y ;
		//		gCard.showInformations();
		//		clone.tag = "PlayableCard";
		//		gnCard.ShowFace();
		//
		//		//GameTimeLine.instance.GameCards.Add(gnCard);
		//		//GameTimeLine.instance.SortCardsBySpeed();
		//		//GameTimeLine.instance.removeBarLife();
		//		//GameTimeLine.instance.Arrange();
		//	}
}

