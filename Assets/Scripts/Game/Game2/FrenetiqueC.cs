using UnityEngine;
using System.Collections.Generic;

public class FrenetiqueC : SkillC
{
	public FrenetiqueC(){
		base.id = 69 ;
		base.ciblage = 0;
		base.animId = 1;
		base.soundId = 25;
		base.nbIntsToSend=0;
	}

	public override void effects(int targetID, int level){
		CardC caster = Game.instance.getCards().getCardC(targetID);

		int degats = caster.getDegatsAgainst(caster, this.getDegats(level));
		int bonusAttack = this.getAttackBonus(level);

		caster.displayAnim(base.animId);
		caster.displaySkillEffect(WordingSkills.getName(this.id)+"\n"+WordingGame.getText(13, new List<int>{bonusAttack})+"\n"+WordingGame.getText(8, new List<int>{degats}), 1);

		caster.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
		caster.addAttackModifyer(new ModifyerM(bonusAttack, 0, "", "",-1));
	}

	public int getDegats(int level){
		if(level==1){
			return 10;
		}
		else if(level<=3){
			return 9;
		}
		else if(level<=5){
			return 8;
		}
		else if(level<=7){
			return 7;
		}
		else if(level<=9){
			return 6;
		}
		else{
			return 5;
		}
	}

	public int getAttackBonus(int level){
		if(level<=2){
			return 3;
		}
		else if(level<=4){
			return 4;
		}
		else if(level<=6){
			return 5;
		}
		else if(level<=8){
			return 6;
		}
		else{
			return 7;
		}
	}
}
