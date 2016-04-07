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
	public bool isUpdatingAttack = true ;
	int actualAttack;
	float timerAttack = 0f ; 
	float attackTime = 1f ;
	public bool canBeDragged ;
	public int nbTurns ;
	
	void Awake()
	{
		this.isDisabled = false;
		this.displayDead(false);
		Transform t = gameObject.transform;
		this.toStop = false ;
		this.isRunning = false;
		this.destinations = new List<Tile>();
		this.nbTurns = 0 ;
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

	public Sprite getBackgroundSprite(int i){
		return this.backgroundSprites[i];
	}
	
	public void hide()
	{
		this.isHidden = true ;
		Transform t = gameObject.transform;
		t.FindChild("Background").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("Circle").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("Character").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("Icon1").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("Icon2").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("Icon3").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("AttackValue").GetComponent<MeshRenderer>().enabled = false ;
		t.Find("Background").FindChild("PVValue").GetComponent<MeshRenderer>().enabled = false ;
	}
	
	public void display()
	{
		this.isHidden = false ;
		Transform t = gameObject.transform;
		t.FindChild("Background").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("Circle").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("Character").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("Icon1").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("Icon2").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("Icon3").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("AttackValue").GetComponent<MeshRenderer>().enabled = true ;
		t.Find("Background").FindChild("PVValue").GetComponent<MeshRenderer>().enabled = true ;
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

		}
		else {
			gameObject.transform.Find("Background").FindChild("Character").transform.localScale = new Vector3(1,1,1);
			gameObject.transform.Find("Background").FindChild("Circle").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[1];
		}

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
		this.updateAttack(0);
	}
	
	public void updateLife(int value){
		this.timerLife = 0f ;
		this.actualLife = value;
		this.isUpdatingLife = true ;
	}

	public void updateAttack(int value){
		this.timerAttack = 0f ;
		this.actualAttack = value;
		this.isUpdatingAttack = true ;
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
		t.Find("Background").GetComponent<SpriteRenderer>().sortingOrder = 51 ;
		t.Find("Background").FindChild("Character").GetComponent<SpriteRenderer>().sortingOrder = 52 ;
		t.Find("Background").FindChild("Circle").GetComponent<SpriteRenderer>().sortingOrder = 53 ;
		t.Find("Background").FindChild("AttackValue").GetComponent<MeshRenderer>().sortingOrder = 59 ;
		t.Find("Background").FindChild("PVValue").GetComponent<MeshRenderer>().sortingOrder = 59 ;
		t.Find("Background").FindChild("Icon1").GetComponent<SpriteRenderer>().sortingOrder = 54 ;
		t.Find("Background").FindChild("Icon2").GetComponent<SpriteRenderer>().sortingOrder = 54 ;
		t.Find("Background").FindChild("Icon3").GetComponent<SpriteRenderer>().sortingOrder = 56 ;
		t.Find("Background").FindChild("DeadLayer").GetComponent<SpriteRenderer>().sortingOrder = 58 ;
	}
	
	public void moveBackward(){
		Transform t = gameObject.transform;
		t.Find("Background").GetComponent<SpriteRenderer>().sortingOrder = 21 ;
		t.Find("Background").FindChild("Character").GetComponent<SpriteRenderer>().sortingOrder = 22 ;
		t.Find("Background").FindChild("Circle").GetComponent<SpriteRenderer>().sortingOrder = 23 ;
		t.Find("Background").FindChild("AttackValue").GetComponent<MeshRenderer>().sortingOrder = 30 ;
		t.Find("Background").FindChild("PVValue").GetComponent<MeshRenderer>().sortingOrder = 30 ;
		t.Find("Background").FindChild("Icon1").GetComponent<SpriteRenderer>().sortingOrder = 24 ;
		t.Find("Background").FindChild("Icon2").GetComponent<SpriteRenderer>().sortingOrder = 24 ;
		t.Find("Background").FindChild("Icon3").GetComponent<SpriteRenderer>().sortingOrder = 26 ;
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
			m.amount = -1*Mathf.Min(-1*m.amount, this.card.GetTotalLife()-this.card.getLife()) ;
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

	public void addAttackModifyer(Modifyer m){
		bool hasFound = false ;
		for(int i = 0 ; i < this.card.attackModifyers.Count && !hasFound ; i++){
			if(m.type==this.card.attackModifyers[i].type){
				this.card.attackModifyers[i].amount += m.amount;
				hasFound = true ;
			}
		}
		if(!hasFound){
			this.card.attackModifyers.Add(m);
		}
		GameView.instance.getMyHoveredCardController().updateCharacter();
		GameView.instance.getHisHoveredCardController().updateCharacter();
	}

	public void addBonusModifyer(Modifyer m){
		bool hasFound = false ;
		for(int i = 0 ; i < this.card.bonusModifyers.Count && !hasFound ; i++){
			if(m.type==this.card.bonusModifyers[i].type && m.targetType==this.card.bonusModifyers[i].targetType){
				this.card.bonusModifyers[i].amount += m.amount;
				hasFound = true ;
			}
		}
		if(!hasFound){
			this.card.bonusModifyers.Add(m);
		}
		GameView.instance.getMyHoveredCardController().updateCharacter();
		GameView.instance.getHisHoveredCardController().updateCharacter();
	}

	public void addPVModifyer(Modifyer m){
		bool hasFound = false ;
		for(int i = 0 ; i < this.card.pvModifyers.Count && !hasFound ; i++){
			if(m.type==this.card.pvModifyers[i].type){
				this.card.pvModifyers[i].amount += m.amount;
				hasFound = true ;
			}
		}
		if(!hasFound){
			this.card.pvModifyers.Add(m);
		}
		GameView.instance.getMyHoveredCardController().updateCharacter();
		GameView.instance.getHisHoveredCardController().updateCharacter();
	}

	public void addMagicalEsquiveModifyer(Modifyer m){
		bool hasFound = false ;
		for(int i = 0 ; i < this.card.magicalEsquiveModifyers.Count && !hasFound ; i++){
			if(m.type==this.card.magicalEsquiveModifyers[i].type){
				this.card.magicalEsquiveModifyers[i].amount += m.amount;
				hasFound = true ;
			}
		}
		if(!hasFound){
			this.card.magicalEsquiveModifyers.Add(m);
		}
		GameView.instance.getMyHoveredCardController().updateCharacter();
		GameView.instance.getHisHoveredCardController().updateCharacter();
	}

	public void addMoveModifyer(Modifyer m){
		bool hasFound = false ;
		for(int i = 0 ; i < this.card.moveModifyers.Count && !hasFound ; i++){
			if(m.type==this.card.moveModifyers[i].type){
				this.card.moveModifyers[i].amount += m.amount;
				hasFound = true ;
			}
		}
		if(!hasFound){
			this.card.moveModifyers.Add(m);
		}
		GameView.instance.getMyHoveredCardController().updateCharacter();
		GameView.instance.getHisHoveredCardController().updateCharacter();
	}

	public void addShieldModifyer(Modifyer m){
		bool hasFound = false ;
		for(int i = 0 ; i < this.card.bouclierModifyers.Count && !hasFound ; i++){
			if(m.type==this.card.bouclierModifyers[i].type){
				this.card.bouclierModifyers[i].amount += m.amount;
				hasFound = true ;
			}
		}
		if(!hasFound){
			this.card.bouclierModifyers.Add(m);
		}
		GameView.instance.getMyHoveredCardController().updateCharacter();
		GameView.instance.getHisHoveredCardController().updateCharacter();
	}

	public void addEsquiveModifyer(Modifyer m){
		bool hasFound = false ;
		for(int i = 0 ; i < this.card.esquiveModifyers.Count && !hasFound ; i++){
			if(m.type==this.card.esquiveModifyers[i].type){
				this.card.esquiveModifyers[i].amount += m.amount;
				hasFound = true ;
			}
		}
		if(!hasFound){
			this.card.esquiveModifyers.Add(m);
		}
		GameView.instance.getMyHoveredCardController().updateCharacter();
		GameView.instance.getHisHoveredCardController().updateCharacter();
	}

	public void showIcons(){
		if(!this.isHidden){
			int compteurIcones = 1;

			for(int j = 0 ; j < this.card.states.Count ; j++){
				if(compteurIcones<4){
					gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().sprite = GameView.instance.getSkillSprite(this.card.states[j].type);
					gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().color = new Color(255f/255f, 255f/255f, 255f/255f, 1f);
					
					gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
					compteurIcones++;
				}
			}

			for(int j = 0 ; j < this.card.esquiveModifyers.Count ; j++){
				if(compteurIcones<4){
					if(this.card.esquiveModifyers[j].amount==0){

					}
					else{
						gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().sprite = GameView.instance.getSkillSprite(this.card.esquiveModifyers[j].type);
						if(this.card.esquiveModifyers[j].amount>0){
							gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().color = new Color(60f/255f, 160f/255f, 100f/255f, 1f);
						}
						else{
							gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
						}
						gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
						compteurIcones++;
					}
				}
			}

			for(int j = 0 ; j < this.card.magicalEsquiveModifyers.Count ; j++){
				if(compteurIcones<4){
					if(this.card.magicalEsquiveModifyers[j].amount==0){

					}
					else{
						gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().sprite = GameView.instance.getSkillSprite(this.card.magicalEsquiveModifyers[j].type);
						if(this.card.magicalEsquiveModifyers[j].amount>0){
							gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().color = new Color(60f/255f, 160f/255f, 100f/255f, 1f);
						}
						else{
							gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
						}
						gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
						compteurIcones++;
					}
				}
			}

			for(int j = 0 ; j < this.card.bouclierModifyers.Count ; j++){
				if(compteurIcones<4){
					if(this.card.bouclierModifyers[j].amount==0){

					}
					else{
						gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().sprite = GameView.instance.getSkillSprite(this.card.bouclierModifyers[j].type);
						if(this.card.bouclierModifyers[j].amount>0){
							gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().color = new Color(60f/255f, 160f/255f, 100f/255f, 1f);
						}
						else{
							gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
						}
						gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
						compteurIcones++;
					}
				}
			}

			for(int j = 0 ; j < this.card.bonusModifyers.Count ; j++){
				if(compteurIcones<4){
					if(this.card.bonusModifyers[j].amount==0){

					}
					else{
						gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().sprite = GameView.instance.getSkillSprite(this.card.bonusModifyers[j].type);
						if(this.card.bonusModifyers[j].amount>0){
							gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().color = new Color(60f/255f, 160f/255f, 100f/255f, 1f);
						}
						else{
							gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
						}
						gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
						compteurIcones++;
					}
				}
			}

			for(int j = 0 ; j < this.card.moveModifyers.Count ; j++){
				if(compteurIcones<4){
					if(this.card.moveModifyers[j].amount==0){

					}
					else{
						gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().sprite = GameView.instance.getSkillSprite(this.card.moveModifyers[j].type);
						if(this.card.moveModifyers[j].amount>0){
							gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().color = new Color(60f/255f, 160f/255f, 100f/255f, 1f);
						}
						else{
							gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
						}
						gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
						compteurIcones++;
					}
				}
			}

			for(int j = compteurIcones ; j < 4 ; j++){
				gameObject.transform.Find("Background").FindChild("Icon"+j).GetComponent<SpriteRenderer>().enabled = false;
			}
		}
	}
	
	public void show()
	{
		if(!this.isHidden){
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
		int actualNumber = System.Convert.ToInt32(transform.Find("Background").FindChild("PVValue").GetComponent<TextMeshPro>().text);
		int nextNumber = Mathf.RoundToInt(this.card.GetTotalLife()*l);
		if(actualNumber!=nextNumber){
			if(nextNumber<10){
				transform.Find("Background").FindChild("PVValue").GetComponent<TextMeshPro>().text = "0"+nextNumber;
			}
			else{
				transform.Find("Background").FindChild("PVValue").GetComponent<TextMeshPro>().text = ""+nextNumber;
			}

			if((this.card.GetTotalLife()/4)>nextNumber){
				transform.Find("Background").FindChild("PVValue").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
			}
			else if((this.card.GetTotalLife()/2)>nextNumber){
				transform.Find("Background").FindChild("PVValue").GetComponent<TextMeshPro>().color = new Color(243f/255f, 110f/255f, 42f/255f, 1f);
			}
			else if((this.card.GetTotalLife())>nextNumber){
				transform.Find("Background").FindChild("PVValue").GetComponent<TextMeshPro>().color = new Color(255f/255f, 220f/255f, 20f/255f, 1f);
			}
			else if(this.card.Life<nextNumber){
				transform.Find("Background").FindChild("PVValue").GetComponent<TextMeshPro>().color = new Color(60f/255f, 160f/255f, 100f/255f, 1f);
			}
			else{
				transform.Find("Background").FindChild("PVValue").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
			}
		}	
	}

	public void addAttackTime(float t){
		this.timerAttack += t ;

		if (this.timerAttack>this.attackTime){
			this.isUpdatingAttack = false ;
			this.timerAttack = this.attackTime ;
		}

		float l = (this.actualAttack-(1.0f*this.timerAttack/attackTime)*(this.actualAttack-this.card.getAttack()))/this.card.Attack;
		int actualNumber = System.Convert.ToInt32(transform.Find("Background").FindChild("PVValue").GetComponent<TextMeshPro>().text);
		int nextNumber = Mathf.RoundToInt(this.card.Attack*l);
		if(actualNumber!=nextNumber){
			if(nextNumber<10){
				transform.Find("Background").FindChild("AttackValue").GetComponent<TextMeshPro>().text = "0"+nextNumber;
			}
			else{
				transform.Find("Background").FindChild("AttackValue").GetComponent<TextMeshPro>().text = ""+nextNumber;
			}

			if((this.card.Attack/4)>nextNumber){
				transform.Find("Background").FindChild("AttackValue").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
			}
			else if((this.card.Attack/2)>nextNumber){
				transform.Find("Background").FindChild("AttackValue").GetComponent<TextMeshPro>().color = new Color(243f/255f, 110f/255f, 42f/255f, 1f);
			}
			else if(this.card.Attack>nextNumber){
				transform.Find("Background").FindChild("AttackValue").GetComponent<TextMeshPro>().color = new Color(255f/255f, 220f/255f, 20f/255f, 1f);
			}
			else if(this.card.Attack<nextNumber){
				transform.Find("Background").FindChild("AttackValue").GetComponent<TextMeshPro>().color = new Color(60f/255f, 160f/255f, 100f/255f, 1f);
			}
			else{
				transform.Find("Background").FindChild("AttackValue").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
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
			int bonusMove = -1*Mathf.Min(card.getMove()-1,1);

			this.addAttackModifyer(new Modifyer(bonusAttack, -1, 73, WordingSkills.getName(card.Skills[0].Id), ". Permanent"));
			this.addMoveModifyer(new Modifyer(bonusMove, -1, 73, WordingSkills.getName(card.Skills[0].Id), ". Permanent"));
			GameView.instance.getPlayingCardController(this.id).showIcons();

			if(toDisplay && !ApplicationModel.player.ToLaunchGameTutorial){
				GameView.instance.displaySkillEffect(this.id, "Paladin\n+"+bonusAttack+" ATK\n"+bonusMove+"MOV", 1);
				GameView.instance.addAnim(GameView.instance.getTile(this.id), 73);
			}
		}
	}

	public void checkAgile(bool toDisplay){
		if((card.Skills[0].Id == 66)){
			int level = 20+card.Skills[0].Power*4;
						
			this.addEsquiveModifyer(new Modifyer(level, -1, 66, "Agile", ". Permanent"));
			GameView.instance.getPlayingCardController(this.id).showIcons();

			if(toDisplay && !ApplicationModel.player.ToLaunchGameTutorial){
				GameView.instance.displaySkillEffect(this.id, "Agile\nEsquive : "+level+"%", 1);
				GameView.instance.addAnim(GameView.instance.getTile(this.id), 66);
			}
		}
	}

	public void checkAguerri(bool toDisplay){
		if((card.Skills[0].Id == 68)){
			GameView.instance.getPlayingCardController(this.id).addMoveModifyer(new Modifyer(-1, -1, 71, "Costaud", ". Permanent."));
			GameView.instance.getPlayingCardController(this.id).showIcons();
			int bonusAttack = 2+card.Skills[0].Power;
			this.addAttackModifyer (new Modifyer(bonusAttack, -1, 68, "Costaud", ". Permanent."));
			if(toDisplay && !ApplicationModel.player.ToLaunchGameTutorial){
				GameView.instance.displaySkillEffect(this.id, "+"+bonusAttack+"ATK", 2);
				GameView.instance.addAnim(GameView.instance.getTile(this.id), 68);
			}
		}
	}

	public void checkCuirasse(bool toDisplay){
		if((card.Skills[0].Id == 70)){
			int bonusShield = card.Skills[0].Power*3+20;
			GameView.instance.getPlayingCardController(this.id).addShieldModifyer(new Modifyer(bonusShield, -1, 70, "Cuirassé", ". Permanent."));
			GameView.instance.displaySkillEffect(this.id, "Cuirassé\nBouclier "+bonusShield+"%", 2);
			if(toDisplay && !ApplicationModel.player.ToLaunchGameTutorial){
				GameView.instance.getPlayingCardController(this.id).showIcons();
				GameView.instance.addAnim(GameView.instance.getTile(this.id), 70);
			}
		}
	}

	public void checkRapide(bool toDisplay){
		if((card.Skills[0].Id == 71)){
			int level = 11-card.Skills[0].Power;
		
			GameView.instance.getPlayingCardController(this.id).addMoveModifyer(new Modifyer(1, -1, 71, "Rapide", ". Permanent."));
			GameView.instance.getPlayingCardController(this.id).showIcons();

			GameView.instance.getPlayingCardController(this.id).addAttackModifyer(new Modifyer(-1*level, -1, 71, "Rapide", ". Permanent."));
			if(toDisplay && !ApplicationModel.player.ToLaunchGameTutorial){
				GameView.instance.displaySkillEffect(this.id, "Rapide\n+1MOV\n-"+level+"ATK", 1);	
				GameView.instance.addAnim(GameView.instance.getTile(this.id), 71);
			}
		}
	}

	public void checkEmbusque(bool toDisplay){
		if((card.Skills[0].Id == 32)){
			int level = 20+4*card.Skills[0].Power;
		
			this.addMagicalEsquiveModifyer(new Modifyer(level, -1, 32, "Embusqué", "Esquive à distance:"+level+"%"));
			GameView.instance.getPlayingCardController(this.id).showIcons();

			if(toDisplay && !ApplicationModel.player.ToLaunchGameTutorial){
				GameView.instance.displaySkillEffect(this.id, "Embusqué\nEsquive : "+level+"%", 2);
				GameView.instance.addAnim(GameView.instance.getTile(this.id), 32);
			}
		}
	}

	public void checkSniper(bool toDisplay){
		if((card.Skills[0].Id == 35)){
			int bonus = 5*card.Skills[0].Power;
		
			GameView.instance.getCard(this.id).states.Add(new Modifyer(0, -1, 35, "Sniper", "Immobile. Permanent."));
			GameView.instance.getPlayingCardController(this.id).showIcons();

			if(toDisplay && !ApplicationModel.player.ToLaunchGameTutorial){
				GameView.instance.displaySkillEffect(this.id, "Sniper\nrésistance météorite "+bonus+"%\nimmobile", 1);
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


