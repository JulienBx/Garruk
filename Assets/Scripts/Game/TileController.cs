﻿using UnityEngine;
using System.Collections.Generic;

public class TileController : GameObjectController
{
	public Sprite[] sprites ;
	
	Tile tile ;
	int type ;
	int characterID = -1;
	
	void Awake()
	{
		
	}
	
	public void initTileController(Tile t, int ty){
		this.tile = t ;
		this.type = ty ;
		gameObject.GetComponent<SpriteRenderer>().sprite = this.sprites[type];
	}
	
	public void resize(Vector3 p, Vector3 s){
		gameObject.transform.position = p ;
		gameObject.transform.localScale = s ;
	}
	
	public Vector3 getPosition()
	{
		return gameObject.transform.position;
	}
	
	public bool canBeDestination(){
		return (type!=1 && characterID==-1);
	}
	
	public void OnMouseEnter(){
		GameView.instance.hoverTile(this.tile);
	}
	
	public void setCharacterID(int i){
		this.characterID = i ;
	}
	
	public void setTargetHalo(HaloTarget h, bool isHaloDisabled=false)
	{
//		this.tileView.tileVM.haloStyle.normal.background = this.halos[h.idImage];
//		this.tileView.tileVM.haloTexts = new List<string>();
//		this.tileView.tileVM.haloStyles = new List<GUIStyle>();
//		
//		for (int i = 0 ; i < h.textsToDisplay.Count ; i++){
//			this.tileView.tileVM.haloTexts.Add(h.textsToDisplay[i]);
//			this.tileView.tileVM.haloStyles.Add(this.haloTextStyles[h.stylesID[i]]);
//		}
//		this.tileView.tileVM.toDisplayHalo = true ;
//		this.tileView.tileVM.isHaloDisabled = isHaloDisabled;
	}
	
	public void checkTrap(int target)
	{
//		if (this.tile.isStatModifier)
//		{
//			if (this.tile.statModifier.Type == ModifierType.Type_Wolftrap)
//			{
//				int[] targets = new int[1];
//				targets[0] = this.characterID;
//				int[] args = new int[1];
//				args[0] = this.tile.statModifier.Amount ;
//				GameController.instance.activateTrap(15, targets, args);
//				int[] t = new int[2];
//				t[0] = this.tile.x;
//				t[1] = this.tile.y;
//				
//				GameController.instance.hideTrap(t);
//			}
//			else if (this.tile.statModifier.Type == ModifierType.Type_SleepingTrap)
//			{
//				int[] targets = new int[1];
//				targets[0] = this.characterID;
//				int[] args = new int[1];
//				args[0] = this.tile.statModifier.Amount ;
//				GameController.instance.activateTrap(61, targets, args);
//				
//				int[] t = new int[2];
//				t[0] = this.tile.x;
//				t[1] = this.tile.y;
//				
//				GameController.instance.hideTrap(t);
//			}
//			else if (this.tile.statModifier.Type == ModifierType.Type_WeakeningTrap)
//			{
//				int[] targets = new int[1];
//				targets[0] = this.characterID;
//				int[] args = new int[1];
//				args[0] = this.tile.statModifier.Amount ;
//				GameController.instance.activateTrap(60, targets, args);
//				
//				int[] t = new int[2];
//				t[0] = this.tile.x;
//				t[1] = this.tile.y;
//				
//				GameController.instance.hideTrap(t);
//			}
//			else{
//				//GameController.instance.displaySkillEffect(GameController.instance.currentPlayingCard, "se déplace", 2, 2);
//			}
//		}
	}
	
	public void hideIcon(){
//		this.tile.statModifier = null ;
//		this.tile.isStatModifier = false ;
//		this.show();
	}
	
	public void removeTrap()
	{
//		this.isTrap = false;
//		this.tileView.tileVM.toDisplayTrap = false;
	}

