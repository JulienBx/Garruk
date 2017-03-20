using System;
using UnityEngine;
using TMPro;

public class GameCardController : MonoBehaviour
{
	public Sprite[] backgroundSprites;
	public Sprite[] characterSprites;
	public Sprite[] foregroundSprites;

	GameCardModel gameCard;
	bool mine;
	IconModel[] icons;
	bool movable;

	TileModel tileModel;

	void Awake(){
		this.icons = new IconModel[3];
		for(int i = 0 ; i < 3 ; i++){
			this.icons[i]=new IconModel();
		}
		this.movable = false;
	}

	public void setGameCard(GameCardModel g){
		this.gameCard = g;
		this.show(true);
	}

	public void setTileModel(TileModel t){
		this.tileModel = t;
	}

	public TileModel getTileModel(){
		return this.tileModel ;
	}

	public bool isMine(){
		return this.mine;
	}

	public void setMovable(bool b){
		this.movable = b ;
	}

	public bool isMovable(){
		return this.movable ;
	}

	public void show(bool b){
		if(this.icons[0].isDisplayed()){
			gameObject.transform.FindChild("Icon1").GetComponent<SpriteRenderer>().enabled = b;
		}
		else{
			gameObject.transform.FindChild("Icon1").GetComponent<SpriteRenderer>().enabled = false;
		}
		if(this.icons[1].isDisplayed()){
			gameObject.transform.FindChild("Icon2").GetComponent<SpriteRenderer>().enabled = b;
		}
		else{
			gameObject.transform.FindChild("Icon2").GetComponent<SpriteRenderer>().enabled = false;
		}
		if(this.icons[2].isDisplayed()){
			gameObject.transform.FindChild("Icon3").GetComponent<SpriteRenderer>().enabled = b;
		}
		else{
			gameObject.transform.FindChild("Icon3").GetComponent<SpriteRenderer>().enabled = false;
		}
		gameObject.transform.FindChild("Foreground").GetComponent<SpriteRenderer>().enabled = b;
		gameObject.transform.FindChild("Background").GetComponent<SpriteRenderer>().enabled = b;
		gameObject.transform.FindChild("Circle").GetComponent<SpriteRenderer>().enabled = b;
		gameObject.transform.FindChild("Character").GetComponent<SpriteRenderer>().enabled = b;
		gameObject.transform.FindChild("AttackValue").GetComponent<MeshRenderer>().enabled = b;
		gameObject.transform.FindChild("PVValue").GetComponent<MeshRenderer>().enabled = b;
	}

	public void moveBackward(){
		gameObject.transform.FindChild("Icon1").GetComponent<SpriteRenderer>().sortingOrder = 23;
		gameObject.transform.FindChild("Icon2").GetComponent<SpriteRenderer>().sortingOrder = 23;
		gameObject.transform.FindChild("Icon3").GetComponent<SpriteRenderer>().sortingOrder = 23;
		gameObject.transform.FindChild("Circle").GetComponent<SpriteRenderer>().sortingOrder = 22;
		gameObject.transform.FindChild("Foreground").GetComponent<SpriteRenderer>().sortingOrder = 30;
		gameObject.transform.FindChild("Background").GetComponent<SpriteRenderer>().sortingOrder = 20;
		gameObject.transform.FindChild("Character").GetComponent<SpriteRenderer>().sortingOrder = 21;
		gameObject.transform.FindChild("AttackValue").GetComponent<MeshRenderer>().sortingOrder = 23;
		gameObject.transform.FindChild("PVValue").GetComponent<MeshRenderer>().sortingOrder = 23;
	}

	public void moveForward(){
		gameObject.transform.FindChild("Icon1").GetComponent<SpriteRenderer>().sortingOrder = 43;
		gameObject.transform.FindChild("Icon2").GetComponent<SpriteRenderer>().sortingOrder = 43;
		gameObject.transform.FindChild("Icon3").GetComponent<SpriteRenderer>().sortingOrder = 43;
		gameObject.transform.FindChild("Circle").GetComponent<SpriteRenderer>().sortingOrder = 42;
		gameObject.transform.FindChild("Foreground").GetComponent<SpriteRenderer>().sortingOrder = 50;
		gameObject.transform.FindChild("Background").GetComponent<SpriteRenderer>().sortingOrder = 40;
		gameObject.transform.FindChild("Character").GetComponent<SpriteRenderer>().sortingOrder = 41;
		gameObject.transform.FindChild("AttackValue").GetComponent<MeshRenderer>().sortingOrder = 43;
		gameObject.transform.FindChild("PVValue").GetComponent<MeshRenderer>().sortingOrder = 43;
	}

	public void initCard(){
		this.updateIcons();
		this.updateForeground();
		this.updateBackground();
		this.updateCharacter();
		this.updateAttackValue(this.gameCard.getAttack());
		this.updateLifeValue(this.gameCard.getLife());
	}

	public void updateIcons(){

	}

	public void updateForeground(){
		gameObject.transform.FindChild("Foreground").GetComponent<SpriteRenderer>().sprite = this.foregroundSprites[0];
	}

	public void updateBackground(){
		if(this.mine){
			gameObject.transform.FindChild("Circle").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[0];
		}
		else{
			gameObject.transform.FindChild("Circle").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[1];
		}
	}

	public void updateCharacter(){
		gameObject.transform.FindChild("Character").GetComponent<SpriteRenderer>().sprite = this.characterSprites[this.gameCard.getCharacterType()];
	}

	public void updateAttackValue(int i){
		gameObject.transform.FindChild("AttackValue").GetComponent<TextMeshPro>().text = ""+i;
	}

	public void updateLifeValue(int i){
		gameObject.transform.FindChild("PVValue").GetComponent<TextMeshPro>().text = ""+i;
	}

	public void setIsMine(bool b){
		this.mine = b;
	}

	public void setPosition(Vector3 p){
		gameObject.transform.localPosition = p;
	}

	public void setScale(Vector3 p){
		gameObject.transform.localScale = p;
	}
}