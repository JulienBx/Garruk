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
	float deadTime = 0.5f;
	public bool isShowingDead;
	public List<Tile> destinations ;
	
	void Awake()
	{
		this.isDisabled = false;
		this.showDescriptionAttack(false);
		this.showDescriptionLife(false);
		this.showDescriptionIcon(1, false);
		this.showDescriptionIcon(2, false);
		this.showDescriptionIcon(3, false);
		this.showHover(false);
		this.displayDead(false);
		Transform t = gameObject.transform;
		this.toStop = false ;
		this.isRunning = false;
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
	
	public void showHover(bool b){
		gameObject.transform.Find("Background").FindChild("HoverLayer").GetComponent<SpriteRenderer>().enabled = b ;
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
		t.Find("Background").FindChild("Icon1").GetComponent<BoxCollider>().enabled = false ;
		t.Find("Background").FindChild("Icon2").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("Icon2").FindChild("Picto").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("Icon2").GetComponent<BoxCollider>().enabled = false ;
		t.Find("Background").FindChild("Icon3").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("Icon3").FindChild("Picto").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("Icon3").GetComponent<BoxCollider>().enabled = false ;
		t.Find("Background").FindChild("AttackPicto").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("AttackValue").GetComponent<MeshRenderer>().enabled = false ;
		t.Find("Background").FindChild("AttackValue").GetComponent<BoxCollider>().enabled = false ;
		t.Find("Background").FindChild("PVPicto").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("Background").FindChild("PVValue").GetComponent<MeshRenderer>().enabled = false ;
		t.Find("Background").FindChild("PVValue").GetComponent<BoxCollider>().enabled = false ;
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
		t.Find("Background").FindChild("Icon1").GetComponent<BoxCollider>().enabled = true ;
		t.Find("Background").FindChild("Icon2").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("Icon2").FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("Icon2").GetComponent<BoxCollider>().enabled = true ;
		t.Find("Background").FindChild("Icon3").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("Icon3").FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("Icon3").GetComponent<BoxCollider>().enabled = true ;
		t.Find("Background").FindChild("AttackPicto").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("AttackValue").GetComponent<MeshRenderer>().enabled = true ;
		t.Find("Background").FindChild("AttackValue").GetComponent<BoxCollider>().enabled = true ;
		t.Find("Background").FindChild("PVPicto").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("PVValue").GetComponent<MeshRenderer>().enabled = true ;
		t.Find("Background").FindChild("PVValue").GetComponent<BoxCollider>().enabled = true ;
		t.Find("Background").FindChild("LifeBar").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("LifeBarEnd").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("Life").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("Background").FindChild("LifeEnd").GetComponent<SpriteRenderer>().enabled = true ;
	}
	
	public void setCard(GameCard c, bool b, int i)
	{
		this.card = c ;
		c.isMine = b ;
		gameObject.transform.Find("Background").FindChild("Character").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[this.card.getSkills()[0].Id];
		if (c.isMine){
			gameObject.transform.Find("Background").FindChild("Character").transform.localScale = new Vector3(1,1,1);
			gameObject.transform.Find("Background").FindChild("Circle").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[0];
			gameObject.transform.Find("Background").FindChild("AttackPicto").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[1];
			gameObject.transform.Find("Background").FindChild("PVPicto").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[2];
		}
		else {
			gameObject.transform.Find("Background").FindChild("Character").transform.localScale = new Vector3(-1,1,1);
			gameObject.transform.Find("Background").FindChild("Circle").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[3];
			gameObject.transform.Find("Background").FindChild("AttackPicto").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[4];
			gameObject.transform.Find("Background").FindChild("PVPicto").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[5];
		}
		
		transform.Find("Background").FindChild("AttackValue").GetComponent<TextMeshPro>().text = c.GetAttackString();
		transform.Find("Background").FindChild("PVValue").GetComponent<TextMeshPro>().text = c.GetLifeString();
		this.card.isMine = b ;
		
		if (b){
			this.display ();
		}
		else{
			this.hide ();
		}
		
		this.id = i ;
		gameObject.name = "Card"+(i);
	}
	
	public void updateLife(){
		float f = 1.0f*this.card.getLife()/this.card.GetTotalLife();
		this.updateLifePercentage(f);
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
		t.FindChild("Background").GetComponent<SpriteRenderer>().sortingOrder = 10 ;
		t.Find("Background").FindChild("Circle").GetComponent<SpriteRenderer>().sortingOrder = 12 ;
		t.Find("Background").FindChild("Character").GetComponent<SpriteRenderer>().sortingOrder = 11 ;
		t.Find("Background").FindChild("Icon1").GetComponent<SpriteRenderer>().sortingOrder = 14 ;
		t.Find("Background").FindChild("Icon1").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 15 ;
		t.Find("Background").FindChild("Icon2").GetComponent<SpriteRenderer>().sortingOrder = 14 ;
		t.Find("Background").FindChild("Icon2").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 15 ;
		t.Find("Background").FindChild("Icon3").GetComponent<SpriteRenderer>().sortingOrder = 14 ;
		t.Find("Background").FindChild("Icon3").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 15 ;
		t.Find("Background").FindChild("AttackPicto").GetComponent<SpriteRenderer>().sortingOrder = 15 ;
		t.Find("Background").FindChild("AttackValue").GetComponent<MeshRenderer>().sortingOrder = 16 ;
		t.Find("Background").FindChild("PVPicto").GetComponent<SpriteRenderer>().sortingOrder = 15 ;
		t.Find("Background").FindChild("PVValue").GetComponent<MeshRenderer>().sortingOrder = 16 ;
		t.Find("Background").FindChild("LifeBar").GetComponent<SpriteRenderer>().sortingOrder = 13 ;
		t.Find("Background").FindChild("LifeBarEnd").GetComponent<SpriteRenderer>().sortingOrder = 13 ;
		t.Find("Background").FindChild("Life").GetComponent<SpriteRenderer>().sortingOrder = 14 ;
		t.Find("Background").FindChild("LifeEnd").GetComponent<SpriteRenderer>().sortingOrder = 14 ;
	}
	
	public void moveBackward(){
		Transform t = gameObject.transform;
		t.FindChild("Background").GetComponent<SpriteRenderer>().sortingOrder = 0 ;
		t.Find("Background").FindChild("Circle").GetComponent<SpriteRenderer>().sortingOrder = 2 ;
		t.Find("Background").FindChild("Character").GetComponent<SpriteRenderer>().sortingOrder = 1 ;
		t.Find("Background").FindChild("Icon1").GetComponent<SpriteRenderer>().sortingOrder = 4 ;
		t.Find("Background").FindChild("Icon1").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 5 ;
		t.Find("Background").FindChild("Icon2").GetComponent<SpriteRenderer>().sortingOrder = 4 ;
		t.Find("Background").FindChild("Icon2").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 5 ;
		t.Find("Background").FindChild("Icon3").GetComponent<SpriteRenderer>().sortingOrder = 4 ;
		t.Find("Background").FindChild("Icon3").FindChild("Picto").GetComponent<SpriteRenderer>().sortingOrder = 5 ;
		t.Find("Background").FindChild("AttackPicto").GetComponent<SpriteRenderer>().sortingOrder = 5 ;
		t.Find("Background").FindChild("AttackValue").GetComponent<MeshRenderer>().sortingOrder = 6 ;
		t.Find("Background").FindChild("PVPicto").GetComponent<SpriteRenderer>().sortingOrder = 5 ;
		t.Find("Background").FindChild("PVValue").GetComponent<MeshRenderer>().sortingOrder = 6 ;
		t.Find("Background").FindChild("LifeBar").GetComponent<SpriteRenderer>().sortingOrder = 3 ;
		t.Find("Background").FindChild("LifeBarEnd").GetComponent<SpriteRenderer>().sortingOrder = 3 ;
		t.Find("Background").FindChild("Life").GetComponent<SpriteRenderer>().sortingOrder = 4 ;
		t.Find("Background").FindChild("LifeEnd").GetComponent<SpriteRenderer>().sortingOrder = 4 ;
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
			gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<BoxCollider>().enabled = true;
			gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.iconeSprites[1];
			gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("DescriptionBox").FindChild("TitleText").GetComponent<TextMeshPro>().text = "Protection";
		
			for(int i = 0 ; i < listeTextes.Count ; i++){
				text+="<b>"+listeTextes[i]+"</b> : ";
				i++;
				text+=listeTextes[i]+"\n";
			}
			gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = text;
			compteurIcones++;
		}
		
		listeTextes = this.card.getMoveIcon();
		if(listeTextes.Count>0){
			gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<BoxCollider>().enabled = true;
			gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.iconeSprites[0];
			gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("DescriptionBox").FindChild("TitleText").GetComponent<TextMeshPro>().text = "Déplacement";
			
			for(int i = 0 ; i < listeTextes.Count ; i++){
				text+="<b>"+listeTextes[i]+"</b> : ";
				i++;
				text+=listeTextes[i]+"\n";
			}
			gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = text;
			compteurIcones++;
		}
		
		if(this.card.isStateModifyed){
			gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).GetComponent<BoxCollider>().enabled = true;
			gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.iconeSprites[this.card.state.type];
			gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("DescriptionBox").FindChild("TitleText").GetComponent<TextMeshPro>().text = this.card.state.title;
			gameObject.transform.Find("Background").FindChild("Icon"+compteurIcones).FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = this.card.state.description;
			compteurIcones++;
		}
		
		for(int j = compteurIcones ; j < 4 ; j++){
			gameObject.transform.Find("Background").FindChild("Icon"+j).GetComponent<SpriteRenderer>().enabled = false;
			gameObject.transform.Find("Background").FindChild("Icon"+j).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = false;
			gameObject.transform.Find("Background").FindChild("Icon"+j).FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled = false;
			gameObject.transform.Find("Background").FindChild("Icon"+j).FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled = false;
			gameObject.transform.Find("Background").FindChild("Icon"+j).FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled = false;
			gameObject.transform.Find("Background").FindChild("Icon"+j).GetComponent<BoxCollider>().enabled = false;
			
		}
	}
	
	public void show()
	{
		if(!this.isHidden){
			this.updateAttack();
			this.updateLife();
			this.showIcons();
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
			GameView.instance.recalculateDestinations();
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

	public void updateAttack()
	{
		int attackBase = this.card.Attack ;
		int attack = this.card.getAttack();
		if(attackBase>attack){
			gameObject.transform.FindChild("Background").FindChild("AttackValue").GetComponent<TextMeshPro>().color = Color.yellow;
		}
		else if(attackBase<attack){
			gameObject.transform.FindChild("Background").FindChild("AttackValue").GetComponent<TextMeshPro>().color = Color.green;
		}
		else{
			gameObject.transform.FindChild("Background").FindChild("AttackValue").GetComponent<TextMeshPro>().color = Color.white;
		}
		gameObject.transform.FindChild("Background").FindChild("AttackValue").GetComponent<TextMeshPro>().text = this.card.GetAttackString();
		
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
		this.show();
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
		transform.Find("Background").FindChild("Icon"+i).FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled=b;
		transform.Find("Background").FindChild("Icon"+i).FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled=b;
		transform.Find("Background").FindChild("Icon"+i).FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled=b;
	}
	
	public void showDescriptionAttack(bool b){
		gameObject.transform.FindChild("Background").FindChild("AttackDB").GetComponent<SpriteRenderer>().enabled=b;
		gameObject.transform.FindChild("Background").FindChild("AttackDB").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled=b;
		gameObject.transform.FindChild("Background").FindChild("AttackDB").FindChild("TitleText").GetComponent<MeshRenderer>().enabled=b;
	}
	
	public void showDescriptionLife(bool b){
		gameObject.transform.FindChild("Background").FindChild("PVDB").GetComponent<SpriteRenderer>().enabled=b;
		gameObject.transform.FindChild("Background").FindChild("PVDB").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled=b;
		gameObject.transform.FindChild("Background").FindChild("PVDB").FindChild("TitleText").GetComponent<MeshRenderer>().enabled=b;
	}

	public void setAttackDescription(string title, string description){
		gameObject.transform.FindChild("Background").FindChild("AttackDB").GetComponent<AttackPictoController>().setTexts(title, description);
	}
	
	public void setPVDescription(string title, string description){
		gameObject.transform.FindChild("Background").FindChild("PVDB").GetComponent<PVPictoController>().setTexts(title, description);
	}
	
	public void displayDead(bool b){
		gameObject.transform.FindChild("Background").FindChild("DeadLayer").GetComponent<SpriteRenderer>().enabled = b ;
		this.timerDead = 0f;
		this.isShowingDead = b;
	}
	
	
}


