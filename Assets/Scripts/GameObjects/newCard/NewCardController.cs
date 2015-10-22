using UnityEngine;
using TMPro;

public class NewCardController : NewFocusedCardController 
{
	private GameObject skillPopUp;
	private NewCardRessources ressources;
	private bool isSkillPopUpDisplayed;
	private int skillDisplayed;

	private int layerVariation;
	private string layerName;

	public override void Update()
	{
		base.Update ();
	}
	public override void Awake()
	{
		base.Awake ();
		this.skillDisplayed = -1;
		this.skillPopUp = this.gameObject.transform.FindChild ("SkillPopUp").gameObject;
	}
	public override void getRessources()
	{
		this.ressources = this.gameObject.GetComponent<NewCardRessources> ();
	}
	public override void show()
	{
		this.applyFrontTexture ();
		this.gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().text = this.c.Title.ToUpper();
		//this.gameObject.transform.FindChild("Power").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Power.ToString();
		//this.gameObject.transform.FindChild ("Face").FindChild ("Text").GetComponent<TextMeshPro> ().color = ressources.colors [this.c.PowerLevel - 1];
		this.gameObject.transform.FindChild("Life").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Life.ToString();
		this.gameObject.transform.FindChild ("Life").FindChild ("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors [this.c.LifeLevel - 1];
		//this.gameObject.transform.FindChild("Move").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Move.ToString();
		this.gameObject.transform.FindChild("Attack").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Attack.ToString();
		this.gameObject.transform.FindChild ("Attack").FindChild ("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors [this.c.AttackLevel - 1];
		this.gameObject.transform.FindChild("Quickness").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Speed.ToString();
		this.gameObject.transform.FindChild ("Quickness").FindChild ("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors [this.c.SpeedLevel - 1];
		//this.experience.GetComponent<NewCardExperienceController> ().setExperience (this.c.ExperienceLevel, this.c.PercentageToNextLevel);

		for(int i=0;i<this.skills.Length;i++)
		{
			if(i<this.c.Skills.Count && this.c.Skills[i].IsActivated==1)
			{
				this.skills[i].transform.GetComponent<NewCardSkillController>().setSkill(this.c.Skills[i]);
				this.skills[i].transform.GetComponent<NewCardSkillController>().setDescription(this.c.getSkillText(this.c.Skills[i].Description));
				this.skills[i].SetActive(true);
			}
			else
			{
				this.skills[i].SetActive(false);
			}
		}
	}
	public override void setExperience()
	{
		this.experience.GetComponent<NewCardExperienceController> ().setExperience (this.c.ExperienceLevel, this.c.PercentageToNextLevel);
	}
	public override void applyFrontTexture()
	{
		this.gameObject.transform.FindChild ("Caracter").GetComponent<SpriteRenderer> ().sprite = ressources.caracters[this.c.IdClass];
		this.gameObject.transform.FindChild ("Face").GetComponent<SpriteRenderer> ().sprite = ressources.faces [this.c.PowerLevel - 1];
	}
	public override void displayPanelSold()
	{
		this.panelSold.SetActive (true);
	}
	public override void hidePanelSold()
	{
		this.panelSold.SetActive (false);
	}
	public void changeLayer(int layerVariation, string layerName)
	{

		this.gameObject.transform.FindChild ("Face").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild ("Face").GetComponent<SpriteRenderer> ().sortingLayerName = layerName;
		this.gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().sortingOrder += layerVariation;

		int sortingLayerID = this.gameObject.transform.FindChild ("Face").GetComponent<SpriteRenderer> ().sortingLayerID;

		this.gameObject.transform.FindChild ("Caracter").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild ("Caracter").GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerID;

		this.gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().sortingLayerID = sortingLayerID;
		//this.gameObject.transform.FindChild("Power").FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		//this.gameObject.transform.FindChild("Power").FindChild("Text").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		//this.gameObject.transform.FindChild("Power").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		//this.gameObject.transform.FindChild("Power").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild("Life").FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Life").FindChild("Text").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild("Life").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Life").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerID;
		//this.gameObject.transform.FindChild("Move").FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		//this.gameObject.transform.FindChild("Move").FindChild("Text").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		//this.gameObject.transform.FindChild("Move").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		//this.gameObject.transform.FindChild("Move").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild("Attack").FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Attack").FindChild("Text").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild("Attack").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Attack").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild("Quickness").FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Quickness").FindChild("Text").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild("Quickness").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Quickness").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild("Experience").FindChild ("ExperienceGauge").GetComponent<SpriteRenderer>().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Experience").FindChild ("ExperienceGauge").GetComponent<SpriteRenderer>().sortingLayerID = sortingLayerID;
		//this.gameObject.transform.FindChild("Experience").FindChild ("ExperienceBar").GetComponent<SpriteRenderer>().sortingOrder += layerVariation;
		//this.gameObject.transform.FindChild("Experience").FindChild ("ExperienceBar").GetComponent<SpriteRenderer>().sortingLayerID = sortingLayerID;
		//this.gameObject.transform.FindChild("Experience").FindChild("ExperienceLevel").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		//this.gameObject.transform.FindChild("Experience").FindChild("ExperienceLevel").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild("CardUpgrade").FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("CardUpgrade").FindChild("Text").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild("CardUpgrade").GetComponent<SpriteRenderer>().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("CardUpgrade").GetComponent<SpriteRenderer>().sortingLayerID = sortingLayerID;

		for(int i=0;i<this.skills.Length;i++)
		{
			this.skills[i].transform.GetComponent<NewCardSkillController>().changeLayer(layerVariation,sortingLayerID);
		}
	} 
	public override void applyBackTexture()
	{
		this.gameObject.transform.FindChild ("Face").GetComponent<SpriteRenderer> ().sprite = ressources.backFace;
	}
	public virtual void OnMouseOver()
	{
		if(!Input.GetMouseButton(0))
		{
			int newSkillHovered = this.skillHovered();
			if(newSkillHovered>-1 && newSkillHovered<this.c.Skills.Count && this.c.Skills[newSkillHovered].IsActivated==1)
			{
				if(newSkillHovered!=skillDisplayed)
				{
					this.skillDisplayed=newSkillHovered;
					if(!isSkillPopUpDisplayed)
					{
						this.isSkillPopUpDisplayed=true;
						this.skillPopUp.SetActive(true);
						Vector3 popUpScale = new Vector3(1f/this.gameObject.transform.localScale.x,1f/this.gameObject.transform.localScale.y,1f/this.gameObject.transform.localScale.z);
						this.skillPopUp.transform.localScale=popUpScale;
					}
					if(newSkillHovered==0)
					{
						this.skillPopUp.transform.position=new Vector3(gameObject.transform.position.x,gameObject.transform.position.y-0.8f+(+0.6f)*(0.5f*this.gameObject.GetComponent<BoxCollider2D>().bounds.size.y),-1f);
					}
					else
					{
						this.skillPopUp.transform.position=new Vector3(gameObject.transform.position.x,gameObject.transform.position.y+0.8f+(-0.6f+(newSkillHovered-1f)*0.24f)*(0.5f*this.gameObject.GetComponent<BoxCollider2D>().bounds.size.y),-1f);
					}
					this.skillPopUp.transform.FindChild("title").GetComponent<TextMeshPro>().text=this.c.Skills[skillDisplayed].Name;
					this.skillPopUp.transform.FindChild("description").GetComponent<TextMeshPro>().text=this.c.getSkillText(this.c.Skills[skillDisplayed].Description);
				}
			}
			else
			{
				if(isSkillPopUpDisplayed)
				{
					this.hideSkillPopUp();
				}
			}
		}
		else if(isSkillPopUpDisplayed)
		{
			this.hideSkillPopUp();
		}
	}
	public virtual void OnMouseExit()
	{
		this.hideSkillPopUp ();
	}
	private void hideSkillPopUp()
	{
		if(isSkillPopUpDisplayed)
		{
			this.isSkillPopUpDisplayed=false;
			this.skillPopUp.SetActive(false);
			this.skillDisplayed=-1;
		}
	}

	public int skillHovered()
	{
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
		if(cursorPosition.x>this.gameObject.transform.position.x-(0.45f*this.gameObject.GetComponent<BoxCollider2D>().bounds.size.x) && 
		   cursorPosition.x<this.gameObject.transform.position.x+(0.45f*this.gameObject.GetComponent<BoxCollider2D>().bounds.size.x))
		{
			if(cursorPosition.y>this.gameObject.transform.position.y+(0.5f*0.50f*this.gameObject.GetComponent<BoxCollider2D>().bounds.size.y) && 
			   cursorPosition.y<this.gameObject.transform.position.y+(0.5f*0.70f*this.gameObject.GetComponent<BoxCollider2D>().bounds.size.y))
			{
				return 0;
			}
			else if(cursorPosition.y>this.gameObject.transform.position.y-(0.5f*0.30f*this.gameObject.GetComponent<BoxCollider2D>().bounds.size.y) && 
			        cursorPosition.y<this.gameObject.transform.position.y-(0.5f*0.10f*this.gameObject.GetComponent<BoxCollider2D>().bounds.size.y))
			{
				return 3;
			}
			else if(cursorPosition.y>this.gameObject.transform.position.y-(0.5f*0.50f*this.gameObject.GetComponent<BoxCollider2D>().bounds.size.y) && 
			        cursorPosition.y<this.gameObject.transform.position.y-(0.5f*0.30f*this.gameObject.GetComponent<BoxCollider2D>().bounds.size.y))
			{
				return 2;
			}
			else if(cursorPosition.y>this.gameObject.transform.position.y-(0.5f*0.70f*this.gameObject.GetComponent<BoxCollider2D>().bounds.size.y) && 
			        cursorPosition.y<this.gameObject.transform.position.y-(0.5f*0.50f*this.gameObject.GetComponent<BoxCollider2D>().bounds.size.y))
			{
				return 1;
			}
			else
			{
				return -1;
			}
		}
		else
		{
			return -1;
		}
	}
	public override void animateExperience()
	{
		this.experience.GetComponent<NewCardExperienceController>().startUpdatingXp(this.c.ExperienceLevel,this.c.PercentageToNextLevel);
	}
	public override void endUpdatingXp(bool hasChangedLevel)
	{
		this.show ();
		if(this.c.GetNewSkill)
		{
			base.setHighlightedSkills();
		}
	}
	public void endUpdatingCardToNextLevel()
	{
		this.hideNextLevelPopUp ();
		if(this.c.GetNewSkill)
		{
			this.c.GetNewSkill=false;
		}
		if(this.caracteristicUpgraded>-1&&this.caracteristicIncrease>0)
		{
			base.setCardUpgrade();
		}
	}
	public override Color getColors(int id)
	{
		return this.ressources.colors[id];	
	}
	public override Vector3 getCardUpgradePosition (int caracteristicUpgraded)
	{
		GameObject refObject = new GameObject ();
		float gap = 0.165f;
		switch(caracteristicUpgraded)
		{
		case 0:
			refObject = transform.FindChild("Life").FindChild("Text").gameObject;
			break;
		case 1:
			refObject=transform.FindChild("Attack").FindChild("Text").gameObject.gameObject;
			break;
		case 2:
			refObject=transform.FindChild("Move").FindChild("Text").gameObject.gameObject;
			break;
		case 3:
			refObject=transform.FindChild("Quickness").FindChild("Text").gameObject.gameObject;
			break;
		case 4:
			refObject=transform.FindChild("Skill0").FindChild ("Name").gameObject;
			break;
		case 5:
			refObject=transform.FindChild("Skill1").FindChild ("Name").gameObject;
			break;
		case 6:
			refObject=transform.FindChild("Skill2").FindChild ("Name").gameObject;
			break;
		case 7:
			refObject=transform.FindChild("Skill3").FindChild ("Name").gameObject;
			break;
		}
		Vector3 refPosition =refObject.transform.position;
		float refSizeX = refObject.transform.GetComponent<MeshRenderer> ().bounds.size.x;
		return new Vector3 (refPosition.x+gap+refSizeX/2f,refPosition.y,0f);
	}
	public virtual Sprite getSkillSprite(int id)
	{
		return this.ressources.skills [id];
	}
}

