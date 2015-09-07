using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class PVPictoController : MonoBehaviour
{	
	bool isHovered = false ;
	int idCard = 2 ;
	
	public void OnMouseEnter(){
		if (!isHovered){
			Vector3 position = gameObject.transform.position;
			position.x -= 1.2f;
			GameView.instance.displayPopUp("Le héros meurt quand son total de PV atteint 0", position, "Points de vie");
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


