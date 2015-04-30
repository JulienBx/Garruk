using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SkillView : MonoBehaviour
{	
	public SkillViewModel skillVM;
	private int skillId;
	private bool isHovered=false;

	void OnMouseOver() 
	{
		isHovered = true;
	}
	void OnMouseExit() 
	{
		isHovered = false;
	}
	void OnGUI () {
		if (isHovered){

			GUI.depth = skillVM.guiDepth;
			GUILayout.BeginArea(skillVM.popUpPosition);
			{
				GUILayout.BeginVertical(skillVM.centralWindowStyle);
				{
					GUILayout.Label ("Description de " + skillVM.name,skillVM.titleStyle);
					GUILayout.Space (4);
					GUILayout.Label (skillVM.description,skillVM.descriptionStyle);
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea();
		}
	}
	public SkillView ()
	{
		this.skillVM = new SkillViewModel ();
	}
	public void show()
	{
		for (int j = 0 ; j < 6 ; j++)
		{
			gameObject.transform.FindChild("PictoMetalSkill").renderer.materials[j].mainTexture = skillVM.skillLevel[j];
		}
		gameObject.transform.FindChild ("Skill").GetComponent<TextMesh> ().text = skillVM.name;
		gameObject.transform.FindChild ("PictoSkill").renderer.material.mainTexture = skillVM.picto;
		gameObject.transform.FindChild ("PictoMetalSkill").FindChild ("SkillForce").GetComponent<TextMesh> ().text = skillVM.power + "/" +skillVM.manaCost ;
	}
	public void setTextResolution(float resolution)
	{
		gameObject.transform.FindChild ("Skill").GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(resolution * 12);	
		gameObject.transform.FindChild ("Skill").localScale = new Vector3(0.06f/resolution,0.6f/resolution,0);
		gameObject.transform.FindChild ("PictoMetalSkill").FindChild ("SkillForce").GetComponent<TextMesh> ().fontSize = Mathf.RoundToInt(resolution * 12);	
		gameObject.transform.FindChild ("PictoMetalSkill").FindChild ("SkillForce").localScale = new Vector3(0.3f/resolution,0.7f/resolution,0);
		//Vector2 size = GUI.skin.GetStyle("ProgressBarText").CalcSize(GUIContent(label));
	}
}

