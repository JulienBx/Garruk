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
	public bool cannotBeTargeted;

	public Texture2D[] icons ;
	public Texture2D[] halos ;

	public Tile tile ;

	public List<StatModifier> statModifiers ;
	public GUIStyle[] styles ;

	void Awake ()
	{
		this.playingCardView = gameObject.AddComponent <PlayingCardView> ();
		this.isMovable = true;
		this.isDead = false;
		this.isSelected = false;
		this.isMoved = false;
		statModifiers = new List<StatModifier> ();
		this.cannotBeTargeted = false;
		this.playingCardView.playingCardVM.iconStyle = styles [0];
		this.playingCardView.playingCardVM.titleStyle = styles [1];
		this.playingCardView.playingCardVM.descriptionStyle = styles [2];
		this.playingCardView.playingCardVM.descriptionRectStyle = styles [3];
	}

	public void setCannotBeTargeted (bool cbt, string title, string description)
	{
		if (cbt == true && this.cannotBeTargeted == false) {
			this.addIntouchable (title, description);
		}
		this.cannotBeTargeted = cbt;
	}

	public void activateTargetHalo ()
	{
		this.playingCardView.playingCardVM.toDisplayHalo = true;
		this.playingCardView.playingCardVM.halo = this.halos [0];
	}

	public void resizeHalo ()
	{
		int height = Screen.height;
		int width = Screen.width;

		int decalage = height / 15;

		Vector3 positionObject = new Vector3 (0, 0, 0);
		positionObject.x = (this.playingCardView.playingCardVM.position.x) * (height / 10f) - (decalage / 2) + (width / 2f);
		positionObject.y = height - ((this.playingCardView.playingCardVM.position.y + this.playingCardView.playingCardVM.scale.y / 2f) * (height / 10f) - (decalage / 2) + (height / 2f));

		Rect position = new Rect (positionObject.x, positionObject.y, decalage, decalage);
		this.playingCardView.playingCardVM.haloRect = position;
	}

	public void removeTargetHalo ()
	{
		this.playingCardView.playingCardVM.toDisplayHalo = false;
	}

	public void setStyles (bool isMyCharacter)
	{
		isMine = isMyCharacter;
		if (isMyCharacter) {

		} else {

		}
	}

	public void setTile (Tile t)
	{
		this.tile = t;
	}

	public void setActive (bool b)
	{
		gameObject.SetActive (b);
	}

	public void setControlActive (bool b)
	{
		this.playingCardView.playingCardVM.isActive = b;
	}

	public void setCard (Card c)
	{
		this.card = c;
		playingCardView.playingCardVM.face = this.faces [c.ArtIndex];
		playingCardView.playingCardVM.attack = c.Attack.ToString ();
		playingCardView.playingCardVM.move = c.Move.ToString ();
	}
	public void show ()
	{
		if (isMine) {
				playingCardView.playingCardVM.lifeGauge = this.lifeGauges [0];
		} else {
				playingCardView.playingCardVM.lifeGauge = this.lifeGauges [1];
		}
		base.getGOCoordinates (gameObject);
		this.setTextResolution ();
		updateAttack ();
		updateMove ();
		playingCardView.show ();
		this.updateLife ();
	}

	public void setTextResolution ()
	{
		float resolution = base.GOSize.y / 150f;
		playingCardView.setTextResolution (resolution);
	}

	public void setPosition (Vector3 p, Vector3 s)
	{
		this.playingCardView.playingCardVM.position = p;
		this.playingCardView.playingCardVM.scale = s;
		this.playingCardView.replace ();
	}

	public void setTile (Tile t, Vector3 p, bool toRotate)
	{
		this.tile = t;
		p.z = -2;
		this.playingCardView.playingCardVM.position = p;
		this.playingCardView.playingCardVM.scale = new Vector3 (1.10f, 1.10f, 1.10f);
		this.playingCardView.replace ();
	}

	public void changeTile (Tile t, Vector3 position)
	{
		this.tile = t;
		position.z = -5;
		this.playingCardView.playingCardVM.position = position;
		this.resize ();
		this.playingCardView.replace ();
	}

	public void resize ()
	{
		this.resizeHalo ();
		this.resizeIcons ();
	}

	public void setIDCharacter (int i)
	{
		this.IDCharacter = i;
	}

	public void updateLife ()
	{
		int life = this.card.GetLife ();
		int maxLife = this.card.Life;
		float percentage = 1.0f * life / maxLife;
		playingCardView.drawLifeGauge (percentage);
	}

	public void updateAttack ()
	{
		this.playingCardView.playingCardVM.attack = this.card.GetAttack ().ToString ();
	}

	public void updateMove ()
	{
			this.playingCardView.playingCardVM.move = this.card.GetMove ().ToString ();
	}

	public void hoverPlayingCard ()
	{
			GameController.instance.hoverPlayingCardHandler (this.IDCharacter);
	}

	public void clickPlayingCard ()
	{
			GameController.instance.clickPlayingCardHandler (this.IDCharacter);
	}

	public void addTarget ()
	{
			this.playingCardView.playingCardVM.toDisplayHalo = false;
			GameController.instance.addTarget (this.IDCharacter);
	}

	public void releaseClickPlayingCard ()
	{
			GameController.instance.releaseClickPlayingCardHandler (this.IDCharacter);
	}

	public void displayHover ()
	{
			this.playingCardView.playingCardVM.position.z = -3;
			this.playingCardView.replace ();
			this.playingCardView.playingCardVM.border = this.borderPC [1];
			this.playingCardView.changeBorder ();
	}

	public void displaySelected ()
	{
			this.playingCardView.playingCardVM.position.z = -4;
			this.playingCardView.replace ();
			this.playingCardView.playingCardVM.border = this.borderPC [4];
			this.playingCardView.changeBorder ();
	}

	public void displayOpponentSelected ()
	{
			this.playingCardView.playingCardVM.position.z = -4;
			this.playingCardView.replace ();
			this.playingCardView.playingCardVM.border = this.borderPC [2];
			this.playingCardView.changeBorder ();
	}

	public void displayPlaying ()
	{
			this.playingCardView.playingCardVM.position.z = -5;
			this.playingCardView.replace ();
			this.playingCardView.playingCardVM.border = this.borderPC [3];
			this.playingCardView.playingCardVM.isPlaying = true;
			this.playingCardView.changeBorder ();
	}

	public void hideHover ()
	{
			this.playingCardView.playingCardVM.position.z = -2;
			this.playingCardView.replace ();
			this.playingCardView.playingCardVM.border = this.borderPC [0];
			this.playingCardView.changeBorder ();
	}

	public void hideSelected ()
	{
			this.playingCardView.playingCardVM.position.z = -2;
			this.playingCardView.replace ();
			this.playingCardView.playingCardVM.border = this.borderPC [0];
			this.playingCardView.changeBorder ();
	}

	public Texture getPicture ()
	{
			return playingCardView.playingCardVM.face;
	}

	public void hidePlaying ()
	{
			this.playingCardView.playingCardVM.position.z = -2;
			this.playingCardView.replace ();
			this.playingCardView.playingCardVM.border = this.borderPC [0];
			this.playingCardView.changeBorder ();
	}

	public void addIntouchable (string title, string description)
	{
		this.addIcon (this.icons [0], title, description);
	}
	
	public void changeEsquive (string title, string description)
	{
		this.addIcon (this.icons [1], title, description);
	}

	public void addIcon (Texture2D icon, string title, string description)
	{
		this.playingCardView.playingCardVM.toDisplayIcon = true;
		this.playingCardView.playingCardVM.icons.Add (icon);
		this.playingCardView.playingCardVM.toDisplayDescriptionIcon.Add (false);
		this.playingCardView.playingCardVM.descriptionIcon.Add (description);
		this.playingCardView.playingCardVM.titlesIcon.Add (title);
		int height = Screen.height;
		int width = Screen.width;
		int decalage = height / 30;

		Vector3 positionObject = new Vector3 (0, 0, 0);
		positionObject.x = (this.playingCardView.playingCardVM.position.x + this.playingCardView.playingCardVM.scale.x / 2f) * (height / 10f) - decalage * (this.playingCardView.playingCardVM.icons.Count) + (width / 2f);
		positionObject.y = height - ((this.playingCardView.playingCardVM.position.y) * (height / 10f) + (height / 2f));

		Rect position = new Rect (positionObject.x, positionObject.y, decalage, decalage);
		this.playingCardView.playingCardVM.iconsRect.Add (position);
	}

	public void resizeIcons ()
	{
		int height = Screen.height;
		int width = Screen.width;
		int decalage = height / 30;
		this.playingCardView.playingCardVM.iconsRect = new List<Rect> ();
		for (int i = 1; i <= this.playingCardView.playingCardVM.icons.Count; i++) {
			Vector3 positionObject = new Vector3 (0, 0, 0);
			positionObject.x = (this.playingCardView.playingCardVM.position.x + this.playingCardView.playingCardVM.scale.x / 2f) * (height / 10f) - decalage * i + (width / 2f);
			positionObject.y = height - ((this.playingCardView.playingCardVM.position.y) * (height / 10f)+ (height / 2f));

			Rect position = new Rect (positionObject.x, positionObject.y, decalage, decalage);
			this.playingCardView.playingCardVM.iconsRect.Add (position);
		}
		this.playingCardView.playingCardVM.descriptionStyle.fontSize = height*15/1000;
		this.playingCardView.playingCardVM.titleStyle.fontSize = height*20/1000;
	}

	public void kill ()
	{
		print (this.IDCharacter+" est mort");
		this.isDead = true;
		this.hasPlayed = true;
		gameObject.renderer.enabled = false ;
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		for (int i = 0 ; i < renderers.Length ; i++) {
			renderers[i].renderer.enabled = false ;
		}
		GameController.instance.emptyTile(this.tile.x, this.tile.y);
	}
}


