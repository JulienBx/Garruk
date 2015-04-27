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
			Vector2 position = this.getScreenPosition(gameObject);
			Vector2 size = this.getScreenSize (gameObject);
			view.skillVM.popUpPosition = new Rect(position.x-125, Screen.height-position.y+size.y/2, 250, 250);
	}
	private Vector2 getScreenPosition(GameObject gameObject)
	{
		Vector2 position = new Vector2 (gameObject.transform.position.x,gameObject.transform.position.y);
		float worldHeight = 2f*Camera.main.camera.orthographicSize;
		float worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		position.x = (worldWidth / 2f + position.x) * (float)Screen.width / worldWidth;
		position.y = (worldHeight / 2f + position.y) * (float)Screen.height / worldHeight;
		return position;
	}
	private Vector2 getScreenSize(GameObject gameObject)
	{
		Vector2 size = new Vector2 (gameObject.GetComponent<Renderer> ().bounds.size.x,gameObject.GetComponent<Renderer> ().bounds.size.y);
		float worldHeight = 2f*Camera.main.camera.orthographicSize;
		float worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		size.x = (size.x / worldWidth) * (float)Screen.width;
		size.y = (size.y / worldHeight) * (float)Screen.height;
		return size;
	}
}

