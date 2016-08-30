using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TileC : MonoBehaviour
{
	TileM tile ;
	bool rock ;
	int characterID ;
	int destination;

	float timerSE;
	float SETime = 1f;
	List<string> skillEffects;
	List<int> skillEffectTypes;
	bool skillEffect ;

	public Sprite[] destinationSprites;

	void Awake(){
		this.characterID = -1;
		this.destination = -1;
		this.skillEffects = new List<string>();
		this.skillEffectTypes = new List<int>();
		this.skillEffect = false;
	}

	public void size(float tileScale){
		Vector3 position ;
		if (Game.instance.isFirstPlayer()){
			position = new Vector3((-2.5f+this.tile.x)*(tileScale), (-3.5f+this.tile.y)*(tileScale), 0);
		}
		else{
			position = new Vector3((2.5f-this.tile.x)*(tileScale), (3.5f-this.tile.y)*(tileScale), 0);
		}
		
		Vector3 scale = new Vector3(tileScale, tileScale, tileScale);

		gameObject.transform.position = position ;
		gameObject.transform.localScale = scale ;
	}

	public int getDestination(){
		return this.destination;
	}

	public Vector3 getPosition()
	{
		return gameObject.transform.position;
	}

	public bool isRock(){
		return this.rock;
	}

	public void setRock(bool b){
		this.rock = b;
		this.showRock(b);
	}

	public void setTile(int x, int y){
		this.tile = new TileM(x,y);
	}

	public TileM getTile(){
		return this.tile ;
	}

	public int getCharacterID(){
		return this.characterID;
	}

	public void setCharacterID(int i){
		this.characterID = i;
		this.showCollider(i==-1);
	}

	public bool hasCharacter(){
		return (this.characterID!=-1);
	}

	public bool isEmpty(){
		return (this.characterID==-1 && !this.rock);
	}

	public void OnMouseEnter()
	{
		this.showHover(true);
			
		if(Game.instance.getDraggingCardID()==-1){
			if(!Game.instance.isMobile()){
				Game.instance.hoverTile();
			}
		}
	}

	public void OnMouseExit()
	{
		this.showHover(false);
	}

	public void showHover(bool b)
	{
		gameObject.transform.FindChild("Hover").GetComponent<SpriteRenderer>().enabled = b;
	}

	public void displayDestination(int i)
	{
		this.destination = i;
		gameObject.transform.FindChild("Destination").GetComponent<SpriteRenderer>().sprite = this.destinationSprites[this.destination];
		this.showDestination(true);
	}

	public void showDestination(bool b)
	{
		if(!b){
			this.destination = -1;
		}
		gameObject.transform.FindChild("Destination").GetComponent<SpriteRenderer>().enabled = b;
	}

	public void showRock(bool b)
	{
		gameObject.transform.FindChild("Rock").GetComponent<SpriteRenderer>().enabled = b;
	}

	public void showSE(bool b)
	{
		gameObject.transform.FindChild("EffectText").GetComponent<MeshRenderer>().enabled = b;
	}

	public void showCollider(bool b)
	{
		gameObject.GetComponent<BoxCollider>().enabled = b;
	}

	public void free(){
		this.displayDestination(0);
	}

	public void displaySkillEffect(string s, int type){
		this.skillEffects.Add(s);
		this.skillEffectTypes.Add(type);
		if(!this.skillEffect){
			this.launchSkillEffect();
		}
	}

	public void launchSkillEffect(){
		this.setSkillEffectText();
		this.skillEffect = true ;
	}

	public bool isSkillEffect(){
		return this.skillEffect;
	}

	public void setSkillEffectText(){
		gameObject.transform.FindChild("EffectText").GetComponent<TextMeshPro>().text = this.skillEffects[0];
		int type = this.skillEffectTypes[0];
		if(type==0){
			gameObject.transform.FindChild("EffectText").GetComponent<TextMeshPro>().color = new Color(60f/255f, 160f/255f, 100f/255f, 0f);
		}
		else if(type==1){
			gameObject.transform.FindChild("EffectText").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 0f);
		}
		else{
			gameObject.transform.FindChild("EffectText").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 0f);
		}
		this.skillEffects.RemoveAt(0);
		this.skillEffectTypes.RemoveAt(0);
		this.timerSE = 0f;
		this.showSE(true);
	}

	public void addSETime(float f){
		this.timerSE+=f;
		gameObject.transform.FindChild("EffectText").localPosition = new Vector3(0f, -0.25f+0.5f*Mathf.Min(1f,this.timerSE/SETime) ,0f);
		Color c = gameObject.transform.FindChild("EffectText").GetComponent<TextMeshPro>().color;
		c.a = Mathf.Min(1f,2f*this.timerSE/SETime);
		gameObject.transform.FindChild("EffectText").GetComponent<TextMeshPro>().color = c;

		if(this.timerSE>SETime){
			this.showSE(false);
			if(this.skillEffects.Count>0){
				this.setSkillEffectText();
			}
			else{
				this.skillEffect = false ;
			}
		}
	}

	public bool canPassOver(bool b){
		bool canPass = true ;
		if(this.isRock()){
			canPass = false ;
		}
		else{
			if(this.characterID==-1){

			}
			else if(Game.instance.getCards().getCardC(this.characterID).getCardM().isMine()!=b){
				canPass = false;
			}
		}
		return canPass;
	}
}


