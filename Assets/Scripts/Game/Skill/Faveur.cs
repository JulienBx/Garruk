using UnityEngine;
using System.Collections.Generic;

public class Faveur : GameSkill
{
	public Faveur()
	{
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Faveur","Favor"});
		texts.Add(new string[]{"Prochaine unité à jouer","Next unit to play"});
		base.ciblage = 4 ;
		base.auto = false;
		base.id = 104 ;
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
				GameController.instance.applyOn(target);
			}
			else{
				GameController.instance.esquive(target,this.getText(0));
			}
		}
		GameController.instance.applyOnMe(-1);
		GameController.instance.playSound(28);

		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		GameView.instance.advanceTurn(target);
		GameView.instance.displaySkillEffect(target, this.getText(1), 2);	
		GameView.instance.addAnim(0,GameView.instance.getTile(target));
	}	

	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);

		string text = this.getText(1);
		
		int amount = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0), 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		int score = 0 ;
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = WordingSkills.getProba(s.Id,s.Power);

		if(targetCard.CardType.Id==3){
			score = 10 ; 
		}
		else{
			score+=Mathf.RoundToInt(((proba-targetCard.getMagicalEsquive())/100f)*(targetCard.getAttack()-15f));
		}
				
		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
