using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class EndTurnPopUpController : MonoBehaviour
{
	public Sprite[] pictos ;

	void Awake(){
		this.hide();
	}

	public void OnMouseEnter(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
	}

	public void OnMouseExit(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
	}

	public void OnMouseDown(){
		this.hide();
		GameView.instance.hideEndTurnPopUp();
	}
	
	public void display(int nbTurns){
		gameObject.GetComponent<SpriteRenderer>().enabled=true;
		gameObject.GetComponent<BoxCollider>().enabled=true;
		gameObject.transform.FindChild("Title").GetComponent<MeshRenderer>().enabled=true;
		gameObject.transform.FindChild("Description").GetComponent<MeshRenderer>().enabled=true;
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().text="Fin du tour";
	}

	public void hide(){
		gameObject.GetComponent<SpriteRenderer>().enabled=false;
		gameObject.GetComponent<BoxCollider>().enabled=false;
		gameObject.transform.FindChild("Title").GetComponent<MeshRenderer>().enabled=false;
		gameObject.transform.FindChild("Description").GetComponent<MeshRenderer>().enabled=false;
	}
}


