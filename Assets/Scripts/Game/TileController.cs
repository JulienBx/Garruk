﻿using UnityEngine;
using System.Collections.Generic;
using TMPro ;

public class TileController : GameObjectController
{
	public Sprite[] trapSprites ;
	public Sprite[] destinationSprites ;
	public Sprite[] targetSprites ;
	public Color[] skillEffectColor;
	public Sprite[] animSprites;
	
	Tile tile ;
	int type ;
	int characterID = -1;
	Trap trap ;
	bool isTrapped ;
	int isDestination = -1;
	
	public bool isDisplayingTarget;
	float timerTarget;
	float targetTime = 0.5f;
	bool isTargetDisplayed;
	
	string skillEffectDescription ;
	bool isDisplayingSkillEffect;
	bool isShowingSE ;
	float timerSE;
	float timerAnim = 0 ;
	float animTime = 0.08f ;
	float skillEffectTime = 0.5f ;
	int animIndex;
	int basicAnimIndex ;

	bool isHovering = false ;

	void Awake()
	{
		this.showTrap(false);
		this.showDestination(false);
		this.showDescription(false);
		gameObject.transform.FindChild("HoverLayer").GetComponent<SpriteRenderer>().enabled = false ;
		this.showTarget(false);
		this.showEffect(false);
		this.displayAnim(false);
	}
	
	public void displayTarget(bool b){
		this.isDisplayingTarget = b ;
		this.isTargetDisplayed = b ;
		gameObject.transform.FindChild("TargetLayer").GetComponent<SpriteRenderer>().enabled = this.targetSprites[0] ;
		gameObject.transform.FindChild("TargetLayer").GetComponent<SpriteRenderer>().enabled = b ;
		this.timerTarget=0f;
	}
	
	public void addTargetTime(float f){
		this.timerTarget += f ;
		if (this.timerTarget>this.targetTime){
			this.isTargetDisplayed = !this.isTargetDisplayed ;
			if(!this.isHovering){
				if(this.isTargetDisplayed){
					gameObject.transform.FindChild("TargetLayer").GetComponent<SpriteRenderer>().sprite = this.targetSprites[0] ;
				}
				else{
					gameObject.transform.FindChild("TargetLayer").GetComponent<SpriteRenderer>().sprite = this.targetSprites[1] ;
				}
			}
			this.timerTarget = 0f ;
		}
	}
	
	public void setTargetText(string t, string d){
		gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<TextMeshPro>().text = t;
		gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = d;
	}
	
	public void showTarget(bool b){
		gameObject.transform.FindChild("TargetLayer").GetComponent<SpriteRenderer>().enabled = b ;
		this.isDisplayingTarget = b ;
	}
	
	public void initTileController(Tile t, int ty){
		this.tile = t ;
		this.type = ty ;
		if (this.type==1){
			this.showRock();
		}
		gameObject.name = "Tile " + (t.x) + "-" + (t.y);
	}
	
	public void resize(Vector3 p, Vector3 s){
		gameObject.transform.position = p ;
		gameObject.transform.localScale = s ;
	}
	
	public void setTrap(Trap t){
		this.trap = t ;
		isTrapped = true ;
			
		this.showTrap (this.trap.getIsVisible());
	}
	
	public int getIsDestination(){
		return this.isDestination;
	}
	
	public void setDestination(int i){
		this.isDestination = i ;
		this.showDestination (true);
	}
	
	public void removeDestination(){
		this.isDestination = -1 ;
		this.showDestination (false);
	}
	
	public bool canBeDestination(){
		return (type!=1 && characterID==-1);
	}
	
	public bool isRock(){
		return (this.type==1);
	}
	
	public bool getIsTrapped(){
		return (this.isTrapped);
	}
	
	public Vector3 getPosition()
	{
		return gameObject.transform.position;
	}
	
	public int getTileType()
	{
		return this.type;
	}
	
	public void changeType(int a)
	{
		this.type = a;
		if (this.type==1){
			this.showRock();
		}
	}
	
	public Tile getTile()
	{
		return this.tile;
	}
	
	public void setCharacterID(int i){
		this.characterID = i ;
		gameObject.transform.FindChild("HoverLayer").GetComponent<SpriteRenderer>().enabled = false ;
	}
	
	public int getCharacterID(){
		return this.characterID ;
	}
	
