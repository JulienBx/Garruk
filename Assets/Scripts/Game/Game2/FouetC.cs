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
		Debug.Log(level);
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
		if(level<=2){
			return 50;
		}
		else if(level<=4){
			return 40;
		}
		else if(level<=6){
			return 30;
		}
		else if(level<=8){
			return 20;
		}
		else{
			return 10;
		}
	}

	public int getAttackBonus(int level){
		if(level<=1){
			return 3;
		}
		else if(level<=3){
			return 4;
		}
		else if(level<=5){
			return 5;
		}
		else if(level<=7){
			return 6;
		}
		else if(level<=9){
			return 7;
		}
		else{
			return 8;
		}
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(Game.instance.getBoard().getTileC(t).getCharacterID());
		CardC caster = Game.instance.getCurrentCard();

		int score = caster.getDamageScore(target, Mathf.RoundToInt(caster.getAttack()*(this.getAttackPercentage(s.Power)/100f)));
	
		int attackBonus = this.getAttackBonus(s.Power)*Mathf.FloorToInt(target.getLife()/10f);
		if(target.getCardM().isMine()){
			score+=attackBonus;
		}
		else{
			score-=attackBonus;
		}
		return score;
	}
}
