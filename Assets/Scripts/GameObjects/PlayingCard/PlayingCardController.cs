using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayingCardController : GameObjectController
{
	private PlayingCardView playingCardView;
	public ScriptableObject[] skills;
	public Texture2D[] borderPC ;
	public Texture[] faces;
	public Texture[] lifeGauges;
	private float scale ;
	public Card card ;
	public int IDCharacter = -1 ;
	public int sortID = -1 ;
	public bool isMovable;
	public int damage = 0;
	public bool isDead ;
	public bool isSelected ;
	public bool isMoved ;
	public bool hasPlayed ;
	public bool isMine;
	public int cannotBeTargeted;
	public int paralyzed;

	public Texture2D[] icons ;
	public Texture2D[] halos ;
	public GUIStyle[] haloTextStyles ;

	public Tile tile ;

	public GUIStyle[] styles ;

	void Awake()
	{
		this.playingCardView = gameObject.AddComponent <PlayingCardView>();
		this.isMovable = true;
		this.isDead = false;
		this.isSelected = false;
		this.isMoved = false;
		this.cannotBeTargeted = -1;
		this.paralyzed = -1;
		this.playingCardView.playingCardVM.iconStyle = styles [0];
		this.playingCardView.playingCardVM.titleStyle = styles [1];
		this.playingCardView.playingCardVM.descriptionStyle = styles [2];
		this.playingCardView.playingCardVM.descriptionRectStyle = styles [3];
		this.playingCardView.playingCardVM.additionnalInfoStyle = styles [7];
		this.playingCardView.playingCardVM.haloStyle = styles [8];
	}

	public void activateTargetHalo()
	{
		this.playingCardView.playingCardVM.toDisplayHalo = true;
		//this.playingCardView.playingCardVM.halo = this.halos [0];
	}

	public void resizeHalo()
	{
		int height = Screen.height;
		int width = Screen.width;

		int decalage = height / 15;

		Vector3 positionObject = new Vector3(0, 0, 0);
		positionObject.x = (this.playingCardView.playingCardVM.position.x - this.playingCardView.playingCardVM.scale.x / 2.2f) * (height / 10f) + (width / 2f);
		positionObject.y = height - ((this.playingCardView.playingCardVM.position.y + this.playingCardView.playingCardVM.scale.y / 2.2f) * (height / 10f) + (height / 2f));

		Rect position = new Rect(positionObject.x, positionObject.y, this.playingCardView.playingCardVM.scale.x * height / 11, this.playingCardView.playingCardVM.scale.x * height / 11);
		this.playingCardView.playingCardVM.haloRect = position;
		
		for (int i = 0; i < this.haloTextStyles.Length; i++)
		{
			this.haloTextStyles [i].fontSize = height * 15 / 1000;
		}
	}

	public void resizeDeadHalo(Vector3 localScale, int i)
	{
		Vector3 min = Utils.getGOScreenPosition(gameObject.renderer.bounds.min);
		Vector3 max = Utils.getGOScreenPosition(gameObject.renderer.bounds.max);
		Vector3 positionScale = Utils.getGOScreenPosition(localScale);
		i++;
		Rect position = new Rect(min.x, (max.y - min.y) * i, max.x - min.x, max.y - min.y);
		Debug.Log("position mort : " + min.x + " " + positionScale.y + " " + (max.x - min.x) + " " + (max.y - min.y) + " " + i);
		this.playingCardView.playingCardVM.haloRect = position;
	}

	public void removeTargetHalo()
	{
		this.playingCardView.playingCardVM.toDisplayHalo = false;
	}
	
	public void addSkillResult(string s, float timer, int colorIndex)
	{
		this.playingCardView.playingCardVM.skillResult = new SkillResult(s, styles [4 + colorIndex]);
		this.playingCardView.playingCardVM.skillResultTimer = timer;
		
		this.playingCardView.playingCardVM.toDisplaySkillResult = true;
	}
	
	public void setSkillControlSkillHandler(string s)
	{
		this.playingCardView.playingCardVM.skillControlHandler = new SkillControlHandler(s, styles [6]);
		this.playingCardView.playingCardVM.toDisplaySkillControlHandler = true;
	}
	
	public void removeSkillResult()
	{
		this.playingCardView.playingCardVM.toDisplaySkillResult = false;
	}

	public void setStyles(bool isMyCharacter)
	{
		isMine = isMyCharacter;
		if (isMyCharacter)
		{

		} else
		{

		}
	}

	public void setTile(Tile t)
	{
		this.tile = t;
	}

	public void setActive(bool b)
	{
		gameObject.SetActive(b);
	}

	public void setControlActive(bool b)
	{
		this.playingCardView.playingCardVM.isActive = b;
	}

	public void setCard(Card c)
	{
		this.card = c;
		playingCardView.playingCardVM.face = this.faces [c.ArtIndex];
		playingCardView.playingCardVM.attack = c.Attack.ToString();
		playingCardView.playingCardVM.move = c.Move.ToString();
	}
	public void show()
	{
		if (isMine)
		{
			playingCardView.playingCardVM.lifeGauge = this.lifeGauges [0];
		} else
		{
			playingCardView.playingCardVM.lifeGauge = this.lifeGauges [1];
		}
		base.getGOCoordinates(gameObject);
		this.setTextResolution();
		this.updateAttack();
		this.updateLifeCount();
		this.updateLife();
		
		int compteurIcones = 0;
		this.playingCardView.playingCardVM.icons = new List<Texture2D>();
		this.playingCardView.playingCardVM.titlesIcon = new List<string>();
		this.playingCardView.playingCardVM.descriptionIcon = new List<string>();
		this.playingCardView.playingCardVM.additionnalInfoIcon = new List<string>();
		
		for (int i = 0; i < this.card.modifiers.Count && compteurIcones < 3; i++)
		{
			if (this.card.modifiers [i].idIcon != -1)
			{
				this.playingCardView.playingCardVM.icons.Add(this.icons [this.card.modifiers [i].idIcon]);
				this.playingCardView.playingCardVM.titlesIcon.Add(this.card.modifiers [i].title);
				this.playingCardView.playingCardVM.descriptionIcon.Add(this.card.modifiers [i].description);
				this.playingCardView.playingCardVM.additionnalInfoIcon.Add(this.card.modifiers [i].additionnalInfo);
				compteurIcones++;
			}
		}
		
		playingCardView.show();
	}

	public void setTextResolution()
	{
		float resolution = base.GOSize.y / 150f;
		playingCardView.setTextResolution(resolution);
	}

	public void setPosition(Vector3 p, Vector3 s)
	{
		this.playingCardView.playingCardVM.position = p;
		this.playingCardView.playingCardVM.scale = s;
		this.playingCardView.replace();
	}
	
	public void setPosition(Vector3 p)
	{
		this.playingCardView.playingCardVM.position = p;
		this.playingCardView.replace();
	}

	public void setTile(Tile t, Vector3 p, bool toRotate)
	{
		this.tile = t;
		p.z = -2;
		this.playingCardView.playingCardVM.position = p;
		this.playingCardView.playingCardVM.scale = new Vector3(1.10f, 1.10f, 1.10f);
		this.playingCardView.replace();
	}

	public void changeTile(Tile t, Vector3 position)
	{
		this.tile = t;
		position.z = -5;
		this.playingCardView.playingCardVM.position = position;
		this.resize();
		this.playingCardView.replace();
	}

	public void resize()
	{
		this.resizeHalo();
		this.resizeIcons();
		int h = Screen.height;
		styles [4].fontSize = 15 * h / 1000;
		styles [5].fontSize = 15 * h / 1000;
		styles [6].fontSize = 15 * h / 1000;
	}

	public void setIDCharacter(int i)
	{
		this.IDCharacter = i;
	}

	public void updateLife()
	{
		int life = this.card.GetLife();
		int maxLife = this.card.Life;
		float percentage = 1.0f * life / maxLife;
		playingCardView.drawLifeGauge(percentage);
	}

	public void updateAttack()
	{
		this.playingCardView.playingCardVM.attack = this.card.GetAttack().ToString();
	}

	public void updateLifeCount()
	{
		this.playingCardView.playingCardVM.move = this.card.GetLife().ToString();
	}

	public void hoverPlayingCard()
	{
		GameController.instance.hoverPlayingCardHandler(this.IDCharacter);
	}

	public void clickPlayingCard()
	{
		GameController.instance.clickPlayingCardHandler(this.IDCharacter);
	}

	public void addTarget()
	{
		this.playingCardView.playingCardVM.toDisplayHalo = false;
		GameController.instance.targetPCCHandler.addTargetPCC(this.IDCharacter);
	}

	public void releaseClickPlayingCard()
	{
		GameController.instance.releaseClickPlayingCardHandler(this.IDCharacter);
	}

	public void displayHover()
	{
		this.playingCardView.playingCardVM.position.z = -3;
		this.playingCardView.replace();
		this.playingCardView.playingCardVM.border = this.borderPC [1];
		this.playingCardView.changeBorder();
	}

	public void displaySelected()
	{
		this.playingCardView.playingCardVM.position.z = -4;
		this.playingCardView.replace();
		this.playingCardView.playingCardVM.border = this.borderPC [4];
		this.playingCardView.changeBorder();
	}

	public void displayOpponentSelected()
	{
		this.playingCardView.playingCardVM.position.z = -4;
		this.playingCardView.replace();
		this.playingCardView.playingCardVM.border = this.borderPC [2];
		this.playingCardView.changeBorder();
	}

	public void displayPlaying()
	{
		this.playingCardView.playingCardVM.position.z = -5;
		this.playingCardView.replace();
		this.playingCardView.playingCardVM.border = this.borderPC [3];
		this.playingCardView.playingCardVM.isPlaying = true;
		this.playingCardView.changeBorder();
	}

	public void hideHover()
	{
		this.playingCardView.playingCardVM.position.z = -2;
		this.playingCardView.replace();
		this.playingCardView.playingCardVM.border = this.borderPC [0];
		this.playingCardView.changeBorder();
	}

	public void hideSelected()
	{
		this.playingCardView.playingCardVM.position.z = -2;
		this.playingCardView.replace();
		this.playingCardView.playingCardVM.border = this.borderPC [0];
		this.playingCardView.changeBorder();
	}

	public Texture getPicture()
	{
		return playingCardView.playingCardVM.face;
	}

	public void hidePlaying()
	{
		this.playingCardView.playingCardVM.position.z = -2;
		this.playingCardView.replace();
		this.playingCardView.playingCardVM.border = this.borderPC [0];
		this.playingCardView.changeBorder();
	}
	
	public void changeEsquive(string title, string description, string additionnalInfo)
	{
		//this.card.modifiers.Add(new StatModifier(ModifierType.Type_EsquivePercentage, -1, 1, title, description, additionnalInfo));
	}

	public void resizeIcons()
	{
		int height = Screen.height;
		int width = Screen.width;
		this.playingCardView.playingCardVM.iconsRect = new List<Rect>();
		
		for (int i = 0; i <= 2; i++)
		{
			Vector3 positionObject = new Vector3(0, 0, 0);
			positionObject.x = (this.playingCardView.playingCardVM.position.x - this.playingCardView.playingCardVM.scale.x / 2f + (this.playingCardView.playingCardVM.scale.x * 4 / 100) + (i * this.playingCardView.playingCardVM.scale.x * 32 / 100)) * (height / 10f) + (width / 2f);
			positionObject.y = height - ((this.playingCardView.playingCardVM.position.y + this.playingCardView.playingCardVM.scale.y / 5f) * (height / 10f) + (height / 2f));

			Rect position = new Rect(positionObject.x, positionObject.y, (this.playingCardView.playingCardVM.scale.x * 28 / 100) * (height / 10f), (this.playingCardView.playingCardVM.scale.x * 28 / 100) * (height / 10f));
			this.playingCardView.playingCardVM.iconsRect.Add(position);
		}
		
		this.playingCardView.playingCardVM.descriptionStyle.fontSize = height * 15 / 1000;
		this.playingCardView.playingCardVM.additionnalInfoStyle.fontSize = height * 15 / 1000;
		this.playingCardView.playingCardVM.titleStyle.fontSize = height * 20 / 1000;
	}
	
	public void kill()
	{
		GameController.instance.displayPopUpMessage(GameController.instance.getCard(this.IDCharacter).Title + " est mort", 3);
		this.isDead = true;
		this.hasPlayed = true;
		gameObject.renderer.enabled = false;
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers [i].renderer.enabled = false;
		}
		GameController.instance.emptyTile(this.tile.x, this.tile.y);
		this.setPosition(new Vector3(-20, -20, -20));
		this.resizeIcons();
		this.tile = null;
	}

	public void relive()
	{
		this.isDead = false;
		gameObject.renderer.enabled = true;
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers [i].renderer.enabled = true;
		}
	}

	public void removeFurtivity()
	{
		print("REMOVEFurtiv");
		this.playingCardView.playingCardVM.icons.RemoveAt(this.cannotBeTargeted);
		this.playingCardView.playingCardVM.descriptionIcon.RemoveAt(this.cannotBeTargeted);
		this.playingCardView.playingCardVM.titlesIcon.RemoveAt(this.cannotBeTargeted);
		this.playingCardView.playingCardVM.iconsRect.RemoveAt(this.cannotBeTargeted);
		this.cannotBeTargeted = -1;
	}
	
	public void removeParalyzed()
	{
		print("REMOVE");
		this.playingCardView.playingCardVM.icons.RemoveAt(this.paralyzed);
		this.playingCardView.playingCardVM.descriptionIcon.RemoveAt(this.paralyzed);
		this.playingCardView.playingCardVM.titlesIcon.RemoveAt(this.paralyzed);
		this.playingCardView.playingCardVM.iconsRect.RemoveAt(this.paralyzed);
		this.paralyzed = -1;
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
					this.removeParalyzed();
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
	
	public void setTargetHalo(HaloTarget h)
	{
		this.playingCardView.playingCardVM.haloStyle.normal.background = this.halos [h.idImage];
		this.playingCardView.playingCardVM.haloTexts = new List<string>();
		this.playingCardView.playingCardVM.haloStyles = new List<GUIStyle>();
		
		for (int i = 0; i < h.textsToDisplay.Count; i++)
		{
			this.playingCardView.playingCardVM.haloTexts.Add(h.textsToDisplay [i]);
			this.playingCardView.playingCardVM.haloStyles.Add(this.haloTextStyles [h.stylesID [i]]);
		}
		this.playingCardView.playingCardVM.toDisplayHalo = true;
	}
	
	public void hideTargetHalo()
	{
		this.playingCardView.playingCardVM.toDisplayHalo = false;
	}
	
	public void cancelSkill()
	{
		this.hideControlSkillHandler();
		GameController.instance.cancelSkill();
	}
	
	public void hideControlSkillHandler()
	{
		this.playingCardView.playingCardVM.toDisplaySkillControlHandler = false;
	}
}


