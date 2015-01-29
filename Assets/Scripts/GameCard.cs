using UnityEngine;
using System.Collections;

public class GameCard : MonoBehaviour 
{
	public Texture[] faces; 										// Les différentes images des cartes
	public Card Card; 												// L'instance de la carte courante 

	Vector3 currentTilePos;
	public Tile currentTile;

	private string URLCard = ApplicationModel.host + "get_card.php";
	//private string URLCard = "http://localhost/GarrukServer/get_card.php";

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

	void OnMouseDown() 
	{
		if (networkView.isMine)
		{
			Debug.Log("card : " + Card.Id);
			GameBoard.instance.CardSelected = this;
			GameBoard.instance.isDragging = true;
			GameTile.instance.SetCursorToDrag();
		}
	}

	void OnMouseEnter()
	{
		if (networkView.isMine)
		{
		GameBoard.instance.CardHovered = this;
			if (GameBoard.instance.isDragging)
			{
				if (!this.Equals(GameBoard.instance.CardSelected))
				{
					GameTile.instance.SetCursorToExchange();
				}
				else
				{
					GameTile.instance.SetCursorToDrag();
				}
			}
		}
	}
	void OnMouseExit()
	{
		if (networkView.isMine)
		{
			if (GameBoard.instance.isDragging)
			{
				GameBoard.instance.CardHovered = null;
			}
		}
	}
	void OnMouseUp()
	{
		if (networkView.isMine)
		{
			if (GameBoard.instance.isDragging)
			{
				if (!this.Equals(GameBoard.instance.CardHovered) && GameBoard.instance.CardHovered)
				{
					Vector3 temp = this.transform.position;
					this.transform.position = GameBoard.instance.CardHovered.transform.position;
					GameBoard.instance.CardHovered.transform.position = temp;
				}
				if (GameBoard.instance.CardHovered == null)
				{
					GameBoard.instance.droppedCard = true;
				}
				GameBoard.instance.isDragging = false;
				GameTile.instance.SetCursorToDefault();
			}
		}
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

	Vector3 calcTilePos(Tile tile)
	{
		Vector2 tileGridPos = new Vector2(tile.X + tile.Y / 2, tile.Y);
		Vector3 tilePos = GameBoardGenerator.instance.calcWorldCoord(tileGridPos);
		tilePos.y = transform.position.y;
		return tilePos;
	}


	public IEnumerator RetrieveCard (int idCard)
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_idcard", idCard);						// ID de la carte
		
		WWW w = new WWW(URLCard, form); 								// On envoie le formulaire à l'url sur le serveur 
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
				if (cardData.Length < 2) 
				{
					break;
				}
				int cardId = System.Convert.ToInt32(cardData[0]); 	// Ici, on récupère l'id en base
				int cardArt = System.Convert.ToInt32(cardData[1]); 	// l'indice de l'image
				string cardTitle = cardData[2]; 					// le titre de la carte
				int cardLife = System.Convert.ToInt32(cardData[3]);	// le nombre de point de vie
				
				Card card = new Card(cardId, cardTitle, cardLife, cardArt);
				this.Card = card;
			}
		}
	}
}
