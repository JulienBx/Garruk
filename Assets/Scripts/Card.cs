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
	public int AttackLevel;
	public int MoveLevel;
	public int LifeLevel;
	public int SpeedLevel;
	public int Price;
	public int Experience;
	public int ExperienceLevel;
	public DateTime OnSaleDate;
	public List<StatModifier> modifiers = new List<StatModifier>();
	public int onSale ;
	public static int[] experienceLevels = new int[] { 0, 10, 40, 100, 200,350,600,1000,1500,2200,3000,0 };
	string URLAddXp =  "http://54.77.118.214/GarrukServer/addxp.php"; 

	public static bool xpDone=false;
	
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
	            DateTime onSaleDate,
	            int experience)
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
	            int experience)
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
	}

	public Card(int experienceLevel)
	{
		this.ExperienceLevel= experienceLevel;
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
			life = modifier.modifyLife(life);
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
		foreach (StatModifier modifier in modifiers) {
			move = modifier.modifyMove(move);
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



	public IEnumerator addXp (int xp, int price)
	{
			
		xpDone = false;
		int experience = this.Experience;
		int idCard = this.Id;
		string attributeName="";
		int idSkill=-1;
		int idLevel=1;
		int newPower=0;
		int cardType = this.IdClass;

		this.Experience = this.Experience+xp;
		getCardXpLevel ();
		
		if (this.ExperienceLevel!=10 && this.Experience>=experienceLevels [this.ExperienceLevel + 1]){
			
			int nbAttributes=4;
			
			for (int i = 0; i < this.Skills.Count; i++){
				
				if(this.Skills[i].IsActivated==1)
					nbAttributes=nbAttributes+1;
			}

			int randomAttribute = Mathf.RoundToInt(UnityEngine.Random.Range(0,nbAttributes));
			int randomPower = Mathf.RoundToInt (UnityEngine.Random.Range (5,10));
			
			switch (randomAttribute)
			{
			case 0:
				newPower=this.Move+1;
				this.Move=newPower;
				attributeName="move";

				break;
			case 1:
				newPower=Mathf.RoundToInt((1+randomPower*0.01f)*this.Life);
				attributeName="life";
				if (newPower>=100 || newPower>(100-Mathf.Sqrt(500f)))
					idLevel=3;
				else if (newPower>(100-Mathf.Sqrt(2000f)))
					idLevel=2;
				this.Life=newPower;
				this.LifeLevel=idLevel;
				
				break;
			case 2:
				newPower=Mathf.RoundToInt((1+randomPower*0.01f)*this.Attack);
				attributeName="attack";
				if (newPower>=100 || newPower>(100-Mathf.Sqrt(500f)))
					idLevel=3;
				else if (newPower>(100-Mathf.Sqrt(2000f)))
					idLevel=2;
				this.Attack=newPower;
				this.AttackLevel=idLevel;
				
				break;
			case 3:
				newPower=Mathf.RoundToInt((1+randomPower*0.01f)*this.Speed);
				attributeName="speed";
				if (newPower>=100 || newPower>(100-Mathf.Sqrt(500f)))
					idLevel=3;
				else if (newPower>(100-Mathf.Sqrt(2000f)))
					idLevel=2;
				this.Speed=newPower;
				this.SpeedLevel=idLevel;
				
				break;
			case 4:
				newPower=Mathf.RoundToInt((1+randomPower*0.01f)*this.Skills[0].Power);
				idSkill=this.Skills[0].Id;
				if (newPower>=100 || newPower>(100-Mathf.Sqrt(500f)))
					idLevel=3;
				else if (newPower>(100-Mathf.Sqrt(2000f)))
					idLevel=2;
				this.Skills[0].Power=newPower;
				this.Skills[0].Level=idLevel;
				
				break;
			case 5:
				newPower=Mathf.RoundToInt((1+randomPower*0.01f)*this.Skills[0].Power);
				idSkill=this.Skills[1].Id;
				if (newPower>=100 || newPower>(100-Mathf.Sqrt(500f)))
					idLevel=3;
				else if (newPower>(100-Mathf.Sqrt(2000f)))
					idLevel=2;
				this.Skills[1].Power=newPower;
				this.Skills[1].Level=idLevel;
				
				break;
			case 6:
				newPower=Mathf.RoundToInt((1+randomPower*0.01f)*this.Skills[0].Power);
				idSkill=this.Skills[2].Id;
				if (newPower>=100 || newPower>(100-Mathf.Sqrt(500f)))
					idLevel=3;
				else if (newPower>(100-Mathf.Sqrt(2000f)))
					idLevel=2;
				this.Skills[2].Power=newPower;
				this.Skills[2].Level=idLevel;
				
				break;
			case 7:
				newPower=Mathf.RoundToInt((1+randomPower*0.01f)*this.Skills[0].Power);
				idSkill=this.Skills[3].Id;
				if (newPower>=100 || newPower>(100-Mathf.Sqrt(500f)))
					idLevel=3;
				else if (newPower>(100-Mathf.Sqrt(2000f)))
					idLevel=2;
				this.Skills[3].Power=newPower;
				this.Skills[3].Level=idLevel;
				
				break;
			default:
				break;
			}
			
		}
		
		
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_idcard", idCard.ToString());
		form.AddField("myform_xp", this.Experience.ToString());
		form.AddField("myform_newpower",newPower.ToString());
		form.AddField("myform_attribute",attributeName);
		form.AddField("myform_idskill",idSkill.ToString());
		form.AddField("myform_cardtype",cardType.ToString());
		form.AddField("myform_level",idLevel.ToString());
		form.AddField ("myform_price", price.ToString ());
		form.AddField ("myform_nick", ApplicationModel.username);
		
		WWW w = new WWW(URLAddXp, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		
		if (w.error != null) 
		{
			MonoBehaviour.print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			if  (attributeName=="move"){
				this.MoveLevel=System.Convert.ToInt32(w.text);
			}
		}
		xpDone=true;
	}


	public int getPriceForNextLevel(){
		
		int experience = this.Experience;
		int level = 0;
		
		while (experience>=experienceLevels[level]&&level<11) {
			level++;
		}
		level =level-1;
		
		int price = 0;
		
		if (level!=10){
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

	public int percentageToNextXpLevel(){

		int percentage;
		if (this.ExperienceLevel==10){
			percentage = 100 ;
		}else if (this.ExperienceLevel==0){
			percentage = Mathf.RoundToInt((this.Experience/experienceLevels[1])*100);
		}else {
			percentage=100*(this.Experience-experienceLevels[this.ExperienceLevel])/(experienceLevels[this.ExperienceLevel+1]-experienceLevels[this.ExperienceLevel]); 
		}
		return percentage;
	}

}