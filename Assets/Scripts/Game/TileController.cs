using UnityEngine;
using System.Collections.Generic;

public class TileController : GameObjectController
{
	public Sprite[] sprites ;
	
	Tile tile ;
	int type ;
	int characterID = -1;
	int trapID =-1; 
	
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
	
	public int getTileType()
	{
		return this.type;
	}
	
	public Tile getTile()
	{
		return this.tile;
	}
	
	public bool canBeDestination(){
		return (type!=1 && characterID==-1);
	}
	
	public void setCharacterID(int i){
		this.characterID = i ;
	}
	
	public int getCharacterID(){
		return this.characterID ;
	}
	
	public void checkTrap(int target)
	{
		if (this.trapID!=-1)
		{
			if (this.tile.statModifier.Type == ModifierType.Type_Wolftrap)
			{
				int[] targets = new int[1];
				targets[0] = this.characterID;
				int[] args = new int[1];
				args[0] = this.tile.statModifier.Amount ;
				GameController.instance.activateTrap(15, targets, args);
				
				int[] t = new int[2];
				t[0] = this.tile.x;
				t[1] = this.tile.y;
				
				GameController.instance.hideTrap(t);
			}
			else if (this.tile.statModifier.Type == ModifierType.Type_SleepingTrap)
			{
				print("Je check sleep "+this.trapID);
				
				int[] targets = new int[1];
				targets[0] = this.characterID;
				int[] args = new int[1];
				args[0] = this.tile.statModifier.Amount ;
				GameController.instance.activateTrap(61, targets, args);
				
				int[] t = new int[2];
				t[0] = this.tile.x;
				t[1] = this.tile.y;
				
				GameController.instance.hideTrap(t);
			}
			else if (this.tile.statModifier.Type == ModifierType.Type_WeakeningTrap)
			{
				int[] targets = new int[1];
				targets[0] = this.characterID;
				int[] args = new int[1];
				args[0] = this.tile.statModifier.Amount ;
				GameController.instance.activateTrap(60, targets, args);
				
				int[] t = new int[2];
				t[0] = this.tile.x;
				t[1] = this.tile.y;
				
				GameController.instance.hideTrap(t);
			}
		}
	}
	
	public void removeTrap()
	{
		this.trapID = -1;
		this.show ();
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
	
	public void changeTrapId(int i)
	{
		this.trapID = i ;
	}
	
	public int getTrapId()
	{
		return this.trapID;
	}
	
	public void show()
	{
		if (this.trapID!=-1 && this.tile.statModifier.Active){
			gameObject.GetComponent<SpriteRenderer>().sprite = this.sprites[trapID];
		}
		else{
			gameObject.GetComponent<SpriteRenderer>().sprite = this.sprites[type];
		}
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

