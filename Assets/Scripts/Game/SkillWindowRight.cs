using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class SkillWindowRight : MonoBehaviour
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
			position.x = 13.9f-this.actualTime*(10f);
			gameObject.transform.localPosition = position ;
		}
		else if(this.actualTime>this.timeToDisplay){
			this.hideLauncher();
			this.resetTime();
			this.timeToDisplay=0;
		}
		else if(this.actualTime>this.timeToDisplay-1){
			position.x = 4.2f+(this.actualTime-this.timeToDisplay+1f)*(10f);
			gameObject.transform.localPosition = position ;
		}
		else{
			if (this.nbReceivers<5){
				int index = (int)Mathf.Floor(this.actualTime)-1;
				float relativeTime = this.actualTime-index-1f;
				
				if(index<nbReceivers){
					if(relativeTime<0.5f){
						Quaternion rotation = gameObject.transform.FindChild("HitMan"+index).FindChild("TextStatus").rotation ;
						rotation = Quaternion.Euler(rotation.x, rotation.y, 420f*(relativeTime*2f));
						gameObject.transform.FindChild("HitMan"+index).FindChild("TextStatus").rotation = rotation ;
						Vector3 scale = new Vector3(1.5f*relativeTime,1.5f*relativeTime,1.5f*relativeTime);
						gameObject.transform.FindChild("HitMan"+index).FindChild("TextStatus").localScale = scale ;	
					}
					else{
						relativeTime-=0.5f;
						Vector3 scale = new Vector3(0.8f*relativeTime,0.8f*relativeTime,0.8f*relativeTime);
						gameObject.transform.FindChild("HitMan"+index).FindChild("PVBox").localScale = scale ;
//						pos = new Vector3(0f,-3.4f+2f*relativeTime,0f);
//						gameObject.transform.FindChild("HitMan"+index).FindChild("PV").localPosition = pos ;
//						pos = new Vector3(1.4f,-3.4f+2f*relativeTime,0f);
//						gameObject.transform.FindChild("HitMan"+index).FindChild("Move").localPosition = pos ;
					}
				}
			}
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
			gameObject.transform.FindChild("HitMan"+i).FindChild("PVBox").GetComponent<SpriteRenderer>().enabled=false;
			gameObject.transform.FindChild("HitMan"+i).FindChild("PVBox").FindChild("BoxText").GetComponent<MeshRenderer>().enabled=false;
			gameObject.transform.FindChild("HitMan"+i).FindChild("PV").GetComponent<MeshRenderer>().enabled=false;
			gameObject.transform.FindChild("HitMan"+i).FindChild("Attack").GetComponent<MeshRenderer>().enabled=false;
			gameObject.transform.FindChild("HitMan"+i).FindChild("Move").GetComponent<MeshRenderer>().enabled=false;
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
	
	public void setReceivers(List<Card> receivers, List<string> textesPV, List<string> textesAttack, List<string> textesMove, List<string> textesEffect, List<int> status){
		this.nbReceivers = receivers.Count;
		string text ; 
		Color color ; 
		
		for (int i = 0 ; i < this.nbReceivers ; i++){
			gameObject.transform.FindChild("HitMan"+i).GetComponent<SpriteRenderer>().enabled=true;
			gameObject.transform.FindChild("HitMan"+i).FindChild("PV").GetComponent<MeshRenderer>().enabled = true; 
			gameObject.transform.FindChild("HitMan"+i).FindChild("Attack").GetComponent<MeshRenderer>().enabled = true; 
			gameObject.transform.FindChild("HitMan"+i).FindChild("TextStatus").GetComponent<MeshRenderer>().enabled = true;
			gameObject.transform.FindChild("HitMan"+i).FindChild("PV").GetComponent<TextMeshPro>().text = receivers[i].GetLifeString(); 
			gameObject.transform.FindChild("HitMan"+i).FindChild("Attack").GetComponent<TextMeshPro>().text = receivers[i].GetAttackString(); 
			gameObject.transform.FindChild("HitMan"+i).FindChild("Move").GetComponent<TextMeshPro>().text = receivers[i].GetMoveString(); 
			if(textesPV[i]!=""){
				gameObject.transform.FindChild("HitMan"+i).FindChild("PVBox").GetComponent<SpriteRenderer>().enabled=true;
				gameObject.transform.FindChild("HitMan"+i).FindChild("PVBox").FindChild("BoxText").GetComponent<MeshRenderer>().enabled=true;
				gameObject.transform.FindChild("HitMan"+i).FindChild("PVBox").FindChild("BoxText").GetComponent<TextMeshPro>().text = textesPV[i];
				gameObject.transform.FindChild("HitMan"+i).FindChild("PVBox").localScale = new Vector3(0f, 0f, 0f);
				if(textesPV[i].StartsWith("-")){
					gameObject.transform.FindChild("HitMan"+i).FindChild("PV").GetComponent<TextMeshPro>().color = Color.yellow ;
					gameObject.transform.FindChild("HitMan"+i).FindChild("PVBox").GetComponent<SpriteRenderer>().color = Color.yellow ;
				}
				else{
					gameObject.transform.FindChild("HitMan"+i).FindChild("PVBox").GetComponent<SpriteRenderer>().color = Color.green ;
					gameObject.transform.FindChild("HitMan"+i).FindChild("PV").GetComponent<TextMeshPro>().color = Color.green ;
				}
			}
			
			gameObject.transform.FindChild("HitMan"+i).FindChild("TextStatus").GetComponent<MeshRenderer>().enabled=true;
			gameObject.transform.FindChild("HitMan"+i).FindChild("characterBackground").GetComponent<SpriteRenderer>().sprite = GameView.instance.getCharacterSprite(receivers[i].IdClass);
			
			gameObject.transform.FindChild("HitMan"+i).FindChild("TextStatus").localScale = new Vector3(0f, 0f, 0f);
			
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


