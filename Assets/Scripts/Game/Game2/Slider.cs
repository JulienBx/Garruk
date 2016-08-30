using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Slider : MonoBehaviour
{
	int nextCard;
	int currentCard;
	public bool moving;
	public bool growing;

	public float timer;
	public float moveTime ;

	float timerClignote;
	float clignoteTime ;
	bool clignoteGrowing;
	bool clignoting;
	bool toStopClignoting ;

	public float endPositionX;
	public float startPositionX;

	public void init(){
		this.nextCard = -1;
		this.currentCard = -1;
		this.moveTime = 0.3f;
		this.clignoteTime = 0.4f;
		this.moving = false;
		this.growing = false;
		this.clignoteGrowing = false;
		this.clignoting = false;
		this.toStopClignoting = false;
	}

	public virtual void activateCollider(bool b){
		gameObject.transform.GetComponent<BoxCollider>().enabled = b;
	}

	public virtual void setNextCard(int i){
		if(i!=this.currentCard){
			this.nextCard = i;
			if(!this.moving && !this.growing){
				this.launchNextMove();
			}
			else if(this.growing){
				this.revertGrow();
			}
		}
		else{
			if(this.moving && !this.growing){
				this.revertGrow();
			}
		}
	}

	public void launchNextMove(){
		if(this.nextCard!=-1){
			this.setCard(this.nextCard);
			if(Game.instance.getInterlude().isDisplaying()){
				if(this.currentCard==Game.instance.getCurrentCardID()){
					this.moveCharacterForward();
				}
			}
			this.growing = true ;
			this.moving = true ;
			this.timer=0f;
		}
		else{
			this.currentCard=-1;
		}
	}

	public void revertGrow(){
		this.timer = Mathf.Max(0f,this.moveTime - this.timer) ;
		this.growing = !this.growing ;
		this.moving = true ;
	}
	
	public virtual void setCard(int id){
		CardC c = Game.instance.getCards().getCardC(id);

		int tempInt ;
		gameObject.transform.FindChild("Character").GetComponent<SpriteRenderer>().sprite = Game.instance.getBigCharacterSprite(c.getCardM().getCharacterType());
		gameObject.transform.FindChild("Title").FindChild("Text").GetComponent<TextMeshPro>().text = WordingCardName.getName(c.getCardM().getCharacterType());
		gameObject.transform.FindChild("Title").FindChild("PVText").GetComponent<TextMeshPro>().text = c.getDoubleCharacterText(c.getLife())+"/"+c.getDoubleCharacterText(c.getTotalLife());
		gameObject.transform.FindChild("Title").FindChild("AttackText").GetComponent<TextMeshPro>().text = c.getDoubleCharacterText(c.getAttack());

		int nbSkills = c.getCardM().getNbSkill() ;
		int compteur = 0 ; 
		for(int i = 1 ; i < nbSkills+1;i++){
			if(i==nbSkills){
				i = 0 ; 
			}
			if(c.getCardM().getSkill(i).IsActivated==1){
				gameObject.transform.FindChild("Skill"+compteur).FindChild("Text").GetComponent<TextMeshPro>().text = c.getSkillText(i);
				gameObject.transform.FindChild("Skill"+compteur).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = Game.instance.getSkillSprite(c.getCardM().getSkill(i).Id);
				tempInt = c.getCardM().getSkill(i).Power;
				gameObject.transform.FindChild("Skill"+compteur).FindChild("Level").GetComponent<TextMeshPro>().text = ""+tempInt;
				if(tempInt<6){
					gameObject.transform.FindChild("Skill"+compteur).FindChild("Picto").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
				}
				else if(tempInt<9){
					gameObject.transform.FindChild("Skill"+compteur).FindChild("Picto").GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
				}
				else{
					gameObject.transform.FindChild("Skill"+compteur).FindChild("Picto").GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
				}
				if(i==0){
					gameObject.transform.FindChild("Skill"+compteur).GetComponent<SpriteRenderer>().color=new Color(100f/255f, 100f/255f, 100f/255f, 1f);
				}
				else{
					gameObject.transform.FindChild("Skill"+compteur).GetComponent<SpriteRenderer>().color=new Color(1f, 1f, 1f, 1f);
				}
				gameObject.transform.FindChild("Skill"+compteur).GetComponent<SpriteRenderer>().enabled=true;
				gameObject.transform.FindChild("Skill"+compteur).FindChild("Text").GetComponent<MeshRenderer>().enabled=true;
				gameObject.transform.FindChild("Skill"+compteur).FindChild("Level").GetComponent<MeshRenderer>().enabled=true;
				gameObject.transform.FindChild("Skill"+compteur).FindChild("Picto").GetComponent<SpriteRenderer>().enabled=true;
				compteur++;
			}
			if(i==0){
				i = nbSkills ; 
			}
		}

		for (int j = compteur ; j < 4 ; j++){
			gameObject.transform.FindChild("Skill"+j).GetComponent<SpriteRenderer>().enabled=false;
			gameObject.transform.FindChild("Skill"+j).FindChild("Text").GetComponent<MeshRenderer>().enabled=false;
			gameObject.transform.FindChild("Skill"+j).FindChild("Level").GetComponent<MeshRenderer>().enabled=false;
			gameObject.transform.FindChild("Skill"+j).FindChild("Picto").GetComponent<SpriteRenderer>().enabled=false;
		}

		List<ModifyerM> effects = c.getEffects();
		compteur = 0 ;

		for(int i = 0 ; i < effects.Count ; i++){
			gameObject.transform.FindChild("Effect"+compteur).FindChild("Text").GetComponent<TextMeshPro>().text = effects[i].getDescription();
			if(effects[i].getAmount()>0){
				gameObject.transform.FindChild("Effect"+compteur).FindChild("Picto").GetComponent<SpriteRenderer>().color = new Color(60f/255f, 160f/255f, 100f/255f, 1f);
				gameObject.transform.FindChild("Effect"+compteur).FindChild("Text").GetComponent<TextMeshPro>().color = new Color(60f/255f, 160f/255f, 100f/255f, 1f);
				gameObject.transform.FindChild("Effect"+compteur).FindChild("Title").GetComponent<TextMeshPro>().color = new Color(60f/255f, 160f/255f, 100f/255f, 1f);
			}
			else if(effects[i].getAmount()<0){
				gameObject.transform.FindChild("Effect"+compteur).FindChild("Picto").GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
				gameObject.transform.FindChild("Effect"+compteur).FindChild("Text").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
				gameObject.transform.FindChild("Effect"+compteur).FindChild("Title").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
			}
			else{
				gameObject.transform.FindChild("Effect"+compteur).FindChild("Picto").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
				gameObject.transform.FindChild("Effect"+compteur).FindChild("Text").GetComponent<TextMeshPro>().color = new Color(1f, 1f, 1f, 1f);
				gameObject.transform.FindChild("Effect"+compteur).FindChild("Title").GetComponent<TextMeshPro>().color = new Color(1f, 1f, 1f, 1f);
			}
			
			gameObject.transform.FindChild("Effect"+compteur).FindChild("Title").GetComponent<TextMeshPro>().text = effects[i].getTitle();
			gameObject.transform.FindChild("Effect"+compteur).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = Game.instance.getCards().getCardC(0).getIconSprite(effects[i].getIdIcon());

			gameObject.transform.FindChild("Effect"+compteur).GetComponent<SpriteRenderer>().enabled = true ;
			gameObject.transform.FindChild("Effect"+compteur).FindChild("Text").GetComponent<MeshRenderer>().enabled = true;
			gameObject.transform.FindChild("Effect"+compteur).FindChild("Title").GetComponent<MeshRenderer>().enabled = true;
			gameObject.transform.FindChild("Effect"+compteur).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true;
			compteur++;
		}

		for(int j = compteur ; j < 10 ; j++){
			gameObject.transform.FindChild("Effect"+j).GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.FindChild("Effect"+j).FindChild("Text").GetComponent<MeshRenderer>().enabled = false;
			gameObject.transform.FindChild("Effect"+j).FindChild("Title").GetComponent<MeshRenderer>().enabled = false;
			gameObject.transform.FindChild("Effect"+j).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = false;
		}
		this.currentCard = id;
		if(id==Game.instance.getCurrentCardID()){
			this.resetTimerClignote();
		}
		else{
			if(this.clignoting){
				this.stopClignote();
			}
		}
	}

	public bool isClignoting(){
		return this.clignoting;
	}
	
	public void resetTimerClignote(){
		this.timerClignote = 0f;
		this.clignoteGrowing = true;
		this.clignoting = true;
		this.toStopClignoting = false;
	}

	public void stopClignote(){
		this.clignoting = false;
		this.toStopClignoting = false;
		gameObject.transform.FindChild("Character").localScale = new Vector3(1f, 1f, 1f) ;
	}
	
	public void addTimeClignoting(float f){
		this.timerClignote += f;
		
		if(this.timerClignote>this.clignoteTime){
			if(!this.clignoteGrowing && toStopClignoting){
				this.stopClignote();
			}
			else{
				this.clignoteGrowing = !this.clignoteGrowing ;
			}
			this.timerClignote = 0f ;
		}
		else{
			if (this.clignoteGrowing){
				float scale = 1f + 0.05f * Mathf.Min(1,this.timerClignote/this.clignoteTime);
				gameObject.transform.FindChild("Character").localScale = new Vector3(scale, scale, scale) ;
			}
			else {
				float scale = 1.05f - 0.05f * Mathf.Min(1,this.timerClignote/this.clignoteTime);
				gameObject.transform.FindChild("Character").localScale = new Vector3(scale, scale, scale) ;
			}
		}
	}

	public void addTime(float f){
		this.timer += f;
		Vector3 position = gameObject.transform.localPosition;
		if(this.growing){
			position.x = this.startPositionX+(this.endPositionX-this.startPositionX)*(Mathf.Min(1,Mathf.Sqrt(this.timer/this.moveTime)));
		}
		else{
			position.x = this.endPositionX+(this.startPositionX-this.endPositionX)*(Mathf.Min(1,this.timer/this.moveTime));
		}
		gameObject.transform.localPosition = position ;

		if (this.timer>this.moveTime){
			this.moving = false;
			if(!this.growing){
				this.launchNextMove();
			}
		}
	}

	public bool isMoving(){
		return this.moving;
	}

	public void resize(float f){
		if(!Game.instance.isMobile()){
			gameObject.transform.Find("Skill0").FindChild("Text").GetComponent<TextContainer>().width = 0.50f*f-4.5f;
			gameObject.transform.Find("Skill1").FindChild("Text").GetComponent<TextContainer>().width = 0.50f*f-4.5f;
			gameObject.transform.Find("Skill2").FindChild("Text").GetComponent<TextContainer>().width = 0.50f*f-4.5f;
			gameObject.transform.Find("Skill3").FindChild("Text").GetComponent<TextContainer>().width = 0.50f*f-4.5f;

			gameObject.transform.Find("Background").GetComponent<SpriteRenderer>().enabled = false;
			this.moveCharacterBackward();

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
			gameObject.transform.Find("Skill0").FindChild("Text").GetComponent<TextContainer>().width = f-1.5f;
			gameObject.transform.Find("Skill1").FindChild("Text").GetComponent<TextContainer>().width = f-1.5f;
			gameObject.transform.Find("Skill2").FindChild("Text").GetComponent<TextContainer>().width = f-1.5f;
			gameObject.transform.Find("Skill3").FindChild("Text").GetComponent<TextContainer>().width= f-1.5f;
			gameObject.transform.Find("Background").GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.Find("Background").GetComponent<SpriteRenderer>().sortingOrder = 200;

			this.moveCharacterForward();

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

	public void OnMouseDown()
	{
		/*
		if(GameView.instance.sequenceID!=8 && GameView.instance.sequenceID!=9){
			this.empty();
		}
		if(GameView.instance.hasFightStarted){
			GameView.instance.removeDestinations();
			if(!GameView.instance.getCurrentCard().hasMoved){
				GameView.instance.displayDestinations(GameView.instance.getCurrentPlayingCard());
			}
		}
		*/
	}

	public void moveCharacterForward()
	{
		gameObject.transform.Find("Character").GetComponent<SpriteRenderer>().sortingOrder = 201;

		gameObject.transform.Find("Title").GetComponent<SpriteRenderer>().sortingOrder = 202;
		gameObject.transform.Find("Title").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 220;
		gameObject.transform.Find("Title").FindChild("PVText").GetComponent<MeshRenderer>().sortingOrder = 220;
		gameObject.transform.Find("Title").FindChild("AttackText").GetComponent<MeshRenderer>().sortingOrder = 220;
		gameObject.transform.Find("Title").FindChild("lifeicon").GetComponent<SpriteRenderer>().sortingOrder = 203;
		gameObject.transform.Find("Title").FindChild("atkicon").GetComponent<SpriteRenderer>().sortingOrder = 203;
	}

	public void moveCharacterBackward()
	{
		gameObject.transform.Find("Character").GetComponent<SpriteRenderer>().sortingOrder = 1;

		gameObject.transform.Find("Title").GetComponent<SpriteRenderer>().sortingOrder = 2;
		gameObject.transform.Find("Title").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 20;
		gameObject.transform.Find("Title").FindChild("PVText").GetComponent<MeshRenderer>().sortingOrder = 20;
		gameObject.transform.Find("Title").FindChild("AttackText").GetComponent<MeshRenderer>().sortingOrder = 20;
		gameObject.transform.Find("Title").FindChild("lifeicon").GetComponent<SpriteRenderer>().sortingOrder = 3;
		gameObject.transform.Find("Title").FindChild("atkicon").GetComponent<SpriteRenderer>().sortingOrder = 3;
	}
}


