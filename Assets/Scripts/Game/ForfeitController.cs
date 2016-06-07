﻿using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ForfeitController : MonoBehaviour
{
	bool isHovered = false ;
	
	public void OnMouseEnter(){
		if(!isHovered){
			gameObject.GetComponent<SpriteRenderer>().color=new Color(255/255f,120f/255f,120f/255f, 1f);
			isHovered = true ;
		}
	}
	
	public void OnMouseExit(){
		if(isHovered){
			gameObject.GetComponent<SpriteRenderer>().color=new Color(1f, 1f, 1f, 1f);
			isHovered = false ;
		}
	}
	
	public void OnMouseDown(){
		GameObject.Find("Hov").GetComponent<SpriteRenderer>().enabled = true ;
		StartCoroutine(GameView.instance.quitGameHandler2(false, false));
		gameObject.GetComponent<BoxCollider>().enabled = false ;
	}
}

