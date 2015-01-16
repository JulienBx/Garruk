using UnityEngine;
using System.Collections;

public class GameDeck : MonoBehaviour 
{
	public Deck Deck; 												// L'instance de la carte courante
	
	public GameDeck(Deck deck) 
	{
		this.Deck = deck;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void Show() 
	{
		renderer.enabled = true;						 			// On affiche le deck
		transform.Find("Name")
			.GetComponent<TextMesh>().text = Deck.Name;				// On lui attribut son nom

		transform.Find("NbCards")
			.GetComponent<TextMesh>().text = 						// Et son nombre de cartes
				"Nombre de cartes : " + Deck.NbCards.ToString();
	}

	void OnMouseDown() {
		ApplicationModel.selectedDeck = Deck;
		Application.LoadLevel("MyDeckPage");
	}
}
