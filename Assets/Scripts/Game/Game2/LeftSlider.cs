using System;
using UnityEngine;

public class LeftSlider : Slider
{
	void Awake()
	{
		
	}

	public void putToStartPosition(float f){
		Vector3 position = gameObject.transform.localPosition;
		position.x = -0.50f*f-5f;
		gameObject.transform.localPosition = position;	
	}
}


