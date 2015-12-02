using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class StartButtonController : MonoBehaviour
{	
	bool isPushed = false ;
	float time ;
	float timeToSwitch = 0.5f;
	bool isVisible ; 
	
	public void OnMouseEnter(){
		if (!isPushed){
			gameObject.transform.FindChild("StartButton").GetComponent<TextMeshPro>().color=new Color(71f/255f,150f/255f,189f/255f, 1f);
		}
	}
	
	public void OnMouseExit(){
		if (!isPushed){
			gameObject.transform.FindChild("StartButton").GetComponent<TextMeshPro>().color=new Color(1f, 1f, 1f, 1f);
		}
	}
	
	public void OnMouseDown(){
		if(this.isPushed==false){
			this.isPushed = true ;
			gameObject.GetComponent<SpriteRenderer>().enabled = false;
			gameObject.transform.FindChild("StartButton").GetComponent<TextMeshPro>().text="En attente du joueur 2";
			gameObject.transform.FindChild("StartButton").GetComponent<TextMeshPro>().color=new Color(1f, 1f, 1f, 1f);
			this.isVisible = true ;
			GameController.instance.playerReady(GameView.instance.getIsFirstPlayer());
		}
	}
	
	public void show(bool b){
		gameObject.GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("StartButton").GetComponent<MeshRenderer>().enabled = b ;
		this.isPushed = false ;
	}
	
	public void showText(bool b){
		gameObject.GetComponent<BoxCollider>().enabled = false ;
		gameObject.transform.FindChild("StartButton").GetComponent<MeshRenderer>().enabled = b ;
		this.isPushed = true ;
		if(!b){
			this.isPushed=false;
		}
	}
	
	public void addTime(float f){
		this.time += f ;
		if(this.time>this.timeToSwitch){
			gameObject.transform.FindChild("StartButton").GetComponent<MeshRenderer>().enabled = !this.isVisible;
			this.time = 0f;
			this.isVisible=!this.isVisible ;
		}
	}	
	
	public bool getIsPushed(){
		return this.isPushed ;
	}
	
	public void setText(string s){
		gameObject.transform.FindChild("StartButton").GetComponent<TextMeshPro>().text=s;
	}
}