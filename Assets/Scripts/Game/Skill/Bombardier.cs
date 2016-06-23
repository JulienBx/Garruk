using UnityEngine;
using System.Collections.Generic;

public class Bombardier : GameSkill
{
	public Bombardier(){
		this.initTexts();
		this.numberOfExpectedTargets = 0 ; 
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Bombardier","Bomber"});
		texts.Add(new string[]{"-ARG1 PV","-ARG1 HP"});
		texts.Add(new string[]{"Fou","Crazy"});
		base.ciblage = 0 ;
		base.auto = true;
		base.id = 24 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(this.getText(0), GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1)));
		GameController.instance.play(this.id);
	}
	
	public override void resolve(List<Tile> targetsP)
	{	
		GameCard currentCard = GameView.instance.getCurrentCard();
		List<int> targets = GameView.instance.getEveryone() ; 
		int mindamages = GameView.instance.getCurrentSkill().Power;
		int maxdamages = 5+GameView.instance.getCurrentSkill().Power;

		if(currentCard.isFou()){
			maxdamages = Mathf.RoundToInt(1.25f*maxdamages);
		}
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		for(int i = 0 ; i < targets.Count ; i++){
			if (Random.Range(1,101) <= GameView.instance.getCard(targets[i]).getMagicalEsquive()){
				GameController.instance.esquive(targets[i],1);
			}
			else{
				if (Random.Range(1,101) <= proba){
					GameController.instance.applyOn2(targets[i], Random.Range(mindamages,maxdamages+1));
				}
				else{
					GameController.instance.esquive(targets[i],this.getText(0));
				}
			}
		}
		GameController.instance.playSound(36);
		if(currentCard.isFou()){
			GameController.instance.applyOnMe(1);
		}
		else{
			GameController.instance.applyOnMe(-1);
		}
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int amount){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getNormalDamagesAgainst(targetCard, amount);

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 24, this.getText(0), ""),  (target==GameView.instance.getCurrentPlayingCard()), GameView.instance.getCurrentPlayingCard());
		GameView.instance.displaySkillEffect(target, this.getText(1, new List<int>{damages}), 0);	
		GameView.instance.addAnim(5,GameView.instance.getTile(target));
	}

	public override void applyOnMe(int value){
		if(value==1){
			int myLevel = GameView.instance.getCurrentCard().Skills[0].Power;
			GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).addDamagesModifyer(new Modifyer((11-myLevel), -1, 24, this.getText(0), ""), true,-1);
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0)+"\n"+this.getText(2)+"\n"+this.getText(1, new List<int>{(11-myLevel)}), 0);
		}
		else{
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0), 1);
		}
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard ;
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int score = 0;
		List<int> neighbours = GameView.instance.getEveryone();
		int min;
		int max;
		int mindamages = GameView.instance.getCurrentSkill().Power;
		int maxdamages = 5+GameView.instance.getCurrentSkill().Power;

		for(int i = 0 ; i < neighbours.Count ; i++){
			targetCard = GameView.instance.getCard(neighbours[i]);
			min = currentCard.getNormalDamagesAgainst(targetCard, mindamages);
			max = currentCard.getNormalDamagesAgainst(targetCard, maxdamages); 

			if(targetCard.isMine){
				score+=Mathf.RoundToInt(((proba-targetCard.getMagicalEsquive())/100f)*((200f*(Mathf.Max(0f,1+max-targetCard.getLife())))+(((min+Mathf.Min(max,targetCard.getLife()))/2f)*(Mathf.Min(max,targetCard.getLife())-min)))/(max-min+1f));
			}
			else{
				score-=Mathf.RoundToInt(((proba-targetCard.getMagicalEsquive())/100f)*((200f*(Mathf.Max(0f,1+max-targetCard.getLife())))+(((min+Mathf.Min(max,targetCard.getLife()))/2f)*(Mathf.Min(max,targetCard.getLife())-min)))/(max-min+1f));
			}
		}

		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
