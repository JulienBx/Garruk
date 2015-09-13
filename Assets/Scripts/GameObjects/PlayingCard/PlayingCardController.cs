using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PlayingCardController : GameObjectController
{
	public Sprite[] backgroundSprites;
	public Sprite[] lifebarSprites;
	public Sprite[] iconSprites;
	 
	Card card ;
	int id = -1 ;
	bool isDead ;
	bool isMine;
	
	bool hasMoved ;
	bool hasPlayed ;

	Tile tile ;

	private bool isDisabled; // variable pour le tutoriel
	
	float timerSelection = 0 ;
	float selectionTime = 0.5f ;
	bool isGettingBigger = true ;

	void Awake()
	{
		this.isDisabled = false;
		this.isDead = false;
		
		Transform t = gameObject.transform;
	}
	
	public void hide()
	{
		Transform t = gameObject.transform;
		t.Find("AttackZone").FindChild("AttackPicto").GetComponent<SpriteRenderer>().enabled = false ;
		t.FindChild("Art").GetComponent<SpriteRenderer>().enabled = false ;
		t.FindChild("Life").GetComponent<SpriteRenderer>().enabled = false ;
		t.FindChild("LifeBar").GetComponent<SpriteRenderer>().enabled = false ;
		t.FindChild("LifeBar").FindChild("PV").GetComponent<MeshRenderer>().enabled = false ;
		t.FindChild("LifeBar").FindChild("PVValue").GetComponent<MeshRenderer>().enabled = false ;
		t.FindChild("Icon0").GetComponent<SpriteRenderer>().enabled = false ;
		t.FindChild("Icon1").GetComponent<SpriteRenderer>().enabled = false ;
		t.FindChild("Icon2").GetComponent<SpriteRenderer>().enabled = false ;
		t.FindChild("Icon3").GetComponent<SpriteRenderer>().enabled = false ;
		t.Find("AttackZone").FindChild("AttackValue").GetComponent<MeshRenderer>().enabled = false ;
		gameObject.GetComponent<BoxCollider>().enabled = false ;
	}
	
	public void showTR(bool b)
	{
		Transform t = gameObject.transform;
		t.FindChild("WaitTime").GetComponent<TextMeshPro>().text = ""+this.card.nbTurnsToWait;
		
		t.FindChild("WaitTime").GetComponent<MeshRenderer>().enabled = b ;
		t.FindChild("PictoTR").GetComponent<SpriteRenderer>().enabled = b ;
		t.FindChild("PictoTR").GetComponent<BoxCollider>().enabled = b ;
	}
	
	public void display(bool toDisplaySkills)
	{
		Transform t = gameObject.transform;
		t.Find("AttackZone").FindChild("AttackPicto").GetComponent<SpriteRenderer>().enabled = true ;
		t.FindChild("Art").GetComponent<SpriteRenderer>().enabled = true ;
		t.FindChild("Life").GetComponent<SpriteRenderer>().enabled = true ;
		t.FindChild("LifeBar").GetComponent<SpriteRenderer>().enabled = true ;
		t.FindChild("LifeBar").FindChild("PV").GetComponent<MeshRenderer>().enabled = true ;
		t.FindChild("LifeBar").FindChild("PVValue").GetComponent<MeshRenderer>().enabled = true ;
		t.FindChild("Icon0").GetComponent<SpriteRenderer>().enabled = true ;
		t.FindChild("Icon1").GetComponent<SpriteRenderer>().enabled = true ;
		t.FindChild("Icon2").GetComponent<SpriteRenderer>().enabled = true ;
		t.FindChild("Icon3").GetComponent<SpriteRenderer>().enabled = true ;
		t.Find("AttackZone").FindChild("AttackValue").GetComponent<MeshRenderer>().enabled = true ;
		
		gameObject.GetComponent<BoxCollider>().enabled = true ;
	}
	
	public void setCard(Card c, int d)
	{
		this.card = c ;
		transform.Find("AttackZone").FindChild("AttackValue").GetComponent<TextMeshPro>().text = c.GetAttackString();
		transform.Find("LifeBar").GetComponent<SpriteRenderer>().sprite = this.lifebarSprites[0];
		transform.FindChild("LifeBar").FindChild("PVValue").GetComponent<TextMeshPro>().text = c.GetLife()+"/"+c.Life;
		
		transform.Find("Life").GetComponent<SpriteRenderer>().sprite = this.lifebarSprites[1];
		transform.Find("Icon0").GetComponent<SpriteRenderer>().sprite = this.iconSprites[0];
		transform.Find("Icon1").GetComponent<SpriteRenderer>().sprite = this.iconSprites[0];
		transform.Find("Icon2").GetComponent<SpriteRenderer>().sprite = this.iconSprites[0];
		transform.Find("Icon3").GetComponent<SpriteRenderer>().sprite = this.iconSprites[0];
		
		if (this.isMine){
			transform.Find("Art").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[c.ArtIndex];
		}
		else{
			transform.Find("Art").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[c.ArtIndex+10];
		}
	}
	
	public void addTime(float t){
		this.timerSelection += t ;
		if (this.timerSelection>this.selectionTime){
			this.isGettingBigger = !this.isGettingBigger ;
			this.timerSelection = 0f ;
		}
		if (this.isGettingBigger){
			float f = 1f + 0.2f * (this.timerSelection/this.selectionTime);
			gameObject.transform.localScale = new Vector3(f, f, f) ;
		}
		else {
			float f = 1.2f - 0.2f * (this.timerSelection/this.selectionTime);
			gameObject.transform.localScale = new Vector3(f, f, f) ;
		}
	}
	
	public void moveForward(){
		Transform t = gameObject.transform;
		t.Find("AttackZone").FindChild("AttackPicto").GetComponent<SpriteRenderer>().sortingOrder = 12 ;
		t.FindChild("Art").GetComponent<SpriteRenderer>().sortingOrder = 10 ;
		t.FindChild("Life").GetComponent<SpriteRenderer>().sortingOrder = 12 ;
		t.FindChild("LifeBar").GetComponent<SpriteRenderer>().sortingOrder = 11 ;
		t.FindChild("LifeBar").FindChild("PV").GetComponent<MeshRenderer>().sortingOrder = 14 ;
		t.FindChild("LifeBar").FindChild("PVValue").GetComponent<MeshRenderer>().sortingOrder = 14 ;
		
		t.FindChild("Icon0").GetComponent<SpriteRenderer>().sortingOrder = 11 ;
		t.FindChild("Icon1").GetComponent<SpriteRenderer>().sortingOrder = 11 ;
		t.FindChild("Icon2").GetComponent<SpriteRenderer>().sortingOrder = 11 ;
		t.FindChild("Icon3").GetComponent<SpriteRenderer>().sortingOrder = 11 ;
		t.Find("AttackZone").FindChild("AttackValue").GetComponent<MeshRenderer>().sortingOrder = 14 ;
		
	}
	
	public void resetTimer(){
		this.timerSelection = 0 ;
		this.isGettingBigger = true ;
		gameObject.transform.localScale = new Vector3(1, 1, 1) ;
		
		Transform t = gameObject.transform;
		t.Find("AttackZone").FindChild("AttackPicto").GetComponent<SpriteRenderer>().sortingOrder = 2 ;
		t.FindChild("Art").GetComponent<SpriteRenderer>().sortingOrder = 1 ;
		t.FindChild("Life").GetComponent<SpriteRenderer>().sortingOrder = 3 ;
		t.FindChild("LifeBar").GetComponent<SpriteRenderer>().sortingOrder = 2 ;
		t.FindChild("LifeBar").FindChild("PV").GetComponent<MeshRenderer>().sortingOrder = 4 ;
		t.FindChild("LifeBar").FindChild("PVValue").GetComponent<MeshRenderer>().sortingOrder = 4 ;
		
		t.FindChild("Icon0").GetComponent<SpriteRenderer>().sortingOrder = 2 ;
		t.FindChild("Icon1").GetComponent<SpriteRenderer>().sortingOrder = 2 ;
		t.FindChild("Icon2").GetComponent<SpriteRenderer>().sortingOrder = 2 ;
		t.FindChild("Icon3").GetComponent<SpriteRenderer>().sortingOrder = 2 ;
		t.Find("AttackZone").FindChild("AttackValue").GetComponent<MeshRenderer>().sortingOrder = 4 ;
	}
	
	
	public void setIDCharacter(int i)
	{
		this.id = i ;
	}
	
	public Tile getTile()
	{
		return this.tile ;
	}
	
	public bool getIsMine()
	{
		return this.isMine ;
	}
	
	public bool getHasPlayed()
	{
		return this.hasPlayed ;
	}
	
	public bool getHasMoved()
	{
		return this.hasMoved ;
	}
	
	public bool getIsDead()
	{
		return this.isDead ;
	}
	
	public void play(bool b)
	{
		this.hasPlayed = b ;
	}
	
	public void move(bool b)
	{
		this.hasMoved = b ;
	}
	
	public Card getCard()
	{
		return this.card ;
	}
	
	public void setIsMine(bool b)
	{
		this.isMine = b ;
	}
	
	public void OnMouseEnter(){
		GameView.instance.hoverTile(this.id, this.tile, true);
	}
	
	public void OnMouseDown(){
		GameController.instance.clickPlayingCard(this.id, this.tile);
		if(GameView.instance.getIsTutorialLaunched())
		{
			TutorialObjectController.instance.actionIsDone();
		}
	}

	public void setActive(bool b)
	{
		gameObject.SetActive(b);
	}

	public void show(bool showTR)
	{
		float percentage = 1.0f*this.card.GetLife()/this.card.GetTotalLife();
		
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
		
		int compteurIcones = 0;
		
		for (int i = 0; i < this.card.modifiers.Count && compteurIcones < 4; i++)
		{
			if (this.card.modifiers [i].idIcon > -1)
			{
				gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<SpriteRenderer>().sprite = this.iconSprites[this.card.modifiers [i].idIcon];
				gameObject.transform.FindChild("Icon"+compteurIcones).GetComponent<IconController>().setInformation(this.card.modifiers [i].title, this.card.modifiers [i].description, this.card.modifiers [i].additionnalInfo);
				compteurIcones++;
			}
		}
		
		for (int i = compteurIcones; i < 4; i++)
		{
			gameObject.transform.FindChild("Icon"+i).GetComponent<SpriteRenderer>().sprite = this.iconSprites[0];
			gameObject.transform.FindChild("Icon"+i).GetComponent<IconController>().resetInformation();
		}
		
		this.showTR(showTR);
	}

	public void setTile(Tile t, Vector3 p)
	{
		this.tile = t;
		p.z = -0.5f;
		gameObject.transform.position = p ;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		this.hasMoved = true ;
	}

	public void changeTile(Tile t, Vector3 position)
	{
		this.tile = t;
		position.z = -5;
		//this.playingCardView.playingCardVM.position = position;
		//this.playingCardView.replace();
	}

	public void updateAttack()
	{
		int attackBase = this.card.Attack ;
		int attack = this.card.GetAttack();
		if(attackBase>attack){
			gameObject.transform.FindChild("AttackZone").FindChild("AttackValue").GetComponent<TextMeshPro>().color = Color.red;
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
		int life = this.card.GetLife();
		gameObject.transform.FindChild("LifeBar").FindChild("PVValue").GetComponent<TextMeshPro>().text = life+"/"+lifeBase;
		if (lifeBase>this.card.Life){
			gameObject.transform.FindChild("LifeBar").FindChild("PVValue").GetComponent<TextMeshPro>().color = Color.green;
		}
		else if(lifeBase<this.card.Life){
			gameObject.transform.FindChild("LifeBar").FindChild("PVValue").GetComponent<TextMeshPro>().color = Color.red;
		}
		else{
			gameObject.transform.FindChild("LifeBar").FindChild("PVValue").GetComponent<TextMeshPro>().color = Color.white;
		}
	}
	
	public void kill()
	{
		this.isDead = true;
		this.hasPlayed = true;
		this.hasMoved = true;
	}
	
	public void disappear(){
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers [i].GetComponent<Renderer>().enabled = false;
		}
		gameObject.transform.position = new Vector3(-20, -20, -20);
	}

	public void relive()
	{
		this.isDead = false;
		gameObject.GetComponent<Renderer>().enabled = true;
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers [i].GetComponent<Renderer>().enabled = true;
		}
	}
	
	public void checkModyfiers()
	{
		List<int> modifiersToSuppress = new List<int>();
		List<int> tileModifiersToSuppress = new List<int>();
		
		for (int i = this.card.modifiers.Count-1; i >= 0; i--)
		{
			if (this.card.modifiers [i].Duration > 0)
			{
				this.card.modifiers [i].Duration--;
			}
			
			if (this.card.modifiers [i].Duration == 0)
			{
				modifiersToSuppress.Add(i);
				if (this.card.modifiers [i].Type == ModifierType.Type_Paralized)
				{
					this.card.modifiers.RemoveAt(i);
				} 
			} else if (this.card.modifiers [i].Duration > 0)
			{
				this.card.modifiers [i].additionnalInfo = "Actif " + this.card.modifiers [i].Duration + " tour";
				if (this.card.modifiers [i].Duration > 1)
				{
					this.card.modifiers [i].additionnalInfo += "s";
				}
			}
		}
		
		for (int i = 0; i < modifiersToSuppress.Count; i++)
		{
			this.card.modifiers.RemoveAt(modifiersToSuppress [i]);
		}

		for (int i = 0; i < this.card.TileModifiers.Count; i++)
		{
			if (this.card.TileModifiers [i].Duration > 0)
			{
				this.card.TileModifiers [i].Duration--;
			}
			
			if (this.card.TileModifiers [i].Duration == 0)
			{
				tileModifiersToSuppress.Add(i);
				if (this.card.TileModifiers [i].Stat == ModifierStat.Stat_Attack || this.card.TileModifiers [i].Stat == ModifierStat.Stat_Move)
				{
					this.show(true);
				}
			}
		}
		for (int i = 0; i < tileModifiersToSuppress.Count; i++)
		{
			this.card.TileModifiers.RemoveAt(tileModifiersToSuppress [i]);
		}
		this.show(true);
	}
	
	public void activateSleepingModifiers()
	{
		for (int i = 0; i < this.card.modifiers.Count; i++)
		{
			if (this.card.modifiers [i].Duration == -10)
			{
				this.card.modifiers [i].Duration = 1;	
			}
		}
	}
	
	public bool canBeTargeted()
	{
		return (!this.isDead && !this.card.isIntouchable());
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
}


