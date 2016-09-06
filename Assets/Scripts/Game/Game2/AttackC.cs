using UnityEngine;
using System.Collections.Generic;

public class AttackC : SkillC
{
	public AttackC(){
		this.id = 0 ;
		base.ciblage = 1;
		base.animId = 0;
		base.soundId = 25;
//		base.texts = new List<string[]>();
//		texts.Add(new string[]{"Attaque","Attack"});
//		texts.Add(new string[]{"-ARG1 PV","-ARG1 HP"});
//		texts.Add(new string[]{"PV : ARG1 -> ARG2","HP : ARG1 -> ARG2"});
//		texts.Add(new string[]{"PV : ARG1 -> ARG2\nlâche","HP : ARG1 -> ARG2\ncoward"});
//		base.ciblage = 1 ;
//		base.auto = false;
//		base.id = 0 ;
	}

	public override void resolve(int x, int y, Skill skill){
		int targetID = Game.instance.getBoard().getTileC(x,y).getCharacterID();
		CardC target = Game.instance.getCards().getCardC(targetID);
		//int level = skill.Power;
		int level = 1;
		if(UnityEngine.Random.Range(0,101)<=WordingSkills.getProba(this.id, level)){
			if(UnityEngine.Random.Range(0,101)<=target.getEsquive()){
				if(Game.instance.isIA() || Game.instance.isTutorial()){
					Game.instance.getSkills().skills[this.id].dodge(targetID);
				}
				else{
					GameRPC.instance.launchRPC("DodgeSkillRPC", this.id, targetID);
				}
			}
			else{
				if(Game.instance.isIA() || Game.instance.isTutorial()){
					Game.instance.getSkills().skills[this.id].effects(targetID);
				}
				else{
					GameRPC.instance.launchRPC("EffectsSkillRPC", this.id, targetID);
				}
			}
		}
		else{
			if(Game.instance.isIA() || Game.instance.isTutorial()){
				Game.instance.getSkills().skills[this.id].fail();
			}
			else{
				GameRPC.instance.launchRPC("FailSkillRPC", this.id);
			}
		}
	}

	public override void effects(int targetID){
		SoundController.instance.playSound(base.soundId);

		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = caster.getDegatsAgainst(target, caster.getAttack());

		Game.instance.getCurrentCard().displaySkillEffect(WordingSkills.getName(this.id), 1);
		Game.instance.getCards().getCardC(targetID).displayAnim(base.animId);
		Game.instance.getCards().getCardC(targetID).displaySkillEffect(WordingGame.getText(77, new List<int>{degats}), 2);

		target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));

	}

	public override string getSkillText(int targetID){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();
		int degats = caster.getDegatsAgainst(target, caster.getAttack());
		string text = WordingGame.getText(78, new List<int>{target.getLife(),target.getLife()-degats});
		return text ;
	}

//
//	public override string getTargetText(int target){
//		GameCard targetCard = GameView.instance.getCard(target);
//		GameCard currentCard = GameView.instance.getCurrentCard();
//		int damages = currentCard.getNormalDamagesAgainst(targetCard, currentCard.getAttack());
//
//		string text = this.getText(2,new List<int>{targetCard.getLife(),targetCard.getLife()-damages});
//						
//		if (currentCard.isLache()){
//			if(GameView.instance.getIsFirstPlayer() == currentCard.isMine){
//				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y-1==GameView.instance.getPlayingCardController(target).getTile().y){
//					damages = Mathf.Min(targetCard.getLife(), 5+currentCard.getSkills()[0].Power+damages);
//					text=this.getText(3,new List<int>{targetCard.getLife(),targetCard.getLife()-damages});
//				}
//			}
//			else{
//				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y==GameView.instance.getPlayingCardController(target).getTile().y-1){
//					damages = Mathf.Min(targetCard.getLife(), 5+currentCard.getSkills()[0].Power+damages);
//					text=this.getText(3,new List<int>{targetCard.getLife(),targetCard.getLife()-damages});
//				}
//			}
//		}
//
//		int probaEsquive = targetCard.getEsquive();
//		int probaHit = Mathf.Max(0,100-probaEsquive) ;
//		
//		text += "\nHIT% : "+probaHit;
//		
//		return text ;
//	}
//
//	public override void applyOnMe(int value){
//		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0), 1);
//		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
//	}
//
//	public override int getActionScore(Tile t, Skill s){
//		int score = 0 ;
//		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
//		GameCard currentCard = GameView.instance.getCurrentCard();
//
//		int damages = currentCard.getNormalDamagesAgainst(targetCard, currentCard.getAttack());
//		if(damages>=targetCard.getLife()){
//			score+=Mathf.RoundToInt((100f-targetCard.getEsquive())*2f+targetCard.getLife()/10f);
//		}
//		else{
//			score+=Mathf.RoundToInt(((100-targetCard.getEsquive())/100f)*(damages+5-targetCard.getLife()/10f));
//		}
//
//		if(currentCard.isHumaHunter() && (targetCard.CardType.Id==5 ||targetCard.CardType.Id==6)){
//			score=0;
//		}
//					
//		score = score * GameView.instance.IA.getAgressiveFactor() ;
//		return score ;
//	}
}
