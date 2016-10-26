﻿using UnityEngine;
using System.Collections.Generic;

public class TelepiegeC : SkillC
{
	public TelepiegeC(){
		base.id = 58 ;
		base.ciblage = 9;
		base.animId = 5;
		base.soundId = 25;
		base.nbIntsToSend = 0;
	}

	public override void effects(int x, int level, int y){
		CardC caster = Game.instance.getCurrentCard();

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		if(Game.instance.getCurrentCard().getCardM().isMine()){
			Game.instance.getBoard().getTileC(x,y).displayAnim(base.animId);
			Game.instance.getBoard().getTileC(x,y).displaySkillEffect(WordingGame.getText(116), 0);
		}

		Game.instance.getBoard().getTileC(x,y).setTrap(0, level, Game.instance.getCurrentCard().getCardM().isMine()==Game.instance.isFirstPlayer());
	}

	public override string getSkillText(int targetID, int level){
		string s = Game.instance.getCurrentSkillButtonC().getSkillText();
		return s ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		int score = 0;
		if(t.y==3 || t.y==4){
			score = 5 ;
		}
		else if(t.y==2 || t.y==5){
			score = 3 ;
		}
		return score;
	}
}
