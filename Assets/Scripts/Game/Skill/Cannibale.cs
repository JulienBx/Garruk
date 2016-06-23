using UnityEngine;
using System.Collections.Generic;

public class Cannibale : GameSkill
{
	public Cannibale(){
		this.initTexts();
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Cannibale","Cannibalism"});
		texts.Add(new string[]{"Dévoré!","Eaten!"});
		texts.Add(new string[]{"Absorbe ARG1 ATK et ARG2 PV","Absorbs ARG1 ATK and ARG2 HP"});
		texts.Add(new string[]{"+ARG1 PV","+ARG1 HP"});
		texts.Add(new string[]{"+ARG1 ATK","+ARG1 ATK"});
		texts.Add(new string[]{". Permanent",". Permanent"});

		base.ciblage = 2 ;
		base.auto = false;
		base.id = 21 ;
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
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive()){
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

		GameController.instance.playSound(37);
		GameController.instance.applyOnMe(target);
		GameController.instance.endPlay();
	}

	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = targetCard.getLife();

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 21, this.getText(0), ""), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.displaySkillEffect(target, this.getText(1), 0);
		GameView.instance.addAnim(4,GameView.instance.getTile(target));
	}

	public override string getTargetText(int target){
		string text = this.getText(0);
		GameCard targetCard = GameView.instance.getCard(target);
		int damages = targetCard.getLife();
		int percentage = 20+GameView.instance.getCurrentSkill().Power*5;

		int bonusLife = Mathf.RoundToInt(damages*percentage/100f);
		int bonusAttack = Mathf.RoundToInt(targetCard.getAttack()*percentage/100f);

		text += "\n"+this.getText(2,new List<int>{bonusAttack,bonusLife});
		
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		GameCard targetCard = GameView.instance.getCard(value);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = targetCard.getLife();
		int percentage = 20+GameView.instance.getCurrentSkill().Power*5;
		int bonusLife = Mathf.RoundToInt(damages*percentage/100f);
		int bonusAttack = Mathf.RoundToInt(targetCard.getAttack()*percentage/100f);

		string text = "";

		text+=this.getText(3, new List<int>{bonusLife})+"\n";
		text+=this.getText(4, new List<int>{bonusAttack});
		int targetMe = GameView.instance.getCurrentPlayingCard();

		GameView.instance.getPlayingCardController(targetMe).updateAttack(currentCard.getAttack());
		GameView.instance.getPlayingCardController(targetMe).updateLife(currentCard.getLife());
		GameView.instance.getPlayingCardController(targetMe).addAttackModifyer(new Modifyer(bonusAttack, -1, 21, this.getText(0), this.getText(5)));
		GameView.instance.getPlayingCardController(targetMe).addPVModifyer(new Modifyer(bonusLife, -1, 21, this.getText(0), this.getText(5)));
		GameView.instance.displaySkillEffect(targetMe, text, 2);
		GameView.instance.addAnim(7,GameView.instance.getTile(targetMe));

	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));

		int score = -10-targetCard.getLife()+Mathf.RoundToInt(2*(0.2f+0.05f*s.Power)*targetCard.getAttack()+(0.2f+0.05f*s.Power)*targetCard.getLife());
		score = score * GameView.instance.IA.getSoutienFactor() ;

		return score ;
	}
}
