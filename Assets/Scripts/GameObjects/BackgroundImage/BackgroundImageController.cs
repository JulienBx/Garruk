using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundImageController : GameObjectController
{
	public Texture2D[] backgroundImages ;

	public BackgroundImageController()
	{
		gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite> ("gameBacksssground");
	}
	
	public void resize(int w, int h)
	{
		
	}
	
	public void setImage(int i)
	{
		//this.GetComponent<Renderer>().material.mainTexture = backgroundImages[i];
	}
}


