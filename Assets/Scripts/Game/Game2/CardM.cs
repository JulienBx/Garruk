﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class CardM
{
	bool mine;
	int attack;
	int life;
	int move;
	int faction;
	int deckOrder;
	List<Skill> skills;

	public CardM()
	{
		
	}

	public CardM(Card card)
	{
		this.attack = card.Attack;
		this.life = card.Life;
		this.move = card.Move;
		this.faction = card.CardType.Id;
		this.deckOrder = card.deckOrder;
		this.skills = card.Skills;
	}

	public bool isMine(){
		return this.mine;
	}

	public void setMine(bool b){
		this.mine = b;
	}

	public int getFaction(){
		return this.faction;
	}

	public int getDeckOrder(){
		return this.deckOrder;
	}

	public int getAttack(){
		return this.attack;
	}

	public int getLife(){
		return this.attack;
	}

	public Skill getSkill(int i){
		return this.skills[i];
	}

	public int getNbSkill(){
		return this.skills.Count;
	}

	public int getCharacterType(){
		return this.skills[0].Id;
	}
}