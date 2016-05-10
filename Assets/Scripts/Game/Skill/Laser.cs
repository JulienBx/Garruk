using UnityEngine;
using System.Collections.Generic;

public class Laser : GameSkill
{
	public Laser()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Laser";
		base.ciblage = 3 ;
		base.auto = false;
		base.id = 22 ;
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
		int level = GameView.instance.getCurrentSkill().Power;
	
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getMagicalEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn2(target, Random.Range(10+level, 20+level));
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
	
	public override void applyOn(int target, int value){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		if(currentCard.isFou()){
			value = Mathf.RoundToInt(1.25f*value);
		}
		int damages = currentCard.getNormalDamagesAgainst(targetCard, value);
		string text = base.name+"\n-"+damages+"PV";
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 22, base.name, damages+" dégats subis"), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.displaySkillEffect(target, text, 0);	
		GameView.instance.addAnim(6,GameView.instance.getTile(target));
		SoundController.instance.playSound(35);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int minDamages = 10 + level;
		int maxDamages = 20 + level;
		if(currentCard.isFou()){
			minDamages = Mathf.RoundToInt(1.25f*minDamages);
			maxDamages = Mathf.RoundToInt(1.25f*maxDamages);
		}
		minDamages = currentCard.getNormalDamagesAgainst(targetCard,minDamages);
		maxDamages = currentCard.getNormalDamagesAgainst(targetCard,maxDamages);

		string text = "PV : "+targetCard.getLife()+" -> ["+(targetCard.getLife()-minDamages)+"-"+(targetCard.getLife()-maxDamages)+"]";
		
		int amount = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int probaEsquive = targetCard.getMagicalEsquive();
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
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		int score = 0 ;
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = WordingSkills.getProba(s.Id,s.Power);

		int levelMin = Mathf.FloorToInt((10+s.Power)*(1f+currentCard.getBonus(targetCard)/100f)*(1f-(targetCard.getBouclier()/100f)));
		int levelMax = Mathf.FloorToInt((20+s.Power)*(1f+currentCard.getBonus(targetCard)/100f)*(1f-(targetCard.getBouclier()/100f)));

		score+=Mathf.RoundToInt(((proba-targetCard.getMagicalEsquive())/100f)*((200*(Mathf.Max(0f,1+levelMax-targetCard.getLife())))+(((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)*(Mathf.Min(levelMax,targetCard.getLife())-levelMin)))/(levelMax-levelMin+1f));

		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
