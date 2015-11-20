using UnityEngine;
using System.Collections.Generic;
using TMPro ;

public class TileController : GameObjectController
{
	public Sprite[] trapSprites ;
	public Sprite[] destinationSprites ;
	
	Tile tile ;
	int type ;
	int characterID = -1;
	Trap trap ;
	bool isTrapped ;
	int isDestination = -1;
	
	void Awake()
	{
		this.showTrap(false);
		this.showDestination(false);
		this.hideDescription();
		gameObject.transform.FindChild("HoverLayer").GetComponent<SpriteRenderer>().enabled = false ;
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
	
	public void checkTrap(int target)
	{
//		if (this.trapID!=-1)
//		{
//			if (this.tile.statModifier.Type == ModifierType.Type_Wolftrap)
//			{
//				int[] targets = new int[1];
//				targets[0] = this.characterID;
//				int[] args = new int[1];
//				args[0] = this.tile.statModifier.Amount ;
//				GameController.instance.activateTrap(15, targets, args);
//				
//				int[] t = new int[2];
//				t[0] = this.tile.x;
//				t[1] = this.tile.y;
//				
//				GameController.instance.hideTrap(t);
//			}
//			else if (this.tile.statModifier.Type == ModifierType.Type_SleepingTrap)
//			{
//				print("Je check sleep "+this.trapID);
//				
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
//		}
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
		}
		gameObject.transform.FindChild("DestinationLayer").GetComponent<SpriteRenderer>().enabled = b;
	}
	
	public void OnMouseEnter()
	{
		if(this.characterID==-1){
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
		if(this.characterID!=-1){
			GameView.instance.clickCharacter(this.characterID);
		}
		else{
			if(this.isDestination!=0 && this.isDestination!=1){
				GameView.instance.clickEmpty();
			}
			else{
				GameController.instance.clickDestination(this.tile);
			}
		}
	}
	
	public void OnMouseExit()
	{
		if(this.characterID==-1){
			gameObject.transform.FindChild("HoverLayer").GetComponent<SpriteRenderer>().enabled = false ;
		}
		else if(GameView.instance.getPlayingCardController(this.characterID).getIsHidden()){
			gameObject.transform.FindChild("HoverLayer").GetComponent<SpriteRenderer>().enabled = false ;	
		}
		else{
			GameView.instance.getPlayingCardController(characterID).showHover(false);
		}
		
		if(this.type==1 || this.isTrapped){
			this.hideDescription();
		}
	}
	
	public void hideDescription(){
		gameObject.transform.FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled = false;
		gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled=false;
		gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled=false;
	}
}

