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
		if (this.skillVM.isActive){
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
					GUILayout.FlexibleSpace();
					GUILayout.Label(this.skillVM.skillTitle, this.skillVM.skillTitleTextStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label(this.skillVM.skillDescription, this.skillVM.skillDescriptionTextStyle);
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
		}
	}

	void OnMouseEnter()
	{
		this.skillVM.toDisplayInfo = true ;
	}

	void OnMouseExit()
	{
		this.skillVM.toDisplayInfo = false ;
	}
}
