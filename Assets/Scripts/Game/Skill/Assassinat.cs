using UnityEngine;
using System.Collections.Generic;

public class Assassinat : GameSkill
{
	public Assassinat(){
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Assassinat","Slaying"});
		texts.Add(new string[]{"Cible détruite!","Target destroyed!"});
		base.auto = false;
		base.id = 10 ;
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
		GameController.instance.playSound(25);

		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}

	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		int damages = targetCard.getLife();
		
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 10, this.getText(0), ""), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.displaySkillEffect(target, this.getText(1), 0);
		GameView.instance.addAnim(3,GameView.instance.getTile(target));
	}
	
	public override string getTargetText(int id){	
		GameCard targetCard = GameView.instance.getCard(id);
		string text = this.getText(1);

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
		int score = 0 ;
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = WordingSkills.getProba(s.Id,s.Power);

		score+=Mathf.RoundToInt((100f*(proba-targetCard.getEsquive())/100f)+targetCard.getLife()/10f);
				
		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
