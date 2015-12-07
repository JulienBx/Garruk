using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class MoveController : MonoBehaviour
{	
	void Awake(){
		this.show(false);
	}
	
	public void show(bool b){
		gameObject.GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = b ;
		gameObject.GetComponent<BoxCollider>().enabled = b;
		
	}
	
	public void updateButtonStatus(GameCard g){
		if(g.canCancelMove){
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f) ;
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(1f, 1f, 1f, 1f) ;
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().text = "Annuler le mouvement";
		}
		this.show(g.canCancelMove);
	}
	
	public void OnMouseEnter(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
	}
	
	public void OnMouseExit(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f) ;
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(1f, 1f, 1f, 1f) ;
	}
	
	public void OnMouseDown(){
		if(ApplicationModel.launchGameTutorial){
			GameView.instance.hideTuto();
		}
		GameView.instance.cancelMove();
	}
}


