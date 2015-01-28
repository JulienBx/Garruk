using UnityEngine;
using System.Collections;

public class Card
{
	public int Id; 												// Id unique de la carte
	public string Art; 												// Nom du dessin à appliquer à la carte
	public string Title; 											// Titre unique de la carte
	public int ArtIndex;											// Index de l'image de la carte
	public int Life; 												// Point de vie de la carte
	public int Speed;

	public Card(int id, string title, int life, int artIndex, int speed) 
	{
		this.Id = id;
		this.Title = title;
		this.Life = life;
		this.ArtIndex = artIndex;
		this.Speed = speed;
	}

	public Card(string title, int life, int artIndex, int speed) 
	{
		this.Title = title;
		this.Life = life;
		this.ArtIndex = artIndex;
		this.Speed = speed;
	}
}