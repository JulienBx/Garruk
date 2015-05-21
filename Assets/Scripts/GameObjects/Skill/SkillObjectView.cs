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
	}

	void OnMouseEnter()
	{

	}
}
