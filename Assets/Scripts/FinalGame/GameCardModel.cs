using System;
using System.Collections;
using System.Collections.Generic;

public class GameCardModel
{
	int life;
	int attack;
	List<SkillModel> skills;

	public GameCardModel (int l, int a, SkillModel s0, SkillModel s1, SkillModel s2, SkillModel s3)
	{
		this.life = l;
		this.attack = a;
		this.skills = new List<SkillModel>();
		this.skills.Add(s0);
		if(s1.getID()==-1){
			this.skills.Add(s1);
		}
		if(s2.getID()==-1){
			this.skills.Add(s2);
		}
		if(s3.getID()==-1){
			this.skills.Add(s3);
		}
	}

	public int getCharacterType(){
		return this.skills[0].getID();
	}

	public int getAttack(){
		return this.attack;
	}

	public int getLife(){
		return this.life;
	}
}