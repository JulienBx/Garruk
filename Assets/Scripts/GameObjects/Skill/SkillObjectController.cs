using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillObjectController : GameObjectController
{

	private SkillObjectView view;
	public Texture2D[] skillPictos ;
	public Texture2D attackPicto;
	public Skill skill;

	void Awake()
	{
		this.view = gameObject.AddComponent <SkillObjectView>();
	}

	public void setSkill(Skill s)
	{
		this.skill = s;
		this.view.skillVM.face = this.skillPictos [s.Id];
	}
	public void setAttack()
	{
		this.view.skillVM.face = this.attackPicto;
	}
	public void show()
	{
		view.show ();
	}
}

