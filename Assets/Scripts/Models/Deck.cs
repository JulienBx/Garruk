using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Deck : Cards 
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
	private static string URLChangeCardsOrder = ApplicationModel.host + "change_cards_order.php"; 

	public int Id; 												// Id unique de la carte
	public string Name; 										// Nom du deck
	public string OwnerUsername;                                // Username de la personne possédant le deck
	public int NbCards; 
	public string Error;
	
	public Deck()
	{
	
	}
	
	public Deck(int id)
	{
		this.Id = id;
		this.cards = new List<Card>();
	}

	public Deck(string username)
	{
		OwnerUsername = username;
		this.cards = new List<Card>();
	}

	public Deck(int id, string name)
	{
		this.Id = id;
		this.Name = name;
		this.cards = new List<Card>();
	}

	public Deck(int id, string name, List<Card> cards)
	{
		this.Name = name;
		this.cards = cards;
	}

	public IEnumerator addCard(int idCard, int deckOrder)
	{
		WWWForm form = new WWWForm(); 						
		form.AddField("myform_hash", ApplicationModel.hash);
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField("myform_deck", Id);
		form.AddField("myform_idCard", idCard);
		form.AddField("myform_deckOrder", deckOrder);
		WWW w = new WWW(URLAddCardToDeck, form); 								
		yield return w; 						

		if (w.error != null)
		{ 
			Debug.Log (w.error); 
		}

		this.cards.Add(new Card(idCard));
		this.cards[this.cards.Count-1].deckOrder=deckOrder;
	}
	public IEnumerator changeCardsOrder(int idCard1, int deckOrder1, int idCard2, int deckOrder2)
	{
		WWWForm form = new WWWForm(); 						
		form.AddField("myform_hash", ApplicationModel.hash);
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField("myform_deck", Id);
		form.AddField("myform_idCard1", idCard1.ToString());
		form.AddField("myform_deckOrder1", deckOrder1.ToString());
		form.AddField("myform_idCard2", idCard2.ToString());
		form.AddField("myform_deckOrder2", deckOrder2.ToString());
		WWW w = new WWW(URLChangeCardsOrder, form); 								
		yield return w; 						
		
		if (w.error != null)
		{
			Debug.Log(w.error);								
		} 

		for (int i =0;i<this.cards.Count;i++)
		{
			if(this.cards[i].Id==idCard1)
			{
				this.cards[i].deckOrder=deckOrder1;
			}
			if(this.cards[i].Id==idCard2)
			{
				this.cards[i].deckOrder=deckOrder2;
			}
		}
	}
	public void addCard(Card c)
	{
		this.cards.Add(c);
	}

	public IEnumerator removeCard(int idCard)
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.player.Username); 	// Pseudo de l'utilisateur connecté
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
			for (int i=0; i<this.cards.Count; i++)
			{
				if (this.cards [i].Id == idCard)
				{
					this.cards.RemoveAt(i);
					break;
				}
			}
		}
	}

	public IEnumerator create(string decksName)
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.player.Username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_name", decksName);

		ServerController.instance.setRequest(URLCreateDeck, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		this.Error=ServerController.instance.getError();

		if(this.Error=="")
		{
			string result = ServerController.instance.getResult();
			this.Id = System.Convert.ToInt32(result);
			this.Name = decksName;
			this.NbCards = 0;
			this.cards = new List<Card>();
		}
	}
	public IEnumerator delete()
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.player.Username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_id", Id);

		ServerController.instance.setRequest(URLDeleteDeck, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		this.Error=ServerController.instance.getError();
	}

	public IEnumerator edit(string newName)
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.player.Username); 	// Pseudo de l'utilisateur connecté
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
		this.cards = new List<Card> ();
		string[] cardsData;
		
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_deck", this.Id);							// Id du	 deck


		ServerController.instance.setRequest(URLCards, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		this.Error=ServerController.instance.getError();

		if(this.Error=="")
		{
			string result = ServerController.instance.getResult();
			cardsData = result.Split(new string[] { "#CARD#" }, System.StringSplitOptions.None);
			for(int i = 0 ; i < cardsData.Length ; i++){
				this.cards.Add(new Card());
				this.cards[i].parseCard(cardsData[i]);
				this.cards[i].deckOrder=i;
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
			}
		}
	}
}
