using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ExperienceView : MonoBehaviour
{	
	public ExperienceViewModel experienceVM;

	public ExperienceView ()
	{
		this.experienceVM = new ExperienceViewModel ();
	}
	public void updateGauge()
	{
		transform.FindChild ("ExperienceGauge").localScale = new Vector3 (transform.FindChild ("ExperienceGauge").localScale.x,
		                                                                  0.06f + experienceVM.currentPercentage * 0.27f,
		                                                                  transform.FindChild ("ExperienceGauge").localScale.z);
		
		transform.FindChild ("Percentage").GetComponent<TextMesh> ().text = Mathf.CeilToInt(100f*(experienceVM.currentPercentage)) + "%";
		
		transform.FindChild ("ExperienceGauge").localPosition = new Vector3 (transform.FindChild ("ExperienceGauge").localPosition.x,
		                                                                     transform.FindChild ("ExperienceGauge").localScale.y / 2 + 0.02f,
		                                                                     transform.FindChild ("ExperienceGauge").localPosition.z);
		
		transform.FindChild ("Percentage").localPosition = new Vector3 (transform.FindChild ("Percentage").localPosition.x,
		                                                                transform.FindChild ("ExperienceGauge").localScale.y - 0.01f,
		                                                                transform.FindChild ("Percentage").localPosition.z);
	}
	public void initializeGauge()
	{
		for (int i = 0 ; i < 6 ; i++)
		{
			transform.FindChild ("ExperienceGauge").renderer.materials[i].mainTexture = experienceVM.xpLevelTexture;
		}
		transform.FindChild("Level").GetComponent<TextMesh> ().text = "Lvl"+experienceVM.currentLevel ;
	}
	public void show()
	{
		for (int i = 0 ; i < 6 ; i++)
		{
			transform.FindChild ("ExperienceGauge").renderer.materials[i].mainTexture = experienceVM.xpLevelTexture;
		}
		
		gameObject.transform.FindChild("Level").GetComponent<TextMesh> ().text = "Lvl"+experienceVM.level ;
		
		gameObject.transform.FindChild ("Percentage").GetComponent<TextMesh> ().text = experienceVM.percentage + "%";

		gameObject.transform.FindChild ("ExperienceGauge").localScale = new Vector3 (transform.FindChild ("ExperienceGauge").localScale.x,
		                                                                             .06f + 0.01f * experienceVM.percentage * 0.27f,
		                                                                             transform.FindChild ("ExperienceGauge").localScale.z);
		
		gameObject.transform.FindChild ("ExperienceGauge").localPosition = new Vector3 (transform.FindChild ("ExperienceGauge").localPosition.x,
		                                                                                transform.FindChild ("ExperienceGauge").localScale.y / 2 + 0.02f,
		                                                                                transform.FindChild ("ExperienceGauge").localPosition.z);
		
		gameObject.transform.FindChild ("Percentage").localPosition = new Vector3 (transform.FindChild ("Percentage").localPosition.x,
		                                                                           transform.FindChild ("ExperienceGauge").localScale.y - 0.01f,
		                                                                           transform.FindChild ("Percentage").localPosition.z);
	}
	public void setTextResolution(float resolution)
	{
		gameObject.transform.FindChild("Percentage").GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(resolution * 15);	
		gameObject.transform.FindChild("Percentage").localScale = new Vector3(0.05f/resolution,0.04f/resolution,0);
		gameObject.transform.FindChild("Level").GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(resolution * 15);	
		gameObject.transform.FindChild("Level").localScale = new Vector3(0.06f/resolution,0.046f/resolution,0);
		
	}

}