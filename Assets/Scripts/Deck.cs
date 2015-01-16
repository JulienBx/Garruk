using UnityEngine;
using System.Collections;

public class Deck {

	public int Id; 												// Id unique de la carte
	public string Name; 											// Nom du deck
	public int NbCards; 												// Nombre de cartes présentes dans le deck
	
	public Deck(int id, string name, int nbCards) 
	{
		this.Id = id;
		this.Name = name;
		this.NbCards = nbCards;
	}

	public Deck(string name, int nbCards) 
	{
		this.Name = name;
		this.NbCards = nbCards;
	}
}
