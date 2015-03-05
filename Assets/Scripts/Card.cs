using UnityEngine;
using System;
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
	public int Price;
	public DateTime OnSaleDate;
	public List<StatModifier> modifiers = new List<StatModifier>();
	
	public Card() {
	}

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
	
	public Card(int id, string title, int life, int artIndex, int speed, int move, int attack)
	{
		this.Id = id;
		this.Title = title;
		this.Life = life;
		this.ArtIndex = artIndex;
		this.Speed = speed;
		this.Move = move;
		this.Attack = attack;
	}
	
	public Card(int id, string title, int life, int artIndex, int speed, int move, int attack, List<Skill> skills)
	{
		this.Id = id;
		this.Title = title;
		this.Life = life;
		this.ArtIndex = artIndex;
		this.Speed = speed;
		this.Move = move;
		this.Attack = attack;
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
	            int attackLevel,
	            int price,
	            DateTime onSaleDate)
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
		this.Price = price;
		this.OnSaleDate = onSaleDate;
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
	
	#region Modifiers
	public int GetAttack()
	{
		int attack = Attack;
		foreach (StatModifier modifier in modifiers) {
			attack = modifier.modifyAttack(attack);
		}
		return attack;
	}
	
	public int GetLife()
	{
		int life = Life;
		foreach (StatModifier modifier in modifiers) {
			life = modifier.modifyLife(life);
		}
		return life;
	}
	
	public int GetSpeed()
	{
		int speed = Speed;
		foreach (StatModifier modifier in modifiers) {
			speed = modifier.modifySpeed(speed);
		}
		return speed;
	}
	
	public int GetMove()
	{
		int move = Move;
		foreach (StatModifier modifier in modifiers) {
			move = modifier.modifyMove(move);
		}
		return move;
	}
	
	/// <summary>
	/// Determines whether this instance has attack modifier.
	/// </summary>
	/// <returns><c>positive or negative value</c> if this instance has positive of negative attack modifier; otherwise, <c>0</c>.</returns>
	public int HasAttackModifier()
	{
		return GetAttack() - Attack;
	}
	
	/// <summary>
	/// Determines whether this instance has life modifier.
	/// </summary>
	/// <returns><c>positive or negative value</c> if this instance has positive of negative life modifier; otherwise, <c>0</c>.</returns>
	public int HasLifeModifier()
	{
		return GetLife() - Life;
	}
	
	/// <summary>
	/// Determines whether this instance has speed modifier.
	/// </summary>
	/// <returns><c>positive or negative value</c> if this instance has positive of negative speed modifier; otherwise, <c>0</c>.</returns>
	public int HasSpeedModifier()
	{
		return GetSpeed() - Speed;
	}
	
	/// <summary>
	/// Determines whether this instance has move modifier.
	/// </summary>
	/// <returns><c>positive or negative value</c> if this instance has positive of negative move modifier; otherwise, <c>0</c>.</returns>
	public int HasMoveModifier()
	{
		return GetMove() - Move;
	}
	#endregion
	
	public bool hasSkill(string s){
		bool b = false;
		int compteur = this.Skills.Count;
		int i = 0;
		while (!b && i<compteur) {
			//UnityEngine.Debug.Log("Je teste le skill "+this.Skills[i].Name+" VS "+s);
			if (Skills[i].IsActivated==1){
				if (Skills[i].Name.ToLower ().Contains(s)){
					//UnityEngine.Debug.Log("Match");
					b = true ;
				}
			}
			i++;
		}
		return b;
	}
	
	public bool verifyC(float minLife,float maxLife,float minAttack,float maxAttack,float minMove,float maxMove,float minQuickness,float maxQuickness){
		if (minLife > this.Life || maxLife < this.Life){
			return false ;
		}
		else if (minAttack > this.Attack || maxAttack < this.Attack){
			return false ;
		}
		else if (minMove > this.Move || maxMove < this.Move){
			return false ;
		}
		else if (minMove > this.Move || maxMove < this.Move){
			return false ;
		}
		else{
			return true ;
		}
	}
	
}