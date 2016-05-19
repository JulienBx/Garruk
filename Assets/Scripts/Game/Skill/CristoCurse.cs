using UnityEngine;
using System.Collections.Generic;

public class CristoCurse : GameSkill
{
	public CristoCurse(){
		this.numberOfExpectedTargets = 1 ; 
		base.name = "CristoCurse";
		base.ciblage = 9 ;
		base.auto = false;
		base.id = 41 ;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		this.displayTargets();
	}

	public override void resolve(List<Tile> targetsP)
	{	
		GameController.instance.play(this.id);
		int target = GameView.instance.getTileCharacterID(targetsP[0].x, targetsP[0].y);

		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
			GameController.instance.esquive(target,1);
		}
		else{
			GameController.instance.applyOn(target);
		}

		List<int> targets = GameView.instance.getEveryoneCardtype(GameView.instance.getCard(target).CardType.Id);
		for (int i = 0 ; i < targets.Count ; i++){
			if (Random.Range(1,101) <= GameView.instance.getCard(targets[i]).getEsquive())
			{                             
				GameController.instance.esquive(targets[i],1);
			}
			else{
				GameController.instance.applyOn(targets[i]);
			}
		}
		GameController.instance.playSound(36);

		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*(GameView.instance.getCurrentSkill().Power*5f+50f)/100f));

		string text = "-"+damages+"PV";
						
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,1,"Attaque",damages+" dégats subis"), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.addAnim(5,GameView.instance.getTile(target));
	}

	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*(GameView.instance.getCurrentSkill().Power*5f+50f)/100f));

		string text = "PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages)+"\nInflige "+damages+" dégats à toutes les unités de cette faction";
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
		SoundController.instance.playSound(36);
	}

	public override int getActionScore(Tile t, Skill s){
		int score = 0 ;

		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard;
		int target = GameView.instance.getTileCharacterID(t.x, t.y);

		List<int> targets = GameView.instance.getEveryoneCardtype(GameView.instance.getCard(target).CardType.Id) ; 
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int damages ;
		int bonus ; 

		for(int i = 0 ; i < targets.Count ; i++){
			targetCard = GameView.instance.getCard(targets[i]);
			damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*(s.Power*5f+50f)/100f));

			if(damages>=targetCard.getLife()){
				if(targetCard.isMine){
					bonus=Mathf.RoundToInt(1000f*(proba-targetCard.getMagicalEsquive())/100f);
				}
				else{
					bonus=-1*Mathf.RoundToInt(1000f*(proba-targetCard.getMagicalEsquive())/100f);
				}
			}
			else{
				bonus=Mathf.RoundToInt((damages+5-targetCard.getLife()/10f)*(proba-targetCard.getMagicalEsquive())/100f);
				if(!targetCard.isMine){
					bonus = bonus*(-1);
				}
			}

			score+=bonus;
		}
		score = score * GameView.instance.IA.getAgressiveFactor() ;

		return score ;
	}
}
