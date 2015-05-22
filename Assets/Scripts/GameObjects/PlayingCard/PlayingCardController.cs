﻿using UnityEngine;
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

	public Tile tile ;

	public List<StatModifier> statModifiers ;

	void Awake()
	{
		this.playingCardView = gameObject.AddComponent <PlayingCardView>();
		this.isMovable = true;
		this.isDead = false;
		this.isSelected = false;
		this.isMoved = false;
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
		playingCardView.show();
		this.updateLife();
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

	public void setTile(Tile t, Vector3 p, bool toRotate)
	{
		this.tile = t;
		p.z = -2;
		this.playingCardView.playingCardVM.position = p;
//		if (!toRotate)
//		{
//			this.playingCharacterView.playingCharacterVM.rotation = Quaternion.Euler(-90, 0, 0);
//		} else
//		{
//			this.playingCharacterView.playingCharacterVM.rotation = Quaternion.Euler(90, 180, 0);
//			
//			//this.playingCardView.playingCardVM.ScreenPosition.y = Screen.height-this.playingCardView.playingCardVM.ScreenPosition.y;
//		}
//		this.playingCharacterView.playingCharacterVM.ScreenPosition = Camera.main.WorldToScreenPoint(this.playingCharacterView.playingCharacterVM.position);
		this.playingCardView.playingCardVM.scale = new Vector3(1.10f, 1.10f, 1.10f);
		this.playingCardView.replace();
	}

	public void changeTile(Tile t, Vector3 position)
	{
		this.tile = t;
		position.z = -5;
		this.playingCardView.playingCardVM.position = position;
		this.playingCardView.replace();
	}

	public void resize(int h)
	{
//		this.scale = h * 0.001f / 1000f;
//		this.playingCardView.playingCardVM.scale = new Vector3(this.scale, this.scale, this.scale);
//		if (this.isMine)
//		{
//			if (this.isSelected)
//			{
//				this.playingCardView.playingCardVM.ScreenPosition = new Vector3(Screen.width * ((this.sortID - 1) * 0.13f + 0.12f + 0.21f), 0.93f * h, 0);
//				this.playingCardView.playingCardVM.infoRect = new Rect(this.playingCardView.playingCardVM.ScreenPosition.x - 0.12f * Screen.width, this.playingCardView.playingCardVM.ScreenPosition.y - 0.07f * h, 0.24f * Screen.width, 0.14f * h);
//				this.playingCardView.playingCardVM.isSelected = true;
//			} else if (this.isMoved)
//			{
//				this.playingCardView.playingCardVM.ScreenPosition = new Vector3(Screen.width * ((this.sortID - 1) * 0.13f + 0.18f + 0.21f), 0.93f * h, 0);
//				this.playingCardView.playingCardVM.infoRect = new Rect(this.playingCardView.playingCardVM.ScreenPosition.x - 0.06f * Screen.width, this.playingCardView.playingCardVM.ScreenPosition.y - 0.07f * Screen.height, 0.12f * Screen.width, 0.14f * Screen.height);
//			} else
//			{
//				this.playingCardView.playingCardVM.ScreenPosition = new Vector3(Screen.width * ((this.sortID - 1) * 0.13f + 0.06f + 0.21f), 0.93f * h, 0);
//				this.playingCardView.playingCardVM.infoRect = new Rect(this.playingCardView.playingCardVM.ScreenPosition.x - 0.06f * Screen.width, this.playingCardView.playingCardVM.ScreenPosition.y - 0.07f * Screen.height, 0.12f * Screen.width, 0.14f * Screen.height);
//			}
//			if (GameController.instance.isFirstPlayer)
//			{
//				this.playingCardView.playingCardVM.position = Camera.main.ScreenToWorldPoint(this.playingCardView.playingCardVM.ScreenPosition);
//				this.playingCardView.playingCardVM.position.y = -5f;
//			} else
//			{
//				this.playingCardView.playingCardVM.position = Camera.main.ScreenToWorldPoint(this.playingCardView.playingCardVM.ScreenPosition);
//				this.playingCardView.playingCardVM.position.y = 5 - this.playingCardView.playingCardVM.position.y;
//				this.playingCardView.playingCardVM.position.y = 5f;
//			}
//		} else
//		{
//			if (this.isSelected)
//			{
//				this.playingCardView.playingCardVM.ScreenPosition = new Vector3(Screen.width * ((this.sortID - 1) * 0.13f + 0.15f), 0.07f * h, 0);
//				this.playingCardView.playingCardVM.infoRect = new Rect(this.playingCardView.playingCardVM.ScreenPosition.x - 0.12f * Screen.width, this.playingCardView.playingCardVM.ScreenPosition.y - 0.07f * h, 0.24f * Screen.width, 0.14f * h);
//				this.playingCardView.playingCardVM.isSelected = true;
//			} else if (this.isMoved)
//			{
//				this.playingCardView.playingCardVM.ScreenPosition = new Vector3(Screen.width * ((this.sortID - 1) * 0.10f), 0.07f * h, 0);
//				this.playingCardView.playingCardVM.infoRect = new Rect(this.playingCardView.playingCardVM.ScreenPosition.x - 0.06f * Screen.width, this.playingCardView.playingCardVM.ScreenPosition.y - 0.07f * Screen.height, 0.12f * Screen.width, 0.14f * Screen.height);
//			} else
//			{
//				this.playingCardView.playingCardVM.ScreenPosition = new Vector3(Screen.width * ((this.sortID - 1) * 0.13f + 0.06f + 0.15f), 0.07f * h, 0);
//				this.playingCardView.playingCardVM.infoRect = new Rect(this.playingCardView.playingCardVM.ScreenPosition.x - 0.06f * Screen.width, this.playingCardView.playingCardVM.ScreenPosition.y - 0.07f * Screen.height, 0.12f * Screen.width, 0.14f * Screen.height);
//			}
//			if (GameController.instance.isFirstPlayer)
//			{
//				this.playingCardView.playingCardVM.position = Camera.main.ScreenToWorldPoint(this.playingCardView.playingCardVM.ScreenPosition);
//				this.playingCardView.playingCardVM.position.y = 5f;
//			} else
//			{
//				this.playingCardView.playingCardVM.position = Camera.main.ScreenToWorldPoint(this.playingCardView.playingCardVM.ScreenPosition);
//				this.playingCardView.playingCardVM.position.y = 5 - this.playingCardView.playingCardVM.position.y;
//				this.playingCardView.playingCardVM.position.y = -5f;
//			}
//		}
//
//		this.playingCardView.playingCardVM.nameTextStyle.fontSize = h * 18 / 1000;
//		this.playingCardView.playingCardVM.attackZoneTextStyle.fontSize = h * 15 / 1000;
//		this.playingCardView.playingCardVM.lifeBarStyle.fontSize = h * 15 / 1000;
//		this.playingCardView.playingCardVM.QuicknessBarStyle.fontSize = h * 15 / 1000;
//		this.playingCardView.playingCardVM.skillTitleTextStyle.fontSize = h * 16 / 1000;
//		this.playingCardView.playingCardVM.skillDescriptionTextStyle.fontSize = h * 12 / 1000;
//		this.playingCardView.playingCardVM.skillInfoRectWidth = 0.12f * Screen.width;
//		
//		this.playingCardView.replace();
	}
	
	public void setIDCharacter(int i)
	{
		this.IDCharacter = i;
	}


	public void getDamage()
	{
		//GameController.instance.inflictDamage(ID);
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
		int attack = this.card.Attack;
		int bonus = 0;
		for (int i = 0; i < this.statModifiers.Count; i++)
		{
			if (statModifiers [i].Stat == ModifierStat.Stat_Attack && statModifiers [i].Type == ModifierType.Type_BonusMalus)
			{
				bonus += statModifiers [i].Amount;
			}
		}
		this.playingCardView.playingCardVM.attack += attack + bonus;
	}

	public void updateMove()
	{
		int move = this.card.Move;
		int bonus = 0;
		for (int i = 0; i < this.statModifiers.Count; i++)
		{
			if (statModifiers [i].Stat == ModifierStat.Stat_Move && statModifiers [i].Type == ModifierType.Type_BonusMalus)
			{
				bonus += statModifiers [i].Amount;
			}
		}
		this.playingCardView.playingCardVM.move += move + bonus;
	}

	public void updateQuickness()
	{
		int speed = this.card.Speed;
		int bonus = 0;
		for (int i = 0; i < this.statModifiers.Count; i++)
		{
			if (statModifiers [i].Stat == ModifierStat.Stat_Speed && statModifiers [i].Type == ModifierType.Type_BonusMalus)
			{
				bonus += statModifiers [i].Amount;
			}
		}
	}

	public void hoverPlayingCard()
	{
		GameController.instance.hoverPlayingCardHandler(this.IDCharacter);
	}

	public void clickPlayingCard()
	{
		GameController.instance.clickPlayingCardHandler(this.IDCharacter);
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
}


