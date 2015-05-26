using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillObjectViewModel
{
	public Texture face;
	public Vector3 position;
	public Vector3 scale;
	public Texture2D border ;

	public bool isActive ;

	public string skillTitle ;
	public string skillDescription ;
	public Rect skillRect ;

	public bool toDisplayInfo ;

	public GUIStyle skillRectStyle ;
	public GUIStyle skillTitleTextStyle ;
	public GUIStyle skillDescriptionTextStyle ;

	public bool isMine ;

	public SkillObjectViewModel()
	{
		this.isActive = false ;
	}

	public void resize(int i){
		int height = Screen.height;
		int width = Screen.width;
		if (isMine){
			this.skillRect = new Rect((width/2f)-height*(5f/16f)+i*(height/10f), height*82/100, height/3f, height/10f);
		}
		else{
			this.skillRect = new Rect((width/2f)-height*(5f/16f)+i*(height/10f), height*8/100, height/3f, height/10f);
		}
		this.skillTitleTextStyle.fontSize = height * 20 / 1000 ;
		this.skillDescriptionTextStyle.fontSize = height * 15 / 1000 ;
	}
}
