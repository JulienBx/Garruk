using UnityEngine;
using System.Collections.Generic;

public class PistolestC : SkillC
{
	public PistolestC(){
		base.id = 5 ;
		base.ciblage = 6;
		base.animId = 4;
		base.soundId = 25;
		base.nbIntsToSend = 1;
	}

	public override void effects(int targetID, int level, int z, int z2){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = (1+Mathf.RoundToInt((2*level-1)*z/100f));

		degats = caster.getDegatsAgainst(target, degats);

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(8, new List<int>{degats})+"\n"+WordingGame.getText(110, new List<int>{-1}), 2);
		target.addMoveModifyer(new ModifyerM(-1, 5, "", "",1));
		target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
		Game.instance.loadDestinations();
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int maxDegats = caster.getDegatsAgainst(target, 2*level);

		string text = WordingGame.getText(96, new List<int>{target.getLife(),Mathf.Max(0,target.getLife()-1), Mathf.Max(0,target.getLife()-maxDegats)})+"\n"+WordingGame.getText(112, new List<int>{target.getMove(), Mathf.Max(1,target.getMove()-1)});
		return text ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(Game.instance.getBoard().getTileC(t).getCharacterID());
		CardC caster = Game.instance.getCurrentCard();

		int maxDegats = caster.getDegatsAgainst(target, 2*s.Power);

		int score = 5+caster.getDamageScore(target, 1, maxDegats);

		score = Mathf.RoundToInt(s.getProba(s.Power)*(score*(100-target.getEsquive())/100f)/100f);

		return score;
	}
}
