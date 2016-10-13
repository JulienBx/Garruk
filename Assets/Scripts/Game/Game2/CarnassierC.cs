using UnityEngine;
using System.Collections.Generic;

public class CarnassierC : SkillC
{
	public CarnassierC(){
		base.id = 71;
		base.ciblage = 0;
		base.animId = 2;
		base.soundId = 25;
	}

	public int getBonus(int level){
		return (10+4*level);
	}
}
