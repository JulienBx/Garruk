using UnityEngine;
using System.Collections.Generic;

public class CannibaleC : SkillC
{
	public CannibaleC(){
		base.id = 21 ;
		base.ciblage = 2;
		base.animId = 1;
		base.soundId = 25;
		base.nbIntsToSend = 0;
	}

	public override void effects(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = target.getLife();
		int attackBonus = Mathf.RoundToInt(target.getAttack()*(0.2f+0.05f*level));
		int lifeBonus = Mathf.RoundToInt(target.getLife()*(0.2f+0.05f*level));

		caster.displaySkillEffect(WordingSkills.getName(this.id)+"\n"+WordingGame.getText(11, new List<int>{lifeBonus})+"\n"+WordingGame.getText(13, new List<int>{attackBonus}), 0);

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(8, new List<int>{attackBonus}), 2);

		target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
		caster.addAttackModifyer(new ModifyerM(attackBonus, 0, "", "",-1));
		caster.addLifeModifyer(new ModifyerM(lifeBonus, 2, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = target.getLife();
		int attackBonus = Mathf.RoundToInt(target.getAttack()*(0.2f+0.05f*level));
		int lifeBonus = Mathf.RoundToInt(target.getLife()*(0.2f+0.05f*level));

		string text = WordingGame.getText(97, new List<int>{attackBonus,lifeBonus});
		return text ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(Game.instance.getBoard().getTileC(t).getCharacterID());
		CardC caster = Game.instance.getCurrentCard();

		int degats = target.getLife();
		int attackBonus = Mathf.RoundToInt(target.getAttack()*(0.2f+0.05f*s.Power));
		int lifeBonus = Mathf.RoundToInt(target.getLife()*(0.2f+0.05f*s.Power));

		int score = -2*target.getLife()+3*attackBonus;

		score = Mathf.RoundToInt(s.getProba(s.Power)*(score*(100-target.getEsquive())/100f)/100f);

		return score;
	}
}
