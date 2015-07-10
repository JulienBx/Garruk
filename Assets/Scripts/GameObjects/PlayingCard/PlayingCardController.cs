using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PlayingCardController : GameObjectController
{
	public Sprite[] backgroundSprites;
	public Sprite[] gradeSprites;
	public Sprite[] lifebarSprites;
	public Sprite[] artSprites;
	public Sprite[] iconSprites;
	 
	Card card ;
	int id = -1 ;
	bool isDead ;
	bool isMine;
	
	bool hasMoved ;
	bool hasPlayed ;

	Tile tile ;

	private bool isDisabled; // variable pour le tutoriel

	void Awake()
	{
		this.isDisabled = false;
		this.isDead = false;
	}
	
	public void hide()
	{
		gameObject.GetComponent<SpriteRenderer>().enabled = false ;
	}
	
	public void display()
	{
		gameObject.GetComponent<SpriteRenderer>().enabled = true ;
	}
	
	public void setCard(Card c, int d)
	{
		this.card = c ;
		transform.Find("Grade").GetComponent<SpriteRenderer>().sprite = this.gradeSprites[d];
		transform.Find("AttackValue").GetComponent<TextMeshPro>().text = c.GetAttackString();
		transform.Find("AttackValue").GetComponent<Renderer>().sortingLayerName = "Foreground" ;
		
		transform.Find("LifeBar").GetComponent<SpriteRenderer>().sprite = this.lifebarSprites[0];
		transform.Find("Life").GetComponent<SpriteRenderer>().sprite = this.lifebarSprites[1];
		transform.Find("Icon1").GetComponent<SpriteRenderer>().sprite = this.iconSprites[0];
		transform.Find("Icon2").GetComponent<SpriteRenderer>().sprite = this.iconSprites[0];
		transform.Find("Icon3").GetComponent<SpriteRenderer>().sprite = this.iconSprites[0];
		transform.Find("Icon4").GetComponent<SpriteRenderer>().sprite = this.iconSprites[0];
		transform.Find("Icon5").GetComponent<SpriteRenderer>().sprite = this.iconSprites[0];
		transform.Find("Icon6").GetComponent<SpriteRenderer>().sprite = this.iconSprites[0];
		transform.Find("Art").GetComponent<SpriteRenderer>().sprite = this.artSprites[c.ArtIndex];
	}
	
	public void setIDCharacter(int i)
	{
		this.id = i ;
	}
	
	public Tile getTile()
	{
		return this.tile ;
	}
	
	public Card getCard()
	{
		return this.card ;
	}
	
	public void setIsMine(bool b)
	{
		this.isMine = b ;
		if (b){
			gameObject.GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[0];
		}
		else{
			gameObject.GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[0];
		}
	}
	
	public void OnMouseEnter(){
		GameView.instance.hoverTile(this.id, this.tile);
	}
	
	public void OnMouseDown(){
		GameController.instance.clickPlayingCard(this.id, this.tile);
	}
	
	public void resizeHalo()
	{
//		int height = Screen.height;
//		int width = Screen.width;
//
//		int decalage = height / 15;
//
//		Vector3 positionObject = new Vector3(0, 0, 0);
//		positionObject.x = (this.playingCardView.playingCardVM.position.x - this.playingCardView.playingCardVM.scale.x / 2.2f) * (height / 10f) + (width / 2f);
//		positionObject.y = height - ((this.playingCardView.playingCardVM.position.y + this.playingCardView.playingCardVM.scale.y / 2.2f) * (height / 10f) + (height / 2f));
//
//		Rect position = new Rect(positionObject.x, positionObject.y, this.playingCardView.playingCardVM.scale.x * height / 11, this.playingCardView.playingCardVM.scale.x * height / 11);
//		this.playingCardView.playingCardVM.haloRect = position;
//		
//		for (int i = 0; i < this.haloTextStyles.Length; i++)
//		{
//			this.haloTextStyles [i].fontSize = height * 15 / 1000;
//		}
	}

	public void resizeDeadHalo(Vector3 localScale, int i)
	{
		Vector3 min = Utils.getGOScreenPosition(gameObject.GetComponent<Renderer>().bounds.min);
		Vector3 max = Utils.getGOScreenPosition(gameObject.GetComponent<Renderer>().bounds.max);
		Vector3 positionScale = Utils.getGOScreenPosition(localScale);
		i++;
		Rect position = new Rect(min.x, (max.y - min.y) * i, max.x - min.x, max.y - min.y);
		Debug.Log("position mort : " + min.x + " " + positionScale.y + " " + (max.x - min.x) + " " + (max.y - min.y) + " " + i);
		//this.playingCardView.playingCardVM.haloRect = position;
	}

	public void removeTargetHalo()
	{
		//this.playingCardView.playingCardVM.toDisplayHalo = false;
	}
	
	public void addSkillResult(string s, float timer, int colorIndex)
	{
//		this.playingCardView.playingCardVM.skillResult = new SkillResult(s, styles [4 + colorIndex]);
//		this.playingCardView.playingCardVM.skillResultTimer = timer;
//		
//		this.playingCardView.playingCardVM.toDisplaySkillResult = true;
	}
	
	public void setSkillControlSkillHandler(string s)
	{
		//this.playingCardView.playingCardVM.skillControlHandler = new SkillControlHandler(s, styles [6]);
		//this.playingCardView.playingCardVM.toDisplaySkillControlHandler = true;
	}
	
	public void removeSkillResult()
	{
		//this.playingCardView.playingCardVM.toDisplaySkillResult = false;
	}

	public void setStyles(bool isMyCharacter)
	{
		isMine = isMyCharacter;
	}

	public void setActive(bool b)
	{
		gameObject.SetActive(b);
	}

	public void setControlActive(bool b)
	{
		//this.playingCardView.playingCardVM.isActive = b;
	}

//	public void setCard(Card c)
//	{
//		this.card = c;
//		//playingCardView.playingCardVM.face = this.faces [c.ArtIndex];
//		//playingCardView.playingCardVM.attack = c.Attack.ToString();
//		//playingCardView.playingCardVM.move = c.Move.ToString();
//	}
	public void show()
	{
		if (isMine)
		{
			//playingCardView.playingCardVM.lifeGauge = this.lifeGauges [0];
		} else
		{
			//playingCardView.playingCardVM.lifeGauge = this.lifeGauges [1];
		}
		base.setGOCoordinates(gameObject);
		this.setTextResolution();
		this.updateAttack();
		this.updateLifeCount();
		this.updateLife();
		
		int compteurIcones = 0;
		//this.playingCardView.playingCardVM.icons = new List<Texture2D>();
		//this.playingCardView.playingCardVM.titlesIcon = new List<string>();
		//this.playingCardView.playingCardVM.descriptionIcon = new List<string>();
		//this.playingCardView.playingCardVM.additionnalInfoIcon = new List<string>();
		
		for (int i = 0; i < this.card.modifiers.Count && compteurIcones < 3; i++)
		{
			if (this.card.modifiers [i].idIcon != -1)
			{
//				this.playingCardView.playingCardVM.icons.Add(this.icons [this.card.modifiers [i].idIcon]);
//				this.playingCardView.playingCardVM.titlesIcon.Add(this.card.modifiers [i].title);
//				this.playingCardView.playingCardVM.descriptionIcon.Add(this.card.modifiers [i].description);
//				this.playingCardView.playingCardVM.additionnalInfoIcon.Add(this.card.modifiers [i].additionnalInfo);
//				compteurIcones++;
			}
		}
		
		//playingCardView.show();
	}

	public void setTextResolution()
	{
		float resolution = base.GOSize.y / 150f;
		//playingCardView.setTextResolution(resolution);
	}

	public void setPosition(Vector3 p, Vector3 s)
	{
//		this.playingCardView.playingCardVM.position = p;
//		this.playingCardView.playingCardVM.scale = s;
//		this.playingCardView.replace();
	}
	
	public void setPosition(Vector3 p)
	{
//		this.playingCardView.playingCardVM.position = p;
//		this.playingCardView.replace();
	}

	public void setTile(Tile t, Vector3 p)
	{
		this.tile = t;
		p.z = -0.5f;
		gameObject.transform.position = p ;
		gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
		this.hasMoved = true ;
	}

	public void changeTile(Tile t, Vector3 position)
	{
		this.tile = t;
		position.z = -5;
		//this.playingCardView.playingCardVM.position = position;
		this.resize();
		//this.playingCardView.replace();
	}

	public void resize()
	{
		this.resizeHalo();
		this.resizeIcons();
		int h = Screen.height;
//		styles [4].fontSize = 15 * h / 1000;
//		styles [5].fontSize = 15 * h / 1000;
//		styles [6].fontSize = 15 * h / 1000;
	}

	public void updateLife()
	{
		int life = this.card.GetLife();
		int maxLife = this.card.Life;
		float percentage = 1.0f * life / maxLife;
		//playingCardView.drawLifeGauge(percentage);
	}

	public void updateAttack()
	{
		//this.playingCardView.playingCardVM.attack = this.card.GetAttack().ToString();
	}

	public void updateLifeCount()
	{
		//this.playingCardView.playingCardVM.move = this.card.GetLife().ToString();
	}

	public void hoverPlayingCard()
	{
		//GameController.instance.hoverPlayingCardHandler(this.IDCharacter);
	}

	public void clickPlayingCard()
	{
		//GameController.instance.clickPlayingCardHandler(this.IDCharacter);
	}

	public void addTarget()
	{
		if(!this.isDisabled)
		{
			//this.playingCardView.playingCardVM.toDisplayHalo = false;
			//GameController.instance.targetPCCHandler.addTargetPCC(this.IDCharacter);
		}
	}

	public void releaseClickPlayingCard()
	{
		//GameController.instance.releaseClickPlayingCardHandler(this.IDCharacter);
	}

	public void displayHover()
	{
//		this.playingCardView.playingCardVM.position.z = -3;
//		this.playingCardView.replace();
//		this.playingCardView.playingCardVM.border = this.borderPC [1];
//		this.playingCardView.changeBorder();
	}

	public void displaySelected()
	{
//		this.playingCardView.playingCardVM.position.z = -4;
//		this.playingCardView.replace();
//		this.playingCardView.playingCardVM.border = this.borderPC [4];
//		this.playingCardView.changeBorder();
	}

	public void displayOpponentSelected()
	{
//		this.playingCardView.playingCardVM.position.z = -4;
//		this.playingCardView.replace();
//		this.playingCardView.playingCardVM.border = this.borderPC [2];
//		this.playingCardView.changeBorder();
	}

	public void displayPlaying()
	{
//		this.playingCardView.playingCardVM.position.z = -5;
//		this.playingCardView.replace();
//		this.playingCardView.playingCardVM.border = this.borderPC [3];
//		this.playingCardView.playingCardVM.isPlaying = true;
//		this.playingCardView.changeBorder();
	}

	public void hideHover()
	{
//		this.playingCardView.playingCardVM.position.z = -2;
//		this.playingCardView.replace();
//		this.playingCardView.playingCardVM.border = this.borderPC [0];
//		this.playingCardView.changeBorder();
	}

	public void hideSelected()
	{
//		this.playingCardView.playingCardVM.position.z = -2;
//		this.playingCardView.replace();
//		this.playingCardView.playingCardVM.border = this.borderPC [0];
//		this.playingCardView.changeBorder();
	}

	public Texture getPicture()
	{
		//return playingCardView.playingCardVM.face;
		return null;
	}

	public void hidePlaying()
	{
//		this.playingCardView.playingCardVM.position.z = -2;
//		this.playingCardView.replace();
//		this.playingCardView.playingCardVM.border = this.borderPC [0];
//		this.playingCardView.changeBorder();
	}
	
	public void changeEsquive(string title, string description, string additionnalInfo)
	{
		//this.card.modifiers.Add(new StatModifier(ModifierType.Type_EsquivePercentage, -1, 1, title, description, additionnalInfo));
	}

	public void resizeIcons()
	{
//		int height = Screen.height;
//		int width = Screen.width;
//		this.playingCardView.playingCardVM.iconsRect = new List<Rect>();
//		
//		for (int i = 0; i <= 2; i++)
//		{
//			Vector3 positionObject = new Vector3(0, 0, 0);
//			positionObject.x = (this.playingCardView.playingCardVM.position.x - this.playingCardView.playingCardVM.scale.x / 2f + (this.playingCardView.playingCardVM.scale.x * 4 / 100) + (i * this.playingCardView.playingCardVM.scale.x * 32 / 100)) * (height / 10f) + (width / 2f);
//			positionObject.y = height - ((this.playingCardView.playingCardVM.position.y + this.playingCardView.playingCardVM.scale.y / 5f) * (height / 10f) + (height / 2f));
//
//			Rect position = new Rect(positionObject.x, positionObject.y, (this.playingCardView.playingCardVM.scale.x * 28 / 100) * (height / 10f), (this.playingCardView.playingCardVM.scale.x * 28 / 100) * (height / 10f));
//			this.playingCardView.playingCardVM.iconsRect.Add(position);
//		}
//		
//		this.playingCardView.playingCardVM.descriptionStyle.fontSize = height * 15 / 1000;
//		this.playingCardView.playingCardVM.additionnalInfoStyle.fontSize = height * 15 / 1000;
//		this.playingCardView.playingCardVM.titleStyle.fontSize = height * 20 / 1000;
	}
	
	public void kill()
	{
		//GameController.instance.displaySkillEffect(this.IDCharacter, "DEAD", 6, 1);
//		if (GameController.instance.currentPlayingCard==this.IDCharacter){
//			GameController.instance.resolvePass();
//		}
//		else{
//			GameController.instance.playingCards [GameController.instance.currentPlayingCard].GetComponentInChildren<PlayingCardController>().tile.setNeighbours(GameController.instance.getCharacterTilesArray(), GameController.instance.playingCards [GameController.instance.currentPlayingCard].GetComponentInChildren<PlayingCardController>().card.GetMove());
//			GameController.instance.setDestinations(GameController.instance.currentPlayingCard);
//		}
		
		this.isDead = true;
		this.hasPlayed = true;
		gameObject.GetComponent<Renderer>().enabled = false;
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers [i].GetComponent<Renderer>().enabled = false;
		}
		GameController.instance.emptyTile(this.tile.x, this.tile.y);
		this.setPosition(new Vector3(-20, -20, -20));
		this.resizeIcons();
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
					this.show();
				}
			}
		}
		for (int i = 0; i < tileModifiersToSuppress.Count; i++)
		{
			this.card.TileModifiers.RemoveAt(tileModifiersToSuppress [i]);
		}
		this.show();
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
	
	public void setTargetHalo(HaloTarget h, bool isDisabled=false)
	{
//		this.playingCardView.playingCardVM.haloStyle.normal.background = this.halos [h.idImage];
//		this.playingCardView.playingCardVM.haloTexts = new List<string>();
//		this.playingCardView.playingCardVM.haloStyles = new List<GUIStyle>();
//		
//		for (int i = 0; i < h.textsToDisplay.Count; i++)
//		{
//			this.playingCardView.playingCardVM.haloTexts.Add(h.textsToDisplay [i]);
//			this.playingCardView.playingCardVM.haloStyles.Add(this.haloTextStyles [h.stylesID [i]]);
//		}
//		this.playingCardView.playingCardVM.toDisplayHalo = true;
//		if(isDisabled)
//		{
//			this.isDisabled=true;
//		}
	}
	
	public void hideTargetHalo()
	{
		//this.playingCardView.playingCardVM.toDisplayHalo = false;
		this.isDisabled = false;
	}
	
	public void cancelSkill()
	{
		this.hideControlSkillHandler();
		GameController.instance.cancelSkill();
	}
	
	public void hideControlSkillHandler()
	{
		//this.playingCardView.playingCardVM.toDisplaySkillControlHandler = false;
	}
	
	public void showDisplay()
	{
		gameObject.GetComponent<Renderer>().enabled = true ;
	}
	public void setIsDisable(bool value)
	{
		this.isDisabled = value;
	}
}


