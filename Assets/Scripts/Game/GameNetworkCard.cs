using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class GameNetworkCard : Photon.MonoBehaviour
{
	private string URLCard = ApplicationModel.host + "get_card.php";

	public GameTile currentTile;                                    	// Tile où la carte est positionnée
	public DiscoveryFeature DiscoveryFeature = new DiscoveryFeature();  // Les caractéristiques que l'adversaire a découvert
	public Card card ;

	public int currentLife ;
	public int currentAttack ;
	public int currentSpeed ;
	public int currentMove ;
	public List<Skill> currentSkills ;

	public bool isMine;
	public bool isLoaded;
	public bool hasPlayed = false ;                                          	// seulement utile pour la timeline

//	#region unity editor
//	public GUIStyle progress_empty;
//	public GUIStyle progress_full;
//	public Texture2D bgImage; 
//	public Texture2D fgImage;
//	public GameObject AttackAnim;
//	public GameObject YellowOutlines;
//	public bool isSelectable = false ;
//	#endregion

	private string URLUpdateStats = ApplicationModel.dev + "update_stat.php";

	void Start()
	{
//		gameCard = GetComponentInChildren<CharacterScript>();
//		if (gameCard.photonView.isMine){
//			isSelectable = true ;
//		}
	}
	void Update()
	{

	}
	
	void OnGUI()
	{

	}
	
//	public Vector2 CalcGridPos()
//	{
//		float x = (transform.position.x / (GameTile.instance.hexWidth * 1.5f/2) + (GameView.instance.gridWidthInHexes / 2f) + 1);
//		float y = Mathf.Floor(-(transform.position.y / (GameTile.instance.hexHeight) - (GameView.instance.gridHeightInHexes / 2f) + 1.5f));
//		
//		return new Vector2(x, y);
//	}
	
//	void OnMouseDown() 
//	{
//		if (gameCard.photonView.isMine)
//		{
//			GameBoard.instance.CardSelected = this;
//			GameBoard.instance.isDragging = true;
//			GameTile.instance.SetCursorToDrag();
//		} 
//			else
//			{
//				GameTile.InitIndexPathTile();
//				if (GameTimeLine.instance.PlayingCard.gameCard.card.Equals(gameCard.card) && !GamePlayingCard.instance.hasMoved) 
//				{
//					GameBoard.instance.CardSelected = this;
//					GameBoard.instance.isMoving = true;
//
//					RaycastHit hit;
//					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//					
//					if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << GameBoardGenerator.instance.GridLayerMask))
//					{
//						currentTile = hit.transform.gameObject.GetComponent<GameTile>();
//					}
//				}
//				if (GameTimeLine.instance.PlayingCard.gameCard.card.Equals(gameCard.card) && GamePlayingCard.instance.hasMoved) 
//				{
//					StartCoroutine(ChangeMessage("la carte s'est déjà déplacée pendant ce tour"));
//				}
//				Vector2 gridPosition = this.CalcGridPos();
//				if (currentTile != null)
//				{
//					currentTile.Passable = true;
//				}
//				//Tile tile = GameBoard.instance.board[new Point((int)gridPosition.x, (int)gridPosition.y)];
//				//colorAndMarkNeighboringTiles(tile.AllNeighbours, gameCard.Card.Move, Color.gray);
//			}                         
//
//		if (!GameTimeLine.instance.PlayingCard.Equals(this) && GamePlayingCard.instance.attemptToAttack && !GamePlayingCard.instance.hasAttacked) 
//		{
//			if (GameTimeLine.instance.PlayingCard.neighbors.Find(e => e.gameCard.card.Equals(this.gameCard.card)))
//			{
//				DiscoveryFeature.Life = true;
//				gameCard.photonView.RPC("GetDamage", PhotonTargets.AllBuffered, this.GetComponent<PhotonView>().viewID, GameTimeLine.instance.PlayingCard.gameCard.card.Attack);
//				GameTimeLine.instance.PlayingCard.GetComponent<GameNetworkCard>().DiscoveryFeature.Attack = true;
//				GamePlayingCard.instance.attemptToAttack = false;
//				GamePlayingCard.instance.hasAttacked = true;
//				GameTile.instance.SetCursorToDefault();
//				if (GamePlayingCard.instance.hasMoved && GamePlayingCard.instance.hasAttacked)
//				{
//					GamePlayingCard.instance.Pass();
//				}
//			}
//		}
//		if (GamePlayingCard.instance.attemptToCast && !GamePlayingCard.instance.hasAttacked)
//		{
//			gameCard.photonView.RPC("GetBuff", PhotonTargets.AllBuffered, this.GetComponent<PhotonView>().viewID, GamePlayingCard.instance.SkillCasted);
//			GamePlayingCard.instance.attemptToCast = false;
//			GamePlayingCard.instance.hasAttacked = true;
//			GameTile.instance.SetCursorToDefault();
//		}
//	}
//
//	void OnMouseExit()
//	{
//		if (GameBoard.instance.TimeOfPositionning)
//		{
//			if (gameCard.photonView.isMine)
//			{
//				if (GameBoard.instance.isDragging)
//				{
//					GameBoard.instance.CardHovered = null;
//				}
//			}
//		}
//		GameHoveredCard.instance.hide();
//	}
//	void OnMouseUp()
//	{
//		if (GameBoard.instance.TimeOfPositionning)
//		{
//			if (gameCard.photonView.isMine)
//			{
//				if (GameBoard.instance.isDragging)
//				{
//					if (!this.Equals(GameBoard.instance.CardHovered) && GameBoard.instance.CardHovered)
//					{
//						Vector3 temp = this.transform.position;
//						this.transform.position = GameBoard.instance.CardHovered.transform.position;
//						GameBoard.instance.CardHovered.transform.position = temp;
//					}
//					if (GameBoard.instance.CardHovered == null)
//					{
//						GameBoard.instance.droppedCard = true;
//					}
//					GameBoard.instance.isDragging = false;
//					GameTile.instance.SetCursorToDefault();
//				}
//			}
//		}
//		else
//		{
//			GameTile.RemovePassableTile();
//			
//			//this.FindNeighbors();
//			if (gameCard.photonView.isMine)
//			{
//				GameBoard.instance.isMoving = false;
//				GameBoard.instance.CardSelected = null;
//			}
//			if (GamePlayingCard.instance.attemptToMoveTo != null)
//			{
//				int nbTiles = CalcNbTiles(currentTile, GamePlayingCard.instance.attemptToMoveTo);
//				if (nbTiles == gameCard.card.GetMove())
//				{
//					DiscoveryFeature.Move = true;
//				}
//				else{
//					DiscoveryFeature.MoveMin = nbTiles;
//				}
//				GamePlayingCard.instance.attemptToMoveTo = null;
//				GamePlayingCard.instance.hasMoved = true;
//
//			}
//			if (GamePlayingCard.instance.hasMoved && GamePlayingCard.instance.hasAttacked)
//			{
//				GamePlayingCard.instance.Pass();
//			}
//			
//		}
//	}
	
	IEnumerator ChangeMessage(string message)
	{
//		GameScript.instance.labelMessage = message;
//		yield return new WaitForSeconds(2);
//		GameScript.instance.labelMessage = "";
		yield break;
	}
	
	
	
	void colorAndMarkNeighboringTiles(IEnumerable allNeighbours, int i, Color color)
	{
//		if (i-- == 0)
//		{
//			return;
//		}
//		foreach (Tile tile in allNeighbours)
//		{
//			GameTile gTile = GameObject.Find("hex " + tile.X + "-" + tile.Y).GetComponent<GameTile>();
//			
//			Vector3 pos = gTile.transform.TransformPoint(Vector3.zero) + new Vector3(0, 0, -2); // les colliders fonctionnent que d'un coté sur les planes, on va donc reculer et regarder ensuite en avant
//			
//			RaycastHit hit;
//			
//			bool hasCard = false;
//			if (Physics.Raycast(pos, Vector3.forward, out hit))
//			{
//				if (hit.transform.gameObject.tag == "PlayableCard")
//				{
//					hasCard = true;
//				}
//			}
//			
//			if (!hasCard && i > gTile.pathIndex)
//			{
//				gTile.pathIndex = i;
//				gTile.Passable = true;
//				gTile.changeColor(color);
//				colorAndMarkNeighboringTiles(tile.AllNeighbours, i, color);
//			}
//		}
	}
	
//	public void FindNeighbors()
//	{
//		neighbors.Clear();
//		GameTile gTile = null;
//		
//		RaycastHit hit;
//		Vector3 pos = transform.TransformPoint(Vector3.zero);
//		
//		if (Physics.Raycast(pos, Vector3.forward, out hit, Mathf.Infinity, 1 << GameBoard.instance.GridLayerMask))
//		{
//			gTile = hit.transform.gameObject.GetComponent<GameTile>();
//		}
//		
//		if (gTile != null)
//		{
//			foreach(Tile tile in gTile.tile.AllNeighbours)
//			{
//				GameTile neighborTile = GameObject.Find("hex " + tile.X + "-" + tile.Y).GetComponent<GameTile>();
//				pos = neighborTile.transform.TransformPoint(Vector3.zero) + new Vector3(0, 0, -2);
//				
//				if (Physics.Raycast(pos, Vector3.forward, out hit))
//				{
//					if (hit.transform.gameObject.tag == "PlayableCard")
//					{
//						neighbors.Add(hit.transform.gameObject.GetComponent<GameNetworkCard>());
//					}
//				}
//			}
//		}
//	}
//	public bool hasNeighbor()
//	{
//		return neighbors.Count > 0;
//	}
	
	public new void ShowFace()
	{
//		gameCard.ShowFace(gameCard.photonView.isMine, DiscoveryFeature);
//		Transform LifeTextPosition = transform.Find("Life");
//		if (LifeTextPosition != null)
//		{
//			string text;
//			if (gameCard.photonView.isMine || DiscoveryFeature.Life)
//			{
//				text = (gameCard.Card.GetLife() - Damage).ToString();
//			}
//			else
//			{
//				text = "?";
//			}
//			LifeTextPosition.GetComponent<TextMesh>().text = text;	// Et son nombre de point de vie
//		}
	}

	int CalcNbTiles(GameTile currentTile, GameTile attemptToMoveTo)
	{
		//var path = PathFinder.FindPath(currentTile.tile, attemptToMoveTo.tile);
		//return (int)path.TotalCost;
		return 0;
	}
	
	// Messages RPC
	[RPC]
	void GetDamage(int id, int attack)
	{
		//Instantiate(AttackAnim, transform.position + new Vector3(0, 0, -2), Quaternion.identity);
		GameObject go = PhotonView.Find(id).gameObject;
		GameNetworkCard gnc = go.GetComponent<GameNetworkCard>();
		int damage = attack;

//		if (damage >= gnc.gameCard.card.Life)
//		{
//			GameTile.RemovePassableTile();
//			GameTimeLine.instance.GameCards.Remove(gnc);
//			
//			if (gnc.ownerNumber == 1)
//			{
//				if (--GameBoard.instance.nbCardsPlayer1 < 1)
//				{
//					GameScript.instance.EndOfGame(2);
//					if (GameBoard.instance.MyPlayerNumber == 1 && ApplicationModel.gameType == 1)
//					{
//					
//						GameScript.instance.addStat(2, 1);
//					}
//				}
//			}
//			else
//			{
//				if (--GameBoard.instance.nbCardsPlayer2 < 1)
//				{
//					GameScript.instance.EndOfGame(1);
//					if (GameBoard.instance.MyPlayerNumber == 2 && ApplicationModel.gameType == 1)
//					{
//						GameScript.instance.addStat(1, 2);
//					}
//				}
//			}
//			gnc.gameObject.SetActive(false);
//		} else
//		{
//			gnc.ShowFace();
//		}
		GameTimeLine.instance.Arrange();
	}
	[RPC]
	void GetBuff(int target, int skillCasted)
	{
		GameObject goCard = GameTimeLine.instance.PlayingCardObject;
		//GameSkill goSkill = goCard.transform.Find("texturedGameCard/Skill" + skillCasted + "Area").gameObject.GetComponent<GameSkill>();
		//goSkill.Apply(target);
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(DiscoveryFeature.Attack);
			stream.SendNext(DiscoveryFeature.Life);
			stream.SendNext(DiscoveryFeature.Move);
			stream.SendNext(DiscoveryFeature.MoveMin);
			stream.SendNext(DiscoveryFeature.Skills [0]);
			stream.SendNext(DiscoveryFeature.Skills [1]);
			stream.SendNext(DiscoveryFeature.Skills [2]);
			stream.SendNext(DiscoveryFeature.Skills [3]);
		} else
		{
			DiscoveryFeature.Attack = (bool)stream.ReceiveNext();
			DiscoveryFeature.Life = (bool)stream.ReceiveNext();
			DiscoveryFeature.Move = (bool)stream.ReceiveNext();
			DiscoveryFeature.MoveMin = (int)stream.ReceiveNext();
			DiscoveryFeature.Skills [0] = (bool)stream.ReceiveNext();
			DiscoveryFeature.Skills [1] = (bool)stream.ReceiveNext();
			DiscoveryFeature.Skills [2] = (bool)stream.ReceiveNext();
			DiscoveryFeature.Skills [3] = (bool)stream.ReceiveNext();
//			if (gameCard != null && gameCard.card != null)
//			{
//				StartCoroutine(updateStat());
//				ShowFace();
//			}
		}
	}

	public IEnumerator updateStat()
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);	// user
		//form.AddField("myform_idcard", gameCard.card.Id.ToString());// ID de la carte
		form.AddField("myform_attack", DiscoveryFeature.Attack ? "1" : "0");	// attaque de la carte
		form.AddField("myform_life", DiscoveryFeature.Life ? "1" : "0");	// attaque de la carte
		form.AddField("myform_move", DiscoveryFeature.Move ? "1" : "0");	// attaque de la carte
		form.AddField("myform_movemin", DiscoveryFeature.MoveMin.ToString());	// attaque de la carte
		form.AddField("myform_skill1", DiscoveryFeature.Skills [0] ? "1" : "0");	// attaque de la carte
		form.AddField("myform_skill2", DiscoveryFeature.Skills [1] ? "1" : "0");	// attaque de la carte
		form.AddField("myform_skill3", DiscoveryFeature.Skills [2] ? "1" : "0");	// attaque de la carte
		form.AddField("myform_skill4", DiscoveryFeature.Skills [3] ? "1" : "0");	// attaque de la carte

		WWW w = new WWW(URLUpdateStats, form); 							// On envoie le formulaire à l'url sur le serveur 
		yield return w; 	
		if (w.error != null)
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		print(w.text);
	}

	public IEnumerator RetrieveCard(int idCard)
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_idcard", idCard);						// ID de la carte
		
		WWW w = new WWW(URLCard, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null)
		{
			print(w.error); 										// donne l'erreur eventuelle
		} else
		{
			string[] cardEntries = w.text.Split('\n'); 				// Chaque ligne du serveur correspond à une carte
			
			for (int i = 0; i < cardEntries.Length - 1; i++) 		// On boucle sur les attributs d'une carte
			{
				string[] cardData = cardEntries [i].Split('\\'); 	// On découpe les attributs de la carte qu'on place dans un tableau
				if (cardData.Length < 2)
				{
					break;
				}
				if (i == 0)
				{
					this.card = new Card(System.Convert.ToInt32(cardData [0]), // id
					                     cardData [1], // title
					                     System.Convert.ToInt32(cardData [2]), // life
					                     System.Convert.ToInt32(cardData [3]), // attack
					                     System.Convert.ToInt32(cardData [4]), // speed
					                     System.Convert.ToInt32(cardData [5]), // move
					                     System.Convert.ToInt32(cardData [6]), // artindex
					                     System.Convert.ToInt32(cardData [7]), // idclass
					                     cardData [8], // titleclass
					                     System.Convert.ToInt32(cardData [9]), // lifelevel
					                     System.Convert.ToInt32(cardData [10]), // movelevel
					                     System.Convert.ToInt32(cardData [11]), // speedlevel
					                     System.Convert.ToInt32(cardData [12])); // attacklevel
					this.card.Skills = new List<Skill>();
				} else
				{
					Skill skill = new Skill(cardData [1],                         // name
					                        System.Convert.ToInt32(cardData [0]), // idskill
					                        System.Convert.ToInt32(cardData [2]), // isactivated
					                        System.Convert.ToInt32(cardData [3]), // level
					                        System.Convert.ToInt32(cardData [4]), // power
					                        System.Convert.ToInt32(cardData [5]), // costmana
					                        cardData [6],                         // description
					                        cardData [7],                         // Nom de la ressource
					                        System.Convert.ToSingle(cardData [8]),// ponderation
					                        System.Convert.ToInt32(cardData [9]),// xmin
					                        cardData [10]);
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
		this.currentLife = this.card.Life;
		this.currentAttack = this.card.Attack;
		this.currentSpeed = this.card.Speed;
		this.currentMove = this.card.Move;
		this.currentSkills = this.card.Skills;

		isLoaded = true;
	}
}

