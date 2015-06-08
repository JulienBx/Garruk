﻿using UnityEngine;

public class TileController : MonoBehaviour
{
//	public Texture2D cursorDragging;
//	public Texture2D cursorExchange;
//	public Texture2D cursorAttack;
//	public Texture2D cursorTarget;

	public Tile tile ;
	private int type ;
	private float scaleTile ;

	public TileView tileView ;
	public bool isDestination ;
	public int characterID = -1 ;
	public TileModification tileModification;
	public StatModifier statModifier;

	public GUIStyle[] styles;
	public Texture2D[] icons;
	public Texture2D[] backTile;
	public Texture2D[] borderTile;

	void Awake()
	{
		this.tileView = gameObject.AddComponent <TileView>();
		this.isDestination = false;
		this.tileView.tileVM.iconStyle = styles [0];
		tileModification = TileModification.Void;
	}

	void Start()
	{

	}

	public int getID()
	{
		return (tile.x * 10 + tile.y);
	}

	public void addTileTarget()
	{
		this.tileView.tileVM.isPotentialTarget = false;
		GameController.instance.addTileTarget(getID());
	}

	public void setTile(int x, int y, int boardWidth, int boardHeight, int type, float scaleTile)
	{
		this.tile = new Tile(x, y);
		this.type = type;

		this.tileView.tileVM.background = backTile [type];
		this.tileView.changeBackground();
		this.tileView.tileVM.border = borderTile [0];
		this.tileView.changeBorder();

		this.resize(1f, 3, 4);
	}

	public void resize(float scaleTile, float offsetX, float offsetY)
	{
		Vector3 position;
		this.tileView.tileVM.scale = new Vector3(scaleTile, scaleTile, scaleTile);
		if (GameController.instance.isFirstPlayer)
		{
			position = new Vector3(scaleTile * (-offsetX + 0.5f + this.tile.x), scaleTile * (-offsetY + 0.5f + this.tile.y), -1);
		} else
		{
			position = new Vector3(scaleTile * (offsetX - 0.5f - this.tile.x), scaleTile * (offsetY - 0.5f - this.tile.y), -1);
		}
		this.tileView.tileVM.position = position;

		this.tileView.resize();
		this.resizeIcons();
	}
	public void addTemple(int power)
	{
		this.tileView.tileVM.toDisplayIcon = true;
		this.tileModification = TileModification.Temple_Sacre;
		this.tileView.tileVM.icon = this.icons [0];
		tile.StatModifier = new StatModifier(power, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack);
	}

	public void addForetIcon()
	{
		this.tileView.tileVM.toDisplayIcon = true;
		this.tileModification = TileModification.Foret_de_Lianes;
		this.tileView.tileVM.icon = this.icons [1];
	}

	public void addSable()
	{
		this.tileView.tileVM.toDisplayIcon = true;
		this.tileModification = TileModification.Sables_Mouvants;
		this.tileView.tileVM.icon = this.icons [2];
		tile.StatModifier = new StatModifier(-999, ModifierType.Type_BonusMalus, ModifierStat.Stat_Move, 3);
	}

	public void addFontaine(int power)
	{
		this.tileView.tileVM.toDisplayIcon = true;
		this.tileModification = TileModification.Fontaine_de_Jouvence;
		this.tileView.tileVM.icon = this.icons [3];
		tile.StatModifier = new StatModifier(-power, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage);
	}

	public void removeIcon()
	{
		this.tileView.tileVM.toDisplayIcon = false;
	}
	public void resizeIcons()
	{
		int height = Screen.height;
		int width = Screen.width;
		
		int decalage = height / 15;
		
		Vector3 positionObject = new Vector3(0, 0, 0);
		positionObject.x = (this.tileView.tileVM.position.x) * (height / 10f) - (decalage / 2) + (width / 2f);
		positionObject.y = height - ((this.tileView.tileVM.position.y + this.tileView.tileVM.scale.y / 2f) * (height / 10f) - (decalage / 2) + (height / 2f));
		
		Rect position = new Rect(positionObject.x, positionObject.y, decalage, decalage);
		this.tileView.tileVM.iconRect = position;
	}
	public void setDestination(bool b)
	{
		this.isDestination = b;
		int facteur = 0;
		int borderIndex = 0;
		if (b)
		{
			facteur = 5;
			borderIndex = 4;
		}
		this.tileView.tileVM.background = this.backTile [facteur + this.type];
		this.tileView.changeBackground();
		this.tileView.tileVM.border = this.borderTile [borderIndex];
		this.tileView.changeBorder();
	}

	public void setStandard()
	{
		this.isDestination = false;
		this.tileView.tileVM.background = this.backTile [this.type];
		this.tileView.changeBackground();
		this.tileView.tileVM.border = this.borderTile [0];
		this.tileView.changeBorder();
	}

	public Vector3 getPosition()
	{
		return this.tileView.tileVM.position;
	}

	public void setBorderTile()
	{
		
	}

	public void hoverTile()
	{
		GameController.instance.hoverTileHandler(this.tile);
	}

	public void releaseClickTile()
	{
		GameController.instance.releaseClickTileHandler(this.tile);
	}

	public void displayHover()
	{
		this.tileView.tileVM.border = this.borderTile [1];
		this.tileView.changeBorder();
	}

	public void hideHover()
	{
		if (this.isDestination)
		{
			this.tileView.tileVM.border = this.borderTile [4];
			this.tileView.changeBorder();
		} else
		{
			this.tileView.tileVM.border = this.borderTile [0];
			this.tileView.changeBorder();
		}
	}

	public void displaySelected()
	{
		this.tileView.tileVM.border = this.borderTile [2];
		this.tileView.changeBorder();
	}
	
	public void hideSelected()
	{
		if (this.isDestination)
		{
			this.tileView.tileVM.border = this.borderTile [4];
			this.tileView.changeBorder();
		} else
		{
			this.tileView.tileVM.border = this.borderTile [0];
			this.tileView.changeBorder();
		}
	}

	public void displayPlaying()
	{
		this.tileView.tileVM.border = this.borderTile [3];
		this.tileView.changeBorder();
	}
	
	public void hidePlaying()
	{
		if (this.isDestination)
		{
			this.tileView.tileVM.border = this.borderTile [4];
			this.tileView.changeBorder();
		} else
		{
			this.tileView.tileVM.border = this.borderTile [0];
			this.tileView.changeBorder();
		}
	}

//	public void drag(){
//		if (this.isMovable){
//			GameController.instance.setCharacterDragged(this.characterID);
//		}
//	}
//	
//	public void release(){
//		if (this.isMovable){
//			GameController.instance.dropCharacter();
//		}
//	}

	public void activatePotentielTarget()
	{
		this.tileView.tileVM.isPotentialTarget = true;
	}

	public void removePotentielTarget()
	{
		this.tileView.tileVM.isPotentialTarget = false;
	}
}

