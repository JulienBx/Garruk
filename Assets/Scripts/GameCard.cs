using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameCard : Photon.MonoBehaviour 
{
	public Texture[] frontFaces; 										// Les différentes images des cartes
	public Texture[] backFaces; 
	public Texture[] leftFaces;
	public Texture[] rightFaces;
	public Texture[] topFaces;
	public Texture[] bottomFaces;
	public Texture[] skillsPictos;
	public Texture[] metals;
	public Texture[] Areas;
	public List<GameSkill> GameSkills;
	public Card Card; 
	
	// L'instance de la carte courante 
	
	private string URLCard = ApplicationModel.host + "get_card.php";
	//private string URLCard = "http://localhost/GarrukServer/get_card.php";
	//	private Vector3 offset;
	
	
	
	public GameCard() {
	}
	
	public GameCard(Card card) 
	{
		this.Card = card;
	}
	// Use this for initialization
	
	
	public void setCard(Card card) 
	{
		this.Card = card;
	}
	
	void Start () {
		
		
	}
	
	// Update is called once per frame
	
	
	
	public void ShowFace(bool mine = true, DiscoveryFeature discoveryFeature = null) 
	{
		
		transform.Find("texturedGameCard").FindChild("AttackArea")
			.renderer.material.mainTexture = Areas [0];
		transform.Find("texturedGameCard").FindChild("SpeedArea")
			.renderer.material.mainTexture = Areas [1];
		transform.Find("texturedGameCard").FindChild("MoveArea")
			.renderer.material.mainTexture = Areas [2];
		
		transform.Find("texturedGameCard")
			.renderer.material.mainTexture = frontFaces[Card.ArtIndex]; 		// On affiche l'image correspondant à la carte
		
		transform.Find("texturedGameCard").FindChild("Title")
			.GetComponent<TextMesh>().text = Card.Title;			// On lui attribut son titre

		ShowClass(Card.TitleClass);
		ShowSpeed(Card.GetSpeed().ToString());
		if (discoveryFeature == null)
		{
			discoveryFeature = new DiscoveryFeature();
		}
		if (discoveryFeature.Attack || mine)
		{
			ShowAttack(Card.GetAttack().ToString());
		}
		else
		{
			ShowAttack("?");
		}
		if (discoveryFeature.Life || mine)
		{
			ShowLife(Card.GetLife().ToString());
		}
		else
		{
			ShowLife("?");
		}
		if (discoveryFeature.Move || mine)
		{
			ShowMove(Card.GetMove().ToString());
		}
		else if (discoveryFeature.MoveMin != -1)
		{
			ShowMinMove(discoveryFeature.MoveMin.ToString());
		}
		else{
			ShowMove("?");
		}
		
		for (int i = 0 ; i < 6 ; i++)
		{		
			transform.Find("texturedGameCard").FindChild("PictoMetalLife")
				.renderer.materials[i].mainTexture = metals [Card.LifeLevel];
			if (Card.GetAttack() > Card.Attack)
			{
				transform.Find("texturedGameCard").FindChild("AttackArea").FindChild("PictoMetalAttack")
					.renderer.materials[i].mainTexture = metals [4]; // On change la couleur des matériaux
			}
			else if (Card.GetAttack() < Card.Attack)
			{
				transform.Find("texturedGameCard").FindChild("AttackArea").FindChild("PictoMetalAttack")
					.renderer.materials[i].mainTexture = metals [5]; // On change la couleur des matériaux
			}
			else
			{
				transform.Find("texturedGameCard").FindChild("AttackArea").FindChild("PictoMetalAttack")
					.renderer.materials[i].mainTexture = metals [Card.AttackLevel]; // On change la couleur des matériaux
			}
			if (Card.GetSpeed() > Card.Speed)
			{
				transform.Find("texturedGameCard").FindChild("SpeedArea").FindChild("PictoMetalSpeed")
					.renderer.materials[i].mainTexture = metals [4]; // On change la couleur des matériaux
			}
			else if (Card.GetSpeed() < Card.Speed)
			{
				transform.Find("texturedGameCard").FindChild("SpeedArea").FindChild("PictoMetalSpeed")
					.renderer.materials[i].mainTexture = metals [5]; // On change la couleur des matériaux
			}
			else
			{
				transform.Find("texturedGameCard").FindChild("SpeedArea").FindChild("PictoMetalSpeed")
					.renderer.materials[i].mainTexture = metals [Card.SpeedLevel]; // On change la couleur des matériaux
			}

			if (Card.GetMove() > Card.Move)
			{
				transform.Find("texturedGameCard").FindChild("MoveArea").FindChild ("PictoMetalMove")
					.renderer.materials[i].mainTexture = metals [Card.MoveLevel];
			}
			else if (Card.GetMove() < Card.Move)
			{
				transform.Find("texturedGameCard").FindChild("MoveArea").FindChild ("PictoMetalMove")
					.renderer.materials[i].mainTexture = metals [Card.MoveLevel];
			}
			else
			{
				transform.Find("texturedGameCard").FindChild("MoveArea").FindChild ("PictoMetalMove")
					.renderer.materials[i].mainTexture = metals [Card.MoveLevel];
			}
		}
		
		for (int i = 0 ; i < Card.Skills.Count ; i++) // boucle sur la liste de compétence 
		{
			if (Card.Skills[i].IsActivated == 1 ) // On vérifie que la compétence existe et qu'elle est active
			{ 
				for (int j = 0 ; j < 6 ; j++)
				{
					transform.Find("texturedGameCard").FindChild("Skill"+(i+1)+"Area").FindChild ("PictoMetalSkill" + (i+1))
						.renderer.materials[j].mainTexture = metals [Card.Skills [i].Level];
				}
				
				transform.Find("texturedGameCard").FindChild("Skill"+(i+1)+"Area").FindChild ("Skill" + (i+1))
					.GetComponent<TextMesh> ().text = Card.Skills[i].Name; // On renseigne les caractéristique des compétences
				
				transform.Find("texturedGameCard").FindChild("Skill"+(i+1)+"Area").FindChild ("PictoSkill" + (i+1))
					.renderer.material.mainTexture = skillsPictos[Card.Skills [i].Id]; // On affecte une couleur pour le matériau
				
				transform.Find("texturedGameCard").FindChild("Skill"+(i+1)+"Area").FindChild ("PictoMetalSkill" + (i+1)).FindChild ("SkillForce" + (i+1))
					.GetComponent<TextMesh> ().text = Card.Skills[i].Power + "/" +Card.Skills[i].ManaCost ;
			}
			else 
			{
				for (int j = 0 ; j < 6 ; j++)
				{
					transform.Find("texturedGameCard").FindChild("Skill"+(i+1)+"Area").FindChild ("PictoMetalSkill" + (i+1))
						.renderer.materials[j].mainTexture = metals [0];
				}
				
				transform.Find("texturedGameCard").FindChild("Skill"+(i+1)+"Area").FindChild ("Skill" + (i+1))
					.GetComponent<TextMesh> ().text = ""; // On renseigne les caractéristique des compétences
				
				transform.Find("texturedGameCard").FindChild("Skill"+(i+1)+"Area").FindChild ("PictoSkill" + (i+1))
					.renderer.material.mainTexture = skillsPictos[199]; // On affecte une couleur pour le matériau
				
				transform.Find("texturedGameCard").FindChild("Skill"+(i+1)+"Area").FindChild ("PictoMetalSkill" + (i+1)).FindChild ("SkillForce" + (i+1))
					.GetComponent<TextMesh> ().text = "";
			}
		}
	}
	
	public void setTextResolution(float resolution)
	{
		transform.Find("texturedGameCard").Find("Title")
			.GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(resolution * 15);	
		transform.Find("texturedGameCard").Find("Title").localScale = new Vector3(0.07f/resolution,0.054f/resolution,0);
		
		transform.Find("texturedGameCard").FindChild("PictoMetalLife").FindChild("Life")
			.GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(resolution * 17);	
		transform.Find("texturedGameCard").FindChild("PictoMetalLife").FindChild("Life").localScale = new Vector3(0.3f/resolution,0.6f/resolution,0);
		
		transform.Find("texturedGameCard").FindChild("AttackArea").FindChild("PictoMetalAttack").FindChild("Attack")
			.GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(resolution * 15);	
		transform.Find("texturedGameCard").FindChild("AttackArea").FindChild("PictoMetalAttack").FindChild("Attack").localScale = new Vector3(0.5f/resolution,0.7f/resolution,0);
		
		transform.Find("texturedGameCard").FindChild("MoveArea").FindChild("PictoMetalMove").FindChild("Move")
			.GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(resolution * 15);	
		transform.Find("texturedGameCard").FindChild("MoveArea").FindChild("PictoMetalMove").FindChild("Move").localScale = new Vector3(0.5f/resolution,0.7f/resolution,0);
		
		transform.Find("texturedGameCard").FindChild("SpeedArea").FindChild("PictoMetalSpeed").FindChild("Speed")
			.GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(resolution * 15);	
		transform.Find("texturedGameCard").FindChild("SpeedArea").FindChild("PictoMetalSpeed").FindChild("Speed").localScale = new Vector3(0.5f/resolution,0.7f/resolution,0);
		
		transform.Find("texturedGameCard").Find("Class")
			.GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(resolution * 15);	
		transform.Find("texturedGameCard").Find("Class").localScale = new Vector3(0.06f/resolution,0.046f/resolution,0);
		
		for(int i = 1 ; i < 5 ; i++) // boucle sur la liste de compétence
		{  	
			transform.Find("texturedGameCard").FindChild("Skill"+(i)+"Area").FindChild ("Skill" + (i))
				.GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(resolution * 12);	
			transform.Find("texturedGameCard").FindChild("Skill"+(i)+"Area").FindChild ("Skill" + (i)).localScale = new Vector3(0.06f/resolution,0.6f/resolution,0);
			
			transform.Find("texturedGameCard").FindChild("Skill"+(i)+"Area").FindChild ("PictoMetalSkill" + (i)).FindChild ("SkillForce" + (i))
				.GetComponent<TextMesh> ().fontSize = Mathf.RoundToInt(resolution * 12);	
			transform.Find("texturedGameCard").FindChild("Skill"+(i)+"Area").FindChild ("PictoMetalSkill" + (i)).FindChild ("SkillForce" + (i)).localScale = new Vector3(0.3f/resolution,0.7f/resolution,0);
		}
	}
	
	public void Hide()
	{
		renderer.material.mainTexture = frontFaces[0]; 		// On affiche l'image correspondant à la carte
		transform.Find("texturedGameCard").Find("Title")
			.GetComponent<TextMesh>().text = "Title";	// On lui attribut son titre
		transform.Find("texturedGameCard").Find("Life")
			.GetComponent<TextMesh>().text = "Life";	// Et son nombre de point de vie
	}

	private void ShowAttack(string attack)
	{
		transform.Find("texturedGameCard").FindChild("AttackArea").FindChild("PictoMetalAttack").FindChild("Attack")
			.GetComponent<TextMesh> ().text = attack;
	}

	private void ShowMinMove(string minMove)
	{
		transform.Find("texturedGameCard").FindChild("MoveArea").FindChild("PictoMetalMove").FindChild("Move")
			.GetComponent<TextMesh> ().text = minMove + "-?";
	}

	private void ShowMove(string move)
	{
		transform.Find("texturedGameCard").FindChild("MoveArea").FindChild("PictoMetalMove").FindChild("Move")
			.GetComponent<TextMesh> ().text = move;
	}

	private void ShowLife(string life)
	{
		transform.Find("texturedGameCard").FindChild("PictoMetalLife").FindChild("Life")
			.GetComponent<TextMesh> ().text = life;
	}

	private void ShowSpeed(string speed)
	{
		transform.Find("texturedGameCard").FindChild("SpeedArea").FindChild("PictoMetalSpeed").FindChild("Speed")
			.GetComponent<TextMesh> ().text = speed;
	}

	private void ShowClass(string clas)
	{
		transform.Find("texturedGameCard").FindChild("Class")
			.GetComponent<TextMesh>().text = clas;
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
			print(w.text); 											// donne le retour
			
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
					Card = new Card(System.Convert.ToInt32(cardData[0]), // id
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
					Card.Skills = new List<Skill>();
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

					Card.Skills.Add(skill);
					Transform go = transform.Find("texturedGameCard/Skill" + Card.Skills.Count + "Area");
					
					switch (skill.ResourceName)
					{
						case "Reflexe": 
							Reflexe cp = go.gameObject.AddComponent("Reflexe") as Reflexe;
							cp.Skill = skill;
							cp.SkillNumber = Card.Skills.Count;
							cp.Init();
							break;
						default: 
							GameSkill skillCp = go.gameObject.AddComponent("GameSkill") as GameSkill;
							skillCp.Skill = skill;
							skillCp.SkillNumber = Card.Skills.Count;
							break;
					}
				}
			}
		}
	}
}
