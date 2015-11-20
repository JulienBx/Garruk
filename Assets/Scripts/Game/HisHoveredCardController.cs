using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class HisHoveredCardController : HoveredCardController
{	
	public void addTime(float f){
		this.timer += f;
		
		if(base.getStatus()>0){
			Vector3 position = gameObject.transform.localPosition;
			position.x = (0.5f*this.realwidth+5f)-(Mathf.Min(1,this.timer/this.animationTime))*(0.5f*realwidth-3f);
			gameObject.transform.localPosition = position ;
			
			if (this.timer>this.animationTime){
				base.setStatus(0);
				base.currentCharacter = this.nextDisplayedCharacter;
				if(base.currentCharacter==GameView.instance.getCurrentPlayingCard()){
					base.run();
				}
				base.timer=0f;
			}
		}
		else if(base.getStatus()<0){
			Vector3 position = gameObject.transform.localPosition;
			position.x = (8f)+(Mathf.Min(1,this.timer/this.animationTime))*(0.5f*realwidth-3f);
			gameObject.transform.localPosition = position ;
			
			if (this.timer>this.animationTime){
				if(this.nextDisplayedCharacter!=-1){
					base.setCard(this.nextDisplayedCard);
					base.setStatus(1);
					base.timer = 0f;
				}
			}
		}
	}
	
	public void launchNextMove(){
//		if (this.nextDisplayedCharacter!=-1){
//			this.setCard(GameView.instance.getCard(this.nextDisplayedCharacter));
//			this.status = 1 ;
//			
//			this.timer = 0 ;
//			this.currentCharacter = this.nextDisplayedCharacter;
//			this.nextDisplayedCharacter = -1 ;
//		}
	}
	
	public void setNextDisplayedCharacter(int i){
		this.nextDisplayedCharacter = i;
	}
	
	public void hide(){
		this.timer = 0 ;
		base.setStatus(-1);
		base.stopAnim();
	}
	
	public void reverse(int i){
		base.setStatus(i);
		this.timer = this.animationTime - this.timer ;
	}
	
	public void resize(float realwidth, float tileScale){
		Vector3 position;
		
		base.realwidth = realwidth ;
		
		Transform tempTransform ;
		
		tempTransform = gameObject.transform;
		position = tempTransform.localPosition ;
		position.x = 0.50f*this.realwidth+5f;
		tempTransform.localPosition = position;	
	}
}


