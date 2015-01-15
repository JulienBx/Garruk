using UnityEngine;
using System.Collections;

public class RemainingCards : MonoBehaviour {

	public string URL;
	public int CardsCount = 0;
	public GameObject CardObject;
	public float speed;

	private GameObject deck;
	private bool cardIsMoving = false;
	private int nbCardsInDeck = 0;
	private GameObject targetCard;
	private GameObject movingCard;
	public Vector3 lastRemainingPosition;
	// Use this for initialization
	void Start () 
	{
		StartCoroutine(RetrieveCardsNotInDeck());
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
					movingCard = hit.transform.gameObject;
					
					if (movingCard.transform.IsChildOf(transform))
					{
						deck = GameObject.Find("DeckBuilder");
						nbCardsInDeck = deck.GetComponent<DeckBuilder>().CardsCount++;
						if (nbCardsInDeck < 4)
						{
							targetCard = deck.GetComponent<DeckBuilder>().Cards[nbCardsInDeck + 1];
							cardIsMoving = true;
						}
					}
				}
			}

		}
		if (cardIsMoving) 
		{
			Vector3 targetPosition = new Vector3(targetCard.transform.position.x, targetCard.transform.position.y, movingCard.transform.position.z);
			movingCard.transform.position = Vector3.MoveTowards(movingCard.transform.position, targetPosition, speed * Time.deltaTime);
			if (Vector3.Distance(movingCard.transform.position, targetPosition) < 0.001f)
			{
				cardIsMoving = false;
				GameCard targetGameCard = targetCard.GetComponent<GameCard>();
				targetGameCard.Card = movingCard.GetComponent<GameCard>().Card;
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
		form.AddField ("myform_deck", ApplicationModel.selectedDeck);// Pseudo de l'utilisateur connecté
		
		WWW w = new WWW (URL, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) {
			print (w.error); 										// donne l'erreur eventuelle
		} else {
			print (w.text);											// donne le retour
			
			string[] cardEntries = w.text.Split ('\n');				// Chaque ligne du serveur correspond à une carte
			
			for (int i = 0; i < cardEntries.Length - 1; i++) 		// On boucle sur les attributs d'une carte
			{ 		
				string[] cardData = cardEntries [i].Split ('\\'); 	// On découpe les attributs de la carte qu'on place dans un tableau
				if (cardData.Length < 2) 
				{
					break;
				}
				//int cardId = System.Convert.ToInt32(cardData[0]); // Ici, on récupère l'id en base
				int cardArt = System.Convert.ToInt32(cardData[1]);// l'indice de l'image
				string cardTitle = cardData[2]; 					// le titre de la carte
				int cardLife = System.Convert.ToInt32(cardData[3]);// le nombre de point de vie
				
				Card card = new Card (cardTitle, cardLife, cardArt);
				GameObject instance = 
					Instantiate(CardObject) as GameObject;			// On charge une instance du prefab Card
				lastRemainingPosition = 
					new Vector3(-32 + (12 * i), 31, -5);	
				instance.transform.localPosition = 	
					lastRemainingPosition;							// ..., de positionnement ...
				instance.GetComponent<GameCard>().Card = card;		// ... et la carte qu'elle représente
				instance.GetComponent<GameCard>().ShowFace();		// On affiche la carte
				instance.transform.parent = transform;
			}
		}
	}
}
