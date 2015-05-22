using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class SkillController : GameObjectController {

	public GUIStyle[] skillVMStyle;
	private SkillView view;
	
	void Awake () 
	{	
		this.view = gameObject.AddComponent <SkillView>();
	}
	public void resize()
	{
		this.defineSkillPopUpPositions ();
		view.skillVM.resize ();
	}
	public void setSkill(Skill skill)
	{
		view.skillVM.number=skill.Id;
		view.skillVM.name = skill.Name;
		view.skillVM.description = skill.Description;
		view.skillVM.power = skill.Power;
		view.skillVM.manaCost = skill.ManaCost;
		view.skillVM.guiDepth = -1;
	}
	public void setSkillLevelMetals(Texture metal)
	{
		for (int i=0;i<6;i++)
		{
		view.skillVM.skillLevel[i] = metal;
		}
	}
	public void setSkillPicto(Texture picto)
	{
		view.skillVM.picto = picto;
	}
	public void show()
	{
		this.initStyles ();
		this.defineSkillPopUpPositions ();
		view.show ();
	}
	public void initStyles()
	{
		view.skillVM.styles=new GUIStyle[this.skillVMStyle.Length];
		for(int i=0;i<this.skillVMStyle.Length;i++)
		{
			view.skillVM.styles[i]=this.skillVMStyle[i];
		}
		view.skillVM.initStyles();
	}
	public void setTextResolution(float resolution)
	{
		view.setTextResolution (resolution);
	}
	public void defineSkillPopUpPositions()
	{
		base.getGOCoordinates (gameObject);
		float tempX=base.GOPosition.x-125;
		if(base.GOPosition.x-125<0)
		{
			tempX=0;
		}
		else if(base.GOPosition.x+250>Screen.width)
		{
			tempX=Screen.width-250;
		}
		view.skillVM.popUpPosition = new Rect(tempX, Screen.height-base.GOPosition.y+base.GOSize.y/2, 250, 400);
	}
}

