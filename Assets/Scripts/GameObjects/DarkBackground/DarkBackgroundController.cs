using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DarkBackgroundController : GameObjectController
{
	
	public void resize()
	{
		float yScale = Camera.main.camera.orthographicSize*2;
		float zPosition = Camera.main.camera.transform.localPosition.z+2f;
		float xScale = ((float)Screen.width / (float)Screen.height) * yScale;
		gameObject.transform.localPosition = new Vector3 (0, 0, zPosition);
		gameObject.transform.localScale = new Vector3 (xScale, yScale, 1);
	}

}


