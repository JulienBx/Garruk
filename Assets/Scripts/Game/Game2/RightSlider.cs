using System;
using UnityEngine;

public class RightSlider : Slider
{
	void Awake()
	{
		base.init();
	}

	public void putToStartPosition(float f){
		Vector3 position = gameObject.transform.localPosition;
		position.x = 0.50f*f+5f;
		this.startPositionX = position.x;
		gameObject.transform.localPosition = position;

		if(Game.instance.isMobile()){
			this.endPositionX = -0.5f*f+5f;
		}
		else{
			this.endPositionX = 8f;
		}
		this.resize(f);
	}
}
