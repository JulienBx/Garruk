using UnityEngine;
using System.Collections.Generic;

public class Synergie : GameSkill
{
	public Synergie(){
		this.initTexts();
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Synergie","Synergy"});
		texts.Add(new string[]{"-ARG1 PV","-ARG1 HP"});
		texts.Add(new string[]{"PV : ARG1 -> ARG2","HP : ARG1 -> ARG2"});
		base.ciblage = 15 ;
		base.auto = false;
		base.id = 133 ;
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

		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn(target);
			}
			else{
				GameController.instance.esquive(target,this.getText(0));
			}
		}
		GameController.instance.applyOnMe(-1);
		GameController.instance.playSound(25);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		string text = this.getText(0);
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getNormalDamagesAgainst(targetCard,Mathf.RoundToInt(currentCard.getAttack()*(0.2f+level/20f)));

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 133, "", ""), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.displaySkillEffect(target, this.getText(1, new List<int>{damages}), 0);
		GameView.instance.addAnim(3,GameView.instance.getTile(target));
	}

	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getNormalDamagesAgainst(targetCard,Mathf.RoundToInt(currentCard.getAttack()*(0.2f+level/20f)));
		string text = this.getText(2, new List<int>{targetCard.getLife(),(targetCard.getLife()-damages)});
		
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
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

		int damages = currentCard.getNormalDamagesAgainst(targetCard,Mathf.RoundToInt(currentCard.getAttack()*(0.2f+s.Power/20f)));
		int score ;
		if(damages>=targetCard.getLife()){
			score=200;
		}
		else{
			score=Mathf.RoundToInt((proba-targetCard.getEsquive()/100f)*damages);
		}

		score = score * GameView.instance.IA.getAgressiveFactor() ;

		return score ;
	}
}