	public void checkTrap()
	{
		if(this.isTrapped){
			if(this.trap.getType()==1){
				GameView.instance.getPlayingCardController(this.characterID).addDamagesModifyer(new Modifyer(this.trap.getAmount(), -1, 0, "Electropiège", this.trap.getAmount()+" dégats subis"));
			}
			else if(this.trap.getType()==2){	
				if(this.trap.getAmount()==2){
					GameView.instance.getCard(this.characterID).setState(new Modifyer(0, 1, 4, "Parapiège", "Paralysé. Ne peut pas agir pendant 1 tour"));
					GameView.instance.getPlayingCardController(this.characterID).showIcons();
					GameView.instance.displaySkillEffect(this.characterID, "Paralysé !", 1);
				}
				else{
					GameView.instance.displaySkillEffect(this.characterID, "Parapiège : n'a pas fonctionné", 1);
				}
				
			}
			this.isTrapped=false;
			this.showTrap(false);
			GameView.instance.getCard(this.characterID).canCancelMove = false;
			this.showDescription(false);
		}
	}
	
	public void removeTrap()
	{
		this.isTrapped = false;
		this.showTrap(false);
	}
	
	public void showTrap(bool b)
	{
		if (this.isTrapped){
			gameObject.transform.FindChild("TrapLayer").GetComponent<SpriteRenderer>().sprite = this.trapSprites[this.trap.getType()];
		}
		gameObject.transform.FindChild("TrapLayer").GetComponent<SpriteRenderer>().enabled = b;
	}
	
	public void showRock()
	{
		gameObject.transform.FindChild("TrapLayer").GetComponent<SpriteRenderer>().sprite = this.trapSprites[0];
		gameObject.transform.FindChild("TrapLayer").GetComponent<SpriteRenderer>().enabled = true;
	}
	
	public void showDestination(bool b)
	{
		if (this.isDestination>=0){
			gameObject.transform.FindChild("DestinationLayer").GetComponent<SpriteRenderer>().sprite = this.destinationSprites[isDestination];
			gameObject.transform.FindChild("DestinationLayer").GetComponent<SpriteRenderer>().enabled = true;
		}
		else{
			gameObject.transform.FindChild("DestinationLayer").GetComponent<SpriteRenderer>().enabled = false;
		}
	}
	
	public void OnMouseEnter()
	{
		this.isHovering = true ;
		if(this.isDisplayingTarget){
			gameObject.transform.FindChild("TargetLayer").GetComponent<SpriteRenderer>().sprite = this.targetSprites[2] ;
			this.showDescription(true);
		}
		if(this.characterID==-1){
			gameObject.transform.FindChild("HoverLayer").GetComponent<SpriteRenderer>().enabled = true ;
			if(this.isDisplayingTarget){
				gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<TextMeshPro>().text = "Poser un piège";
				gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = GameSkills.instance.getCurrentGameSkill().getTargetText(-1);
				gameObject.transform.FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled = true;
				gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled=true;
				gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled=true;
			}
			else if(this.type==1){
				gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<TextMeshPro>().text = "Cristal";
				gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = "Les unités ne peuvent pas se déplacer sur cette case";
				gameObject.transform.FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled = true;
				gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled=true;
				gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled=true;
			}
			else if (this.isTrapped){
				if(this.trap.getIsVisible()){
					gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<TextMeshPro>().text = this.trap.title;
					gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = this.trap.description;
					gameObject.transform.FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled = true;
					gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled=true;
					gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled=true;
				}
			}
			GameView.instance.hoverTile();
		}
		else{
			if(!GameView.instance.getPlayingCardController(this.characterID).getIsHidden()){
				GameView.instance.hoverCharacter(this.characterID);
			}
			else{
				gameObject.transform.FindChild("HoverLayer").GetComponent<SpriteRenderer>().enabled = true ;
				if(this.type==1){
					gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<TextMeshPro>().text = "Case infranchissable";
					gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = "Les unités ne peuvent pas s'arreter sur cette case";
					gameObject.transform.FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled = true;
					gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled=true;
					gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled=true;
				}
				else if (this.isTrapped){
					if(this.trap.getIsVisible()){
						gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<TextMeshPro>().text = this.trap.title;
						gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = this.trap.description;
						gameObject.transform.FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled = true;
						gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled=true;
						gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled=true;
					}
				}
				GameView.instance.hoverTile();
			}
		}
	}
	
