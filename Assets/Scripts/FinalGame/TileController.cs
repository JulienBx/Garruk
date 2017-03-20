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
				NewGameController.instance.getGameCardsController().getCardController(this.characterID).moveForward();
				NewGameController.instance.setDraggingCardID(this.characterID);
			}
		}
	}

	public void OnMouseUp()
	{
		if(this.characterID!=-1){

		}
	}

	public void setTile(TileModel t){
		this.tile = t;
	}

	public void show(bool b){
		gameObject.transform.FindChild("Background").GetComponent<SpriteRenderer>().enabled = b;
		gameObject.transform.FindChild("Foreground").GetComponent<SpriteRenderer>().enabled = b;
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
	}

	public bool isMyEmptyDestination(){
		return (this.destination==0 && this.characterID==-1);
	}
}