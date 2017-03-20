using System;

public class SkillModel
{
	int id;
	int level;

	public SkillModel (int i, int l)
	{
		this.id = i;
		this.level = l;
	}

	public int getID(){
		return this.id;
	}
}