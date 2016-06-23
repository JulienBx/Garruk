using UnityEngine;
using System.Collections.Generic;

public class Laser : GameSkill
{
	public Laser()
	{
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Laser","Laser Beam"});
		texts.Add(new string[]{"-ARG1 PV","-ARG1 HP"});
		texts.Add(new string[]{"PV : ARG1 -> [ARG2-ARG3]","HP : ARG1 -> [ARG2-ARG3]"});
		texts.Add(new string[]{"Fou","Crazy"});
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
				GameController.instance.applyOn2(target, Random.Range(5+level, 15+level));
			}
			else{
				GameController.instance.esquive(target,this.getText(0));
			}
		}
		GameController.instance.playSound(35);

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
		string text = this.getText(0)+"\n"+this.getText(1, new List<int>{damages});
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 22, this.getText(0), ""), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.displaySkillEffect(target, text, 0);	
		GameView.instance.addAnim(6,GameView.instance.getTile(target));
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int minDamages = 5 + level;
		int maxDamages = 15 + level;
		if(currentCard.isFou()){
			minDamages = Mathf.RoundToInt(1.25f*minDamages);
			maxDamages = Mathf.RoundToInt(1.25f*maxDamages);
		}
		minDamages = currentCard.getNormalDamagesAgainst(targetCard,minDamages);
		maxDamages = currentCard.getNormalDamagesAgainst(targetCard,maxDamages);

		string text = this.getText(2, new List<int>{targetCard.getLife(),(targetCard.getLife()-minDamages),(targetCard.getLife()-maxDamages)});
		
		int amount = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		if(value==1){
			int myLevel = GameView.instance.getCurrentCard().Skills[0].Power;
			GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).addDamagesModifyer(new Modifyer((11-myLevel), -1, 24, this.getText(0), ""), true,-1);
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0)+"\n"+this.getText(3)+"\n"+this.getText(1, new List<int>{11-myLevel}), 0);
		}
		else{
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0), 1);
		}
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		int score = 0 ;
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = WordingSkills.getProba(s.Id,s.Power);

		int levelMin = Mathf.FloorToInt((5+s.Power)*(1f+currentCard.getBonus(targetCard)/100f)*(1f-(targetCard.getBouclier()/100f)))+Mathf.RoundToInt(5-targetCard.getLife()/10f);;
		int levelMax = Mathf.FloorToInt((15+s.Power)*(1f+currentCard.getBonus(targetCard)/100f)*(1f-(targetCard.getBouclier()/100f)))+Mathf.RoundToInt(5-targetCard.getLife()/10f);;

		score+=Mathf.RoundToInt(((proba-targetCard.getMagicalEsquive())/100f)*((200*(Mathf.Max(0f,1+levelMax-targetCard.getLife())))+(((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)*(Mathf.Min(levelMax,targetCard.getLife())-levelMin)))/(levelMax-levelMin+1f));

		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
