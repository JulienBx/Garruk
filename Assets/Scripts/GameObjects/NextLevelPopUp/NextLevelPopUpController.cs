using UnityEngine;
using TMPro;

public class NextLevelPopUpController : MonoBehaviour 
{
	private NextLevelPopUpRessources ressources;
	private GameObject skillPopUp;
	private GameObject attributePopUp;
	private Card c;
	private bool isSkillPopUpDisplayed;
	private bool isAttributePopUpDisplayed;

	public virtual void clickOnAttribute(int index, int newPower, int newLevel)
	{
	}
	public virtual void resize()
	{
	}
	public void initialize(Card c)
	{
		this.c = c;
		this.resize ();
		this.skillPopUp = gameObject.transform.FindChild ("SkillPopUp").gameObject;
		this.attributePopUp = gameObject.transform.FindChild ("AttributePopUp").gameObject;

		this.ressources = this.gameObject.GetComponent<NextLevelPopUpRessources> ();
		this.gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingNextLevelPopUp.getReference(0);
		this.gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild ("Description").GetComponent<TextMeshPro> ().text = c.Title +  WordingNextLevelPopUp.getReference(1) + c.ExperienceLevel + WordingNextLevelPopUp.getReference(2);
		this.gameObject.transform.FindChild ("Description").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild("AttackButton").GetComponent<NextLevelPopUpAttributeController> ().initialize (0, c.UpgradedAttack, c.UpgradedAttackLevel);
		this.gameObject.transform.FindChild("LifeButton").GetComponent<NextLevelPopUpAttributeController> ().initialize (1, c.UpgradedLife, c.UpgradedLifeLevel);
		this.gameObject.transform.FindChild("Attack").FindChild("Title").GetComponent<TextMeshPro>().text = c.Attack.ToString();
		this.gameObject.transform.FindChild("Attack").FindChild ("Title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild("Attack").FindChild("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors [c.AttackLevel - 1];
		this.gameObject.transform.FindChild("Life").FindChild("Title").GetComponent<TextMeshPro>().text = c.Life.ToString();
		this.gameObject.transform.FindChild("Life").FindChild ("Title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild ("Life").FindChild ("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors [c.LifeLevel - 1];


		if(c.UpgradedAttack<=c.Attack)
		{
			this.gameObject.transform.FindChild("AttackButton").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
			this.gameObject.transform.FindChild("AttackButton").GetComponent<NextLevelPopUpAttributeController> ().setIsNotClickable();
		}
		if(c.UpgradedLife<=c.Life)
		{
			this.gameObject.transform.FindChild("LifeButton").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
			this.gameObject.transform.FindChild("LifeButton").GetComponent<NextLevelPopUpAttributeController> ().setIsNotClickable();
		}

		for(int i=0;i<4;i++)
		{
			if(i<c.Skills.Count && c.Skills[i].IsActivated==1)
			{
				gameObject.transform.FindChild("Skill"+i).gameObject.SetActive(true);
				gameObject.transform.FindChild("SkillButton"+i).GetComponent<SpriteRenderer>().sprite=this.getContourSprite(0);
				gameObject.transform.FindChild("SkillButton"+i).GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
				gameObject.transform.FindChild("Skill"+i).GetComponent<NextLevelPopUpAttributeController> ().initialize (i+3, c.Skills[i].Power+1, c.Skills[i].nextLevel);
				gameObject.transform.FindChild("Skill"+i).FindChild ("Picto").GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnSkillPicto(this.c.Skills[i].getPictureId());
				gameObject.transform.FindChild("Skill"+i).FindChild ("Picto").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.returnCardColor(this.c.Skills[i].Level);
				gameObject.transform.FindChild("Skill"+i).FindChild ("Name").GetComponent<TextMeshPro> ().text = WordingSkills.getName(c.Skills[i].Id);
				gameObject.transform.FindChild("Skill"+i).FindChild ("Power").GetComponent<TextMeshPro> ().text = WordingNextLevelPopUp.getReference(15)+c.Skills[i].Power.ToString();
				//gameObject.transform.FindChild("Skill"+i).FindChild ("SkillType").GetComponent<SpriteRenderer> ().sprite = this.getSkillTypeSprite(c.Skills[i].IdSkillType);
				//gameObject.transform.FindChild("Skill"+i).FindChild ("SkillType").FindChild ("Title").GetComponent<TextMeshPro> ().text = s.SkillType.Name.Substring (0, 1).ToUpper ();
				//gameObject.transform.FindChild("Skill"+i).FindChild ("Proba").FindChild ("Title").GetComponent<TextMeshPro> ().text = c.Skills[i].proba.ToString ();
				gameObject.transform.FindChild("Skill"+i).FindChild ("Description").GetComponent<TextMeshPro> ().text = this.c.getSkillText(WordingSkills.getDescription(this.c.Skills[i].Id,this.c.Skills[i].Power-1));
				if(c.Skills[i].Power==10 || c.Skills[i].Upgrades>=3)
				{
					gameObject.transform.FindChild("Skill"+i).GetComponent<NextLevelPopUpAttributeController> ().setIsNotClickable();
					gameObject.transform.FindChild("SkillButton"+i).GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
				}
			}
			else
			{
				gameObject.transform.FindChild("SkillButton"+i).GetComponent<SpriteRenderer>().sprite=this.getContourSprite(1);
				gameObject.transform.FindChild("SkillButton"+i).GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
				gameObject.transform.FindChild("SkillButton"+i).FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.redColor;
				gameObject.transform.FindChild("Skill"+i).gameObject.SetActive(false);
				if(i==2)
				{
					gameObject.transform.FindChild("SkillButton"+i).FindChild("Title").gameObject.SetActive(true);
					gameObject.transform.FindChild("SkillButton"+i).FindChild("Title").GetComponent<TextMeshPro>().text= WordingNextLevelPopUp.getReference(3);
				}
				else if(i==3)
				{
					gameObject.transform.FindChild("SkillButton"+i).FindChild("Title").gameObject.SetActive(true);
					gameObject.transform.FindChild("SkillButton"+i).FindChild("Title").GetComponent<TextMeshPro>().text=WordingNextLevelPopUp.getReference(4);
				}
			}
		}
	}
	public void displaySkillPopUp(int id)
	{
		this.isSkillPopUpDisplayed = true;
		this.skillPopUp.SetActive (true);
		Vector3 popUpPosition = gameObject.transform.FindChild("Skill"+(id-3)).position;
		popUpPosition.y=popUpPosition.y+1.6f*ApplicationDesignRules.cardFocusedScale.x;
		this.skillPopUp.transform.position=popUpPosition;

		if (c.Skills [id-3].Power != 10 && c.Skills [id-3].Upgrades < 3) 
		{
			this.skillPopUp.transform.FindChild ("Picto").gameObject.SetActive(true);
			this.skillPopUp.transform.FindChild ("Title").gameObject.SetActive(true);
			this.skillPopUp.transform.FindChild ("Value").gameObject.SetActive(true);
			this.skillPopUp.transform.FindChild ("Description").gameObject.SetActive(true);
			this.skillPopUp.transform.FindChild ("Limit").gameObject.SetActive(false);
			this.skillPopUp.transform.FindChild ("Title").GetComponent<TextMeshPro>().text=WordingNextLevelPopUp.getReference(5)+WordingSkills.getName(c.Skills[id-3].Id);
			this.skillPopUp.transform.FindChild ("Value").GetComponent<TextMeshPro> ().text = WordingNextLevelPopUp.getReference(6) + (c.Skills [id - 3].Power + 1);
			this.skillPopUp.transform.FindChild ("Description").GetComponent<TextMeshPro> ().text = this.c.getSkillText(WordingSkills.getDescription(this.c.Skills[id-3].Id,this.c.Skills[id-3].Power));	
			if(id-3!=0)
			{
				this.skillPopUp.transform.FindChild ("Description").GetComponent<TextMeshPro> ().text+=WordingNextLevelPopUp.getReference(7)+this.c.Skills [id - 3].nextProba.ToString()+WordingNextLevelPopUp.getReference(8);
			}
			this.skillPopUp.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnSkillPicto(this.c.Skills[id-3].getPictureId());
			this.skillPopUp.transform.FindChild("Picto").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.returnCardColor(this.c.Skills[id-3].Level);
			this.skillPopUp.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
		}
		else
		{
			this.skillPopUp.transform.FindChild ("Picto").gameObject.SetActive(false);
			this.skillPopUp.transform.FindChild ("Title").gameObject.SetActive(false);
			this.skillPopUp.transform.FindChild ("Value").gameObject.SetActive(false);
			this.skillPopUp.transform.FindChild ("Description").gameObject.SetActive(false);
			this.skillPopUp.transform.FindChild ("Limit").gameObject.SetActive(true);
			this.skillPopUp.transform.FindChild ("Limit").GetComponent<TextMeshPro>().color=ApplicationDesignRules.redColor;
			this.skillPopUp.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
			if(c.Skills[id-3].Upgrades>=3)
			{
				this.skillPopUp.transform.FindChild ("Limit").GetComponent<TextMeshPro>().text=WordingNextLevelPopUp.getReference(9);

			}
			else if(c.Skills[id-3].Power==10)
			{
				this.skillPopUp.transform.FindChild ("Limit").GetComponent<TextMeshPro>().text=WordingNextLevelPopUp.getReference(10);	
			}
		}
	}
	public void hideSkillPopUp()
	{
		this.isSkillPopUpDisplayed = false;
		this.skillPopUp.SetActive (false);
	}
	public void displayAttributePopUp(int id)
	{
		Vector3 popUpPosition = new Vector3 ();
		this.isAttributePopUpDisplayed = true;
		this.attributePopUp.SetActive (true);

		bool toShowNewValue=false;
		if(id==0)
		{
			if(c.UpgradedAttack>c.Attack)
			{
				toShowNewValue=true;
			}
			popUpPosition = gameObject.transform.FindChild("Attack").position;
		}
		else if(id==1)
		{
			if(c.UpgradedLife>c.Life)
			{
				toShowNewValue=true;
			}
			popUpPosition = gameObject.transform.FindChild("Life").position;
		}
//		else if(id==2)
//		{
//			if(c.UpgradedSpeed>c.Speed)
//			{
//				toShowNewValue=true;
//				popUpPosition = gameObject.transform.FindChild("Speed").position;
//			}
//		}
		if(toShowNewValue)
		{
			this.attributePopUp.transform.FindChild("Title").gameObject.SetActive(true);
			this.attributePopUp.transform.FindChild("NewPicto").gameObject.SetActive(true);
			this.attributePopUp.transform.FindChild("NewValue").gameObject.SetActive(true);
			this.attributePopUp.transform.FindChild("Limit").gameObject.SetActive(false);
			this.attributePopUp.transform.FindChild("NewPicto").GetComponent<SpriteRenderer>().sprite = this.getAttributeSprite(id);
			this.attributePopUp.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;

			if(id==0)
			{
				this.attributePopUp.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingNextLevelPopUp.getReference(11);
				this.attributePopUp.transform.FindChild ("NewValue").GetComponent<TextMeshPro> ().text = this.c.UpgradedAttack.ToString();
				this.attributePopUp.transform.FindChild ("NewPicto").GetComponent<SpriteRenderer> ().color = ressources.colors[this.c.UpgradedAttackLevel-1];
			}
			else if(id==1)
			{
				this.attributePopUp.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingNextLevelPopUp.getReference(12);
				this.attributePopUp.transform.FindChild ("NewValue").GetComponent<TextMeshPro> ().text = this.c.UpgradedLife.ToString();
				this.attributePopUp.transform.FindChild ("NewPicto").GetComponent<SpriteRenderer> ().color = ressources.colors[this.c.UpgradedLifeLevel-1];
			}
			else if(id==2)
			{
				this.attributePopUp.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingNextLevelPopUp.getReference(13);
				this.attributePopUp.transform.FindChild ("NewValue").GetComponent<TextMeshPro> ().text = this.c.UpgradedSpeed.ToString();
				this.attributePopUp.transform.FindChild ("NewPicto").GetComponent<SpriteRenderer> ().color = ressources.colors[this.c.UpgradedSpeedLevel-1];
			}
		}
		else
		{
			this.attributePopUp.transform.FindChild("Title").gameObject.SetActive(false);
			this.attributePopUp.transform.FindChild("NewPicto").gameObject.SetActive(false);
			this.attributePopUp.transform.FindChild("NewValue").gameObject.SetActive(false);
			this.attributePopUp.transform.FindChild("Limit").gameObject.SetActive(true);
			this.attributePopUp.transform.FindChild("Limit").GetComponent<TextMeshPro>().text=WordingNextLevelPopUp.getReference(14);
			this.attributePopUp.transform.FindChild("Limit").GetComponent<TextMeshPro>().color=ApplicationDesignRules.redColor;
			this.attributePopUp.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
		}
		popUpPosition.y=popUpPosition.y+1.55f*ApplicationDesignRules.cardFocusedScale.x;
		this.attributePopUp.transform.position=popUpPosition;
	}
	public void hideAttributePopUp()
	{
		this.isAttributePopUpDisplayed = false;
		this.attributePopUp.SetActive (false);
	}
	public Sprite getSkillSprite(int id)
	{
		return ressources.skillsSprites [id];
	}
	public Sprite getSkillTypeSprite(int id)
	{
		return ressources.skillsTypesSprites [id];
	}
	public Sprite getAttributeSprite(int id)
	{
		return ressources.attributesSprites [id];
	}
	public Sprite getContourSprite(int id)
	{
		return ressources.contours [id];
	}
}

