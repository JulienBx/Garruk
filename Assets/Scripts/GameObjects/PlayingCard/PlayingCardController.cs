using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PlayingCardController : GameObjectController
{
	public Sprite[] backgroundSprites;
	public Sprite[] lifebarSprites;
	public Sprite[] iconeBackgroundSprites;
	public Sprite[] iconeSprites;
	
	GameCard card ;
	int id = -1 ;
	
	bool isHidden = false ; 
	Tile tile ;

	private bool isDisabled; // variable pour le tutoriel
	
	float timerSelection = 0 ;
	float selectionTime = 0.5f ;
	float timerMove = 0 ;
	float MoveTime = 0.5f ;
	bool isGettingBigger = true ;
	bool toStop ;
	bool isRunning ;
	bool isMoving ;
	Vector3 initialP;
	Vector3 finalP;
	float timerDead;
	float deadTime = 2f;
	public bool isShowingDead;
	public List<Tile> destinations ;
	public bool isUpdatingLife = true ;
	int actualLife;
	float timerLife = 0f ; 
	float lifeTime = 1f ;
	public bool canBeDragged ;
	
	void Awake()
	{
		this.isDisabled = false;
		this.displayDead(false);
		Transform t = gameObject.transform;
		this.toStop = false ;
		this.isRunning = false;
		this.destinations = new List<Tile>();
	}
	
	public void stopAnim(){
		this.toStop = true ;
	}
	
	public void run(){
		this.isRunning = true ;
		this.toStop = false;
	}
	
	public bool getIsRunning(){
		return this.isRunning;
	}
	
	public bool getIsMoving(){
		return this.isMoving;
	}
	
	public bool getIsHidden(){
		return this.isHidden ;
	}
	
	public void setDestinations(List<Tile> l)
	{
		this.destinations = l;
	}
	
	public List<Tile> getDestinations()
	{
		return this.destinations;
	}
	
	public void hide()
	{
		this.isHidden = true ;
		Transform t = gameObject.transform;
		t.FindChild("Background").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("Circle").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("Character").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("Icon1").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("Icon1").FindChild("Picto").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("Icon2").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("Icon2").FindChild("Picto").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("Icon3").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("Icon3").FindChild("Picto").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("AttackPicto").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("AttackValue").GetComponent<MeshRenderer>().enabled = false ;
		t.Find("Background").FindChild("PVPicto").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("PVValue").GetComponent<MeshRenderer>().enabled = false ;
		t.Find("Background").FindChild("LifeBar").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("LifeBarEnd").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("Life").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("LifeEnd").GetComponent<SpriteRenderer>().enabled = false ;
	}
	
	public void display()
	{
		this.isHidden = false ;
		Transform t = gameObject.transform;
		t.FindChild("Background").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("Circle").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("Character").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("Icon1").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("Icon1").FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("Icon2").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("Icon2").FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("Icon3").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("Icon3").FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("AttackPicto").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("AttackValue").GetComponent<MeshRenderer>().enabled = true ;
		t.Find("Background").FindChild("PVPicto").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("PVValue").GetComponent<MeshRenderer>().enabled = true ;
		t.Find("Background").FindChild("LifeBar").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("LifeBarEnd").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("Life").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("LifeEnd").GetComponent<SpriteRenderer>().enabled = true ;
		this.show();
	}
	
	public void setCard(GameCard c, bool b, int i)
	{
		this.card = c ;
		c.isMine = b ;
		gameObject.transform.Find("Background").FindChild("Character").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[this.card.getSkills()[0].Id];
		if (c.isMine){
			gameObject.transform.Find("Background").FindChild("Character").transform.localScale = new Vector3(-1,1,1);
			gameObject.transform.Find("Background").FindChild("Circle").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[0];
			gameObject.transform.Find("Background").FindChild("AttackPicto").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[1];
			gameObject.transform.Find("Background").FindChild("PVPicto").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[2];
		}
		else {
			gameObject.transform.Find("Background").FindChild("Character").transform.localScale = new Vector3(1,1,1);
			gameObject.transform.Find("Background").FindChild("Circle").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[3];
			gameObject.transform.Find("Background").FindChild("AttackPicto").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[4];
			gameObject.transform.Find("Background").FindChild("PVPicto").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[5];
		}
		
		transform.Find("Background").FindChild("AttackValue").GetComponent<TextMeshPro>().text = c.GetAttackString();

		this.card.isMine = b ;
		
		if (b){
			this.display ();
		}
		else{
			this.hide ();
		}
		
		this.id = i ;
		gameObject.name = "Card"+(i);

		this.updateLife(0);
	}
	
	public void updateLife(int value){
		this.timerLife = 0f ;
		this.actualLife = value;
		this.isUpdatingLife = true ;
	}
	
	public void updateLifePercentage(float percentage){
		gameObject.transform.Find("Background").FindChild("Life").transform.localPosition = new Vector3(-0.28f+percentage*(0.37f),-0.41f,0);
		gameObject.transform.Find("Background").FindChild("Life").transform.localScale = new Vector3(80f*percentage,0.9f,1);
		gameObject.transform.Find("Background").FindChild("LifeEnd").transform.localPosition = new Vector3(-0.26f+percentage*(0.74f),-0.41f,0);
	}
	
	public void addTime(float t){

		this.timerSelection += t ;
		if (this.timerSelection>this.selectionTime){
			if(!this.isGettingBigger && this.toStop){
				this.isRunning = false ;
				this.toStop = false ;
			}
			else{
				this.isGettingBigger = !this.isGettingBigger ;
			}
			this.timerSelection = 0f ;
		}
		else{
			if (this.isGettingBigger){
				float f = 1f + 0.2f * (this.timerSelection/this.selectionTime);
				gameObject.transform.localScale = new Vector3(f, f, f) ;
			}
			else {
				float f = 1.2f - 0.2f * (this.timerSelection/this.selectionTime);
				gameObject.transform.localScale = new Vector3(f, f, f) ;
			}
		}
	}
	
	public void moveForward(){
		Transform t = gameObject.transform;
		t.Find("Background").GetComponent<SpriteRenderer>().sortingOrder = 31 ;
		t.Find("Background").FindChild("Character").GetComponent<SpriteRenderer>().sortingOrder = 32 ;
		t.Find("Background").FindChild("Circle").GetComponent<SpriteRenderer>().sortingOrder = 33 ;
		t.Find("Background").FindChild("AttackPicto").GetComponent<SpriteRenderer>().sortingOrder = 34 ;
		t.Find("Background").FindChild("AttackValue").GetComponent<MeshRenderer>().sortingOrder = 40 ;
		t.Find("Background").FindChild("PVPicto").GetComponent<SpriteRenderer>().sortingOrder = 36 ;
		t.Find("Background").FindChild("PVValue").GetComponent<MeshRenderer>().sortingOrder = 40 ;
		t.Find("Background").FindChild("Icon1").GetComponent<SpriteRenderer>().sortingOrder = 34 ;
		t.Find("Background").FindChild("Icon1").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 35 ;
		t.Find("Background").FindChild("Icon2").GetComponent<SpriteRenderer>().sortingOrder = 34 ;
		t.Find("Background").FindChild("Icon2").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 35 ;
		t.Find("Background").FindChild("Icon3").GetComponent<SpriteRenderer>().sortingOrder = 34 ;
		t.Find("Background").FindChild("Icon3").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 35 ;
		t.Find("Background").FindChild("LifeBar").GetComponent<SpriteRenderer>().sortingOrder = 34 ;
		t.Find("Background").FindChild("Life").GetComponent<SpriteRenderer>().sortingOrder = 35 ;
		t.Find("Background").FindChild("LifeBarEnd").GetComponent<SpriteRenderer>().sortingOrder = 34 ;
		t.Find("Background").FindChild("LifeEnd").GetComponent<SpriteRenderer>().sortingOrder = 35 ;
		t.Find("Background").FindChild("DeadLayer").GetComponent<SpriteRenderer>().sortingOrder = 38 ;
	}
	
	public void moveBackward(){
		Transform t = gameObject.transform;
		t.Find("Background").GetComponent<SpriteRenderer>().sortingOrder = 21 ;
		t.Find("Background").FindChild("Character").GetComponent<SpriteRenderer>().sortingOrder = 22 ;
		t.Find("Background").FindChild("Circle").GetComponent<SpriteRenderer>().sortingOrder = 23 ;
		t.Find("Background").FindChild("AttackPicto").GetComponent<SpriteRenderer>().sortingOrder = 24 ;
		t.Find("Background").FindChild("AttackValue").GetComponent<MeshRenderer>().sortingOrder = 30 ;
		t.Find("Background").FindChild("PVPicto").GetComponent<SpriteRenderer>().sortingOrder = 26 ;
		t.Find("Background").FindChild("PVValue").GetComponent<MeshRenderer>().sortingOrder = 30 ;
		t.Find("Background").FindChild("Icon1").GetComponent<SpriteRenderer>().sortingOrder = 24 ;
		t.Find("Background").FindChild("Icon1").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 25 ;
		t.Find("Background").FindChild("Icon2").GetComponent<SpriteRenderer>().sortingOrder = 24 ;
		t.Find("Background").FindChild("Icon2").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 25 ;
		t.Find("Background").FindChild("Icon3").GetComponent<SpriteRenderer>().sortingOrder = 24 ;
		t.Find("Background").FindChild("Icon3").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 25 ;
		t.Find("Background").FindChild("LifeBar").GetComponent<SpriteRenderer>().sortingOrder = 24 ;
		t.Find("Background").FindChild("Life").GetComponent<SpriteRenderer>().sortingOrder = 25 ;
		t.Find("Background").FindChild("LifeBarEnd").GetComponent<SpriteRenderer>().sortingOrder = 24 ;
		t.Find("Background").FindChild("LifeEnd").GetComponent<SpriteRenderer>().sortingOrder = 25 ;
		t.Find("Background").FindChild("DeadLayer").GetComponent<SpriteRenderer>().sortingOrder = 28 ;
	}
	
	public Tile getTile()
	{
		return this.tile ;
	}
	
	public bool getIsMine()
	{
		return this.card.isMine ;
	}
	
	public GameCard getCard()
	{
		return this.card ;
	}

	public void setActive(bool b)
	{
		gameObject.SetActive(b);
	}
	
	public void addDamagesModifyer(Modifyer m, bool endTurn){
		this.updateLife(this.card.getLife());
		if(m.amount<0){
			m.amount = Mathf.Min(m.amount, this.card.GetTotalLife()-this.card.getLife()) ;
		}
		else{
			if (this.card.getLife()-m.amount<=0){
				this.kill(endTurn);
			}
			else{
	
			}
		}
		this.card.damagesModifyers.Add(m);
	}

	public void showIcons(){
		if(!this.isHidden){
			int compteurIcones = 1;

			if(this.card.isPoisoned()){
				gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
				gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true;
				gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.iconeSprites[4];
				compteurIcones++;
			}
			if(this.card.isFurious()){
				gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
				gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true;
				gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.iconeSprites[2];
				compteurIcones++;
			}
			if(this.card.isEffraye()){
				gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
				gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true;
				gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.iconeSprites[3];
				compteurIcones++;
			}
			if(this.card.getBouclier()>0){
				if(compteurIcones>3){
					gameObject.transform.Find("Background").FindChild("Icon3").GetComponent<SpriteRenderer>().enabled = true;
					gameObject.transform.Find("Background").FindChild("Icon3").FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true;
					gameObject.transform.Find("Background").FindChild("Icon3").FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.iconeSprites[5];
				}
				else{
					gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
					gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true;
					gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.iconeSprites[1];
				}
				compteurIcones++;
			}
			if(this.card.getEsquive()>0){
				if(compteurIcones>3){
					gameObject.transform.Find("Background").FindChild("Icon3").GetComponent<SpriteRenderer>().enabled = true;
					gameObject.transform.Find("Background").FindChild("Icon3").FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true;
					gameObject.transform.Find("Background").FindChild("Icon3").FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.iconeSprites[5];
				}
				else{
					gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
					gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true;
					gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.iconeSprites[6];
				}
				compteurIcones++;
			}
			if(this.card.getMove()!=this.card.Move){
				if(compteurIcones>3){
					gameObject.transform.Find("Background").FindChild("Icon3").GetComponent<SpriteRenderer>().enabled = true;
					gameObject.transform.Find("Background").FindChild("Icon3").FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true;
					gameObject.transform.Find("Background").FindChild("Icon3").FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.iconeSprites[5];
				}
				else{
					gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
					gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true;
					gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.iconeSprites[0];
				}
				compteurIcones++;
			}


			for(int j = compteurIcones ; j < 4 ; j++){
				gameObject.transform.Find("Background").FindChild("Icon"+j).GetComponent<SpriteRenderer>().enabled = false;
				gameObject.transform.Find("Background").FindChild("Icon"+j).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = false;
			}
		}
	}
	
	public void show()
	{
		if(!this.isHidden){
			this.updateAttack();
			this.showIcons();
		}
	}

	public void setTile(Tile t, Vector3 p)
	{
		this.tile = t;
		p.z = -0.5f;
		gameObject.transform.localPosition = new Vector3(p.x, p.y, p.z);
	}

	public void setPosition(Vector3 p){
		p.z = -0.5f;
		gameObject.transform.localPosition = new Vector3(p.x, p.y, p.z);
	}
	
	public void changeTile(Tile t, Vector3 p)
	{
		this.tile = t;
		p.z = -0.5f;
		this.initialP = gameObject.transform.localPosition;
		this.finalP = p;
		this.isMoving = true;
		this.timerMove=0f;
	}
	
	public void addMoveTime(float t){
		this.timerMove += t ;
		if (this.timerMove>this.MoveTime){
			this.isMoving = false ;
			gameObject.transform.localPosition = this.finalP;
			if(GameView.instance.getCard(this.id).isMine){
				GameView.instance.recalculateDestinations();
			}
			StartCoroutine(GameView.instance.checkDestination(this.id));
			if(GameView.instance.hasFightStarted){
				GameView.instance.updateActionStatus();
			}
			this.moveBackward();
		}
		else{
			float rapport = this.timerMove/this.MoveTime;
			gameObject.transform.localPosition = new Vector3(this.initialP.x+(this.finalP.x-this.initialP.x)*rapport, this.initialP.y+(this.finalP.y-this.initialP.y)*rapport, -0.5f);
		}
	}

	public void addLifeTime(float t){
		this.timerLife += t ;

		if (this.timerLife>this.lifeTime){
			this.isUpdatingLife = false ;
			this.timerLife = this.lifeTime ;
		}

		float l = (this.actualLife-(1.0f*this.timerLife/lifeTime)*(this.actualLife-this.card.getLife()))/this.card.GetTotalLife();
		this.updateLifePercentage(l);
		int actualNumber = System.Convert.ToInt32(transform.Find("Background").FindChild("PVValue").GetComponent<TextMeshPro>().text);
		int nextNumber = Mathf.RoundToInt(this.card.GetTotalLife()*l);
		if(actualNumber!=nextNumber){
			if(nextNumber<10){
				transform.Find("Background").FindChild("PVValue").GetComponent<TextMeshPro>().text = "0"+nextNumber;
			}
			else{
				transform.Find("Background").FindChild("PVValue").GetComponent<TextMeshPro>().text = ""+nextNumber;
			}

			if(this.card.GetTotalLife()>nextNumber){
				transform.Find("Background").FindChild("PVValue").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
			}
			else if(this.card.Life<nextNumber){
				transform.Find("Background").FindChild("PVValue").GetComponent<TextMeshPro>().color = new Color(60f/255f, 160f/255f, 100f/255f, 1f);
			}
			else{
				transform.Find("Background").FindChild("PVValue").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
			}
		}	
	}
	
	public void addDeadTime(float t){
		this.timerDead += t ;
		if (this.timerDead>this.deadTime){
			this.displayDead(false);
			this.moveToDeadZone();
			GameView.instance.emptyTile(this.id);
			GameView.instance.recalculateDestinations();
			GameView.instance.removeDead(this.id);
			if(GameView.instance.areAllMyPlayersDead()){
				StartCoroutine(GameView.instance.quitGame());
			}
		}
	}

	public void updateAttack()
	{
		if(!this.isHidden){
			int attackBase = this.card.Attack ;
			int attack = this.card.getAttack();
			if(attackBase>attack){
				gameObject.transform.FindChild("Background").FindChild("AttackValue").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
			}
			else if(attackBase<attack){
				gameObject.transform.FindChild("Background").FindChild("AttackValue").GetComponent<TextMeshPro>().color = new Color(60f/255f, 160f/255f, 100f/255f, 1f);
			}
			else{
				gameObject.transform.FindChild("Background").FindChild("AttackValue").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
			}
			gameObject.transform.FindChild("Background").FindChild("AttackValue").GetComponent<TextMeshPro>().text = this.card.GetAttackString();
		}
	}
	
	public void kill(bool endTurn)
	{
		this.card.isDead = true;
		
		if(this.card.isLeader()){
			GameView.instance.removeLeaderEffect(this.id, this.card.isMine);
		}
				
		GameView.instance.killHandle (this.id, endTurn);
	}
	
	public void moveToDeadZone(){
		gameObject.transform.localPosition = new Vector3(0f, -20f, 0f);
	}
	
	public void checkModyfiers()
	{
		this.card.checkModifyers();
		this.show();
	}
	
	public bool canBeTargeted()
	{
		return (!this.card.isDead);
	}
	
	public void showDisplay()
	{
		gameObject.GetComponent<Renderer>().enabled = true ;
	}
	
	public void setIsDisable(bool value)
	{
		this.isDisabled = value;
	}
	
	public void changeScale(float f)
	{
		gameObject.transform.localScale = new Vector3(f,f,f);
	}
	
	public void displayDead(bool b){
		gameObject.transform.FindChild("Background").FindChild("DeadLayer").GetComponent<SpriteRenderer>().enabled = b ;
		this.timerDead = 0f;
		this.isShowingDead = b;
	}

	public void checkPaladin(bool toDisplay){
		if((card.Skills[0].Id == 73)){
			int level = card.Skills[0].Power;
			int bonusAttack = Mathf.RoundToInt((20f+level*10f)*card.getAttack()/100f);
			int bonusMove = -1*Mathf.Min(card.getMove()-1,2);

			this.card.attackModifyers.Add(new Modifyer(bonusAttack, -1, 73, card.Skills[0].Name, "+"+bonusAttack+" ATK. Permanent"));
			GameView.instance.getPlayingCardController(this.id).updateAttack();
			this.card.moveModifyers.Add(new Modifyer(bonusMove, -1, 73, card.Skills[0].Name, bonusMove+"MOV. Permanent"));
			GameView.instance.getPlayingCardController(this.id).showIcons();

			if(toDisplay){
				GameView.instance.displaySkillEffect(this.id, "Paladin\n+"+bonusAttack+" ATK\n"+bonusMove+"MOV", 1);
				GameView.instance.addAnim(GameView.instance.getTile(this.id), 73);
			}
		}
	}

	public void checkAgile(bool toDisplay){
		if((card.Skills[0].Id == 66)){
			int level = 20+card.Skills[0].Power*3;
						
			this.card.esquiveModifyers.Add(new Modifyer(level, -1, 66, "Agile", "Esquive au contact:"+level+"%"));
			GameView.instance.getPlayingCardController(this.id).showIcons();

			if(toDisplay){
				GameView.instance.displaySkillEffect(this.id, "Agile\nEsquive : "+level+"%", 1);
				GameView.instance.addAnim(GameView.instance.getTile(this.id), 66);
			}
		}
	}

	public void checkAguerri(bool toDisplay){
		if((card.Skills[0].Id == 68)){
			int bonusAttack = 5+card.Skills[0].Power;
			this.card.attackModifyers.Add (new Modifyer(bonusAttack, -1, 68, "Psycho", "+"+bonusAttack+"ATK. Permanent"));
			GameView.instance.getPlayingCardController(this.id).updateAttack();
			if(toDisplay){
				GameView.instance.displaySkillEffect(this.id, "+"+bonusAttack+"ATK", 1);
				GameView.instance.addAnim(GameView.instance.getTile(this.id), 68);
			}
		}
	}

	public void checkCuirasse(bool toDisplay){
		if((card.Skills[0].Id == 70)){
			int bonusShield = card.Skills[0].Power*3+20;
			GameView.instance.getCard(this.id).addShieldModifyer(new Modifyer(bonusShield, -1, 70, "Cuirassé", "Bouclier "+bonusShield+"%"));
			GameView.instance.displaySkillEffect(this.id, "Bouclier "+bonusShield+"%", 1);
			if(toDisplay){
				GameView.instance.getPlayingCardController(this.id).showIcons();
				GameView.instance.addAnim(GameView.instance.getTile(this.id), 70);
			}
		}
	}

	public void checkRapide(bool toDisplay){
		if((card.Skills[0].Id == 71)){
			int level = 10-card.Skills[0].Power;
		
			GameView.instance.getCard(this.id).moveModifyers.Add(new Modifyer(1, -1, 71, "Rapide", "+1MOV. Permanent"));
			GameView.instance.getPlayingCardController(this.id).showIcons();
			GameView.instance.getCard(this.id).attackModifyers.Add(new Modifyer(-1*level, -1, 71, "Rapide", "-"+level+"ATK. Permanent"));
			GameView.instance.getPlayingCardController(this.id).updateAttack();
			if(toDisplay){
				GameView.instance.displaySkillEffect(this.id, "+1MOV\n-"+level+"ATK", 1);	
				GameView.instance.addAnim(GameView.instance.getTile(this.id), 71);
			}
		}
	}

	public void checkEmbusque(bool toDisplay){
		if((card.Skills[0].Id == 32)){
			int level = 5*card.Skills[0].Power;
		
			this.card.esquiveModifyers.Add(new Modifyer(level, -1, 32, "Embusqué", "Esquive à distance:"+level+"%"));
			GameView.instance.getPlayingCardController(this.id).showIcons();

			if(toDisplay){
				GameView.instance.displaySkillEffect(this.id, "Embusqué\nEsquive : "+level+"%", 1);
				GameView.instance.addAnim(GameView.instance.getTile(this.id), 32);
			}
		}
	}

	public void checkSniper(bool toDisplay){
		if((card.Skills[0].Id == 35)){
			int bonus = 5*card.Skills[0].Power;
		
			GameView.instance.getCard(this.id).moveModifyers.Add(new Modifyer(-1, -1, 35, "Sniper", "Immobile. Permanent"));
			GameView.instance.getPlayingCardController(this.id).updateAttack();

			if(toDisplay){
				GameView.instance.displaySkillEffect(this.id, "Dégats -"+bonus+"%", 1);
				GameView.instance.addAnim(GameView.instance.getTile(this.id), 35);
			}
		}
	}

	public void checkPassiveSkills(bool toDisplay)
	{
		this.checkPaladin(toDisplay);
		this.checkAguerri(toDisplay);
		this.checkRapide(toDisplay);
		this.checkAgile(toDisplay);
		this.checkCuirasse(toDisplay);
		this.checkEmbusque(toDisplay);
		this.checkSniper(toDisplay);
	}
}


