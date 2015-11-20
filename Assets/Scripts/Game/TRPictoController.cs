using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class TRPictoController : MonoBehaviour
{	
	int idCard ;
	
	string title ; 
	string description ;
	
	public void setTexts(string t, string d){
		this.title = t;
		this.description = d;
		gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<TextMeshPro>().text = this.title;
		gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = this.description;
	}
	
	public void OnMouseEnter(){
		GameView.instance.getPlayingCardController(this.idCard).showDescriptionTurns(true);
	}
	
	public void OnMouseExit(){
		GameView.instance.getPlayingCardController(this.idCard).showDescriptionTurns(false);
	}
	
	public void setIDCard(int i){
		this.idCard = i ;
	}
}


