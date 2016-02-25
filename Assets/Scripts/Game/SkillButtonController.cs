using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class SkillButtonController : MonoBehaviour
{
	public Skill skill ;
	public bool isLaunched = false ;
	public bool isLaunchable = false ;
	public string launchabilityText ;
	public int id ; 
	
	void Awake(){
		this.showDescription(false);
	}
	
	public void setSkill(Skill s){
		this.skill = s  ;
		gameObject.GetComponent<SpriteRenderer>().sprite = GameView.instance.getSkillSprite(s.Id);
		gameObject.transform.FindChild("SkillTextZone").FindChild("Description").GetComponent<TextMeshPro>().text = s.Name;
		gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().text = s.Name;
		gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = GameView.instance.getCurrentCard().getSkillText(s.Description)+"\n\nProbabilité de succès : "+s.proba+"%";
	}
	
	public void getLaunchability(){
		this.launchabilityText = GameSkills.instance.getSkill(this.skill.Id).isLaunchable() ;
		if(this.launchabilityText.Length>1){
			gameObject.transform.FindChild("DescriptionZone").FindChild("LaunchabilityText").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
			gameObject.transform.FindChild("SkillTextZone").FindChild("Description").GetComponent<TextMeshPro>().color = new Color(80f/255f, 80f/255f, 80f/255f, 255f/255f) ;
			gameObject.GetComponent<SpriteRenderer>().color = new Color(80f/255f, 80f/255f, 80f/255f, 255f/255f) ;
		}
		else{
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
			gameObject.transform.FindChild("SkillTextZone").FindChild("Description").GetComponent<TextMeshPro>().color = new Color(1f, 1f, 1f, 1f) ;
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
		}
		gameObject.transform.FindChild("DescriptionZone").FindChild("LaunchabilityText").GetComponent<TextMeshPro>().text = this.launchabilityText;
	}

	public void setLaunchability(string s){
		this.launchabilityText = s ;
		if(this.launchabilityText.Length>1){
			gameObject.transform.FindChild("DescriptionZone").FindChild("LaunchabilityText").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
			gameObject.transform.FindChild("SkillTextZone").FindChild("Description").GetComponent<TextMeshPro>().color = new Color(80f/255f, 80f/255f, 80f/255f, 255f/255f) ;
			gameObject.GetComponent<SpriteRenderer>().color = new Color(80f/255f, 80f/255f, 80f/255f, 255f/255f) ;
		}
		else{
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
			gameObject.transform.FindChild("SkillTextZone").FindChild("Description").GetComponent<TextMeshPro>().color = new Color(1f, 1f, 1f, 1f) ;
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
		}
		gameObject.transform.FindChild("DescriptionZone").FindChild("LaunchabilityText").GetComponent<TextMeshPro>().text = this.launchabilityText;
	}
	
	public void show(bool b){
		gameObject.GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("SkillTextZone").FindChild("Description").GetComponent<MeshRenderer>().enabled = b ;
		gameObject.GetComponent<BoxCollider>().enabled = b;
	}
	
	public void showDescription(bool b){
		gameObject.transform.FindChild("DescriptionZone").GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<MeshRenderer>().enabled = b ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled = b ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("LaunchabilityText").GetComponent<MeshRenderer>().enabled = b ;
	}
	
	public void OnMouseEnter(){
		if (this.launchabilityText.Length<2){
			gameObject.GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
		}
		GameView.instance.runningSkill = this.skill.Id ;
		if(!GameSkills.instance.getCurrentGameSkill().auto){
			GameSkills.instance.getSkill(this.skill.Id).launch();
		}
		this.showDescription(true);
	}
	
	public void OnMouseExit(){
		if (this.launchabilityText.Length<2){
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
		}
		if(!GameView.instance.getSkillZoneController().isRunningSkill){
			GameView.instance.hideTargets();
			this.showDescription(false);
		}
	}
	
	public void OnMouseDown(){
		if (this.launchabilityText.Length<2){
			if(GameSkills.instance.getSkill(this.skill.Id).ciblage==0){
				GameSkills.instance.getSkill(this.skill.Id).launch();
			}
			GameView.instance.getSkillZoneController().isRunningSkill = true ;
			GameView.instance.getSkillZoneController().updateButtonStatus(GameView.instance.getCurrentCard());
			GameSkills.instance.getSkill(this.skill.Id).launch();
			this.showDescription(false);
		}
		if(ApplicationModel.player.ToLaunchGameTutorial){
			GameView.instance.hideTuto();
		}
	}
}


