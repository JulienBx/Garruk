//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Linq;
//
//public class SkillBookStatsViewModel
//{
//	public GUIStyle[] styles;
//	public GUIStyle[] stars;
//	public GUIStyle titleStyle;
//
//	
//	public SkillBookStatsViewModel ()
//	{
//		this.stars=new GUIStyle[5];
//		for(int i =0;i<5;i++)
//		{
//			this.stars[i]=new GUIStyle();
//		}
//		this.titleStyle = new GUIStyle ();
//	}
//	public void initStyles()
//	{
//		this.titleStyle = this.styles [0];
//	}
//	public void resize(int heightScreen)
//	{
//		this.titleStyle.fontSize = heightScreen * 3 / 100;
//	}
//}
//
