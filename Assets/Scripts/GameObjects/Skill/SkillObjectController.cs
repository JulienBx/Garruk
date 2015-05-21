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
	public GameSkill gameSkill;

	void Awake()
	{
		this.view = gameObject.AddComponent <SkillObjectView>();
		gameSkill = new GameSkill();
	}

	public void setSkill(Skill s)
	{
		retrieveSkillByName(s);
		this.gameSkill.skill = s;
		this.view.skillVM.face = this.skillPictos [s.Id];
	}

	public void setActive(bool b)
	{
		gameObject.SetActive(b);
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

	public void cast()
	{
		this.gameSkill.cast();
		Debug.Log(gameSkill.GetType());
	}

	void retrieveSkillByName(Skill s)
	{
		switch (s.ResourceName)
		{
			case "Reflexe":
				gameSkill = new Reflexe();
				break;
			case "test":
				gameSkill = new Reflexe();
				break;
			default :
				gameSkill = new GameSkill();
				break;
		}
	}
}

