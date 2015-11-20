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
	public int onSale ;
	public int RenameCost = 5;
	public string Error;
	public List<int> Decks;
	public int deckOrder;
	public int destructionPrice;
	public int Power;
	public int PowerLevel;
	public bool GetNewSkill;
	public int UpgradedAttack;
	public int UpgradedLife;
	public int UpgradedSpeed;
	public int UpgradedAttackLevel;
	public int UpgradedLifeLevel;
	public int UpgradedSpeedLevel;
	public static bool xpDone = false;
	public bool isMine ;
	
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
	
	public int GetEsquive()
	{
		int esquive = 0;
//		foreach (StatModifier modifier in modifiers)
//		{
//			if (modifier.Type == ModifierType.Type_EsquivePercentage)
//			{
//				esquive = modifier.Amount;
//			}
//		}
//		
		return esquive;
	}
	
	public int GetMagicalEsquive()
	{
		int esquive = 0;
//		foreach (StatModifier modifier in modifiers)
//		{
//			if (modifier.Type == ModifierType.Type_MagicalEsquivePercentage)
//			{
//				esquive = modifier.Amount;
//			}
//		}
		
		return esquive;
	}
	
	public int GetDamagesPercentageBonus(Card c)
	{
		int damagePercentageBonus = 0;
//		foreach (StatModifier modifier in modifiers)
//		{
//			if (modifier.Type == ModifierType.Type_DommagePercentage)
//			{
//				damagePercentageBonus += modifier.Amount;
//			}
//		}
		
		damagePercentageBonus += this.GetDamagesPercentageBonusAgainst(c.ArtIndex);
		
		return damagePercentageBonus;
	}
	
	public int GetDamagesPercentageBonusAgainst(int type)
	{
		int damagePercentageBonus = 0;
//		foreach (StatModifier modifier in modifiers)
//		{
//			if ((int)modifier.Type-6 == type)
//			{
//				damagePercentageBonus += modifier.Amount;
//			}
//		}
		return damagePercentageBonus;
	}
	
	public bool isIntouchable()
	{
		bool isIntouchable = false;
		int i = 0 ;
//		int max = modifiers.Count ;
//		while (i<max && !isIntouchable)
//		{
//			if (modifiers[i].Type == ModifierType.Type_Intouchable)
//			{
//				isIntouchable = true ;
//			}
//			i++;
//		}
		return isIntouchable;
	}
	
	public bool isSleeping()
	{
		bool isSleeping = false;
		int i = 0 ;
//		int max = modifiers.Count ;
//		while (i<max && !isSleeping)
//		{
//			if (modifiers[i].Type == ModifierType.Type_Sleeping)
//			{
//				isSleeping = true ;
//			}
//			i++;
//		}
		return isSleeping;
	}
	
	public List<Skill> getSkills()
	{
		return this.Skills;
	}
	
	public void emptyModifiers()
	{
//		for (int i = modifiers.Count-1 ; i >= 0 ; i--)
//		{
//			if (modifiers[i].Stat != ModifierStat.Stat_Dommage)
//			{
//				modifiers.RemoveAt(i);
//			}
//		}
//		
//		if(this.GetLife()<0){
//			this.addModifier(this.GetTotalLife()-1,ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
//		}
	}
	
	public void removeLeaderEffect(){
//		for (int i = modifiers.Count-1 ; i >= 0 ; i--)
//		{
//			if(modifiers[i].idIcon==-4 ||modifiers[i].idIcon==4){
//				modifiers.RemoveAt(i);
//			}
//		}
	}
	
	public bool hasModifiers()
	{
		bool hasModifiers = false;
//		for (int i = modifiers.Count-1 ; i >= 0 && !hasModifiers ; i--)
//		{
//			if (modifiers[i].Stat != ModifierStat.Stat_Dommage)
//			{
//				hasModifiers = true ;
//			}
//		}
		return hasModifiers ;
	}
	
	public virtual int getAttack (){
		return this.Attack;
	}
	
	public virtual int getLife (){
		return this.Life;
	}
	
	public virtual int getMove (){
		return this.Move;
	}
	
	public string GetAttackString()
	{
		int attack = this.getAttack();
		if (attack<10){
			return ("0"+attack);
		}
		else{
			return attack.ToString();
		}
	}
	
	public int getPassiveSkillLevel(){
		return this.Skills[0].Level;
	}
	
	public string GetMoveString()
	{
		int move = this.getMove();
		if (move<10){
			return ("0"+move);
		}
		else{
			return move.ToString();
		}
	}
	
	public string GetLifeString()
	{
		int life = this.getLife();
		if (life<10){
			return ("0"+life);
		}
		else{
			return life.ToString();
		}
	}
	
	public int GetBouclier()
	{
		int bouclier = 0;
//		foreach (StatModifier modifier in modifiers)
//		{
//			if (modifier.Type==ModifierType.Type_Bouclier){
//				bouclier += modifier.Amount;
//			}
//		}
//		if (bouclier > 100)
//		{
//			return 100;
//		}
		return bouclier;
	}
	
	public int GetTotalLife()
	{
		int life = this.Life;
		
//		foreach (StatModifier modifier in modifiers)
//		{
//			if (modifier.Stat == ModifierStat.Stat_Life)
//			{
//				life += modifier.Amount;
//			}
//		}
		return life ;
	}
	
	public List<string> getIconMove()
	{
		List<string> iconMoveTexts = new List<string>();
		int i = 0;
//		while (i < this.modifiers.Count)
//		{
//			if (this.modifiers [i].idIcon == 11 || this.modifiers [i].idIcon == 12 || this.modifiers [i].idIcon == 13 || this.modifiers [i].idIcon == 14 || this.modifiers [i].idIcon == 15 || this.modifiers [i].idIcon == 16)
//			{
//				iconMoveTexts.Add(this.modifiers [i].title);
//				iconMoveTexts.Add(this.modifiers [i].description);
//			}
//			i++;
//		}
		return iconMoveTexts;
	}
	
	public List<string> getIconShield()
	{
		List<string> iconMoveTexts = new List<string>();
		int i = 0;
//		while (i < this.modifiers.Count)
//		{
//			if (this.modifiers [i].idIcon == 1 || this.modifiers [i].idIcon == 2)
//			{
//				iconMoveTexts.Add(this.modifiers [i].title);
//				iconMoveTexts.Add(this.modifiers [i].description);
//			}
//			i++;
//		}
		return iconMoveTexts;
	}
	
	public List<string> getIconEffect()
	{
		List<string> iconMoveTexts = new List<string>();
		int i = 0;
//		while (i < this.modifiers.Count)
//		{
//			if (this.modifiers [i].idIcon == 51 || this.modifiers [i].idIcon == 52 || this.modifiers [i].idIcon == 53 || this.modifiers [i].idIcon == 54)
//			{
//				iconMoveTexts.Add(this.modifiers [i].title);
//				iconMoveTexts.Add(this.modifiers [i].description);
//				if (this.modifiers [i].idIcon == 51 || this.modifiers [i].idIcon == 52){
//					iconMoveTexts.Add("Bonus");
//				}
//				else{
//					iconMoveTexts.Add("Malus");
//				}
//			}
//			i++;
//		}
		return iconMoveTexts;
	}
	
	public int getIdIconEffect()
	{
		int idIcon = -1 ;
		int i = 0;
//		while (i < this.modifiers.Count)
//		{
//			if (this.modifiers [i].idIcon == 51 || this.modifiers [i].idIcon == 52 || this.modifiers [i].idIcon == 53 || this.modifiers [i].idIcon == 54)
//			{
//				idIcon = this.modifiers [i].idIcon;
//			}
//			i++;
//		}
		return idIcon;
	}
	
	public bool isFurious()
	{
		bool isCrazy = false;
		int i = 0;
//		while (i < this.modifiers.Count && !isCrazy)
//		{
//			if (this.modifiers [i].Type == ModifierType.Type_Crazy)
//			{
//				isCrazy = true;
//			}
//			i++;
//		}
		return isCrazy;
	}

	public void clearBuffs()
	{
//		List<StatModifier> temp = new List<StatModifier>();
//		for (int i = 0; i < modifiers.Count; i++)
//		{
//			if (modifiers [i].Stat == ModifierStat.Stat_Dommage)
//			{
//				temp.Add(modifiers [i]);
//			}
//		}
//		modifiers = temp;
	}
	
	public bool isGenerous()
	{
		bool b = false ;
		if (this.Skills[0].Id == 74){
			if(UnityEngine.Random.Range(1,101)<this.Skills[0].proba){
				b = true ;
			}
		}
		return b;
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
			index = s.IndexOf("%ATK");
			
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.CeilToInt(Int32.Parse(tempstring)*this.getAttack()/100f);
			s = s.Substring(0,index-4)+" "+percentage+" "+s.Substring(0,index+3);
		}
		if (s.Contains("%PV")){
			index = s.IndexOf("%PV");
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.CeilToInt(Int32.Parse(tempstring)*this.getLife()/100f);
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
				this.Skills[j-1].SkillType=new SkillType();
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