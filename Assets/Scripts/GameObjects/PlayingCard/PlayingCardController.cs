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
	
	public List<Tile> destinations ;

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
		this.showTarget(false);
		
		Transform t = gameObject.transform;
		this.toStop = false ;
		this.isRunning = false;
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
	
	public void showTarget(bool b){
		gameObject.transform.FindChild("TargetLayer").GetComponent<SpriteRenderer>().enabled = b ;
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
		Transform t = gameObject.transform;
		t.FindChild("WaitTime").GetComponent<TextMeshPro>().text = ""+this.card.getNbTurnsToWait();
		t.FindChild("WaitTime").GetComponent<MeshRenderer>().enabled = b ;
		t.FindChild("PictoTR").GetComponent<SpriteRenderer>().enabled = b ;
		t.FindChild("PictoTR").GetComponent<BoxCollider>().enabled = b ;
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
		t.FindChild("Life").GetComponent<SpriteRenderer>().sortingOrder = 12 ;
		t.FindChild("LifeBar").GetComponent<SpriteRenderer>().sortingOrder = 11 ;
		t.FindChild("LifeBar").FindChild("PV").GetComponent<MeshRenderer>().sortingOrder = 14 ;
		t.FindChild("LifeBar").FindChild("PVValue").GetComponent<MeshRenderer>().sortingOrder = 14 ;
		t.Find("PictoTR").GetComponent<SpriteRenderer>().sortingOrder = 14 ;
		t.Find("WaitTime").GetComponent<MeshRenderer>().sortingOrder = 14 ;
		t.FindChild("HoverLayer").GetComponent<SpriteRenderer>().sortingOrder = 15 ;
		t.FindChild("TargetLayer").GetComponent<SpriteRenderer>().sortingOrder = 15 ;
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
		t.FindChild("Life").GetComponent<SpriteRenderer>().sortingOrder = 2 ;
		t.FindChild("LifeBar").GetComponent<SpriteRenderer>().sortingOrder = 1 ;
		t.FindChild("LifeBar").FindChild("PV").GetComponent<MeshRenderer>().sortingOrder = 4 ;
		t.FindChild("LifeBar").FindChild("PVValue").GetComponent<MeshRenderer>().sortingOrder = 4 ;
		t.Find("PictoTR").GetComponent<SpriteRenderer>().sortingOrder = 4 ;
		t.Find("WaitTime").GetComponent<MeshRenderer>().sortingOrder = 4 ;
		t.FindChild("HoverLayer").GetComponent<SpriteRenderer>().sortingOrder = 5 ;
		t.FindChild("TargetLayer").GetComponent<SpriteRenderer>().sortingOrder = 5 ;
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

	public void showIcons(){
		int compteurIcones = 1;
		
		List<string> listeTextes = new List<string>();
		string description = "";
		int idIcon ;
		
		listeTextes = new List<string>();
		idIcon = this.card.getIdIconEffect();
		listeTextes = this.card.getIconEffect();
		description = "";
		
		if(listeTextes.Count>0){
			gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.FindChild("Icon"+compteurIcones).FindChild("Effect"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.FindChild("Icon"+compteurIcones).FindChild("Effect"+compteurIcones).GetComponent<SpriteRenderer>().sprite = this.iconeSprites[idIcon];
			
			gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<IconController>().setInformation(listeTextes[0], listeTextes[1]);
			if(listeTextes[2]=="BONUS"){
				gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().sprite = this.iconeSprites[2];
			}
			else{
				gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().sprite = this.iconeSprites[1];
			}
			compteurIcones++;
		}
		
		listeTextes = new List<string>();
		listeTextes = this.card.getIconMove();
		if(listeTextes.Count>0){
			description = "Déplacement de base : "+this.card.Move+"\n";
			gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.FindChild("Icon"+compteurIcones).FindChild("Effect"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.FindChild("Icon"+compteurIcones).FindChild("Effect"+compteurIcones).GetComponent<SpriteRenderer>().sprite = this.iconeSprites[11];
			
			for(int i = 0 ; i < listeTextes.Count ; i++){
				description += "<b>"+listeTextes[i]+" : "+"</b>";
				i++;
				description += listeTextes[i]+"\n";
			}
			if (listeTextes.Count>0){
				description += "---> TOTAL : "+this.card.getMove();
			}
			gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<IconController>().setInformation("Déplacement", description);
			if(this.card.getMove()>=this.card.Move){
				gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().sprite = this.iconeSprites[2];
			}
			else{
				gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().sprite = this.iconeSprites[1];
			}
			compteurIcones++;
		}
		
		listeTextes = new List<string>();
		listeTextes = this.card.getIconShield();
		description = "";
		if(listeTextes.Count>0){
			gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = true;
			
			gameObject.transform.FindChild("Icon"+compteurIcones).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.iconeSprites[10];
			
			for(int i = 0 ; i < listeTextes.Count ; i++){
				description += "<b>"+listeTextes[i]+" : "+"</b>";
				i++;
				description += listeTextes[i]+"\n";
			}
			gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<IconController>().setInformation("Protection", description);
			gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().sprite = this.iconeSprites[2];
			compteurIcones++;
		}
		
		for (int i = compteurIcones; i < 4 ; i++)
		{
			gameObject.transform.FindChild("Icon"+i).FindChild("Picto").GetComponent<SpriteRenderer>().enabled = false;
			gameObject.transform.FindChild("Icon"+i).GetComponent<SpriteRenderer>().enabled = false;
		}
	}
	
	public void show(bool showTR)
	{
		if(!this.isHidden){
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
			
			this.updateAttack();
			this.updateLife();
			
			this.showIcons();
			
			this.showTR(showTR);
			
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
			
			title = "Temps d'attente";
			description = "L'unité doit attendre "+this.card.nbTurnsToWait+" tours avant de combattre";
			this.setTRDescription(title, description);
			
			title = "Points de vie";
			description = "Points de vie de base : "+this.card.Life+"\n";
			textes = this.card.getIconLife();
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
			this.setPVDescription(title, description);
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
		print ("Je add");
		if (this.timerMove>this.MoveTime){
			this.isMoving = false ;
			gameObject.transform.localPosition = this.finalP;
		}
		else{
			float rapport = this.timerMove/this.MoveTime;
			gameObject.transform.localPosition = new Vector3(this.initialP.x+(this.finalP.x-this.initialP.x)*rapport, this.initialP.y+(this.finalP.y-this.initialP.y)*rapport, -0.5f);
		}
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
	}
	
	public void updateLife()
	{
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
	}
	
	public void kill()
	{
		this.card.isDead = true;
	}
	
	public void disappear(){
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers [i].GetComponent<Renderer>().enabled = false;
		}
		gameObject.transform.position = new Vector3(-20, -20, -20);
	}
	
	public void checkModyfiers()
	{
		this.card.checkModifyers();
		this.show(true);
	}
	
	public void activateSleepingModifiers()
	{
//		for (int i = 0; i < this.card.modifiers.Count; i++)
//		{
//			if (this.card.modifiers [i].Duration == -10)
//			{
//				this.card.modifiers [i].Duration = 1;	
//			}
//		}
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
}


