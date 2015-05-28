using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillObjectViewModel
{
	public Vector3 position;
	public Vector3 scale;
	public Texture2D border ;
	public Texture2D face;

	public bool isControlActive ;

	public string skillDescription ;
	public Rect skillRect ;

	public bool toDisplayInfo ;

	public GUIStyle skillRectStyle ;
	public GUIStyle skillTitleTextStyle ;
	public GUIStyle skillDescriptionTextStyle ;
	public GUIStyle cadreStyle ;
	public GUIStyle powerStyle ;
 
	public Rect habillageRect ;

	public bool isMine ;
	public string skillName ;
	public int power ;

	public SkillObjectViewModel()
	{
		this.isControlActive = false ;
	}

	public void resize(int i){
		int height = Screen.height;
		int width = Screen.width;

		Vector3 positionObject = new Vector3(0,0,0) ;
		positionObject.x = (position.x-this.scale.x/2f)*(height/10f)+(width/2f);
		positionObject.y = (this.scale.x)*(height/10f);

		if (isMine){
			this.skillRect = new Rect(positionObject.x-positionObject.y, height*85/100, 3*positionObject.y, positionObject.y);
			this.habillageRect = new Rect(positionObject.x, height*97/100, positionObject.y, height*2/100);
		}
		else{
			this.skillRect = new Rect(positionObject.x-positionObject.y, height*9/100, 3*positionObject.y, positionObject.y);
			this.habillageRect = new Rect(positionObject.x, height*1/100, (width/10f), height*2/100);
		}
		this.skillTitleTextStyle.fontSize = height * 20 / 1000 ;
		this.skillDescriptionTextStyle.fontSize = height * 15 / 1000 ;
		this.cadreStyle.fontSize = height * 15 / 1000 ;
	}
}
