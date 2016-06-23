using UnityEngine;
using System.Collections.Generic;

public class Combo : GameSkill
{
	public Combo(){
		this.initTexts();
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Combo","Combo"});
		texts.Add(new string[]{"HIT XARG1\n-ARG2PV","HIT XARG1\n-ARG2HP"});
		texts.Add(new string[]{"HIT XARG1\n-ARG2PV\nlâche","HIT XARG1\n-ARG2HP\ncoward"});
		texts.Add(new string[]{"PV : ARG1 -> [ARG2-ARG3]","HP : ARG1 -> [ARG2-ARG3]"});
		texts.Add(new string[]{"PV : ARG1 -> [ARG2-ARG3]\nlâche","HP : ARG1 -> [ARG2-ARG3]\ncoward"});
		base.ciblage = 1 ;
		base.auto = false;
		base.id = 12 ;
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
		int damages = currentCard.getNormalDamagesAgainst(targetCard,value*Mathf.Max(1,Mathf.RoundToInt(20*currentCard.getAttack()/100f)));
		string text = this.getText(1, new List<int>{value,damages});

		if (currentCard.isLache() && !currentCard.hasMoved){
			damages = currentCard.getNormalDamagesAgainst(targetCard, damages+5+currentCard.getSkills()[0].Power);
			text = this.getText(2, new List<int>{value,damages});
		}

		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,12,this.getText(0),""), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.addAnim(3,GameView.instance.getTile(target));
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damagesMin = currentCard.getNormalDamagesAgainst(targetCard,Mathf.Max(1,Mathf.RoundToInt(20*currentCard.getAttack()/100f)));
		int damagesMax = currentCard.getNormalDamagesAgainst(targetCard,(5+GameView.instance.getCurrentSkill().Power)*Mathf.Max(1,Mathf.RoundToInt(20*currentCard.getAttack()/100f)));
		string text = this.getText(3,new List<int>{currentCard.getLife(),(currentCard.getLife()-damagesMax),(currentCard.getLife()-damagesMin)});

		if (currentCard.isLache() && !currentCard.hasMoved){
			damagesMin = currentCard.getNormalDamagesAgainst(targetCard,damagesMin+5+currentCard.getSkills()[0].Power);
			damagesMax = currentCard.getNormalDamagesAgainst(targetCard,damagesMax+5+currentCard.getSkills()[0].Power);
		
			text = this.getText(4,new List<int>{currentCard.getLife(),(currentCard.getLife()-damagesMax),(currentCard.getLife()-damagesMin)});	
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
		SoundController.instance.playSound(25);
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
				score=Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*(200)+targetCard.getLife()/10f);
			}
			else{
				score=Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*((damages*i)+5-targetCard.getLife()/10f));
			}
		}

		score = Mathf.RoundToInt(score/nbHitMax);
		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
