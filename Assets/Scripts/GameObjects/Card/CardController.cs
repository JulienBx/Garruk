using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class CardController : MonoBehaviour {

	public Texture[] frontFaces; 								
	public Texture[] backFaces; 
	public Texture[] leftFaces;
	public Texture[] rightFaces;
	public Texture[] topFaces;
	public Texture[] bottomFaces;
	public Texture[] skillsPictos;
	public Texture[] metals;
	public Texture[] Areas;
	public GameObject skillAreaObject;
	public GameObject experienceAreaObject;

	public Card card;

	private CardView view;
	private IList<GameObject> skills;
	private GameObject experience;

	void Awake () 
	{	
		this.view = gameObject.AddComponent <CardView>();
	}
	public void setCard(Card c)
	{
		this.card = c;
		view.cardVM.attackArea = this.Areas [0];
		view.cardVM.speedArea = this.Areas [1];
		view.cardVM.moveArea = this.Areas [2];
		view.cardVM.cardFaces = new Texture[]{this.frontFaces [c.ArtIndex],
										      this.backFaces [0],
											  this.leftFaces [0],
											  this.rightFaces [0],
				                              this.topFaces [0],
											  this.bottomFaces [0]};
		view.cardVM.title = c.Title;
		view.cardVM.attack = c.Attack;
		view.cardVM.life = c.Life;
		view.cardVM.move = c.Move;
		view.cardVM.speed = c.Speed;
		view.cardVM.titleClass = c.TitleClass;
		for (int i = 0 ; i < 6 ; i++)
		{		
			view.cardVM.lifeLevel[i]=metals[c.LifeLevel];
			view.cardVM.attackLevel[i]=metals[c.AttackLevel];
			view.cardVM.speedLevel[i]=metals[c.SpeedLevel];
			view.cardVM.moveLevel[i]=metals[c.MoveLevel];
		}
		this.skills = new List<GameObject> ();
	}
	public void setSkills()
	{
		for (int i = 0 ; i < this.card.Skills.Count ; i++) 
		{
			if (card.Skills[i].IsActivated == 1) 
			{
				this.skills.Add (Instantiate(skillAreaObject) as GameObject);
				this.skills[skills.Count-1].name="Skill"+(i+1);
				this.skills[skills.Count-1].transform.parent=gameObject.transform.Find("texturedGameCard");
				this.skills[skills.Count-1].transform.localPosition=new Vector3(0,-0.12f-0.08f*(skills.Count-1),-0.5f);
				this.skills[skills.Count-1].transform.localScale=new Vector3(0.9f, 0.07f, 20f);
				this.skills[skills.Count-1].GetComponent<SkillController>().setSkill(card.Skills[i]);
				this.skills[skills.Count-1].GetComponent<SkillController>().setSkillLevelMetals(this.metals[card.Skills[i].Level]);
				this.skills[skills.Count-1].GetComponent<SkillController>().setSkillPicto(this.skillsPictos[card.Skills[i].Id]);
			}
		}
	}
	public void setExperience()
	{
		this.experience=Instantiate(experienceAreaObject) as GameObject;
		this.experience.name = "experienceArea";
		this.experience.transform.parent=gameObject.transform.Find("texturedGameCard");
		this.experience.transform.localPosition=new Vector3(0f,0f,0f);
		this.experience.transform.localScale=new Vector3(1f, 1f, 1f);
		this.updateExperience ();
	}
	public void updateExperience()
	{
		this.experience.GetComponent<ExperienceController> ().setXp (this.card.getXpLevel(),this.card.percentageToNextXpLevel());
	}
	public void animateExperience()
	{
		this.experience.GetComponent<ExperienceController> ().animateXp (this.card.getXpLevel(),this.card.percentageToNextXpLevel());
	}
	public void show()
	{
		this.setTextResolution ();
		view.show ();
		for (int i=0;i<skills.Count;i++)
		{
			this.skills[i].GetComponent<SkillController>().show();
		}
		if(this.experience!=null)
		{
			this.experience.GetComponent<ExperienceController> ().show ();
		}
	}
	public void setTextResolution()
	{
		Vector3 tempVector = new Vector3 (0, gameObject.transform.Find("texturedGameCard").renderer.bounds.max.y-gameObject.transform.Find("texturedGameCard").renderer.bounds.min.y, 0);
		tempVector= Camera.main.camera.WorldToScreenPoint(tempVector);
		float resolution = tempVector.y / 300f;
		view.setTextResolution (resolution);
		for (int i=0;i<skills.Count;i++)
		{
			this.skills[i].GetComponent<SkillController>().setTextResolution(resolution);
		}
		if(this.experience!=null)
		{
			this.experience.GetComponent<ExperienceController> ().setTextResolution (resolution);
		}
	}
	public void resize()
	{
		this.setTextResolution ();
		for (int i=0;i<skills.Count;i++)
		{
			this.skills[i].GetComponent<SkillController>().resize();
		}
	}
}

