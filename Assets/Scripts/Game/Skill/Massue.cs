using UnityEngine;
using System.Collections.Generic;

public class Massue : GameSkill
{
	public Massue()
	{
		this.initTexts();
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Massue","Hammer"});
		texts.Add(new string[]{"-ARG1 PV","-ARG1 HP"});
		texts.Add(new string[]{"PV : ARG1 -> [ARG2-ARG3]","HP : ARG1 -> [ARG2-ARG3]"});
		base.ciblage = 1 ;
		base.auto = false;
		base.id = 63 ;
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
		int damages = currentCard.getNormalDamagesAgainst(targetCard, value);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,63,this.getText(0),""), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.displaySkillEffect(target, this.getText(1, new List<int>{damages}), 0);
		GameView.instance.addAnim(3,GameView.instance.getTile(target));
	}

	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damagesMin = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*0.5f)); ;
		int damagesMax = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*(120f+10f*GameView.instance.getCurrentCard().Skills[0].Power)/100f));
		string text = this.getText(2, new List<int>{targetCard.getLife(),(targetCard.getLife()-damagesMin),(targetCard.getLife()-damagesMax)});
		
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
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

		int levelMin = Mathf.FloorToInt((Mathf.RoundToInt(targetCard.getAttack()*0.5f))*(1f+currentCard.getBonus(targetCard)/100f)*(1f-(targetCard.getBouclier()/100f)));
		int levelMax = Mathf.FloorToInt((Mathf.RoundToInt(targetCard.getAttack()*(1.2f+0.1f*s.Power)))*(1f+currentCard.getBonus(targetCard)/100f)*(1f-(targetCard.getBouclier()/100f)));

		score+=Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*((200*(Mathf.Max(0f,1+levelMax-targetCard.getLife())))+(((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)*(Mathf.Min(levelMax,targetCard.getLife())-levelMin)))/(levelMax-levelMin+1f));

		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
