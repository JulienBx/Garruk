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

	bool target;
	float timerTarget;

	List<string> skillEffects;
	List<int> skillEffectTypes;
	bool skillEffect ;
	int anim ;

	public Sprite[] destinationSprites;
	public Sprite[] trapSprites;

	int trapId ; 
	int trapValue ;
	bool trapIsMine ;

	void Awake(){
		this.characterID = -1;
		this.destination = -1;
		this.anim = -1;
		this.trapValue = -1;
		this.trapId = -1;
		this.skillEffects = new List<string>();
		this.skillEffectTypes = new List<int>();
		this.skillEffect = false;
	}

	public bool isTarget(){
		return this.target;
	}

	public void setTarget(bool b){
		if(b){
			this.timerTarget = 0f;
			gameObject.transform.FindChild("Target").GetComponent<SpriteRenderer>().transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		}
		gameObject.transform.FindChild("Target").GetComponent<SpriteRenderer>().enabled = b;
		this.target = b ;
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
		Game.instance.getBoard().updateCristoMasters();
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
		if(i!=-1){
			if(Game.instance.getCurrentCardID()!=-1 || Game.instance.gamecards.getCardC(i).getCardM().isMine()){
				this.showCollider(i==-1);
			}
		}
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
		if(Game.instance.getDraggingCardID()!=-1 || Game.instance.getDraggingSBID()!=-1){
			this.BruteStopSE();
		}
		if(Game.instance.getDraggingSBID()!=-1){
			if(target){
				Game.instance.getCurrentSkillButtonC().blue();
				Game.instance.getCurrentSkillButtonC().getTargetText(this.characterID);
			}
			else{
				Game.instance.getCurrentSkillButtonC().red();
				Game.instance.getCurrentSkillButtonC().reinitSkillText();
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

	public void displayAnim(int type){
		this.skillEffects.Add("");
		this.skillEffectTypes.Add(10*(type+1));
		if(!this.skillEffect){
			this.launchSkillEffect();
		}
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
		this.showCollider(true);
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
		int type = this.skillEffectTypes[0];
		Game.instance.setSE(true);
		if(type<10){
			this.SETime = 1.5f;
			gameObject.transform.Find("EffectText").GetComponent<TextMeshPro>().text = this.skillEffects[0];
			if(type==0){
				gameObject.transform.Find("EffectText").GetComponent<TextMeshPro>().color = new Color(60f/255f, 160f/255f, 100f/255f, 0f);
			}
			else if(type==1){
				gameObject.transform.Find("EffectText").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 0f);
			}
			else{
				gameObject.transform.Find("EffectText").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 0f);
			}
			this.anim = -1;
			this.showSE(true);
		}
		else{
			this.SETime = 0.8f;
			this.anim = (type-10);

			gameObject.transform.Find("Anim").GetComponent<SpriteRenderer>().sprite = Game.instance.getAnimSprite(this.anim);
			this.showAnim(true);
		}
		this.skillEffects.RemoveAt(0);
		this.skillEffectTypes.RemoveAt(0);
		this.timerSE = 0f;
	}

	public void showAnim(bool b)
	{
		print(b);
		gameObject.transform.Find("Anim").GetComponent<SpriteRenderer>().enabled = b;
	}

	public void addSETime(float f){
		this.timerSE+=f;
		if(this.anim<0){
			gameObject.transform.FindChild("EffectText").localPosition = new Vector3(0f, -0.25f+0.5f*Mathf.Min(1f,this.timerSE/SETime) ,0f);
			Color c = gameObject.transform.FindChild("EffectText").GetComponent<TextMeshPro>().color;
			c.a = Mathf.Min(1f,2f*this.timerSE/SETime);
			gameObject.transform.FindChild("EffectText").GetComponent<TextMeshPro>().color = c;
		}
		else{
			int tempInt = Mathf.Min(9,Mathf.FloorToInt(10f*this.timerSE/SETime));
			if(tempInt!=this.anim%10){
				this.anim++ ;
				gameObject.transform.Find("Anim").GetComponent<SpriteRenderer>().sprite = Game.instance.getAnimSprite(this.anim);
			}
		}
		if(this.timerSE>SETime){
			if(this.anim>=0){
				this.showAnim(false);
			}
			else{
				this.showSE(false);
			}
			if(this.skillEffects.Count>0){
				this.setSkillEffectText();
			}
			else{
				this.skillEffect = false ;
			}
		}
	}

	public void BruteStopSE(){
		this.showSE(false);
		this.skillEffect = false ;
		this.skillEffects = new List<string>();
	}

	public void addTargetTime(float f){
		this.timerTarget+=f;
		gameObject.transform.FindChild("Target").GetComponent<SpriteRenderer>().transform.localRotation = Quaternion.Euler(0f, 0f, 360f*(this.timerTarget%1f));
		if((this.timerTarget%2f)<1){
			gameObject.transform.FindChild("Target").GetComponent<SpriteRenderer>().transform.localScale = new Vector3(0.8f+0.2f*(this.timerTarget%1f), 0.8f+0.2f*(this.timerTarget%1f), 0.8f+0.2f*(this.timerTarget%1f));
		}
		else{
			gameObject.transform.FindChild("Target").GetComponent<SpriteRenderer>().transform.localScale = new Vector3(1f-0.2f*(this.timerTarget%1f), 1f-0.2f*(this.timerTarget%1f), 1f-0.2f*(this.timerTarget%1f));
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

	public int getBoardValue(){
		int toBeReturned=-9;
		if(this.characterID!=-1){
			if(Game.instance.getCards().getCardC(this.characterID).canBeTargeted()){
				toBeReturned=this.characterID;
			}
			else{
				toBeReturned=-1;
			}
		}
		else if (this.rock){
			toBeReturned=-2;
		}
		else{
			toBeReturned=-1;
		}
		return toBeReturned;
	}

	public void setTrap(int id, int value, bool isFirstP){
		this.trapId = id;
		this.trapValue = value;
		trapIsMine = Game.instance.isFirstPlayer()==isFirstP;
		if(trapIsMine){
			this.showTrap();
		}
	}

	public void showTrap(){
		gameObject.transform.FindChild("Trap").GetComponent<SpriteRenderer>().sprite = this.trapSprites[this.trapId];
		gameObject.transform.FindChild("Trap").GetComponent<SpriteRenderer>().enabled = true;
	}

	public void removeTrap(){
		this.trapId=-1;
		gameObject.transform.FindChild("Trap").GetComponent<SpriteRenderer>().enabled = false;
	}

	public bool isTrapped(){
		return (this.trapId!=-1);
	}

	public bool isMyTrap(){
		bool test = false ;
		if(this.trapId!=-1){
			if(!this.trapIsMine){
				test = true;
			}
		}
		return (test);
	}

	public int getTrapId(){
		return this.trapId;
	}

	public int getTrapValue(){
		return this.trapValue;
	}

	public bool getTrapIsMine(){
		return this.trapIsMine;
	}
}


