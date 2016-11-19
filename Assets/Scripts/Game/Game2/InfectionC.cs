using UnityEngine;
using System.Collections.Generic;

public class InfectionC : SkillC
{
	public InfectionC(){
		base.id = 12 ;
		base.ciblage = 1;
		base.animId = 1;
		base.soundId = 25;
		base.nbIntsToSend = 1;
	}

	public override void effects(int targetID, int level, int z){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = caster.getDegatsAgainst(target, Mathf.RoundToInt(caster.getAttack()*(20f+8f*level)/100f));
		bool poison = (z<=25) ;

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		target.displayAnim(base.animId);
		if(poison){
			target.displaySkillEffect(WordingGame.getText(77, new List<int>{degats})+"\n"+WordingGame.getText(9), 2);
		}
		else{
			target.displaySkillEffect(WordingGame.getText(77, new List<int>{degats}), 2);
		}

		if(poison){
			target.addStateModifyer(new ModifyerM(10, 6, WordingGame.getText(113, new List<int>{10}), WordingSkills.getName(this.id),-1));
		}
		target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = caster.getDegatsAgainst(target, Mathf.RoundToInt(caster.getAttack()*(20f+8f*level)/100f));

		string text = WordingGame.getText(78, new List<int>{target.getLife(),target.getLife()-degats})+"\n"+WordingGame.getText(118);
		return text ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(board[t.x,t.y]);
		CardC caster = Game.instance.getCurrentCard();

		int score = caster.getDamageScore(target, Mathf.RoundToInt(caster.getAttack()*(20f+8f*s.Power)/100f));
		score = Mathf.RoundToInt(s.getProba(s.Power)*(score*(100-target.getEsquive())/100f)/100f);
		score+=10;

		return score;
	}
}
