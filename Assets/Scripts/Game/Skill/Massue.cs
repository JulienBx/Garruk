using UnityEngine;
using System.Collections.Generic;

public class Massue : GameSkill
{
	public Massue()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Massue";
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
		GameCard currentCard = GameView.instance.getCurrentCard();
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn2(target, Random.Range(Mathf.RoundToInt(currentCard.getAttack()*50f/100f),Mathf.RoundToInt(currentCard.getAttack()*(120f+10f*GameView.instance.getCurrentCard().Skills[0].Power)/100f)));
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
		int damages = currentCard.getNormalDamagesAgainst(targetCard, value);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,63,base.name,damages+" dégats subis"), false);
		GameView.instance.displaySkillEffect(target, "-"+damages+"PV", 0);
		GameView.instance.addAnim(GameView.instance.getTile(target), 63);
	}

	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damagesMin = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*0.5f)); ;
		int damagesMax = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*(120f+10f*GameView.instance.getCurrentCard().Skills[0].Power)/100f));
		string text = "PV : "+targetCard.getLife()+" -> ["+(targetCard.getLife()-damagesMin)+"-"+(targetCard.getLife()-damagesMax)+"]";
		
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}
}
