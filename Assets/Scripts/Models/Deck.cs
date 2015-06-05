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
	private static string URLAddXpToDeck = ApplicationModel.host + "add_xp_to_deck.php";

	public int Id; 												// Id unique de la carte
	public string Name; 										// Nom du deck
	public int NbCards = 0; 										// Nombre de cartes présentes dans le deck
	public List<Card> Cards;									// Liste de carte du deck
	public string OwnerUsername;                                // Username de la personne possédant le deck
	public int CollectionPoints;

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
					c.ExperienceLevel = System.Convert.ToInt32(cardData [12]);
					c.PercentageToNextLevel = System.Convert.ToInt32(cardData [13]);

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
	public IEnumerator addXpToDeck(int earnXp)
	{

		string idCards = "";

		for (int i=0; i<5; i++)
		{
			idCards = idCards + this.Cards [i].Id.ToString() + "SEPARATOR";
		}

		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_idcard", idCards);
		form.AddField("myform_xp", earnXp);
		form.AddField("myform_nick", ApplicationModel.username); 
		
		WWW w = new WWW(URLAddXpToDeck, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		
		if (w.error != null)
		{
			Debug.Log(w.error); 											// donne l'erreur eventuelle
		}
		else
		{
			string [] cardsData = w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.CollectionPoints=System.Convert.ToInt32(cardsData[5]);
			for(int i=0;i<5;i++)
			{
				string [] cardData =  cardsData[i].Split(new string[] { "#S#" }, System.StringSplitOptions.None);
				for(int j = 0 ; j < cardData.Length-1 ; j++)
				{
					string[] cardInfo = cardData[j].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
					if (j==0)
					{
						
						this.Cards[i].Life=System.Convert.ToInt32(cardInfo[0]);
						this.Cards[i].Attack=System.Convert.ToInt32(cardInfo[1]);
						this.Cards[i].Speed=System.Convert.ToInt32(cardInfo[2]);
						this.Cards[i].Move=System.Convert.ToInt32(cardInfo[3]);
						this.Cards[i].LifeLevel=System.Convert.ToInt32(cardInfo[4]);
						this.Cards[i].MoveLevel=System.Convert.ToInt32(cardInfo[5]);
						this.Cards[i].SpeedLevel=System.Convert.ToInt32(cardInfo[6]);
						this.Cards[i].AttackLevel=System.Convert.ToInt32(cardInfo[7]);
						this.Cards[i].Experience=System.Convert.ToInt32(cardInfo[8]);
						this.Cards[i].ExperienceLevel=System.Convert.ToInt32(cardInfo[9]);
						this.Cards[i].NextLevelPrice=System.Convert.ToInt16(cardInfo[10]);
						this.Cards[i].PercentageToNextLevel=System.Convert.ToInt16(cardInfo[11]);
					}
					else
					{
						this.Cards[i].Skills[j-1].Level=System.Convert.ToInt32(cardInfo[0]);
						this.Cards[i].Skills[j-1].Power=System.Convert.ToInt32(cardInfo[1]);
						this.Cards[i].Skills[j-1].Description=cardInfo[2];
					}
				}
			}
		}
	}
}
