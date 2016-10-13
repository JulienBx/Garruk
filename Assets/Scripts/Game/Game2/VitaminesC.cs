using UnityEngine;
using System.Collections.Generic;

public class VitaminesC : SkillC
{
	public VitaminesC(){
		base.id = 6 ;
		base.ciblage = 2;
		base.animId = 2;
		base.soundId = 25;
		base.nbIntsToSend = 0;
	}

	public override void effects(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int bonus = Mathf.Min(2*level,target.getTotalLife()-target.getLife());

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(11, new List<int>{bonus})+"\n"+WordingGame.getText(108, new List<int>{1}), 0);
		target.addDamageModifyer(new ModifyerM(-1*bonus, 0, "", "",-1));
		target.addMoveModifyer(new ModifyerM(1, 5, "", "",1));
		target.showIcons(true);
		Game.instance.loadDestinations();
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int bonus = 2*level;

		string text = WordingGame.getText(78, new List<int>{target.getLife(), Mathf.Min(target.getTotalLife(),target.getLife()+bonus)})+"\n"+WordingGame.getText(112, new List<int>{target.getMove(), target.getMove()+1});
		return text ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(Game.instance.getBoard().getTileC(t).getCharacterID());
		CardC caster = Game.instance.getCurrentCard();

		int score = Mathf.Min(2*s.Power,target.getTotalLife()-target.getLife())+5;
		score = Mathf.RoundToInt(s.getProba(s.Power)*(score*(100-target.getEsquive())/100f)/100f);

		return score;
	}
}
