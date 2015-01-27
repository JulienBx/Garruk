using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck 
{

	public int Id; 												// Id unique de la carte
	public string Name; 										// Nom du deck
	public int NbCards; 										// Nombre de cartes présentes dans le deck
	public List<Card> Cards;									// Liste de carte du deck

	private string URLCards = ApplicationModel.host + "get_cards_by_deck.php";
	//private string URLCards = "http://localhost/GarrukServer/get_cards_by_deck.php";

	public Deck(int id) 
	{
		this.Id = id;
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

	public void AddCard(Card card)
	{
		Cards.Add(card);
	}

	public IEnumerator RetrieveCards() {
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		//form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_deck", Id);							// Id du	 deck
		
		WWW w = new WWW(URLCards, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log(w.error); 										// donne l'erreur eventuelle
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
				int cardId = System.Convert.ToInt32(cardData[0]); 	// Ici, on récupère l'id en base
				int cardArt = System.Convert.ToInt32(cardData[1]); 	// l'indice de l'image
				string cardTitle = cardData[2]; 					// le titre de la carte
				int cardLife = System.Convert.ToInt32(cardData[3]);	// le nombre de point de vie
				
				Card card = new Card(cardId, cardTitle, cardLife, cardArt);
				AddCard(card);
				NbCards = i + 1;
			}
		}
	}
}
