using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class HisHoveredCardController : HoveredCardController
{	
	public void addTime(float f){
		this.timer += f;
		
		if(base.getStatus()>0){
			Vector3 position = gameObject.transform.localPosition;
			if(!GameView.instance.isMobile){
				position.x = (0.5f*this.realwidth+7f)-(Mathf.Min(1,this.timer/this.animationTime))*(0.5f*realwidth-1f);
			}
			else{
				position.x = (0.5f*this.realwidth+5f)-(Mathf.Min(1,this.timer/this.animationTime))*(realwidth);
			}
			gameObject.transform.localPosition = position ;
			if (this.timer>this.animationTime){
				base.setStatus(0);
				base.currentCharacter = this.nextDisplayedCharacter;
				if(base.currentCharacter==GameView.instance.getCurrentPlayingCard()){
					base.run();
				}
				base.timer=0f;
			}
		}
		else if(base.getStatus()<0){
			Vector3 position = gameObject.transform.localPosition;
			if(!GameView.instance.isMobile){
				position.x = (10f)+(Mathf.Min(1,this.timer/this.animationTime))*(0.5f*realwidth-1f);
			}
			else{
				position.x = (-0.5f*this.realwidth+5f)+(Mathf.Min(1,this.timer/this.animationTime))*(realwidth);
			}
			gameObject.transform.localPosition = position ;
			
			if (this.timer>this.animationTime){
				if(this.nextDisplayedCharacter!=-1){
					base.setCard(this.nextDisplayedCard);
					base.setStatus(1);
					base.timer = 0f;
				}
			}
		}
	}
	
	public void setNextDisplayedCharacter(int i){
		this.nextDisplayedCharacter = i;
	}
	
	public void hide(){
		this.timer = 0 ;
		base.setStatus(-1);
		base.stopAnim();
	}
	
	public void reverse(int i){
		base.setStatus(i);
		this.timer = this.animationTime - this.timer ;
	}
	
	public void resize(float realwidth, float tileScale){
		Vector3 position;
		
		base.realwidth = realwidth ;
		
		Transform tempTransform ;
		
		tempTransform = gameObject.transform;
		position = tempTransform.localPosition ;
		position.x = 0.50f*this.realwidth+7f;
		tempTransform.localPosition = position;

		if(!GameView.instance.isMobile){
			gameObject.transform.Find("Skill0").FindChild("Text").GetComponent<TextContainer>().width = 0.50f*this.realwidth-4.5f;
			gameObject.transform.Find("Skill1").FindChild("Text").GetComponent<TextContainer>().width = 0.50f*this.realwidth-4.5f;
			gameObject.transform.Find("Skill2").FindChild("Text").GetComponent<TextContainer>().width = 0.50f*this.realwidth-4.5f;
			gameObject.transform.Find("Skill3").FindChild("Text").GetComponent<TextContainer>().width = 0.50f*this.realwidth-4.5f;

			gameObject.transform.Find("Character").GetComponent<SpriteRenderer>().sortingOrder = 1;

			gameObject.transform.Find("Title").GetComponent<SpriteRenderer>().sortingOrder = 2;
			gameObject.transform.Find("Title").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Title").FindChild("PVText").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Title").FindChild("AttackText").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Title").FindChild("lifeicon").GetComponent<SpriteRenderer>().sortingOrder = 3;
			gameObject.transform.Find("Title").FindChild("atkicon").GetComponent<SpriteRenderer>().sortingOrder = 3;

			gameObject.transform.Find("Skill0").GetComponent<SpriteRenderer>().sortingOrder = 2;
			gameObject.transform.Find("Skill0").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Skill0").FindChild("Level").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Skill0").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 3;

			gameObject.transform.Find("Skill1").GetComponent<SpriteRenderer>().sortingOrder = 2;
			gameObject.transform.Find("Skill1").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Skill1").FindChild("Level").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Skill1").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 3;

			gameObject.transform.Find("Skill2").GetComponent<SpriteRenderer>().sortingOrder = 2;
			gameObject.transform.Find("Skill2").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Skill2").FindChild("Level").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Skill2").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 3;

			gameObject.transform.Find("Skill3").GetComponent<SpriteRenderer>().sortingOrder = 2;
			gameObject.transform.Find("Skill3").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Skill3").FindChild("Level").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Skill3").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 3;

			gameObject.transform.Find("Effect0").GetComponent<SpriteRenderer>().sortingOrder = 2;
			gameObject.transform.Find("Effect0").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Effect0").FindChild("Title").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Effect0").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 3;

			gameObject.transform.Find("Effect1").GetComponent<SpriteRenderer>().sortingOrder = 2;
			gameObject.transform.Find("Effect1").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Effect1").FindChild("Title").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Effect1").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 3;

			gameObject.transform.Find("Effect2").GetComponent<SpriteRenderer>().sortingOrder = 2;
			gameObject.transform.Find("Effect2").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Effect2").FindChild("Title").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Effect2").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 3;

			gameObject.transform.Find("Effect3").GetComponent<SpriteRenderer>().sortingOrder = 2;
			gameObject.transform.Find("Effect3").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Effect3").FindChild("Title").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Effect3").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 3;

			gameObject.transform.Find("Effect4").GetComponent<SpriteRenderer>().sortingOrder = 2;
			gameObject.transform.Find("Effect4").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Effect4").FindChild("Title").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Effect4").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 3;

			gameObject.transform.Find("Effect5").GetComponent<SpriteRenderer>().sortingOrder = 2;
			gameObject.transform.Find("Effect5").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Effect5").FindChild("Title").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Effect5").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 3;

			gameObject.transform.Find("Effect6").GetComponent<SpriteRenderer>().sortingOrder = 2;
			gameObject.transform.Find("Effect6").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Effect6").FindChild("Title").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Effect6").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 3;

			gameObject.transform.Find("Effect7").GetComponent<SpriteRenderer>().sortingOrder = 2;
			gameObject.transform.Find("Effect7").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Effect7").FindChild("Title").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Effect7").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 3;

			gameObject.transform.Find("Effect8").GetComponent<SpriteRenderer>().sortingOrder = 2;
			gameObject.transform.Find("Effect8").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Effect8").FindChild("Title").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Effect8").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 3;

			gameObject.transform.Find("Effect9").GetComponent<SpriteRenderer>().sortingOrder = 2;
			gameObject.transform.Find("Effect9").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Effect9").FindChild("Title").GetComponent<MeshRenderer>().sortingOrder = 20;
			gameObject.transform.Find("Effect9").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 3;
		}
		else{
			gameObject.transform.Find("Skill0").FindChild("Text").GetComponent<TextContainer>().width = this.realwidth-1.5f;
			gameObject.transform.Find("Skill1").FindChild("Text").GetComponent<TextContainer>().width = this.realwidth-1.5f;
			gameObject.transform.Find("Skill2").FindChild("Text").GetComponent<TextContainer>().width = this.realwidth-1.5f;
			gameObject.transform.Find("Skill3").FindChild("Text").GetComponent<TextContainer>().width = this.realwidth-1.5f;

			gameObject.transform.Find("Character").GetComponent<SpriteRenderer>().sortingOrder = 201;

			gameObject.transform.Find("Title").GetComponent<SpriteRenderer>().sortingOrder = 202;
			gameObject.transform.Find("Title").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Title").FindChild("PVText").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Title").FindChild("AttackText").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Title").FindChild("lifeicon").GetComponent<SpriteRenderer>().sortingOrder = 203;
			gameObject.transform.Find("Title").FindChild("atkicon").GetComponent<SpriteRenderer>().sortingOrder = 203;

			gameObject.transform.Find("Skill0").GetComponent<SpriteRenderer>().sortingOrder = 202;
			gameObject.transform.Find("Skill0").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Skill0").FindChild("Level").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Skill0").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 203;

			gameObject.transform.Find("Skill1").GetComponent<SpriteRenderer>().sortingOrder = 202;
			gameObject.transform.Find("Skill1").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Skill1").FindChild("Level").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Skill1").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 203;

			gameObject.transform.Find("Skill2").GetComponent<SpriteRenderer>().sortingOrder = 202;
			gameObject.transform.Find("Skill2").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Skill2").FindChild("Level").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Skill2").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 203;

			gameObject.transform.Find("Skill3").GetComponent<SpriteRenderer>().sortingOrder = 202;
			gameObject.transform.Find("Skill3").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Skill3").FindChild("Level").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Skill3").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 203;

			gameObject.transform.Find("Effect0").GetComponent<SpriteRenderer>().sortingOrder = 202;
			gameObject.transform.Find("Effect0").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Effect0").FindChild("Title").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Effect0").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 203;

			gameObject.transform.Find("Effect1").GetComponent<SpriteRenderer>().sortingOrder = 202;
			gameObject.transform.Find("Effect1").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Effect1").FindChild("Title").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Effect1").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 203;

			gameObject.transform.Find("Effect2").GetComponent<SpriteRenderer>().sortingOrder = 202;
			gameObject.transform.Find("Effect2").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Effect2").FindChild("Title").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Effect2").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 203;

			gameObject.transform.Find("Effect3").GetComponent<SpriteRenderer>().sortingOrder = 202;
			gameObject.transform.Find("Effect3").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Effect3").FindChild("Title").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Effect3").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 203;

			gameObject.transform.Find("Effect4").GetComponent<SpriteRenderer>().sortingOrder = 202;
			gameObject.transform.Find("Effect4").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Effect4").FindChild("Title").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Effect4").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 203;

			gameObject.transform.Find("Effect5").GetComponent<SpriteRenderer>().sortingOrder = 202;
			gameObject.transform.Find("Effect5").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Effect5").FindChild("Title").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Effect5").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 203;

			gameObject.transform.Find("Effect6").GetComponent<SpriteRenderer>().sortingOrder = 202;
			gameObject.transform.Find("Effect6").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Effect6").FindChild("Title").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Effect6").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 203;

			gameObject.transform.Find("Effect7").GetComponent<SpriteRenderer>().sortingOrder = 202;
			gameObject.transform.Find("Effect7").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Effect7").FindChild("Title").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Effect7").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 203;

			gameObject.transform.Find("Effect8").GetComponent<SpriteRenderer>().sortingOrder = 202;
			gameObject.transform.Find("Effect8").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Effect8").FindChild("Title").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Effect8").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 203;

			gameObject.transform.Find("Effect9").GetComponent<SpriteRenderer>().sortingOrder = 202;
			gameObject.transform.Find("Effect9").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Effect9").FindChild("Title").GetComponent<MeshRenderer>().sortingOrder = 220;
			gameObject.transform.Find("Effect9").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 203;
		}
	}
}