	public int getID()
	{
		//return (tile.x * 10 + tile.y);
		return 0 ;
	}

	public void setTile(int x, int y, int boardWidth, int boardHeight, int type, float scaleTile)
	{
//		this.tile = new Tile(x, y);
//		this.type = type;
//
//		this.tileView.tileVM.background = backTile [type];
//		this.tileView.changeBackground();
//		this.tileView.tileVM.border = borderTile [0];
//		this.tileView.changeBorder();
//
//		this.resize(1f, 3, 4);
	}

	public void resize(float scaleTile, float offsetX, float offsetY)
	{
//		int height = Screen.height;
//		int width = Screen.width;
//		
//		Vector3 position;
//		this.tileView.tileVM.scale = new Vector3(scaleTile, scaleTile, scaleTile);
//		if (GameController.instance.isFirstPlayer)
//		{
//			position = new Vector3(scaleTile * (-offsetX + 0.5f + this.tile.x), scaleTile * (-offsetY + 0.5f + this.tile.y), -1);
//		} else
//		{
//			position = new Vector3(scaleTile * (offsetX - 0.5f - this.tile.x), scaleTile * (offsetY - 0.5f - this.tile.y), -1);
//		}
		//this.tileView.tileVM.position = position;
		
//		int decalage = height / 15;
//		
//		Vector3 positionObject = new Vector3(0, 0, 0);
//		positionObject.x = (this.tileView.tileVM.position.x) * (height / 10f) - (decalage / 2) + (width / 2f);
//		positionObject.y = height - ((this.tileView.tileVM.position.y + this.tileView.tileVM.scale.y / 2f) * (height / 10f) - (decalage / 2) + (height / 2f));
//		
//		Rect haloRect = new Rect(positionObject.x, positionObject.y, decalage, decalage);
//		this.tileView.tileVM.haloRect = haloRect;
//		
//		this.tileView.resize();
//		this.resizeIcons();
//		this.resizeHalo();
	}
	
	public void resizeHalo()
	{
//		int height = Screen.height;
//		int width = Screen.width;
//		
//		int decalage = height / 15;
//		
//		Vector3 positionObject = new Vector3(0, 0, 0);
//		positionObject.x = (this.tileView.tileVM.position.x - this.tileView.tileVM.scale.x / 2.2f) * (height / 10f) + (width / 2f);
//		positionObject.y = height - ((this.tileView.tileVM.position.y + this.tileView.tileVM.scale.y / 2.2f) * (height / 10f) + (height / 2f));
//		
//		Rect position = new Rect(positionObject.x, positionObject.y, this.tileView.tileVM.scale.x * height / 11, this.tileView.tileVM.scale.x * height / 11);
//		this.tileView.tileVM.haloRect = position;
//		
//		for (int i = 0 ; i < this.haloTextStyles.Length ; i++){
//			this.haloTextStyles[i].fontSize = height * 15 / 1000;
//		}
	}
	
	public void addTemple(int amount)
	{
//		this.tileView.tileVM.toDisplayIcon = true;
//		this.tileModification = TileModification.Temple_Sacre;
//		this.tileView.tileVM.icon = this.icons [0];
//		statModifierActive = true;
//		statModifierEachTurn = false;
		//this.tile.StatModifier.Clear();

		//this.tile.StatModifier.Add(new StatModifier(amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, -1, 0, "", "", ""));
	}

	public void addForetIcon(int amount)
	{
//		this.tileView.tileVM.toDisplayIcon = true;
//		this.tileModification = TileModification.Foret_de_Lianes;
//		this.tileView.tileVM.icon = this.icons [1];
//		statModifierActive = true;
//		statModifierEachTurn = false;
		//this.tile.StatModifier.Clear();

		//this.tile.StatModifier.Add(new StatModifier(-amount, ModifierType.Type_Multiplier, ModifierStat.Stat_Move, -1, 0, "", "", ""));
	}

