using UnityEngine;
using System.Collections.Generic;

public class GrosCalibre : GameSkill
{
	public GrosCalibre()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Gros Calibre";
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
		GameCard currentCard = GameView.instance.getCurrentCard();
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
				GameController.instance.esquive(target,base.name);
			}
		}
		if(currentCard.isFou()){
			GameController.instance.applyOnMe(1);
		}
		else{
			GameController.instance.applyOnMe(-1);
		}

		GameController.instance.endPlay();
	}

	public override void applyOn(int target){
		int level = GameView.instance.getCurrentSkill().Power;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int maxDamages = Mathf.RoundToInt(currentCard.getAttack()*(1.2f+level/10f));
		if(currentCard.isFou()){
			maxDamages = Mathf.RoundToInt(1.25f*maxDamages);
		}
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(maxDamages));

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 26, base.name, damages+" dégats subis"), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.displaySkillEffect(target, "-"+damages+"PV", 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 26);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int maxDamages = Mathf.RoundToInt(currentCard.getAttack()*(1.2f+level/10f));
		if(currentCard.isFou()){
			maxDamages = Mathf.RoundToInt(1.25f*maxDamages);
		}
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(maxDamages));
		string text = "-"+damages+"PV";		
		
		int amount = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		if(value==1){
			int myLevel = GameView.instance.getCurrentCard().Skills[0].Power;
			GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).addDamagesModifyer(new Modifyer((11-myLevel), -1, 24, base.name, (10-myLevel)+" dégats subis"), false,-1);
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name+"\nFou\n-"+(11-myLevel)+"PV", 0);
		}
		else{
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		}
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}

	public override int getActionScore(Tile t, Skill s){
		int score = 0 ;
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
		GameCard currentCard = GameView.instance.getCurrentCard();

		int maxDamages = Mathf.RoundToInt(currentCard.getAttack()*(1.2f+s.Power/10f));
		if(currentCard.isFou()){
			maxDamages = Mathf.RoundToInt(1.25f*maxDamages);
		}
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(maxDamages));

		if(damages>=targetCard.getLife()){
			score+=200;
		}
		else{
			score+=Mathf.RoundToInt(((100-targetCard.getEsquive())/100f)*(damages+Mathf.Max(0,30-(targetCard.getLife()-damages))));
		}
					
		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
