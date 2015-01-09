using UnityEngine;
using System.Collections;

public class Card
{
	private int id; // Id unique de la carte
	public string art; // Nom du dessin à appliquer à la carte
	public string title; // Titre unique de la carte

	public int Life; // Point de vie de la carte
	public GameObject front; // image à appliquer à la carte (fonction de son nom en base)
			
	public Card(int id, string art, string title, int life ,GameObject front) {
		this.id = id;
		this.art = art;
		this.title = title;
		this.Life = life;
		this.front = front;
	}
}