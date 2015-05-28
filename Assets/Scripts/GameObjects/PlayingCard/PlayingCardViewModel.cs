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

	public bool toDisplayIcon = false ;
	public List<Texture2D> icons ;
	public List<Rect> iconsRect ;

	public GUIStyle iconStyle ;

	public Texture2D halo ;
	public Rect haloRect ;
	public bool toDisplayHalo = false; 


	public PlayingCardViewModel ()
	{
		this.icons = new List<Texture2D>();
		this.iconsRect = new List<Rect>();
		this.iconStyle = new GUIStyle();
	}
}

