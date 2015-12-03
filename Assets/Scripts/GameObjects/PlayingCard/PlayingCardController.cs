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
	public Sprite[] skillEffectSprites;
	
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
	float timerSE;
	float deadTime = 5f;
	float SETime = 0.5f;
	public bool isShowingDead;
	public List<Tile> destinations ;
	string skillEffectDescription ;
	bool isDisplayingSkillEffect;
	bool isShowingSE ;

	void Awake()
	{
		this.isDisabled = false;
		
		this.showDescriptionAttack(false);
		this.showDescriptionLife(false);
		this.showDescriptionTurns(false);
		this.showDescriptionIcon(1, false);
		this.showDescriptionIcon(2, false);
		this.showDescriptionIcon(3, false);
		
		this.showHover(false);
		this.showEffect(false);
		this.displayDead(false);
		
		Transform t = gameObject.transform;
		this.toStop = false ;
		this.isRunning = false;
		this.isDisplayingSkillEffect=false;
	}
	
	public bool getIsDisplayingSkillEffect(){
		return this.isDisplayingSkillEffect;
	}
	
	public void stopAnim(){
		this.toStop = true ;
	}
	
	public void run(){
		this.isRunning = true ;
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
	
	public void showHover(bool b){
		gameObject.transform.FindChild("HoverLayer").GetComponent<SpriteRenderer>().enabled = b ;
	}
	
	public void showEffect(bool b){
		gameObject.transform.FindChild("SkillEffect").GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("SkillEffect").FindChild("Text").GetComponent<MeshRenderer>().enabled = b ;
	}
	
	public void hide()
	{
		this.isHidden = true ;
		Transform t = gameObject.transform;
		t.FindChild("Art").GetComponent<SpriteRenderer>().enabled = false ;
		
		t.Find("AttackZone").FindChild("AttackPicto").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("AttackZone").GetComponent<BoxCollider>().enabled = false ;
		t.Find("AttackZone").FindChild("AttackValue").GetComponent<MeshRenderer>().enabled = false ;
		t.FindChild("Life").GetComponent<SpriteRenderer>().enabled = false ;
		t.FindChild("LifeBar").GetComponent<SpriteRenderer>().enabled = false ;
		t.FindChild("LifeBar").FindChild("PV").GetComponent<MeshRenderer>().enabled = false ;
		t.FindChild("LifeBar").FindChild("PVValue").GetComponent<MeshRenderer>().enabled = false ;
		t.FindChild("Icon1").GetComponent<SpriteRenderer>().enabled = false ;
		t.FindChild("Icon1").FindChild("Picto").GetComponent<SpriteRenderer>().enabled = false ;
		t.FindChild("Icon2").GetComponent<SpriteRenderer>().enabled = false ;
		t.FindChild("Icon2").FindChild("Picto").GetComponent<SpriteRenderer>().enabled = false ;
		t.FindChild("Icon3").GetComponent<SpriteRenderer>().enabled = false ;
		t.FindChild("Icon3").FindChild("Picto").GetComponent<SpriteRenderer>().enabled = false ;
		
		t.FindChild("PictoTR").GetComponent<SpriteRenderer>().enabled = false ;
		t.FindChild("WaitTime").GetComponent<MeshRenderer>().enabled = false ;
		
		t.FindChild("PictoTR").GetComponent<BoxCollider>().enabled = false ;
		t.FindChild("AttackZone").GetComponent<BoxCollider>().enabled = false ;
		t.FindChild("LifeBar").FindChild("PV").GetComponent<BoxCollider>().enabled = false ;
	}
	
	public void showTR(bool b)
	{
		if(b){
			this.updateTR();
		}
		Transform t = gameObject.transform;
		t.FindChild("WaitTime").GetComponent<TextMeshPro>().text = ""+this.card.getNbTurnsToWait();
		t.FindChild("WaitTime").GetComponent<MeshRenderer>().enabled = false ;
		t.FindChild("PictoTR").GetComponent<SpriteRenderer>().enabled = false ;
		t.FindChild("PictoTR").GetComponent<BoxCollider>().enabled = false ;
	}
	
	public void display()
	{
		this.isHidden = false ;
		Transform t = gameObject.transform;
		t.FindChild("Art").GetComponent<SpriteRenderer>().enabled = true ;
		
		t.Find("AttackZone").FindChild("AttackPicto").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("AttackZone").GetComponent<BoxCollider>().enabled = true ;
		t.Find("AttackZone").FindChild("AttackValue").GetComponent<MeshRenderer>().enabled = true ;
		t.FindChild("Life").GetComponent<SpriteRenderer>().enabled = true ;
		t.FindChild("LifeBar").GetComponent<SpriteRenderer>().enabled = true ;
		t.FindChild("LifeBar").FindChild("PV").GetComponent<MeshRenderer>().enabled = true ;
		t.FindChild("LifeBar").FindChild("PVValue").GetComponent<MeshRenderer>().enabled = true ;
		t.FindChild("Icon1").GetComponent<SpriteRenderer>().enabled = true ;
		t.FindChild("Icon1").FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true ;
		t.FindChild("Icon2").GetComponent<SpriteRenderer>().enabled = true ;
		t.FindChild("Icon2").FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true ;
		t.FindChild("Icon3").GetComponent<SpriteRenderer>().enabled = true ;
		t.FindChild("Icon3").FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true ;
		t.FindChild("AttackZone").GetComponent<BoxCollider>().enabled = true ;
		t.FindChild("LifeBar").FindChild("PV").GetComponent<BoxCollider>().enabled = true ;
	}
	
	public void setCard(GameCard c, bool b, int i)
	{
		this.card = c ;
		if (c.isMine){
			transform.Find("Art").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[this.card.IdClass];
		}
		else {
			transform.Find("Art").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[this.card.IdClass+10];
		}
		
		transform.Find("AttackZone").FindChild("AttackValue").GetComponent<TextMeshPro>().text = c.GetAttackString();
		transform.Find("LifeBar").GetComponent<SpriteRenderer>().sprite = this.lifebarSprites[0];
		transform.FindChild("LifeBar").FindChild("PVValue").GetComponent<TextMeshPro>().text = c.getLife()+"/"+c.Life;
		
		transform.Find("Life").GetComponent<SpriteRenderer>().sprite = this.lifebarSprites[1];
		
		this.card.isMine = b ;
		
		if (b){
			this.display ();
			transform.Find("Art").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[this.card.IdClass];
		}
		else{
			this.hide ();
			transform.Find("Art").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[this.card.IdClass+10];
		}
		
		this.id = i ;
		gameObject.name = "Card"+(i);
		
		gameObject.transform.FindChild("AttackZone").GetComponent<AttackPictoController>().setIDCard(i);
		gameObject.transform.FindChild("LifeBar").transform.FindChild("PV").GetComponent<PVPictoController>().setIDCard(i);
		gameObject.transform.FindChild("PictoTR").GetComponent<TRPictoController>().setIDCard(i);
		
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
		t.FindChild("Art").GetComponent<SpriteRenderer>().sortingOrder = 11 ;
		t.FindChild("Icon1").GetComponent<SpriteRenderer>().sortingOrder = 12 ;
		t.FindChild("Icon1").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 13 ;
		t.FindChild("Icon2").GetComponent<SpriteRenderer>().sortingOrder = 12 ;
		t.FindChild("Icon2").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 13 ;
		t.FindChild("Icon3").GetComponent<SpriteRenderer>().sortingOrder = 12 ;
		t.FindChild("Icon3").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 13 ;
		t.Find("AttackZone").FindChild("AttackPicto").GetComponent<SpriteRenderer>().sortingOrder = 12 ;
		t.Find("AttackZone").FindChild("AttackValue").GetComponent<MeshRenderer>().sortingOrder = 14 ;
		t.FindChild("Life").GetComponent<SpriteRenderer>().sortingOrder = 13 ;
		t.FindChild("LifeBar").GetComponent<SpriteRenderer>().sortingOrder = 12 ;
		t.FindChild("LifeBar").FindChild("PV").GetComponent<MeshRenderer>().sortingOrder = 14 ;
		t.FindChild("LifeBar").FindChild("PVValue").GetComponent<MeshRenderer>().sortingOrder = 14 ;
		t.Find("PictoTR").GetComponent<SpriteRenderer>().sortingOrder = 14 ;
		t.Find("WaitTime").GetComponent<MeshRenderer>().sortingOrder = 14 ;
		t.FindChild("HoverLayer").GetComponent<SpriteRenderer>().sortingOrder = 15 ;
		t.FindChild("SkillEffect").GetComponent<SpriteRenderer>().sortingOrder = 16 ;
		t.FindChild("SkillEffect").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 17 ;
	}
	
	public void moveBackward(){
		Transform t = gameObject.transform;
		t.FindChild("Art").GetComponent<SpriteRenderer>().sortingOrder = 1 ;
		t.FindChild("Icon1").GetComponent<SpriteRenderer>().sortingOrder = 2 ;
		t.FindChild("Icon1").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 3 ;
		t.FindChild("Icon2").GetComponent<SpriteRenderer>().sortingOrder = 2 ;
		t.FindChild("Icon2").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 3 ;
		t.FindChild("Icon3").GetComponent<SpriteRenderer>().sortingOrder = 2 ;
		t.FindChild("Icon3").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 3 ;
		t.Find("AttackZone").FindChild("AttackPicto").GetComponent<SpriteRenderer>().sortingOrder = 2 ;
		t.Find("AttackZone").FindChild("AttackValue").GetComponent<MeshRenderer>().sortingOrder = 4 ;
		t.FindChild("Life").GetComponent<SpriteRenderer>().sortingOrder = 3 ;
		t.FindChild("LifeBar").GetComponent<SpriteRenderer>().sortingOrder = 2 ;
		t.FindChild("LifeBar").FindChild("PV").GetComponent<MeshRenderer>().sortingOrder = 4 ;
		t.FindChild("LifeBar").FindChild("PVValue").GetComponent<MeshRenderer>().sortingOrder = 4 ;
		t.Find("PictoTR").GetComponent<SpriteRenderer>().sortingOrder = 4 ;
		t.Find("WaitTime").GetComponent<MeshRenderer>().sortingOrder = 4 ;
		t.FindChild("HoverLayer").GetComponent<SpriteRenderer>().sortingOrder = 5 ;
		t.FindChild("SkillEffect").GetComponent<SpriteRenderer>().sortingOrder = 6 ;
		t.FindChild("SkillEffect").FindChild("Text").GetComponent<MeshRenderer>().sortingOrder = 7 ;
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
	
	public void addDamagesModifyer(Modifyer m){
		
		if(m.amount<0){
			if (this.card.getLife()-m.amount>=this.card.GetTotalLife()){
				m.amount = 0 ;	
			}
			else{
				m.amount = Mathf.Min(m.amount, this.card.GetTotalLife()-this.card.getLife()) ;
				GameView.instance.displaySkillEffect(this.id, m.title+"\nSoin de "+(-1*m.amount)+"PV", 1);
			}
		}
		else{
			if (this.card.getLife()-m.amount<=0){
				this.kill();
			}
			else{
				GameView.instance.displaySkillEffect(this.id, m.title+"\n"+m.description, 1);
			}
		}
		this.card.damagesModifyers.Add(m);
	}

	public void showIcons(){
		string text = "";
		int compteurIcones = 1;
		List<string> listeTextes = this.card.getEsquiveIcon();
		if(listeTextes.Count>0){
			gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<BoxCollider>().enabled = true;
			gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().sprite = this.iconeBackgroundSprites[1];
			gameObject.transform.FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.iconeSprites[1];
			gameObject.transform.FindChild("Icon"+compteurIcones).FindChild("DescriptionBox").FindChild("TitleText").GetComponent<TextMeshPro>().text = "Protection";
		
			for(int i = 0 ; i < listeTextes.Count ; i++){
				text+="<b>"+listeTextes[i]+"</b> : ";
				i++;
				text+=listeTextes[i]+"\n";
			}
			gameObject.transform.FindChild("Icon"+compteurIcones).FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = text;
			compteurIcones++;
		}
		
		listeTextes = this.card.getMoveIcon();
		if(listeTextes.Count>0){
			gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<BoxCollider>().enabled = true;
			if(this.card.getMove()>=this.card.Move){
				gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().sprite = this.iconeBackgroundSprites[1];
			}
			else{
				gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().sprite = this.iconeBackgroundSprites[0];
			}
			gameObject.transform.FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.iconeSprites[0];
			gameObject.transform.FindChild("Icon"+compteurIcones).FindChild("DescriptionBox").FindChild("TitleText").GetComponent<TextMeshPro>().text = "Déplacement";
			
			for(int i = 0 ; i < listeTextes.Count ; i++){
				text+="<b>"+listeTextes[i]+"</b> : ";
				i++;
				text+=listeTextes[i]+"\n";
			}
			gameObject.transform.FindChild("Icon"+compteurIcones).FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = text;
			compteurIcones++;
		}
		
		if(this.card.isStateModifyed){
			gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<BoxCollider>().enabled = true;
			if(this.card.state.type==4){
				gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().sprite = this.iconeBackgroundSprites[1];
			}
			else{
				gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().sprite = this.iconeBackgroundSprites[0];
			}
			gameObject.transform.FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.iconeSprites[this.card.state.type];
			gameObject.transform.FindChild("Icon"+compteurIcones).FindChild("DescriptionBox").FindChild("TitleText").GetComponent<TextMeshPro>().text = this.card.state.title;
			gameObject.transform.FindChild("Icon"+compteurIcones).FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = this.card.state.description;
			compteurIcones++;
		}
		
		for(int j = compteurIcones ; j < 4 ; j++){
			gameObject.transform.FindChild("Icon"+j).GetComponent<SpriteRenderer>().enabled = false;
			gameObject.transform.FindChild("Icon"+j).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = false;
			gameObject.transform.FindChild("Icon"+j).FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled = false;
			gameObject.transform.FindChild("Icon"+j).FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled = false;
			gameObject.transform.FindChild("Icon"+j).FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled = false;
			gameObject.transform.FindChild("Icon"+j).GetComponent<BoxCollider>().enabled = false;
			
		}
	}
	
	public void show(bool showTR)
	{
		if(!this.isHidden){
			this.updateAttack();
			this.updateLife();
			this.updateTR();
			
			this.showIcons();
			
			this.showTR(showTR);
		}
	}

	public void setTile(Tile t, Vector3 p)
	{
		this.tile = t;
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
		}
		else{
			float rapport = this.timerMove/this.MoveTime;
			gameObject.transform.localPosition = new Vector3(this.initialP.x+(this.finalP.x-this.initialP.x)*rapport, this.initialP.y+(this.finalP.y-this.initialP.y)*rapport, -0.5f);
		}
	}
	
	public void addDeadTime(float t){
		this.timerDead += t ;
		if (this.timerDead>this.deadTime){
			this.displayDead(false);
			this.moveToDeadZone();
			GameView.instance.emptyTile(this.id);
			GameView.instance.recalculateDestinations();
		}
	}
	
	public void addSETime(float t){
		this.timerSE += t ;
		if (this.timerSE>this.SETime){
			if(!GameView.instance.getTileController(this.id).isDisplayingTarget){
				this.switchSE();
			}
			this.timerSE = 0f;
		}
	}
	
	public void switchSE(){
		if(this.isShowingSE){
			gameObject.transform.FindChild("SkillEffect").GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.FindChild("SkillEffect").FindChild("Text").GetComponent<MeshRenderer>().enabled = false ;
		}
		else{
			gameObject.transform.FindChild("SkillEffect").GetComponent<SpriteRenderer>().enabled = true ;
			gameObject.transform.FindChild("SkillEffect").FindChild("Text").GetComponent<MeshRenderer>().enabled = true ;
		}
		this.isShowingSE = !this.isShowingSE;
	}

	public void updateAttack()
	{
		int attackBase = this.card.Attack ;
		int attack = this.card.getAttack();
		if(attackBase>attack){
			gameObject.transform.FindChild("AttackZone").FindChild("AttackValue").GetComponent<TextMeshPro>().color = Color.yellow;
		}
		else if(attackBase<attack){
			gameObject.transform.FindChild("AttackZone").FindChild("AttackValue").GetComponent<TextMeshPro>().color = Color.green;
		}
		else{
			gameObject.transform.FindChild("AttackZone").FindChild("AttackValue").GetComponent<TextMeshPro>().color = Color.white;
		}
		gameObject.transform.FindChild("AttackZone").FindChild("AttackValue").GetComponent<TextMeshPro>().text = this.card.GetAttackString();
		
		string title = "Points d'attaque";
		string description = "Attaque de base : "+this.card.Attack+"\n";
		List<string> textes = this.card.getIconAttack();
		for(int i = 0 ; i < textes.Count ; i++){
			description += "<b>"+textes[i]+" : "+"</b>";
			i++;
			description += textes[i]+"\n";
		}
		if (textes.Count>0){
			description += "---> TOTAL : "+this.card.getAttack();
		}
		else{
			description += "Pas de bonus ou malus en cours";
		}
		this.setAttackDescription(title, description);
	}
	
	public void updateLife()
	{
		float percentage = 1.0f*this.card.getLife()/this.card.GetTotalLife();
		
		Transform tempGO = gameObject.transform.FindChild("Life");
		
		if (percentage>(2f/3f)){
			tempGO.GetComponent<SpriteRenderer>().sprite = this.lifebarSprites[1];
		}
		else if(percentage>(1f/3f)){
			tempGO.GetComponent<SpriteRenderer>().sprite = this.lifebarSprites[2];
		}
		else{
			tempGO.GetComponent<SpriteRenderer>().sprite = this.lifebarSprites[3];
		}
		
		Vector3 position = tempGO.transform.localPosition;
		Vector3 scale = tempGO.transform.localScale;
		
		position.x = -0.25f*(1f-percentage)*1.75f;
		scale.x = percentage*0.25f;
		
		tempGO.transform.localPosition = position;
		tempGO.transform.localScale = scale;
		
		int lifeBase = this.card.GetTotalLife() ;
		int life = this.card.getLife();
		gameObject.transform.FindChild("LifeBar").FindChild("PVValue").GetComponent<TextMeshPro>().text = life+"/"+lifeBase;
		if (lifeBase>this.card.Life){
			gameObject.transform.FindChild("LifeBar").FindChild("PVValue").GetComponent<TextMeshPro>().color = Color.green;
		}
		else if(lifeBase<this.card.Life){
			gameObject.transform.FindChild("LifeBar").FindChild("PVValue").GetComponent<TextMeshPro>().color = Color.yellow;
		}
		else{
			gameObject.transform.FindChild("LifeBar").FindChild("PVValue").GetComponent<TextMeshPro>().color = Color.white;
		}
		string title = "Points de vie";
		string description = "Points de vie de base : "+this.card.Life+"\n";
		List<string> textes = this.card.getIconLife();
		for(int i = 0 ; i < textes.Count ; i++){
			description += "<b>"+textes[i]+" : "+"</b>";
			i++;
			description += textes[i]+"\n";
		}
		if (textes.Count>0){
			description += "---> TOTAL : "+this.card.getLife();
		}
		else{
			description += "Pas de bonus ou malus en cours";
		}
		this.setPVDescription(title, description);
	}
	
	public void updateTR(){
		string title = "Temps d'attente";
		string description = "L'unité doit attendre "+this.card.nbTurnsToWait+" tours avant de combattre";
		this.setTRDescription(title, description);
	}
	
	public void kill()
	{
		this.card.isDead = true;
		
		if(this.card.isLeader()){
			GameView.instance.removeLeaderEffect(this.id, this.card.isMine);
		}
				
		GameView.instance.killHandle (this.id);
	}
	
	public void moveToDeadZone(){
		gameObject.transform.localPosition = new Vector3(0f, -20f, 0f);
	}
	
	public void checkModyfiers()
	{
		this.card.checkModifyers();
		this.show(true);
	}
	
	public bool canBeTargeted()
	{
		return (!this.card.isDead && !this.card.isIntouchable());
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
	
	public void showDescriptionIcon(int i, bool b){
		gameObject.transform.FindChild("Icon"+i).FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled=b;
		gameObject.transform.FindChild("Icon"+i).FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled=b;
		gameObject.transform.FindChild("Icon"+i).FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled=b;
	}
	
	public void showDescriptionAttack(bool b){
		gameObject.transform.FindChild("AttackZone").FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled=b;
		gameObject.transform.FindChild("AttackZone").FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled=b;
		gameObject.transform.FindChild("AttackZone").FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled=b;
	}
	
	public void showDescriptionLife(bool b){
		gameObject.transform.FindChild("LifeBar").FindChild("PV").FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled=b;
		gameObject.transform.FindChild("LifeBar").FindChild("PV").FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled=b;
		gameObject.transform.FindChild("LifeBar").FindChild("PV").FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled=b;
	}
	
	public void showDescriptionTurns(bool b){
		gameObject.transform.FindChild("PictoTR").FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled=b;
		gameObject.transform.FindChild("PictoTR").FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled=b;
		gameObject.transform.FindChild("PictoTR").FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled=b;
	}
	
	public void setAttackDescription(string title, string description){
		gameObject.transform.FindChild("AttackZone").GetComponent<AttackPictoController>().setTexts(title, description);
	}
	
	public void setTRDescription(string title, string description){
		gameObject.transform.FindChild("PictoTR").GetComponent<TRPictoController>().setTexts(title, description);
	}
	
	public void setPVDescription(string title, string description){
		gameObject.transform.FindChild("LifeBar").FindChild("PV").GetComponent<PVPictoController>().setTexts(title, description);
	}
	
	public void displaySkillEffect(bool b){
		gameObject.transform.FindChild("SkillEffect").GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("SkillEffect").FindChild("Text").GetComponent<MeshRenderer>().enabled = b ;
		this.isDisplayingSkillEffect = b;
		this.timerSE = 0f;
		this.isShowingSE = true ;
	}
	
	public void setSkillEffect(string s, int type){
		gameObject.transform.FindChild("SkillEffect").GetComponent<SpriteRenderer>().sprite = this.skillEffectSprites[type] ;
		gameObject.transform.FindChild("SkillEffect").FindChild("Text").GetComponent<TextMeshPro>().text= s;
		if(type==0){
			gameObject.transform.FindChild("SkillEffect").FindChild("Text").GetComponent<TextMeshPro>().color = Color.white ;
		}
		else{
			gameObject.transform.FindChild("SkillEffect").FindChild("Text").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f) ;
		}
		this.displaySkillEffect(true);
	}
	
	public void displayDead(bool b){
		gameObject.transform.FindChild("DeadLayer").GetComponent<SpriteRenderer>().enabled = b ;
		this.timerDead = 0f;
		this.isShowingDead = b;
	}
}


