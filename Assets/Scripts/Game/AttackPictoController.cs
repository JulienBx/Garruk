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
			position.x -= 2f;
			string description = "Attaque de base : "+GameView.instance.getCard(this.idCard).Attack+"\n";
			List<string> textes = GameView.instance.getCard(this.idCard).getIconAttack();
			for(int i = 0 ; i < textes.Count ; i++){
				description += "<b>"+textes[i]+" : "+"</b>";
				i++;
				description += textes[i]+"\n";
			}
			if (textes.Count>0){
				description += "---> TOTAL : "+GameView.instance.getCard(this.idCard).GetAttack();
			}
			
			GameView.instance.displayPopUp(description, position, "Attaque");
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


