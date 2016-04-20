using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class PictoChoiceController : MonoBehaviour
{
	int face ;
	public void setFace(Sprite s, int i){
		this.face = i ;
		gameObject.transform.FindChild("Character").GetComponent<SpriteRenderer>().sprite = s ; 
	}

	public void show(bool b){
		gameObject.GetComponent<SpriteRenderer>().enabled=b;
		gameObject.transform.FindChild("Character").GetComponent<SpriteRenderer>().enabled=b;
		gameObject.GetComponent<BoxCollider>().enabled=b;
		if(b){
			gameObject.GetComponent<SpriteRenderer>().color = new Color(255f/255f,255f/255f,255f/255f, 1f);
		}
	}
	
	public void OnMouseDown(){
		GameView.instance.hitTarget(new Tile(this.face,0));
	}

	public void OnMouseEnter(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
	}

	public void OnMouseExit(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(255f/255f,255f/255f,255f/255f, 1f);
	}
}


