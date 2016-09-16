using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable] 
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
	public CardType CardType;
	public string TitleClass;
	public int IdOWner;
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
	public int LifeNbUpgrades;
	public int AttackNbUpgrades;
	public int CaracteristicUpgraded;
	public int CaracteristicIncrease;
	public string String;
	public bool ToSync;
	private string urlSyncCard = ApplicationModel.host + "card_sync.php"; 
	
	public Card()
	{
		this.Skills = new List<Skill>();
		this.CardType=new CardType();
		this.Decks=new List<int>();
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
		this.CardType=new CardType();
		this.CardType.Id = artIndex;
		this.Speed = speed;
		this.Move = move;
		this.Attack = attack;
		this.Skills = skills;
	}

	public Card(string title, int life, int artIndex, int move, int attack, List<Skill> skills)
	{
		this.Title = title;
		this.Life = life;
		this.CardType=new CardType();
		this.CardType.Id = artIndex;
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
		this.CardType=new CardType();
		this.CardType.Id = idClass;
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
		this.CardType=new CardType();
		this.CardType.Id = idClass;
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
		this.CardType=new CardType();
		this.CardType.Id = idClass;
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
	
	public List<Skill> getSkills()
	{
		return this.Skills;
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
		return this.Skills[0].Power;
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
	
	public virtual string getSkillText(string s){
		int index ;
		int percentage ;
		string tempstring ;
		if (s.Contains("%BTK")){
			index = s.IndexOf("%BTK");
			
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.Max(1,Mathf.RoundToInt(Int32.Parse(tempstring)*this.Attack/100f));
			s = s.Substring(0,index-3)+percentage+s.Substring(index+4,s.Length-index-4);
		}
		if (s.Contains("%ATK")){
			index = s.IndexOf("%ATK");
			
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.Max(Mathf.RoundToInt(Int32.Parse(tempstring)*this.getAttack()/100f));
			s = s.Substring(0,index-3)+percentage+s.Substring(index+4,s.Length-index-4);
		}
		if (s.Contains("%ATK")){
			index = s.IndexOf("%ATK");
			
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.Max(Mathf.RoundToInt(Int32.Parse(tempstring)*this.getAttack()/100f));
			s = s.Substring(0,index-3)+percentage+s.Substring(index+4,s.Length-index-4);
		}
		if (s.Contains("%PV")){
			index = s.IndexOf("%PV");
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.Max(Mathf.RoundToInt(Int32.Parse(tempstring)*this.getLife()/100f));
			s = s.Substring(0,index-4)+" "+percentage+" "+s.Substring(index+4,s.Length-index-4);
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
	public string getName()
	{
		if(this.Title!="")
		{
			return this.Title;
		}
		else
		{
			return WordingCardName.getName(this.Skills[0].Id);
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
				this.CardType=new CardType();
				this.CardType.Id=System.Convert.ToInt32(cardInfo[7]);
				this.PowerLevel=System.Convert.ToInt32(cardInfo[8]);
				this.LifeLevel=System.Convert.ToInt32(cardInfo[9]);
				this.AttackLevel=System.Convert.ToInt32(cardInfo[10]);
				this.MoveLevel=System.Convert.ToInt32(cardInfo[11]);
				this.SpeedLevel=System.Convert.ToInt32(cardInfo[12]);
				this.Experience=System.Convert.ToInt32(cardInfo[13]);
				this.ExperienceLevel=System.Convert.ToInt32(cardInfo[14]);
				this.PercentageToNextLevel=System.Convert.ToInt32(cardInfo[15]);
				this.NextLevelPrice=System.Convert.ToInt32(cardInfo[16]);
				this.onSale=System.Convert.ToInt32(cardInfo[17]);
				this.Price=System.Convert.ToInt32(cardInfo[18]);
				this.OnSaleDate=DateTime.ParseExact(cardInfo[19], "yyyy-MM-dd HH:mm:ss", null);
				this.nbWin=System.Convert.ToInt32(cardInfo[20]);
				this.nbLoose=System.Convert.ToInt32(cardInfo[21]);
				this.destructionPrice=System.Convert.ToInt32(cardInfo[22]);
				this.Power=System.Convert.ToInt32(cardInfo[23]);
				this.LifeNbUpgrades=System.Convert.ToInt32(cardInfo[24]);
				this.AttackNbUpgrades=System.Convert.ToInt32(cardInfo[25]);
				this.UpgradedLife=System.Convert.ToInt32(cardInfo[26]);
				this.UpgradedAttack=System.Convert.ToInt32(cardInfo[27]);
				this.UpgradedSpeed=System.Convert.ToInt32(cardInfo[28]);
				this.UpgradedLifeLevel=System.Convert.ToInt32(cardInfo[29]);
				this.UpgradedAttackLevel=System.Convert.ToInt32(cardInfo[30]);
				this.UpgradedSpeedLevel=System.Convert.ToInt32(cardInfo[31]);
				this.Skills=new List<Skill>();
			}
			else
			{
				this.Skills.Add(new Skill ());
				this.Skills[j-1].AllProbas=new int[10];
				this.Skills[j-1].Name=WordingSkills.getName(System.Convert.ToInt32(cardInfo[0]));
				this.Skills[j-1].Id=System.Convert.ToInt32(cardInfo[0]);
				this.Skills[j-1].IdSkillType=System.Convert.ToInt32(cardInfo[2]);
				this.Skills[j-1].IsActivated=System.Convert.ToInt32(cardInfo[3]);
				this.Skills[j-1].Level=System.Convert.ToInt32(cardInfo[4]);
				this.Skills[j-1].Power=System.Convert.ToInt32(cardInfo[5]);
				this.Skills[j-1].Upgrades=System.Convert.ToInt32(cardInfo[6]);
				this.Skills[j-1].Description=WordingSkills.getDescription(this.Skills[j-1].Id,this.Skills[j-1].Power-1);
				this.Skills[j-1].nextDescription=cardInfo[9];
				this.Skills[j-1].nextProba=System.Convert.ToInt32(cardInfo[10]);
				this.Skills[j-1].nextLevel=System.Convert.ToInt32(cardInfo[11]);
				this.Skills[j-1].IdCardType=this.CardType.Id;
			}
		}
	}
	public void updateCardXp(bool toNextLevel, int xp)
	{
		this.defineUpgradedCard();
		this.GetNewSkill = false;
		int newCardXp=this.Experience+xp;
		int newCardLevel = this.ExperienceLevel;
		int newNextLevelPrice=0;
		int newPercentageToNextLevel=0;
		int backLevel=0;
		if(!toNextLevel)
		{
			for(int i=0;i<ApplicationModel.xpLevels.Count;i++)
			{
				if(newCardXp<ApplicationModel.xpLevels[i])
				{
					newCardLevel=i-1;
					newNextLevelPrice=ApplicationModel.xpLevels[i+1]-newCardXp;
					newPercentageToNextLevel=Mathf.CeilToInt(100f*((newCardXp-backLevel)/(ApplicationModel.xpLevels[i]-backLevel)));
					break;
				}
				else if(newCardXp==ApplicationModel.xpLevels[i])
				{
					newCardLevel=i;
					if(i==ApplicationModel.xpLevels.Count-1)
					{
						newNextLevelPrice=-1;
						newPercentageToNextLevel=100;
					}
					else
					{
						newNextLevelPrice=ApplicationModel.xpLevels[i+1]-newCardXp;
						newPercentageToNextLevel=0;
					}
					break;
				}
				backLevel=ApplicationModel.xpLevels[i];
			}
			if(newCardXp>ApplicationModel.xpLevels[ApplicationModel.xpLevels.Count-1])
			{
				newCardLevel=ApplicationModel.xpLevels[ApplicationModel.xpLevels.Count-1];
				newNextLevelPrice=-1;
				newPercentageToNextLevel=100;
			}
		}
		else
		{
			for(int i=0;i<ApplicationModel.xpLevels.Count;i++)
			{
				if(i==this.ExperienceLevel+1)
				{
					newCardLevel=i;
					newCardXp=ApplicationModel.xpLevels[i];
					if(i==ApplicationModel.xpLevels.Count-1)
					{
						newNextLevelPrice=-1;
						newPercentageToNextLevel=100;
					}
					else
					{
						newNextLevelPrice=ApplicationModel.xpLevels[i+1]-newCardXp;
						newPercentageToNextLevel=0;
					}
					break;
				}
			}
		}
		for (int i = 0; i < this.Skills.Count; i++) 
		{
			this.Skills[i].IsNew = false;
		}
		if(newCardLevel!=this.ExperienceLevel)
		{
			if(newCardLevel==4)
			{
				this.GetNewSkill=true;
				this.Skills[2].IsActivated=1;
				this.Skills[2].IsNew = true;
			}
			else if(newCardLevel==8)
			{
				this.GetNewSkill=true;
				this.Skills[3].IsActivated=1;
				this.Skills[3].IsNew = true;
			}
		}

		this.Experience=newCardXp;
		this.ExperienceLevel=newCardLevel;
		this.PercentageToNextLevel=newPercentageToNextLevel;
		this.NextLevelPrice=newNextLevelPrice;
	}
	public void updateCardAttribute(int attribute, int newPower, int newLevel)
	{
		int caracteristicincrease=-1;
		switch(attribute)
		{
			case -1:
				caracteristicincrease=-1;
				break;
			case 0:
				caracteristicincrease = newPower - this.Attack;
				this.AttackNbUpgrades = this.AttackNbUpgrades + 1;
				this.Attack = newPower;
				break;
			case 1:
				caracteristicincrease = newPower - this.Life;
				this.LifeNbUpgrades = this.LifeNbUpgrades + 1;
				this.Life = newPower;
				break;
			case 2:
				break;
			case 3: case 4: case 5: case 6:
				caracteristicincrease=1;
				this.Skills[attribute-3].Upgrades=this.Skills[attribute-3].Upgrades+1;
				this.Skills[attribute-3].Power=newPower;
				this.Skills[attribute-3].Level=newLevel;
				break;
		}
		this.CaracteristicIncrease = caracteristicincrease;
		this.CaracteristicUpgraded = attribute;
		this.updateCardPower();
		//this.defineUpgradedCard();
	}
	public void updateCardPower()
	{
		int nbLevel2=0;
		int nbLevel3=0;
		int cardPower=0;
		int cardPowerLevel=0;

		if(this.LifeLevel==3)
		{
			nbLevel3++;
		}
		else if(this.LifeLevel==2)
		{
			nbLevel2++;
		}
		if(this.AttackLevel==3)
		{
			nbLevel3++;
		}
		else if(this.AttackLevel==2)
		{
			nbLevel2++;
		}
		for(int i=0;i<this.Skills.Count;i++)
		{
			if(this.Skills[i].IsActivated==1)
			{
				if(this.Skills[i].Level==3)
				{
					nbLevel3++;
				}
				else if(this.Skills[i].Level==2)
				{
					nbLevel2++;
				}
			}
		}
		if(nbLevel3>=3)
		{
			this.PowerLevel=3;
			this.Power=80;
		}
		else if(nbLevel3+nbLevel2>=3)
		{
			this.PowerLevel=2;
			this.Power=50;
		}
		else
		{
			this.PowerLevel=1;
			this.Power=20;
		}
		this.destructionPrice=this.Power;
	}
	public void defineUpgradedCard()
	{
		float calculus=0;
		this.UpgradedLife=this.Life+Mathf.CeilToInt(ApplicationModel.cardTypes.getCardType(this.CardType.Id).MaxLife/50f);
		if(this.UpgradedLife>ApplicationModel.cardTypes.getCardType(this.CardType.Id).MaxLife)
		{
			this.UpgradedLife=ApplicationModel.cardTypes.getCardType(this.CardType.Id).MaxLife;
		}
		calculus=10f*(((float)this.UpgradedLife-(float)(ApplicationModel.cardTypes.getCardType(this.CardType.Id).MinLife))/((float)ApplicationModel.cardTypes.getCardType(this.CardType.Id).MaxLife-(float)ApplicationModel.cardTypes.getCardType(this.CardType.Id).MinLife));
		if(calculus>8f)
		{
			this.UpgradedLifeLevel=3;
		}
		else if(calculus>5f)
		{
			this.UpgradedLifeLevel=2;
		}
		else
		{
			this.UpgradedLifeLevel=1;
		}

		calculus = 0;

		this.UpgradedAttack=this.Attack+Mathf.CeilToInt(ApplicationModel.cardTypes.getCardType(this.CardType.Id).MaxAttack/40f);
		if(this.UpgradedAttack>ApplicationModel.cardTypes.getCardType(this.CardType.Id).MaxAttack)
		{
			this.UpgradedAttack=ApplicationModel.cardTypes.getCardType(this.CardType.Id).MaxAttack;
		}
		calculus=10f*(((float)this.UpgradedAttack-(float)(ApplicationModel.cardTypes.getCardType(this.CardType.Id).MinAttack))/((float)ApplicationModel.cardTypes.getCardType(this.CardType.Id).MaxAttack-(float)ApplicationModel.cardTypes.getCardType(this.CardType.Id).MinAttack));
		if(calculus>8f)
		{
			this.UpgradedAttackLevel=3;
		}
		else if(calculus>5f)
		{
			this.UpgradedAttackLevel=2;
		}
		else
		{
			this.UpgradedAttackLevel=1;
		}
	}
	public void setString()
	{
		this.String="";
		this.String=this.Id.ToString()+"DATA"; //0
		this.String+=this.Title+"DATA"; //1
		this.String+=this.Life.ToString()+"DATA"; //2
		this.String+=this.Attack.ToString()+"DATA"; //3
		this.String+=this.Speed.ToString()+"DATA"; //4
		this.String+=this.Move.ToString()+"DATA"; //5
		this.String+=this.IdOWner.ToString()+"DATA"; //6
		this.String+=this.CardType.Id.ToString()+"DATA"; //7
		this.String+=this.PowerLevel.ToString()+"DATA"; //8
		this.String+=this.LifeLevel.ToString()+"DATA"; //9
		this.String+=this.AttackLevel.ToString()+"DATA"; //10
		this.String+=this.MoveLevel.ToString()+"DATA"; //11
		this.String+=this.SpeedLevel.ToString()+"DATA"; //12
		this.String+=this.Experience.ToString()+"DATA"; //13
		this.String+=this.ExperienceLevel.ToString()+"DATA"; //14
		this.String+=this.PercentageToNextLevel.ToString()+"DATA"; //15
		this.String+=this.NextLevelPrice.ToString()+"DATA"; //16
		this.String+=this.onSale.ToString()+"DATA"; //17
		this.String+=this.Price.ToString()+"DATA"; //18
		this.String+=this.OnSaleDate.ToString("yyyy-MM-dd HH:mm:ss")+"DATA"; //19
		this.String+=this.nbWin.ToString()+"DATA"; //20
		this.String+=this.nbLoose.ToString()+"DATA"; //21
		this.String+=this.destructionPrice.ToString()+"DATA"; //22
		this.String+=this.Power.ToString()+"DATA"; //23
		this.String+=this.LifeNbUpgrades.ToString()+"DATA"; //24
		this.String+=this.AttackNbUpgrades.ToString()+"DATA"; //25
		this.String+=this.UpgradedLife.ToString()+"DATA"; //26
		this.String+=this.UpgradedAttack.ToString()+"DATA"; //27
		this.String+=this.UpgradedSpeed.ToString()+"DATA"; //28
		this.String+=this.UpgradedLifeLevel.ToString()+"DATA"; //29
		this.String+=this.UpgradedAttackLevel.ToString()+"DATA"; //30
		this.String+=this.UpgradedSpeedLevel.ToString()+"DATA"; //31

		for(int i=0;i<this.Skills.Count;i++)
		{
			this.String	+="SKILL";
			this.String+=this.Skills[i].Id.ToString()+"DATA"; //0
			this.String+=this.Skills[i].IdSkillType.ToString()+"DATA"; //1
			this.String+=this.Skills[i].IsActivated.ToString()+"DATA"; //2
			this.String+=this.Skills[i].Level.ToString()+"DATA"; //3
			this.String+=this.Skills[i].Power.ToString()+"DATA"; //4
			this.String+=this.Skills[i].Upgrades.ToString()+"DATA"; //5
			this.String+=this.Skills[i].nextDescription+"DATA"; //6
			this.String+=this.Skills[i].nextProba.ToString()+"DATA"; //7
			this.String+=this.Skills[i].nextLevel.ToString(); //8
		}
	}
	public IEnumerator syncCard()
	{
		bool isConnected = true;
		if (ApplicationModel.player.IsOnline) {
			WWWForm form = new WWWForm (); 								// Création de la connexion
			form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
			form.AddField ("myform_cardData", this.String);

			ServerController.instance.setRequest (urlSyncCard, form);
			yield return ServerController.instance.StartCoroutine ("executeRequest");
			this.Error = ServerController.instance.getError ();
			if (this.Error != "") {
				isConnected = false;
			}
		}
		if (!isConnected || !ApplicationModel.player.IsOnline) {
			ApplicationModel.player.cardsToSync.update (this);
		}
		ApplicationModel.Save ();
		yield break;
	}
}