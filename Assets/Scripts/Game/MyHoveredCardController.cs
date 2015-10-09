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
		
		this.realwidth = realwidth ;
		
		Transform tempTransform ;
		
		tempTransform = gameObject.transform;
		position = tempTransform.localPosition ;
		position.x = -0.50f*realwidth-5f;
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MyPassiveSkill");
		position = tempTransform.localPosition ;
		position.x = -5f+(realwidth/2f-3f)/2f;
		tempTransform.localPosition = position;
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill1");
		position = tempTransform.localPosition ;
		position.x = -5f+(realwidth/2f-3f)/2f;
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill2");
		position = tempTransform.localPosition ;
		position.x = -5f+(realwidth/2f-3f)/2f;
		tempTransform.localPosition = position;
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill3");
		position = tempTransform.localPosition ;
		position.x = -5f+(realwidth/2f-3f)/2f;
		tempTransform.localPosition = position;
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill1").FindChild("MySkill1Cristal");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(85f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill2").FindChild("MySkill2Cristal");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(85f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill3").FindChild("MySkill3Cristal");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(85f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MyPassiveSkill").FindChild("MySpecialiteCristal");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(85f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill1").FindChild("MySkill1Title");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(70f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill2").FindChild("MySkill2Title");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(70f/100f);
		tempTransform.localPosition = position;
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill3").FindChild("MySkill3Title");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(70f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MyPassiveSkill").FindChild("MySpecialite");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(70f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill1").FindChild("MySkill1Description");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(70f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill2").FindChild("MySkill2Description");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(70f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill3").FindChild("MySkill3Description");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(70f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MyPassiveSkill").FindChild("MySpecialiteDescription");
		position = tempTransform.localPosition ;
		position.x = ((realwidth/2f-3f)/2f)*(70f/100f);
		tempTransform.localPosition = position;
		
		gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill1").FindChild("MySkill1Description").GetComponent<TextContainer>().width=((realwidth/2f-3f))*(70f/100f);
		gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill2").FindChild("MySkill2Description").GetComponent<TextContainer>().width=((realwidth/2f-3f))*(70f/100f);
		gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill3").FindChild("MySkill3Description").GetComponent<TextContainer>().width=((realwidth/2f-3f))*(70f/100f);
		gameObject.transform.FindChild("MyMainDescription").FindChild("MyPassiveSkill").FindChild("MySpecialiteDescription").GetComponent<TextContainer>().width=((realwidth/2f-3f))*(70f/100f);
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MyCarac").FindChild("MyAttackPicto");
		position = tempTransform.localPosition ;
		position.x = -(5-(realwidth/2f-3f))-(realwidth/2f-3f)*(10f/100f);
		tempTransform.localPosition = position;
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MyCarac").FindChild("MyAttackText");
		position = tempTransform.localPosition ;
		position.x = -(5-(realwidth/2f-3f))-(realwidth/2f-3f)*(22f/100f);
		tempTransform.localPosition = position;	
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MyCarac").FindChild("MyLifePicto");
		position = tempTransform.localPosition ;
		position.x = -(5-(realwidth/2f-3f))-(realwidth/2f-3f)*(39f/100f);
		tempTransform.localPosition = position;
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MyCarac").FindChild("MyLifeText");
		position = tempTransform.localPosition ;
		position.x = -(5-(realwidth/2f-3f))-(realwidth/2f-3f)*(53f/100f);
		tempTransform.localPosition = position;
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MyCarac").FindChild("MyHastePicto");
		position = tempTransform.localPosition ;
		position.x = -(5-(realwidth/2f-3f))-(realwidth/2f-3f)*(70f/100f);
		tempTransform.localPosition = position;
		
		tempTransform = gameObject.transform.FindChild("MyMainDescription").FindChild("MyCarac").FindChild("MyHasteText");
		position = tempTransform.localPosition ;
		position.x = -(5-(realwidth/2f-3f))-(realwidth/2f-3f)*(82f/100f);
		tempTransform.localPosition = position;
	}
	
	public void setCard(Card c){
		gameObject.GetComponent<SpriteRenderer>().sprite = GameView.instance.getSprite(c.ArtIndex);
		
		gameObject.transform.FindChild("MyMainDescription").FindChild("MyTitle").GetComponent<TextMeshPro>().text = c.Title;
		gameObject.transform.FindChild("MyMainDescription").FindChild("MyPassiveSkill").FindChild("MySpecialite").GetComponent<TextMeshPro>().text = c.getSkills()[0].Name;
		gameObject.transform.FindChild("MyMainDescription").FindChild("MyPassiveSkill").FindChild("MySpecialiteDescription").GetComponent<TextMeshPro>().text = c.getSkills()[0].Description;
		
		gameObject.transform.FindChild("MyMainDescription").FindChild("MyCarac").FindChild("MyAttackText").GetComponent<TextMeshPro>().text = ""+c.GetAttackString();
		gameObject.transform.FindChild("MyMainDescription").FindChild("MyCarac").FindChild("MyLifeText").GetComponent<TextMeshPro>().text = ""+c.GetLifeString();
		gameObject.transform.FindChild("MyMainDescription").FindChild("MyCarac").FindChild("MyHasteText").GetComponent<TextMeshPro>().text = ""+c.GetMoveString();
		
		if(c.GetMove()>c.Move){
			gameObject.transform.FindChild("MyMainDescription").FindChild("MyCarac").FindChild("MyHasteText").GetComponent<TextMeshPro>().color = Color.green;
		}
		else if(c.GetMove()<c.Move){
			gameObject.transform.FindChild("MyMainDescription").FindChild("MyCarac").FindChild("MyHasteText").GetComponent<TextMeshPro>().color = Color.red;
		}
		else{
			gameObject.transform.FindChild("MyMainDescription").FindChild("MyCarac").FindChild("MyHasteText").GetComponent<TextMeshPro>().color = Color.white;
		}
		
		if(c.GetAttack()>c.Attack){
			gameObject.transform.FindChild("MyMainDescription").FindChild("MyCarac").FindChild("MyAttackText").GetComponent<TextMeshPro>().color = Color.green;
		}
		else if(c.GetAttack()<c.Attack){
			gameObject.transform.FindChild("MyMainDescription").FindChild("MyCarac").FindChild("MyAttackText").GetComponent<TextMeshPro>().color = Color.red;
		}
		else{
			gameObject.transform.FindChild("MyMainDescription").FindChild("MyCarac").FindChild("MyAttackText").GetComponent<TextMeshPro>().color = Color.white;
		}
		
		if(c.GetLife()>c.Life){
			gameObject.transform.FindChild("MyMainDescription").FindChild("MyCarac").FindChild("MyLifeText").GetComponent<TextMeshPro>().color = Color.green;
		}
		else if(c.GetLife()<c.Life){
			gameObject.transform.FindChild("MyMainDescription").FindChild("MyCarac").FindChild("MyLifeText").GetComponent<TextMeshPro>().color = Color.red;
		}
		else{
			gameObject.transform.FindChild("MyMainDescription").FindChild("MyCarac").FindChild("MyLifeText").GetComponent<TextMeshPro>().color = Color.white;
		}
		
		if(c.getSkills()[0].Level==1){
			gameObject.transform.FindChild("MyMainDescription").FindChild("MyPassiveSkill").FindChild("MySpecialiteCristal").GetComponent<SpriteRenderer>().color=new Color(255f/255f,255f/255f,255f/255f, 1f);
		}
		else if(c.getSkills()[0].Level==2){
			gameObject.transform.FindChild("MyMainDescription").FindChild("MyPassiveSkill").FindChild("MySpecialiteCristal").GetComponent<SpriteRenderer>().color=new Color(71f/255f,150f/255f,189f/255f, 1f);
		}
		else{
			gameObject.transform.FindChild("MyMainDescription").FindChild("MyPassiveSkill").FindChild("MySpecialiteCristal").GetComponent<SpriteRenderer>().color=new Color(210f/255f,3f/255f,64f/255f, 1f);
		}
		
		for (int i = 1 ; i < c.getSkills().Count ; i++){
			gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill"+i).FindChild("MySkill"+i+"Cristal").GetComponent<SpriteRenderer>().enabled=true;
			gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill"+i).FindChild("MySkill"+i+"Title").GetComponent<MeshRenderer>().enabled=true;
			gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill"+i).FindChild("MySkill"+i+"Description").GetComponent<MeshRenderer>().enabled=true;
			gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill"+i+"Background").GetComponent<SpriteRenderer>().enabled=true;
			
			gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill"+i).FindChild("MySkill"+(i)+"Title").GetComponent<TextMeshPro>().text = c.getSkills()[i].Name;
			gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill"+i).FindChild(("MySkill"+(i)+"Description")).GetComponent<TextMeshPro>().text = c.getSkills()[i].Description;
			if(c.getSkills()[i].Level==1){
				gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill"+i).FindChild("MySkill"+i+"Cristal").GetComponent<SpriteRenderer>().color=new Color(255f/255f,255f/255f,255f/255f, 1f);
			}
			else if(c.getSkills()[i].Level==2){
				gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill"+i).FindChild("MySkill"+i+"Cristal").GetComponent<SpriteRenderer>().color=new Color(71f/255f,150f/255f,189f/255f, 1f);
			}
			else{
				gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill"+i).FindChild("MySkill"+i+"Cristal").GetComponent<SpriteRenderer>().color=new Color(210f/255f,3f/255f,64f/255f, 1f);
			}
		}
		
		for (int i = c.getSkills().Count ; i < 4 ; i++){
			gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill"+i+"Background").GetComponent<SpriteRenderer>().enabled=false;
			gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill"+i).FindChild("MySkill"+i+"Cristal").GetComponent<SpriteRenderer>().enabled=false;
			gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill"+i).FindChild("MySkill"+i+"Title").GetComponent<MeshRenderer>().enabled=false;
			gameObject.transform.FindChild("MyMainDescription").FindChild("MySkill"+i).FindChild("MySkill"+i+"Description").GetComponent<MeshRenderer>().enabled=false;
		}
	}
}


