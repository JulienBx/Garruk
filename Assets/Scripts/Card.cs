using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card
{
	public int Id; 												// Id unique de la carte
	public string Art; 									    	// Nom du dessin à appliquer à la carte
	public string Title; 										// Titre unique de la carte
	public int ArtIndex;										// Index de l'image de la carte
	public int Life; 											// Point de vie de la carte
	public int Speed;                                           // Vitesse de la carte
	public int Move;                                            // Points de mouvement de la carte    
	public int Attack;                                          // les points d'attaque de la carte
	public int Energy;                                          // les points d'energie de la carte
	public List<Skill> Skills;

	public Card(int id, string title, int life, int artIndex, int speed, int move)
	{
		this.Id = id;
		this.Title = title;
		this.Life = life;
		this.ArtIndex = artIndex;
		this.Speed = speed;
		this.Move = move;
	}

	public Card(string title, int life, int artIndex, int speed, int move) 
	{
		this.Title = title;
		this.Life = life;
		this.ArtIndex = artIndex;
		this.Speed = speed;
		this.Move = move;
	}

	public Card(int id, string title, int life, int artIndex, int speed, int move, int attack, int energy)
	{
		this.Id = id;
		this.Title = title;
		this.Life = life;
		this.ArtIndex = artIndex;
		this.Speed = speed;
		this.Move = move;
		this.Attack = attack;
		this.Energy = energy;
	}

	public Card(int id, string title, int life, int artIndex, int speed, int move, int attack, int energy, List<Skill> skills)

	{
		this.Id = id;
		this.Title = title;
		this.Life = life;
		this.ArtIndex = artIndex;
		this.Speed = speed;
		this.Move = move;
		this.Attack = attack;
		this.Energy = energy;
		this.Skills = skills;
	}

	public override int GetHashCode() 
	{
		return Id.GetHashCode();
	}
	
	public override bool Equals(object obj) 
	{
		return Equals(obj as Card);
	}
	
	public bool Equals(Card obj) 
	{
		return obj != null && obj.Id == this.Id;
	}

}