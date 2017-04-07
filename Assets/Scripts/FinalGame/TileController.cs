using System;
using UnityEngine;

public class TileController : MonoBehaviour
{
	public Sprite[] backgroundSprites;
	public Sprite[] foregroundSprites;

	TileModel tile ;
	int characterID ;
	int destination ;
	bool rock ;

	void Awake ()
	{
		this.show(false);
		this.characterID = -1;
		this.destination = -1;
		this.rock = false;
	}

	public void OnMouseEnter()
	{
		
	}

	public void OnMouseExit()
	{
		
	}

	public void OnMouseDown()
	{
		if(this.characterID!=-1){
			if(NewGameController.instance.getGameCardsController().getCardController(characterID).isMovable()){
				NewGameController.instance.getGameCardsController().getCardController(this.characterID).setMoving(false);
				NewGameController.instance.getGameCardsController().getCardController(this.characterID).moveForward();
				NewGameController.instance.setDraggingCardID(this.characterID);
			}
		}
	}

	public void OnMouseUp()
	{
		if(this.characterID!=-1){
			if(NewGameController.instance.getDraggingCardID()==this.characterID){
				NewGameController.instance.setDraggingCardID(-1);
				TileModel targetTile = NewGameController.instance.getTilesController().getMouseTile();
				if(targetTile.getX()!=-1){
					if(NewGameController.instance.getTilesController().getTileController(targetTile).isMyEmptyDestination()){
						NewGameController.instance.getGameCardsController().getCardController(this.characterID).startMove(NewGameController.instance.getTilesController().getTileController(targetTile).getLocalPosition());
						NewGameController.instance.setCharacterToTile(this.characterID, targetTile);
					}
					else{
						NewGameController.instance.getGameCardsController().getCardController(this.characterID).startMove(this.getLocalPosition());
					}
				}
				else{
					NewGameController.instance.getGameCardsController().getCardController(this.characterID).startMove(this.getLocalPosition());
				}
			}
		}
	}

	public void setTile(TileModel t){
		this.tile = t;
	}

	public void show(bool b){
		gameObject.transform.FindChild("Background").GetComponent<SpriteRenderer>().enabled = b;
		gameObject.transform.FindChild("Foreground").GetComponent<SpriteRenderer>().enabled = b;
	}

	public void showCollider(bool b){
		gameObject.transform.GetComponent<BoxCollider>().enabled = b;
	}

	public Vector3 getLocalPosition(){
		return gameObject.transform.localPosition;
	}

	public Vector3 getLocalScale(){
		return gameObject.transform.localScale;
	}

	public void setPosition(Vector3 p){
		gameObject.transform.localPosition = p;
	}

	public void setScale(Vector3 s){
		gameObject.transform.localScale = s;
	}

	public void setRock(){
		gameObject.transform.FindChild("Background").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[1];
		this.rock = true;
	}

	public bool isRock(){
		return this.rock;
	}

	public void setCharacterID(int id){
		this.characterID = id;
	}

	public int getCharacterID(){
		return this.characterID;
	}

	public void setDestination(int i){
		this.destination = i;
		this.updateDestinations();
	}

	public void updateDestinations(){
		if(this.destination==-1){
			gameObject.transform.FindChild("Foreground").GetComponent<SpriteRenderer>().sprite = this.foregroundSprites[0];
		}
		else if(this.destination==0){
			gameObject.transform.FindChild("Foreground").GetComponent<SpriteRenderer>().sprite = this.foregroundSprites[1];
		}
		else if(this.destination==1){
			gameObject.transform.FindChild("Foreground").GetComponent<SpriteRenderer>().sprite = this.foregroundSprites[2];
		}
		else if(this.destination==2){
			gameObject.transform.FindChild("Foreground").GetComponent<SpriteRenderer>().sprite = this.foregroundSprites[3];
		}
	}

	public bool isMyEmptyDestination(){
		return (this.destination==0 && this.characterID==-1);
	}

	public bool canPassOver(bool b){
		bool canPass = true ;
		if(this.isRock()){
			canPass = false ;
		}
		else{
			if(this.characterID==-1){

			}
			else if(NewGameController.instance.getGameCardsController().getCardController(this.characterID).isMine()!=b){
				canPass = false;
			}
		}
		return canPass;
	}

	public bool isEmpty(){
		return (this.characterID==-1 && !this.rock);
	}

	public void displayDestinations(){
		int type = 0 ;
		if(this.characterID==NewGameController.instance.getGameCardsController().getCurrentCardID() && !NewGameController.instance.getGameCardsController().getCurrentCard().isMoved()){
			if(NewGameController.instance.getGameCardsController().getCurrentCard().isMine()){
				type = 0;
			}
			else{
				type = 1 ;
			}
		}
		else{
			type = 2 ;
		}
		for (int i = 0 ; i < 6 ; i++){
			for (int j = 0 ; j < 8 ; j++){
				if(NewGameController.instance.getGameCardsController().getCardController(this.characterID).canMoveOn(i,j)){
					NewGameController.instance.getTilesController().getTileController(i,j).setDestination(type);
				}
				else{
					NewGameController.instance.getTilesController().getTileController(i,j).setDestination(-1);
				}
			}
		}
	}
}