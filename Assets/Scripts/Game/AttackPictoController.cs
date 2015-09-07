using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class AttackPictoController : MonoBehaviour
{	
	bool isHovered = false ;
	int idCard = 2 ;
	
	public void OnMouseEnter(){
		if (!isHovered){
			Vector3 position = gameObject.transform.position;
			position.y += 0.65f;
			GameView.instance.displayPopUp("Le héros inflige "+GameView.instance.getCard(idCard).GetAttack()+" dégats de base à chaque attaque", position, "Attaque");
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


