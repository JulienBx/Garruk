using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
	public string UsernameOwner;
	public int AttackLevel;
	public int MoveLevel;
	public int LifeLevel;
	public int SpeedLevel;
	public int Price;
	public int Experience;
	public int nbWin;
	public int nbLoose;
	public int ExperienceLevel;
	public int NextLevelPrice;
	public int PercentageToNextLevel;
	public DateTime OnSaleDate;
	public List<StatModifier> modifiers = new List<StatModifier>();
	public List<StatModifier> TileModifiers = new List<StatModifier>();
	public int onSale ;
	public int RenameCost = 5;
	public string Error;
	public List<int> Decks;
	public int deckOrder;
	public int destructionPrice;
	public int Power;
	public int PowerLevel;
	public bool GetNewSkill;
	public int nbTurnsToWait ;
	public bool isMine;
	public int UpgradedAttack;
	public int UpgradedLife;
	public int UpgradedSpeed;
	public int UpgradedAttackLevel;
	public int UpgradedLifeLevel;
	public int UpgradedSpeedLevel;
	public static bool xpDone = false;
	
	public Card()
	{
		this.Skills = new List<Skill>();
	}
	
	public Card(string title)
	{
		this.Title = title;
	}
	
	public Card(int id)
	{
		this.Id = id;
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
	            DateTime onSaleDate,
	            int experience,
	            int nbWin,
	            int nbLoose)
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
		this.Experience = experience;
		this.nbWin = nbWin;
		this.nbLoose = nbLoose;
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
	            int onSale,
	            int experience,
	            int nbWin,
	            int nbLoose)
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
		this.onSale = onSale;
		this.Experience = experience;
		this.nbWin = nbWin;
		this.nbLoose = nbLoose;
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
	public void addModifier(int amount, ModifierType type, ModifierStat stat, int duration, int idIcon, string t, string d, string a){
		bool found ;
		if (stat == ModifierStat.Stat_Dommage){
			int i ;
			for (i = 0 ; i < this.modifiers.Count ; i++){
				if (modifiers[i].Stat==ModifierStat.Stat_Dommage){
					modifiers[i].Amount += amount ; 
					if(modifiers[i].Amount<0){
						modifiers[i].Amount=0;
					}
					i = this.modifiers.Count+1; 
				}
			}
			if (i==this.modifiers.Count){
				this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, d, a));
				if(modifiers[this.modifiers.Count-1].Amount<0){
					modifiers[this.modifiers.Count-1].Amount=0;
				}
			}
		}
		else if (idIcon == 1){
			for (int j = this.modifiers.Count-1 ; j >= 0 ; j--){
				if (modifiers[j].idIcon==1){
					modifiers.RemoveAt(j) ; 
				}
			}
			this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, d, a));
		}
		else if (idIcon == 2){
			for (int j = this.modifiers.Count-1 ; j >= 0 ; j--){
				if (modifiers[j].idIcon==2){
					modifiers.RemoveAt(j) ; 
				}
			}
			this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, d, a));
		}
		else if (idIcon == 13){
			for (int j = this.modifiers.Count-1 ; j >= 0 ; j--){
				if (modifiers[j].idIcon==13){
					amount += modifiers[j].Amount;
					modifiers.RemoveAt(j) ; 
				}
			}
			this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, "+"+amount+" MOV. Permanent", a));
		}
		else if (idIcon == 14){
			for (int j = this.modifiers.Count-1 ; j >= 0 ; j--){
				if (modifiers[j].idIcon==14){
					amount += modifiers[j].Amount;
					modifiers.RemoveAt(j) ; 
				}
			}
			this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, "-"+amount+" MOV. Permanent", a));
		}
		else if (idIcon == 15){
			for (int j = this.modifiers.Count-1 ; j >= 0 ; j--){
				if (modifiers[j].idIcon==15){
					amount += modifiers[j].Amount;
					modifiers.RemoveAt(j) ; 
				}
			}
			this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, "+"+amount+" ATK. Permanent", a));
		}
		else if (idIcon == 16){
			for (int j = this.modifiers.Count-1 ; j >= 0 ; j--){
				if (modifiers[j].idIcon==16){
					amount += modifiers[j].Amount;
					modifiers.RemoveAt(j) ; 
				}
			}
			this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, "+"+amount+" ATK. Permanent", a));
		}
		else if (idIcon == 17){
			for (int j = this.modifiers.Count-1 ; j >= 0 ; j--){
				if (modifiers[j].idIcon==17){
					amount += modifiers[j].Amount;
					modifiers.RemoveAt(j) ; 
				}
			}
			this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, "+"+amount+" PV. Actif tant que le leader est en vie", a));
		}
		
		else if (idIcon == 20){
			for (int j = this.modifiers.Count-1 ; j >= 0 ; j--){
				if (modifiers[j].idIcon==20){
					amount += modifiers[j].Amount;
					modifiers.RemoveAt(j) ; 
				}
			}
			this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, "+"+amount+" ATK. Permanent", a));
		}
		
		else if (idIcon == 22){
			for (int j = this.modifiers.Count-1 ; j >= 0 ; j--){
				if (modifiers[j].idIcon==22){
					amount += modifiers[j].Amount;
					modifiers.RemoveAt(j) ; 
				}
			}
			this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, "-"+amount+" ATK. Permanent", a));
		}
		
		else if (idIcon == 23){
			for (int j = this.modifiers.Count-1 ; j >= 0 ; j--){
				if (modifiers[j].idIcon==23){
					amount += modifiers[j].Amount;
					modifiers.RemoveAt(j) ; 
				}
			}
			this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, "-"+amount+" ATK. Permanent", a));
		}
		else if (idIcon == 24){
			for (int j = this.modifiers.Count-1 ; j >= 0 ; j--){
				if (modifiers[j].idIcon==24){
					amount += modifiers[j].Amount;
					modifiers.RemoveAt(j) ; 
				}
			}
			this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, "-"+amount+" ATK. Permanent", a));
		}
		else if (idIcon == 25){
			for (int j = this.modifiers.Count-1 ; j >= 0 ; j--){
				if (modifiers[j].idIcon==25){
					amount += modifiers[j].Amount;
					modifiers.RemoveAt(j) ; 
				}
			}
			this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, "+"+amount+" ATK. Permanent", a));
		}
		else if (idIcon == 26){
			for (int j = this.modifiers.Count-1 ; j >= 0 ; j--){
				if (modifiers[j].idIcon==26){
					amount += modifiers[j].Amount;
					modifiers.RemoveAt(j) ; 
				}
			}
			this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, "-"+amount+" ATK. Permanent", a));
		}
		else if (idIcon == 27){
			for (int j = this.modifiers.Count-1 ; j >= 0 ; j--){
				if (modifiers[j].idIcon==27){
					amount += modifiers[j].Amount;
					modifiers.RemoveAt(j) ; 
				}
			}
			this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, "+"+amount+" ATK. Permanent", a));
		}
		else if (idIcon == 28){
			for (int j = this.modifiers.Count-1 ; j >= 0 ; j--){
				if (modifiers[j].idIcon==28){
					modifiers.RemoveAt(j) ; 
				}
			}
			this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, "+"+amount+" ATK. Actif tant que le leader est en vie", a));
		}
		else if (idIcon == 30){
			for (int j = this.modifiers.Count-1 ; j >= 0 ; j--){
				if (modifiers[j].idIcon==30){
					amount += modifiers[j].Amount;
					modifiers.RemoveAt(j) ; 
				}
			}
			this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, "+"+amount+" PV. Permanent", a));
		}
		else if (idIcon == 31){
			for (int j = this.modifiers.Count-1 ; j >= 0 ; j--){
				if (modifiers[j].idIcon==31){
					amount += modifiers[j].Amount;
					modifiers.RemoveAt(j) ; 
				}
			}
			this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, "+"+amount+" PV. Permanent", a));
		}
		else if (idIcon == 51 || idIcon == 52 || idIcon == 53 || idIcon == 54){
			for (int j = this.modifiers.Count-1 ; j >= 0 ; j--){
				if (modifiers[j].idIcon==51 || modifiers[j].idIcon==52 || modifiers[j].idIcon==53 || modifiers[j].idIcon==54){
					modifiers.RemoveAt(j) ; 
				}
			}
			this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, "+"+amount+" PV. Permanent", a));
		}
		else if(stat == ModifierStat.Stat_Speed || stat == ModifierStat.Stat_Move){
			if(type==ModifierType.Type_BonusMalus && duration == -1 && idIcon!=4){
				for (int j = this.modifiers.Count-1 ; j >= 0 ; j--){
					if (modifiers[j].Stat==stat && type==ModifierType.Type_BonusMalus && duration == -1){
						modifiers.RemoveAt(j) ; 
					}
				}
			}
			this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, d, a));
		}
		else if(type==ModifierType.Type_Paralized || type==ModifierType.Type_Bouclier || type==ModifierType.Type_EsquivePercentage || type==ModifierType.Type_MagicalEsquivePercentage){
			for (int j = this.modifiers.Count-1 ; j >= 0 ; j--){
				if (modifiers[j].Type==type){
					modifiers.RemoveAt(j);
				}
			}
			this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, d, a));
		}
		else{
			this.modifiers.Add(new StatModifier(amount, type, stat, duration, idIcon, t, d, a));
		}
	}
	
	public int GetEsquive()
	{
		int esquive = 0;
		foreach (StatModifier modifier in modifiers)
		{
			if (modifier.Type == ModifierType.Type_EsquivePercentage)
			{
				esquive = modifier.Amount;
			}
		}
		
		return esquive;
	}
	
	public int GetMagicalEsquive()
	{
		int esquive = 0;
		foreach (StatModifier modifier in modifiers)
		{
			if (modifier.Type == ModifierType.Type_MagicalEsquivePercentage)
			{
				esquive = modifier.Amount;
			}
		}
		
		return esquive;
	}
	
	public int GetDamagesPercentageBonus(Card c)
	{
		int damagePercentageBonus = 0;
		foreach (StatModifier modifier in modifiers)
		{
			if (modifier.Type == ModifierType.Type_DommagePercentage)
			{
				damagePercentageBonus += modifier.Amount;
			}
		}
		
		damagePercentageBonus += this.GetDamagesPercentageBonusAgainst(c.ArtIndex);
		
		return damagePercentageBonus;
	}
	
	public int GetDamagesPercentageBonusAgainst(int type)
	{
		int damagePercentageBonus = 0;
		foreach (StatModifier modifier in modifiers)
		{
			if ((int)modifier.Type-6 == type)
			{
				damagePercentageBonus += modifier.Amount;
			}
		}
		return damagePercentageBonus;
	}
	
	public bool isIntouchable()
	{
		bool isIntouchable = false;
		int i = 0 ;
		int max = modifiers.Count ;
		while (i<max && !isIntouchable)
		{
			if (modifiers[i].Type == ModifierType.Type_Intouchable)
			{
				isIntouchable = true ;
			}
			i++;
		}
		return isIntouchable;
	}
	
	public bool isSleeping()
	{
		bool isSleeping = false;
		int i = 0 ;
		int max = modifiers.Count ;
		while (i<max && !isSleeping)
		{
			if (modifiers[i].Type == ModifierType.Type_Sleeping)
			{
				isSleeping = true ;
			}
			i++;
		}
		return isSleeping;
	}
	
	public int getSleepingPercentage()
	{
		bool isSleeping = false;
		int percentage = 0 ;
		int i = 0 ;
		int max = modifiers.Count ;
		while (i<max && !isSleeping)
		{
			if (modifiers[i].Type == ModifierType.Type_Sleeping)
			{
				isSleeping = true ;
				percentage = modifiers[i].Amount ;
			}
			i++;
		}
		return percentage;
	}
	
	public List<Skill> getSkills()
	{
		return this.Skills;
	}
	
	public void removeSleeping()
	{
		bool isSleeping = false;
		int i = 0 ;
		int max = modifiers.Count ;
		while (i<max && !isSleeping)
		{
			if (modifiers[i].Type == ModifierType.Type_Sleeping)
			{
				isSleeping = true ;
				this.modifiers.RemoveAt(i);
			}
			i++;
		}
	}
	
	public void emptyModifiers()
	{
		for (int i = modifiers.Count-1 ; i >= 0 ; i--)
		{
			if (modifiers[i].Stat != ModifierStat.Stat_Dommage)
			{
				modifiers.RemoveAt(i);
			}
		}
		
		if(this.GetLife()<0){
			this.addModifier(this.GetTotalLife()-1,ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		}
	}
	
	public void removeLeaderEffect(){
		for (int i = modifiers.Count-1 ; i >= 0 ; i--)
		{
			if(modifiers[i].idIcon==-4 ||modifiers[i].idIcon==4){
				modifiers.RemoveAt(i);
			}
		}
	}
	
	public bool hasModifiers()
	{
		bool hasModifiers = false;
		for (int i = modifiers.Count-1 ; i >= 0 && !hasModifiers ; i--)
		{
			if (modifiers[i].Stat != ModifierStat.Stat_Dommage)
			{
				hasModifiers = true ;
			}
		}
		return hasModifiers ;
	}
	
	public int GetAttack()
	{
		int attack = Attack;
		foreach (StatModifier modifier in modifiers)
		{
			if (modifier.Stat==ModifierStat.Stat_Attack){
				attack += modifier.Amount;
			}
		}
		foreach (StatModifier modifier in TileModifiers)
		{
			attack = modifier.modifyAttack(attack);
		}
		if (attack < 0)
		{
			return 0;
		}
		return attack;
	}
	
	public string GetAttackString()
	{
		int attack = this.GetAttack();
		if (attack<10){
			return ("0"+attack);
		}
		else{
			return attack.ToString();
		}
	}
	
	public Skill GetAttackSkill()
	{
		return new Skill("Attaque", "Inflige "+this.GetAttack()+" dégats au contact",0);
	}
	
	public string GetMoveString()
	{
		int move = this.GetMove();
		if (move<10){
			return ("0"+move);
		}
		else{
			return move.ToString();
		}
	}
	
	public string GetLifeString()
	{
		int life = this.GetLife();
		if (life<10){
			return ("0"+life);
		}
		else{
			return life.ToString();
		}
	}
	
	public string GetSpeedString()
	{
		int speed = this.GetSpeed();
		if (speed<10){
			return ("0"+speed);
		}
		else{
			return speed.ToString();
		}
	}
	
	public int GetBouclier()
	{
		int bouclier = 0;
		foreach (StatModifier modifier in modifiers)
		{
			if (modifier.Type==ModifierType.Type_Bouclier){
				bouclier += modifier.Amount;
			}
		}
		if (bouclier > 100)
		{
			return 100;
		}
		return bouclier;
	}
	
	public int GetTotalLife()
	{
		int life = this.Life;
		
		foreach (StatModifier modifier in modifiers)
		{
			if (modifier.Stat == ModifierStat.Stat_Life)
			{
				life += modifier.Amount;
			}
		}
		return life ;
	}
	
	public int GetLife()
	{
		int life = this.Life;
		
		foreach (StatModifier modifier in modifiers)
		{
			if (modifier.Stat == ModifierStat.Stat_Life)
			{
				life += modifier.Amount;
			}
		}
		
		foreach (StatModifier modifier in modifiers)
		{
			if (modifier.Stat == ModifierStat.Stat_Dommage)
			{
				life -= modifier.Amount;
			}
		}
		
		return life ;
	}
	
	public int GetSpeed()
	{
		int speed = Speed;
		foreach (StatModifier modifier in modifiers)
		{
			speed = modifier.modifySpeed(speed);
		}
		foreach (StatModifier modifier in TileModifiers)
		{
			speed = modifier.modifySpeed(speed);
		}

		if (speed < 0)
		{
			return 0;
		}
		return speed;
	}
	
	public int GetMove()
	{
		int move = Move;
		foreach (StatModifier modifier in modifiers)
		{
			move = modifier.modifyMove(move);
		}
		foreach (StatModifier modifier in TileModifiers)
		{
			move = modifier.modifyMove(move);
		}
		if (move < 0)
		{
			return 0;
		}
		return move;
	}
	
	public bool isParalyzed()
	{
		bool isParalyzed = false;
		int i = 0;
		while (i < this.modifiers.Count && !isParalyzed)
		{
			if (this.modifiers [i].Type == ModifierType.Type_Paralized && this.modifiers[i].Duration>0)
			{
				isParalyzed = true;
			}
			i++;
		}
		return isParalyzed;
	}
	
	public List<string> getIconAttack()
	{
		List<string> iconAttackTexts = new List<string>();
		int i = 0;
		while (i < this.modifiers.Count)
		{
			if (this.modifiers [i].idIcon == 16 || this.modifiers [i].idIcon == 18 || this.modifiers [i].idIcon == 19 || this.modifiers [i].idIcon == 20 || this.modifiers [i].idIcon == 22 || this.modifiers [i].idIcon == 23 || this.modifiers [i].idIcon == 24 || this.modifiers [i].idIcon == 25 || this.modifiers [i].idIcon == 26 || this.modifiers [i].idIcon == 27 || this.modifiers [i].idIcon == 28)
			{
				iconAttackTexts.Add(this.modifiers [i].title);
				iconAttackTexts.Add(this.modifiers [i].description);
			}
			i++;
		}
		return iconAttackTexts;
	}
	
	public List<string> getIconLife()
	{
		List<string> iconLifeTexts = new List<string>();
		int i = 0;
		while (i < this.modifiers.Count)
		{
			if (this.modifiers [i].idIcon == 30 || this.modifiers [i].idIcon == 31 || this.modifiers [i].idIcon == 17)
			{
				iconLifeTexts.Add(this.modifiers [i].title);
				iconLifeTexts.Add(this.modifiers [i].description);
			}
			i++;
		}
		return iconLifeTexts;
	}
	
	public List<string> getIconMove()
	{
		List<string> iconMoveTexts = new List<string>();
		int i = 0;
		while (i < this.modifiers.Count)
		{
			if (this.modifiers [i].idIcon == 11 || this.modifiers [i].idIcon == 12 || this.modifiers [i].idIcon == 13 || this.modifiers [i].idIcon == 14 || this.modifiers [i].idIcon == 15 || this.modifiers [i].idIcon == 16)
			{
				iconMoveTexts.Add(this.modifiers [i].title);
				iconMoveTexts.Add(this.modifiers [i].description);
			}
			i++;
		}
		return iconMoveTexts;
	}
	
	public List<string> getIconShield()
	{
		List<string> iconMoveTexts = new List<string>();
		int i = 0;
		while (i < this.modifiers.Count)
		{
			if (this.modifiers [i].idIcon == 1 || this.modifiers [i].idIcon == 2)
			{
				iconMoveTexts.Add(this.modifiers [i].title);
				iconMoveTexts.Add(this.modifiers [i].description);
			}
			i++;
		}
		return iconMoveTexts;
	}
	
	public List<string> getIconEffect()
	{
		List<string> iconMoveTexts = new List<string>();
		int i = 0;
		while (i < this.modifiers.Count)
		{
			if (this.modifiers [i].idIcon == 51 || this.modifiers [i].idIcon == 52 || this.modifiers [i].idIcon == 53 || this.modifiers [i].idIcon == 54)
			{
				iconMoveTexts.Add(this.modifiers [i].title);
				iconMoveTexts.Add(this.modifiers [i].description);
			}
			i++;
		}
		return iconMoveTexts;
	}
	
	public int getIdIconEffect()
	{
		int idIcon = -1 ;
		int i = 0;
		while (i < this.modifiers.Count)
		{
			if (this.modifiers [i].idIcon == 51 || this.modifiers [i].idIcon == 51 || this.modifiers [i].idIcon == 51 || this.modifiers [i].idIcon == 51)
			{
				idIcon = this.modifiers [i].idIcon;
			}
			i++;
		}
		return idIcon;
	}
	
	public bool isFurious()
	{
		bool isCrazy = false;
		int i = 0;
		while (i < this.modifiers.Count && !isCrazy)
		{
			if (this.modifiers [i].Type == ModifierType.Type_Crazy)
			{
				isCrazy = true;
			}
			i++;
		}
		return isCrazy;
	}

	public void clearBuffs()
	{
		List<StatModifier> temp = new List<StatModifier>();
		for (int i = 0; i < modifiers.Count; i++)
		{
			if (modifiers [i].Stat == ModifierStat.Stat_Dommage)
			{
				temp.Add(modifiers [i]);
			}
		}
		modifiers = temp;
	}
	
	public bool isGenerous()
	{
		return (this.Skills[0].Id == 74);
	}
	
	public bool isGiant()
	{
		return (this.Skills[0].Id == 72);
	}
	
	public bool isPacifiste()
	{
		return (this.Skills[0].Id == 75);
	}
	
	public bool isNurse()
	{
		return (this.Skills[0].Id == 77);
	}
	
	public bool isLeader()
	{
		return (this.Skills[0].Id == 78);
	}
	
	public bool isAguerri()
	{
		return (this.Skills[0].Id == 70);
	}
	
	public bool isFrenetique()
	{
		return (this.Skills[0].Id == 71);
	}
	
	public bool isGeant()
	{
		return (this.Skills[0].Id == 72);
	}
	
	public bool isRapide()
	{
		return (this.Skills[0].Id == 73);
	}
	
	public bool isRobuste()
	{
		return (this.Skills[0].Id == 69);
	}
	
	public bool isLache()
	{
		return (this.Skills[0].Id == 67);
	}
	
	public bool isPiegeur()
	{
		return (this.Skills[0].Id == 66);
	}
	
	public bool isAgile()
	{
		return (this.Skills[0].Id == 68);
	}
	
	public int getPassiveManacost()
	{
		return (this.Skills[0].ManaCost);
	}

//	public void changeModifiers()
//	{
//		List<StatModifier> temp = new List<StatModifier>();
//		foreach (StatModifier modifier in modifiers)
//		{
//			if (!modifier.Active)
//			{
//				modifier.Active = true;
//				modifier.Duration--;
//				temp.Add(modifier);
//			} else if (modifier.Duration != 0)
//			{
//				temp.Add(modifier);
//				modifier.Duration--;
//			}
//		}
//		modifiers = temp;
//
//		temp = new List<StatModifier>();
//		foreach (StatModifier modifier in TileModifiers)
//		{
//			if (!modifier.Active)
//			{
//				modifier.Active = true;
//				modifier.Duration--;
//				temp.Add(modifier);
//			} else if (modifier.Duration != 0)
//			{
//				temp.Add(modifier);
//				modifier.Duration--;
//			}
//		}
//		TileModifiers = temp;
//	}

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
	
	public bool hasSkill(string s)
	{
		bool b = false;
		int compteur = this.Skills.Count;
		int i = 0;
		while (!b && i<compteur)
		{
			if (Skills [i].IsActivated == 1)
			{
				if (Skills [i].Name.ToLower().Contains(s))
				{
					b = true;
				}
			}
			i++;
		}
		return b;
	}
	
	public string getSkillText(string s){
		int index ;
		int percentage ;
		string tempstring ;
		if (s.Contains("%ATK")){
			Debug.Log("COUNTATK : "+s.Length);
			index = s.IndexOf("%ATK");
			Debug.Log("INDEXATK : "+index);
			
			tempstring = s.Substring(index-3,3);
			Debug.Log(tempstring);
			percentage = Mathf.CeilToInt(Int32.Parse(tempstring)*this.GetAttack()/100f);
			s = s.Substring(0,index-4)+" "+percentage+" "+s.Substring(0,index+3);
		}
		if (s.Contains("%PV")){
			Debug.Log("COUNTPV : "+s.Length);
			index = s.IndexOf("%PV");
			Debug.Log("INDEXPV : "+index);
			tempstring = s.Substring(index-3,3);
			Debug.Log(tempstring);
			percentage = Mathf.CeilToInt(Int32.Parse(tempstring)*this.GetLife()/100f);
			s = s.Substring(0,index-4)+" "+percentage+" "+s.Substring(0,index+3);
		}
		return s;
	}
	public bool verifyC(float minPower, float maxPower, float minLife, float maxLife, float minAttack, float maxAttack, float minQuickness, float maxQuickness)
	{
		if (minPower > this.Power || maxPower < this.Power)
		{
			return false;
		}
		else if (minLife > this.Life || maxLife < this.Life)
		{
			return false;
		} 
		else if (minAttack > this.Attack || maxAttack < this.Attack)
		{
			return false;
		} 
		else if (minQuickness > this.Speed || maxQuickness < this.Speed)
		{
			return false;
		} 
		else
		{
			return true;
		}
	}

	public bool verifyC2(float minPrice, float maxPrice, float minPower, float maxPower, float minLife, float maxLife, float minAttack, float maxAttack, float minQuickness, float maxQuickness)
	{
		if (minPrice > this.Price || maxPrice < this.Price)
		{
			return false;
		}
		else if (minPower > this.Power || maxPower < this.Power)
		{
			return false;
		}
		else if (minLife > this.Life || maxLife < this.Life)
		{
			return false;
		} 
		else if (minAttack > this.Attack || maxAttack < this.Attack)
		{
			return false;
		} 
		else if (minQuickness > this.Speed || maxQuickness < this.Speed)
		{
			return false;
		} 
		else
		{
			return true;
		}
	}

	public void parseCard(string s)
	{
		string[] cardData = null;
		string[] cardInfo = null;

		cardData = s.Split(new string[] { "#SKILL#" }, System.StringSplitOptions.None);
		for(int j = 0 ; j < cardData.Length ; j++)
		{	
			cardInfo = cardData[j].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
			if (j==0)
			{
				this.Id=System.Convert.ToInt32(cardInfo[0]);
				this.Title=cardInfo[1];
				this.Life=System.Convert.ToInt32(cardInfo[2]);
				this.Attack=System.Convert.ToInt32(cardInfo[3]);
				this.Speed=System.Convert.ToInt32(cardInfo[4]);
				this.Move=System.Convert.ToInt32(cardInfo[5]);
				this.IdOWner=System.Convert.ToInt32(cardInfo[6]);
				this.UsernameOwner=cardInfo[7];
				this.IdClass=System.Convert.ToInt32(cardInfo[8]);
				this.PowerLevel=System.Convert.ToInt32(cardInfo[9]);
				this.LifeLevel=System.Convert.ToInt32(cardInfo[10]);
				this.AttackLevel=System.Convert.ToInt32(cardInfo[11]);
				this.MoveLevel=System.Convert.ToInt32(cardInfo[12]);
				this.SpeedLevel=System.Convert.ToInt32(cardInfo[13]);
				this.Experience=System.Convert.ToInt32(cardInfo[14]);
				this.ExperienceLevel=System.Convert.ToInt32(cardInfo[15]);
				this.PercentageToNextLevel=System.Convert.ToInt32(cardInfo[16]);
				this.NextLevelPrice=System.Convert.ToInt32(cardInfo[17]);
				this.onSale=System.Convert.ToInt32(cardInfo[18]);
				this.Price=System.Convert.ToInt32(cardInfo[19]);
				this.OnSaleDate=DateTime.ParseExact(cardInfo[20], "yyyy-MM-dd HH:mm:ss", null);
				this.nbWin=System.Convert.ToInt32(cardInfo[21]);
				this.nbLoose=System.Convert.ToInt32(cardInfo[22]);
				this.destructionPrice=System.Convert.ToInt32(cardInfo[23]);
				this.Power=System.Convert.ToInt32(cardInfo[24]);
				this.UpgradedLife=System.Convert.ToInt32(cardInfo[25]);
				this.UpgradedAttack=System.Convert.ToInt32(cardInfo[26]);
				this.UpgradedSpeed=System.Convert.ToInt32(cardInfo[27]);
				this.UpgradedLifeLevel=System.Convert.ToInt32(cardInfo[28]);
				this.UpgradedAttackLevel=System.Convert.ToInt32(cardInfo[29]);
				this.UpgradedSpeedLevel=System.Convert.ToInt32(cardInfo[30]);
				this.Skills=new List<Skill>();
			}
			else
			{
				this.Skills.Add(new Skill ());
				this.Skills[j-1].Name=cardInfo[1];
				this.Skills[j-1].Id=System.Convert.ToInt32(cardInfo[0]);
				this.Skills[j-1].cible=System.Convert.ToInt32(cardInfo[2]);
				this.Skills[j-1].IsActivated=System.Convert.ToInt32(cardInfo[3]);
				this.Skills[j-1].Level=System.Convert.ToInt32(cardInfo[4]);
				this.Skills[j-1].Power=System.Convert.ToInt32(cardInfo[5]);
				this.Skills[j-1].Upgrades=System.Convert.ToInt32(cardInfo[6]);
				this.Skills[j-1].Description=cardInfo[7];
				this.Skills[j-1].proba=System.Convert.ToInt32(cardInfo[8]);
				this.Skills[j-1].nextDescription=cardInfo[9];
				this.Skills[j-1].nextProba=System.Convert.ToInt32(cardInfo[10]);
				this.Skills[j-1].nextLevel=System.Convert.ToInt32(cardInfo[11]);
				
				if (this.Skills[j-1].Id==9){
					this.Skills[j-1].nbLeft = 1 ;
				}
			}
		}

	}
}