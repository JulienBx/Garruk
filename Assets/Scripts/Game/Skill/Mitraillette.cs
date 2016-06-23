using UnityEngine;
using System.Collections.Generic;

public class Mitraillette : GameSkill
{
	public Mitraillette()
	{
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Mitraillette","Machine Gun"});
		texts.Add(new string[]{"-ARG1 PV","-ARG1 HP"});
		texts.Add(new string[]{"Fou","Crazy"});
		base.ciblage = 0 ;
		base.auto = true;
		base.id = 30 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(this.getText(0), GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1)));
		GameController.instance.play(this.id);
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
		int maxDamages = 3+GameView.instance.getCurrentSkill().Power*2;
		if(currentCard.isFou()){
			maxDamages = Mathf.RoundToInt(1.25f*maxDamages);
		}

		for(int i = 0 ; i < potentialTargets.Count ; i++){
			targetTile = GameView.instance.getTile(potentialTargets[i]); 
			if(ApplicationModel.player.ToLaunchGameIA){
				if(currentCard.isMine){
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
			else{
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
					GameController.instance.esquive(targets[chosenTarget],this.getText(0));
				}
			}

			targets.RemoveAt(chosenTarget);
			nbTargets++ ;
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
		int damages = currentCard.getNormalDamagesAgainst(targetCard, value);

		string text =this.getText(1, new List<int>{damages});
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,30,this.getText(0),""), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.addAnim(6,GameView.instance.getTile(target));
	}

	public override void applyOnMe(int value){
		if(value==1){
			int myLevel = GameView.instance.getCurrentCard().Skills[0].Power;
			GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).addDamagesModifyer(new Modifyer((11-myLevel), -1, 24, this.getText(0), ""), false,-1);
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0)+"\n"+this.getText(2)+"\n"+this.getText(1, new List<int>{11-myLevel}), 0);
		}
		else{
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0), 1);
		}
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		int score = 0 ;
		GameCard targetCard ;
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = WordingSkills.getProba(s.Id,s.Power);

		List<int> potentialTargets = GameView.instance.getEveryone();
		List<int> targets = new List<int>(); 
		Tile targetTile ; 
		int levelMin ; 
		int levelMax;
		for(int i = 0 ; i < potentialTargets.Count ; i++){
			targetTile = GameView.instance.getTile(potentialTargets[i]); 
			if(t.y>targetTile.y){
				targets.Add(potentialTargets[i]);
			}
		}

		if(targets.Count>0){
			for(int i = 0 ; i < targets.Count ; i++){
				targetCard = GameView.instance.getCard(targets[i]);
				levelMin = 1;
				levelMax = Mathf.FloorToInt((2*s.Power+3)*(1f+currentCard.getBonus(targetCard)/100f)*(1f-(targetCard.getBouclier()/100f)));

				if(currentCard.isFou()){
					levelMax = Mathf.RoundToInt(1.25f*levelMax);
				}

				if(targetCard.isMine){
					score+=Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*((200*(Mathf.Max(0f,levelMax-targetCard.getLife())))+(((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f));
				}
				else{
					score-=Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*((200*(Mathf.Max(0f,levelMax-targetCard.getLife())))+(((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f));
				}
			}

			score = Mathf.RoundToInt(score/targets.Count);
		}

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

