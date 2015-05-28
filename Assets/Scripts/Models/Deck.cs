using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Deck
{
	//Interconnexion BDD
	private string URLCards = ApplicationModel.host + "get_cards_by_deck_by_user.php";
	private string URLGetCardIDS = ApplicationModel.host + "get_cardsIDs_by_deck.php";
	private string URLSelectedDeck = ApplicationModel.host + "get_selected_deck_by_username.php";
	private static string URLEditDeck = ApplicationModel.host + "update_deck_name.php";
	private static string URLCreateDeck = ApplicationModel.host + "add_new_deck.php";
	private static string URLDeleteDeck = ApplicationModel.host + "delete_deck.php";
	private static string URLAddCardToDeck = ApplicationModel.host + "add_card_to_deck_by_user.php";
	private static string URLRemoveCardFromDeck = ApplicationModel.host + "remove_card_from_deck_by_user.php";
	private static string URLGetCardsByDeck = ApplicationModel.host + "get_cards_by_deck.php";
	private static string URLUpdateXpCards = ApplicationModel.host + "update_xp_cards.php";

	public int Id; 												// Id unique de la carte
	public string Name; 										// Nom du deck
	public int NbCards = 0; 										// Nombre de cartes présentes dans le deck
	public List<Card> Cards;									// Liste de carte du deck
	public string OwnerUsername;                                // Username de la personne possédant le deck

	public Deck()
	{
	}
	public Deck(int id)
	{
		this.Id = id;
		this.Cards = new List<Card>();
	}

	public Deck(string username)
	{
		OwnerUsername = username;
		this.Cards = new List<Card>();
	}

	public Deck(int id, string name)
	{
		this.Id = id;
		this.Name = name;
		this.NbCards = 0;
		this.Cards = new List<Card>();
	}

	public Deck(int id, string name, int nbCards)
	{
		this.Id = id;
		this.Name = name;
		this.NbCards = nbCards;
		this.Cards = new List<Card>();
	}

	public Deck(int id, string name, int nbCards, List<Card> cards)
	{
		this.Name = name;
		this.NbCards = nbCards;
		this.Cards = cards;
	}

	public IEnumerator addCard(int idCard)
	{
		WWWForm form = new WWWForm(); 						
		form.AddField("myform_hash", ApplicationModel.hash);
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_deck", Id);
		form.AddField("myform_idCard", idCard);
		WWW w = new WWW(URLAddCardToDeck, form); 								
		yield return w; 						
		
		if (w.error != null)
		{
			Debug.Log(w.error);								
		} else
		{
			this.NbCards++;
			this.Cards.Add(new Card(idCard));
		}
	}

	public void addCard(Card c)
	{
		this.Cards.Add(c);
	}

	public IEnumerator removeCard(int idCard)
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_deck", Id);
		form.AddField("myform_idCard", idCard);
		WWW w = new WWW(URLRemoveCardFromDeck, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente

		if (w.error != null)
		{
			Debug.Log(w.error); 									// donne l'erreur eventuelle
		} else
		{
			this.NbCards--;
			for (int i=0; i<this.Cards.Count; i++)
			{
				if (this.Cards [i].Id == idCard)
				{
					this.Cards.RemoveAt(i);
					break;
				}
			}
		}
	}

	public IEnumerator create(string decksName)
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_name", decksName);
		WWW w = new WWW(URLCreateDeck, form);						// On envoie le formulaire à l'url sur le serveur 
		yield return w;

		if (w.error != null)
		{
			Debug.Log(w.error);
		} else
		{
			this.Id = System.Convert.ToInt32(w.text);
			this.Name = decksName;
			this.NbCards = 0;
			this.Cards = new List<Card>();
		}
	}
	public IEnumerator delete()
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_id", Id);
		WWW w = new WWW(URLDeleteDeck, form);						// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null)
		{
			Debug.Log(w.error);
		}
	}

	public IEnumerator edit(string newName)
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_id", Id);
		form.AddField("myform_name", newName);
		WWW w = new WWW(URLEditDeck, form); 						// On envoie le formulaire à l'url sur le serveur 
		yield return w; 

		if (w.error != null)
		{
			Debug.Log(w.error);
		} else
		{
			this.Name = newName;
		}
	}

	public IEnumerator LoadDeck()
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", this.OwnerUsername); 	// Pseudo de l'utilisateur connecté

		WWW w = new WWW(URLSelectedDeck, form); 							// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null)
		{
			Debug.Log(w.error); 									// donne l'erreur eventuelle
		} else
		{
			string[] deckEntries = w.text.Split('\n'); 				// Chaque ligne du serveur correspond à une carte
			
			for (int i = 0; i < 1; i++)
			{
				string[] deckData = deckEntries [i].Split('\\'); 	// On découpe les attributs de la carte qu'on place dans un tableau
				int idDeck = System.Convert.ToInt32(deckData [0]);
				this.Id = idDeck;
			}	
		}
	}

	public IEnumerator RetrieveCards()
	{
		Card c;
		string[] cardData;
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_deck", this.Id);							// Id du	 deck
		
		WWW w = new WWW(URLCards, form); 							// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null)
		{
			Debug.Log(w.error); 									// donne l'erreur eventuelle
		} else
		{
			//Debug.Log(w.text);
			string[] cardEntries = w.text.Split('\n'); 				// Chaque ligne du serveur correspond à une carte
			
			for (int i = 0; i < cardEntries.Length - 1; i++) 		// On boucle sur les attributs d'une carte
			{
				cardData = cardEntries [i].Split('\\');
				if (!cardEntries [i].StartsWith("skill"))
				{
					c = new Card();
					c.Id = System.Convert.ToInt32(cardData [0]); 	// Ici, on récupère l'id en base
					c.ArtIndex = System.Convert.ToInt32(cardData [1]); 	// l'indice de l'image
					c.Title = cardData [2]; 					// le titre de la carte
					c.Life = System.Convert.ToInt32(cardData [3]);	// le nombre de point de vie
					c.Speed = System.Convert.ToInt32(cardData [4]);	// la rapidité
					c.Move = System.Convert.ToInt32(cardData [5]);	    // le mouvement
					c.Attack = System.Convert.ToInt32(cardData [6]);	// l'attaque
					c.Experience = System.Convert.ToInt32(cardData [7]);	// l'experience
					c.LifeLevel = System.Convert.ToInt32(cardData [8]);
					c.AttackLevel = System.Convert.ToInt32(cardData [9]);
					c.MoveLevel = System.Convert.ToInt32(cardData [10]);
					c.SpeedLevel = System.Convert.ToInt32(cardData [11]);

					c.Skills = new List<Skill>();
					this.addCard(c);
					NbCards ++;
				} else
				{
					this.Cards [NbCards - 1].Skills.Add(new Skill(cardData [1], (System.Convert.ToInt32(cardData [2]) + 2), System.Convert.ToInt32(cardData [3]), System.Convert.ToInt32(cardData [4]), System.Convert.ToInt32(cardData [5]), System.Convert.ToInt32(cardData [6]), cardData [7], cardData [8]));
				}
			}
		}
	}

	public IEnumerator RetrieveCardIDs()
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_deck", this.Id);							// Id du	 deck
		
		WWW w = new WWW(URLGetCardIDS, form); 							// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null)
		{
			Debug.Log(w.error); 									// donne l'erreur eventuelle
		} else
		{
			string[] cardEntries = w.text.Split('\n'); 				// Chaque ligne du serveur correspond à une carte
			
			for (int i = 0; i < cardEntries.Length - 1; i++)
			{
				this.addCard(new Card(System.Convert.ToInt32(cardEntries [i])));
				this.NbCards = i + 1;

				NbCards = i + 1;
			}
		}
	}
	public IEnumerator updateXpCards(int earnXp)
	{

		string idCards = "";
		string experienceCards = "";
		string newPowerCards = "";
		string attributeNameCards = "";
		string idSkillCards = "";
		string idClassCards = "";
		string idLevelCards = "";

		string[] attributeName = new string[5];
		int[] idSkill = new int[5];
		int[] idLevel = new int[5];
		int[] newPower = new int[5];
		int[] experience = new int[5];
		int[] randomAttribute = new int[5];

		for (int i=0; i<5; i++)
		{
			this.Cards [i].ExperienceLevel = this.Cards [i].getXpLevel();
			
			attributeName [i] = "-1";
			idSkill [i] = -1;
			idLevel [i] = 1;
			newPower [i] = 0;
			experience [i] = this.Cards [i].Experience + earnXp;
			randomAttribute [i] = -1;
			
			if (this.Cards [i].ExperienceLevel != 10 && experience [i] >= Card.experienceLevels [this.Cards [i].ExperienceLevel + 1])
			{
				
				int nbAttributes = 4;
				
				for (int j = 0; j < this.Cards[i].Skills.Count; j++)
				{
					
					if (this.Cards [i].Skills [j].IsActivated == 1)
						nbAttributes = nbAttributes + 1;
				}
				
				randomAttribute [i] = Mathf.RoundToInt(UnityEngine.Random.Range(0, nbAttributes));
				int randomPower = Mathf.RoundToInt(UnityEngine.Random.Range(5, 10));
				
				switch (randomAttribute [i])
				{
					case 0:
						newPower [i] = this.Cards [i].Move + 1;
						attributeName [i] = "move";
						break;
					case 1:
						newPower [i] = Mathf.RoundToInt((1 + randomPower * 0.01f) * this.Cards [i].Life);
						attributeName [i] = "life";
						if (newPower [i] >= 100 || newPower [i] > (100 - Mathf.Sqrt(500f)))
							idLevel [i] = 3;
						else if (newPower [i] > (100 - Mathf.Sqrt(2000f)))
							idLevel [i] = 2;
						break;
					case 2:
						newPower [i] = Mathf.RoundToInt((1 + randomPower * 0.01f) * this.Cards [i].Attack);
						attributeName [i] = "attack";
						if (newPower [i] >= 100 || newPower [i] > (100 - Mathf.Sqrt(500f)))
							idLevel [i] = 3;
						else if (newPower [i] > (100 - Mathf.Sqrt(2000f)))
							idLevel [i] = 2;
						break;
					case 3:
						newPower [i] = Mathf.RoundToInt((1 + randomPower * 0.01f) * this.Cards [i].Speed);
						attributeName [i] = "speed";
						if (newPower [i] >= 100 || newPower [i] > (100 - Mathf.Sqrt(500f)))
							idLevel [i] = 3;
						else if (newPower [i] > (100 - Mathf.Sqrt(2000f)))
							idLevel [i] = 2;
						break;
					case 4:
						newPower [i] = Mathf.RoundToInt((1 + randomPower * 0.01f) * this.Cards [i].Skills [0].Power);
						idSkill [i] = this.Cards [i].Skills [0].Id;
						if (newPower [i] >= 100 || newPower [i] > (100 - Mathf.Sqrt(500f)))
							idLevel [i] = 3;
						else if (newPower [i] > (100 - Mathf.Sqrt(2000f)))
							idLevel [i] = 2;
						break;
					case 5:
						newPower [i] = Mathf.RoundToInt((1 + randomPower * 0.01f) * this.Cards [i].Skills [0].Power);
						idSkill [i] = this.Cards [i].Skills [1].Id;
						if (newPower [i] >= 100 || newPower [i] > (100 - Mathf.Sqrt(500f)))
							idLevel [i] = 3;
						else if (newPower [i] > (100 - Mathf.Sqrt(2000f)))
							idLevel [i] = 2;
						break;
					case 6:
						newPower [i] = Mathf.RoundToInt((1 + randomPower * 0.01f) * this.Cards [i].Skills [0].Power);
						idSkill [i] = this.Cards [i].Skills [2].Id;
						if (newPower [i] >= 100 || newPower [i] > (100 - Mathf.Sqrt(500f)))
							idLevel [i] = 3;
						else if (newPower [i] > (100 - Mathf.Sqrt(2000f)))
							idLevel [i] = 2;
						break;
					case 7:
						newPower [i] = Mathf.RoundToInt((1 + randomPower * 0.01f) * this.Cards [i].Skills [0].Power);
						idSkill [i] = this.Cards [i].Skills [3].Id;
						if (newPower [i] >= 100 || newPower [i] > (100 - Mathf.Sqrt(500f)))
							idLevel [i] = 3;
						else if (newPower [i] > (100 - Mathf.Sqrt(2000f)))
							idLevel [i] = 2;
						break;
					default:
						break;
				}
			}
			idCards = idCards + this.Cards [i].Id.ToString() + "SEPARATOR";
			experienceCards = experienceCards + experience [i].ToString() + "SEPARATOR";
			newPowerCards = newPowerCards + newPower [i].ToString() + "SEPARATOR";
			attributeNameCards = attributeNameCards + attributeName [i] + "SEPARATOR";
			idSkillCards = idSkillCards + idSkill [i].ToString() + "SEPARATOR";
			idClassCards = idClassCards + this.Cards [i].IdClass.ToString() + "SEPARATOR";
			idLevelCards = idLevelCards + idLevel [i].ToString() + "SEPARATOR";
		}

		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_idcard", idCards);
		form.AddField("myform_xp", experienceCards);
		form.AddField("myform_newpower", newPowerCards);
		form.AddField("myform_attribute", attributeNameCards);
		form.AddField("myform_idskill", idSkillCards);
		form.AddField("myform_cardtype", idClassCards);
		form.AddField("myform_level", idLevelCards);
		
		WWW w = new WWW(URLUpdateXpCards, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		
		if (w.error != null)
		{
			Debug.Log(w.error); 											// donne l'erreur eventuelle
		} else
		{
			string[] data = w.text.Split(new string[] { "//" }, System.StringSplitOptions.None);
			
			for (int i=0; i<5; i++)
			{
				this.Cards [i].Experience = experience [i];
				if (attributeName [i] == "move")
				{
					this.Cards [i].MoveLevel = System.Convert.ToInt32(data [i]);
				}
				switch (randomAttribute [i])
				{
					case 0:
						this.Cards [i].Move = newPower [i];
						break;
					case 1:
						this.Cards [i].Life = newPower [i];
						this.Cards [i].LifeLevel = idLevel [i];
						break;
					case 2:
						this.Cards [i].Attack = newPower [i];
						this.Cards [i].AttackLevel = idLevel [i];
						break;
					case 3:
						this.Cards [i].Speed = newPower [i];
						this.Cards [i].SpeedLevel = idLevel [i];
						break;
					case 4:
						this.Cards [i].Skills [0].Power = newPower [i];
						this.Cards [i].Skills [0].Level = idLevel [i];
						break;
					case 5:
						this.Cards [i].Skills [1].Power = newPower [i];
						this.Cards [i].Skills [1].Level = idLevel [i];
						break;
					case 6:
						this.Cards [i].Skills [2].Power = newPower [i];
						this.Cards [i].Skills [2].Level = idLevel [i];
						break;
					case 7:
						this.Cards [i].Skills [3].Power = newPower [i];
						this.Cards [i].Skills [3].Level = idLevel [i];
						break;
					default:
						break;
				}
				this.Cards [i].ExperienceLevel = this.Cards [i].getXpLevel();
			}
		}
	}
}
