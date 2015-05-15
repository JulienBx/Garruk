using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayingCardViewModel
{
	public Vector3 position ;
	public Vector3 ScreenPosition ;
	public Vector3 scale ;
	
	public Texture face;
	public Texture2D border ;
	public Texture2D background ;
	
	public string attack ;
	public string move ;
	public bool isSelected ;
	public bool isPlaying ; 

	public PlayingCardViewModel ()
	{

		
	}
}

