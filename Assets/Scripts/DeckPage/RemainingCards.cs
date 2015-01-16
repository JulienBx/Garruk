using UnityEngine;
using System.Collections;

public class RemainingCards : MonoBehaviour {

	public string URLCards;					// URL de récupération des cartes
	public string URLAddCard;				// URL pour ajouter une carte dans un deck
	public GameObject CardObject;			// Prefab d'une carte	
	public float speed;						// Vitesse de déplacement d'une carte

	private GameObject deck;				// le deck courant
	private bool cardIsMoving = false;		// Variable permettant le déplacement d'une carte à la fois en mm temps
	private int nbCardsInDeck = 0;
	private GameObject targetCard;			// L'emplacement de la carte dans le deck
	private GameObject movingCard;			// La carte qui va etre ajouté au deck
	public Vector3 lastRemainingPosition;	// Position possible pour ajouter une carte dans la liste des cartes disponibles

	// Use this for initialization
	void Start () 
	{
		StartCoroutine(RetrieveCardsNotInDeck()); // Nouvelle coroutine pour récupérer la liste des cartes qui ne sont pas déjà dans le deck 
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) 						// Au clic gauche
		{
			// le but de cette condition est d'ajouter une carte dans un deck
			if (!cardIsMoving) 									// test s'il n'y a pas déjà une carte en mouvement
			{
				RaycastHit hit;									// création d'un raycast pour detecter ce que la souris à cliqué
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // on trace un rayon sur le pointeur de la souris
				
				if (Physics.Raycast(ray, out hit)) 				// test si le rayon rencontre un collider
				{
					movingCard = hit.transform.gameObject; 		// récupèration l'objet cliqué
					
					if (movingCard.transform.IsChildOf(transform) && movingCard.GetComponent<GameCard>() != null) // test s'il sagit bien d'une carte
					{
						deck = GameObject.Find("DeckBuilder");	// récupèration dans la scène l'object Deckbuilder
						nbCardsInDeck = deck.GetComponent<DeckBuilder>().CardsCount; // on récupère le nombres de carte déjà présentes dans le deck
						if (nbCardsInDeck < 5)					// test s'il y en a pas déjà 5
						{
							targetCard = deck.GetComponent<DeckBuilder>().Cards[nbCardsInDeck]; // récupèration le furur emplacement pour la carte sélectionnée
							cardIsMoving = true;													// positionnement du drapeau à true pour ne pas pouvoir bouger 2 cartes en mm temps
							deck.GetComponent<DeckBuilder>().CardsCount++;							// incrémentation du nombre de cartes dans le deck

							StartCoroutine(AddCardToDeck(nbCardsInDeck));						// Nouvelle coroutine pour sauvegarder cette carte dans le deck en base
						}
					}
				}
			}
		}
		if (cardIsMoving) 										// Mouvement de la carte cliquée
		{
			Vector3 targetPosition = 							// Récupération des coordonnées de l'emplacement
				new Vector3(targetCard.transform.position.x, 
				            targetCard.transform.position.y, 
				            movingCard.transform.position.z);
													
			movingCard.transform.position = 					// translation de la carte vers son futur emplacement dans le deck
				Vector3.MoveTowards(movingCard.transform.position,
				                    targetPosition,
				                    speed * Time.deltaTime);

			if (Vector3.Distance(movingCard.transform.position, targetPosition) < 0.001f) // test si la carte "est arrivée" dans le deck 
			{
				cardIsMoving = false;												// Si oui alors repositionnement du flag à false
				GameCard targetGameCard = targetCard.GetComponent<GameCard>();		// Récupération du game object de la carte
				targetGameCard.Card = movingCard.GetComponent<GameCard>().Card;		// 
				targetGameCard.ShowFace();
				Destroy(movingCard);
			}
		}

	}
	
	private IEnumerator RetrieveCardsNotInDeck()
	{
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField ("myform_deck", ApplicationModel.selectedDeck.Id);// Deck sélectionné
		
		WWW w = new WWW (URLCards, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) {
			print (w.error); 										// donne l'erreur eventuelle
		} else {
			print (w.text);											// donne le retour
			
			string[] cardEntries = w.text.Split ('\n');				// Chaque ligne du serveur correspond à une carte

			int j = -1;
			int k = 0;
			for (int i = 0; i < cardEntries.Length - 1; i++) 		// On boucle sur les attributs d'une carte
			{ 	
				string[] cardData = cardEntries [i].Split ('\\'); 	// On découpe les attributs de la carte qu'on place dans un tableau
				if (cardData.Length < 2) 
				{
					break;
				}
				int cardId = System.Convert.ToInt32(cardData[0]); // Ici, on récupère l'id en base
				int cardArt = System.Convert.ToInt32(cardData[1]);// l'indice de l'image
				string cardTitle = cardData[2]; 					// le titre de la carte
				int cardLife = System.Convert.ToInt32(cardData[3]);// le nombre de point de vie
				
				Card card = new Card (cardId, cardTitle, cardLife, cardArt);

				GameObject instance = 
					Instantiate(CardObject) as GameObject;			// On charge une instance du prefab Card
				if ((j * 12) > 80) 
				{
					j = 0;
					k++;
				}
				else 
				{
					j++;
				}

				lastRemainingPosition = 
					new Vector3(-32 + (12 * j), 31 + (k * -16), -5);	
				instance.transform.localPosition = 	
					lastRemainingPosition;							// ..., de positionnement ...
				instance.GetComponent<GameCard>().Card = card;		// ... et la carte qu'elle représente
				instance.GetComponent<GameCard>().ShowFace();		// On affiche la carte
				instance.transform.parent = transform;
			}
		}
	}

	private IEnumerator AddCardToDeck(int cardsCount)
	{		
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField ("myform_deck", 								// deck en cours
		               ApplicationModel.selectedDeck.Id.ToString());
		form.AddField ("myform_idCard", 							// Pseudo de l'utilisateur connecté
		               movingCard.GetComponent<GameCard>().Card.Id);
		form.AddField ("myform_nbCards", cardsCount + 1);			// Pseudo de l'utilisateur connecté
		
		WWW w = new WWW (URLAddCard, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			print (w.error); 										// donne l'erreur eventuelle
		} else {
			print (w.text);
		}
	}
}

