using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TileHandlerController : GameObjectController
{
	public Sprite[] sprites ;
	
	Tile tile ;
	int type ;
	int characterID = -1 ;
	bool isHovered = false ; 
	
	void Awake()
	{
		
	}
	
	public void initTileHandlerController(Tile t){
		this.tile = t ;
		gameObject.GetComponent<SpriteRenderer>().sprite = this.sprites[0];
	}
	
	public void resize(Vector3 p, Vector3 s){
		gameObject.transform.position = p ;
		gameObject.transform.localScale = s ;
	}
	
	public void setCharacterID(int i){
		this.characterID = i ;
	}
	
	public void moveForward(){
		gameObject.transform.GetComponent<SpriteRenderer>().sortingOrder = 20;
	}
	
	public void moveBack(){
		gameObject.transform.GetComponent<SpriteRenderer>().sortingOrder = 5;
	}
	
	public int getTypeNumber(){
		return this.type;
	}
	
	public void changeType(int i){
		this.setText("");
		this.type = i ;
		gameObject.GetComponent<SpriteRenderer>().sprite = this.sprites[i];
	}
	
	public bool isOccupied(){
		return (characterID!=-1);
	}
	
	void OnMouseEnter(){
		if(type==6){
			GameView.instance.hoverTileHandler(characterID, this.tile);
		}
		else{
			GameView.instance.hoverTile(characterID, this.tile, true);
		}
		gameObject.GetComponent<SpriteRenderer>().enabled = true ;
		this.isHovered = true ;
	}
	
	void OnMouseExit(){
		if(type==2){
			this.isHovered = false ;
			type=6;
			this.GetComponentInChildren<TextMeshPro>().text = "";
		}
	}
	
	void OnMouseDown()
	{
		if (this.type==1){
			GameController.instance.moveToDestination(this.tile);
		}
		else if (this.type==2){
			if(this.characterID==-1){
				GameController.instance.hitTarget(this.tile);
			}
			else{
				GameController.instance.hitTarget(this.characterID);
			}
		}
		if(GameView.instance.getIsTutorialLaunched())
		{
			TutorialObjectController.instance.actionIsDone();
		}
	}
	
	public void setText(string s){
		gameObject.GetComponentInChildren<TextMeshPro>().text = s ;
	}
	
	public bool getIsHovered(){
		return this.isHovered;
	}
	
	public void disable(){
		this.GetComponent<SpriteRenderer>().enabled = false ;
		Transform tempGO = gameObject.transform.FindChild("TileHandlerText");
		tempGO.GetComponent<TextMeshPro>().enabled = false ;
	}
	
	public void enable(){
		this.GetComponent<SpriteRenderer>().enabled = true ;
		Transform tempGO = gameObject.transform.FindChild("TileHandlerText");
		tempGO.GetComponent<TextMeshPro>().enabled = true ;
	}
	
}

