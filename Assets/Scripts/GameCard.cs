using UnityEngine;
using System.Collections;

public class GameCard : MonoBehaviour 
{
	public Texture[] faces; 										// Les différentes images des cartes
	public Card Card; 												// L'instance de la carte courante

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
}
