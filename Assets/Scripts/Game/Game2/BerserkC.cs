using UnityEngine;
using System.Collections.Generic;

public class BerserkC : SkillC
{
	public BerserkC(){
		base.id = 16 ;
		base.ciblage = 1;
		base.animId = 0;
		base.soundId = 25;
		base.nbIntsToSend = 0;
	}

	public override void effects(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = caster.getDegatsAgainst(target, Mathf.RoundToInt(caster.getAttack()*(this.getAttackPercentage(level)/100f)));
		int selfDegats = caster.getDegatsAgainst(caster, this.getDegats(level));

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);
		caster.displayAnim(base.animId);
		caster.displaySkillEffect(WordingGame.getText(77, new List<int>{selfDegats}), 2);

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(77, new List<int>{degats}), 2);

		caster.addDamageModifyer(new ModifyerM(selfDegats, -1, "", "",-1));
		target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = caster.getDegatsAgainst(target, Mathf.RoundToInt(caster.getAttack()*(this.getAttackPercentage(level)/100f)));
		int selfDegats = caster.getDegatsAgainst(caster, this.getDegats(level));

		string text = WordingGame.getText(78, new List<int>{target.getLife(),target.getLife()-degats});
		if(selfDegats<=1){
			text+="\n"+WordingGame.getText(89, new List<int>{selfDegats});
		}
		else{
			text+="\n"+WordingGame.getText(90, new List<int>{selfDegats});
		}

		return text ;
	}

	public int getDegats(int level){
		if(level<=1){
			return 20;
		}
		else if(level<=3){
			return 17;
		}
		else if(level<=5){
			return 14;
		}
		else if(level<=7){
			return 11;
		}
		else if(level<=9){
			return 8;
		}
		else{
			return 5;
		}
	}

	public int getAttackPercentage(int level){
		if(level<=2){
			return 110;
		}
		else if(level<=4){
			return 115;
		}
		else if(level<=6){
			return 120;
		}
		else if(level<=8){
			return 125;
		}
		else{
			return 130;
		}
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(board[t.x,t.y]);
		CardC caster = Game.instance.getCurrentCard();

		int score = caster.getDamageScore(target, Mathf.RoundToInt(caster.getAttack()*(this.getAttackPercentage(s.Power)/100f)));
		score = Mathf.RoundToInt(s.getProba(s.Power)*(score*(100-target.getEsquive())/100f)/100f);

		int score2 = caster.getDamageScore(caster, this.getDegats(s.Power));
		score2 = Mathf.RoundToInt(s.getProba(s.Power)*(score2*(100-caster.getEsquive())/100f)/100f);

		score+=score2 ;
		return score;
	}
}
