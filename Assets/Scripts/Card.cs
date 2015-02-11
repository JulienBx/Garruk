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
	public int IdClass;
	public string TitleClass;
	public int IdOWner;
	public int AttackLevel;
	public int MoveLevel;
	public int LifeLevel;
	public int SpeedLevel;

	public Card(int id, string title, int life, int artIndex, int speed, int move)
	{
		this.Id = id;
		this.Title = title;
		this.Life = life;
		this.ArtIndex = artIndex;
		this.Speed = speed;
		this.Move = move;
	}

	public Card(string title, int life, int artIndex, int idclass) 
	{
		this.Title = title;
		this.Life = life;
		this.ArtIndex = artIndex;
		this.IdClass = idclass;
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


	public Card(int id, 
	            string title, 
	            int life, 
	            int attack, 
	            int speed, 
	            int move, 
	            int artIndex, 
	            int idClass,
	            string titleClass, 
	            int lifeLevel, 
	            int moveLevel, 
	            int speedLevel, 
	            int attackLevel)
	{
		this.Id = id;
		this.Title = title;
		this.Life = life;
		this.Attack = attack;
		this.Speed = speed;
		this.Move = move;
		this.ArtIndex = artIndex;
		this.IdClass = idClass;
		this.TitleClass = titleClass;
		this.LifeLevel = lifeLevel;
		this.MoveLevel = moveLevel;
		this.SpeedLevel = speedLevel;
		this.AttackLevel = attackLevel;
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

	public bool hasSkill(string s){
		bool b = false;
		int compteur = this.Skills.Count;
		int i = 0;
		while (!b && i<compteur) {
			//UnityEngine.Debug.Log("Je teste le skill "+this.Skills[i].Name+" VS "+s);
			if (Skills[i].Name.ToLower ().Contains(s)){
				//UnityEngine.Debug.Log("Match");
				b = true ;
			}
			i++;
		}
		return b;
	}

}