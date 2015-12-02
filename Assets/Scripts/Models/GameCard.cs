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
	public List<Modifyer> magicalEsquiveModifyers = new List<Modifyer>();
	public List<Modifyer> bouclierModifyers = new List<Modifyer>();
	public Modifyer state ;
	public bool isStateModifyed;
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
		this.isStateModifyed = false;
	}
	
	public void setState(Modifyer m){
		this.isStateModifyed = true ;
		this.state = m;
	}
	
	public void checkPassiveSkills()
	{
		this.checkPacifiste();
		this.checkAguerri();
		this.checkRapide();
		this.checkRobuste();
		this.checkAgile();
		this.checkCuirasse();
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
	
	public bool isLache()
	{
		return (base.Skills[0].Id == 65);
	}
	
	public bool isNurse()
	{
		return (base.Skills[0].Id == 75);
	}
	
	public bool isFrenetique()
	{
		return (base.Skills[0].Id == 69);
	}	
	
	public bool isGenerous()
	{
		bool b = false ;
		if (this.Skills[0].Id == 72){
			if(UnityEngine.Random.Range(1,101)<this.Skills[0].Power*3){
				b = true ;
			}
		}
		return b;
	}
	
	public void checkPacifiste(){
		if((base.Skills[0].Id == 73)){
			int level = base.Skills[0].Power;
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
			this.pvModifyers.Add (new Modifyer(amountLife, -1, 0, "Pacifisme", "Bonus permanent de "+amountLife+" PV"));
			
			int amountAttack = -1*Mathf.CeilToInt(base.getAttack()*malusAttack);
			this.attackModifyers.Add (new Modifyer(amountAttack, -1, 0, "Pacifisme", "Malus permanent de "+amountAttack+" ATK"));
		}
	}
	
	public void checkAguerri(){
		if((base.Skills[0].Id == 68)){
			int bonusAttack = base.Skills[0].Power;
			this.attackModifyers.Add (new Modifyer(bonusAttack, -1, 0, "Aguerri", "Bonus permanent de "+bonusAttack+" ATK"));
		}
	}
	
	public void checkCuirasse(){
		if((base.Skills[0].Id == 70)){
			int bonusShield = base.Skills[0].Power*4;
			this.bouclierModifyers.Add (new Modifyer(bonusShield, -1, 70, "Cuirassé", "Bouclier permanent : "+bonusShield+"%"));
		}
	}
	
	public void checkRapide(){
		if((base.Skills[0].Id == 71)){
			int level = base.Skills[0].Power;
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
			
			this.moveModifyers.Add (new Modifyer(bonusMove, -1, 0, "Rapide", "Bonus permanent de "+bonusMove+" MOV"));
			this.attackModifyers.Add (new Modifyer(malusAttack, -1, 0, "Rapide", "Malus permanent de "+malusAttack+" ATK"));
		}
	}
	
	public void checkRobuste(){
		if((base.Skills[0].Id == 67)){
			int level = base.Skills[0].Power;
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
			
			this.attackModifyers.Add (new Modifyer(bonusAttack, -1, 0, "Robuste", "Bonus permanent de "+bonusAttack+" ATK"));
			this.moveModifyers.Add (new Modifyer(malusMove, -1, 0, "Robuste", "Malus permanent de "+malusMove+" MOV"));
			
		}
	}
	
	public void checkAgile(){
		if((base.Skills[0].Id == 66)){
			int level = base.Skills[0].Power;
			int esquive = level * 5;
						
			this.esquiveModifyers.Add(new Modifyer(esquive, -1, 66, "Agilité", "Esquive aux compétences combat : "+esquive+"%"));
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
			else if (this.pvModifyers [i].duration > 1){
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
			this.isStateModifyed=false;
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
		return(this.state.type==4);
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
	
	public void removeLeaderEffect(){
		for (int i = attackModifyers.Count-1 ; i >= 0 ; i--)
		{
			if(attackModifyers[i].type==76){
				attackModifyers.RemoveAt(i);
			}
		}
		for (int i = pvModifyers.Count-1 ; i >= 0 ; i--)
		{
			if(pvModifyers[i].type==76){
				pvModifyers.RemoveAt(i);
			}
		}
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
	
	public int GetTotalLife()
	{
		int l = base.Life ;
		for(int i = 0 ; i < this.pvModifyers.Count ; i++){
			l+=this.pvModifyers[i].amount;
		}
		return Mathf.Max (0,l);
	}
	
	public int getBouclier(){
		int b = 0 ;
		for(int i = 0 ; i < this.bouclierModifyers.Count ; i++){
			b+=this.bouclierModifyers[i].amount;
		}
		return b;
	}
	
	public Skill GetAttackSkill()
	{
		return new Skill("Attaque", "Inflige "+this.getAttack()+" dégats au contact",0,1,100);
	}
	
	public int getDamagesAgainst(GameCard g){
		int amount = Mathf.FloorToInt(this.getAttack()*(1f-(g.getBouclier()/100f)));
		amount = Mathf.Min(g.getLife(), amount);
		return amount ;
	}
	
	public int getDamagesAgainst(GameCard g, int percentage){
		int amount = Mathf.FloorToInt(percentage*this.getAttack()*(1f-(g.getBouclier()/100f))/100f);
		amount = Mathf.Min(g.getLife(), amount);
		return amount ;
	}
	
	public int getDamagesAgainstWS(GameCard g){
		int amount = this.getAttack();
		amount = Mathf.Min(g.getLife(), amount);
		return amount ;
	}
	
	public int getEsquive(){
		int esquive = 0 ; 
		for(int i = 0 ; i < this.esquiveModifyers.Count ; i++){
			esquive+=this.esquiveModifyers[i].amount;
		}
		return esquive ;
	}
	
	public int getMagicalEsquive(){
		int esquive = 0 ; 
		for(int i = 0 ; i < this.magicalEsquiveModifyers.Count ; i++){
			esquive+=this.magicalEsquiveModifyers[i].amount;
		}
		return esquive ;
	}
	
	public List<string> getEsquiveIcon(){
		List<string> textes = new List<string>();
		for (int i = 0 ; i < this.esquiveModifyers.Count ; i++){
			textes.Add (esquiveModifyers[i].title);
			textes.Add (esquiveModifyers[i].description);
		}
		for (int i = 0 ; i < this.bouclierModifyers.Count ; i++){
			textes.Add (bouclierModifyers[i].title);
			textes.Add (bouclierModifyers[i].description);
		}
		return textes;
	}
	
	public List<string> getMoveIcon(){
		List<string> textes = new List<string>();
		for (int i = 0 ; i < this.moveModifyers.Count ; i++){
			textes.Add (moveModifyers[i].title);
			textes.Add (moveModifyers[i].description);
		}
		return textes;
	}
	
	public Skill findSkill(int j){
		Skill s = new Skill();
		for(int i = 0; i < this.Skills.Count;i++){
			if (this.Skills[i].Id==j){
				s = this.Skills[i];
			}
		}
		return s ;
	}
	
	public override string getSkillText(string s){
		int index ;
		int percentage ;
		string tempstring ;
		if (s.Contains("%ATK")){
			index = s.IndexOf("%ATK");
			
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.CeilToInt(Int32.Parse(tempstring)*this.getAttack()/100f);
			s = s.Substring(0,index-4)+" "+percentage+" "+s.Substring(index+5,s.Length-index-5);
		}
		if (s.Contains("%PV")){
			index = s.IndexOf("%PV");
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.CeilToInt(Int32.Parse(tempstring)*this.getLife()/100f);
			s = s.Substring(0,index-4)+" "+percentage+" "+s.Substring(0,index+3);
		}
		return s;
	}
	
	public void addEsquiveModifyer(Modifyer m){
		if(m.type==14){
			this.esquiveModifyers = new List<Modifyer>();
		}
		this.esquiveModifyers.Add (m);
	}
	
	public void addShieldModifyer(Modifyer m){
		if(m.type==39){
			this.bouclierModifyers = new List<Modifyer>();
		}
		this.bouclierModifyers.Add (m);
	}
	
	public void emptyModifiers()
	{
		this.pvModifyers = new List<Modifyer>();
		this.attackModifyers = new List<Modifyer>();
		this.moveModifyers = new List<Modifyer>();
		this.esquiveModifyers = new List<Modifyer>();
		this.magicalEsquiveModifyers = new List<Modifyer>();
		this.bouclierModifyers = new List<Modifyer>();
		this.isStateModifyed = false ;
		this.state = new Modifyer();
	}
	
	public bool isIntouchable()
	{
		return (this.state.type==2);
	}
}