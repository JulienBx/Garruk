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
		gameObject.transform.FindChild("Character").GetComponent<SpriteRenderer>().sprite = GameView.instance.getSprite(c.Skills[0].Id);
		gameObject.transform.FindChild("Title").FindChild("Text").GetComponent<TextMeshPro>().text = c.getName();
		gameObject.transform.FindChild("Title").FindChild("PVText").GetComponent<TextMeshPro>().text = c.GetLifeString();
		gameObject.transform.FindChild("Title").FindChild("AttackText").GetComponent<TextMeshPro>().text = c.GetAttackString();

		int nbSkills = 0 ;
		for(int i = 1 ; i < c.Skills.Count;i++){
			if(c.Skills[i].IsActivated==1){
				gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Text").GetComponent<TextMeshPro>().text = c.getSkillText(WordingSkills.getDescription(c.Skills [i].Id, c.Skills[i].Power-1));
				gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = GameView.instance.getSkillSprite(c.Skills[i].Id);
				gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Level").GetComponent<TextMeshPro>().text = ""+c.Skills[i].Power;
				if(c.Skills[i].Level==1){
					gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Picto").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
					gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Level").GetComponent<TextMeshPro>().color = new Color(1f, 1f, 1f, 1f);
				}
				else if(c.Skills[i].Level==2){
					gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Picto").GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
					gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Level").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
				}
				else{
					gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Picto").GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
					gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Level").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
				}
				gameObject.transform.FindChild("Skill"+nbSkills).GetComponent<SpriteRenderer>().color=new Color(1f, 1f, 1f, 1f);
				gameObject.transform.FindChild("Skill"+nbSkills).GetComponent<SpriteRenderer>().enabled=true;
				gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Text").GetComponent<MeshRenderer>().enabled=true;
				gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Level").GetComponent<MeshRenderer>().enabled=true;
				gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Picto").GetComponent<SpriteRenderer>().enabled=true;
				nbSkills++;
			}
		}
		gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Text").GetComponent<TextMeshPro>().text = c.getSkillText(WordingSkills.getDescription(c.Skills [0].Id, c.Skills[0].Power-1));
		gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = GameView.instance.getSkillSprite(c.Skills[0].Id);
		gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Level").GetComponent<TextMeshPro>().text = ""+c.Skills[0].Power;
		if(c.Skills[0].Level==1){
			gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Picto").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
			gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Level").GetComponent<TextMeshPro>().color = new Color(1f, 1f, 1f, 1f);
		}
		else if(c.Skills[0].Level==2){
			gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Picto").GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
			gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Level").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
		}
		else{
			gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Picto").GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
			gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Level").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
		}
		gameObject.transform.FindChild("Skill"+nbSkills).GetComponent<SpriteRenderer>().color=new Color(100f/255f, 100f/255f, 100f/255f, 1f);
				
		gameObject.transform.FindChild("Skill"+nbSkills).GetComponent<SpriteRenderer>().enabled=true;
		gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Text").GetComponent<MeshRenderer>().enabled=true;
		gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Level").GetComponent<MeshRenderer>().enabled=true;
		gameObject.transform.FindChild("Skill"+nbSkills).FindChild("Picto").GetComponent<SpriteRenderer>().enabled=true;
		nbSkills++;

		for (int j = nbSkills ; j < 4 ; j++){
			gameObject.transform.FindChild("Skill"+j).GetComponent<SpriteRenderer>().enabled=false;
			gameObject.transform.FindChild("Skill"+j).FindChild("Text").GetComponent<MeshRenderer>().enabled=false;
			gameObject.transform.FindChild("Skill"+j).FindChild("Level").GetComponent<MeshRenderer>().enabled=false;
			gameObject.transform.FindChild("Skill"+j).FindChild("Picto").GetComponent<SpriteRenderer>().enabled=false;
		}

		List<Modifyer> effects = c.getEffects();
		for(int i = 0 ; i < effects.Count ; i++){
			gameObject.transform.FindChild("Effect"+i).FindChild("Text").GetComponent<TextMeshPro>().text = effects[i].description;
			gameObject.transform.FindChild("Effect"+i).FindChild("Title").GetComponent<TextMeshPro>().text = effects[i].title;
			gameObject.transform.FindChild("Effect"+i).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = GameView.instance.getIconSprite(effects[i].type);
			gameObject.transform.FindChild("Effect"+i).GetComponent<SpriteRenderer>().enabled = true ;
			gameObject.transform.FindChild("Effect"+i).FindChild("Text").GetComponent<MeshRenderer>().enabled = true;
			gameObject.transform.FindChild("Effect"+i).FindChild("Title").GetComponent<MeshRenderer>().enabled = true;
			gameObject.transform.FindChild("Effect"+i).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true;
		}
		for(int i = effects.Count ; i < 10 ; i++){
			gameObject.transform.FindChild("Effect"+i).GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.FindChild("Effect"+i).FindChild("Text").GetComponent<MeshRenderer>().enabled = false;
			gameObject.transform.FindChild("Effect"+i).FindChild("Title").GetComponent<MeshRenderer>().enabled = false;
			gameObject.transform.FindChild("Effect"+i).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = false;
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


