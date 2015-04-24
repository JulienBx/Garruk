using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class SkillController : MonoBehaviour {

	public GUIStyle[] skillVMStyle;
	private SkillView view;
	
	void Awake () 
	{	
		this.view = gameObject.AddComponent <SkillView>();
	}
	public void resize()
	{
		this.defineSkillPopUpPositions ();
	}
	public void setSkill(Skill skill)
	{
		view.skillVM.number=skill.Id;
		view.skillVM.name = skill.Name;
		view.skillVM.description = skill.Description;
		view.skillVM.power = skill.Power;
		view.skillVM.manaCost = skill.ManaCost;
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
			Vector3 screenPos = new Vector3(gameObject.transform.position.x,gameObject.renderer.bounds.min.y,0);
			screenPos = Camera.main.camera.WorldToScreenPoint(screenPos);
			view.skillVM.popUpPosition = new Rect(screenPos.x-125, Screen.height-screenPos.y, 250, 250);
	}
}

