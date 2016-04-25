using UnityEngine;
using System.Collections.Generic;

public class Combo : GameSkill
{
	public Combo(){
		this.numberOfExpectedTargets = 1 ;
		base.name = "Combo";
		base.ciblage = 1 ;
		base.auto = false;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		this.displayTargets(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}
	
	public override void resolve(List<Tile> targets)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		int target = GameView.instance.getTileCharacterID(targets[0].x, targets[0].y);
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int max = 5+GameView.instance.getCurrentSkill().Power;
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn2(target, Random.Range(1,max+1));
			}
			else{
				GameController.instance.esquive(target,base.name);
			}
		}
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}

	public override void applyOn(int target, int value){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getNormalDamagesAgainst(targetCard,value*Mathf.Max(1,Mathf.RoundToInt(20*currentCard.getAttack()/100f)));
		string text = "HIT X"+value+"\n-"+damages+"PV";

		if (currentCard.isLache() && !currentCard.hasMoved){
			damages = currentCard.getNormalDamagesAgainst(targetCard, damages+5+currentCard.getSkills()[0].Power);
			text = "HIT X"+value+"\n-"+damages+"PV\n(lâche)";			
		}

		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,12,base.name,damages+" dégats subis"), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.addAnim(GameView.instance.getTile(target), 12);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damagesMin = currentCard.getNormalDamagesAgainst(targetCard,Mathf.Max(1,Mathf.RoundToInt(20*currentCard.getAttack()/100f)));
		int damagesMax = currentCard.getNormalDamagesAgainst(targetCard,(5+GameView.instance.getCurrentSkill().Power)*Mathf.Max(1,Mathf.RoundToInt(20*currentCard.getAttack()/100f)));
		string text = "PV : "+currentCard.getLife()+" -> ["+(currentCard.getLife()-damagesMax)+"-"+(currentCard.getLife()-damagesMin)+"]";

		if (currentCard.isLache() && !currentCard.hasMoved){
			damagesMin = currentCard.getNormalDamagesAgainst(targetCard,damagesMin+5+currentCard.getSkills()[0].Power);
			damagesMax = currentCard.getNormalDamagesAgainst(targetCard,damagesMax+5+currentCard.getSkills()[0].Power);
		
			text = "PV : "+targetCard.getLife()+" -> ["+(targetCard.getLife()-damagesMax)+"-"+(targetCard.getLife()-damagesMin)+"]";	
		}
		
		int amount = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*0.2f));
		int score = 0;

		int nbHitMax = s.Power+5;
		for (int i = 1 ; i <= nbHitMax ; i++){
			if((damages*i)>=targetCard.getLife()){
				score=200;
			}
			else{
				score=Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*((damages*i)+Mathf.Max(0,30-(targetCard.getLife()-damages))));
			}
		}

		score = Mathf.RoundToInt(score/nbHitMax);
		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
