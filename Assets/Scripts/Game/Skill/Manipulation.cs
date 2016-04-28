using UnityEngine;
using System.Collections.Generic;

public class Manipulation : GameSkill
{
	public Manipulation()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Manipulation";
		base.ciblage = 4 ;
		base.auto = false;
		base.id = 52 ;
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
		int level = GameView.instance.getCurrentSkill().Power;
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getMagicalEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn2(target, Random.Range(1, 3+2*level+1));
			}
			else{
				GameController.instance.esquive(target,base.name);
			}
		}
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int level){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);

		GameView.instance.changePlayer(target);

		GameView.instance.displaySkillEffect(target, "Contrôlé", 1);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 54);
	}	

	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);

		string text = "Prend le contrôle de l'unité à ce tour";
		
		int amount = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}

	public override int getActionScore(Tile t, Skill s){
		int score = 2*s.Power ;
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
		GameCard currentCard = GameView.instance.getCurrentCard();

		if(targetCard.CardType.Id==2 || targetCard.CardType.Id==1 || targetCard.CardType.Id==3 || targetCard.CardType.Id==5 || targetCard.CardType.Id==9 || targetCard.CardType.Id==8){
			score+=5;
		}

		return score ;
	}
}
