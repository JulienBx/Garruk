using UnityEngine;
using System.Collections.Generic;

public class FaveurC : SkillC
{
	public FaveurC(){
		base.id = 104 ;
		base.ciblage = 2;
		base.animId = 2;
		base.soundId = 25;
		base.nbIntsToSend = 0;
	}

	public override void effects(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(128),0);
		Game.instance.setNextPlayer(targetID);
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		string text = WordingGame.getText(128);
		return text ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(board[t.x,t.y]);
		CardC caster = Game.instance.getCurrentCard();

		int score = Mathf.RoundToInt(target.getAttack()/5f);

		return score;
	}
}