using UnityEngine;
using System.Collections;

public class LifeBarsController : MonoBehaviour
{
	public GameObject lifeBarPanel;
	GameObject lifeBar;
	int maxLife = 50;
	int currentLife = 50;
	bool changeLife = false;
	RectTransform lifeBarRect;
	Vector2 MaskLifeBarsize;
	Vector2 changeLifeValueOrigMin, changeLifeValueOrigMax;
	Vector2 changeLifeValueDestMin, changeLifeValueDestMax;
	float timeSinceStarted = 0f;

	// Use this for initialization
	void Awake()
	{
		lifeBar = lifeBarPanel.transform.Find("LifeBarOutline/MaskLifeBar/LifeBar").gameObject;
		lifeBarRect = lifeBar.GetComponent<RectTransform>();
		MaskLifeBarsize = lifeBarPanel.transform.Find("LifeBarOutline/MaskLifeBar").GetComponent<RectTransform>().sizeDelta;
	}
	
	// Update is called once per frame
	void Update()
	{
	
		if (changeLife)
		{
			timeSinceStarted += Time.deltaTime;
			lifeBarRect.offsetMin = Vector3.Lerp(lifeBarRect.offsetMin, changeLifeValueDestMin, timeSinceStarted * 0.2f);
			lifeBarRect.offsetMax = Vector3.Lerp(lifeBarRect.offsetMax, changeLifeValueDestMax, timeSinceStarted * 0.2f);

			if (changeLifeValueDestMin.AlmostEquals(lifeBarRect.offsetMin, 0.1f))
			{
				changeLife = false;
				timeSinceStarted = 0f;
			}
		}
	}

	public void removeLife1(int dommage)
	{
		timeSinceStarted = Time.deltaTime;
		currentLife -= dommage;
		changeLifeValueDestMin = new Vector2((MaskLifeBarsize.x) * (1f - (float)currentLife / maxLife), lifeBarRect.offsetMin.y);
		changeLifeValueDestMax = new Vector2((MaskLifeBarsize.x) * (1f - (float)currentLife / maxLife), lifeBarRect.offsetMax.y);
		changeLife = true;
	}
}
