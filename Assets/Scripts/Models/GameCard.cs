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
	public List<Modifyer> bonusModifyers = new List<Modifyer>();
	public List<Modifyer> states ;

	public bool isStateModifyed;
	public int nbTurnsToWait ;
	public bool isDead;
	public bool hasMoved;
	public bool hasPlayed;

	public GameCard(){
	
	}

	public GameCard(int atk, int pv, string title, int move, int deckorder, int powerlevel){
		base.Attack = atk;
		base.Life = pv;
		base.Title = title;
		base.Move = move;
		base.deckOrder = deckorder;
		base.PowerLevel = powerlevel;
		this.states = new List<Modifyer>();
		this.isStateModifyed = false;
	}

	public GameCard(Card c){
		base.CardType = new CardType();
		base.Title = c.Title; 									    
		base.ArtIndex = c.ArtIndex;
		base.Life = c.Life;
		base.Speed = c.Speed;
		base.Move = c.Move;
		base.Attack = c.Attack;
		base.Skills = c.Skills;
		base.CardType.Id = c.CardType.Id;
		base.TitleClass = c.TitleClass;
		base.AttackLevel = c.AttackLevel;   
		base.MoveLevel = c.MoveLevel;
		base.LifeLevel = c.LifeLevel;
		base.SpeedLevel = c.SpeedLevel;
		base.deckOrder = c.deckOrder;
		base.PowerLevel = c.PowerLevel;
		this.isDead=false;
		this.states = new List<Modifyer>();
		this.isStateModifyed = false;
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

	public bool isMutant()
	{
		return (base.Skills[0].Id == 138);
	}

	public bool isGolem()
	{
		return (base.Skills[0].Id == 141);
	}
	
	public bool isLeader()
	{
		return (base.Skills[0].Id == 76);
	}
	
	public bool isLache()
	{
		return (base.Skills[0].Id == 65);
	}

	public bool isNinja()
	{
		return (base.Skills[0].Id == 67);
	}

	public bool isSanguinaire()
	{
		return (base.Skills[0].Id == 34);
	}

	public bool isSniper()
	{
		return (base.Skills[0].Id == 35);
	}

	public bool isSniperActive()
	{
		bool hasFound = false ;
		for (int i = moveModifyers.Count-1 ; i >= 0 ; i--)
		{
			if(moveModifyers[i].type==35){
				hasFound = true;
			}
		}
		return hasFound ;
	}
	
	public bool isNurse()
	{
		return (base.Skills[0].Id == 75);
	}

	public bool isFou()
	{
		return (base.Skills[0].Id == 33);
	}
	
	public bool isFrenetique()
	{
		return (base.Skills[0].Id == 69);
	}	
	
	public bool isVirologue()
	{
		return (this.Skills[0].Id == 72);
	}

	public bool isCristoMaster()
	{
		return (this.Skills[0].Id == 139);
	}

	public bool isCreator()
	{
		return (this.Skills[0].Id == 140);
	}

	public void replaceCristoMasterModifyer(Modifyer m)
	{
		for (int i = attackModifyers.Count-1 ; i >= 0 ; i--)
		{
			if(attackModifyers[i].type==139){
				attackModifyers.RemoveAt(i);
			}
		}
		this.attackModifyers.Add(m);
	}
	
	public void checkModifyers(){
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

		for (int i = this.magicalEsquiveModifyers.Count-1 ; i >=0 ; i--){
			if (this.magicalEsquiveModifyers [i].duration > 2){
				this.magicalEsquiveModifyers [i].description.Replace(this.magicalEsquiveModifyers [i].duration+" tours", (this.magicalEsquiveModifyers [i].duration-1)+" tours");
			}
			else if (this.magicalEsquiveModifyers [i].duration > 1){
				this.magicalEsquiveModifyers [i].description.Replace(this.magicalEsquiveModifyers [i].duration+" tours", "1 tour");
			}
			
			if (this.magicalEsquiveModifyers [i].duration > 0)
			{
				this.magicalEsquiveModifyers[i].duration--;
			}
			else if (this.magicalEsquiveModifyers [i].duration == -2){
				this.magicalEsquiveModifyers [i].duration = 1;
			}
			
			if (this.magicalEsquiveModifyers [i].duration == 0)
			{
				this.magicalEsquiveModifyers.RemoveAt(i);
			}
		}

		for (int i = this.states.Count-1 ; i >=0 ; i--){
			if (this.states [i].duration > 2){
				this.states [i].description.Replace(this.states [i].duration+" tours", (this.states [i].duration-1)+" tours");
			}
			else if (this.states [i].duration > 1){
				this.states [i].description.Replace(this.states [i].duration+" tours", "1 tour");
			}
			
			if (this.states [i].duration > 0)
			{
				this.states[i].duration--;
			}
			else if (this.states [i].duration == -2){
				this.states [i].duration = 1;
			}
			
			if (this.states [i].duration == 0)
			{
				this.states.RemoveAt(i);
			}
		}
	}

	public List<Modifyer> getEffects(){
		List<Modifyer> effects = new List<Modifyer>();

		for(int i = 0 ; i < states.Count;i++){
			effects.Add(states[i]);
		}
		for(int i = 0 ; i < attackModifyers.Count;i++){
			effects.Add(new Modifyer(attackModifyers[i].amount, -1, 1, attackModifyers[i].title, attackModifyers[i].description));
		}
		for(int i = 0 ; i < pvModifyers.Count;i++){
			effects.Add(new Modifyer(pvModifyers[i].amount, -1, 0, pvModifyers[i].title, pvModifyers[i].description));
		}
		for(int i = 0 ; i < moveModifyers.Count;i++){
			effects.Add(new Modifyer(moveModifyers[i].amount, -1, 2, moveModifyers[i].title, moveModifyers[i].description));
		}
		for(int i = 0 ; i < esquiveModifyers.Count;i++){
			effects.Add(new Modifyer(esquiveModifyers[i].amount, -1, 3, esquiveModifyers[i].title, esquiveModifyers[i].description));
		}
		for(int i = 0 ; i < magicalEsquiveModifyers.Count;i++){
			effects.Add(new Modifyer(magicalEsquiveModifyers[i].amount, -1, 4, magicalEsquiveModifyers[i].title, magicalEsquiveModifyers[i].description));
		}
		for(int i = 0 ; i < bouclierModifyers.Count;i++){
			effects.Add(new Modifyer(bouclierModifyers[i].amount, -1, 5, bouclierModifyers[i].title, bouclierModifyers[i].description));
		}
		return effects ;
	}

	public List<Modifyer> getIconAttack()
	{
		return attackModifyers;
	}

	public List<Modifyer> getIconLife()
	{
		return pvModifyers;
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
				if(this.getLife()-pvModifyers[i].amount<1){
					pvModifyers[i].amount = this.getLife()-1;
				}
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

	public int getBonus(GameCard g){
		int b = 0 ;
		for(int i = 0 ; i < this.bonusModifyers.Count ; i++){
			if(this.bonusModifyers[i].targetType!=-1){
				if(this.bonusModifyers[i].targetType==g.CardType.Id){
					b+=this.bonusModifyers[i].amount;
				}
			}
			else{
				b+=this.bonusModifyers[i].amount;
			}
		}
		return b;
	}
	
	public Skill GetAttackSkill()
	{
		return new Skill("Attaque", "Inflige "+this.getAttack()+" dégats au contact",0,1,100,1);
	}
	
	public int getNormalDamagesAgainst(GameCard g, int attack){
		int amount = Mathf.FloorToInt(attack*(1f+this.getBonus(g)/100f)*(1f-(g.getBouclier()/100f)));
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
		for (int i = 0 ; i < this.magicalEsquiveModifyers.Count ; i++){
			textes.Add (magicalEsquiveModifyers[i].title);
			textes.Add (magicalEsquiveModifyers[i].description);
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
			percentage = Mathf.Max(1,Mathf.RoundToInt(Int32.Parse(tempstring)*this.getAttack()/100f));
			s = s.Substring(0,index-3)+percentage+s.Substring(index+4,s.Length-index-4);
		}
		if (s.Contains("%ATK")){
			index = s.IndexOf("%ATK");
			
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.Max(1, Mathf.RoundToInt(Int32.Parse(tempstring)*this.getAttack()/100f));
			s = s.Substring(0,index-3)+percentage+s.Substring(index+4,s.Length-index-4);
		}
		if (s.Contains("%PV")){
			index = s.IndexOf("%PV");
			tempstring = s.Substring(index-3,3);
			percentage = Mathf.Max(1,Mathf.RoundToInt(Int32.Parse(tempstring)*this.getLife()/100f));
			s = s.Substring(0,index-4)+" "+percentage+" "+s.Substring(index+3,s.Length-index-3);
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
		this.states = new List<Modifyer>();
		GameView.instance.getMyHoveredCardController().updateCharacter();
		GameView.instance.getHisHoveredCardController().updateCharacter();
	}

	public void emptyDamageModifyers(){
		this.damagesModifyers = new List<Modifyer>();
		GameView.instance.getMyHoveredCardController().updateCharacter();
		GameView.instance.getHisHoveredCardController().updateCharacter();
	}

	public void emptyShieldModifyers(){
		this.bouclierModifyers = new List<Modifyer>();
		GameView.instance.getMyHoveredCardController().updateCharacter();
		GameView.instance.getHisHoveredCardController().updateCharacter();
	}

	public bool isPoisoned()
	{
		bool hasFound = false ;
		for (int i = 0 ; i < states.Count && !hasFound ; i++){
			if(states[i].type==58 || states[i].type==94){
				hasFound = true ;
			}
		}
		return (hasFound);
	}

	public bool isFurious()
	{
		bool hasFound = false ;
		for (int i = 0 ; i < states.Count && !hasFound ; i++){
			if(states[i].type==93){
				hasFound = true ;
			}
		}
		return (hasFound);
	}

	public int getPoisonAmount(){
		int amount = -1 ;
		for (int i = 0 ; i < states.Count ; i++){
			if(states[i].type==58 || states[i].type==94){
				amount = states[i].amount ;
			}
		}

		return amount ;
	}

	public void setFurious(Modifyer m){
		for (int i = this.states.Count-1 ; i >= 0 ; i--){
			if(this.states[i].type==93){
				this.states.RemoveAt(i);
			}
		}
		this.states.Add(m);
		GameView.instance.getMyHoveredCardController().updateCharacter();
		GameView.instance.getHisHoveredCardController().updateCharacter();
	}

	public void setPoison(Modifyer m){
		for (int i = this.states.Count-1 ; i >= 0 ; i--){
			if(states[i].type==58 || states[i].type==94){
				this.states.RemoveAt(i);
			}
		}
		this.states.Add(m);
		GameView.instance.getMyHoveredCardController().updateCharacter();
		GameView.instance.getHisHoveredCardController().updateCharacter();
	}

	public void setTerreur(Modifyer m){
		for (int i = this.states.Count-1 ; i >= 0 ; i--){
			if(states[i].type==20){
				this.states.RemoveAt(i);
			}
		}
		this.states.Add(m);
		GameView.instance.getMyHoveredCardController().updateCharacter();
		GameView.instance.getHisHoveredCardController().updateCharacter();
	}

	public void setFatality(Modifyer m){
		for (int i = this.states.Count-1 ; i >= 0 ; i--){
			if(this.states[i].type==101){
				this.states.RemoveAt(i);
			}
		}
		this.states.Add(m);
		GameView.instance.getMyHoveredCardController().updateCharacter();
		GameView.instance.getHisHoveredCardController().updateCharacter();
	}

	public bool isEffraye()
	{
		bool hasFound = false ;
		for (int i = 0 ; i < states.Count && !hasFound ; i++){
			if(states[i].type==20){
				hasFound = true ;
			}
		}
		return (hasFound);
	}
}