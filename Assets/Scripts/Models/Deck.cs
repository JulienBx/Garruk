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
	private static string URLEditDeck          = ApplicationModel.host + "update_deck_name.php";
	private static string URLCreateDeck        = ApplicationModel.host + "add_new_deck.php";
	private static string URLDeleteDeck        = ApplicationModel.host + "delete_deck.php";
	private static string URLAddCardToDeck     = ApplicationModel.host + "add_card_to_deck_by_user.php";
	private static string URLRemoveCardFromDeck= ApplicationModel.host + "remove_card_from_deck_by_user.php";
	private static string URLGetCardsByDeck    = ApplicationModel.host + "get_cards_by_deck.php";

	public int Id; 												// Id unique de la carte
	public string Name; 										// Nom du deck
	public int NbCards = 0; 										// Nombre de cartes présentes dans le deck
	public List<Card> Cards;									// Liste de carte du deck
	public string OwnerUsername;                                // Username de la personne possédant le deck

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
		WWWForm form = new WWWForm (); 						
		form.AddField ("myform_hash", ApplicationModel.hash);
		form.AddField ("myform_nick", ApplicationModel.username);
		form.AddField ("myform_deck", Id);
		form.AddField ("myform_idCard", idCard);
		WWW w = new WWW (URLAddCardToDeck, form); 								
		yield return w; 						
		
		if (w.error != null) 
		{
			Debug.Log(w.error);								
		} 
	}

	public void addCard(Card c)
	{
		this.Cards.Add (c);
	}

	public IEnumerator removeCard(int idCard)
	{
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField ("myform_deck", Id);
		form.AddField ("myform_idCard", idCard);
		WWW w = new WWW (URLRemoveCardFromDeck, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente

		if (w.error != null) 
		{
			Debug.Log(w.error); 									// donne l'erreur eventuelle
		}
	}

	public static IEnumerator create(string decksName)
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
			
			for(int i = 0 ; i < 1 ; i++)
			{
				string[] deckData = deckEntries[i].Split('\\'); 	// On découpe les attributs de la carte qu'on place dans un tableau
				int idDeck = System.Convert.ToInt32(deckData[0]);
				this.Id = idDeck;
			}	
		}
	}

	public IEnumerator retrieveCards(Action<string> callback)
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_deck", Id);                       // id du deck courant
		WWW w = new WWW(URLGetCardsByDeck, form); 					// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente

		if (w.error != null) 
		{
			Debug.Log(w.error); 									// donne l'erreur eventuelle
		}
		else
		{
			callback(w.text);
		}
	}

	public IEnumerator RetrieveCards() {
		Card c ;
		string[] cardData ;
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_deck", this.Id);							// Id du	 deck
		
		WWW w = new WWW(URLCards, form); 							// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log(w.error); 									// donne l'erreur eventuelle
		} 
		else 
		{
			Debug.Log (w.text);
			string[] cardEntries = w.text.Split('\n'); 				// Chaque ligne du serveur correspond à une carte
			
			for(int i = 0 ; i < cardEntries.Length - 1 ; i++) 		// On boucle sur les attributs d'une carte
			{
				Debug.Log (cardEntries[i]);
				cardData = cardEntries[i].Split('\\');
				if (!cardEntries[i].StartsWith("skill")){
					 	// On découpe les attributs de la carte qu'on place dans un tableau
					int cardId = System.Convert.ToInt32(cardData[0]); 	// Ici, on récupère l'id en base
					int cardArt = System.Convert.ToInt32(cardData[1]); 	// l'indice de l'image
					string cardTitle = cardData[2]; 					// le titre de la carte
					int cardLife = System.Convert.ToInt32(cardData[3]);	// le nombre de point de vie
					int speed = System.Convert.ToInt32(cardData[4]);	// la rapidité
					int move = System.Convert.ToInt32(cardData[5]);	    // le mouvement
					int attack = System.Convert.ToInt32(cardData[6]);	// l'attaque
					//int energy = System.Convert.ToInt32(cardData[7]);	// l'attaque
					
					c = new Card(cardId, cardTitle, cardLife, cardArt, speed, move, attack, new List<Skill>());
					this.addCard(c);
					NbCards ++;
				}
				else{
					this.Cards[NbCards-1].Skills.Add(new Skill(cardData[1], System.Convert.ToInt32(cardData[2]), System.Convert.ToInt32(cardData[3]), System.Convert.ToInt32(cardData[4]), System.Convert.ToInt32(cardData[5]), System.Convert.ToInt32(cardData[6]), cardData[7]));
				}
			}
		}
	}

	public IEnumerator RetrieveCardIDs() {
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_deck", this.Id);							// Id du	 deck
		
		WWW w = new WWW(URLGetCardIDS, form); 							// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log(w.error); 									// donne l'erreur eventuelle
		} 
		else 
		{
			string[] cardEntries = w.text.Split('\n'); 				// Chaque ligne du serveur correspond à une carte
			
			for(int i = 0 ; i < cardEntries.Length - 1 ; i++)
			{
				this.addCard(new Card(System.Convert.ToInt32(cardEntries[i])));
				this.NbCards = i + 1;

				NbCards = i + 1;
			}
		}
	}
}
