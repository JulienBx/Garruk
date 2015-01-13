using UnityEngine;
using System.Collections;

public class MyDecksScript : MonoBehaviour {

	public GameObject DeckObject;									// Prefab de la carte vide
	public string URL; 												// L'url de récupération des decks du joueur sur le serveur
	
	void Start() {
		
	}
	
	void Awake()
	{
		StartCoroutine(RetrieveDecks()); 							// Appel de la coroutine pour récupérer les decks de l'utilisateur sur le serveur distant		
	}
	
	IEnumerator RetrieveDecks() {

		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		
		WWW w = new WWW(URL, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			print(w.text); 											// donne le retour
			
			string[] cardEntries = w.text.Split('\n'); 				// Chaque ligne du serveur correspond à un deck
			
			for(int i = 0 ; i < cardEntries.Length - 1 ; i++) 		// On boucle sur les attributs d'un deck
			{
				string[] deckData = cardEntries[i].Split('\\'); 	// On découpe les attributs du deck qu'on place dans un tableau
				
				int deckId = System.Convert.ToInt32(deckData[0]); 	// Ici, on récupère l'id en base
				string deckName = deckData[1]; 						// Le nom du deck
				int deckNbC = System.Convert.ToInt32(deckData[2]);	// le nombre de cartes
				
				Deck deck = new Deck(deckId, deckName, deckNbC);
				
				
				GameObject instance = 
					Instantiate(DeckObject) as GameObject;			// On charge une instance du prefab Deck
				instance.transform.localScale = 
					new Vector3(0.23f, 0.02f, 0.30f);				// On change ses attributs d'échelle ...																	
				instance.transform.localPosition = 
					new Vector3(-8 + (3 * i), 3, 0);					// ..., de positionnement ...
				instance.GetComponent<GameDeck>().Deck = deck;		// ... et le deck qu'elle représente
				instance.GetComponent<GameDeck>().Show();			// On affiche le deck
			}
		}
	}
	
	void OnGUI()
	{
		if (GUI.Button(new Rect(10, 10, 100, 20), "Accueil"))		// Au clic sr le bouton d'accueil
		{
			Application.LoadLevel("HomePage");						// On charge la page d'accueil
		}
		
	}
}
