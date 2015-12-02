using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class PassController : MonoBehaviour
{	
	bool isAnimated;
	bool toStop ;
	float timer ;
	float animationTime = 0.5f ;
	bool isGettingBigger;
	
	void Awake(){
		this.show(false);
		this.isAnimated = false;
		this.isGettingBigger = true ;
		this.toStop = false;
	}
	
	public bool getIsAnimated(){
		return this.isAnimated;
	}
	
	public void show(bool b){
		gameObject.GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("Picto").GetComponent<SpriteRenderer>().enabled = b ;
		if(!b){
			gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = false ;
		}
		gameObject.GetComponent<BoxCollider>().enabled = b;
	}
	
	public void updateButtonStatus(GameCard g, bool b){
		
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f) ;
		gameObject.transform.FindChild("Picto").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f) ;
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(1f, 1f, 1f, 1f) ;
		
		if(b){
			this.isAnimated = true;
		}
	}
	
	public void addTime(float f){
		this.timer+=f;
		if(timer>animationTime){
			this.isGettingBigger=!this.isGettingBigger;
			if(!this.isGettingBigger && this.toStop){
				this.isAnimated=false;
			}
			this.timer=0f;
		}
		else{
			if(this.isGettingBigger){
				float rapport = 0.20f+(0.1f*this.timer/this.animationTime);
				gameObject.transform.FindChild("Picto").localScale = new Vector3(rapport, rapport, rapport);
			}
			else{
				float rapport = 0.3f-(0.1f*this.timer/this.animationTime);
				gameObject.transform.FindChild("Picto").localScale = new Vector3(rapport, rapport, rapport);
			}
		}
	}
	
	public void OnMouseEnter(){
		if(gameObject.transform.FindChild("Picto").GetComponent<SpriteRenderer>().enabled){
			gameObject.transform.FindChild("Picto").GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = true ;
		}
	}
	
	public void OnMouseExit(){
		if(gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled){
			gameObject.transform.FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true ;
			gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = false ;
		}
	}
	
	public void OnMouseDown(){
		GameController.instance.findNextPlayer();
	}
}


