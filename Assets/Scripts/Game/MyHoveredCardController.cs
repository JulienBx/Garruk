using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class MyHoveredCardController : MonoBehaviour
{	
	float timer = 0f;
	float realwidth ;
	float animationTime = 0.25f;
	Vector3 myPosition ;
	
	int status = 0;
	bool isDisplayed = false ;
	
	int nextDisplayedCharacter ;
	int currentCharacter = -1;
	
	public void addTime(float f){
		this.timer += f;
		
		if(status>0){
			this.myPosition = gameObject.transform.localPosition;
			this.myPosition.x = (-0.5f*this.realwidth-5f)+(Mathf.Min(1,this.timer/this.animationTime))*(0.5f*realwidth-3f);
			gameObject.transform.localPosition = this.myPosition ;
			
			this.myPosition = GameObject.Find("MySpecialiteBackground").transform.localPosition;	
			this.myPosition.x = -realwidth/2f-(realwidth/2f-3f)/2f+(Mathf.Min(1,this.timer/this.animationTime))*(0.5f*realwidth-3f);
			GameObject.Find("MySpecialiteBackground").transform.localPosition = this.myPosition ;
			
			this.myPosition = GameObject.Find("MySkill1Background").transform.localPosition;	
			this.myPosition.x = -realwidth/2f-(realwidth/2f-3f)/2f+(Mathf.Min(1,this.timer/this.animationTime))*(0.5f*realwidth-3f);
			GameObject.Find("MySkill1Background").transform.localPosition = this.myPosition ;
			
			this.myPosition = GameObject.Find("MySkill2Background").transform.localPosition;	
			this.myPosition.x = -realwidth/2f-(realwidth/2f-3f)/2f+(Mathf.Min(1,this.timer/this.animationTime))*(0.5f*realwidth-3f);
			GameObject.Find("MySkill2Background").transform.localPosition = this.myPosition ;
			
			this.myPosition = GameObject.Find("MySkill3Background").transform.localPosition;	
			this.myPosition.x = -realwidth/2f-(realwidth/2f-3f)/2f+(Mathf.Min(1,this.timer/this.animationTime))*(0.5f*realwidth-3f);
			GameObject.Find("MySkill3Background").transform.localPosition = this.myPosition ;
			
			if (this.timer>this.animationTime){
				this.status = 0 ;
				this.isDisplayed = true ;
				if(GameView.instance.getIsTutorialLaunched())
				{
					if(TutorialObjectController.instance.getSequenceID()==1)
					{
						StartCoroutine(TutorialObjectController.instance.launchSequence(2));
					}
				}
			}
		}
		else if(status<0){
			
			this.myPosition = gameObject.transform.localPosition;
			this.myPosition.x = (-8f)-(Mathf.Min(1,this.timer/this.animationTime))*(0.5f*realwidth-3f);
			gameObject.transform.localPosition = this.myPosition ;
			
			this.myPosition = GameObject.Find("MySpecialiteBackground").transform.localPosition;	
			this.myPosition.x = -realwidth/2f+(realwidth/2f-3f)/2f-(Mathf.Min(1,this.timer/this.animationTime))*(0.5f*realwidth-3f);
			GameObject.Find("MySpecialiteBackground").transform.localPosition = this.myPosition ;
			
			this.myPosition = GameObject.Find("MySkill1Background").transform.localPosition;	
			this.myPosition.x = -realwidth/2f+(realwidth/2f-3f)/2f-(Mathf.Min(1,this.timer/this.animationTime))*(0.5f*realwidth-3f);
			GameObject.Find("MySkill1Background").transform.localPosition = this.myPosition ;
			
			this.myPosition = GameObject.Find("MySkill2Background").transform.localPosition;	
			this.myPosition.x = -realwidth/2f+(realwidth/2f-3f)/2f-(Mathf.Min(1,this.timer/this.animationTime))*(0.5f*realwidth-3f);
			GameObject.Find("MySkill2Background").transform.localPosition = this.myPosition ;
			
			this.myPosition = GameObject.Find("MySkill3Background").transform.localPosition;	
			this.myPosition.x = -realwidth/2f+(realwidth/2f-3f)/2f-(Mathf.Min(1,this.timer/this.animationTime))*(0.5f*realwidth-3f);
			GameObject.Find("MySkill3Background").transform.localPosition = this.myPosition ;
			
			if (this.timer>this.animationTime){
				this.status = 0 ;
				this.isDisplayed = false ;
				this.launchNextMove() ;
			}
		}
	}
	
	public void launchNextMove(){
		if (this.nextDisplayedCharacter!=-1){
			this.setCard(GameView.instance.getCard(this.nextDisplayedCharacter));
			this.status = 1 ;
			
			this.timer = 0 ;
			this.currentCharacter = this.nextDisplayedCharacter;
			this.nextDisplayedCharacter = -1 ;
		}
	}
	
	public void setNextDisplayedCharacter(int i){
		this.nextDisplayedCharacter = i;
	}
	
	public int getCurrentCharacter(){
		return this.currentCharacter;
	}
	
	public bool getIsDisplayed(){
		return this.isDisplayed;
	}
	
	public int getStatus(){
		return this.status;
	}
	
	public void hide(){
		this.timer = 0 ;
		this.status = -1 ;
	}
	
	public void reverse(int i){
		this.status = i;
		this.timer = this.animationTime - this.timer ;
	}
	
	public void resize(float realwidth, float tileScale){
		Vector3 position;
		Vector3 scale;
		
		this.realwidth = realwidth ;
		
		GameObject tempGO ; 
		Transform tempTransform ;
		
		tempTransform = gameObject.transform;
		position = tempTransform.localPosition ;
		position.x = -0.50f*realwidth-5f;
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MyPassiveSkill");
		position = tempTransform.localPosition ;
		position.x = -5f+(realwidth/2f-3f)/2f;
		tempTransform.localPosition = position;
		
		tempGO = GameObject.Find("MySpecialiteBackground");
		position = tempGO.transform.localPosition ;
		position.x = -realwidth/2f-(realwidth/2f-3f)/2f;
		tempGO.transform.localPosition = position;
		
		tempGO = GameObject.Find("MySpecialiteBackground");
		scale = tempGO.transform.localScale ;
		scale.x = (95f/100f)*(realwidth/10f)/(16f/9f);
		tempGO.transform.localScale = scale;
		
		tempGO = GameObject.Find("MySkill1Background");
		position = tempGO.transform.localPosition ;
		position.x = -realwidth/2f-(realwidth/2f-3f)/2f;
		tempGO.transform.localPosition = position;
		
		tempGO = GameObject.Find("MySkill1Background");
		scale = tempGO.transform.localScale ;
		scale.x = (95f/100f)*(realwidth/10f)/(16f/9f);
		tempGO.transform.localScale = scale;
		
		tempGO = GameObject.Find("MySkill2Background");
		position = tempGO.transform.localPosition ;
		position.x = -realwidth/2f-(realwidth/2f-3f)/2f;
		tempGO.transform.localPosition = position;
		
		tempGO = GameObject.Find("MySkill2Background");
		scale = tempGO.transform.localScale ;
		scale.x = (95f/100f)*(realwidth/10f)/(16f/9f);
		tempGO.transform.localScale = scale;
		
		tempGO = GameObject.Find("MySkill3Background");
		position = tempGO.transform.localPosition ;
		position.x = -realwidth/2f-(realwidth/2f-3f)/2f;
		tempGO.transform.localPosition = position;
		
		tempGO = GameObject.Find("MySkill3Background");
		scale = tempGO.transform.localScale ;
		scale.x = (95f/100f)*(realwidth/10f)/(16f/9f);
		tempGO.transform.localScale = scale;
		
		tempTransform = gameObject.transform.FindChild("MyPassiveSkill");
		position = tempTransform.localPosition ;
		position.x = -5f+(realwidth/2f-3f)/2f;
		tempTransform.localPosition = position;
		
		tempTransform = gameObject.transform.FindChild("MyPassiveSkill");
		position = tempTransform.localPosition ;
		position.x = -5f+(realwidth/2f-3f)/2f;
		tempTransform.localPosition = position;
		
		tempTransform = gameObject.transform.FindChild("MySkill1");
		position = tempTransform.localPosition ;
		position.x = -5f+(realwidth/2f-3f)/2f;
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MySkill2");
		position = tempTransform.localPosition ;
		position.x = -5f+(realwidth/2f-3f)/2f;
		tempTransform.localPosition = position;
		
		tempTransform = gameObject.transform.FindChild("MySkill3");
		position = tempTransform.localPosition ;
		position.x = -5f+(realwidth/2f-3f)/2f;
		tempTransform.localPosition = position;
		
		tempTransform = gameObject.transform.FindChild("MySkill1");
		position = tempTransform.localPosition ;
		position.x = -5f+(realwidth/2f-3f)/2f;
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MySkill1").FindChild("MySkill1Cristal");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(68f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MySkill2").FindChild("MySkill2Cristal");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(68f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MySkill3").FindChild("MySkill3Cristal");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(68f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MyPassiveSkill").FindChild("MySpecialiteCristal");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(68f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MySkill1").FindChild("MySkill1Title");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(55f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MySkill2").FindChild("MySkill2Title");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(55f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MySkill3").FindChild("MySkill3Title");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(55f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MyPassiveSkill").FindChild("MySpecialite");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(55f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MySkill1").FindChild("MySkill1Description");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(55f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MySkill2").FindChild("MySkill2Description");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(55f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MySkill3").FindChild("MySkill3Description");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(55f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MyPassiveSkill").FindChild("MySpecialiteDescription");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(55f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MyCarac").FindChild("MyAttackPicto");
		position = tempTransform.localPosition ;
		position.x = -(5-(realwidth/2f-3f))-(realwidth/2f-3f)*(10f/100f);
		tempTransform.localPosition = position;
		
		tempTransform = gameObject.transform.FindChild("MyCarac").FindChild("MyAttackText");
		position = tempTransform.localPosition ;
		position.x = -(5-(realwidth/2f-3f))-(realwidth/2f-3f)*(22f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MyCarac").FindChild("MyLifePicto");
		position = tempTransform.localPosition ;
		position.x = -(5-(realwidth/2f-3f))-(realwidth/2f-3f)*(40f/100f);
		tempTransform.localPosition = position;
		
		tempTransform = gameObject.transform.FindChild("MyCarac").FindChild("MyLifeText");
		position = tempTransform.localPosition ;
		position.x = -(5-(realwidth/2f-3f))-(realwidth/2f-3f)*(52f/100f);
		tempTransform.localPosition = position;
		
		tempTransform = gameObject.transform.FindChild("MyCarac").FindChild("MyHastePicto");
		position = tempTransform.localPosition ;
		position.x = -(5-(realwidth/2f-3f))-(realwidth/2f-3f)*(70f/100f);
		tempTransform.localPosition = position;
		
		tempTransform = gameObject.transform.FindChild("MyCarac").FindChild("MyHasteText");
		position = tempTransform.localPosition ;
		position.x = -(5-(realwidth/2f-3f))-(realwidth/2f-3f)*(82f/100f);
		tempTransform.localPosition = position;
	}
	
	public void setCard(Card c){
		gameObject.GetComponent<SpriteRenderer>().sprite = GameView.instance.getSprite(c.ArtIndex);
		
		gameObject.transform.FindChild("MyMainDescription").FindChild("MyTitle").GetComponent<TextMeshPro>().text = c.Title;
		gameObject.transform.FindChild("MyPassiveSkill").FindChild("MySpecialite").GetComponent<TextMeshPro>().text = c.getSkills()[0].Name;
		gameObject.transform.FindChild("MyPassiveSkill").FindChild("MySpecialiteDescription").GetComponent<TextMeshPro>().text = c.getSkills()[0].Description;
		
		gameObject.transform.FindChild("MyCarac").FindChild("MyAttackText").GetComponent<TextMeshPro>().text = ""+c.GetAttack();
		gameObject.transform.FindChild("MyCarac").FindChild("MyLifeText").GetComponent<TextMeshPro>().text = ""+c.GetLife();
		gameObject.transform.FindChild("MyCarac").FindChild("MyHasteText").GetComponent<TextMeshPro>().text = ""+c.GetSpeed();
		
		if(c.getSkills()[0].Level==1){
			gameObject.transform.FindChild("MyPassiveSkill").FindChild("MySpecialiteCristal").GetComponent<SpriteRenderer>().color=new Color(255f/255f,255f/255f,255f/255f, 1f);
		}
		else if(c.getSkills()[0].Level==2){
			gameObject.transform.FindChild("MyPassiveSkill").FindChild("MySpecialiteCristal").GetComponent<SpriteRenderer>().color=new Color(76f/255f,163f/255f,174f/255f, 1f);
		}
		else{
			gameObject.transform.FindChild("MyPassiveSkill").FindChild("MySpecialiteCristal").GetComponent<SpriteRenderer>().color=new Color(218f/255f,70f/255f,70f/255f, 1f);
		}
		
		for (int i = 1 ; i < c.getSkills().Count ; i++){
			gameObject.transform.FindChild("MySkill"+i).FindChild("MySkill"+i+"Cristal").GetComponent<SpriteRenderer>().enabled=true;
			gameObject.transform.FindChild("MySkill"+i).FindChild("MySkill"+(i)+"Title").GetComponent<TextMeshPro>().text = c.getSkills()[i].Name;
			gameObject.transform.FindChild("MySkill"+i).FindChild(("MySkill"+(i)+"Description")).GetComponent<TextMeshPro>().text = c.getSkills()[i].Description;
			if(c.getSkills()[i].Level==1){
				gameObject.transform.FindChild("MySkill"+i).FindChild("MySkill"+i+"Cristal").GetComponent<SpriteRenderer>().color=new Color(255f/255f,255f/255f,255f/255f, 1f);
			}
			else if(c.getSkills()[i].Level==2){
				gameObject.transform.FindChild("MySkill"+i).FindChild("MySkill"+i+"Cristal").GetComponent<SpriteRenderer>().color=new Color(76f/255f,163f/255f,174f/255f, 1f);
			}
			else{
				gameObject.transform.FindChild("MySkill"+i).FindChild("MySkill"+i+"Cristal").GetComponent<SpriteRenderer>().color=new Color(218f/255f,70f/255f,70f/255f, 1f);
			}
		}
		
		for (int i = c.getSkills().Count ; i < 4 ; i++){
			gameObject.transform.FindChild("MySkill"+i).FindChild("MySkill"+(i)+"Title").GetComponent<TextMeshPro>().text = "?????";
			gameObject.transform.FindChild("MySkill"+i).FindChild(("MySkill"+(i)+"Description")).GetComponent<TextMeshPro>().text = "Faites progresser l'unité pour débloquer cette compétence";
			gameObject.transform.FindChild("MySkill"+i).FindChild("MySkill"+i+"Cristal").GetComponent<SpriteRenderer>().enabled=false;
		}
	}
}


