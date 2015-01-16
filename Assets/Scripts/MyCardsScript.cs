using UnityEngine; 
using System.Collections;
using System.Collections.Generic;

public class MyCardsScript : MonoBehaviour
{
	public GameObject CardObject;									// Prefab de la carte vide
	public string URL; 												// L'url de récupération des cartes du joueur sur le serveur
	
	void Start() {

	}

	void Awake()
	{
		StartCoroutine(RetrieveCards()); 							// Appel de la coroutine pour récupérer les cartes de l'utilisateur sur le serveur distant		
	}

	IEnumerator RetrieveCards() {
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_deck", ApplicationModel.selectedDeck.Id);// Deck en cours
		
		WWW w = new WWW(URL, form); 								// On envoie le formulaire à l'url sur le serveur 
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

				//int cardId = System.Convert.ToInt32(cardData[0]); 	// Ici, on récupère l'id en base
				int cardArt = System.Convert.ToInt32(cardData[1]); 	// l'indice de l'image
				string cardTitle = cardData[2]; 					// le titre de la carte
				int cardLife = System.Convert.ToInt32(cardData[3]);	// le nombre de point de vie
												
				Card card = new Card(cardTitle, cardLife, cardArt);

																	
				GameObject instance = 
					Instantiate(CardObject) as GameObject;			// On charge une instance du prefab Card
				instance.transform.localScale = 
					new Vector3(0.15f, 0.02f, 0.20f);				// On change ses attributs d'échelle ...																	
				instance.transform.localPosition = 
					new Vector3(-8 + (2 * i), 3, 0);				// ..., de positionnement ...
				instance.GetComponent<GameCard>().Card = card;		// ... et la carte qu'elle représente
				instance.GetComponent<GameCard>().ShowFace();		// On affiche la carte
			}
		}
	}
	
	void OnGUI()
	{
		if (GUI.Button(new Rect(10, 10, 100, 20), "Accueil"))		// Au clic sr le bouton d'accueil
		{
			Application.LoadLevel("HomePage");						// On charge la page d'accueil
		}
		if(Application.loadedLevelName.Equals("MyDeckPage")) 
		{
			if (GUI.Button(new Rect(120, 10, 100, 20), "Mes decks"))	// bouton pour aller sur la page "Mes decks"
			{ 
				Application.LoadLevel("MyDecksPage"); 				// Chargement de la page "Mes decks"
			}
		}

	
	}
}