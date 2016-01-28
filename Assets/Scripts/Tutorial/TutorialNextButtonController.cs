using UnityEngine;
using TMPro;

public class TutorialNextButtonController : MonoBehaviour 
{
	
	void OnMouseOver()
	{
		if(ApplicationDesignRules.isMobileScreen)
		{
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
			gameObject.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
		}
	}
	void OnMouseExit()
	{
		if(ApplicationDesignRules.isMobileScreen)
		{
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
			gameObject.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		}
	}
	void OnMouseDown()
	{
		TutorialController.instance.nextButtonHandler();
	}
	
}

