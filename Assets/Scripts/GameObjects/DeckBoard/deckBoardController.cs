using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class deckBoardController : MonoBehaviour 
{
	
	public void resize(float ratio)
	{
		float positionX = this.gameObject.transform.position.x;
		if(ratio<1.5f)
		{
			float transformPercent = (1.5f-ratio)/0.25f;
			if(transformPercent>1f)
			{
				transformPercent=1f;
			}
			float scale = 1f-transformPercent*0.28f;
			float positionY=3.25f+transformPercent*0.5f;
			this.gameObject.transform.position=new Vector3(positionX,positionY,0f);
			this.gameObject.transform.localScale=new Vector3(scale,scale,scale);
		}
		else
		{
			this.gameObject.transform.position=new Vector3(positionX,3.25f,0f);
			this.gameObject.transform.localScale=new Vector3(1f,1f,1f);
		}
	}
}

