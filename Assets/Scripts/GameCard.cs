using UnityEngine;
using System.Collections;

public class GameCard : MonoBehaviour 
{
	public Texture[] faces; 										// Les différentes images des cartes
	public Card Card; 												// l'instance de la carte courante

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
		renderer.material.mainTexture = faces[Card.ArtIndex]; 		// on affiche l'image correspondant à la carte
	}
}
