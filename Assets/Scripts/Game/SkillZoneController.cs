using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class SkillZoneController : MonoBehaviour
{	
	bool isAnimated;
	bool toStop ;
	float timer ;
	float animationTime = 0.5f ;
	bool isGettingBigger;
	bool isShowingSkills;
	GameCard card ;
	float attackP;
	float skill1P;
	float skill2P;
	float skill3P;
	
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
	
	public void updateButtonStatus(GameCard g){
		this.card = g;
		if(g.hasPlayed){
			gameObject.GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
			gameObject.transform.FindChild("Picto").GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().text = "Compétence déjà utilisée ce tour-ci !";
			this.isAnimated = false;
		}
		else{
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f) ;
			gameObject.transform.FindChild("Picto").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f) ;
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(1f, 1f, 1f, 1f) ;
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().text = "Utiliser une compétence";
			this.isAnimated = true;
			this.isShowingSkills=false;
		}
	}
	
	public void addTime(float f){
		this.timer+=f;
		if(!isShowingSkills){
			if(timer>animationTime){
				this.isGettingBigger=!this.isGettingBigger;
				if(!this.isGettingBigger && this.toStop){
					this.isAnimated=false;
				}
				this.timer=0f;
			}
			else{
				if(this.isGettingBigger){
					float rapport = 0.65f+(0.2f*this.timer/this.animationTime);
					gameObject.transform.FindChild("Picto").localScale = new Vector3(rapport, rapport, rapport);
				}
				else{
					float rapport = 0.85f-(0.2f*this.timer/this.animationTime);
					gameObject.transform.FindChild("Picto").localScale = new Vector3(rapport, rapport, rapport);
				}
			}
		}
		else{
			float rapport = Mathf.Min(1,this.timer/this.animationTime);
			gameObject.transform.FindChild("AttackButton").localPosition = new Vector3(this.attackP*rapport, 0.1f, 0);
			gameObject.transform.FindChild("SkillButton0").localPosition = new Vector3(this.skill1P*rapport, 0.1f, 0);
			gameObject.transform.FindChild("SkillButton1").localPosition = new Vector3(this.skill2P*rapport, 0.1f, 0);
			gameObject.transform.FindChild("SkillButton2").localPosition = new Vector3(this.skill3P*rapport, 0.1f, 0);
			if(timer>animationTime){
				this.isAnimated=false;
				this.timer=0f;
			}
		}
	}
	
	public void OnMouseEnter(){
		if(!this.isShowingSkills){
			gameObject.transform.FindChild("Picto").GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = true ;
		}
	}
	
	public void OnMouseExit(){
		if(!this.isShowingSkills){
			gameObject.transform.FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true ;
			gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = false ;
		}
	}
	
	public void OnMouseDown(){
		this.showSkills();
	}
	
	public void showSkills(){
		if(this.card.getSkills()[3].IsActivated==1){
			this.attackP = -1.5f;
			this.skill1P = -0.5f;
			this.skill2P = 0.5f;
			this.skill3P = 1.5f;
			gameObject.transform.FindChild("AttackButton").GetComponent<SkillButtonController>().show(true);
			gameObject.transform.FindChild("AttackButton").GetComponent<SkillButtonController>().setSkill(this.card.GetAttackSkill());
			gameObject.transform.FindChild("SkillButton0").GetComponent<SkillButtonController>().show(true);
			gameObject.transform.FindChild("SkillButton0").GetComponent<SkillButtonController>().setSkill(this.card.getSkills()[0]);
			gameObject.transform.FindChild("SkillButton1").GetComponent<SkillButtonController>().show(true);
			gameObject.transform.FindChild("SkillButton1").GetComponent<SkillButtonController>().setSkill(this.card.getSkills()[1]);
			gameObject.transform.FindChild("SkillButton2").GetComponent<SkillButtonController>().show(true);
			gameObject.transform.FindChild("SkillButton2").GetComponent<SkillButtonController>().setSkill(this.card.getSkills()[2]);
		}
		else if(this.card.getSkills()[2].IsActivated==1){
			this.attackP = -1.1f;
			this.skill1P = 0f;
			this.skill2P = 1.1f;
			gameObject.transform.FindChild("AttackButton").GetComponent<SkillButtonController>().show(true);
			gameObject.transform.FindChild("AttackButton").GetComponent<SkillButtonController>().setSkill(this.card.GetAttackSkill());
			gameObject.transform.FindChild("SkillButton0").GetComponent<SkillButtonController>().show(true);
			gameObject.transform.FindChild("SkillButton0").GetComponent<SkillButtonController>().setSkill(this.card.getSkills()[0]);
			gameObject.transform.FindChild("SkillButton1").GetComponent<SkillButtonController>().show(true);
			gameObject.transform.FindChild("SkillButton1").GetComponent<SkillButtonController>().setSkill(this.card.getSkills()[1]);
			gameObject.transform.FindChild("SkillButton2").GetComponent<SkillButtonController>().show(false);
		}
		else{
			this.attackP = -0.8f;
			this.skill1P = 0.8f;
			gameObject.transform.FindChild("AttackButton").GetComponent<SkillButtonController>().show(true);
			gameObject.transform.FindChild("AttackButton").GetComponent<SkillButtonController>().setSkill(this.card.GetAttackSkill());
			gameObject.transform.FindChild("SkillButton0").GetComponent<SkillButtonController>().show(true);
			gameObject.transform.FindChild("SkillButton0").GetComponent<SkillButtonController>().setSkill(this.card.getSkills()[0]);
			gameObject.transform.FindChild("SkillButton1").GetComponent<SkillButtonController>().show(false);
			gameObject.transform.FindChild("SkillButton2").GetComponent<SkillButtonController>().show(false);
		}
		this.isShowingSkills=true;
		this.show (false);
		this.timer=0f;
	}
}


