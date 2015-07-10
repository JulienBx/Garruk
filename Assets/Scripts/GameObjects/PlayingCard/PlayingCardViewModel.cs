using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayingCardViewModel
{
	public Vector3 position ;
	public Vector3 scale ;
	public Texture face;
	public Texture lifeGauge;
	public Texture2D border;
	public Texture2D background ;
	public string attack ;
	public string move ;
	public bool isSelected ;
	public bool isPlaying ; 
	public bool isActive = true ;

	public List<Texture2D> icons ;
	public List<Rect> iconsRect ;
	public List<string> titlesIcon ;
	public List<string> descriptionIcon ;
	public List<string> additionnalInfoIcon ;
	public int toDisplayDescriptionIcon ;
	
	public GUIStyle iconStyle ;
	public GUIStyle titleStyle ;
	public GUIStyle descriptionStyle ;
	public GUIStyle additionnalInfoStyle ;
	public GUIStyle descriptionRectStyle ;

	public GUIStyle haloStyle ;
	public Rect haloRect ;
	public bool toDisplayHalo = false;
	public List<string> haloTexts ;
	public List<GUIStyle> haloStyles ;
	
	public SkillResult skillResult ;
	public bool toDisplaySkillResult = false;
	public float skillResultTimer = 0 ;
	
	public bool toDisplaySkillControlHandler = false;
	
	public PlayingCardViewModel ()
	{
		this.icons = new List<Texture2D>();
		this.iconsRect = new List<Rect>();
		this.titlesIcon = new List<string>();
		this.descriptionIcon = new List<string>();
		this.iconStyle = new GUIStyle();
		this.titleStyle = new GUIStyle();
		this.descriptionStyle = new GUIStyle();
		this.descriptionRectStyle = new GUIStyle();
		this.additionnalInfoStyle = new GUIStyle();
		this.haloStyle = new GUIStyle();
	}
}

