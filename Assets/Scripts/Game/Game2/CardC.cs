using System;
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
	List<ModifyerM> bonusModifyers ;
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

		this.stateModifyers = new List<ModifyerM>();
		this.moveModifyers = new List<ModifyerM>();
		this.esquiveModifyers = new List<ModifyerM>();
		this.bonusModifyers = new List<ModifyerM>();
		this.lifeModifyers = new List<ModifyerM>();
		this.attackModifyers = new List<ModifyerM>();
		this.bouclierModifyers = new List<ModifyerM>();
		this.damageModifyers = new List<ModifyerM>();

		attackText = gameObject.transform.FindChild("Background").FindChild("AttackValue").GetComponent<TextMeshPro>();
		lifeText = gameObject.transform.FindChild("Background").FindChild("PVValue").GetComponent<TextMeshPro>();

		this.initDestinations();
	}

	public bool isClignoting(){
		return this.clignoting;
	}

	public void stopClignote(){
		this.stopClignoting=true;
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

	public void move(){
		this.moved = true ;
	}

	public bool hasMoved(){
		return this.moved;
	}

	public bool hasPlayed(){
		return this.played;
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
		gameObject.transform.Find("Background").GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.Find("Background").FindChild("Character").GetComponent<SpriteRenderer>().enabled = b ;
	}

	public void showCollider(bool b){
		gameObject.GetComponent<BoxCollider>().enabled = b;
	}

	public void showIcons(bool b){
		if(b){
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
			gameObject.transform.Find("Background").FindChild("Icon1").GetComponent<SpriteRenderer>().enabled = b ;
			gameObject.transform.Find("Background").FindChild("Icon2").GetComponent<SpriteRenderer>().enabled = b ;
			gameObject.transform.Find("Background").FindChild("Icon3").GetComponent<SpriteRenderer>().enabled = b ;
		}
	}

	public void setIcon(int i, int idSkill){
		gameObject.transform.Find("Background").FindChild("Icon"+(i+1)).GetComponent<SpriteRenderer>().sprite = this.iconSprites[idSkill];
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
		this.upgradingAttack = this.card.getAttack()!=this.displayedAttack ;
		this.originalAttack = int.Parse(this.attackText.text);
		this.upgradeAttackTimer = 0f;
	}

	public void setLife(){
		this.upgradingLife = this.card.getLife()!=this.displayedLife ;
		this.originalLife = int.Parse(this.lifeText.text);
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
		}
		else{
			int tempInt = (this.originalLife+Mathf.RoundToInt((this.upgradeLifeTimer/this.upgradeTime)*(this.getLife()-this.originalLife)));
			if(this.displayedLife!=tempInt){
				this.setLifeText(tempInt) ;
			}
		}
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
	}

	public int getLife(){
		int life = this.getTotalLife();
		for(int i = 0 ; i < this.damageModifyers.Count ; i++){
			life-=this.damageModifyers[i].getAmount();
		}
		return life;
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
		return attack;
	}

	public int getMove(){
		int move = this.card.getMove();
		for(int i = 0 ; i < this.moveModifyers.Count ; i++){
			move+=this.moveModifyers[i].getAmount();
		}
		return move;
	}

	public List<int> getIcons(){
		List<int> icons = new List<int>();
		int compteur = 0;
		for(int i = 0 ; i < stateModifyers.Count && compteur<3 ; i++){
			icons.Add(this.stateModifyers[i].getIdIcon());
			compteur++;
		}
		for(int i = 0 ; i < esquiveModifyers.Count && compteur<3 ; i++){
			icons.Add(this.esquiveModifyers[i].getIdIcon());
			compteur++;
		}
		for(int i = 0 ; i < bouclierModifyers.Count && compteur<3 ; i++){
			icons.Add(this.bouclierModifyers[i].getIdIcon());
			compteur++;
		}
		for(int i = 0 ; i < moveModifyers.Count && compteur<3 ; i++){
			icons.Add(this.moveModifyers[i].getIdIcon());
			compteur++;
		}
		for(int i = 0 ; i < bonusModifyers.Count && compteur<3 ; i++){
			icons.Add(this.bonusModifyers[i].getIdIcon());
			compteur++;
		}
		return icons;
	}

	public void OnMouseEnter()
	{
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
				Game.instance.dropOnTile(tile.x,tile.y);
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
			percentage = Mathf.CeilToInt(Int32.Parse(tempstring)*this.getAttack()/100f);
			s = s.Substring(0,index-3)+percentage+s.Substring(index+4,s.Length-index-4);
		}
		if (s.Contains("%ATK")){
			index = s.IndexOf("%ATK");
			
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.CeilToInt(Int32.Parse(tempstring)*this.getAttack()/100f);
			s = s.Substring(0,index-3)+percentage+s.Substring(index+4,s.Length-index-4);
		}
		if (s.Contains("%PV")){
			index = s.IndexOf("%PV");
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.CeilToInt(Int32.Parse(tempstring)*this.getLife()/100f);
			s = s.Substring(0,index-4)+" "+percentage+" "+s.Substring(index+4,s.Length-index-4);
		}
		if (WordingSkills.getProba(this.card.getSkill(i).Id, this.card.getSkill(i).Power-1)!=100){
			s+=". "+WordingSkills.getProba(this.card.getSkill(i).Id, this.card.getSkill(i).Power-1)+"HIT%";
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
			percentage = Mathf.CeilToInt(Int32.Parse(tempstring)*this.getAttack()/100f);
			s = s.Substring(0,index-3)+percentage+s.Substring(index+4,s.Length-index-4);
		}
		if (s.Contains("%ATK")){
			index = s.IndexOf("%ATK");
			
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.CeilToInt(Int32.Parse(tempstring)*this.getAttack()/100f);
			s = s.Substring(0,index-3)+percentage+s.Substring(index+4,s.Length-index-4);
		}
		if (s.Contains("%PV")){
			index = s.IndexOf("%PV");
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.CeilToInt(Int32.Parse(tempstring)*this.getLife()/100f);
			s = s.Substring(0,index-4)+" "+percentage+" "+s.Substring(index+4,s.Length-index-4);
		}

		return s;
	}

	public List<ModifyerM> getEffects(){
		List<ModifyerM> effects = new List<ModifyerM>();
		for(int i = 0 ; i < this.stateModifyers.Count && effects.Count<10 ; i++){
			effects.Add(new ModifyerM(this.stateModifyers[i].getAmount(), this.stateModifyers[i].getIdIcon(), this.stateModifyers[i].getDescription(), this.stateModifyers[i].getTitle()));
		}

		for(int i = 0 ; i < this.esquiveModifyers.Count && effects.Count<10 ; i++){
			effects.Add(new ModifyerM(this.esquiveModifyers[i].getAmount(), this.esquiveModifyers[i].getIdIcon(), this.esquiveModifyers[i].getDescription(), this.esquiveModifyers[i].getTitle()));
		}

		for(int i = 0 ; i < this.bouclierModifyers.Count && effects.Count<10 ; i++){
			effects.Add(new ModifyerM(this.bouclierModifyers[i].getAmount(), this.bouclierModifyers[i].getIdIcon(), this.bouclierModifyers[i].getDescription(), this.bouclierModifyers[i].getTitle()));
		}

		for(int i = 0 ; i < this.bonusModifyers.Count && effects.Count<10 ; i++){
			effects.Add(new ModifyerM(this.bonusModifyers[i].getAmount(), this.bonusModifyers[i].getIdIcon(), this.bonusModifyers[i].getDescription(), this.bonusModifyers[i].getTitle()));
		}

		for(int i = 0 ; i < this.moveModifyers.Count && effects.Count<10 ; i++){
			effects.Add(new ModifyerM(this.moveModifyers[i].getAmount(), this.moveModifyers[i].getIdIcon(), this.moveModifyers[i].getDescription(), this.moveModifyers[i].getTitle()));
		}

		for(int i = 0 ; i < this.attackModifyers.Count && effects.Count<10 ; i++){
			effects.Add(new ModifyerM(this.attackModifyers[i].getAmount(), this.attackModifyers[i].getIdIcon(), this.attackModifyers[i].getDescription(), this.attackModifyers[i].getTitle()));
		}

		for(int i = 0 ; i < this.lifeModifyers.Count && effects.Count<10 ; i++){
			effects.Add(new ModifyerM(this.lifeModifyers[i].getAmount(), this.lifeModifyers[i].getIdIcon(), this.lifeModifyers[i].getDescription(), this.lifeModifyers[i].getTitle()));
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
			this.showCollider(true);
			if(this.id!=Game.instance.getCurrentCardID()){
				this.displayBackTile(true);
			}
			this.moveBackward();
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
}