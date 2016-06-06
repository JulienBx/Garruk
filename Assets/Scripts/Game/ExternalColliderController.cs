using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class ExternalColliderController : MonoBehaviour
{	
	public void OnMouseEnter(){
		if(GameView.instance.hasFightStarted){
			GameView.instance.hitExternalCollider();
		}
	}

	public void OnMouseDown(){
		if(GameView.instance.isDisplayedPopUp){
			GameView.instance.hideValidationButton();
			if(GameView.instance.getSkillZoneController().isRunningSkill){
				GameView.instance.hideTargets();
				GameView.instance.getSkillZoneController().updateButtonStatus(GameView.instance.getCurrentCard());
				GameView.instance.getSkillZoneController().isRunningSkill = false ;
			}
		}
	}
}


