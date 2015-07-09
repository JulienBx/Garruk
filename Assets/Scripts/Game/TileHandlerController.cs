using UnityEngine;
using System.Collections.Generic;

public class TileHandlerController : GameObjectController
{
	public Sprite[] sprites ;
	
	Tile tile ;
	int type ;
	int characterID = -1 ;
	
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
	
	public void changeType(int i){
		this.type = i ;
		gameObject.GetComponent<SpriteRenderer>().sprite = this.sprites[i];
	}
	
	public bool isOccupied(){
		return (characterID!=-1);
	}
	
	void OnMouseEnter(){
		GameView.instance.hoverTile(this.tile);
	}
	
	void OnMouseDown()
	{
		if (this.type==1){
			GameController.instance.moveToDestination(this.tile);
		}
	}
}

