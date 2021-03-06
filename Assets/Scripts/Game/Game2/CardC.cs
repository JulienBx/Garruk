﻿using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CardC : MonoBehaviour
{
	public Sprite[] circleSprites;
	public Sprite[] backTileSprites;
	public Sprite[] iconSprites;
	public Sprite[] characterSprites;

	TileM tile;
	CardM card;
	int id;

	int displayedLife;
	int displayedAttack;

	int originalLife;
	int originalAttack;

	bool upgradingLife;
	bool upgradingAttack;

	float upgradeTime = 1f;
	float upgradeAttackTimer;
	float upgradeLifeTimer;

	TextMeshPro attackText ;
	TextMeshPro lifeText ;
	bool deadLayer;
	bool backTileLayer;

	List<ModifyerM> damageModifyers ;
	List<ModifyerM> stateModifyers ;
	List<ModifyerM> moveModifyers ;
	List<ModifyerM> esquiveModifyers ;
	List<ModifyerM> bouclierModifyers ;

	List<ModifyerM> lifeModifyers ;
	List<ModifyerM> attackModifyers ;

	float timerMove;
	float moveTime = 0.25f;
	bool moving;

	Vector3 startPosition, endPosition;

	float timerPush;
	float pushTime = 0.5f;
	bool measuringPush;

	float timerClignote;
	float clignoteTime = 0.5f;
	bool clignoting;
	bool clignoteGrowing;
	bool stopClignoting;

	bool moved ;
	bool played ;
	bool dead;
	bool[,] destinations ;

	float timerSE;
	float SETime = 1.5f;
	List<string> skillEffects;
	List<int> skillEffectTypes;
	bool skillEffect ;
	int anim ;

	void Awake(){
		this.upgradingLife = false ;
		this.upgradingAttack = false ;
		this.deadLayer = false ;
		this.backTileLayer = false ;
		this.moved = false ;
		this.dead = false ;
		this.played = false ;
		this.clignoting = false ;
		this.clignoteGrowing = false ;
		this.stopClignoting = false ;
		this.skillEffects = new List<string>();
		this.skillEffectTypes = new List<int>();
		this.skillEffect = false;
		this.anim = -1;
		this.displayedLife = 0 ;

		this.stateModifyers = new List<ModifyerM>();
		this.moveModifyers = new List<ModifyerM>();
		this.esquiveModifyers = new List<ModifyerM>();
		this.lifeModifyers = new List<ModifyerM>();
		this.attackModifyers = new List<ModifyerM>();
		this.bouclierModifyers = new List<ModifyerM>();
		this.damageModifyers = new List<ModifyerM>();

		attackText = gameObject.transform.FindChild("Background").FindChild("AttackValue").GetComponent<TextMeshPro>();
		lifeText = gameObject.transform.FindChild("Background").FindChild("PVValue").GetComponent<TextMeshPro>();

		this.initDestinations();
	}

	public bool[,] getDestinations(){
		return this.destinations;
	}

	public bool isClignoting(){
		return this.clignoting;
	}

	public void stopClignote(){
		this.stopClignoting=true;
	}

	public void addDamageModifyer(ModifyerM m){
		bool hasFound = false ;
		for(int i = 0 ; i < damageModifyers.Count ;i++){
			if(m.getDuration()==damageModifyers[i].getDuration()){
				hasFound = true ;
				damageModifyers[i].addAmount(m.getAmount());
				if(damageModifyers[i].getAmount()<=0){
					damageModifyers.RemoveAt(i);
				}
			}
		}
		if(!hasFound){
			this.damageModifyers.Add(m);
		}
		if(m.getAmount()>0 && this.getCardM().getCharacterType()==112){
			List<int> allys = Game.instance.getBoard().getAllys(this, this.id);
			for (int j = 0 ; j < allys.Count ;j++){
				if(Game.instance.isIA() || Game.instance.isTutorial()){
					Game.instance.getSkills().skills[112].effects(allys[j], Game.instance.getCurrentCard().getCardM().getSkill(0).Power, UnityEngine.Random.Range(1,101));
				}
				else{
					Game.instance.launchCorou("EffectsSkillRPC", 112, allys[j], Game.instance.getCurrentCard().getCardM().getSkill(0).Power, UnityEngine.Random.Range(1,101));
				}
			}
		}
		this.setLife();
	}

	public void addStateModifyer(ModifyerM m){
		for (int i = this.stateModifyers.Count-1 ; i >=0 ; i--){
			if(this.stateModifyers[i].getIdIcon()==17){
				this.stateModifyers.RemoveAt(i);
			}
		}
		this.stateModifyers.Add(m);
		this.showIcons(true);
		Game.instance.updateModifyers(this.id);
	}

	public void addAttackModifyer(ModifyerM m){
		bool hasFound = false ;
		string durationText = "" ;
		if(m.getDuration()==1){
			durationText = WordingGame.getText(100);
		}
		else{
			durationText = WordingGame.getText(2);
		}
		for(int i = 0 ; i < attackModifyers.Count ;i++){
			if(m.getDuration()==attackModifyers[i].getDuration() && m.getIdIcon()==0 && attackModifyers[i].getIdIcon()==0){
				hasFound = true ;
				attackModifyers[i].addAmount(m.getAmount());
				if(attackModifyers[i].getAmount()==0){
					attackModifyers.RemoveAt(i);
				}
				else{
					if(attackModifyers[i].getAmount()>0){
						attackModifyers[i].setTitle(WordingGame.getText(81));
						attackModifyers[i].setDescription(WordingGame.getText(13, new List<int>{attackModifyers[i].getAmount()})+durationText);
					}
					else{
						attackModifyers[i].setTitle(WordingGame.getText(82));
						attackModifyers[i].setDescription(WordingGame.getText(83, new List<int>{attackModifyers[i].getAmount()})+durationText);
					}
				}
			}
		}
		if(!hasFound){
			if(m.getAmount()>0){
				m.setTitle(WordingGame.getText(81));
				m.setDescription(WordingGame.getText(13, new List<int>{m.getAmount()})+WordingGame.getText(2));
			}
			else{
				m.setTitle(WordingGame.getText(82));
				m.setDescription(WordingGame.getText(83, new List<int>{m.getAmount()})+WordingGame.getText(2));
			}
			this.attackModifyers.Add(m);
		}
		Game.instance.updateModifyers(this.id);
		this.setAttack();
	}

	public void addLifeModifyer(ModifyerM m){
		bool hasFound = false ;
		string durationText = "" ;
		if(m.getDuration()==1){
			durationText = WordingGame.getText(100);
		}
		else{
			durationText = WordingGame.getText(2);
		}
		for(int i = 0 ; i < lifeModifyers.Count ;i++){
			if(m.getDuration()==lifeModifyers[i].getDuration()){
				hasFound = true ;
				lifeModifyers[i].addAmount(m.getAmount());
				if(lifeModifyers[i].getAmount()==0){
					lifeModifyers.RemoveAt(i);
				}
				else{
					lifeModifyers[i].setTitle(WordingGame.getText(98));
					lifeModifyers[i].setDescription(WordingGame.getText(11, new List<int>{lifeModifyers[i].getAmount()})+durationText);
				}
			}
		}
		if(!hasFound){
			m.setTitle(WordingGame.getText(98));
			m.setDescription(WordingGame.getText(11, new List<int>{m.getAmount()})+durationText);
			this.lifeModifyers.Add(m);
		}
		Game.instance.updateModifyers(this.id);
		this.setLife();
	}

	public void addMoveModifyer(ModifyerM m){
		bool hasFound = false ;
		string durationText = "" ;
		if(m.getDuration()==1){
			durationText = WordingGame.getText(100);
		}
		else{
			durationText = WordingGame.getText(2);
		}
		for(int i = 0 ; i < moveModifyers.Count ;i++){
			if(m.getDuration()==moveModifyers[i].getDuration()){
				hasFound = true ;
				moveModifyers[i].addAmount(m.getAmount());
				if(moveModifyers[i].getAmount()==0){
					moveModifyers.RemoveAt(i);
				}
				else{
					if(attackModifyers[i].getAmount()>0){
						moveModifyers[i].setTitle(WordingGame.getText(109));
						moveModifyers[i].setDescription(WordingGame.getText(108, new List<int>{moveModifyers[i].getAmount()})+durationText);
					}
					else{
						moveModifyers[i].setTitle(WordingGame.getText(111));
						moveModifyers[i].setDescription(WordingGame.getText(110, new List<int>{moveModifyers[i].getAmount()})+durationText);
					}
				}
			}
		}
		if(!hasFound){
			if(m.getAmount()>0){
				m.setTitle(WordingGame.getText(109));
				m.setDescription(WordingGame.getText(108, new List<int>{m.getAmount()})+durationText);
			}
			else{
				m.setTitle(WordingGame.getText(111));
				m.setDescription(WordingGame.getText(110, new List<int>{m.getAmount()})+durationText);
			}
			this.lifeModifyers.Add(m);
		}
		Game.instance.updateModifyers(this.id);
		this.showIcons(true);
	}

	public void addShieldModifyer(ModifyerM m){
		string durationText = "" ;
		if(m.getDuration()==1){
			durationText = WordingGame.getText(100);
		}
		else{
			durationText = WordingGame.getText(2);
		}
		for(int i = 0 ; i < bouclierModifyers.Count ;i++){
			if(m.getDuration()==bouclierModifyers[i].getDuration()){
				bouclierModifyers.RemoveAt(i);
			}
		}

		m.setTitle(WordingGame.getText(88));
		m.setDescription(WordingGame.getText(87, new List<int>{m.getAmount()})+durationText);
		this.bouclierModifyers.Add(m);
		
		Game.instance.updateModifyers(this.id);
	}

	public void addEsquiveModifyer(ModifyerM m){
		string durationText = "" ;
		if(m.getDuration()==1){
			durationText = WordingGame.getText(100);
		}
		else{
			durationText = WordingGame.getText(2);
		}
		for(int i = 0 ; i < esquiveModifyers.Count ;i++){
			if(m.getDuration()==esquiveModifyers[i].getDuration()){
				esquiveModifyers.RemoveAt(i);
			}
		}

		m.setTitle(WordingGame.getText(122));
		m.setDescription(WordingGame.getText(121, new List<int>{m.getAmount()})+durationText);
		this.esquiveModifyers.Add(m);
		
		Game.instance.updateModifyers(this.id);
	}

	public void startClignote(){
		this.timerClignote = 0f;
		this.clignoteGrowing = true ;
		this.moveForward();
		this.showBackTile(false);
		this.clignoting=true;
	}

	public bool canMoveOn(int x, int y){
		return this.destinations[x,y] ;
	}

	public void setDestinations(bool[,] tab){
		this.destinations = tab;
	}

	public void initDestinations(){
		this.destinations = new bool[Game.instance.getBoard().getBoardWidth(),Game.instance.getBoard().getBoardHeight()];
		for (int i = 0 ; i < Game.instance.getBoard().getBoardWidth() ; i++){
			for (int j = 0 ; j < Game.instance.getBoard().getBoardHeight() ; j++){
				this.destinations[i,j]=false;
			}
		}
	}

	public void move(bool b){
		this.moved = b ;
	}

	public bool hasMoved(){
		return this.moved;
	}

	public bool hasPlayed(){
		return this.played;
	}

	public void play(bool b){
		this.played = b;
	}

	public bool isDead(){
		return this.dead;
	}

	public bool canMove(){
		return !this.moved;
	}

	public void resize(float tileScale){
		gameObject.transform.localScale = new Vector3(tileScale,tileScale,tileScale);
		Vector3 p = Game.instance.getBoard().getTileC(this.tile).getPosition();
		gameObject.transform.localPosition = new Vector3(p.x, p.y, -0.5f);
	}

	public void setPosition(Vector3 p){
		gameObject.transform.localPosition = new Vector3(p.x, p.y, -0.5f);
	}

	public Vector3 getPosition(){
		return gameObject.transform.localPosition;
	}

	public bool isMeasuringPush(){
		return this.measuringPush;
	}

	public void addPushTime(float f){
		this.timerPush += f ; 
		if(this.timerPush>this.pushTime){
			this.longPush();
		}
	}

	public void addClignoteTime(float f){
		this.timerClignote += f ; 
		if(this.timerClignote>this.clignoteTime){
			this.timerClignote = 0f;
			if(stopClignoting&&!this.clignoteGrowing){
				this.clignoting = false;
				this.stopClignoting=false;
				if(!this.moving){
					this.moveBackward();
					this.showBackTile(true);
				}
			}
			else{
				this.clignoteGrowing=!this.clignoteGrowing;
			}
		}
		else{
			if(clignoteGrowing){
				gameObject.transform.localScale = new Vector3(1f+0.2f*(this.timerClignote/this.clignoteTime), 1f+0.2f*(this.timerClignote/this.clignoteTime), 1f+0.2f*(this.timerClignote/this.clignoteTime));
			}
			else{
				gameObject.transform.localScale = new Vector3(1.2f-0.2f*(this.timerClignote/this.clignoteTime), 1.2f-0.2f*(this.timerClignote/this.clignoteTime), 1.2f-0.2f*(this.timerClignote/this.clignoteTime));
			}
		}
	}

	public void setCard(CardM c, bool b, int i)
	{
		this.card = c ;
		c.setMine(b);
		backTileLayer = true;

		this.id = i ;

		this.displayedLife = 0;
		this.displayedAttack = 0;

		this.setCircles();
		this.actuCharacter();
		this.setAttack();
		this.setLife();
	}

	public int getFaction(){
		return this.card.getFaction();
	}

	public void setTile(TileM t){
		this.tile = t ;
	}

	public void setBackTile(bool b){
		if(b){
			gameObject.transform.Find("Background").FindChild("BackTile").GetComponent<SpriteRenderer>().sprite = this.backTileSprites[0];
		}
		else{
			gameObject.transform.Find("Background").FindChild("BackTile").GetComponent<SpriteRenderer>().sprite = this.backTileSprites[1];
		}
	}

	public void displayBackTile(bool b){
		this.backTileLayer = b ;
		this.showBackTile(b);
	}

	public void actuCharacter(){
		gameObject.transform.Find("Background").FindChild("Character").GetComponent<SpriteRenderer>().sprite = this.characterSprites[this.card.getSkill(0).Id];
	}

	public void checkPassiveSkills(){

	}

	public void show(bool b){
		this.showCircles(b);
		this.showAttack(b);
		this.showLife(b);
		this.showDeadLayer(b);
		this.showIcons(b);
		this.showBackTile(b);
		this.showCollider(b);
		if(!b){
			this.showHover(false);
		}
		gameObject.transform.Find("Background").GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.Find("Background").FindChild("Character").GetComponent<SpriteRenderer>().enabled = b ;
	}

	public void showCollider(bool b){
		gameObject.GetComponent<BoxCollider>().enabled = b;
	}

	public void showIcons(bool b){
		if(b && gameObject.transform.Find("Background").GetComponent<SpriteRenderer>().enabled == true){
			List<int> icons = this.getIcons();
			for(int i = 0 ; i < 3 ; i++){
				if(i<icons.Count){
					this.setIcon(i,icons[i]);
					gameObject.transform.Find("Background").FindChild("Icon"+(i+1)).GetComponent<SpriteRenderer>().enabled = true;
				}
				else{
					gameObject.transform.Find("Background").FindChild("Icon"+(i+1)).GetComponent<SpriteRenderer>().enabled = false;
				}
			}
		}
		else{
			gameObject.transform.Find("Background").FindChild("Icon1").GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.Find("Background").FindChild("Icon2").GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.Find("Background").FindChild("Icon3").GetComponent<SpriteRenderer>().enabled = false ;
		}
	}

	public void checkModifyers(){
		for (int i = this.attackModifyers.Count-1 ; i >=0 ; i--){
			if (this.attackModifyers [i].getDuration() == 1)
			{
				this.attackModifyers.RemoveAt(i);
			}
		}
		
		for (int i = this.moveModifyers.Count-1 ; i >=0 ; i--){
			if (this.moveModifyers [i].getDuration() == 1)
			{
				this.moveModifyers.RemoveAt(i);
			}
		}

		for (int i = this.lifeModifyers.Count-1 ; i >=0 ; i--){
			if (this.lifeModifyers [i].getDuration() == 1)
			{
				this.lifeModifyers.RemoveAt(i);
			}
		}
		
		for (int i = this.esquiveModifyers.Count-1 ; i >=0 ; i--){
			if (this.esquiveModifyers [i].getDuration() == 1)
			{
				this.esquiveModifyers.RemoveAt(i);
			}
		}

		for (int i = this.stateModifyers.Count-1 ; i >=0 ; i--){
			if (this.stateModifyers [i].getDuration() == 1)
			{
				this.stateModifyers.RemoveAt(i);
			}
		}

		for (int i = this.bouclierModifyers.Count-1 ; i >=0 ; i--){
			if (this.bouclierModifyers [i].getDuration() == 1)
			{
				this.bouclierModifyers.RemoveAt(i);
			}
		}
		this.setLife();
		this.setAttack();
		this.showIcons(true);
	}

	public void setIcon(int i, int idSkill){
		gameObject.transform.Find("Background").FindChild("Icon"+(i+1)).GetComponent<SpriteRenderer>().sprite = this.iconSprites[Math.Abs(idSkill)];

		if(idSkill>=0){
			gameObject.transform.Find("Background").FindChild("Icon"+(i+1)).GetComponent<SpriteRenderer>().color = new Color(60f/255f, 160f/255f, 100f/255f, 1f);
		}
		else{
			gameObject.transform.Find("Background").FindChild("Icon"+(i+1)).GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
		}
	}

	public void showDeadLayer(bool b){
		if(b){
			gameObject.transform.Find("Background").FindChild("DeadLayer").GetComponent<SpriteRenderer>().enabled = this.deadLayer ;
		}
		else{
			gameObject.transform.Find("Background").FindChild("DeadLayer").GetComponent<SpriteRenderer>().enabled = b ;
		}
	}

	public void showBackTile(bool b){
		if(b){
			gameObject.transform.Find("Background").FindChild("BackTile").GetComponent<SpriteRenderer>().enabled = this.backTileLayer ;
		}
		else{
			gameObject.transform.Find("Background").FindChild("BackTile").GetComponent<SpriteRenderer>().enabled = b ;
		}
	}

	public void showCircles(bool b){
		gameObject.transform.Find("Background").FindChild("Circle").GetComponent<SpriteRenderer>().enabled = b ;
	}

	public void showAttack(bool b){
		gameObject.transform.Find("Background").FindChild("AttackValue").GetComponent<MeshRenderer>().enabled = b ;
	}

	public void showLife(bool b){
		gameObject.transform.Find("Background").FindChild("PVValue").GetComponent<MeshRenderer>().enabled = b ;
	}

	public void setCircles(){
		if (this.card.isMine()){
			gameObject.transform.Find("Background").FindChild("Circle").GetComponent<SpriteRenderer>().sprite = this.circleSprites[0];
		}
		else {
			gameObject.transform.Find("Background").FindChild("Circle").GetComponent<SpriteRenderer>().sprite = this.circleSprites[1];
		}
	}

	public void setAttack(){
		this.upgradingAttack = this.getAttack()!=this.displayedAttack ;
		this.originalAttack = int.Parse(this.attackText.text);
		this.upgradeAttackTimer = 0f;
	}

	public void setLife(){
		this.upgradingLife = this.getLife()!=this.displayedLife ;
		this.originalLife = this.displayedLife;
		this.upgradeLifeTimer = 0f;
	}

	public bool isUpgradingAttack(){
		return this.upgradingAttack;
	}

	public bool isUpgradingLife(){
		return this.upgradingLife;
	}

	public void addTimeAttack(float f){
		this.upgradeAttackTimer += f ; 
		if(this.upgradeAttackTimer>this.upgradeTime){
			this.setAttackText(this.getAttack());
			this.upgradeAttackTimer = 0f;
			this.upgradingAttack = false;
		}
		else{
			int tempInt = (this.originalAttack+Mathf.RoundToInt((this.upgradeAttackTimer/this.upgradeTime)*(this.getAttack()-this.originalAttack)));
			if(this.displayedAttack!=tempInt){
				this.setAttackText(tempInt) ;
			}
		}
	}

	public void addTimeLife(float f){
		this.upgradeLifeTimer += f ; 
		if(this.upgradeLifeTimer>this.upgradeTime){
			this.setLifeText(this.getLife());
			this.upgradeLifeTimer = 0f;
			this.upgradingLife = false;
			if(this.displayedLife==0){
				this.kill();
			}
		}
		else{
			int tempInt = (this.originalLife+Mathf.RoundToInt((this.upgradeLifeTimer/this.upgradeTime)*(this.getLife()-this.originalLife)));
			if(this.displayedLife!=tempInt){
				this.setLifeText(tempInt) ;
			}
		}
	}

	public void kill(){
		this.showCollider(false);
		this.displayDead();
	}

	public string getDoubleCharacterText(int i){
		string text = "";
		if(i<10){
			text = "0"+i;
		}
		else{
			text = ""+i;
		}
		return text;
	}

	public void setLifeText(int i){
		if(1.0f*i/this.card.getLife()<0.25f){
			this.lifeText.color = new Color(231f/255f, 0f, 66f/255f, 1f);
		}
		else if(1.0f*i/this.card.getLife()<0.5f){
			this.lifeText.color = new Color(243f/255f, 110f/255f, 42f/255f, 1f);
		}
		else if(1.0f*i/this.card.getLife()<1f){
			this.lifeText.color = new Color(255f/255f, 220f/255f, 20f/255f, 1f);
		}
		else if(1.0f*i/this.card.getLife()==1f){
			this.lifeText.color = new Color(71f/255f,150f/255f,189f/255f, 1f);
		}
		else{
			this.lifeText.color = new Color(60f/255f, 160f/255f, 100f/255f, 1f);
		}
		this.lifeText.text = this.getDoubleCharacterText(i);
		this.displayedLife = i;
	}

	public void setAttackText(int i){

		if(1.0f*i/this.card.getAttack()<0.25f){
			this.attackText.color = new Color(231f/255f, 0f, 66f/255f, 1f);
		}
		else if(1.0f*i/this.card.getAttack()<0.5f){
			this.attackText.color = new Color(243f/255f, 110f/255f, 42f/255f, 1f);
		}
		else if(1.0f*i/this.card.getAttack()<1f){
			this.attackText.color = new Color(255f/255f, 220f/255f, 20f/255f, 1f);
		}
		else if(1.0f*i/this.card.getAttack()==1f){
			this.attackText.color = new Color(71f/255f,150f/255f,189f/255f, 1f);
		}
		else{
			this.attackText.color = new Color(60f/255f, 160f/255f, 100f/255f, 1f);
		}
		this.attackText.text = this.getDoubleCharacterText(i);
		this.displayedAttack = i;
	}

	public int getEsquive(){
		int esquive = 0;
		for(int i = 0 ; i < this.esquiveModifyers.Count ; i++){
			esquive+=this.esquiveModifyers[i].getAmount();
		}
		return esquive;
	}

	public int getBouclier(){
		int bouclier = 0;
		for(int i = 0 ; i < this.bouclierModifyers.Count ; i++){
			bouclier+=this.bouclierModifyers[i].getAmount();
		}
		return bouclier;
	}

	public int getLife(){
		int life = this.getTotalLife();
		for(int i = 0 ; i < this.damageModifyers.Count ; i++){
			life-=this.damageModifyers[i].getAmount();
		}
		if(life>this.getTotalLife()){
			life = this.getTotalLife();
		}

		return Mathf.Max(0,life);
	}

	public int getTotalLife(){
		int totalLife = this.card.getLife();
		for(int i = 0 ; i < this.lifeModifyers.Count ; i++){
			totalLife+=this.lifeModifyers[i].getAmount();
		}
		return totalLife;
	}

	public int getAttack(){
		int attack = this.card.getAttack();
		for(int i = 0 ; i < this.attackModifyers.Count ; i++){
			attack+=this.attackModifyers[i].getAmount();
		}
		return Mathf.Max(1,attack);
	}

	public int getMove(){
		int move = this.card.getMove();
		for(int i = 0 ; i < this.moveModifyers.Count ; i++){
			move+=this.moveModifyers[i].getAmount();
		}
		if(this.isSniper()){
			return 1;
		}
		else{
			return Mathf.Max(1,move);
		}
	}

	public List<int> getIcons(){
		List<int> icons = new List<int>();
		int compteur = 0;
		int id ;
		for(int i = 0 ; i < stateModifyers.Count && compteur<3 ; i++){
			id = this.stateModifyers[i].getIdIcon();
			if(this.stateModifyers[i].getAmount()<=0){
				id = -1*id;
			}
			icons.Add(id);
			compteur++;
		}
		if(esquiveModifyers.Count>0 && compteur<3){
			id = this.esquiveModifyers[0].getIdIcon();
			if(this.esquiveModifyers[0].getAmount()<=0){
				id = -1*id;
			}
			icons.Add(id);
			compteur++;
		}
		if(bouclierModifyers.Count>0 && compteur<3){
			id = this.bouclierModifyers[0].getIdIcon();
			if(this.bouclierModifyers[0].getAmount()<=0){
				id = -1*id;
			}
			icons.Add(this.bouclierModifyers[0].getIdIcon());
			compteur++;
		}
		if(moveModifyers.Count>0 && compteur<3){
			id = this.moveModifyers[0].getIdIcon();
			if(this.moveModifyers[0].getAmount()<=0){
				id = -1*id;
			}
			icons.Add(id);
			compteur++;
		}
		return icons;
	}

	public void OnMouseEnter()
	{
		if(Game.instance.getDraggingSBID()!=-1){
			Game.instance.getBoard().getTileC(this.tile).OnMouseEnter();
		}
		else{
			if(Game.instance.getDraggingCardID()==-1){
				this.showHover(true);
				if(!Game.instance.isMobile()){
					if(this.card.isMine()){
						Game.instance.hoverMyCard(this.id);
					}
					else{
						Game.instance.hoverHisCard(this.id);
					}
				}
			}
		}
	}

	public void OnMouseDown()
	{
		if(Game.instance.isMobile()){
			this.timerPush = 0f;
			this.measuringPush = true;
		}
		else{
			Game.instance.hitCard(this.id);
		}
	}

	public void OnMouseUp()
	{
		if(this.measuringPush){
			this.shortPush();
		}
		else if(Game.instance.getDraggingCardID()!=-1){
			TileM tile = Game.instance.getBoard().getMouseTile();

			if(tile.x>=0 && tile.x<Game.instance.getBoard().getBoardWidth() && tile.y>=0 && tile.y<Game.instance.getBoard().getBoardHeight()){
				if(tile.x!=this.tile.x || tile.y!=this.tile.y){
					Game.instance.dropOnTile(tile.x,tile.y);
				}
				else{
					Game.instance.dropOutsideBoard();
				}
			}
			else{
				Game.instance.dropOutsideBoard();
			}
		}
	}

	public void shortPush(){
		this.measuringPush = false ;
	}

	public void longPush(){
		this.measuringPush = false ;
		Game.instance.hitCard(this.id);
	}

	public void OnMouseExit()
	{
		this.showHover(false);
	}

	public void showHover(bool b)
	{
		gameObject.transform.Find("Background").FindChild("Hover").GetComponent<SpriteRenderer>().enabled = b;
	}

	public Sprite getCharacterSprite(){
		return this.characterSprites[this.card.getCharacterType()];
	}

	public CardM getCardM(){
		return this.card;
	}

	public virtual string getSkillText(int i){
		int index ;
		int percentage ;
		string tempstring ;
		string s = WordingSkills.getDescription(this.card.getSkill(i).Id, this.card.getSkill(i).Power-1);
		if (s.Contains("%BTK")){
			index = s.IndexOf("%BTK");
			
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.Max(1,Mathf.RoundToInt(Int32.Parse(tempstring)*this.card.getAttack()/100f));
			s = s.Substring(0,index-3)+percentage+s.Substring(index+4,s.Length-index-4);
		}
		if (s.Contains("%ATK")){
			index = s.IndexOf("%ATK");
			
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.Max(1,Mathf.RoundToInt(Int32.Parse(tempstring)*this.getAttack()/100f));
			s = s.Substring(0,index-3)+percentage+s.Substring(index+4,s.Length-index-4);
		}
		if (s.Contains("%ATK")){
			index = s.IndexOf("%ATK");
			
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.Max(1,Mathf.RoundToInt(Int32.Parse(tempstring)*this.getAttack()/100f));
			s = s.Substring(0,index-3)+percentage+s.Substring(index+4,s.Length-index-4);
		}
		if (s.Contains("%PV")){
			index = s.IndexOf("%PV");
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.Max(1,Mathf.RoundToInt(Int32.Parse(tempstring)*this.getLife()/100f));
			s = s.Substring(0,index-4)+" "+percentage+" "+s.Substring(index+4,s.Length-index-4);
		}
		if (WordingSkills.getProba(this.card.getSkill(i).Id, this.card.getSkill(i).Power-1)!=100){
			s+=". HIT%"+WordingSkills.getProba(this.card.getSkill(i).Id, this.card.getSkill(i).Power-1);
		}
		return s;
	}

	public virtual string getAttackText(){
		int index ;
		int percentage ;
		string tempstring ;
		string s = WordingSkills.getDescription(0, 1);
		if (s.Contains("%BTK")){
			index = s.IndexOf("%BTK");
			
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.Max(1,Mathf.RoundToInt(Int32.Parse(tempstring)*this.card.getAttack()/100f));
			s = s.Substring(0,index-3)+percentage+s.Substring(index+4,s.Length-index-4);
		}
		if (s.Contains("%ATK")){
			index = s.IndexOf("%ATK");
			
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.Max(1,Mathf.RoundToInt(Int32.Parse(tempstring)*this.getAttack()/100f));
			s = s.Substring(0,index-3)+percentage+s.Substring(index+4,s.Length-index-4);
		}
		if (s.Contains("%ATK")){
			index = s.IndexOf("%ATK");
			
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.Max(1,Mathf.RoundToInt(Int32.Parse(tempstring)*this.getAttack()/100f));
			s = s.Substring(0,index-3)+percentage+s.Substring(index+4,s.Length-index-4);
		}
		if (s.Contains("%PV")){
			index = s.IndexOf("%PV");
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.Max(1,Mathf.RoundToInt(Int32.Parse(tempstring)*this.getLife()/100f));
			s = s.Substring(0,index-4)+" "+percentage+" "+s.Substring(index+4,s.Length-index-4);
		}

		return s;
	}

	public List<ModifyerM> getEffects(){
		List<ModifyerM> effects = new List<ModifyerM>();
		for(int i = 0 ; i < this.stateModifyers.Count && effects.Count<10 ; i++){
			effects.Add(new ModifyerM(this.stateModifyers[i].getAmount(), this.stateModifyers[i].getIdIcon(), this.stateModifyers[i].getDescription(), this.stateModifyers[i].getTitle(),this.stateModifyers[i].getDuration()));
		}

		for(int i = 0 ; i < this.esquiveModifyers.Count && effects.Count<10 ; i++){
			effects.Add(new ModifyerM(this.esquiveModifyers[i].getAmount(), this.esquiveModifyers[i].getIdIcon(), this.esquiveModifyers[i].getDescription(), this.esquiveModifyers[i].getTitle(),this.esquiveModifyers[i].getDuration()));
		}

		for(int i = 0 ; i < this.bouclierModifyers.Count && effects.Count<10 ; i++){
			effects.Add(new ModifyerM(this.bouclierModifyers[i].getAmount(), this.bouclierModifyers[i].getIdIcon(), this.bouclierModifyers[i].getDescription(), this.bouclierModifyers[i].getTitle(),this.bouclierModifyers[i].getDuration()));
		}

		for(int i = 0 ; i < this.moveModifyers.Count && effects.Count<10 ; i++){
			effects.Add(new ModifyerM(this.moveModifyers[i].getAmount(), this.moveModifyers[i].getIdIcon(), this.moveModifyers[i].getDescription(), this.moveModifyers[i].getTitle(),this.moveModifyers[i].getDuration()));
		}

		for(int i = 0 ; i < this.attackModifyers.Count && effects.Count<10 ; i++){
			effects.Add(new ModifyerM(this.attackModifyers[i].getAmount(), this.attackModifyers[i].getIdIcon(), this.attackModifyers[i].getDescription(), this.attackModifyers[i].getTitle(),this.attackModifyers[i].getDuration()));
		}

		for(int i = 0 ; i < this.lifeModifyers.Count && effects.Count<10 ; i++){
			effects.Add(new ModifyerM(this.lifeModifyers[i].getAmount(), this.lifeModifyers[i].getIdIcon(), this.lifeModifyers[i].getDescription(), this.lifeModifyers[i].getTitle(),this.lifeModifyers[i].getDuration()));
		}

		return effects ;
	}

	public Sprite getIconSprite(int i){
		return this.iconSprites[i];
	}

	public TileM getTileM(){
		return this.tile;
	}

	public void startMove(Vector3 s, Vector3 e){
		this.startPosition = new Vector3(s.x, s.y, -0.5f) ;
		this.endPosition = new Vector3(e.x, e.y, -0.5f) ;
		this.timerMove = 0f;
		this.moving = true;
		this.moveForward();
	}

	public void addMoveTime(float f){
		this.timerMove+=f;
		Vector3 p = new Vector3(0f, 0f, -0.5f);
		p.x = this.startPosition.x+(this.endPosition.x-this.startPosition.x)*(Mathf.Sqrt(Mathf.Min(1,this.timerMove/this.moveTime)));
		p.y = this.startPosition.y+(this.endPosition.y-this.startPosition.y)*(Mathf.Sqrt(Mathf.Min(1,this.timerMove/this.moveTime)));
		this.setPosition(p);
		if(this.timerMove>this.moveTime){
			this.moving=false;
			if(Game.instance.getCurrentCardID()!=-1 || this.getCardM().isMine()){
				this.showCollider(true);
			}

			if(this.id!=Game.instance.getCurrentCardID() && (Game.instance.getCurrentCardID()!=-1 || this.getCardM().isMine())){
				this.displayBackTile(true);
			}
			if(this.id!=Game.instance.getCurrentCardID()){
				this.moveBackward();
			}

			if(Game.instance.getBoard().getTileC(this.getTileM()).isTrapped()){
				int value = Game.instance.getBoard().getTileC(this.getTileM()).getTrapValue();
				TileM tile ;
				Game.instance.getBoard().getTileC(this.getTileM()).displayAnim(1);
				if(Game.instance.getBoard().getTileC(this.getTileM()).getTrapId()==0){
					this.displaySkillEffect(WordingGame.getText(8, new List<int>{2+value})+"\n"+WordingGame.getText(117),2);
					this.addDamageModifyer(new ModifyerM(2+value, -1, "", "", -1));
					tile = Game.instance.getBoard().getRandomEmptyTile();
					Game.instance.getBoard().getTileC(this.getTileM()).removeTrap();
					Game.instance.moveOn(tile.x,tile.y,this.id);
				}
				else if(Game.instance.getBoard().getTileC(this.getTileM()).getTrapId()==1){
					this.displaySkillEffect(WordingGame.getText(8, new List<int>{5+2*value}),2);
					this.addDamageModifyer(new ModifyerM(5+2*value, -1, "", "", -1));
					Game.instance.getBoard().getTileC(this.getTileM()).removeTrap();
				}
				else if(Game.instance.getBoard().getTileC(this.getTileM()).getTrapId()==2){
					this.displaySkillEffect(WordingGame.getText(83, new List<int>{-1*value})+"\n"+WordingGame.getText(100),2);
					this.addAttackModifyer(new ModifyerM(-1*value, 0, "", "", -1));
					Game.instance.getBoard().getTileC(this.getTileM()).removeTrap();
				}
			}
			Game.instance.endMove();
		}
	}

	public bool isMoving(){
		return this.moving;
	}

	public void moveForward(){
		Transform t = gameObject.transform;
		t.Find("Background").GetComponent<SpriteRenderer>().sortingOrder = 31 ;
		t.Find("Background").FindChild("Hover").GetComponent<SpriteRenderer>().sortingOrder = 36 ;
		t.Find("Background").FindChild("DeadLayer").GetComponent<SpriteRenderer>().sortingOrder = 35 ;
		t.Find("Background").FindChild("AttackValue").GetComponent<MeshRenderer>().sortingOrder = 34 ;
		t.Find("Background").FindChild("PVValue").GetComponent<MeshRenderer>().sortingOrder = 34 ;
		t.Find("Background").FindChild("Icon1").GetComponent<SpriteRenderer>().sortingOrder = 34 ;
		t.Find("Background").FindChild("Icon2").GetComponent<SpriteRenderer>().sortingOrder = 34 ;
		t.Find("Background").FindChild("Icon3").GetComponent<SpriteRenderer>().sortingOrder = 34 ;
		t.Find("Background").FindChild("Circle").GetComponent<SpriteRenderer>().sortingOrder = 33 ;
		t.Find("Background").FindChild("Character").GetComponent<SpriteRenderer>().sortingOrder = 32 ;
		t.Find("Background").FindChild("BackTile").GetComponent<SpriteRenderer>().sortingOrder = 1 ;
	}
	
	public void moveBackward(){
		Transform t = gameObject.transform;
		t.Find("Background").GetComponent<SpriteRenderer>().sortingOrder = 21 ;
		t.Find("Background").FindChild("Hover").GetComponent<SpriteRenderer>().sortingOrder = 26 ;
		t.Find("Background").FindChild("DeadLayer").GetComponent<SpriteRenderer>().sortingOrder = 25 ;
		t.Find("Background").FindChild("AttackValue").GetComponent<MeshRenderer>().sortingOrder = 24 ;
		t.Find("Background").FindChild("PVValue").GetComponent<MeshRenderer>().sortingOrder = 24 ;
		t.Find("Background").FindChild("Icon1").GetComponent<SpriteRenderer>().sortingOrder = 24 ;
		t.Find("Background").FindChild("Icon2").GetComponent<SpriteRenderer>().sortingOrder = 24 ;
		t.Find("Background").FindChild("Icon3").GetComponent<SpriteRenderer>().sortingOrder = 24 ;
		t.Find("Background").FindChild("Circle").GetComponent<SpriteRenderer>().sortingOrder = 23 ;
		t.Find("Background").FindChild("Character").GetComponent<SpriteRenderer>().sortingOrder = 22 ;
		t.Find("Background").FindChild("BackTile").GetComponent<SpriteRenderer>().sortingOrder = 1 ;
	}

	public bool canBeTargeted(){
		return !this.isDead() ;
	}

	public void displaySkillEffect(string s, int type){
		if(this.getCardM().isMine()||Game.instance.getCurrentCardID()!=-1){
			this.skillEffects.Add(s);
			this.skillEffectTypes.Add(type);
			Game.instance.getPassButton().grey();
			if(!this.skillEffect){
				this.launchSkillEffect();
			}
		}
	}

	public void displayAnim(int type){
		this.skillEffects.Add("");
		this.skillEffectTypes.Add(10*(type+1));
		if(!this.skillEffect){
			this.launchSkillEffect();
		}
	}

	public void displayDead(){
		this.skillEffects.Add("");
		this.skillEffectTypes.Add(666);
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
		if(type==666){
			this.SETime = 2f;
			this.deadLayer=true;
			this.anim = 666;
			this.showDeadLayer(true);
		}
		else if(type<10){
			this.SETime = 1.5f;
			gameObject.transform.Find("Background").FindChild("EffectText").GetComponent<TextMeshPro>().text = this.skillEffects[0];
			if(type==0){
				gameObject.transform.Find("Background").FindChild("EffectText").GetComponent<TextMeshPro>().color = new Color(60f/255f, 160f/255f, 100f/255f, 0f);
			}
			else if(type==1){
				gameObject.transform.Find("Background").FindChild("EffectText").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 0f);
			}
			else{
				gameObject.transform.Find("Background").FindChild("EffectText").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 0f);
			}
			this.anim = -1;
			this.showSE(true);
		}
		else{
			this.SETime = 0.8f;
			this.anim = (type-10);
			gameObject.transform.Find("Background").FindChild("Anim").GetComponent<SpriteRenderer>().sprite = Game.instance.getAnimSprite(this.anim);
			this.showAnim(true);
		}
		this.skillEffects.RemoveAt(0);
		this.skillEffectTypes.RemoveAt(0);
		this.timerSE = 0f;
	}

	public void addSETime(float f){
		this.timerSE+=f;
		if(this.anim==666){
			
		}
		else if(this.anim<0){
			gameObject.transform.Find("Background").FindChild("EffectText").localPosition = new Vector3(0f, -0.25f+0.5f*Mathf.Min(1f,this.timerSE/SETime) ,0f);
			Color c = gameObject.transform.Find("Background").FindChild("EffectText").GetComponent<TextMeshPro>().color;
			c.a = Mathf.Min(1f,2f*this.timerSE/SETime);
			gameObject.transform.Find("Background").FindChild("EffectText").GetComponent<TextMeshPro>().color = c;
		}
		else{
			int tempInt = Mathf.Min(9,Mathf.FloorToInt(10f*this.timerSE/SETime));
			if(tempInt!=this.anim%10){
				this.anim++ ;
				gameObject.transform.Find("Background").FindChild("Anim").GetComponent<SpriteRenderer>().sprite = Game.instance.getAnimSprite(this.anim);
			}
		}

		if(this.timerSE>SETime){
			if(this.anim==666){
				this.show(false);
				this.dead = true;
				int characterID;

				if(this.getCardM().getCharacterType()==73){
					List<TileM> neighbours = Game.instance.getBoard().getTileNeighbours(this.getTileM());
					for (int j = 0 ; j < neighbours.Count ;j++){
						characterID = Game.instance.getBoard().getTileC(neighbours[j]).getCharacterID();
						if(characterID!=-1){
							if(this.getCardM().isMine()!=Game.instance.getCards().getCardC(characterID).getCardM().isMine()){
								Game.instance.getCards().getCardC(characterID).removePaladinModifyer();
							}
						}
					}
				}
				else if(this.getCardM().getCharacterType()==111){
					List<TileM> neighbours = Game.instance.getBoard().getTileNeighbours(this.getTileM());
					for (int j = 0 ; j < neighbours.Count ;j++){
						characterID = Game.instance.getBoard().getTileC(neighbours[j]).getCharacterID();
						if(characterID!=-1){
							if(this.getCardM().isMine()!=Game.instance.getCards().getCardC(characterID).getCardM().isMine()){
								Game.instance.getCards().getCardC(characterID).removeProtecteurModifyer();
							}
						}
					}
				}

				if(this.getCardM().getCharacterType()==76){
					for (int j = 0 ; j < Game.instance.getCards().getNumberOfCards() ;j++){
						if(Game.instance.getCards().getCardC(j).getCardM().isMine()==this.getCardM().isMine()){
							if(Game.instance.getCards().getCardC(j).hasLeaderModifyer()){
								Game.instance.getCards().getCardC(j).removeLeaderModifyer();
							}
						}
					}
				}

				if(Game.instance.isIA()||Game.instance.isTutorial()){
					Game.instance.areUnitsDead(this.getCardM().isMine());
				}
				else{
					if(this.getCardM().isMine()){
						Game.instance.areUnitsDead(this.getCardM().isMine());
					}
				}
				Game.instance.getBoard().getTileC(this.tile).setCharacterID(-1);
				Game.instance.getBoard().getTileC(this.tile).showHover(false);
				Game.instance.getBoard().getTileC(this.tile).showCollider(true);
				if(this.id==Game.instance.getCurrentCardID()){
					this.displayBackTile(false);
					this.showHover(false);
					Game.instance.hitPassButton();
				}
				Game.instance.loadDestinations();
			}
			else if(this.anim>=0){
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

	public void showSE(bool b)
	{
		gameObject.transform.Find("Background").FindChild("EffectText").GetComponent<MeshRenderer>().enabled = b;
	}

	public void showAnim(bool b)
	{
		gameObject.transform.Find("Background").FindChild("Anim").GetComponent<SpriteRenderer>().enabled = b;
	}

	public int getChasseurLevel(){
		int level = -1 ;
		for (int i = this.stateModifyers.Count-1 ; i >=0 ; i--){
			if(this.stateModifyers[i].getIdIcon()==17){
				level = this.stateModifyers[i].getDuration();
			}
		}
		return level;
	}

	public int getChasseurTarget(){
		int level = 0 ;
		for (int i = this.stateModifyers.Count-1 ; i >=0 ; i--){
			if(this.stateModifyers[i].getIdIcon()==17){
				level = this.stateModifyers[i].getAmount();
			}
		}
		return level;
	}

	public int getDegatsAgainst(CardC target, int d){
		int degats = 0;
		if(this.card.getCharacterType()==71 && target.getLife()>=50){ 
			degats = Mathf.Min(target.getLife(),Mathf.Max(1,Mathf.RoundToInt(Mathf.RoundToInt(d*(110f+4f*this.card.getSkill(0).Power)/100f)*(1-(target.getBouclier()/100f)))));
		}
		else{
			degats = Mathf.Min(target.getLife(),Mathf.Max(1,Mathf.RoundToInt(d*(1-(target.getBouclier()/100f)))));
		}

		if(this.getChasseurLevel()>0){
			if(Game.instance.getCards().getCardC(this.getChasseurTarget()).getCardM().getCharacterType()==target.getCardM().getCharacterType()){
				degats = degats+Mathf.RoundToInt((degats*this.getChasseurLevel())/100f);
			}
		}

		if(this.isSniper()){
			degats = degats - Mathf.RoundToInt(((50f-5f*this.getCardM().getSkill(0).Power)*degats)/100f);
		}

		if(this.getBonusDamages()>0){
			degats = degats+Mathf.RoundToInt((degats*this.getBonusDamages())/100f);
		}

		if(this.isSanguinaire()){
			degats = degats+Mathf.RoundToInt((25f*degats)/100f);
		}

		if(this.card.getCharacterType()==65){
			if(!Game.instance.getCurrentCard().hasMoved()){
				degats+=2+this.getCardM().getSkill(0).Power;
			}
		}

		return degats;
	}

	public int getDegatsNoShieldAgainst(CardC target, int d){
		int degats = 0;
		if(this.card.getCharacterType()==71 && target.getLife()>=50){ 
			degats = Mathf.Min(target.getLife(),Mathf.Max(1,Mathf.RoundToInt(Mathf.RoundToInt(d*(110f+4f*this.card.getSkill(0).Power)/100f))));
		}
		else{
			degats = Mathf.Min(target.getLife(),Mathf.Max(1,Mathf.RoundToInt(d*(1-(target.getBouclier()/100f)))));
		}

		if(this.getChasseurLevel()>0){
			if(Game.instance.getCards().getCardC(this.getChasseurTarget()).getCardM().getCharacterType()==target.getCardM().getCharacterType()){
				degats = degats+Mathf.RoundToInt((degats*this.getChasseurLevel())/100f);
			}
		}

		if(this.isSniper()){
			degats = degats - Mathf.RoundToInt(((50f-5f*this.getCardM().getSkill(0).Power)*degats)/100f);
		}

		if(this.getBonusDamages()>0){
			degats = degats+Mathf.RoundToInt((degats*this.getBonusDamages())/100f);
		}

		if(this.isSanguinaire()){
			degats = degats+Mathf.RoundToInt(25f/100f);
		}

		if(this.card.getCharacterType()==65){
			if(!Game.instance.getCurrentCard().hasMoved()){
				degats+=2+this.getCardM().getSkill(0).Power;
			}
		}

		return degats;
	}

	public int getDegatsMaxAgainst(CardC target, int d){
		int degats = 0;
		if(this.card.getCharacterType()==71 && target.getLife()>=50){ 
			degats = Mathf.Max(1,Mathf.RoundToInt(Mathf.RoundToInt(d*(110f+4f*this.card.getSkill(0).Power)/100f)*(1-(target.getBouclier()/100f))));
		}
		else{
			degats = Mathf.Max(1,Mathf.RoundToInt(d*(1-(target.getBouclier()/100f))));
		}

		if(this.getChasseurLevel()>0){
			if(Game.instance.getCards().getCardC(this.getChasseurTarget()).getCardM().getCharacterType()==target.getCardM().getCharacterType()){
				degats = degats+Mathf.RoundToInt((degats*this.getChasseurLevel())/100f);
			}
		}

		if(this.isSniper()){
			degats = degats - Mathf.RoundToInt(((50f-5f*this.getCardM().getSkill(0).Power)*degats)/100f);
		}

		if(this.getBonusDamages()>0){
			degats = degats+Mathf.RoundToInt((degats*this.getBonusDamages())/100f);
		}

		if(this.isSanguinaire()){
			degats = degats+Mathf.RoundToInt(25f/100f);
		}

		if(this.card.getCharacterType()==65){
			if(!Game.instance.getCurrentCard().hasMoved()){
				degats+=2+this.getCardM().getSkill(0).Power;
			}
		}

		return degats;
	}

	public int getDamageScore(CardC target, int d){
		int degats = this.getDegatsAgainst(target, d);
		if(degats!=0){
			if(degats==target.getLife()){
				degats = 100+target.getLife()+target.getAttack();
			}
			else{
				degats+=Mathf.RoundToInt((60-target.getLife())/10f)+(Mathf.RoundToInt(target.getAttack()/5f));
			}
		}
		if(!target.getCardM().isMine()){
			degats = -1*degats;
		}
		return degats;
	}

	public int getDamageScoreNoShield(CardC target, int d){
		int degats = this.getDegatsNoShieldAgainst(target, d);
		if(degats!=0){
			if(degats==target.getLife()){
				degats = 100+target.getLife()+target.getAttack();
			}
			else{
				degats+=Mathf.RoundToInt((60-target.getLife())/10f)+(Mathf.RoundToInt(target.getAttack()/5f));
			}
		}
		if(!target.getCardM().isMine()){
			degats = -1*degats;
		}
		return degats;
	}

	public int getDamageScore(CardC target, int dmin, int dmax){
		int degatsMin = this.getDegatsAgainst(target, dmin);
		int degatsMax = this.getDegatsMaxAgainst(target, dmax);
		int degats = Mathf.RoundToInt((100+target.getLife()+target.getAttack())*(Mathf.Max(0,degatsMax+1-target.getLife()))+(Mathf.RoundToInt((60-target.getLife())/10f)+(Mathf.RoundToInt(target.getAttack()/5f)))*(Mathf.Max(0,target.getLife()-degatsMin)))/(degatsMax+1-degatsMin);

		if(!target.getCardM().isMine()){
			degats = -1*degats;
		}
		return degats;
	}

	public bool isParalized(){
		bool p = false;
		for(int i = 0 ; i < stateModifyers.Count ;i++){
			if(stateModifyers[i].getIdIcon()==2){
				p = true ;
			}
		}
		return p;
	}

	public void meteorEffect(int degats){
		this.displayAnim(0);

		degats = degats - Mathf.RoundToInt((this.getSniperModifyer()*degats)/100f);

		this.displaySkillEffect(WordingGame.getText(43, new List<int>{degats}), 2);
		this.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
	}

	public void removePaladinModifyer(){
		for (int i = attackModifyers.Count-1 ; i>=0 ; i--){
			if(this.attackModifyers[i].getIdIcon()==3){
				this.attackModifyers.RemoveAt(i);
			}
		}
		Game.instance.updateModifyers(this.id);
		this.setAttack();
	}

	public void removeProtecteurModifyer(){
		for (int i = bouclierModifyers.Count-1 ; i>=0 ; i--){
			if(this.bouclierModifyers[i].getIdIcon()==16){
				this.bouclierModifyers.RemoveAt(i);
			}
		}
		Game.instance.updateModifyers(this.id);
	}

	public void removeLeaderModifyer(){
		for (int i = attackModifyers.Count-1 ; i>=0 ; i--){
			if(this.attackModifyers[i].getIdIcon()==4){
				this.attackModifyers.RemoveAt(i);
			}
		}
		for (int i = lifeModifyers.Count-1 ; i>=0 ; i--){
			if(this.lifeModifyers[i].getIdIcon()==4){
				this.lifeModifyers.RemoveAt(i);
			}
		}
		this.displaySkillEffect(WordingGame.getText(107),0);
		Game.instance.updateModifyers(this.id);
		this.setAttack();
		this.setLife();
	}

	public bool hasPaladinModifyer(){
		bool found = false ;
		for (int i = attackModifyers.Count-1 ; i>=0 ; i--){
			if(this.attackModifyers[i].getIdIcon()==3){
				found = true ;
			}
		}
		return found;
	}

	public bool hasProtecteurModifyer(){
		bool found = false ;
		for (int i = bouclierModifyers.Count-1 ; i>=0 ; i--){
			if(this.bouclierModifyers[i].getIdIcon()==16){
				found = true ;
			}
		}
		return found;
	}

	public int getSniperModifyer(){
		int compteur = 0 ;
		for (int i = stateModifyers.Count-1 ; i>=0 ; i--){
			if(this.stateModifyers[i].getIdIcon()==8){
				compteur = this.stateModifyers[i].getAmount() ;
			}
		}
		return compteur;
	}

	public bool hasLeaderModifyer(){
		bool found = false ;
		for (int i = attackModifyers.Count-1 ; i>=0 ; i--){
			if(this.attackModifyers[i].getIdIcon()==4){
				found = true ;
			}
		}
		for (int i = lifeModifyers.Count-1 ; i>=0 ; i--){
			if(this.lifeModifyers[i].getIdIcon()==4){
				found = true ;
			}
		}
		return found;
	}

	public void emptyModifyers(){
		this.attackModifyers = new List<ModifyerM>();
		this.lifeModifyers = new List<ModifyerM>();
		this.bouclierModifyers = new List<ModifyerM>();
		this.esquiveModifyers = new List<ModifyerM>();
		this.stateModifyers = new List<ModifyerM>();
		this.moveModifyers = new List<ModifyerM>();
		this.setLife();
		this.setAttack();
		this.showIcons(true);
	}

	public bool isSniper(){
		bool found = false ;
		for (int i = stateModifyers.Count-1 ; i>=0 ; i--){
			if(this.stateModifyers[i].getIdIcon()==9){
				found = true ;
			}
		}
		return found;
	}

	public bool isFatality(){
		bool found = false ;
		for (int i = stateModifyers.Count-1 ; i>=0 ; i--){
			if(this.stateModifyers[i].getIdIcon()==15){
				found = true ;
			}
		}
		return found;
	}

	public bool isSanguinaire(){
		bool found = false ;
		for (int i = stateModifyers.Count-1 ; i>=0 ; i--){
			if(this.stateModifyers[i].getIdIcon()==11){
				found = true ;
			}
		}
		return found;
	}

	public int getBonusDamages(){
		int compteur = 0 ;
		for (int i = stateModifyers.Count-1 ; i>=0 ; i--){
			if(this.stateModifyers[i].getIdIcon()==10){
				compteur += this.stateModifyers[i].getAmount();
			}
		}
		return compteur;
	}

	public void godTransform(){
		int attackBonus = this.card.getSkill(0).Power*2+5;
		int lifeBonus = this.card.getSkill(0).Power*2+10;

		this.card.setCharacterType(144);
		this.show(true);
		this.actuCharacter();
		this.displayAnim(2);
		this.displaySkillEffect(WordingGame.getText(23), 0);
		this.addAttackModifyer(new ModifyerM(attackBonus, 20, "", "",-1));
		this.addLifeModifyer(new ModifyerM(lifeBonus, 20, "", "",-1));
		this.setAttack();
		this.setLife();
	}

	public void removeCristoMaster(){
		for (int i = attackModifyers.Count-1 ; i>=0 ; i--){
			if(this.attackModifyers[i].getIdIcon()==18){
				this.attackModifyers.RemoveAt(i);
			}
		}
	}

	public void setSkill(int i, Skill s){
		this.getCardM().setSkill(i,s);
		Game.instance.updateModifyers(this.id);
	}
}