	public void addSable(bool isVisible)
	{
//		if (isVisible)
//		{
//			this.tileView.tileVM.toDisplayIcon = true;
//		}
//		this.tileModification = TileModification.Sables_Mouvants;
//		this.tileView.tileVM.icon = this.icons [2];
//		statModifierActive = true;
//		statModifierEachTurn = false;
		//this.tile.StatModifier.Clear();

		//this.tile.StatModifier.Add(new StatModifier(-999, ModifierType.Type_BonusMalus, ModifierStat.Stat_Move, 4, 0, "", "", ""));
	}

	public bool getIconVisibility()
	{
//		return this.tileView.tileVM.toDisplayIcon;
	return false ;
	}

	public void addFontaine(int power)
	{
//		this.tileView.tileVM.toDisplayIcon = true;
//		this.tileModification = TileModification.Fontaine_de_Jouvence;
//		this.tileView.tileVM.icon = this.icons [3];
//		statModifierActive = true;
//		statModifierNewTurn = true;
//		statModifierEachTurn = true;
		//this.tile.StatModifier.Clear();

		//this.tile.StatModifier.Add(new StatModifier(-power, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, 0, "", "", ""));
	}

	public void removeIcon()
	{
//		this.tileView.tileVM.toDisplayIcon = false;
	}
	public void resizeIcons()
	{
//		int height = Screen.height;
//		int width = Screen.width;
//		
//		Vector3 positionObject = new Vector3(0, 0, 0);
//		positionObject.x = (this.tileView.tileVM.position.x - this.tileView.tileVM.scale.x / 2.2f) * (height / 10f) + (width / 2f);
//		positionObject.y = height - ((this.tileView.tileVM.position.y + this.tileView.tileVM.scale.y / 2.2f) * (height / 10f) + (height / 2f));
//		
//		Rect position = new Rect(positionObject.x, positionObject.y, this.tileView.tileVM.scale.x * height / 11, this.tileView.tileVM.scale.x * height / 11);
//		this.tileView.tileVM.iconRect = position;
	}
	
	public void setDestination(bool b)
	{
//		if (type!=1)
//		{
//			this.isDestination = b;
//			this.setGrey(b);
//			this.setGreyBorder (b);
//		}
	}
	public void setGrey(bool b)
	{
//		int facteur = 0;
//		int borderIndex = 0;
//		if (b)
//		{
//			facteur = 5;
//			borderIndex = 4;
//		}
//		this.tileView.tileVM.background = this.backTile [facteur + this.type];
//		this.tileView.changeBackground();
//		this.tileView.tileVM.border = this.borderTile [borderIndex];
//		this.tileView.changeBorder();
	}
	public void setGreyBorder(bool value)
	{
//		this.isGreyBorder = value;
	}
	public void setStandard()
	{
//		this.isDestination = false;
//		this.tileView.tileVM.background = this.backTile [this.type];
//		this.tileView.changeBackground();
//		this.tileView.tileVM.border = this.borderTile [0];
//		this.tileView.changeBorder();
	}
	
	public void setLookingForTileZone(bool b)
	{
//		this.tileView.tileVM.isZoneEffect = b;
	}
	
	public void hoverTarget()
	{
//		if (this.tileView.tileVM.isZoneEffect)
//		{
//			GameController.instance.changeZoneTargetTile(this.tile);
//		}
	}

	

	public void setBorderTile(int index)
	{
//		this.tileView.tileVM.border = this.borderTile [index];	
//		this.tileView.changeBorder();
	}

	public void hoverTile()
	{
//		GameController.instance.hoverTileHandler(this.tile);
	}

	public void releaseClickTile()
	{
//		GameController.instance.releaseClickTileHandler(this.tile);
	}

	public void displayHover()
	{
//		this.tileView.tileVM.border = this.borderTile [1];
//		this.tileView.changeBorder();
	}

