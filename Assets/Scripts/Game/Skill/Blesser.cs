using UnityEngine;
using System.Collections.Generic;

public class Blesser : GameSkill
{
	public Blesser(){
		this.numberOfExpectedTargets = 1 ; 
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Blesser","Cripple"});
		texts.Add(new string[]{"-ARG1 PV\n-ARG2 ATK\npour 1 tour","-ARG1 HP\n-ARG2 ATK\nfor 1 turn"});
		texts.Add(new string[]{"-ARG1 PV\n-ARG2 ATK\npour 1 tour","-ARG1 HP\n-ARG2 ATK\nfor 1 turn\ncoward"});
		texts.Add(new string[]{". Actif 1 tour",". For 1 turn"});
		texts.Add(new string[]{"PV : ARG1 -> ARG2\nATK : ARG3 -> [ARG4-ARG5]\nActif 1 tour", "HP : ARG1 -> ARG2\nATK : ARG3 -> [ARG4-ARG5]\nFor 1 turn"});
		texts.Add(new string[]{"PV : ARG1 -> ARG2\nATK : ARG3 -> [ARG4-ARG5]\nActif 1 tour", "HP : ARG1 -> ARG2\nATK : ARG3 -> [ARG4-ARG5]\nFor 1 turn\nCoward"});

		base.ciblage = 1 ; 

		base.auto = false;
		base.id = 11 ;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		this.displayTargets();
	}
	
	public override void resolve(List<Tile> targets)
	{	
		GameController.instance.play(this.id);
		int target = GameView.instance.getTileCharacterID(targets[0].x, targets[0].y);
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int minMalus = GameView.instance.getCurrentSkill().Power ;
		int maxMalus = GameView.instance.getCurrentSkill().Power+5;

		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn2(target, Random.Range(minMalus, maxMalus+1));
			}
			else{
				GameController.instance.esquive(target,this.getText(0));
			}
		}
		GameController.instance.playSound(25);
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int value){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*0.5f));
		int malus = Mathf.Min(targetCard.getAttack()-1, value);
		string text = this.getText(1, new List<int>{damages,value});				
		if (currentCard.isLache() && !currentCard.hasMoved){
			damages = currentCard.getNormalDamagesAgainst(targetCard, damages+5+currentCard.getSkills()[0].Power);
			text = this.getText(2, new List<int>{damages,value});
		}
		GameView.instance.getPlayingCardController(target).updateAttack(targetCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(-1*malus, 1, 11, this.getText(0), this.getText(3)));
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,11,this.getText(0),""), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.addAnim(3,GameView.instance.getTile(target));
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*0.5f));
		int minMalus = GameView.instance.getCurrentSkill().Power ;
		int maxMalus = GameView.instance.getCurrentSkill().Power+5;
		string text = this.getText(4, new List<int>{targetCard.getLife(),targetCard.getLife()-damages, targetCard.getAttack(), Mathf.Max(1, targetCard.getAttack()-minMalus),Mathf.Max(1, targetCard.getAttack()-maxMalus)});				

		if (currentCard.isLache() && !currentCard.hasMoved){
			damages = currentCard.getNormalDamagesAgainst(targetCard, damages+5+currentCard.getSkills()[0].Power);
			text = this.getText(5, new List<int>{targetCard.getLife(),targetCard.getLife()-damages, targetCard.getAttack(), Mathf.Max(1, targetCard.getAttack()-minMalus),Mathf.Max(1, targetCard.getAttack()-maxMalus)});			
		}

		int amount = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0), 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*0.5f));
		int score ;
		if(damages>=targetCard.getLife()){
			score=200;
		}
		else{
			score=Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*(damages+5-targetCard.getLife()/10f));
		}

		int levelMin = s.Power;
		int levelMax = 5+s.Power;

		score+=Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*(targetCard.getLife()/50f)*Mathf.Min(targetCard.getAttack(),((levelMin+levelMax)/2)));

		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
