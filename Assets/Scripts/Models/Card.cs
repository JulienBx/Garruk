using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card
{
	private string URLAddXp                 = ApplicationModel.host + "addxp.php"; 
	private string URLSellCard              = ApplicationModel.host + "sellCard.php";
	private string URLPutOnMarket           = ApplicationModel.host + "putonmarket.php";
	private string URLRemoveFromMarket      = ApplicationModel.host + "removeFromMarket.php";
	private string URLChangeMarketPrice     = ApplicationModel.host + "changeMarketPrice.php";
	private string URLRenameCard            = ApplicationModel.host + "renameCard.php";
	private string URLBuyCard 				= ApplicationModel.host + "buyCard.php";
	private string URLBuyRandomCard 		= ApplicationModel.host + "buyRandomCard.php";

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
	public DateTime OnSaleDate;
	public List<StatModifier> modifiers = new List<StatModifier>();
	public int onSale ;
	public static int[] experienceLevels = new int[] { 0, 10, 40, 100, 200,350,600,1000,1500,2200,3000,0 };
	public int RenameCost = 200;
	public int buyRandomCardCost = 500;
	public string Error;

	public static bool xpDone=false;
	
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
	            int onSale ,
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
	public int GetAttack()
	{
		int attack = Attack;
		foreach (StatModifier modifier in modifiers) {
			//attack = modifier.modifyAttack(attack);
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
		foreach (StatModifier modifier in modifiers) {
			//life = modifier.modifyLife(life);
		}
		if (life < 0)
		{
			return 0;
		}
		return life;
	}
	
	public int GetSpeed()
	{
		int speed = Speed;
		foreach (StatModifier modifier in modifiers) {
			//speed = modifier.modifySpeed(speed);
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
		foreach (StatModifier modifier in modifiers) {
			//move = modifier.modifyMove(move);
		}
		if (move < 0)
		{
			return 0;
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
			if (Skills[i].IsActivated==1){
				if (Skills[i].Name.ToLower ().Contains(s)){
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
		else if (minQuickness > this.Speed || maxQuickness < this.Speed){
			return false ;
		}
		else{
			return true ;
		}
	}

	public bool verifyC2(float minLife,float maxLife,float minAttack,float maxAttack,float minMove,float maxMove,float minQuickness,float maxQuickness, int minPrice, int maxPrice){
		if (minLife > this.Life || maxLife < this.Life){
			return false ;
		}
		else if (minAttack > this.Attack || maxAttack < this.Attack){
			return false ;
		}
		else if (minMove > this.Move || maxMove < this.Move){
			return false ;
		}
		else if (minQuickness > this.Speed || maxQuickness < this.Speed){
			return false ;
		}
		else if (minPrice > this.Price || maxPrice < this.Price){
			return false ;
		}
		else{
			return true ;
		}
	}

	public int getCost(){
		int cost = Mathf.RoundToInt (this.Speed +
		                             this.Attack +
		                             this.Move * 10 +
		                             this.Life);
		for (int i = 0; i < this.Skills.Count; i++) {
			if (this.Skills[i].IsActivated==1){
				cost += this.Skills[i].Power * (1/this.Skills[i].ManaCost);
			}
		}
		return cost;
	}

	public IEnumerator buyRandomCard()
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_cost", this.buyRandomCardCost);		
		WWW w = new WWW(URLBuyRandomCard, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null)
		{
			this.Error=w.error;
		}
		else
		{
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.Error=data[1];
			if(this.Error=="")
			{
				string[] cardData = data[0].Split(new string[] { "\n" }, System.StringSplitOptions.None);
				string[] cardInformation = cardData[0].Split(new string[] { "//" }, System.StringSplitOptions.None);
				this.Id=System.Convert.ToInt32(cardInformation[0]);
				this.Title=cardInformation[1];
				this.Life=System.Convert.ToInt32(cardInformation[2]);
				this.Attack=System.Convert.ToInt32(cardInformation[3]);
				this.Speed=System.Convert.ToInt32(cardInformation[4]);
				this.Move=System.Convert.ToInt32(cardInformation[5]);
				this.ArtIndex=System.Convert.ToInt32(cardInformation[6]);
				this.IdClass=System.Convert.ToInt32(cardInformation[7]);
				this.TitleClass=cardInformation[8];
				this.LifeLevel=System.Convert.ToInt32(cardInformation[9]);
				this.MoveLevel=System.Convert.ToInt32(cardInformation[10]);
				this.SpeedLevel=System.Convert.ToInt32(cardInformation[11]);
				this.AttackLevel=System.Convert.ToInt32(cardInformation[12]);
				this.onSale=0;
				this.Experience=0;
				
				
				this.Skills = new List<Skill>();
				
				for (int i = 1; i < 5; i++) 
				{         
					cardInformation = cardData[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
					this.Skills.Add (new Skill (cardInformation [0], //skillName
					                            System.Convert.ToInt32 (cardInformation [1]), // idskill
					                            System.Convert.ToInt32 (cardInformation [2]), // isactivated
					                            System.Convert.ToInt32 (cardInformation [3]), // level
					                            System.Convert.ToInt32 (cardInformation [4]), // power
					                            System.Convert.ToInt32 (cardInformation [5]), // manaCost
					                            cardInformation [6])); // description
				}
			}
		}
	}
	public IEnumerator addXp (int xp, int price)
	{
		this.ExperienceLevel = this.getXpLevel ();

		string attributeName="";
		int idSkill=-1;
		int idLevel=1;
		int newPower=0;
		int experience = this.Experience + xp;
		int randomAttribute=0;
		
		if (this.ExperienceLevel!=10 && this.Experience>=experienceLevels [this.ExperienceLevel]){
			
			int nbAttributes=4;
			
			for (int i = 0; i < this.Skills.Count; i++){
				
				if(this.Skills[i].IsActivated==1)
					nbAttributes=nbAttributes+1;
			}

			randomAttribute = Mathf.RoundToInt(UnityEngine.Random.Range(0,nbAttributes));
			int randomPower = Mathf.RoundToInt (UnityEngine.Random.Range (5,10));
			
			switch (randomAttribute)
			{
			case 0:
				newPower=this.Move+1;
				attributeName="move";
				break;
			case 1:
				newPower=Mathf.RoundToInt((1+randomPower*0.01f)*this.Life);
				attributeName="life";
				if (newPower>=100 || newPower>(100-Mathf.Sqrt(500f)))
					idLevel=3;
				else if (newPower>(100-Mathf.Sqrt(2000f)))
					idLevel=2;
				break;
			case 2:
				newPower=Mathf.RoundToInt((1+randomPower*0.01f)*this.Attack);
				attributeName="attack";
				if (newPower>=100 || newPower>(100-Mathf.Sqrt(500f)))
					idLevel=3;
				else if (newPower>(100-Mathf.Sqrt(2000f)))
					idLevel=2;
				break;
			case 3:
				newPower=Mathf.RoundToInt((1+randomPower*0.01f)*this.Speed);
				attributeName="speed";
				if (newPower>=100 || newPower>(100-Mathf.Sqrt(500f)))
					idLevel=3;
				else if (newPower>(100-Mathf.Sqrt(2000f)))
					idLevel=2;
				break;
			case 4:
				newPower=Mathf.RoundToInt((1+randomPower*0.01f)*this.Skills[0].Power);
				idSkill=this.Skills[0].Id;
				if (newPower>=100 || newPower>(100-Mathf.Sqrt(500f)))
					idLevel=3;
				else if (newPower>(100-Mathf.Sqrt(2000f)))
					idLevel=2;
				break;
			case 5:
				newPower=Mathf.RoundToInt((1+randomPower*0.01f)*this.Skills[0].Power);
				idSkill=this.Skills[1].Id;
				if (newPower>=100 || newPower>(100-Mathf.Sqrt(500f)))
					idLevel=3;
				else if (newPower>(100-Mathf.Sqrt(2000f)))
					idLevel=2;
				break;
			case 6:
				newPower=Mathf.RoundToInt((1+randomPower*0.01f)*this.Skills[0].Power);
				idSkill=this.Skills[2].Id;
				if (newPower>=100 || newPower>(100-Mathf.Sqrt(500f)))
					idLevel=3;
				else if (newPower>(100-Mathf.Sqrt(2000f)))
					idLevel=2;
				break;
			case 7:
				newPower=Mathf.RoundToInt((1+randomPower*0.01f)*this.Skills[0].Power);
				idSkill=this.Skills[3].Id;
				if (newPower>=100 || newPower>(100-Mathf.Sqrt(500f)))
					idLevel=3;
				else if (newPower>(100-Mathf.Sqrt(2000f)))
					idLevel=2;
				break;
			default:
				break;
			}
		}
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_idcard", this.Id.ToString());
		form.AddField("myform_xp", experience.ToString());
		form.AddField("myform_newpower",newPower.ToString());
		form.AddField("myform_attribute",attributeName);
		form.AddField("myform_idskill",idSkill.ToString());
		form.AddField("myform_cardtype",this.IdClass.ToString());
		form.AddField("myform_level",idLevel.ToString());
		form.AddField ("myform_price", price.ToString ());
		form.AddField ("myform_nick", ApplicationModel.username);
		
		WWW w = new WWW(URLAddXp, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		
		if (w.error != null) 
		{
			this.Error=w.error; 										// donne l'erreur eventuelle
		} 
		else 
		{
			string[] data=w.text.Split(new string[] { "//" }, System.StringSplitOptions.None);
			this.Error=data[1];

			if (this.Error=="")
			{
				this.Experience=experience;
				if  (attributeName=="move")
				{
					this.MoveLevel=System.Convert.ToInt32(data[0]);
				}
				switch (randomAttribute)
				{
				case 0:
					this.Move=newPower;
					break;
				case 1:
					this.Life=newPower;
					this.LifeLevel=idLevel;
					break;
				case 2:
					this.Attack=newPower;
					this.AttackLevel=idLevel;
					break;
				case 3:
					this.Speed=newPower;
					this.SpeedLevel=idLevel;
					break;
				case 4:
					this.Skills[0].Power=newPower;
					this.Skills[0].Level=idLevel;
					break;
				case 5:
					this.Skills[1].Power=newPower;
					this.Skills[1].Level=idLevel;
					break;
				case 6:
					this.Skills[2].Power=newPower;
					this.Skills[2].Level=idLevel;
					break;
				case 7:
					this.Skills[3].Power=newPower;
					this.Skills[3].Level=idLevel;
					break;
				default:
					break;
				}
			}
			this.ExperienceLevel = this.getXpLevel ();
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
			this.Error=w.error;
		}
		else
		{
			this.Error=w.text;
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
			this.Error=w.error;
		}
		else
		{
			this.Error=w.text;
			if(this.Error=="")
			{
				this.onSale=1;
				this.Price=price;
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
			this.Error=w.error;
		}
		else
		{
			this.Error=w.text;
			if(this.Error=="")
			{
				this.onSale=0;
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
			this.Error=w.error;
		}
		else
		{
			this.Error=w.text;
			if(this.Error=="")
			{
				this.Price=price;
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
			this.Error=w.error;
		}
		else
		{
			this.Error=w.text;
			if(this.Error=="")
			{
				this.Title=newName;
			}
		}
	}
	public IEnumerator buyCard()
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", this.Id);
		
		WWW w = new WWW(URLBuyCard, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		
		if (w.error != null)
		{
			this.Error=w.error;
		}
		else
		{
			string[] data=w.text.Split(new string[] { "//" }, System.StringSplitOptions.None);
			this.Error=data[1];
			if(this.Error=="" )
			{
				this.onSale=0;
			}
			else
			{
				this.onSale=System.Convert.ToInt32(data[0]);
			}
		}
	}
	public int getPriceForNextLevel(){
		
		int experience = this.Experience;
		int level = 0;
		
		while (experience>=experienceLevels[level]&&level<11) 
		{
			level++;
		}
		level =level-1;
		
		int price = 0;
		
		if (level!=10)
		{
			price = experienceLevels [level + 1] - experience;
		}
		
		return price;	
	}
	
	public void getCardXpLevel(){

		int cardXp = this.Experience;
		int level = 0;
		
		while (cardXp>=experienceLevels[level]&&level<11) {
			level++;
		}
		level =level-1;

		this.ExperienceLevel = level;
	}
	public int getXpLevel(){
		
		int cardXp = this.Experience;
		int level = 0;
		
		while (cardXp>=experienceLevels[level]&&level<11) {
			level++;
		}
		level =level-1;
		
		return level;
	}	
	public int percentageToNextXpLevel(){

		int experienceLevel = this.getXpLevel ();
		int percentage;
		if (experienceLevel==10){
			percentage = 100 ;
		}else if (experienceLevel==0){
			percentage = Mathf.RoundToInt((this.Experience/experienceLevels[1])*100);
		}else {
			percentage=100*(this.Experience-experienceLevels[experienceLevel])/(experienceLevels[experienceLevel+1]-experienceLevels[experienceLevel]); 
		}
		return percentage;
	}

}