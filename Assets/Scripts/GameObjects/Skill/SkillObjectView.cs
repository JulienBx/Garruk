using UnityEngine ;
using System.Collections;
using System.Collections.Generic;

public class SkillObjectView : MonoBehaviour
{
	public SkillObjectViewModel skillVM;
	
	public SkillObjectView()
	{
		this.skillVM = new SkillObjectViewModel();
	}

	public void show()
	{
		transform.renderer.materials [1].mainTexture = skillVM.face;
		transform.renderer.materials [0].mainTexture = skillVM.border; 
	}

	public void changeBorder()
	{
		transform.renderer.materials [0].mainTexture = skillVM.border; 
	}

	public void replace()
	{
		gameObject.transform.localPosition = skillVM.position;
		gameObject.transform.localScale = skillVM.scale;
	}


	void OnMouseDown()
	{

	}

	void OnMouseUp()
	{
		if (this.skillVM.isControlActive){
			gameObject.GetComponentInChildren<SkillObjectController>().clickSkill();
		}
		else{
			print ("Bouton inactif");
		}
	}

	void OnGUI()
	{
		if (this.skillVM.toDisplayInfo){
			GUILayout.BeginArea(this.skillVM.skillRect, this.skillVM.skillRectStyle);
			{
				GUILayout.BeginVertical();
				{
					GUILayout.Label(this.skillVM.skillDescription, this.skillVM.skillDescriptionTextStyle);
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
		}

		GUILayout.BeginArea(this.skillVM.habillageRect);
		{
			GUILayout.BeginVertical();
			{
				GUILayout.Label(" ", this.skillVM.powerStyle, GUILayout.Width(this.skillVM.habillageRect.width*this.skillVM.power/100), GUILayout.Height(this.skillVM.habillageRect.height));
				GUILayout.Space(-this.skillVM.habillageRect.height);
				GUILayout.Label(this.skillVM.skillName, this.skillVM.cadreStyle, GUILayout.Height(this.skillVM.habillageRect.height));
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndArea();
	}

	void OnMouseEnter()
	{
		this.skillVM.toDisplayInfo = true ;
		gameObject.GetComponentInChildren<SkillObjectController>().hoverSkill();
	}

	void OnMouseExit()
	{
		this.skillVM.toDisplayInfo = false ;
		gameObject.GetComponentInChildren<SkillObjectController>().endHoverSkill();
	}
}
