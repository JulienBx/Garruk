using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class NewCardExperienceController : NewFocusedCardExperienceController
{
	
	public override void Update ()
	{
		base.Update ();
	}
	public override void setExperience(int Level, int Percentage)
	{
		base.setExperience (Level, Percentage);
	}
	public override void updateExperienceLevel()
	{
	}
	public override void startUpdatingXp(int endLevel, int endPercentage)
	{
		base.startUpdatingXp (endLevel, endPercentage);
	}
	public override void updateGauge(float currentPercentage)
	{
		this.gameObject.transform.FindChild ("ExperienceGauge").localPosition = new Vector3 (-0.4f+currentPercentage*(0.54f), 0f, 0);
		this.gameObject.transform.FindChild ("ExperienceGauge").localScale = new Vector3 (currentPercentage*1.2f, 1.25f, 1.25f);
	}
	public override void endUpdatingXp(bool hasLevelChanged)
	{
		gameObject.transform.parent.GetComponent<NewFocusedCardController>().endUpdatingXp (hasLevelChanged);
	}
}

