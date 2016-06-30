using System;
using UnityEngine;

public class RightSlider : Slider
{
	void Awake()
	{
		
	}

	public void putToStartPosition(float f){
		Vector3 position = gameObject.transform.localPosition;
		position.x = -0.50f*f+5f;
		gameObject.transform.localPosition = position;	
	}
}
