﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillObjectController : GameObjectController
{
	private SkillObjectView view;
	public Texture2D[] skillPictos ;
	public Texture2D attackPicto;
	public Texture2D passPicto;
	public Texture2D noSkillPicto;

	public int id ;

	void Awake()
	{
		this.view = gameObject.AddComponent <SkillObjectView>();
	}

	public void setID(int i)
	{
		this.id = i;
	}

	public void setSkill(Skill s)
	{
		this.view.skillVM.face = this.skillPictos [s.Id];
	}

	public void setActive(bool b)
	{
		gameObject.SetActive(b);
	}

	public void setControlsActive(bool b)
	{
		this.view.skillVM.isActive = b;
	}

	public void setAttack()
	{
		this.view.skillVM.face = this.attackPicto;
	}

	public void setPass()
	{
		this.view.skillVM.face = this.passPicto;
	}

	public void show()
	{
		view.show();
	}

	public void setPosition(Vector3 p)
	{
		this.view.skillVM.position = p;
		this.view.replace();
	}

	public void setPosition(Vector3 p, Vector3 s)
	{
		this.view.skillVM.position = p;
		this.view.skillVM.scale = s;
		this.view.replace();
	}


	public void clickSkill(){
		GameController.instance.clickSkillHandler(this.id);
	}
}

