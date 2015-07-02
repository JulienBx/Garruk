using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeftLifeBarController : GameObjectController
{
	float percentageLife = 100 ;
	
	public void resize(int w, int h)
	{
		GameObject LS = GameObject.Find("LLB/Template/LeftSide");
		GameObject C = GameObject.Find("LLB/Template/Center");
		GameObject RS = GameObject.Find("LLB/Template/RightSide");
		
		GameObject LBC = GameObject.Find("LLB/LifeBar/Center");
		GameObject LBLS = GameObject.Find("LLB/LifeBar/LeftSide");
		
		float realWidth = w*10f/h;
		Vector3 scale = new Vector3(((realWidth-2f)/22f), 1f, 0.05f);
		float ratioScale = scale.x/scale.z;
		Vector3 position = new Vector3((-0.25f*realWidth-0.50f), 4.50f, -1f);
		
		gameObject.transform.position = position;
		gameObject.transform.localScale = scale;
		
		LS.transform.localScale = new Vector3(0.465517f/ratioScale,1,1);
		LS.transform.localPosition = new Vector3(5.044f-0.465517f/2, 0, 0);
		
		C.transform.localPosition = new Vector3(0, 0, 0);
		C.transform.localScale = new Vector3(0.93f, 1f, 1f);
		
		RS.transform.localScale = new Vector3(0.327586f/ratioScale, 1, 1);
		RS.transform.localPosition = new Vector3(-4.925f+0.327586f/2, 0, 0);
		
		LBC.transform.localPosition = new Vector3(-100, 0, 0);
		LBC.transform.localScale = new Vector3(0.5f, 1, 1);
		
		LBLS.transform.localPosition = new Vector3(-100, 0, 0);
		LBLS.transform.localScale = new Vector3(0.5f, 1, 1);
		
		
	}
}


