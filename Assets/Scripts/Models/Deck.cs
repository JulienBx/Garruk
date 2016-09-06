using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] 
public class Deck : Cards 
{
	//Interconnexion BDD
	private static string URLCards = ApplicationModel.host + "get_cards_by_deck_by_user.php";
	private static string URLEditDeck = ApplicationModel.host + "update_deck_name.php";
	private static string URLCreateDeck = ApplicationModel.host + "add_new_deck.php";
	private static string URLDeleteDeck = ApplicationModel.host + "delete_deck.php";
	private static string URLAddCardToDeck = ApplicationModel.host + "add_card_to_deck_by_user.php";
	private static string URLRemoveCardFromDeck = ApplicationModel.host + "remove_card_from_deck_by_user.php";
	private static string URLChangeCardsOrder = ApplicationModel.host + "change_cards_order.php"; 

	public int Id; 												// Id unique de la carte
	public string Name; 										// Nom du deck
	public string OwnerUsername;                                // Username de la personne possédant le deck
	public int NbCards; 
	public string Error;
	public bool ToDelete;
	public bool IsNew;
	public int OwnerId;
	
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
		bool isConnected = true;
		if (ApplicationModel.player.IsOnline) {
			WWWForm form = new WWWForm (); 						
			form.AddField ("myform_hash", ApplicationModel.hash);
			form.AddField ("myform_nick", ApplicationModel.player.Username);
			form.AddField ("myform_deck", Id);
			form.AddField ("myform_idCard", idCard);
			form.AddField ("myform_deckOrder", deckOrder);
			WWW w = new WWW (URLAddCardToDeck, form); 								
			yield return w; 						

			if (w.error != null) { 
				Debug.Log (w.error); 
				isConnected=false;
			}
		}
		this.cards.Add(new Card(idCard));
		this.cards[this.cards.Count-1].deckOrder=deckOrder;
		if (!isConnected || !ApplicationModel.player.IsOnline) {
			Deck deck = this;
			ApplicationModel.player.decksToSync.update (deck);
		}
		ApplicationModel.Save ();
		yield break;
	}
	public IEnumerator changeCardsOrder(int idCard1, int deckOrder1, int idCard2, int deckOrder2)
	{
		bool isConnected = true;
		if (ApplicationModel.player.IsOnline) {
			WWWForm form = new WWWForm (); 						
			form.AddField ("myform_hash", ApplicationModel.hash);
			form.AddField ("myform_nick", ApplicationModel.player.Username);
			form.AddField ("myform_deck", Id);
			form.AddField ("myform_idCard1", idCard1.ToString ());
			form.AddField ("myform_deckOrder1", deckOrder1.ToString ());
			form.AddField ("myform_idCard2", idCard2.ToString ());
			form.AddField ("myform_deckOrder2", deckOrder2.ToString ());
			WWW w = new WWW (URLChangeCardsOrder, form); 								
			yield return w; 						
			
			if (w.error != null) {
				Debug.Log (w.error);
				isConnected=false;
			} 
		}
		for (int i = 0; i < this.cards.Count; i++) {
			if (this.cards [i].Id == idCard1) {
				this.cards [i].deckOrder = deckOrder1;
			}
			if (this.cards [i].Id == idCard2) {
				this.cards [i].deckOrder = deckOrder2;
			}
		}
		if (!isConnected || !ApplicationModel.player.IsOnline) {
			Deck deck = this;
			ApplicationModel.player.decksToSync.update (deck);
		}
		ApplicationModel.Save ();
		yield break;
	}
	public void addCard(Card c)
	{
		this.cards.Add(c);
	}

	public IEnumerator removeCard(int idCard)
	{
		bool isConnected = true;
		if (ApplicationModel.player.IsOnline) 
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
				isConnected=false;
			} 
		}
		this.NbCards--;
		for (int i=0; i<this.cards.Count; i++)
		{
			if (this.cards [i].Id == idCard)
			{
				this.cards.RemoveAt(i);
				break;
			}
		}
		if (!isConnected || !ApplicationModel.player.IsOnline) {
			Deck deck = this;
			ApplicationModel.player.decksToSync.update (deck);
		}
		ApplicationModel.Save ();
		yield break;
	}

	public IEnumerator create(string decksName)
	{
		bool isConnected = true;
		if (ApplicationModel.player.IsOnline) 
		{
			WWWForm form = new WWWForm (); 								// Création de la connexion
			form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
			form.AddField ("myform_nick", ApplicationModel.player.Username); 	// Pseudo de l'utilisateur connecté
			form.AddField ("myform_name", decksName);

			ServerController.instance.setRequest (URLCreateDeck, form);
			yield return ServerController.instance.StartCoroutine ("executeRequest");
			this.Error = ServerController.instance.getError ();
			if (this.Error != "") {
				isConnected = false;
			}
		}
		//string result = ServerController.instance.getResult ();
		//this.Id = System.Convert.ToInt32 (result);
		this.Name = decksName;
		this.NbCards = 0;
		this.cards = new List<Card> ();
		if (!isConnected || !ApplicationModel.player.IsOnline) {
			Deck deck = this;
			deck.IsNew = true;
			ApplicationModel.player.decksToSync.update (deck);
		}
		ApplicationModel.Save ();
		yield break;
	}
	public IEnumerator delete()
	{
		bool isConnected = true;
		if (ApplicationModel.player.IsOnline) {
			WWWForm form = new WWWForm (); 								// Création de la connexion
			form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
			form.AddField ("myform_nick", ApplicationModel.player.Username); 	// Pseudo de l'utilisateur connecté
			form.AddField ("myform_id", Id);
			WWW w = new WWW(URLDeleteDeck, form); 				// On envoie le formulaire à l'url sur le serveur 
			yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
			if (w.error != null)
			{
				Debug.Log(w.error); 									// donne l'erreur eventuelle
				isConnected=false;
			} 
		}
		if (!isConnected || !ApplicationModel.player.IsOnline) {
			Deck deck = this;
			deck.ToDelete = true;
			ApplicationModel.player.decksToSync.update (deck);
		}
		ApplicationModel.Save ();
		yield break;
	}
	public IEnumerator edit(string newName)
	{
		bool isConnected = true;
		if (ApplicationModel.player.IsOnline) 
		{
			WWWForm form = new WWWForm (); 								// Création de la connexion
			form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
			form.AddField ("myform_nick", ApplicationModel.player.Username); 	// Pseudo de l'utilisateur connecté
			form.AddField ("myform_id", Id);
			form.AddField ("myform_name", newName);
			WWW w = new WWW(URLEditDeck, form); 				// On envoie le formulaire à l'url sur le serveur 
			yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
			if (w.error != null)
			{
				Debug.Log(w.error); 									// donne l'erreur eventuelle
				isConnected=false;
			} 
		}
		this.Name = newName;
		if (!isConnected || !ApplicationModel.player.IsOnline) {
			Deck deck = this;
			ApplicationModel.player.decksToSync.update (deck);
		}
		ApplicationModel.Save ();
		yield break;
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
	public void setString()
	{
		this.String="";
		this.String += this.Id.ToString()+"DATA"; //0
		this.String += this.Name+"DATA";
		this.String += ApplicationModel.player.Username.ToString() + "DATA";
		this.String += this.NbCards.ToString () + "DATA";
		this.String += System.Convert.ToInt32 (this.ToDelete).ToString ()+"DATA";
		this.String += System.Convert.ToInt32 (this.IsNew).ToString () + "DATA";

		for (int i = 0; i < this.cards.Count; i++) 
		{
			this.String += this.cards [i].Id.ToString () + "DATA";
			this.String += this.cards [i].deckOrder.ToString() + "DATA";
		}
	}
}
