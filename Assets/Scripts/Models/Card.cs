using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card
{
	private string URLAddXpLevel = ApplicationModel.host + "add_xplevel_to_card.php"; 
	private string URLSellCard = ApplicationModel.host + "sellCard.php";
	private string URLPutOnMarket = ApplicationModel.host + "putonmarket.php";
	private string URLRemoveFromMarket = ApplicationModel.host + "removeFromMarket.php";
	private string URLChangeMarketPrice = ApplicationModel.host + "changeMarketPrice.php";
	private string URLRenameCard = ApplicationModel.host + "renameCard.php";
	private string URLBuyCard = ApplicationModel.host + "buyCard.php";
	private string URLBuyRandomCard = ApplicationModel.host + "buyRandomCard.php";

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
	public StatModifier TileModifier = null;
	public int onSale ;
	public int RenameCost = 200;
	public string Error;
	public List<int> Decks;
	public IList<Skill> NewSkills;
	public int CollectionPoints;

	public static bool xpDone = false;
	
	public Card()
	{
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
	
	public int GetAttack()
	{
		int attack = Attack;
		foreach (StatModifier modifier in modifiers)
		{
			attack = modifier.modifyAttack(attack);
		}
		if (attack < 0)
		{
			return 0;
		}
		return attack;
	}
	
	public int GetLife()
	{
		int life = Life;
		foreach (StatModifier modifier in modifiers)
		{
			life = modifier.modifyLife(life);
		}
		
		if (life < 0)
		{
			return 0;
		}
		
		if (life>this.Life){
			return Life ;
		}
		else{
			return life;
		}
	}
	
	public int GetSpeed()
	{
		int speed = Speed;
		foreach (StatModifier modifier in modifiers)
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
		if (TileModifier != null)
		{
			move = TileModifier.modifyMove(move);
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
			if (this.modifiers [i].Type == ModifierType.Type_Paralized)
			{
				isParalyzed = true;
			}
			i++;
		}
		return isParalyzed;
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

	public void changeModifiers()
	{
		List<StatModifier> temp = new List<StatModifier>();
		foreach (StatModifier modifier in modifiers)
		{
			if (!modifier.Active)
			{
				modifier.Active = true;
				modifier.Duration--;
				temp.Add(modifier);
			} else if (modifier.Duration != 0)
			{
				temp.Add(modifier);
				modifier.Duration--;
			}
		}
		modifiers = temp;
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
	
	public bool verifyC(float minLife, float maxLife, float minAttack, float maxAttack, float minMove, float maxMove, float minQuickness, float maxQuickness)
	{
		if (minLife > this.Life || maxLife < this.Life)
		{
			return false;
		} else if (minAttack > this.Attack || maxAttack < this.Attack)
		{
			return false;
		} else if (minMove > this.Move || maxMove < this.Move)
		{
			return false;
		} else if (minQuickness > this.Speed || maxQuickness < this.Speed)
		{
			return false;
		} else
		{
			return true;
		}
	}

	public bool verifyC2(float minLife, float maxLife, float minAttack, float maxAttack, float minMove, float maxMove, float minQuickness, float maxQuickness, int minPrice, int maxPrice)
	{
		if (minLife > this.Life || maxLife < this.Life)
		{
			return false;
		} else if (minAttack > this.Attack || maxAttack < this.Attack)
		{
			return false;
		} else if (minMove > this.Move || maxMove < this.Move)
		{
			return false;
		} else if (minQuickness > this.Speed || maxQuickness < this.Speed)
		{
			return false;
		} else if (minPrice > this.Price || maxPrice < this.Price)
		{
			return false;
		} else
		{
			return true;
		}
	}

	public int getCost()
	{
		int cost = Mathf.RoundToInt(this.Speed +
			this.Attack +
			this.Move * 10 +
			this.Life);
		for (int i = 0; i < this.Skills.Count; i++)
		{
			if (this.Skills [i].IsActivated == 1)
			{
				cost += this.Skills [i].Power * (1 / this.Skills [i].ManaCost);
			}
		}
		return cost;
	}

	public IEnumerator buyRandomCard(int cost, int cardType=-1)
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_cost", cost);
		form.AddField("myform_cardtype", cardType.ToString());
		WWW w = new WWW(URLBuyRandomCard, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null)
		{
			this.Error = w.error;
		} else
		{
			string[] data = w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.Error = data [0];
			if (this.Error == "")
			{
				string[] cardData = data [1].Split(new string[] { "\n" }, System.StringSplitOptions.None);
				string[] cardInformation = cardData [0].Split(new string[] { "//" }, System.StringSplitOptions.None);
				this.Id = System.Convert.ToInt32(cardInformation [0]);
				this.Title = cardInformation [1];
				this.Life = System.Convert.ToInt32(cardInformation [2]);
				this.Attack = System.Convert.ToInt32(cardInformation [3]);
				this.Speed = System.Convert.ToInt32(cardInformation [4]);
				this.Move = System.Convert.ToInt32(cardInformation [5]);
				this.ArtIndex = System.Convert.ToInt32(cardInformation [6]);
				this.IdClass = System.Convert.ToInt32(cardInformation [7]);
				this.TitleClass = cardInformation [8];
				this.LifeLevel = System.Convert.ToInt32(cardInformation [9]);
				this.MoveLevel = System.Convert.ToInt32(cardInformation [10]);
				this.SpeedLevel = System.Convert.ToInt32(cardInformation [11]);
				this.AttackLevel = System.Convert.ToInt32(cardInformation [12]);
				this.onSale = 0;
				this.Experience = 0;

				this.Skills = new List<Skill>();
				
				for (int i = 1; i < 5; i++)
				{         
					cardInformation = cardData [i].Split(new string[] { "//" }, System.StringSplitOptions.None);
					this.Skills.Add(new Skill());
					this.Skills [this.Skills.Count - 1].Name = cardInformation [0];
					this.Skills [this.Skills.Count - 1].Id = System.Convert.ToInt32(cardInformation [1]);
					this.Skills [this.Skills.Count - 1].IsActivated = System.Convert.ToInt32(cardInformation [2]);
					this.Skills [this.Skills.Count - 1].Level = System.Convert.ToInt32(cardInformation [3]);
					this.Skills [this.Skills.Count - 1].Power = System.Convert.ToInt32(cardInformation [4]);
					this.Skills [this.Skills.Count - 1].ManaCost = System.Convert.ToInt32(cardInformation [5]);
					this.Skills [this.Skills.Count - 1].Description = cardInformation [6];
				}
			}
		}
	}
	public IEnumerator addXpLevel()
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_idcard", this.Id.ToString());
		form.AddField("myform_nick", ApplicationModel.username);
		
		WWW w = new WWW(URLAddXpLevel, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		
		if (w.error != null)
		{
			this.Error = w.error; 										// donne l'erreur eventuelle
		} else
		{
			if (w.text.Contains("#ERROR#"))
			{
				string[] errors = w.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
				this.Error = errors [1];
			} else
			{
				this.Error = "";
				string [] cardData = w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
				string [] cardInformations = cardData [0].Split(new string[] { "#S#" }, System.StringSplitOptions.None);
				this.CollectionPoints = System.Convert.ToInt32(cardData [1]);
				for (int j = 0; j < cardInformations.Length-1; j++)
				{
					string[] cardInfo = cardInformations [j].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
					if (j == 0)
					{

						this.Life = System.Convert.ToInt32(cardInfo [0]);
						this.Attack = System.Convert.ToInt32(cardInfo [1]);
						this.Speed = System.Convert.ToInt32(cardInfo [2]);
						this.Move = System.Convert.ToInt32(cardInfo [3]);
						this.LifeLevel = System.Convert.ToInt32(cardInfo [4]);
						this.MoveLevel = System.Convert.ToInt32(cardInfo [5]);
						this.SpeedLevel = System.Convert.ToInt32(cardInfo [6]);
						this.AttackLevel = System.Convert.ToInt32(cardInfo [7]);
						this.Experience = System.Convert.ToInt32(cardInfo [8]);
						this.ExperienceLevel = System.Convert.ToInt32(cardInfo [9]);
						this.NextLevelPrice = System.Convert.ToInt16(cardInfo [10]);
						this.PercentageToNextLevel = System.Convert.ToInt16(cardInfo [11]);
					} else
					{
						this.Skills [j - 1].Level = System.Convert.ToInt32(cardInfo [0]);
						this.Skills [j - 1].Power = System.Convert.ToInt32(cardInfo [1]);
						this.Skills [j - 1].Description = cardInfo [2];
					}
				}
			}
		}
	}

	public IEnumerator sellCard()
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", this.Id);
		form.AddField("myform_cost", this.getCost());		
		WWW w = new WWW(URLSellCard, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null)
		{
			this.Error = w.error;
		} else
		{
			this.Error = w.text;
		}
	}
	public IEnumerator toSell(int price)
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", Id);
		form.AddField("myform_price", price);	
		WWW w = new WWW(URLPutOnMarket, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		
		if (w.error != null)
		{
			this.Error = w.error;
		} else
		{
			this.Error = w.text;
			if (this.Error == "")
			{
				this.onSale = 1;
				this.Price = price;
			}
		}
	}
	public IEnumerator notToSell()
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", Id);
		WWW w = new WWW(URLRemoveFromMarket, form);             				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		
		if (w.error != null)
		{
			this.Error = w.error;
		} else
		{
			this.Error = w.text;
			if (this.Error == "")
			{
				this.onSale = 0;
			}
		}
	}
	
	public IEnumerator changePriceCard(int price)
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", Id);
		form.AddField("myform_price", price);
		WWW w = new WWW(URLChangeMarketPrice, form); 				            // On envoie le formulaire à l'url sur le serveur 
		yield return w;
		
		if (w.error != null)
		{
			this.Error = w.error;
		} else
		{
			this.Error = w.text;
			if (this.Error == "")
			{
				this.Price = price;
			}
		}
	}
	
	public IEnumerator renameCard(string newName, int renameCost)
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", Id);
		form.AddField("myform_title", newName);
		form.AddField("myform_cost", renameCost);
		
		WWW w = new WWW(URLRenameCard, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		
		if (w.error != null)
		{
			this.Error = w.error;
		} else
		{
			this.Error = w.text;
			if (this.Error == "")
			{
				this.Title = newName;
			}
		}
	}
	public IEnumerator buyCard()
	{
		this.NewSkills = new List<Skill>();
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", this.Id);
		
		WWW w = new WWW(URLBuyCard, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		
		if (w.error != null)
		{
			this.Error = w.error;
		} else
		{
			if (w.text.Contains("#ERROR#"))
			{
				string[] errors = w.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
				this.Error = errors [1];
				if (w.text.Contains("#SOLD#"))
				{
					this.onSale = 0;
				}
			} else
			{
				this.Error = "";
				this.onSale = 0;
				string[] data = w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
				this.CollectionPoints = System.Convert.ToInt32(data [0]);
				string[] newSkills = data [1].Split(new string[] { "//" }, System.StringSplitOptions.None);
				for (int i=0; i<newSkills.Length-1; i++)
				{
					this.NewSkills.Add(new Skill());
					this.NewSkills [i].Name = newSkills [i];
				}
			}
		}
	}
}