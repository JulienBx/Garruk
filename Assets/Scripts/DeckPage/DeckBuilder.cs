using UnityEngine;
using System.Collections;
using UnityEditor;

public class DeckBuilder : MonoBehaviour {

	public GameObject[] Cards;
	public string URL;
	public GameObject CardObject;
	public int CardsCount = 0;
	public float speed;
	
	private GameObject deck;
	private bool cardIsMoving = false;
	private GameObject movingCard;
	private Vector3 targetPosition;

	// Use this for initialization
	void Start () {
		StartCoroutine(RetrieveCardsFromDeck()); 							// Appel de la coroutine pour récupérer les cartes de l'utilisateur sur le serveur distant		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) 
		{
			if (!cardIsMoving) 
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				
				if (Physics.Raycast(ray, out hit))
				{
					GameObject clickedCard = hit.transform.gameObject;

					if (clickedCard.GetComponent<GameCard>().Card != null && clickedCard.transform.IsChildOf(transform))
					{
						movingCard = Instantiate(clickedCard, clickedCard.transform.position, clickedCard.transform.rotation) as GameObject;
						CardsCount--;
						//clickedCard.GetComponent<MeshRenderer>().material.mainTexture = 
						//	clickedCard.GetComponent<GameCard>().faces[0];
						clickedCard.renderer.material = Instantiate(clickedCard.renderer.material) as Material;	
						clickedCard.GetComponent<GameCard>().Card = null;
						clickedCard.GetComponent<GameCard>().Hide();
						RemainingCards rCards = GameObject.Find("Cards User Area").GetComponent<RemainingCards>();
						targetPosition = rCards.lastRemainingPosition;

						if (rCards.lastRemainingPosition.x != 40) 
						{
							rCards.lastRemainingPosition.x +=12;
						}
						else 
						{
							rCards.lastRemainingPosition.x = -44;
							rCards.lastRemainingPosition.y += -16;
						}

						targetPosition.x += 12;
						targetPosition.z = -4;
						cardIsMoving = true;
					}
				}
			}
		}
		if (cardIsMoving) 
		{
			//Vector3 targetPosition = new Vector3(targetCard.transform.position.x, targetCard.transform.position.y, movingCard.transform.position.z);
			movingCard.transform.position = Vector3.MoveTowards(movingCard.transform.position, targetPosition, speed * Time.deltaTime);
			if (Vector3.Distance(movingCard.transform.position, targetPosition) < 0.001f)
			{
				cardIsMoving = false;
			//	GameCard targetGameCard = targetCard.GetComponent<GameCard>();
			//	targetGameCard.Card = movingCard.GetComponent<GameCard>().Card;
			//	targetGameCard.ShowFace();
			//	Destroy(movingCard);
			}
		}

	}

	private IEnumerator RetrieveCardsFromDeck()
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_deck", ApplicationModel.selectedDeck);// Pseudo de l'utilisateur connecté
		
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
				AddCard(i, card);
				CardsCount = i;
			}
		}
	}

	private void AddCard(int index, Card card)
	{
		GameCard gCard = Cards[index].GetComponent<GameCard>();		// On récupère l'instance de la GameCard
		gCard.Card = card;											// On défini la carte qu'elle représente
		gCard.ShowFace();											// On affiche la carte
	}
}
