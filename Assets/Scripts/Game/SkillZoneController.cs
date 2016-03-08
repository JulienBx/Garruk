using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class SkillZoneController : MonoBehaviour
{	
	bool isAnimated;
	float timer ;
	float attackP;
	float skill1P;
	float skill2P;
	float skill3P;
	public bool isRunningSkill;
	
	void Awake(){
		this.showCancelButton(false);
		this.showSkillButtons(false);
		this.isRunningSkill = false ;
	}
	
	public void showSkillButtons(bool b){
		gameObject.transform.FindChild("AttackButton").GetComponent<SkillButtonController>().show(b);
		gameObject.transform.FindChild("SkillButton0").GetComponent<SkillButtonController>().show(b);
		gameObject.transform.FindChild("SkillButton1").GetComponent<SkillButtonController>().show(b);
		gameObject.transform.FindChild("SkillButton2").GetComponent<SkillButtonController>().show(b);
	}

	public void setLaunchability(string s){
		gameObject.transform.FindChild("AttackButton").GetComponent<SkillButtonController>().setLaunchability(s);
		gameObject.transform.FindChild("SkillButton0").GetComponent<SkillButtonController>().setLaunchability(s);
		gameObject.transform.FindChild("SkillButton1").GetComponent<SkillButtonController>().setLaunchability(s);
		gameObject.transform.FindChild("SkillButton2").GetComponent<SkillButtonController>().setLaunchability(s);
	}
	
	public void showCancelButton(bool b){
		gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = b ;
	}
	
	public void updateButtonStatus(GameCard g){
		if(this.isRunningSkill){
			//gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(1f, 1f, 1f, 1f) ;
			//gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().text = "Annuler la compétence en cours : "+GameSkills.instance.getCurrentGameSkill().name;
			//this.showSkillButtons(false);
			//this.showCancelButton (true);
		}
		else if(g.hasPlayed){
			this.showSkillButtons(false);
			this.showCancelButton (false);
		}
		else{
			this.showCancelButton (false);
			this.showSkills(g);
		}
	}
	
	public void OnMouseEnter(){
		if(this.isRunningSkill){
			gameObject.GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f) ;
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
		}
	}
	
	public void OnMouseExit(){
		if(this.isRunningSkill){
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f) ;
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(1f, 1f, 1f, 1f) ;
		}
	}

	public void OnMouseDown(){
		if(ApplicationModel.player.ToLaunchGameTutorial){
			GameView.instance.hideTuto();
		}
		if(this.isRunningSkill){
			GameView.instance.hideValidationButton();
			GameView.instance.hideTargets();
			this.updateButtonStatus(GameView.instance.getCurrentCard());
			this.isRunningSkill = false ;
		}
	}
	
	public void showSkills(GameCard card){
		if(card.getSkills()[3].IsActivated==1){
			gameObject.transform.FindChild("AttackButton").GetComponent<SkillButtonController>().setSkill(card.GetAttackSkill());
			gameObject.transform.FindChild("AttackButton").GetComponent<SkillButtonController>().getLaunchability();
			gameObject.transform.FindChild("AttackButton").GetComponent<SkillButtonController>().show(true);
			gameObject.transform.FindChild("SkillButton0").GetComponent<SkillButtonController>().setSkill(card.getSkills()[1]);
			gameObject.transform.FindChild("SkillButton0").GetComponent<SkillButtonController>().getLaunchability();
			gameObject.transform.FindChild("SkillButton0").GetComponent<SkillButtonController>().show(true);
			gameObject.transform.FindChild("SkillButton1").GetComponent<SkillButtonController>().setSkill(card.getSkills()[2]);
			gameObject.transform.FindChild("SkillButton1").GetComponent<SkillButtonController>().getLaunchability();
			gameObject.transform.FindChild("SkillButton1").GetComponent<SkillButtonController>().show(true);
			gameObject.transform.FindChild("SkillButton2").GetComponent<SkillButtonController>().setSkill(card.getSkills()[3]);
			gameObject.transform.FindChild("SkillButton2").GetComponent<SkillButtonController>().getLaunchability();
			gameObject.transform.FindChild("SkillButton2").GetComponent<SkillButtonController>().show(true);	
		}
		else if(card.getSkills()[2].IsActivated==1){
			gameObject.transform.FindChild("AttackButton").GetComponent<SkillButtonController>().setSkill(card.GetAttackSkill());
			gameObject.transform.FindChild("AttackButton").GetComponent<SkillButtonController>().getLaunchability();
			gameObject.transform.FindChild("AttackButton").GetComponent<SkillButtonController>().show(true);
			gameObject.transform.FindChild("SkillButton0").GetComponent<SkillButtonController>().setSkill(card.getSkills()[1]);
			gameObject.transform.FindChild("SkillButton0").GetComponent<SkillButtonController>().getLaunchability();
			gameObject.transform.FindChild("SkillButton0").GetComponent<SkillButtonController>().show(true);
			gameObject.transform.FindChild("SkillButton1").GetComponent<SkillButtonController>().setSkill(card.getSkills()[2]);
			gameObject.transform.FindChild("SkillButton1").GetComponent<SkillButtonController>().getLaunchability();
			gameObject.transform.FindChild("SkillButton1").GetComponent<SkillButtonController>().show(true);
			gameObject.transform.FindChild("SkillButton2").GetComponent<SkillButtonController>().show(false);
		}
		else{
			gameObject.transform.FindChild("AttackButton").GetComponent<SkillButtonController>().setSkill(card.GetAttackSkill());
			gameObject.transform.FindChild("AttackButton").GetComponent<SkillButtonController>().getLaunchability();
			gameObject.transform.FindChild("AttackButton").GetComponent<SkillButtonController>().show(true);
			gameObject.transform.FindChild("SkillButton0").GetComponent<SkillButtonController>().setSkill(card.getSkills()[1]);
			gameObject.transform.FindChild("SkillButton0").GetComponent<SkillButtonController>().getLaunchability();
			gameObject.transform.FindChild("SkillButton0").GetComponent<SkillButtonController>().show(true);
			gameObject.transform.FindChild("SkillButton1").GetComponent<SkillButtonController>().show(false);
			gameObject.transform.FindChild("SkillButton2").GetComponent<SkillButtonController>().show(false);
		}
	}

	public SkillButtonController getSkillButtonController(int i){
		if(i<3){
			return gameObject.transform.FindChild("SkillButton"+i).GetComponent<SkillButtonController>();
		}
		else{
			return gameObject.transform.FindChild("AttackButton").GetComponent<SkillButtonController>();
		}
	}
}


