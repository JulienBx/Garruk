using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class AttackPictoController : MonoBehaviour
{	
	int idCard ;
	
	string title ; 
	string description ;
	
	public void setTexts(string t, string d){
		this.title = t;
		this.description = d;
		gameObject.transform.FindChild("TitleText").GetComponent<TextMeshPro>().text = this.title;
		gameObject.transform.FindChild("DescriptionText").GetComponent<TextMeshPro>().text = this.description;
	}
	
	public void OnMouseEnter(){
		GameView.instance.getPlayingCardController(this.idCard).showDescriptionAttack(true);
	}
	
	public void OnMouseExit(){
		GameView.instance.getPlayingCardController(this.idCard).showDescriptionAttack(false);
	}
	
	public void setIDCard(int i){
		this.idCard = i ;
	}
}


