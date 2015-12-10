using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class InterludeController : MonoBehaviour
{	
	public float realwidth ;
	public float animationTime = 0.4f;
	public float time;
	public bool isRunning;
	public bool isDisplayedCharacter;
	public Sprite[] sprites ;
	public Sprite[] characterSprites ;
	bool isEnabledUnderText = false ;
	
	bool isPaused ;
		
	void Awake(){
		this.isRunning = false ;
		gameObject.GetComponent<SpriteRenderer>().enabled = false ;
		gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().enabled = false ;
		gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().enabled = false ;
		gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().enabled = false ;
		gameObject.transform.FindChild("UnderText").GetComponent<MeshRenderer>().enabled = false ;
		gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = false ;
		this.isPaused = false ;
		isEnabledUnderText = false ;
	}
	
	public void resize(float realwidth){
		this.realwidth = realwidth ;
	}
	
	public void OnMouseDown(){
		if(this.isPaused){
			this.unPause();
		}
		else{
			if(this.time>1*this.animationTime){
				this.time=5*this.animationTime;
			}
		}
	}
	
	public void pause(){
		this.isPaused = true;
	}
	
	public void unPause(){
		this.isPaused = false;
		GameView.instance.blockFury = true ;
	}
	
	public void set(string s, bool isMine, bool displayCharacter){
		this.isDisplayedCharacter = displayCharacter;
		gameObject.GetComponent<SpriteRenderer>().enabled = true ;
		Vector3 position ;
		position = gameObject.transform.FindChild("Bar1").localPosition ;
		position.x = realwidth/2f+10f;
		gameObject.transform.FindChild("Bar1").localPosition = position;
		position = gameObject.transform.FindChild("Bar2").localPosition ;
		position.x = -realwidth/2f-10f;
		gameObject.transform.FindChild("Bar2").localPosition = position;
		position = gameObject.transform.FindChild("Bar3").localPosition ;
		position.x = realwidth/2f+10f;
		gameObject.transform.FindChild("Bar3").localPosition = position;
		if(isMine){
			gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().sprite = this.sprites[0];
			gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().sprite = this.sprites[1];
			gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().sprite = this.sprites[2];
		}
		else{
			gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().sprite = this.sprites[3];
			gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().sprite = this.sprites[4];
			gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().sprite = this.sprites[5];
		}
		if(displayCharacter){
			gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().enabled = true ;
		}
		else{
			gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().enabled = false;
			gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().enabled = false;
			gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().enabled = false ;
		}
		
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().text = s ;
		gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = true ;
		gameObject.transform.FindChild("UnderText").GetComponent<MeshRenderer>().enabled = false ;
		isEnabledUnderText = false ;
		
		GameView.instance.getMyHoveredCardController().lowerCharacter();
		GameView.instance.getHisHoveredCardController().lowerCharacter();
		if(displayCharacter){
			if(GameView.instance.getCurrentCard().isMine){
				GameView.instance.getMyHoveredCardController().raiseCharacter();
			}
			else{
				GameView.instance.getHisHoveredCardController().raiseCharacter();
			}
		}
		
		this.time=0f;
		this.isRunning = true ;
	}
	
	public void addTime(float f){
		if(!isPaused){
			this.time += f ;
		}
		
		if(this.time>4f*this.animationTime){
			gameObject.GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = false ;
			gameObject.transform.FindChild("UnderText").GetComponent<MeshRenderer>().enabled = false ;
			isEnabledUnderText = false;
			
			if(GameView.instance.getCard(GameView.instance.getCurrentPlayingCard()).isMine){
				if(ApplicationModel.launchGameTutorial){
					if(!GameView.instance.hasStep2){
						GameView.instance.launchTutoStep(2);
						GameView.instance.hasStep2 = true ;
					}
				}
				
				GameView.instance.SB.GetComponent<StartButtonController>().showText(false);
				GameView.instance.getSkillZoneController().isRunningSkill = false ;
				GameView.instance.updateActionStatus();
				GameView.instance.getMyHoveredCardController().lowerCharacter();
			}
			else{
				GameView.instance.skillZone.GetComponent<SkillZoneController>().showCancelButton(false);
				GameView.instance.skillZone.GetComponent<SkillZoneController>().showSkillButtons(false);
				GameView.instance.getMoveZoneController().show(false);
				GameView.instance.getPassZoneController().show(false);
				GameView.instance.SB.GetComponent<StartButtonController>().setText("En attente du joueur adverse");
				GameView.instance.SB.GetComponent<StartButtonController>().showText(true);
				GameView.instance.getHisHoveredCardController().lowerCharacter();
			}
			this.isRunning = false ;
		}
		else if(this.time>3f*this.animationTime){
			Vector3 position ;
			float rapport = Mathf.Min(1,(this.time-3f*this.animationTime)/this.animationTime);
			position = gameObject.transform.FindChild("Bar1").localPosition ;
			position.x = -realwidth/2f+11f+(realwidth-1f)*rapport;
			gameObject.transform.FindChild("Bar1").localPosition = position;
			position = gameObject.transform.FindChild("Bar2").localPosition ;
			position.x = realwidth/2f-11f+(realwidth-1f)*rapport;
			gameObject.transform.FindChild("Bar2").localPosition = position;
			position = gameObject.transform.FindChild("Bar3").localPosition ;
			position.x = -realwidth/2f+10.5f-(realwidth-0.5f)*rapport;
			gameObject.transform.FindChild("Bar3").localPosition = position;
			gameObject.transform.FindChild("Text").localScale=new Vector3(1-rapport, 1-rapport, 1-rapport);
		}
		else if(this.time<this.animationTime){
			Vector3 position ;
			float rapport = Mathf.Min(1,(this.time)/this.animationTime);
			position = gameObject.transform.FindChild("Bar1").localPosition ;
			position.x = realwidth/2f+10f-(realwidth-1f)*rapport;
			gameObject.transform.FindChild("Bar1").localPosition = position;
			position = gameObject.transform.FindChild("Bar2").localPosition ;
			position.x = -realwidth/2f-10f+(realwidth-1.5f)*rapport;
			gameObject.transform.FindChild("Bar2").localPosition = position;
			position = gameObject.transform.FindChild("Bar3").localPosition ;
			position.x = realwidth/2f+10f-(realwidth-0.5f)*rapport;
			gameObject.transform.FindChild("Bar3").localPosition = position;
			gameObject.transform.FindChild("Text").localScale=new Vector3(rapport, rapport, rapport);
		}
		else if(!isEnabledUnderText){
			isEnabledUnderText = true ;
			gameObject.transform.FindChild("UnderText").GetComponent<MeshRenderer>().enabled = true ;
		}
	}	
	
	public bool getIsRunning(){
		return this.isRunning;
	}
	
	public void setUnderText(string s){
		gameObject.transform.FindChild("UnderText").GetComponent<TextMeshPro>().text = s ;
		isEnabledUnderText = false ;
	}
}