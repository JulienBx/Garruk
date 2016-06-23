using UnityEngine;
using System.Collections.Generic;

public class Fatality : GameSkill
{
	public Fatality()
	{
		this.initTexts();
		this.numberOfExpectedTargets = 1 ; 
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Fatalité","Fatality"});
		texts.Add(new string[]{"Meurt au prochain tour","Will die at the end of next turn"});
		texts.Add(new string[]{"1 cristal créé","Creation of 1 cristal"});
		texts.Add(new string[]{"échec","fail"});
		base.ciblage = 3 ;
		base.auto = false;
		base.id = 101 ;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		this.displayTargets();
	}
	
	public override void resolve(List<Tile> targets)
	{	
		GameController.instance.play(this.id);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int target = GameView.instance.getTileCharacterID(targets[0].x, targets[0].y);
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);

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
		GameController.instance.playSound(30);

		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();

		GameView.instance.getCard(target).setFatality(new Modifyer(0, 1, 101, this.getText(0), this.getText(1)));
		GameView.instance.getPlayingCardController(target).showIcons();

		GameView.instance.displaySkillEffect(target, this.getText(1), 0);	
		GameView.instance.addAnim(4,GameView.instance.getTile(target));
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();

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

		score+=Mathf.RoundToInt((60f*(proba-targetCard.getMagicalEsquive())/100f)*(targetCard.getLife()/50f));
				
		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
