using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class TRPictoController : MonoBehaviour
{	
	bool isHovered = false ;
	int idCard = 2 ;
	
	public void OnMouseEnter(){
		if (!isHovered){
			Vector3 position = gameObject.transform.position;
			position.x -= 2f;
			//GameView.instance.displayPopUp("Il reste "+GameView.instance.getCard(idCard).nbTurnsToWait+" tour(s) avant que le héros joue", position, "Tours");
			this.isHovered = true ;
		}
	}
	
	public void OnMouseExit(){
		if (isHovered){
			GameView.instance.hidePopUp();
			this.isHovered = false ;
		}
	}
	
	public void setIDCard(int i){
		this.idCard = i ;
	}
}


