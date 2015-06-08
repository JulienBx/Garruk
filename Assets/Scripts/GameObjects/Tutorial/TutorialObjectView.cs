using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class TutorialObjectView : MonoBehaviour
{
	public TutorialObjectViewModel VM;
	
	public TutorialObjectView ()
	{
		this.VM=new TutorialObjectViewModel();
	}
	void OnGUI()
	{
		GUI.depth = -1;
		GUILayout.BeginArea (VM.popUpRect);
		{
			GUILayout.BeginVertical(VM.windowStyle);
			{
				GUILayout.Label(VM.title,VM.titleStyle);
				GUILayout.Label(VM.description,VM.labelStyle);
				if(VM.displayNextButton)
				{
					GUILayout.Space(VM.popUpRect.height*0.05f);
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if(GUILayout.Button("Continuer",VM.buttonStyle,GUILayout.Width(VM.popUpRect.width*0.4f)))
						{
							TutorialObjectController.instance.nextStepHandler();
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.Space(VM.popUpRect.height*0.05f);
			}
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea ();

		if(VM.displayArrow)
		{
			GUILayout.BeginArea(VM.arrowRect,VM.arrowStyle);
			{
			}
			GUILayout.EndArea();
		}
	}

}

