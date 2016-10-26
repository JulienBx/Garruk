using UnityEngine;
using System.Collections.Generic;

public class KunaiC : SkillC
{
	public KunaiC(){
		base.id = 8 ;
		base.ciblage = 10;
		base.animId = 1;
		base.soundId = 25;
		base.nbIntsToSend = 2;
	}

	public override void effects(int targetID, int level, int z, int z2){
		CardC caster = Game.instance.getCurrentCard();

		Debug.Log(Game.instance.getCurrentCard().getCardM().isMine());
		List<int> opponents = Game.instance.getTargetableOpponents(Game.instance.getCurrentCard().getCardM().isMine());
		z = opponents[Mathf.Min(Mathf.FloorToInt(z/(100/opponents.Count)),opponents.Count-1)];
		CardC target = Game.instance.getCards().getCardC(z);

		int degats = caster.getDegatsAgainst(target, Mathf.RoundToInt(2+level+Mathf.RoundToInt((5f*z2)/100f)));

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(77, new List<int>{degats}), 2);

		target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		string s = Game.instance.getCurrentSkillButtonC().getSkillText();
		return s ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC caster = Game.instance.getCurrentCard();
		CardC target ;

		List<int> opponents = Game.instance.getTargetableOpponents(Game.instance.getCurrentCard().getCardM().isMine());
		int score = 0 ;
		int tempScore ;

		for(int i = 0 ; i < opponents.Count ; i++){
			target = Game.instance.getCards().getCardC(opponents[i]);
		
			tempScore = caster.getDamageScore(target, s.Power, s.Power+5);
			tempScore = Mathf.RoundToInt(s.getProba(s.Power)*(score*(100-target.getEsquive())/100f)/100f);
			score+=tempScore;
		}
		score = score/opponents.Count;

		return score;
	}
}
