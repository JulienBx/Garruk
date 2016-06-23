using UnityEngine;
using System.Collections.Generic;

public class Terreur : GameSkill
{
	public Terreur(){
		this.initTexts();
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Terreur","Terror"});
		texts.Add(new string[]{"-ARG1 PV","-ARG1 HP"});
		texts.Add(new string[]{"Effrayé!","Scared!"});
		texts.Add(new string[]{"Inactif au prochain tour","Can not use skills during next turn"});
		texts.Add(new string[]{"PV : ARG1 -> ARG2\n25% de chances de paralyser","HP : ARG1 -> ARG2\n25% chances of paralyzing"});

		base.ciblage = 1 ; 
		base.auto = false;
		base.id = 20 ;
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
				int result = -1 ;
				if (Random.Range(1,101)<26){
					result = 1;
				}
				else{
					result = 0;
				}
				GameController.instance.applyOn2(target,result);
			}
			else{
				GameController.instance.esquive(target,this.getText(0));
			}
		}
		GameController.instance.applyOnMe(-1);
		GameController.instance.playSound(25);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int result){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;

		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*(0.5f+0.05f*level)));

		string text = this.getText(1, new List<int>{damages});				
		if(result==1){
			text+="\n"+this.getText(2);
			GameView.instance.getCard(target).setTerreur(new Modifyer(0, 1, 20, this.getText(0), this.getText(3)));
			GameView.instance.getPlayingCardController(target).showIcons();
		}

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,11,this.getText(0),""), false, GameView.instance.getCurrentPlayingCard());

		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.addAnim(3,GameView.instance.getTile(target));
	}
		
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*(0.5f+0.05f*level)));

		string text = this.getText(4, new List<int>{targetCard.getLife(),(targetCard.getLife()-damages)});				
		
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
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));

		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*(0.5f+0.05f*s.Power)));
		if(damages>=targetCard.getLife()){
			score+=200;
		}
		else{
			score+=Mathf.RoundToInt(((100-targetCard.getEsquive())/100f)*(15+damages+Mathf.Max(0,30-(targetCard.getLife()-damages))));
		}

		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
