using UnityEngine;
using System.Collections.Generic;

public class FouetC : SkillC
{
	public FouetC(){
		this.id = 18 ;
		base.ciblage = 2;
		base.animId = 0;
		base.soundId = 25;
	}

	public override void effects(int targetID, int level){
		SoundController.instance.playSound(base.soundId);

		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = caster.getDegatsAgainst(target, Mathf.RoundToInt(caster.getAttack()*(this.getAttackPercentage(level)/100f)));
		int attackBonus = this.getAttackBonus(level);

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);
		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(13, new List<int>{attackBonus})+"\n"+WordingGame.getText(8, new List<int>{degats}), 2);

		target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
		target.addAttackModifyer(new ModifyerM(attackBonus, -1, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = caster.getDegatsAgainst(target, Mathf.RoundToInt(caster.getAttack()*(this.getAttackPercentage(level)/100f)));
		int attackBonus = this.getAttackBonus(level);

		string text = WordingGame.getText(84, new List<int>{target.getAttack(),target.getAttack()+attackBonus});
		text+="\n"+WordingGame.getText(78, new List<int>{target.getLife(),target.getLife()-degats});
		return text ;
	}

	public int getAttackPercentage(int level){
		if(level<=1){
			return 50;
		}
		else if(level<=3){
			return 40;
		}
		else if(level<=5){
			return 30;
		}
		else if(level<=7){
			return 20;
		}
		else{
			return 10;
		}
	}

	public int getAttackBonus(int level){
		if(level<=0){
			return 3;
		}
		else if(level<=2){
			return 4;
		}
		else if(level<=4){
			return 5;
		}
		else if(level<=6){
			return 6;
		}
		else if(level<=8){
			return 7;
		}
		else{
			return 8;
		}
	}

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
