using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class SkillPopUpController : MonoBehaviour
{	
	bool isHovered = false ;
	float timeToDisplay ;
	float actualTime ;
	
	public void Awake(){
		//gameObject.transform.FindChild("Receiver0").GetComponent
	}
	
	public void setTimeToDisplay(float a){
		this.timeToDisplay = a 	;
	}
	
	public float getTimeToDisplay(){
		return this.timeToDisplay ;
	}
	
	public void setActualTime(float a){
		this.actualTime = a ;
	}
	
	public float getActualTime(){
		return this.actualTime ;
	}
	
	public void addTime(float delta){
		this.actualTime += delta;
		if(this.actualTime>this.timeToDisplay && (!GameView.instance.getIsTutorialLaunched() || TutorialObjectController.instance.getSequenceID()!=22)){
			this.hide();
			this.resetTime();
			this.timeToDisplay=0;
		}
	}
	
	public void hide(){
		gameObject.GetComponent<SpriteRenderer>().enabled=false;
		gameObject.transform.FindChild("Launcher").GetComponent<PlayingCardController>().hide();
		gameObject.transform.FindChild("LauncherText").GetComponent<MeshRenderer>().enabled=false;
		gameObject.transform.FindChild("Receiver0").GetComponent<PlayingCardController>().hide();
		gameObject.transform.FindChild("Receiver1").GetComponent<PlayingCardController>().hide();
		gameObject.transform.FindChild("Receiver2").GetComponent<PlayingCardController>().hide();
		gameObject.transform.FindChild("Receiver3").GetComponent<PlayingCardController>().hide();
		gameObject.transform.FindChild("Receiver4").GetComponent<PlayingCardController>().hide();
		gameObject.transform.FindChild("Receiver0Text").GetComponent<MeshRenderer>().enabled=false;
		gameObject.transform.FindChild("Receiver1Text").GetComponent<MeshRenderer>().enabled=false;
		gameObject.transform.FindChild("Receiver2Text").GetComponent<MeshRenderer>().enabled=false;
		gameObject.transform.FindChild("Receiver3Text").GetComponent<MeshRenderer>().enabled=false;
		gameObject.transform.FindChild("Receiver4Text").GetComponent<MeshRenderer>().enabled=false;	
	}
	
	public void setPopUp(string texte, Card launcher, List<Card> receivers, List<string> textsReceivers){
		gameObject.GetComponent<SpriteRenderer>().enabled=true;
		
		gameObject.transform.FindChild("Launcher").GetComponent<PlayingCardController>().setCard(launcher);
		gameObject.transform.FindChild("Launcher").GetComponent<PlayingCardController>().display(true);
		gameObject.transform.FindChild("Launcher").GetComponent<PlayingCardController>().show(false);
		gameObject.transform.FindChild("LauncherText").GetComponent<TextMeshPro>().text=texte;
		gameObject.transform.FindChild("LauncherText").GetComponent<MeshRenderer>().enabled=true;
		
		if(receivers.Count==0){
			
		}
		if(receivers.Count==1){
			gameObject.transform.FindChild(("Receiver2")).GetComponent<PlayingCardController>().setCard(receivers[0]);
			gameObject.transform.FindChild(("Receiver2")).GetComponent<PlayingCardController>().display(true);
			gameObject.transform.FindChild(("Receiver2")).GetComponent<PlayingCardController>().show(false);
			gameObject.transform.FindChild("Receiver2Text").GetComponent<TextMeshPro>().text=textsReceivers[0];
			gameObject.transform.FindChild("Receiver2Text").GetComponent<MeshRenderer>().enabled=true;
		}
		else if(receivers.Count==2){
			gameObject.transform.FindChild(("Receiver1")).GetComponent<PlayingCardController>().setCard(receivers[0]);
			gameObject.transform.FindChild(("Receiver1")).GetComponent<PlayingCardController>().display(true);
			gameObject.transform.FindChild(("Receiver1")).GetComponent<PlayingCardController>().show(false);
			gameObject.transform.FindChild("Receiver1Text").GetComponent<TextMeshPro>().text=textsReceivers[0];
			gameObject.transform.FindChild("Receiver1Text").GetComponent<MeshRenderer>().enabled=true;
			gameObject.transform.FindChild(("Receiver3")).GetComponent<PlayingCardController>().setCard(receivers[1]);
			gameObject.transform.FindChild(("Receiver3")).GetComponent<PlayingCardController>().display(true);
			gameObject.transform.FindChild(("Receiver3")).GetComponent<PlayingCardController>().show(false);
			
			gameObject.transform.FindChild("Receiver3Text").GetComponent<TextMeshPro>().text=textsReceivers[1];
			gameObject.transform.FindChild("Receiver3Text").GetComponent<MeshRenderer>().enabled=true;
		}
		else if(receivers.Count==3){
			gameObject.transform.FindChild(("Receiver1")).GetComponent<PlayingCardController>().setCard(receivers[0]);
			gameObject.transform.FindChild(("Receiver1")).GetComponent<PlayingCardController>().display(true);
			gameObject.transform.FindChild(("Receiver1")).GetComponent<PlayingCardController>().show(false);
			
			gameObject.transform.FindChild("Receiver1Text").GetComponent<TextMeshPro>().text=textsReceivers[0];
			gameObject.transform.FindChild("Receiver1Text").GetComponent<MeshRenderer>().enabled=true;
			
			gameObject.transform.FindChild(("Receiver2")).GetComponent<PlayingCardController>().setCard(receivers[1]);
			gameObject.transform.FindChild(("Receiver2")).GetComponent<PlayingCardController>().display(true);
			gameObject.transform.FindChild(("Receiver2")).GetComponent<PlayingCardController>().show(false);
			
			gameObject.transform.FindChild("Receiver2Text").GetComponent<TextMeshPro>().text=textsReceivers[1];
			gameObject.transform.FindChild("Receiver2Text").GetComponent<MeshRenderer>().enabled=true;
			
			gameObject.transform.FindChild(("Receiver3")).GetComponent<PlayingCardController>().setCard(receivers[2]);
			gameObject.transform.FindChild(("Receiver3")).GetComponent<PlayingCardController>().display(true);
			gameObject.transform.FindChild(("Receiver3")).GetComponent<PlayingCardController>().show(false);
			
			gameObject.transform.FindChild("Receiver3Text").GetComponent<TextMeshPro>().text=textsReceivers[2];
			gameObject.transform.FindChild("Receiver3Text").GetComponent<MeshRenderer>().enabled=true;
			
		}
		else if(receivers.Count==4){
			gameObject.transform.FindChild(("Receiver0")).GetComponent<PlayingCardController>().setCard(receivers[0]);
			gameObject.transform.FindChild(("Receiver0")).GetComponent<PlayingCardController>().display(true);
			gameObject.transform.FindChild(("Receiver0")).GetComponent<PlayingCardController>().show(false);
			
			gameObject.transform.FindChild("Receiver0Text").GetComponent<TextMeshPro>().text=textsReceivers[0];
			gameObject.transform.FindChild("Receiver0Text").GetComponent<MeshRenderer>().enabled=true;
			gameObject.transform.FindChild(("Receiver1")).GetComponent<PlayingCardController>().setCard(receivers[1]);
			gameObject.transform.FindChild(("Receiver1")).GetComponent<PlayingCardController>().display(true);
			gameObject.transform.FindChild(("Receiver1")).GetComponent<PlayingCardController>().show(false);
			
			gameObject.transform.FindChild("Receiver1Text").GetComponent<TextMeshPro>().text=textsReceivers[1];
			gameObject.transform.FindChild("Receiver1Text").GetComponent<MeshRenderer>().enabled=true;
			gameObject.transform.FindChild(("Receiver3")).GetComponent<PlayingCardController>().setCard(receivers[2]);
			gameObject.transform.FindChild(("Receiver3")).GetComponent<PlayingCardController>().display(true);
			gameObject.transform.FindChild(("Receiver3")).GetComponent<PlayingCardController>().show(false);
			
			gameObject.transform.FindChild("Receiver3Text").GetComponent<TextMeshPro>().text=textsReceivers[2];
			gameObject.transform.FindChild("Receiver3Text").GetComponent<MeshRenderer>().enabled=true;
			gameObject.transform.FindChild(("Receiver4")).GetComponent<PlayingCardController>().setCard(receivers[3]);
			gameObject.transform.FindChild(("Receiver4")).GetComponent<PlayingCardController>().display(true);
			gameObject.transform.FindChild(("Receiver4")).GetComponent<PlayingCardController>().show(false);
			
			gameObject.transform.FindChild("Receiver4Text").GetComponent<TextMeshPro>().text=textsReceivers[3];
			gameObject.transform.FindChild("Receiver4Text").GetComponent<MeshRenderer>().enabled=true;
		}
		else if(receivers.Count>4){
			gameObject.transform.FindChild(("Receiver0")).GetComponent<PlayingCardController>().setCard(receivers[0]);
			gameObject.transform.FindChild(("Receiver0")).GetComponent<PlayingCardController>().display(true);
			gameObject.transform.FindChild(("Receiver0")).GetComponent<PlayingCardController>().show(false);
			
			gameObject.transform.FindChild("Receiver0Text").GetComponent<TextMeshPro>().text=textsReceivers[0];
			gameObject.transform.FindChild("Receiver0Text").GetComponent<MeshRenderer>().enabled=true;
			gameObject.transform.FindChild(("Receiver1")).GetComponent<PlayingCardController>().setCard(receivers[1]);
			gameObject.transform.FindChild("Receiver1Text").GetComponent<TextMeshPro>().text=textsReceivers[1];
			gameObject.transform.FindChild("Receiver1Text").GetComponent<MeshRenderer>().enabled=true;
			gameObject.transform.FindChild(("Receiver1")).GetComponent<PlayingCardController>().display(true);
			gameObject.transform.FindChild(("Receiver1")).GetComponent<PlayingCardController>().show(false);
			
			gameObject.transform.FindChild(("Receiver2")).GetComponent<PlayingCardController>().setCard(receivers[2]);
			gameObject.transform.FindChild(("Receiver2")).GetComponent<PlayingCardController>().display(true);
			gameObject.transform.FindChild("Receiver2Text").GetComponent<TextMeshPro>().text=textsReceivers[2];
			gameObject.transform.FindChild("Receiver2Text").GetComponent<MeshRenderer>().enabled=true;
			gameObject.transform.FindChild(("Receiver3")).GetComponent<PlayingCardController>().setCard(receivers[3]);
			gameObject.transform.FindChild(("Receiver3")).GetComponent<PlayingCardController>().display(true);
			gameObject.transform.FindChild(("Receiver3")).GetComponent<PlayingCardController>().show(false);
			
			gameObject.transform.FindChild("Receiver3Text").GetComponent<TextMeshPro>().text=textsReceivers[3];
			gameObject.transform.FindChild("Receiver3Text").GetComponent<MeshRenderer>().enabled=true;
			gameObject.transform.FindChild(("Receiver4")).GetComponent<PlayingCardController>().setCard(receivers[4]);
			gameObject.transform.FindChild(("Receiver4")).GetComponent<PlayingCardController>().display(true);
			gameObject.transform.FindChild(("Receiver4")).GetComponent<PlayingCardController>().show(false);
			
			gameObject.transform.FindChild("Receiver4Text").GetComponent<TextMeshPro>().text=textsReceivers[4];
			gameObject.transform.FindChild("Receiver4Text").GetComponent<MeshRenderer>().enabled=true;
		}
	}
	
	public void resetTime(){
		this.actualTime = 0f ;
	}
}


