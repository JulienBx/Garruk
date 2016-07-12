using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class ExternalColliderC : MonoBehaviour
{	
	public void OnMouseEnter(){
		if(Game.instance.getDraggingCardID()==-1){
			Game.instance.hoverTile();
		}
	}

	public void OnMouseDown(){
		
	}
}


