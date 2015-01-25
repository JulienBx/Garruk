﻿using UnityEngine;
using System.Collections;

public class GameCard : MonoBehaviour 
{
	public Texture[] faces; 										// Les différentes images des cartes
	public Card Card; 												// L'instance de la carte courante 

	//private string URLCards = ApplicationModel.host + "get_cards_from_deck.php";
	private string URLCard = "http://localhost/GarrukServer/get_card.php";

	public GameCard() {
	}

	public GameCard(Card card) 
	{
		this.Card = card;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowFace() 
	{
		renderer.material.mainTexture = faces[Card.ArtIndex]; 		// On affiche l'image correspondant à la carte
		transform.Find("Title")
			.GetComponent<TextMesh>().text = Card.Title;			// On lui attribut son titre
		transform.Find("Life")
			.GetComponent<TextMesh>().text = Card.Life.ToString();	// Et son nombre de point de vie
	}
	public void Hide()
	{
		renderer.material.mainTexture = faces[0]; 		// On affiche l'image correspondant à la carte
		transform.Find("Title")
			.GetComponent<TextMesh>().text = "Title";			// On lui attribut son titre
		transform.Find("Life")
			.GetComponent<TextMesh>().text = "Life";	// Et son nombre de point de vie
	}

//	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
//		int idArt = 0;
//		if (stream.isWriting) {
//			idArt = Card.ArtIndex;
//			stream.Serialize(ref idArt);
//		} else {
//			stream.Serialize(ref idArt);
//			Card.ArtIndex = idArt;
//			ShowFace();
//		}
//	}


//	public IEnumerator RetrieveCard (int idCard)
//	{
//		WWWForm form = new WWWForm(); 								// Création de la connexion
//		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
//		form.AddField("myform_idcard", idCard);						// ID de la carte
//		
//		WWW w = new WWW(URLCard, form); 								// On envoie le formulaire à l'url sur le serveur 
//		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
//		if (w.error != null) 
//		{
//			print(w.error); 										// donne l'erreur eventuelle
//		} 
//		else 
//		{
//			print(w.text); 											// donne le retour
//			
//			string[] cardEntries = w.text.Split('\n'); 				// Chaque ligne du serveur correspond à une carte
//			
//			for(int i = 0 ; i < cardEntries.Length - 1 ; i++) 		// On boucle sur les attributs d'une carte
//			{
//				string[] cardData = cardEntries[i].Split('\\'); 	// On découpe les attributs de la carte qu'on place dans un tableau
//				if (cardData.Length < 2) 
//				{
//					break;
//				}
//				int cardId = System.Convert.ToInt32(cardData[0]); 	// Ici, on récupère l'id en base
//				int cardArt = System.Convert.ToInt32(cardData[1]); 	// l'indice de l'image
//				string cardTitle = cardData[2]; 					// le titre de la carte
//				int cardLife = System.Convert.ToInt32(cardData[3]);	// le nombre de point de vie
//				
//				Card card = new Card(cardId, cardTitle, cardLife, cardArt);
//				this.Card = card;
//			}
//		}
//	}
}