	public void hideHover()
	{
//		if (this.isGreyBorder)
//		{
//			this.tileView.tileVM.border = this.borderTile [4];
//			this.tileView.changeBorder();
//		} else
//		{
//			this.tileView.tileVM.border = this.borderTile [0];
//			this.tileView.changeBorder();
//		}
	}

	public void displaySelected()
	{
//		this.tileView.tileVM.border = this.borderTile [2];
//		this.tileView.changeBorder();
	}
	
	public void hideSelected()
	{
//		if (this.isDestination)
//		{
//			this.tileView.tileVM.border = this.borderTile [4];
//			this.tileView.changeBorder();
//		} else
//		{
//			this.tileView.tileVM.border = this.borderTile [0];
//			this.tileView.changeBorder();
//		}
	}

	public void displayPlaying()
	{
//		this.tileView.tileVM.border = this.borderTile [3];
//		this.tileView.changeBorder();
	}
	
	public void hidePlaying()
	{
//		if (this.isDestination)
//		{
//			this.tileView.tileVM.border = this.borderTile [4];
//			this.tileView.changeBorder();
//		} else
//		{
//			this.tileView.tileVM.border = this.borderTile [0];
//			this.tileView.changeBorder();
//		}
	}

//	public void drag(){
//		if (this.isMovable){
//			GameController.instance.setCharacterDragged(this.characterID);
//		}
//	}
//	
//	public void release(){
//		if (this.isMovable){
//			GameController.instance.dropCharacter();
//		}
//	}

	public void activatePotentielTarget(HaloSkill halo)
	{
//		this.tileView.tileVM.isPotentialTarget = true;
//		this.tileView.tileVM.toDisplayHalo = true;
//		this.tileView.tileVM.halo = this.halos [(int)halo];
	}

	public void removePotentielTarget()
	{
		//this.tileView.tileVM.isPotentialTarget = false;
	}
	
	public void removeHalo()
	{
		//this.tileView.tileVM.toDisplayHalo = false;
	}
	
	public void removeZoneEffect()
	{
		//this.tileView.tileVM.isZoneEffect = false;
	}
	
	public void activateWolfTrapTarget()
	{
		//this.tileView.tileVM.toDisplayHalo = true;
		//this.tileView.tileVM.halo = this.halos [0];
	}
	
	public void activateEffectZoneHalo()
	{
		//this.tileView.tileVM.toDisplayHalo = true;
		//this.tileView.tileVM.halo = this.halos [1];
	}
	
	public void show()
	{
//		if (this.tile.isStatModifier){
//			if (this.tile.statModifier.Active){
//				this.tileView.tileVM.toDisplayIcon = true ;
//				this.tileView.tileVM.icon = this.icons[this.tile.statModifier.idIcon];
//				this.tileView.tileVM.title = this.tile.statModifier.title;
//				this.tileView.tileVM.description = this.tile.statModifier.description;
//				this.tileView.tileVM.additionnalInfo = this.tile.statModifier.additionnalInfo;
//			}
//			else{
//				this.tileView.tileVM.icon = this.icons[this.tile.statModifier.idIcon];
//				this.tileView.tileVM.title = this.tile.statModifier.title;
//				this.tileView.tileVM.description = this.tile.statModifier.description;
//				this.tileView.tileVM.additionnalInfo = this.tile.statModifier.additionnalInfo;
//				this.tileView.tileVM.toDisplayIcon = false ;
//			}
//		}
//		else{
//			this.tileView.tileVM.toDisplayIcon = false ;
//		}
	}
	
	public void addTileTarget(){
		//this.tileView.tileVM.toDisplayHalo = false;
		//GameController.instance.targetTileHandler.addTargetTile(this.tile);
	}
	
	public void hideTargetHalo(){
		//this.tileView.tileVM.toDisplayHalo = false;
		//this.tileView.tileVM.isHaloDisabled = false;
	}
}

