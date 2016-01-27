using UnityEngine;
using TMPro;

public class NewCardController : NewFocusedCardController 
{
	private NewCardRessources cardRessources;
	private bool isCardSkillPopUpDisplayed;
	private int skillDisplayed;

	private int layerVariation;
	private string layerName;

	private GameObject buyPopUp;

	public override void Update()
	{
		base.Update ();
	}
	public override void Awake()
	{
		base.Awake ();
		this.skillDisplayed = -1;
	}
	public override void getCardsComponents()
	{
		this.experience = this.gameObject.transform.FindChild ("Experience").gameObject;
		for(int i=0;i<this.skills.Length;i++)
		{
			this.skills[i]=this.gameObject.transform.FindChild("Skill"+i).gameObject;
		}
		this.name = this.gameObject.transform.FindChild ("Name").gameObject;
		this.attack = this.gameObject.transform.FindChild ("Attack").gameObject;
		this.life = this.gameObject.transform.FindChild ("Life").gameObject;
		this.quickness = this.gameObject.transform.FindChild ("Quickness").gameObject;
		this.face = this.gameObject.transform.FindChild ("Face").gameObject;
		this.caracter = this.gameObject.transform.FindChild ("Caracter").gameObject;
		this.card = this.gameObject;
	}
	public override void getRessources()
	{
		this.cardRessources = this.gameObject.GetComponent<NewCardRessources> ();
	}
	public override void show()
	{
		this.applyFrontTexture ();
		this.name.transform.GetComponent<TextMeshPro> ().text = this.c.Title.ToUpper();
		//this.gameObject.transform.FindChild("Power").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Power.ToString();
		//this.gameObject.transform.FindChild ("Face").FindChild ("Text").GetComponent<TextMeshPro> ().color = cardRessources.colors [this.c.PowerLevel - 1];
		this.life.transform.FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Life.ToString();
		this.life.transform.FindChild ("Picto").GetComponent<SpriteRenderer> ().color = cardRessources.colors [this.c.LifeLevel - 1];
		//this.gameObject.transform.FindChild("Move").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Move.ToString();
		this.attack.transform.FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Attack.ToString();
		this.attack.transform.FindChild ("Picto").GetComponent<SpriteRenderer> ().color = cardRessources.colors [this.c.AttackLevel - 1];
		this.quickness.transform.FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Speed.ToString();
		this.quickness.transform.FindChild ("Picto").GetComponent<SpriteRenderer> ().color = cardRessources.colors [this.c.SpeedLevel - 1];
		//this.experience.GetComponent<NewCardExperienceController> ().setExperience (this.c.ExperienceLevel, this.c.PercentageToNextLevel);

		for(int i=0;i<this.skills.Length;i++)
		{
			if(i<this.c.Skills.Count && this.c.Skills[i].IsActivated==1)
			{
				this.skills[i].transform.GetComponent<NewCardSkillController>().setSkill(this.c.Skills[i]);
				this.skills[i].transform.GetComponent<NewCardSkillController>().setDescription(this.c.getSkillText(WordingSkills.getDescription(this.c.Skills[i].Id,this.c.Skills[i].Level)));
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
		this.caracter.GetComponent<SpriteRenderer> ().sprite = cardRessources.caracters[this.c.IdClass];
		this.face.GetComponent<SpriteRenderer> ().sprite = cardRessources.faces [this.c.PowerLevel - 1];
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

		this.face.GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.face.GetComponent<SpriteRenderer> ().sortingLayerName = layerName;
		this.name.GetComponent<TextMeshPro> ().sortingOrder += layerVariation;

		int sortingLayerID = this.gameObject.transform.FindChild ("Face").GetComponent<SpriteRenderer> ().sortingLayerID;

		this.caracter.GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.caracter.GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerID;

		this.name.GetComponent<TextMeshPro> ().sortingLayerID = sortingLayerID;
		//this.gameObject.transform.FindChild("Power").FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		//this.gameObject.transform.FindChild("Power").FindChild("Text").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		//this.gameObject.transform.FindChild("Power").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		//this.gameObject.transform.FindChild("Power").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerID;
		this.life.transform.FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.life.transform.FindChild("Text").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		this.life.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.life.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerID;
		//this.gameObject.transform.FindChild("Move").FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		//this.gameObject.transform.FindChild("Move").FindChild("Text").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		//this.gameObject.transform.FindChild("Move").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		//this.gameObject.transform.FindChild("Move").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerID;
		this.attack.transform.FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.attack.transform.FindChild("Text").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		this.attack.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.attack.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerID;
		this.quickness.transform.FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.quickness.transform.FindChild("Text").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		this.quickness.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.quickness.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerID;
		this.experience.transform.FindChild ("ExperienceGauge").GetComponent<SpriteRenderer>().sortingOrder += layerVariation;
		this.experience.transform.FindChild ("ExperienceGauge").GetComponent<SpriteRenderer>().sortingLayerID = sortingLayerID;
		//this.gameObject.transform.FindChild("Experience").FindChild ("ExperienceBar").GetComponent<SpriteRenderer>().sortingOrder += layerVariation;
		//this.gameObject.transform.FindChild("Experience").FindChild ("ExperienceBar").GetComponent<SpriteRenderer>().sortingLayerID = sortingLayerID;
		//this.gameObject.transform.FindChild("Experience").FindChild("ExperienceLevel").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		//this.gameObject.transform.FindChild("Experience").FindChild("ExperienceLevel").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		this.cardUpgrade.transform.FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.cardUpgrade.transform.FindChild("Text").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		this.cardUpgrade.GetComponent<SpriteRenderer>().sortingOrder += layerVariation;
		this.cardUpgrade.GetComponent<SpriteRenderer>().sortingLayerID = sortingLayerID;

		for(int i=0;i<this.skills.Length;i++)
		{
			this.skills[i].transform.GetComponent<NewCardSkillController>().changeLayer(layerVariation,sortingLayerID);
		}
	} 
	public override void applyBackTexture()
	{
		this.face.GetComponent<SpriteRenderer> ().sprite = cardRessources.backFace;
	}
	public virtual void OnMouseOver()
	{
		if(!Input.GetMouseButton(0))
		{
			int newSkillHovered = this.skillHovered();
			float skillPopUpWorldSize=0f;
			float skillPopUpXPosition=0f;
			if(newSkillHovered>-1 && newSkillHovered<this.c.Skills.Count && this.c.Skills[newSkillHovered].IsActivated==1)
			{
				if(newSkillHovered!=skillDisplayed)
				{
					this.skillDisplayed=newSkillHovered;
					if(!isCardSkillPopUpDisplayed)
					{
						this.isCardSkillPopUpDisplayed=true;
						this.skillPopUp.SetActive(true);
						Vector3 popUpScale = new Vector3(1f/this.gameObject.transform.localScale.x,1f/this.gameObject.transform.localScale.y,1f/this.gameObject.transform.localScale.z);
						this.skillPopUp.transform.localScale=popUpScale;
					}

					skillPopUpWorldSize=this.skillPopUp.GetComponent<SpriteRenderer>().bounds.size.x;

					if(this.gameObject.transform.position.x-skillPopUpWorldSize/2f<-ApplicationDesignRules.worldWidth/2f)
					{
						skillPopUpXPosition=this.gameObject.transform.position.x-(this.gameObject.transform.position.x-skillPopUpWorldSize/2f+ApplicationDesignRules.worldWidth/2f);
					}
					else if(this.gameObject.transform.position.x+skillPopUpWorldSize/2f>ApplicationDesignRules.worldWidth/2f)
					{
						skillPopUpXPosition=this.gameObject.transform.position.x-(this.gameObject.transform.position.x+skillPopUpWorldSize/2f-ApplicationDesignRules.worldWidth/2f);
					}
					else
					{
						skillPopUpXPosition=this.gameObject.transform.position.x;
					}

					this.skillPopUp.transform.FindChild("description").GetComponent<TextMeshPro>().text=this.c.getSkillText(this.c.Skills[skillDisplayed].Description);
					if(newSkillHovered==0)
					{
						this.skillPopUp.transform.position=new Vector3(skillPopUpXPosition,gameObject.transform.position.y-0.8f+(+0.6f)*(0.5f*this.gameObject.GetComponent<BoxCollider2D>().bounds.size.y),-1f);
					}
					else
					{
						this.skillPopUp.transform.position=new Vector3(skillPopUpXPosition,gameObject.transform.position.y+0.8f+(-0.6f+(newSkillHovered-1f)*0.24f)*(0.5f*this.gameObject.GetComponent<BoxCollider2D>().bounds.size.y),-1f);
						this.skillPopUp.transform.FindChild("description").GetComponent<TextMeshPro>().text+=(". P : "+this.c.Skills[skillDisplayed].proba+"%");
					}
					this.skillPopUp.transform.FindChild("title").GetComponent<TextMeshPro>().text=this.c.Skills[skillDisplayed].Name;
				}
			}
			else
			{
				if(isCardSkillPopUpDisplayed)
				{
					this.hideSkillPopUp();
				}
			}
		}
		else if(isCardSkillPopUpDisplayed)
		{
			this.hideSkillPopUp();
		}
	}
	public virtual void OnMouseExit()
	{
		this.hideSkillPopUp ();
	}
	public override void exitCard ()
	{
	}
	private void hideSkillPopUp()
	{
		if(isCardSkillPopUpDisplayed)
		{
			this.isCardSkillPopUpDisplayed=false;
			this.skillPopUp.SetActive(false);
			this.skillDisplayed=-1;
		}
	}

	public int skillHovered()
	{
		Vector3 cursorPosition = this.getCurrentCamera().ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));

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
		return this.cardRessources.colors[id];	
	}
	public override Vector3 getCardUpgradePosition (int caracteristicUpgraded)
	{
		GameObject refObject = new GameObject ();
		float gap = 0.165f;
		switch(caracteristicUpgraded)
		{
		case 0:
			refObject = this.attack.transform.FindChild("Text").gameObject;
			break;
		case 1:
			refObject=this.life.transform.FindChild("Text").gameObject.gameObject;
			break;
		case 2:
			refObject=this.quickness.transform.FindChild("Text").gameObject.gameObject;
			break;
		case 3:
			refObject=this.skills[0].transform.FindChild ("Name").gameObject;
			break;
		case 4:
			refObject=this.skills[1].transform.FindChild ("Name").gameObject;
			break;
		case 5:
			refObject=this.skills[2].transform.FindChild ("Name").gameObject;
			break;
		case 6:
			refObject=this.skills[3].transform.FindChild ("Name").gameObject;
			break;
		}
		Vector3 refPosition =refObject.transform.position;
		float refSizeX = refObject.transform.GetComponent<MeshRenderer> ().bounds.size.x;
		return new Vector3 (refPosition.x+gap+refSizeX/2f,refPosition.y,0f);
	}
	public virtual Sprite getSkillSprite(int id)
	{
		return this.cardRessources.skills [id];
	}
	public virtual Camera getCurrentCamera()
	{
		return new Camera();
	}
}

