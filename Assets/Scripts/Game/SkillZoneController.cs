using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class SkillZoneController : MonoBehaviour
{	
	bool isAnimated;
	float timer ;
	float animationTime = 0.50f ;
	float animationTime2 = 0.25f ;
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
	
	public void showCancelButton(bool b){
		gameObject.GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = b ;
		gameObject.GetComponent<BoxCollider>().enabled = b;
	}
	
	public void updateButtonStatus(GameCard g){
		if(this.isRunningSkill){
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f) ;
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(1f, 1f, 1f, 1f) ;
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().text = "Annuler la compétence en cours : "+GameSkills.instance.getCurrentGameSkill().name;
			this.showSkillButtons(false);
			this.showCancelButton (true);
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
			gameObject.GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
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
			GameView.instance.hideTargets();
			this.updateButtonStatus(GameView.instance.getCurrentCard());
			this.isRunningSkill = false ;
		}
	}
	
	public void showSkills(GameCard card){
		if(card.getSkills()[3].IsActivated==1){
			Vector3 position;
			Transform tempGO = gameObject.transform.FindChild("AttackButton");
			position = tempGO.transform.localPosition;
			position.x = -1.5f;
			tempGO.transform.localPosition = position;
			tempGO = gameObject.transform.FindChild("SkillButton0");
			position = tempGO.transform.localPosition;
			position.x = -0.5f;
			tempGO.transform.localPosition = position;
			tempGO = gameObject.transform.FindChild("SkillButton1");
			position = tempGO.transform.localPosition;
			position.x = 0.5f;
			tempGO.transform.localPosition = position;
			tempGO = gameObject.transform.FindChild("SkillButton2");
			position = tempGO.transform.localPosition;
			position.x = 1.5f;
			tempGO.transform.localPosition = position;
			
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
			Vector3 position;
			Transform tempGO = gameObject.transform.FindChild("AttackButton");
			position = tempGO.transform.localPosition;
			position.x = -1.1f;
			tempGO.transform.localPosition = position;
			tempGO = gameObject.transform.FindChild("SkillButton0");
			position = tempGO.transform.localPosition;
			position.x = 0f;
			tempGO.transform.localPosition = position;
			tempGO = gameObject.transform.FindChild("SkillButton1");
			position = tempGO.transform.localPosition;
			position.x = 1.1f;
			tempGO.transform.localPosition = position;
			
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
			Vector3 position;
			Transform tempGO = gameObject.transform.FindChild("AttackButton");
			position = tempGO.transform.localPosition;
			position.x = -0.8f;
			tempGO.transform.localPosition = position;
			tempGO = gameObject.transform.FindChild("SkillButton0");
			position = tempGO.transform.localPosition;
			position.x = 0.8f;
			tempGO.transform.localPosition = position;
			
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
}


