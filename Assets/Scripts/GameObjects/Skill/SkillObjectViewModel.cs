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

	public SkillObjectViewModel()
	{
		this.isActive = false ;
		this.skillRectStyle = new GUIStyle();
		this.skillDescriptionTextStyle = new GUIStyle();
		this.skillTitleTextStyle = new GUIStyle();
	}

	public void resize(int i){
		 
	}
}

