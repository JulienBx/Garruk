using UnityEngine;
using System.Collections.Generic;
using System;

public class SkillButtonController : MonoBehaviour
{
	Skill skill ;
	
	public void setSkill(Skill s){
		this.skill = s ;
	}
	
	public void OnMouseEnter(){
		gameObject.GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f, 1f);
		GameView.instance.hoverSkill(this.skill);
	}
	
	public void OnMouseExit(){
		gameObject.GetComponent<SpriteRenderer>().color=new Color(1f, 1f, 1f, 1f);
	}
}