	public void OnMouseDown()
	{
		if(this.isDisplayingTarget){
			this.showDescription(false);
		}
		if(this.type!=1){
			if(this.characterID!=-1){
				if(GameView.instance.getTileController(this.characterID).isDisplayingTarget){
					print("Je hit");
					GameView.instance.hitTarget(this.characterID);
				}
				GameView.instance.clickCharacter(this.characterID);
			}
			else{
				if(this.isDisplayingTarget){
					GameView.instance.hitTarget(this.tile);
				}
				else if(this.isDestination!=0 && this.isDestination!=1){
					GameView.instance.clickEmpty();
				}
				else{
					GameController.instance.clickDestination(this.tile, GameView.instance.getCurrentPlayingCard(), false);
				}
			}
			if(ApplicationModel.player.ToLaunchGameTutorial){
				GameView.instance.hideTuto();
			}
		}
	}
	
	public void OnMouseExit()
	{
		this.isHovering = false ;
		if(this.isDisplayingTarget){
			this.showDescription(false);
			if(this.isTargetDisplayed){
				gameObject.transform.FindChild("TargetLayer").GetComponent<SpriteRenderer>().sprite = this.targetSprites[0] ;
			}
			else{
				gameObject.transform.FindChild("TargetLayer").GetComponent<SpriteRenderer>().sprite = this.targetSprites[1] ;
			}
		}
		if(this.characterID==-1){
			gameObject.transform.FindChild("HoverLayer").GetComponent<SpriteRenderer>().enabled = false ;
			if(this.isDisplayingTarget){
				gameObject.transform.FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled = false;
				gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled=false;
				gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled=false;
			}
		}
		else if(GameView.instance.getPlayingCardController(this.characterID).getIsHidden()){
			gameObject.transform.FindChild("HoverLayer").GetComponent<SpriteRenderer>().enabled = false ;	
		}
		else{
			GameView.instance.getPlayingCardController(characterID).showHover(false);
		}
		
		if(this.type==1 || this.isTrapped){
			this.showDescription(false);
		}
	}
	
	public void showDescription(bool b){
		gameObject.transform.FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled = b;
		gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled=b;
		gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled=b;
	}
	
	public void setSkillEffect(string s, int type){
		gameObject.transform.FindChild("SkillEffect").FindChild("Text").GetComponent<TextMeshPro>().text= s;
		gameObject.transform.FindChild("SkillEffect").FindChild("Text").GetComponent<TextMeshPro>().color = this.skillEffectColor[type] ;
	}
	
	public void addAnimTime(float t){
		this.timerAnim += t ;
		int newIndex = Mathf.FloorToInt(timerAnim/animTime);
		if(newIndex!=animIndex){
			animIndex = newIndex;
			if(newIndex>9){
				this.timerAnim = 0f ;
				GameView.instance.removeAnim(this.tile);
			}
			else{
				this.changeAnimSprite(basicAnimIndex + animIndex);
			}
		}
	}

	public void addSETime(float t){
		this.timerAnim += t ;
		if(timerAnim<3*skillEffectTime){
			gameObject.transform.FindChild("SkillEffect").localScale = new Vector3(0.5f+0.5f*(1.0f*timerAnim/(3*skillEffectTime)), 0.5f+0.5f*(1.0f*timerAnim/(3*skillEffectTime)), 0.5f+0.5f*(1.0f*timerAnim/(3*skillEffectTime)));

			if(timerAnim>2*skillEffectTime){
				Color c = gameObject.transform.FindChild("SkillEffect").FindChild("Text").GetComponent<TextMeshPro>().color ;
				c.a = 1f-1f*((timerAnim-2*skillEffectTime)/(skillEffectTime));
				gameObject.transform.FindChild("SkillEffect").FindChild("Text").GetComponent<TextMeshPro>().color = c;
			}
			gameObject.transform.FindChild("SkillEffect").localPosition = new Vector3(0, 0.5f*(1.0f*timerAnim/skillEffectTime), 0f);
		}
		else{
			this.timerAnim = 0f ;
			GameView.instance.removeSE(this.tile);
		}
	}
	
	public void changeAnimSprite(int index){
		gameObject.transform.FindChild("AnimLayer").GetComponent<SpriteRenderer>().sprite = this.animSprites[index] ;
	}
	
	public void displayAnim(bool b){
		gameObject.transform.FindChild("AnimLayer").GetComponent<SpriteRenderer>().enabled = b ;
		this.timerAnim = 0f;
		this.animIndex = 0;
	}
	
	public void showEffect(bool b){
		gameObject.transform.FindChild("SkillEffect").FindChild("Text").GetComponent<MeshRenderer>().enabled = b ;
	}


	public void setAnimIndex(int i){
		basicAnimIndex = i*10 ;
	}
}

