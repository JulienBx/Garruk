using UnityEngine;
using System.Collections;

public class Card
{
	private int Id; 												// Id unique de la carte

	public string Art; 												// Nom du dessin à appliquer à la carte
	public string Title; 											// Titre unique de la carte
	public int ArtIndex;											// Index de l'image de la carte
	public int Life; 												// Point de vie de la carte
			
	public Card(string title, int life, int artIndex) 
	{
		//this.Id = id;
		this.Title = title;
		this.Life = life;
		this.ArtIndex = artIndex;
	}
}