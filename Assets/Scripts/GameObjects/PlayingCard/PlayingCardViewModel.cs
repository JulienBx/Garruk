using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayingCardViewModel
{
	public Vector3 position ;
	public Vector3 ScreenPosition ;
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
	public List<bool> toDisplayDescriptionIcon ;

	public GUIStyle iconStyle ;
	public GUIStyle titleStyle ;
	public GUIStyle descriptionStyle ;
	public GUIStyle descriptionRectStyle ;

	public Texture2D halo ;
	public Rect haloRect ;
	public bool toDisplayHalo = false; 


	public PlayingCardViewModel ()
	{
		this.icons = new List<Texture2D>();
		this.iconsRect = new List<Rect>();
		this.titlesIcon = new List<string>();
		this.descriptionIcon = new List<string>();
		this.toDisplayDescriptionIcon = new List<bool>();
		this.iconStyle = new GUIStyle();
		this.titleStyle = new GUIStyle();
		this.descriptionStyle = new GUIStyle();
		this.descriptionRectStyle = new GUIStyle();
	}
}

