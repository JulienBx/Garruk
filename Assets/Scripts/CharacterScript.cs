using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterScript : Photon.MonoBehaviour {

	private string URLCard = ApplicationModel.host + "get_card.php";

	public Card card ;
	GUIStyle statsZoneStyle;
	GUIStyle NamePoliceStyle;
	GUIStyle LifePoliceStyle;
	GUIStyle SpeedPoliceStyle;
	GUIStyle AttackPoliceStyle;
	GUIStyle MovePoliceStyle;
	GUIStyle lifebarPoliceStyle;
	GUIStyle skillInfoStyle;
	Texture2D attackIcon ;
	Texture2D quicknessIcon ;
	Texture2D moveIcon ;
	public int x ;
	public int y ;

	bool isFocused = false ;
	public bool isHovered = false ;

	float life ;

	public int widthScreen = Screen.width; 
	int heightScreen = Screen.height;
	Bounds bounds;

	public Rect stats ; 

	Animator animator;

	bool toHide=true;

	// Use this for initialization

	void Start () {
		animator = transform.parent.GetComponent<Animator> ();
		bounds = renderer.bounds;
		//setStyles();
	}

	public void setRectStats(float x, float y, float sizeX, float sizeY){
		stats = new Rect(x, y, sizeX, sizeY);
	}

	
	// Update is called once per frame
	void Update () {
		if (Screen.width != widthScreen || Screen.height != heightScreen) {
			this.setStyles();
		}
	}

	void OnGUI ()
	{
		if(!toHide){

			bounds = renderer.bounds;
			foreach(var r in GetComponentsInChildren<Renderer>())
			{
				bounds.Encapsulate(r.bounds);
			}
				
			GUILayout.BeginArea (stats);
			{
				GUILayout.BeginHorizontal(statsZoneStyle, GUILayout.Width(stats.width));
				{
					GUILayout.Label("", lifebarPoliceStyle, GUILayout.Width(stats.width*life/this.card.Life), GUILayout.Height(stats.height/2));
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndArea ();

			GUILayout.BeginArea (stats);
				{
					GUILayout.BeginVertical();
					{
					if (!isFocused){
						if(GUILayout.Button (card.Title,NamePoliceStyle)){
								isFocused = true ;
							}
						}
					else{
						if (GUILayout.Button (life+"/"+this.card.Life,LifePoliceStyle)){
								isFocused = false ;
						}
					}
					GUILayout.BeginHorizontal(statsZoneStyle);
						{
						GUILayout.FlexibleSpace();
						GUILayout.BeginVertical();
							{
								GUILayout.FlexibleSpace();
								GUILayout.Box (attackIcon, AttackPoliceStyle);
								GUILayout.FlexibleSpace();
							}
						GUILayout.EndVertical();
						GUILayout.FlexibleSpace();
						GUILayout.Label (""+card.Attack,MovePoliceStyle);
						GUILayout.FlexibleSpace();
						GUILayout.BeginVertical();
							{
								GUILayout.FlexibleSpace();
								GUILayout.Box (quicknessIcon,AttackPoliceStyle);
								GUILayout.FlexibleSpace();
							}
						GUILayout.EndVertical();
						GUILayout.FlexibleSpace();
						GUILayout.Label (""+card.Speed,MovePoliceStyle);
						GUILayout.FlexibleSpace();
						GUILayout.BeginVertical();
							{
								GUILayout.FlexibleSpace();
								GUILayout.Box (moveIcon, AttackPoliceStyle);
								GUILayout.FlexibleSpace();
							}
						GUILayout.EndVertical();
						GUILayout.FlexibleSpace();
						GUILayout.Label (""+card.Move,MovePoliceStyle);
						GUILayout.FlexibleSpace();
						}
						GUILayout.EndHorizontal();
						
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndArea ();

			if (isHovered){
				int j = 0 ;
				GUI.depth=2;
				for (int i = 0 ; i < 4 ; i++){
					if (card.Skills[i].IsActivated==1){
						GUI.Label (new Rect(stats.x, stats.yMax+j*stats.height/2, stats.width, stats.height/2), card.Skills[i].Level+"."+card.Skills[i].Name, skillInfoStyle);
						j++ ; 
					}
				}
				GUI.depth=0;
			}
		}
	}

	public void setStyles(GUIStyle name, GUIStyle life, GUIStyle lifebar, GUIStyle attack, GUIStyle move, GUIStyle quickness, GUIStyle zone, Texture2D attackI, Texture2D quicknessI, Texture2D moveI, GUIStyle skillInfo) {
		heightScreen = Screen.height;
		widthScreen = Screen.width;
		this.NamePoliceStyle = name;
		this.SpeedPoliceStyle = quickness ;
		this.AttackPoliceStyle = attack ;
		this.MovePoliceStyle = move ;
		this.LifePoliceStyle = life ;
		this.lifebarPoliceStyle = lifebar;
		this.statsZoneStyle = zone ;
		this.NamePoliceStyle.fontSize = heightScreen*15/1000;
		this.SpeedPoliceStyle.fontSize = heightScreen*15/1000;
		this.SpeedPoliceStyle.fixedHeight = (int)heightScreen*2/100;
		this.AttackPoliceStyle.fontSize = heightScreen*15/1000;
		this.AttackPoliceStyle.fixedHeight = heightScreen/60;
		this.AttackPoliceStyle.fixedWidth = heightScreen/60;
		this.MovePoliceStyle.fontSize = heightScreen*15/1000;
		this.LifePoliceStyle.fontSize = heightScreen*15/1000;
		this.attackIcon = attackI;
		this.skillInfoStyle = skillInfo;
		this.skillInfoStyle.fontSize=heightScreen*15/1000;
		this.moveIcon = moveI ;
		this.quicknessIcon = quicknessI ;
	}

	private void setStyles() {
		heightScreen = Screen.height;
		widthScreen = Screen.width;
		this.NamePoliceStyle.fontSize = heightScreen*15/1000;
		this.SpeedPoliceStyle.fontSize = heightScreen*15/1000;
		this.SpeedPoliceStyle.fixedHeight = (int)heightScreen*2/100;
		this.AttackPoliceStyle.fontSize = heightScreen*15/1000;
		this.AttackPoliceStyle.fixedHeight = heightScreen/60;
		this.AttackPoliceStyle.fixedWidth = heightScreen/60;
		this.MovePoliceStyle.fontSize = heightScreen*15/1000;
		this.LifePoliceStyle.fontSize = heightScreen*15/1000;
		this.skillInfoStyle.fontSize=heightScreen*15/1000;
	}

	void OnMouseEnter()
	{
		isHovered = true ;
//		if (GameBoard.instance.isDragging)
//		{
//			if (!this.Equals(GameBoard.instance.CardSelected))
//			{
//				GameTile.instance.SetCursorToExchange();
//			} else
//			{
//				GameTile.instance.SetCursorToDrag();
//			}
//		}
//		
//		if (this.gameCard.card != null)
//		{
//			GameHoveredCard.instance.ChangeCard(this);
//		}
	}

	void OnMouseExit()
	{
		isHovered = false ;
		//		if (GameBoard.instance.isDragging)
		//		{
		//			if (!this.Equals(GameBoard.instance.CardSelected))
		//			{
		//				GameTile.instance.SetCursorToExchange();
		//			} else
		//			{
		//				GameTile.instance.SetCursorToDrag();
		//			}
		//		}
		//		
		//		if (this.gameCard.card != null)
		//		{
		//			GameHoveredCard.instance.ChangeCard(this);
		//		}
	}

	public void hideInformations(){
		toHide=true;
	}

	public void showInformations(){
		toHide=false;
	}

	public void toWalk(){
		animator.SetBool("isWalking",true );
	}

	public void stopWalking(){
		animator.SetBool("isWalking",false );
	}

	public IEnumerator RetrieveCard (int idCard)
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_idcard", idCard);						// ID de la carte
		
		WWW w = new WWW(URLCard, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			string[] cardEntries = w.text.Split('\n'); 				// Chaque ligne du serveur correspond à une carte
			
			for(int i = 0 ; i < cardEntries.Length - 1 ; i++) 		// On boucle sur les attributs d'une carte
			{
				string[] cardData = cardEntries[i].Split('\\'); 	// On découpe les attributs de la carte qu'on place dans un tableau
				if (cardData.Length < 2) 
				{
					break;
				}
				if (i == 0)
				{
					this.card = new Card(System.Convert.ToInt32(cardData[0]), // id
					                cardData[1], // title
					                System.Convert.ToInt32(cardData[2]), // life
					                System.Convert.ToInt32(cardData[3]), // attack
					                System.Convert.ToInt32(cardData[4]), // speed
					                System.Convert.ToInt32(cardData[5]), // move
					                System.Convert.ToInt32(cardData[6]), // artindex
					                System.Convert.ToInt32(cardData[7]), // idclass
					                cardData[8], // titleclass
					                System.Convert.ToInt32(cardData[9]), // lifelevel
					                System.Convert.ToInt32(cardData[10]), // movelevel
					                System.Convert.ToInt32(cardData[11]), // speedlevel
					                System.Convert.ToInt32(cardData[12])); // attacklevel
					this.card.Skills = new List<Skill>();
				}
				else
				{
					Skill skill = new Skill(  cardData[1],                         // name
					                        System.Convert.ToInt32(cardData[0]), // idskill
					                        System.Convert.ToInt32(cardData[2]), // isactivated
					                        System.Convert.ToInt32(cardData[3]), // level
					                        System.Convert.ToInt32(cardData[4]), // power
					                        System.Convert.ToInt32(cardData[5]), // costmana
					                        cardData[6],                         // description
					                        cardData[7],                         // Nom de la ressource
					                        System.Convert.ToSingle(cardData[8]),// ponderation
					                        System.Convert.ToInt32(cardData[9]));// xmin
					
					this.card.Skills.Add(skill);

//					Transform go = transform.Find("texturedGameCard/Skill" + Card.Skills.Count + "Area");
//					
//					switch (skill.ResourceName)
//					{
//					case "Reflexe": 
//						Reflexe rx = go.gameObject.AddComponent("Reflexe") as Reflexe;
//						rx.Skill = skill;
//						rx.SkillNumber = Card.Skills.Count;
//						rx.Init();
//						break;
//					case "Apathie":
//						Apathie ap = go.gameObject.AddComponent("Apathie") as Apathie;
//						ap.Skill = skill;
//						ap.SkillNumber = Card.Skills.Count;
//						ap.Init();
//						break;
//					case "Renforcement":
//						Renforcement rf = go.gameObject.AddComponent("Renforcement") as Renforcement;
//						rf.Skill = skill;
//						rf.SkillNumber = Card.Skills.Count;
//						rf.Init();
//						break;
//					case "Sape":
//						Sape sp = go.gameObject.AddComponent("Sape") as Sape;
//						sp.Skill = skill;
//						sp.SkillNumber = Card.Skills.Count;
//						sp.Init();
//						break;
//					default: 
//						GameSkill skillCp = go.gameObject.AddComponent("GameSkill") as GameSkill;
//						skillCp.Skill = skill;
//						skillCp.SkillNumber = Card.Skills.Count;
//						break;
//					}
				}

			}
		}
		this.life = this.card.Life;
	}

}
