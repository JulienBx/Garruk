using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class SkillThunder : MonoBehaviour
{	
	float timeToDisplay = 0f;
	float actualTime = 0f;
	
	public void Awake(){
		this.hide ();
	}
	
	public void setTimeToDisplay(float a){
		this.timeToDisplay = a 	;
	}
	
	public float getTimeToDisplay(){
		return this.timeToDisplay ;
	}
	
	public void setActualTime(float a){
		this.actualTime = a ;
	}
	
	public float getActualTime(){
		return this.actualTime ;
	}
	
	public void addTime(float delta){
		this.actualTime += delta;
		Quaternion rotation = gameObject.transform.rotation ;
		if(this.actualTime<=1){
			//rotation = Quaternion.Euler(rotation.x, rotation.y, 720f*(this.actualTime));
			Vector3 scale = new Vector3(1f*this.actualTime,1f*this.actualTime,1f*this.actualTime);
			//gameObject.transform.rotation = rotation ;
			gameObject.transform.localScale = scale ;
		}
		else if(this.actualTime>this.timeToDisplay){
			this.hide();
			this.resetTime();
			this.timeToDisplay=0;
		}
		else if(this.actualTime>this.timeToDisplay-1){
			rotation = Quaternion.Euler(rotation.x, rotation.y, 720f*(1f-(this.actualTime-this.timeToDisplay+1)));
			
			Vector3 scale = new Vector3(1f-(this.actualTime-this.timeToDisplay+1),1f-(this.actualTime-this.timeToDisplay+1),1f-(this.actualTime-this.timeToDisplay+1));
			gameObject.transform.rotation = rotation ;
			gameObject.transform.localScale = scale ;
		}
	}
	
	public void hide(){
		gameObject.GetComponent<SpriteRenderer>().enabled=false;
		gameObject.transform.FindChild("GameObject").GetComponent<MeshRenderer>().enabled = false ;
	}
	
	public void show(){
		gameObject.GetComponent<SpriteRenderer>().enabled=true;
		gameObject.transform.FindChild("GameObject").GetComponent<MeshRenderer>().enabled = true ;
	}
	
	public void resetTime(){
		this.actualTime = 0f ;
	}
}


