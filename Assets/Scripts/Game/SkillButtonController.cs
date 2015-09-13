using UnityEngine;
using System.Collections.Generic;
using System;

public class SkillButtonController : MonoBehaviour
{
	public Skill skill ;
	public bool isLaunched = false ;
	public bool isLaunchable = false ;
	public string launchabilityText ;
	public int id ; 
	bool isHovered = false ;
	
	public void setSkill(Skill s, Sprite sprite){
		this.skill = s  ;
		this.checkLaunchability() ;
		this.isLaunched = false ;
		gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
	}
	
	public void checkLaunchability(){
		if(this.skill.Id!=-99){
			if(GameView.instance.getCard(GameController.instance.getCurrentPlayingCard()).isSleeping() && this.id!=1){
				this.launchabilityText = "Le personnage est endormi";
				gameObject.GetComponent<SpriteRenderer>().color=new Color(255/255f,120f/255f,120f/255f, 1f);
				this.isLaunchable = false ;
			}
			else if(this.skill.Id==1 || !GameView.instance.hasPlayed(GameController.instance.getCurrentPlayingCard())){
				this.launchabilityText = GameSkills.instance.getSkill(this.skill.Id).isLaunchable();
				if (this.launchabilityText.Length<4){
					gameObject.GetComponent<SpriteRenderer>().color=new Color(1f, 1f, 1f, 1f);
					this.isLaunchable = true ;
					this.launchabilityText = "";
				}
				else{
					gameObject.GetComponent<SpriteRenderer>().color=new Color(255/255f,120f/255f,120f/255f, 1f);
					this.isLaunchable = false ;
				}
			}
			else{
				this.launchabilityText = "Le personnage a déjà joué";
				gameObject.GetComponent<SpriteRenderer>().color=new Color(255/255f,120f/255f,120f/255f, 1f);
				this.isLaunchable = false ;
			}
		}
		else{
			this.launchabilityText = "Compétence non débloquée";
			this.isLaunchable = false ;
		}
	}
	
	public void OnMouseEnter(){
		if (!isHovered && !GameController.instance.getIsRunningSkill()){
			if(this.isLaunchable){
				gameObject.GetComponent<SpriteRenderer>().color=new Color(120/255f,120f/255f,1f, 1f);
				Vector3 position = gameObject.transform.position;
				position.y += 0.65f;
				GameView.instance.displayPopUp(this.skill.Description, position, this.skill.Name);
				this.isHovered = true ;
			}
			else{
				Vector3 position = gameObject.transform.position;
				position.y += 0.65f;
				GameView.instance.displayPopUp(this.launchabilityText, position, "Indisponible");
				this.isHovered = true ;
			}
		}
	}
	
	public void OnMouseExit(){
		if (isHovered && !GameController.instance.getIsRunningSkill()){
			if(this.isLaunchable){
				GameView.instance.hidePopUp();
				this.isHovered = false ;
				gameObject.GetComponent<SpriteRenderer>().color=new Color(1f, 1f, 1f, 1f);
			}
			else{
				GameView.instance.hidePopUp();
				this.isHovered = false ;
				gameObject.GetComponent<SpriteRenderer>().color=new Color(255/255f,120f/255f,120f/255f, 1f);
			}
		}
	}
	
	public void OnMouseDown(){
		if(this.isLaunched){
			gameObject.GetComponent<SpriteRenderer>().color=new Color(1f, 1f, 1f, 1f);
			GameController.instance.cancelSkill();
			this.isLaunched = false ;
		}
		else{
			if (!GameController.instance.getIsRunningSkill() && this.isLaunchable){
				gameObject.GetComponent<SpriteRenderer>().color=new Color(120f/255f,255f/255f,120f/255f, 1f);
				GameController.instance.launchSkill(this.id);
				this.isLaunched = true ;
				this.isHovered = false ;
				GameView.instance.hidePopUp();
				
			}
		}
	}
}


