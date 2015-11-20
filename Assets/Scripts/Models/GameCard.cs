using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameCard : Card 
{
	public List<Modifyer> damagesModifyers = new List<Modifyer>();
	public List<Modifyer> pvModifyers = new List<Modifyer>();
	public List<Modifyer> attackModifyers = new List<Modifyer>();
	public List<Modifyer> moveModifyers = new List<Modifyer>();
	public List<Modifyer> esquiveModifyers = new List<Modifyer>();
	public Modifyer state ;
	public int nbTurnsToWait ;
	public bool isDead;
	public bool hasMoved;
	public bool hasPlayed;
	
	public GameCard(){
	}
	
	public GameCard(Card c){
		base.Title = c.Title; 									    
		base.ArtIndex = c.ArtIndex;
		base.Life = c.Life;
		base.Speed = c.Speed;
		base.Move = c.Move;
		base.Attack = c.Attack;
		base.Skills = c.Skills;
		base.IdClass = c.IdClass;
		base.TitleClass = c.TitleClass;
		base.AttackLevel = c.AttackLevel;   
		base.MoveLevel = c.MoveLevel;
		base.LifeLevel = c.LifeLevel;
		base.SpeedLevel = c.SpeedLevel;
		base.deckOrder = c.deckOrder;
		base.PowerLevel = c.PowerLevel;
		this.isDead=false;
		this.state = new Modifyer();
	}
	
	public void checkPassiveSkills()
	{
		this.checkPacifiste();
		this.checkAguerri();
		this.checkRapide();
		this.checkRobuste();
		this.checkAgile();
	}
	
	public int getNbTurnsToWait(){
		return this.nbTurnsToWait ;
	}
	
	public void setNbTurnsToWait(int i){
		this.nbTurnsToWait = i;
	}
	
	public int getGameSpeed(){
		return base.Speed;
	}
	
	public bool getHasMoved(){
		return this.hasMoved;
	}
	
	public bool getHasPlayed(){
		return this.hasPlayed;
	}
	
	public void setHasMoved(bool b){
		this.hasMoved = b ;
	}
	
	public void setHasPlayed(bool b){
		this.hasPlayed = b ;
	}
	
	public bool isPiegeur()
	{
		return (base.Skills[0].Id == 64);
	}
	
	public bool isLeader()
	{
		return (base.Skills[0].Id == 76);
	}
	
	public bool isNurse()
	{
		return (base.Skills[0].Id == 75);
	}
	
	public bool isFrenetique()
	{
		return (base.Skills[0].Id == 69);
	}	
	
	public void checkPacifiste(){
		if((base.Skills[0].Id == 73)){
			int level = base.Skills[0].Level;
			float bonusLife = 0f ;
			float malusAttack = 0f;
			if (level==1){
				bonusLife = 0.2f;
				malusAttack = 0.5f;
			}
			else if (level==2){
				bonusLife = 0.25f;
				malusAttack = 0.5f;
			}
			else if (level==3){
				bonusLife = 0.25f;
				malusAttack = 0.4f;
			}
			else if (level==4){
				bonusLife = 0.3f;
				malusAttack = 0.4f;
			}
			else if (level==5){
				bonusLife = 0.3f;
				malusAttack = 0.3f;
			}
			else if (level==6){
				bonusLife = 0.35f;
				malusAttack = 0.3f;
			}
			else if (level==7){
				bonusLife = 0.4f;
				malusAttack = 0.3f;
			}
			else if (level==8){
				bonusLife = 0.4f;
				malusAttack = 0.2f;
			}
			else if (level==9){
				bonusLife = 0.45f;
				malusAttack = 0.2f;
			}
			else if (level==10){
				bonusLife = 0.5f;
				malusAttack = 0.2f;
			}
			
			int amountLife = Mathf.CeilToInt(base.getLife()*bonusLife);
			this.pvModifyers.Add (new Modifyer(amountLife, 0, 0, "Pacifisme", "Bonus permanent de "+bonusLife+" PV"));
			
			int amountAttack = -1*Mathf.CeilToInt(base.getAttack()*malusAttack);
			this.attackModifyers.Add (new Modifyer(amountAttack, 0, 0, "Pacifisme", "Malus permanent de "+bonusLife+" ATK"));
		}
	}
	
	public void checkAguerri(){
		if((base.Skills[0].Id == 68)){
			int bonusAttack = base.Skills[0].Level;
			this.attackModifyers.Add (new Modifyer(bonusAttack, 0, 0, "Aguerri", "Bonus permanent de "+bonusAttack+" ATK"));
		}
	}
	
	public void checkRapide(){
		if((base.Skills[0].Id == 71)){
			int level = base.Skills[0].Level;
			int bonusMove = 0 ;
			int malusAttack = 0;
			if (level==1){
				bonusMove = 1;
				malusAttack = -10;
			}
			else if (level==2){
				bonusMove = 1;
				malusAttack = -9;
			}
			else if (level==3){
				bonusMove = 1;
				malusAttack = -8;
			}
			else if (level==4){
				bonusMove = 1;
				malusAttack = -7;
			}
			else if (level==5){
				bonusMove = 1;
				malusAttack = -6;
			}
			else if (level==6){
				bonusMove = 1;
				malusAttack = -5;
			}
			else if (level==7){
				bonusMove = 1;
				malusAttack = -4;
			}
			else if (level==8){
				bonusMove = 2;
				malusAttack = -4;
			}
			else if (level==9){
				bonusMove = 2;
				malusAttack = -3;
			}
			else if (level==10){
				bonusMove = 2;
				malusAttack = -2;
			}
			
			this.moveModifyers.Add (new Modifyer(bonusMove, 0, 0, "Rapide", "Bonus permanent de "+bonusMove+" MOV"));
			this.attackModifyers.Add (new Modifyer(malusAttack, 0, 0, "Rapide", "Malus permanent de "+malusAttack+" ATK"));
		}
	}
	
	public void checkRobuste(){
		if((base.Skills[0].Id == 67)){
			int level = base.Skills[0].Level;
			int bonusAttack = 0;
			int malusMove = 0;
			if (level==1){
				bonusAttack = 2;
				malusMove = -3;
			}
			else if (level==2){
				bonusAttack = 3;
				malusMove = -3;
			}
			else if (level==3){
				bonusAttack = 4;
				malusMove = -3;
			}
			else if (level==4){
				bonusAttack = 5;
				malusMove = -3;
			}
			else if (level==5){
				bonusAttack = 5;
				malusMove = -2;
			}
			else if (level==6){
				bonusAttack = 6;
				malusMove = -2;
			}
			else if (level==7){
				bonusAttack = 7;
				malusMove = -2;
			}
			else if (level==8){
				bonusAttack = 7;
				malusMove = -1;
			}
			else if (level==9){
				bonusAttack = 8;
				malusMove = -1;
			}
			else if (level==10){
				bonusAttack = 9;
				malusMove = -1;
			}
			
			this.attackModifyers.Add (new Modifyer(bonusAttack, 0, 0, "Robuste", "Bonus permanent de "+bonusAttack+" ATK"));
			this.moveModifyers.Add (new Modifyer(malusMove, 0, 0, "Robuste", "Malus permanent de "+malusMove+" MOV"));
		}
	}
	
	public void checkAgile(){
		if((base.Skills[0].Id == 66)){
			int level = base.Skills[0].Level;
			int esquive = level * 5;
						
			this.esquiveModifyers.Add(new Modifyer(esquive, 0, 0, "Agile", "Esquive aux attaques de type combat : "+esquive+"%"));
		}
	}
	
	public void checkModifyers(){
		List<int> modifiersToSuppress = new List<int>();
		
		for (int i = this.attackModifyers.Count-1 ; i >=0 ; i--){
			if (this.attackModifyers [i].duration > 2){
				this.attackModifyers [i].description.Replace(this.attackModifyers [i].duration+" tours", (this.attackModifyers [i].duration-1)+" tours");
			}
			else if (this.attackModifyers [i].duration > 1){
				this.attackModifyers [i].description.Replace(this.attackModifyers [i].duration+" tours", "1 tour");
			}
			
			if (this.attackModifyers [i].duration > 0)
			{
				this.attackModifyers[i].duration--;
			}
			else if (this.attackModifyers [i].duration == -2){
				this.attackModifyers [i].duration = 1;
			}
			
			if (this.attackModifyers [i].duration == 0)
			{
				this.attackModifyers.RemoveAt(i);
			}
		}
		
		for (int i = this.moveModifyers.Count-1 ; i >=0 ; i--){
			if (this.moveModifyers [i].duration > 2){
				this.moveModifyers [i].description.Replace(this.moveModifyers [i].duration+" tours", (this.moveModifyers [i].duration-1)+" tours");
			}
			else if (this.moveModifyers [i].duration > 1){
				this.moveModifyers [i].description.Replace(this.moveModifyers [i].duration+" tours", "1 tour");
			}
			
			if (this.moveModifyers [i].duration > 0)
			{
				this.moveModifyers[i].duration--;
			}
			else if (this.moveModifyers [i].duration == -2){
				this.moveModifyers [i].duration = 1;
			}
			
			if (this.moveModifyers [i].duration == 0)
			{
				this.moveModifyers.RemoveAt(i);
			}
		}
		
		for (int i = this.damagesModifyers.Count-1 ; i >=0 ; i--){
			if (this.damagesModifyers [i].duration > 2){
				this.damagesModifyers [i].description.Replace(this.damagesModifyers [i].duration+" tours", (this.damagesModifyers [i].duration-1)+" tours");
			}
			else if (this.damagesModifyers [i].duration > 1){
				this.damagesModifyers [i].description.Replace(this.damagesModifyers [i].duration+" tours", "1 tour");
			}
			
			if (this.damagesModifyers [i].duration > 0)
			{
				this.damagesModifyers[i].duration--;
			}
			else if (this.damagesModifyers [i].duration == -2){
				this.damagesModifyers [i].duration = 1;
			}
			
			if (this.damagesModifyers [i].duration == 0)
			{
				this.damagesModifyers.RemoveAt(i);
			}
		}
		
		for (int i = this.pvModifyers.Count-1 ; i >=0 ; i--){
			if (this.pvModifyers [i].duration > 2){
				this.pvModifyers [i].description.Replace(this.pvModifyers [i].duration+" tours", (this.pvModifyers [i].duration-1)+" tours");
			}
			else if (this.damagesModifyers [i].duration > 1){
				this.pvModifyers [i].description.Replace(this.pvModifyers [i].duration+" tours", "1 tour");
			}
			
			if (this.pvModifyers [i].duration > 0)
			{
				this.pvModifyers[i].duration--;
			}
			else if (this.pvModifyers [i].duration == -2){
				this.pvModifyers [i].duration = 1;
			}
			
			if (this.pvModifyers [i].duration == 0)
			{
				this.pvModifyers.RemoveAt(i);
			}
		}
		
		for (int i = this.esquiveModifyers.Count-1 ; i >=0 ; i--){
			if (this.esquiveModifyers [i].duration > 2){
				this.esquiveModifyers [i].description.Replace(this.esquiveModifyers [i].duration+" tours", (this.esquiveModifyers [i].duration-1)+" tours");
			}
			else if (this.esquiveModifyers [i].duration > 1){
				this.esquiveModifyers [i].description.Replace(this.esquiveModifyers [i].duration+" tours", "1 tour");
			}
			
			if (this.esquiveModifyers [i].duration > 0)
			{
				this.esquiveModifyers[i].duration--;
			}
			else if (this.esquiveModifyers [i].duration == -2){
				this.esquiveModifyers [i].duration = 1;
			}
			
			if (this.esquiveModifyers [i].duration == 0)
			{
				this.esquiveModifyers.RemoveAt(i);
			}
		}
		
		if (state.duration > 2){
			state.description.Replace(state.duration+" tours", (state.duration-1)+" tours");
		}
		else if (state.duration > 1){
			state.description.Replace(state.duration+" tours", "1 tour");
		}
		
		if (state.duration > 0)
		{
			state.duration--;
		}
		else if (state.duration == -2){
			state.duration = 1;
		}
		
		if (state.duration == 0)
		{
			this.state = new Modifyer();
		}
	}
	
	public int getSleepingPercentage(){
		if(this.state.type==101){
			return this.state.amount;
		}
		else{
			return -1;
		}
	}
	
	public void removeSleeping()
	{
		this.state = new Modifyer();
	}
	
	public bool isParalyzed(){
		return(this.state.type==102);
	}
	
	public List<string> getIconAttack()
	{
		List<string> iconAttackTexts = new List<string>();
		int i = 0;
		while (i < this.attackModifyers.Count)
		{			
			iconAttackTexts.Add(this.attackModifyers [i].title);
			iconAttackTexts.Add(this.attackModifyers [i].description);
			i++;
		}
		return iconAttackTexts;
	}
	
	public List<string> getIconLife()
	{
		List<string> iconLifeTexts = new List<string>();
		int i = 0;
		if(this.getLife()!=base.Life){
		
		}
		while (i < this.pvModifyers.Count)
		{
			iconLifeTexts.Add(this.pvModifyers [i].title);
			iconLifeTexts.Add(this.pvModifyers [i].description);
			i++;
		}
		return iconLifeTexts;
	}
	
	public override int getLife(){
		int l = base.Life ;
		for(int i = 0 ; i < this.pvModifyers.Count ; i++){
			l+=this.pvModifyers[i].amount;
		}
		for(int i = 0 ; i < this.damagesModifyers.Count ; i++){
			l-=this.damagesModifyers[i].amount;
		}
		return Mathf.Max (0,l);
	}
	
	public override int getMove(){
		int m = base.Move ;
		for(int i = 0 ; i < this.moveModifyers.Count ; i++){
			m+=this.moveModifyers[i].amount;
		}
		return Mathf.Max (0,m);
	}
	
	public override int getAttack(){
		int a = base.Attack ;
		for(int i = 0 ; i < this.attackModifyers.Count ; i++){
			a+=this.attackModifyers[i].amount;
		}
		return Mathf.Max (0,a);
	}
	
	public Skill GetAttackSkill()
	{
		return new Skill("Attaque", "Inflige "+this.getAttack()+" dégats au contact",0,1,100);
	}
}