using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class NewFocusedCardExperienceController : MonoBehaviour 
{
	private Vector3 destination;
	private bool toUpdateXp;
	private int startLevel;
	private float startPercentage;
	private int currentLevel;
	private float currentPercentage;
	private int endLevel;
	private float endPercentage;
	private bool hasLevelChanged;
	private float scaleSpeed;
	private float nbIteration;
	
	public virtual void Update ()
	{
		if(toUpdateXp)
		{
			float increase = scaleSpeed * Time.deltaTime;
			
			if (this.currentLevel!=this.endLevel || this.currentPercentage!=this.endPercentage)
			{
				if(this.currentPercentage==1f)
				{
					if (this.currentLevel!=9)
					{
						this.currentPercentage=0;
					}
					this.currentLevel=this.currentLevel+1;
					this.updateExperienceLevel();
				}
				else
				{
					this.currentPercentage=this.currentPercentage+increase;
					if (this.currentPercentage>1f)
					{
						this.currentPercentage=1f;
					}
					if(this.currentLevel==this.endLevel)
					{
						if(this.currentPercentage>this.endPercentage)
						{
							this.currentPercentage=this.endPercentage;
						}
					}
				}
				this.updateGauge(this.currentPercentage);
			}
			else 
			{	
				this.setToUpdateXp(false);
				gameObject.transform.parent.GetComponent<NewFocusedCardController>().endUpdatingXp ();
			}
		}
	}
	public virtual void setExperience(int Level, int Percentage)
	{
		this.currentLevel = Level;
		this.currentPercentage = 0.01f*(float)Percentage;
		this.updateGauge (this.currentPercentage);
		this.updateExperienceLevel ();
	}
	public virtual void updateExperienceLevel()
	{
		this.gameObject.transform.FindChild("ExperienceLevel").GetComponent<TextMeshPro>().text = "Niveau "+currentLevel.ToString();
	}
	public virtual void startUpdatingXp(int endLevel, int endPercentage)
	{
		this.startLevel = this.currentLevel;
		this.startPercentage = this.currentPercentage;
		this.endLevel = endLevel;
		this.endPercentage = 0.01f*(float)endPercentage;
		this.setToUpdateXp(false);
		if(this.endLevel!=this.startLevel)
		{
			this.hasLevelChanged=true;
		}
		if(this.endLevel!=10)
		{
			this.nbIteration=this.endLevel+(float)this.endPercentage-(float)this.startLevel-this.startPercentage;
		}
		else
		{
			this.nbIteration=10f-(float)this.startLevel-this.startPercentage;
		}
		this.scaleSpeed = 0.5f*nbIteration;
		this.setToUpdateXp(true);
	}
	public virtual void updateGauge(float currentPercentage)
	{
		this.gameObject.transform.FindChild ("ExperienceGauge").localPosition = new Vector3 (-0.216f+currentPercentage*0.578f, 0f, 0);
		this.gameObject.transform.FindChild ("ExperienceGauge").localScale = new Vector3 (currentPercentage*1.26f, 1.25f, 1.25f);
	}
	public void setToUpdateXp(bool value)
	{
		this.toUpdateXp=value;
	}
}

