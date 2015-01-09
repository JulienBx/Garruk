using UnityEngine; 
using System.Collections;
using System.Collections.Generic;

public class MyCardsScript : MonoBehaviour
{
	private List<Card> cards = new List<Card>();

	public string URL = "http://localhost/GarrukServer/get_cards_by_user.php"; // L'url d'authentification du serveur
	
	void Start()
	{
		//StartCoroutine(RetrieveCards());
		//DisplayCards ();

	}

	void Awake()
	{
		StartCoroutine(RetrieveCards());
		//DisplayCards ();
		
	}

	IEnumerator RetrieveCards() {
		WWWForm form = new WWWForm(); // Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); // hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); // Pseudo de l'utilisateur connecté
		
		WWW w = new WWW(URL, form); // On envoie le formulaire à l'url sur le serveur 
		yield return w; // On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) {
			print(w.error); // donne l'erreur eventuelle
		} else {
			print("echo"); // donne l'erreur eventuelle
			print(w.text); // donne le retour

			string[] cardEntries = w.text.Split('\n');

			for(int i = 0 ; i < cardEntries.Length - 1 ; i++)
			{
				string[] cardData = cardEntries[i].Split('\\');

				int cardId = System.Convert.ToInt32(cardData[0]);
				string cardArt = cardData[1];
				string cardTitle = cardData[2];
				int cardLife = System.Convert.ToInt32(cardData[3]);
				GameObject cardFront = (GameObject)Resources.Load("Prefabs/" + cardArt, typeof(GameObject));

				Card card = new Card(cardId, cardArt, cardTitle, cardLife, cardFront);

				cards.Add(card);

				GameObject instance = Instantiate((GameObject)card.front) as GameObject;
				instance.transform.localScale = new Vector3(15, 2, 20);
				instance.transform.localPosition = new Vector3(-800 + (200 * i), 300, 0);
			}
		}
	}

	void DisplayCards() {
		foreach (Card card in cards) {
			//GameObject g = GameObject.Instantiate(gameCard.front,new Vector3(0,0,0),Quaternion.identity) as GameObject;

			//GameCard gameCard = g.GetComponent<GameCard>();
			Instantiate((GameObject)card.front);
		}
	}
	
	void OnGUI()
	{

		// Deal button
		if (GUI.Button(new Rect(10, 10, 100, 20), "Accueil"))
		{
			Application.LoadLevel("HomePage");
		}
	
	}
}