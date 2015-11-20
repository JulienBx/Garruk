using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class InterludeController : MonoBehaviour
{	
	public float realwidth ;
	public float animationTime = 0.5f;
	public float time;
	public bool isRunning;
	public Sprite[] sprites ;
	
	void Awake(){
		this.isRunning = false ;
		gameObject.GetComponent<SpriteRenderer>().enabled = false ;
		gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().enabled = false ;
		gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().enabled = false ;
		gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().enabled = false ;
		gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = false ;
	}
	
	public void resize(float realwidth){
		this.realwidth = realwidth ;
	}
	
	public void OnMouseDown(){
		if(this.time>1*this.animationTime){
			this.time=5*this.animationTime;
		}
	}
	
	public void set(string s, bool isMine){
		gameObject.GetComponent<SpriteRenderer>().enabled = true ;
		Vector3 position ;
		position = gameObject.transform.FindChild("Bar1").localPosition ;
		position.x = realwidth/2f+10f;
		gameObject.transform.FindChild("Bar1").localPosition = position;
		position = gameObject.transform.FindChild("Bar2").localPosition ;
		position.x = -realwidth/2f-10f;
		gameObject.transform.FindChild("Bar2").localPosition = position;
		position = gameObject.transform.FindChild("Bar3").localPosition ;
		position.x = realwidth/2f+10f;
		gameObject.transform.FindChild("Bar3").localPosition = position;
		if(isMine){
			gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().sprite = this.sprites[0];
			gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().sprite = this.sprites[1];
			gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().sprite = this.sprites[2];
		}
		else{
			gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().sprite = this.sprites[3];
			gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().sprite = this.sprites[4];
			gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().sprite = this.sprites[5];
		}
		gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().enabled = true ;
		gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().enabled = true ;
		gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().enabled = true ;
		
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().text = s ;
		gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = true ;
		this.time=0f;
		this.isRunning = true ;
	}
	
	public void addTime(float f){
		this.time += f ;
		if(this.time>6*this.animationTime){
			gameObject.GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = false ;
			this.isRunning = false ;
		}
		else if(this.time>5*this.animationTime){
			Vector3 position ;
			float rapport = Mathf.Min(1,(this.time-5f*this.animationTime)/this.animationTime);
			position = gameObject.transform.FindChild("Bar1").localPosition ;
			position.x = -realwidth/2f+11f+(realwidth-1f)*rapport;
			gameObject.transform.FindChild("Bar1").localPosition = position;
			position = gameObject.transform.FindChild("Bar2").localPosition ;
			position.x = realwidth/2f-11f+(realwidth-1f)*rapport;
			gameObject.transform.FindChild("Bar2").localPosition = position;
			position = gameObject.transform.FindChild("Bar3").localPosition ;
			position.x = -realwidth/2f+10.5f-(realwidth-0.5f)*rapport;
			gameObject.transform.FindChild("Bar3").localPosition = position;
			gameObject.transform.FindChild("Text").localScale=new Vector3(1-rapport, 1-rapport, 1-rapport);
		}
		else if(this.time<this.animationTime){
			Vector3 position ;
			float rapport = Mathf.Min(1,(this.time)/this.animationTime);
			position = gameObject.transform.FindChild("Bar1").localPosition ;
			position.x = realwidth/2f+10f-(realwidth-1f)*rapport;
			gameObject.transform.FindChild("Bar1").localPosition = position;
			position = gameObject.transform.FindChild("Bar2").localPosition ;
			position.x = -realwidth/2f-10f+(realwidth-1.5f)*rapport;
			gameObject.transform.FindChild("Bar2").localPosition = position;
			position = gameObject.transform.FindChild("Bar3").localPosition ;
			position.x = realwidth/2f+10f-(realwidth-0.5f)*rapport;
			gameObject.transform.FindChild("Bar3").localPosition = position;
			gameObject.transform.FindChild("Text").localScale=new Vector3(rapport, rapport, rapport);
		}
	}	
	
	public bool getIsRunning(){
		return this.isRunning;
	}
}