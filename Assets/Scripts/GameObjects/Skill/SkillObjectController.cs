using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillObjectController : GameObjectController
{

	private SkillObjectView view;
	public Texture2D[] skillPictos ;
	public Texture2D attackPicto;
	public Texture2D passPicto;
	public Texture2D noSkillPicto;
	public Skill skill;

	void Awake()
	{
		this.view = gameObject.AddComponent <SkillObjectView>();
	}

	public void setSkill(Skill s)
	{
		this.skill = s;
		this.view.skillVM.face = this.skillPictos [0];
	}

	public void setAttack()
	{
		this.view.skillVM.face = this.attackPicto;
	}

	public void show()
	{
		view.show ();
	}

	public void setPosition(Vector3 p){
		this.view.skillVM.position = p ;
		this.view.replace();
	}

	public void setPosition(Vector3 p, Vector3 s){
		this.view.skillVM.position = p ;
		this.view.skillVM.scale = s ;
		this.view.replace();
	}
}

