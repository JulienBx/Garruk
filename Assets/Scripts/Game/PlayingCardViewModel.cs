using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayingCardViewModel
{
	public Vector3 position ;
	public Vector3 ScreenPosition ;

	public Vector3 scale ;

	public Rect infoRect ;
	public float skillInfoRectWidth ;

	public Texture2D attackIcon ;
	public Texture2D moveIcon ;
	public Texture2D picture ;

	public GUIStyle backgroundStyle;
	public GUIStyle nameTextStyle;
	public GUIStyle imageStyle;
	public GUIStyle attackZoneTextStyle;
	public GUIStyle lifeBarStyle;
	public GUIStyle backgroundBarStyle;
	public GUIStyle QuicknessBarStyle;
	public GUIStyle emptyButtonStyle;
	public GUIStyle skillTitleTextStyle;
	public GUIStyle skillDescriptionTextStyle;

	public string name ;
	public string attack ;
	public string move ;
	public int quickness ;
	public int life ;
	public int maxLife ;

	public List<string> skillTitles ;
	public List<string> skillDescriptions ;

	public bool isSelected ; 

	public PlayingCardViewModel ()
	{
		this.position = new Vector3(0,0,0);
		this.ScreenPosition = new Vector3(0,0,0);
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
		this.lifeBarStyle = new GUIStyle();
		this.backgroundBarStyle = new GUIStyle();
		this.QuicknessBarStyle = new GUIStyle();
		this.emptyButtonStyle = new GUIStyle();

		this.skillTitleTextStyle = new GUIStyle();
		this.skillDescriptionTextStyle = new GUIStyle();

		this.attackIcon = null ;
		this.moveIcon = null ;

		skillTitles = new List<string>();
		skillDescriptions = new List<string>();
		
	}
}

