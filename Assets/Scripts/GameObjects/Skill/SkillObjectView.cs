using UnityEngine ;
using System.Collections;
using System.Collections.Generic;

public class SkillObjectView : MonoBehaviour
{
	public SkillObjectViewModel skillVM;
	
	public SkillObjectView ()
	{
		this.skillVM = new SkillObjectViewModel ();
	}

	public void show()
	{
		transform.renderer.materials[1].mainTexture = skillVM.face; 
	}
}
