using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillObjectViewModel
{
	public Texture face;
	public Vector3 position;
	public Vector3 scale;
	public Texture2D border ;

	public bool isActive ;

	public SkillObjectViewModel()
	{
		this.isActive = false ;
	}
}

