using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class HoveredCardController : MonoBehaviour
{	
	public float timer = 0f;
	public float timerC = 0f;
	public float animationTime = 0.25f;
	public float animationTimeC = 0.5f;
	public float realwidth ;
	
	private int status = 0;
	
	public GameCard nextDisplayedCard ;
	public int nextDisplayedCharacter = -1 ;
	public int currentCharacter = -1;
	
	public bool isGettingBigger;
	
	bool toStop ;
	bool isRunning;
	
	public virtual void Awake(){
		this.nextDisplayedCharacter = -1 ;
		this.isGettingBigger = true ;
		this.toStop = false ;
	}
	
	public void setStatus(int i){
		this.status = i ;
	}
	
	public virtual void setNextDisplayedCharacter(int i, GameCard c){
		if(this.nextDisplayedCharacter!=i){
			this.nextDisplayedCharacter = i;
			this.nextDisplayedCard = c;
			this.stopAnim();
			if(this.status==0){
				if (this.currentCharacter!=-1){
					this.status=-1 ;
				}
				else{
					this.setCard(c);
					this.status=1;
				}
			}
			else if(this.status==1){
				this.timer = this.animationTime - this.timer ;
				this.status = -1 ;
			}
		}
	}
	
	public virtual void setCard(GameCard c){
		if(c.PowerLevel==1){
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
		}
		else if(c.PowerLevel==2){
			gameObject.GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
		}
		else{
			gameObject.GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
		}
		gameObject.transform.FindChild("Character").GetComponent<SpriteRenderer>().sprite = GameView.instance.getSprite(c.Skills[0].Id);
		gameObject.transform.FindChild("Title").FindChild("Text").GetComponent<TextMeshPro>().text = c.getSkills()[0].Name;
		gameObject.transform.FindChild("Title").FindChild("Picto").GetComponent<SpriteRenderer>().sprite = GameView.instance.getCardTypeSprite(c.CardType.Id);
		for (int i = 1 ; i < c.getSkills().Count ; i++){
			if(c.getSkills()[i].IsActivated==1){
				gameObject.transform.FindChild("Skill"+i).GetComponent<SpriteRenderer>().enabled = true;
				gameObject.transform.FindChild("Skill"+i).FindChild("TitleText").GetComponent<MeshRenderer>().enabled = true ;
				gameObject.transform.FindChild("Skill"+i).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true ;
				gameObject.transform.FindChild("Skill"+i).FindChild("TitleText").GetComponent<TextMeshPro>().text = c.getSkills()[i].Name+" Niv."+c.getSkills()[i].Power;
				gameObject.transform.FindChild("Skill"+i).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = GameView.instance.getSkillSprite(c.getSkills()[i].Id);
				if(c.getSkills()[i].Power>8){
					gameObject.transform.FindChild("Skill"+i).FindChild("Picto").GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
				}
				else if(c.getSkills()[i].Power>5){
					gameObject.transform.FindChild("Skill"+i).FindChild("Picto").GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
				}
				else{
					gameObject.transform.FindChild("Skill"+i).FindChild("Picto").GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
				}
			}
			else{
				gameObject.transform.FindChild("Skill"+i).GetComponent<SpriteRenderer>().enabled = false;
				gameObject.transform.FindChild("Skill"+i).FindChild("TitleText").GetComponent<MeshRenderer>().enabled = false ;
				gameObject.transform.FindChild("Skill"+i).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = false ;
			}
		}
		this.resetTimerC();
		this.isRunning=false;
		gameObject.transform.FindChild("Character").localScale = new Vector3(1f, 1f, 1f) ;
	}
	
	public virtual void empty(){
		this.nextDisplayedCharacter = -1;
		if(this.status==0){
			this.status = -1;
		}
		else if(status==1){
			this.timer = this.animationTime - this.timer ;
			this.status = -1 ;
		}
	}
	
	public virtual int getCurrentCharacter(){
		return this.currentCharacter;
	}
	
	public virtual int getNextDisplayedCharacter(){
		return this.nextDisplayedCharacter;
	}
	
	public virtual int getStatus(){
		return this.status;
	}
	
	public void resetTimerC(){
		gameObject.transform.FindChild("Character").localScale = new Vector3(1f, 1f, 1f) ;
		this.timerC = 0f;
		this.toStop = false;
	}
	
	public void addTimeC(float f){
		this.timerC += f;
		
		if(this.timerC>this.animationTimeC){
			if(!this.isGettingBigger && toStop){
				this.isRunning = false ;
				this.toStop = false ;
			}
			else{
				this.isGettingBigger = !this.isGettingBigger ;
			}
			this.timerC = 0f ;
		}
		else{
			if (this.isGettingBigger){
				float scale = 1f + 0.05f * (this.timerC/this.animationTimeC);
				gameObject.transform.FindChild("Character").localScale = new Vector3(scale, scale, scale) ;
			}
			else {
				float scale = 1.05f - 0.05f * (this.timerC/this.animationTimeC);
				gameObject.transform.FindChild("Character").localScale = new Vector3(scale, scale, scale) ;
			}
		}
	}
	
	public void stopAnim(){
		this.toStop = true ;
	}
	
	public bool getIsRunning(){
		return this.isRunning ;
	}
	
	public void run(){
		this.isRunning = true ;
	}
	
	public void toRun(){
		if(status==0 && this.currentCharacter==GameView.instance.getCurrentPlayingCard()){
			this.run (); 
		}
	}
	
	public void raiseCharacter(){
		gameObject.transform.FindChild("Character").GetComponent<SpriteRenderer>().sortingOrder = 15;
		print("Je raise ");
	}
	
	public void lowerCharacter(){
		gameObject.transform.FindChild("Character").GetComponent<SpriteRenderer>().sortingOrder = 1;
	}
}


