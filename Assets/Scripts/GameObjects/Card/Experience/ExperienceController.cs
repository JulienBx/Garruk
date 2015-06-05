using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ExperienceController : MonoBehaviour {

	private ExperienceView view;

	public Texture[] xplevels; 	
	public GameObject addXpGO;

	private bool toMove;
	private bool hasLevelChanged;
	private GameObject animation;
	private Vector3 destination;
	private float scaleSpeed;
	private float animationRatio;
	private float nbIteration;
	
	void Awake () 
	{	
		this.view = gameObject.AddComponent <ExperienceView>();
	}
	void Update ()
	{
		if(toMove)
		{
			float increase = scaleSpeed * Time.deltaTime;

			if (view.experienceVM.currentLevel!=view.experienceVM.endLevel || view.experienceVM.currentPercentage!=view.experienceVM.endPercentage)
			{
				if(view.experienceVM.currentPercentage==1f)
				{
					if (view.experienceVM.currentLevel!=9)
					{
						view.experienceVM.currentPercentage=0;
					}
					view.experienceVM.currentLevel=view.experienceVM.currentLevel+1;
					view.experienceVM.xpLevelTexture=this.xplevels[view.experienceVM.currentLevel];
					view.initializeGauge();
				}
				else
				{
					view.experienceVM.currentPercentage=view.experienceVM.currentPercentage+increase;
					if (view.experienceVM.currentPercentage>1f)
					{
						view.experienceVM.currentPercentage=1f;
					}
					if(view.experienceVM.currentLevel==view.experienceVM.endLevel)
					{
						if(view.experienceVM.currentPercentage>view.experienceVM.endPercentage)
						{
							view.experienceVM.currentPercentage=view.experienceVM.endPercentage;
						}
					}
				}
				view.updateGauge();
				this.animationRatio=((float)view.experienceVM.currentLevel+view.experienceVM.currentPercentage-(float)view.experienceVM.startLevel-view.experienceVM.startPercentage)/this.nbIteration;
				if(this.animationRatio>1f)
				{
					this.animationRatio=1;
				}
				this.animation.transform.localScale = new Vector3(this.animationRatio,this.animationRatio,this.animation.transform.localScale.z);
			}
			else 
			{	
				Destroy (animation);
				this.toMove=false;
				gameObject.transform.parent.transform.parent.transform.GetComponent<CardController>().updateExperience ();
				if(hasLevelChanged)
				{
					gameObject.transform.parent.transform.parent.transform.GetComponent<CardController>().updateCardXpLevel ();
				}
			}
		}
	}
	public void show()
	{
		view.show ();
	}
	public void setTextResolution(float resolution)
	{
		view.setTextResolution (resolution);
	}
	public void setXp(int level, int percentage)
	{
		view.experienceVM.level = level;
		view.experienceVM.percentage = percentage;
		view.experienceVM.xpLevelTexture = this.xplevels [level];
	}
	public void animateXp(int level, int percentage)
	{
		view.experienceVM.currentLevel=view.experienceVM.level;
		view.experienceVM.currentPercentage=0.01f*(float)view.experienceVM.percentage;
		view.experienceVM.startLevel = view.experienceVM.level;
		view.experienceVM.startPercentage = 0.01f*(float)view.experienceVM.percentage;
		view.experienceVM.endLevel = level;
		view.experienceVM.endPercentage = 0.01f*(float)percentage;
		if(view.experienceVM.endLevel!=view.experienceVM.startLevel)
		{
			this.hasLevelChanged=true;
		}
		
		if(level!=10)
		{
			this.nbIteration=view.experienceVM.endLevel+(float)view.experienceVM.endPercentage-(float)view.experienceVM.startLevel-view.experienceVM.startPercentage;
		}
		else
		{
			this.nbIteration=10f-(float)view.experienceVM.startLevel-view.experienceVM.startPercentage;
		}
		
		this.animation = Instantiate(this.addXpGO) as GameObject;
		this.animation.transform.parent = gameObject.transform.parent.transform.parent.transform;
		this.animation.transform.localScale=new Vector3(0f,0f,1f);
		this.animation.transform.position = new Vector3(transform.parent.transform.position.x,
		                                           transform.parent.transform.position.y,
		                                           transform.parent.transform.position.z-0.3f);
		this.scaleSpeed = 0.5f*nbIteration;
		this.toMove=true;
	}
}

