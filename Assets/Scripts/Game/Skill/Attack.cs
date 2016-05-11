using UnityEngine;
using System.Collections.Generic;

public class Attack : GameSkill
{
	public Attack(){
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Attaque";
		base.ciblage = 1 ;
		base.auto = false;
		base.id = 0 ;
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
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
			GameController.instance.esquive(target,1);
		}
		else{
			GameController.instance.applyOn(target);
		}
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getNormalDamagesAgainst(targetCard, currentCard.getAttack());
		if(currentCard.isHumaHunter() && (targetCard.CardType.Id==5 ||targetCard.CardType.Id==6)){
			damages=0;
		}
		string text = "-"+damages+"PV";
						
		if (currentCard.isLache()){
			if(GameView.instance.getIsFirstPlayer() == currentCard.isMine){
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y-1==GameView.instance.getPlayingCardController(target).getTile().y){
					damages = Mathf.Min(targetCard.getLife(), 5+currentCard.getSkills()[0].Power+damages);
					text="-"+damages+"PV"+"\n(lache)";
				}
			}
			else{
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y==GameView.instance.getPlayingCardController(target).getTile().y-1){
					damages = Mathf.Min(targetCard.getLife(), 5+currentCard.getSkills()[0].Power+damages);
					text="-"+damages+"PV"+"\n(lache)";
				}
			}
		}
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,1,"Attaque",damages+" dégats subis"), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.addAnim(3,GameView.instance.getTile(target));
		SoundController.instance.playSound(25);

		if(ApplicationModel.player.ToLaunchGameTutorial && GameView.instance.sequenceID<14){
			GameView.instance.hitNextTutorial();
		}
	}

	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getNormalDamagesAgainst(targetCard, currentCard.getAttack());

		string text = "PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages);
						
		if (currentCard.isLache()){
			if(GameView.instance.getIsFirstPlayer() == currentCard.isMine){
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y-1==GameView.instance.getPlayingCardController(target).getTile().y){
					damages = Mathf.Min(targetCard.getLife(), 5+currentCard.getSkills()[0].Power+damages);
					text="PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages)+"\n(lache)";
				}
			}
			else{
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y==GameView.instance.getPlayingCardController(target).getTile().y-1){
					damages = Mathf.Min(targetCard.getLife(), 5+currentCard.getSkills()[0].Power+damages);
					text="PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages)+"\n(lache)";
				}
			}
		}

		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		int score = 0 ;
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
		GameCard currentCard = GameView.instance.getCurrentCard();

		int damages = currentCard.getNormalDamagesAgainst(targetCard, currentCard.getAttack());
		if(damages>=targetCard.getLife()){
			score+=200;
		}
		else{
			score+=Mathf.RoundToInt(((100-targetCard.getEsquive())/100f)*(damages+5-targetCard.getLife()/10f));
		}

		if(currentCard.isHumaHunter() && (targetCard.CardType.Id==5 ||targetCard.CardType.Id==6)){
			score=0;
		}
					
		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
