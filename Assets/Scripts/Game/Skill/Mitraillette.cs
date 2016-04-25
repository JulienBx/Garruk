using UnityEngine;
using System.Collections.Generic;

public class Mitraillette : GameSkill
{
	public Mitraillette()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Mitraillette";
		base.ciblage = 0 ;
		base.auto = true;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name, WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1));
		GameController.instance.play(GameView.instance.runningSkill);
	}
	
	public override void resolve(List<Tile> targetsP)
	{	
		GameCard currentCard = GameView.instance.getCurrentCard();

		List<int> potentialTargets = GameView.instance.getEveryone();
		List<int> targets = new List<int>();
		bool isFirstP = GameView.instance.getIsFirstPlayer();
		Tile currentTile = GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()); 
		Tile targetTile ; 
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int maxDamages = 5+GameView.instance.getCurrentSkill().Power*2;
		if(currentCard.isFou()){
			maxDamages = Mathf.RoundToInt(1.25f*maxDamages);
		}

		for(int i = 0 ; i < potentialTargets.Count ; i++){
			targetTile = GameView.instance.getTile(potentialTargets[i]); 
			if(isFirstP){
				if(currentTile.y<targetTile.y){
					targets.Add(potentialTargets[i]);
				}
			}
			else{
				if(currentTile.y>targetTile.y){
					targets.Add(potentialTargets[i]);
				}
			}
		}

		int nbTargets = 0 ; 

		while (nbTargets<3 && targets.Count>0){
			int chosenTarget = Random.Range(0,targets.Count);

			if (Random.Range(1,101) <= GameView.instance.getCard(targets[chosenTarget]).getMagicalEsquive()){
				GameController.instance.esquive(targets[chosenTarget],1);
			}
			else{
				if (Random.Range(1,101) <= proba){
					int value = Random.Range(1,maxDamages+1);
					GameController.instance.applyOn2(targets[chosenTarget], value);
				}
				else{
					GameController.instance.esquive(targets[chosenTarget],base.name);
				}
			}

			targets.RemoveAt(chosenTarget);
			nbTargets++ ;
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
		int damages = currentCard.getNormalDamagesAgainst(targetCard, value);

		string text ="-"+damages+"PV";
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,30,base.name,damages+" dégats subis"), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.addAnim(GameView.instance.getTile(target), 30);
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
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}

	public override int getActionScore(Tile t, Skill s){
		int score = 0 ;
		GameCard targetCard ;
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = WordingSkills.getProba(s.Id,s.Power);

		List<int> potentialTargets = GameView.instance.getEveryone();
		List<int> targets = new List<int>();
		Tile currentTile = GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()); 
		Tile targetTile ; 
		int levelMin ; 
		int levelMax;
		for(int i = 0 ; i < potentialTargets.Count ; i++){
			targetTile = GameView.instance.getTile(potentialTargets[i]); 
			if(currentTile.y>targetTile.y){
				targets.Add(potentialTargets[i]);
			}
		}

		for(int i = 0 ; i < targets.Count ; i++){
			targetCard = GameView.instance.getCard(targets[i]);
			levelMin = 1;
			levelMax = Mathf.FloorToInt((5+2*s.Power)*(1f+currentCard.getBonus(targetCard)/100f)*(1f-(targetCard.getBouclier()/100f)));

			if(targetCard.isMine){
				score+=Mathf.RoundToInt((proba-targetCard.getEsquive()/100f)*((200*(Mathf.Max(0f,levelMax-targetCard.getLife())))+(((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f));
			}
			else{
				score-=Mathf.RoundToInt((proba-targetCard.getEsquive()/100f)*((200*(Mathf.Max(0f,levelMax-targetCard.getLife())))+(((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f));
			}
		}

		score = Mathf.RoundToInt(score/targets.Count);

		if(currentCard.isFou()){
			int damages = 11-currentCard.Skills[0].Power;
			if(damages>=currentCard.getLife()){
				score-=100;
			}
			else{
				score-=damages;
			}
		}

		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
		}
}

