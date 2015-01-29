using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card
{
	public int Id; 												// Id unique de la carte
	public string Art; 												// Nom du dessin à appliquer à la carte
	public string Title; 											// Titre unique de la carte
	public int ArtIndex;											// Index de l'image de la carte
	public int Life; 
	public string[] Skills;
	public int idClasse ;

	// Point de vie de la carte

	public Card(int id, string title, int life, int artIndex, int idclasse) 
	{
		this.Id = id;
		this.Title = title;
		this.Life = life;
		this.ArtIndex = artIndex;
		this.idClasse = idclasse;

	}

	public Card(string title, int life, int artIndex, int idclasse) 
	{
		this.Title = title;
		this.Life = life;
		this.ArtIndex = artIndex;
		this.idClasse = idclasse;
	}

	public void addSkills(string[] skills) 
	{
		this.Skills = skills;
	}
	

}