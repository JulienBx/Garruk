using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class SkillWindowLeft : MonoBehaviour
{	
	float timeToDisplay = 0f;
	float actualTime = 0f;
	
	int nbReceivers = 0 ;
	
	public void Awake(){
		this.hideLauncher ();
		this.hideReceivers ();
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
		Vector3 position = gameObject.transform.localPosition ;
		if(this.actualTime<=1){
			position.x = -13.9f+this.actualTime*(10f);
			gameObject.transform.localPosition = position ;
		}
		else if(this.actualTime>this.timeToDisplay){
			this.hideLauncher();
			this.hideReceivers();
			this.resetTime();
			this.timeToDisplay=0;
		}
		else if(this.actualTime>this.timeToDisplay-1){
			position.x = -3.9f-(this.actualTime-this.timeToDisplay+1f)*(10f);
			gameObject.transform.localPosition = position ;
		}
	}
	
	public void hideLauncher(){
		gameObject.transform.FindChild("SkillCaster").GetComponent<SpriteRenderer>().enabled=false;
		gameObject.transform.FindChild("SkillCaster").FindChild("SkillCasterLegend").GetComponent<SpriteRenderer>().enabled=false;
		gameObject.transform.FindChild("SkillCaster").FindChild("SkillCasterLegend").FindChild("SkillCasterLegendText").GetComponent<MeshRenderer>().enabled=false;
	}
	
	public void hideReceivers(){
		for (int i = 0 ; i < 1 ; i++){
			gameObject.transform.FindChild("HitMan"+i).GetComponent<SpriteRenderer>().enabled=false;
			gameObject.transform.FindChild("HitMan"+i).FindChild("UpgradeBox").GetComponent<SpriteRenderer>().enabled=false;
			gameObject.transform.FindChild("HitMan"+i).FindChild("DowngradeBox").GetComponent<SpriteRenderer>().enabled=false;
			gameObject.transform.FindChild("HitMan"+i).FindChild("UpgradeBox").FindChild("BoxText").GetComponent<MeshRenderer>().enabled=false;
			gameObject.transform.FindChild("HitMan"+i).FindChild("DowngradeBox").FindChild("BoxText").GetComponent<MeshRenderer>().enabled=false;
			gameObject.transform.FindChild("HitMan"+i).FindChild("TextStatus").GetComponent<MeshRenderer>().enabled=false;
		}
	}
	
	public void setLauncher(string texte, Sprite s){
		gameObject.transform.FindChild("SkillCaster").GetComponent<SpriteRenderer>().sprite = s;
		gameObject.transform.FindChild("SkillCaster").FindChild("SkillCasterLegend").FindChild("SkillCasterLegendText").GetComponent<TextMeshPro>().text=texte;
		
		gameObject.GetComponent<SpriteRenderer>().enabled=true;
		gameObject.transform.FindChild("SkillCaster").GetComponent<SpriteRenderer>().enabled=true;
		gameObject.transform.FindChild("SkillCaster").FindChild("SkillCasterLegend").GetComponent<SpriteRenderer>().enabled=true;
		gameObject.transform.FindChild("SkillCaster").FindChild("SkillCasterLegend").FindChild("SkillCasterLegendText").GetComponent<MeshRenderer>().enabled=true;
	}
	
	public void setReceivers(List<Card> receivers, List<string> textesUp, List<string> textesDown){
		this.nbReceivers = receivers.Count;
		
		for (int i = 0 ; i < this.nbReceivers ; i++){
			gameObject.transform.FindChild("HitMan"+i).GetComponent<SpriteRenderer>().enabled=false;
			gameObject.transform.FindChild("HitMan"+i).FindChild("UpgradeBox").GetComponent<SpriteRenderer>().enabled=false;
			gameObject.transform.FindChild("HitMan"+i).FindChild("DowngradeBox").GetComponent<SpriteRenderer>().enabled=false;
			gameObject.transform.FindChild("HitMan"+i).FindChild("UpgradeBox").FindChild("BoxText").GetComponent<MeshRenderer>().enabled=false;
			gameObject.transform.FindChild("HitMan"+i).FindChild("DowngradeBox").FindChild("BoxText").GetComponent<MeshRenderer>().enabled=false;
			
			gameObject.transform.FindChild("HitMan"+i).FindChild("UpgradeBox").FindChild("BoxText").GetComponent<TextMeshPro>().text = textesUp[i];
			gameObject.transform.FindChild("HitMan"+i).FindChild("DowngradeBox").FindChild("BoxText").GetComponent<TextMeshPro>().text = textesDown[i];
			
			gameObject.transform.FindChild("HitMan"+i).FindChild("characterBackground").GetComponent<SpriteRenderer>().sprite = GameView.instance.getCharacterSprite(receivers[i].IdClass);
		}
	}
	
	public void setReceivers(List<Card> receivers, List<string> textesUp, List<string> textesDown, List<int> status){
		this.nbReceivers = receivers.Count;
		string text ; 
		Color color ; 
		
		for (int i = 0 ; i < this.nbReceivers ; i++){
			gameObject.transform.FindChild("HitMan"+i).GetComponent<SpriteRenderer>().enabled=false;
			gameObject.transform.FindChild("HitMan"+i).FindChild("UpgradeBox").GetComponent<SpriteRenderer>().enabled=false;
			gameObject.transform.FindChild("HitMan"+i).FindChild("DowngradeBox").GetComponent<SpriteRenderer>().enabled=false;
			gameObject.transform.FindChild("HitMan"+i).FindChild("UpgradeBox").FindChild("BoxText").GetComponent<MeshRenderer>().enabled=false;
			gameObject.transform.FindChild("HitMan"+i).FindChild("DowngradeBox").FindChild("BoxText").GetComponent<MeshRenderer>().enabled=false;
			
			gameObject.transform.FindChild("HitMan"+i).FindChild("UpgradeBox").FindChild("BoxText").GetComponent<TextMeshPro>().text = textesUp[i];
			gameObject.transform.FindChild("HitMan"+i).FindChild("DowngradeBox").FindChild("BoxText").GetComponent<TextMeshPro>().text = textesDown[i];
			
			gameObject.transform.FindChild("HitMan"+i).FindChild("characterBackground").GetComponent<SpriteRenderer>().sprite = GameView.instance.getCharacterSprite(receivers[i].IdClass);
			if(status[i]==0){
				text = "Succès";
				color = new Color(71f,150f,189f);
			}
			else if(status[i]==1){
				text = "Echec";
				color = new Color(231f,0f,66f);
			}
			else if(status[i]==2){
				text = "Esquive";
				color = new Color(231f,0f,66f);
			}
			else if(status[i]==3){
				text = "Bonus 'Généreux'\nEsquive";
				color = new Color(231f,0f,66f);
			}
			else if(status[i]==4){
				text = "Bonus 'Géant'\nEsquive";
				color = new Color(231f,0f,66f);
			}
			else{
				text = "Statut inconnu";
				color = new Color(231f,0f,66f);
			}
			gameObject.transform.FindChild("HitMan"+i).FindChild("TextStatus").GetComponent<TextMeshPro>().text = text;
			gameObject.transform.FindChild("HitMan"+i).FindChild("TextStatus").GetComponent<TextMeshPro>().color = color;
		}
	}
	
	public void resetTime(){
		this.actualTime = 0f ;
	}
}


