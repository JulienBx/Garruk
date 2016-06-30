using System;
using UnityEngine;
using TMPro;

public class InterludeC : MonoBehaviour
{
	public InterludeC ()
	{
		
	}

	public void size(float realwidth){
		gameObject.transform.FindChild("Bar1").localScale = new Vector3(1f*realwidth/20f, 1f*realwidth/20f, 1f*realwidth/20f);
		Vector3 position = gameObject.transform.FindChild("Bar1").localPosition ;
		position.y = 0.75f*(realwidth/20f);
		gameObject.transform.FindChild("Bar1").localPosition = position;

		gameObject.transform.FindChild("Bar2").localScale = new Vector3(1f*realwidth/20f, 1f*realwidth/20f, 1f*realwidth/20f);

		gameObject.transform.FindChild("Bar3").localScale = new Vector3(1f*realwidth/20f, 1f*realwidth/20f, 1f*realwidth/20f);
		position = gameObject.transform.FindChild("Bar3").localPosition ;
		position.y = -0.75f*(realwidth/20f);
		gameObject.transform.FindChild("Bar3").localPosition = position;

		gameObject.transform.FindChild("Text").GetComponent<TextContainer>().width = realwidth ;
		gameObject.transform.FindChild("Text").GetComponent<TextContainer>().height = 2*(realwidth/20f) ;
	}
}

