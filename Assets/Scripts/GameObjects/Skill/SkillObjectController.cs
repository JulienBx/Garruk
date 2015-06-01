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
	public Texture2D[] borders ;
	public Texture2D[] faces ;

	public GUIStyle[] styles;

	public int id ;
	public bool isActive ;

	void Awake ()
	{
			this.view = gameObject.AddComponent <SkillObjectView> ();
			this.view.skillVM.skillRectStyle = styles [0];
			this.view.skillVM.skillTitleTextStyle = styles [1];
			this.view.skillVM.skillDescriptionTextStyle = styles [2];
			this.view.skillVM.cadreStyle = styles [3];
			this.view.skillVM.powerStyle = styles [5];
	}

	public void setOwner (bool b)
	{
			this.view.skillVM.isMine = b;
	}

	public void resize ()
	{
			this.view.skillVM.resize (this.id);
	}

	public void setID (int i)
	{
			this.id = i;
	}

	public void setSkill (Skill s)
	{
		this.view.skillVM.skillDescription = s.Description;
		
		if (s.Name.Length > 5) {
			this.view.skillVM.skillName = s.Name.Substring (0, 5) + ".";
		}
		else {
			this.view.skillVM.skillName = s.Name;
		}
		this.view.skillVM.power = s.Power;
		this.view.skillVM.face = this.skillPictos [s.Id - 2];
	}

	public void setActiveStatus (bool isactive)
	{
		this.isActive = isactive;
	}
	
	public void setControlStatus (bool isControlActive)
	{
		this.view.skillVM.isControlActive = isControlActive;
		
		if (this.isActive) {
			if (isControlActive) {
					this.view.skillVM.border = borders [1];
			} else {
					this.view.skillVM.border = borders [2];
			}
		} else {
				this.view.skillVM.border = borders [0];
		}
	}

	public void hoverSkill ()
	{
			this.view.skillVM.border = borders [3];
			this.view.changeBorder ();
	}

	public void setControlActive (bool b)
	{
			this.view.skillVM.isControlActive = b;
			this.endHoverSkill ();
	}

	public void endHoverSkill ()
	{
			if (this.isActive) {
					if (this.view.skillVM.isControlActive) {
							this.view.skillVM.border = borders [1];
					} else {
							this.view.skillVM.border = borders [2];
					}
			} else {
					this.view.skillVM.border = borders [0];
			}
			this.view.changeBorder ();
	}

	public void setActive (bool b)
	{
			gameObject.SetActive (b);
	}

	public void setAttack ()
	{	
		this.view.skillVM.face = this.attackPicto;
	}

	public void setPass ()
	{
		this.view.skillVM.face = this.passPicto;
	}

	public void show ()
	{
		view.show ();
	}

	public void setPosition (Vector3 p)
	{
			this.view.skillVM.position = p;
			this.view.replace ();
	}

	public void setPosition (Vector3 p, Vector3 s)
	{
			this.view.skillVM.position = p;
			this.view.skillVM.scale = s;
			this.view.replace ();
	}

	public void clickSkill ()
	{
			GameController.instance.clickSkillHandler (this.id);
	}
}

