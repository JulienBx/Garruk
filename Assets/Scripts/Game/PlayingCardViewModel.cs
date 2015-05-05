using UnityEngine;


public class PlayingCardViewModel
{
	public Vector3 position ;
	public Vector3 ScreenPosition ;
	public Quaternion rotation ;
	public Vector3 scale ;
	public Rect infoRect ;
	public float skillInfoRectWidth ;

	public Texture2D attackIcon ;
	public Texture2D moveIcon ;
	public Texture2D quicknessIcon ;
	public Texture2D picture ;

	public GUIStyle backgroundStyle;
	public GUIStyle nameTextStyle;
	public GUIStyle attackZoneTextStyle;

	public GUIStyle imageStyle;
	public GUIStyle lifeTextStyle;
	public GUIStyle backgroundLifeBar;
	public GUIStyle emptyButtonStyle;

	public string name ;
	public string attack ;
	public string move ;
	public int quickness ;
	public int life ;
	public int maxLife ;

	public bool isSelected ; 

	public PlayingCardViewModel ()
	{
		this.position = new Vector3(0,0,0);
		this.ScreenPosition = new Vector3(0,0,0);
		this.rotation = Quaternion.Euler(0,0,0);
		this.scale = new Vector3(0,0,0);
		this.infoRect = new Rect(0,0,0,0);
		this.skillInfoRectWidth = 0;

	 	this.name = "";
		this.attack = "";
		this.move = "";
		this.quickness = 0;
		this.life = 0;
		this.maxLife = 0;

		this.backgroundStyle = new GUIStyle();
		this.nameTextStyle = new GUIStyle();
		this.attackZoneTextStyle = new GUIStyle();

		this.imageStyle = new GUIStyle();
		this.lifeTextStyle = new GUIStyle();
		this.backgroundLifeBar = new GUIStyle();
		this.emptyButtonStyle = new GUIStyle();

		this.attackIcon = null ;
		this.moveIcon = null ;
		this.quicknessIcon = null ;
	}
}

