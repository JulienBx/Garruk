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
		this.gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "NOUVEAU NIVEAU !";
		this.gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild ("Description").GetComponent<TextMeshPro> ().text = c.Title + " est passé au niveau " + c.ExperienceLevel +".\nChoisissez une caractéristique à augmenter";
		this.gameObject.transform.FindChild ("Description").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild ("Picto").GetComponent<SpriteRenderer> ().sprite = ressources.cardTypesSprites [c.IdClass];

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
				gameObject.transform.FindChild("Skill"+i).FindChild ("Picto").GetComponent<SpriteRenderer> ().sprite = this.getSkillSprite(c.Skills[i].Level - 1);
				gameObject.transform.FindChild("Skill"+i).FindChild ("Name").GetComponent<TextMeshPro> ().text = c.Skills[i].Name;
				gameObject.transform.FindChild("Skill"+i).FindChild ("Power").GetComponent<TextMeshPro> ().text = "Niv "+c.Skills[i].Power.ToString();
				gameObject.transform.FindChild("Skill"+i).FindChild ("SkillType").GetComponent<SpriteRenderer> ().sprite = this.getSkillTypeSprite(c.Skills[i].SkillType.Id);
				//gameObject.transform.FindChild("Skill"+i).FindChild ("SkillType").FindChild ("Title").GetComponent<TextMeshPro> ().text = s.SkillType.Name.Substring (0, 1).ToUpper ();
				gameObject.transform.FindChild("Skill"+i).FindChild ("Proba").FindChild ("Title").GetComponent<TextMeshPro> ().text = c.Skills[i].proba.ToString ();
				gameObject.transform.FindChild("Skill"+i).FindChild ("Description").GetComponent<TextMeshPro> ().text = c.getSkillText(c.Skills[i].Description);
				Color probaColor = new Color ();
				if(c.Skills[i].proba<50)
				{
					probaColor=ApplicationDesignRules.greyTextColor;
				}
				else if(c.Skills[i].proba<80)
				{
					probaColor=ApplicationDesignRules.blueColor;
				}
				else
				{
					probaColor=ApplicationDesignRules.redColor;
				}
				gameObject.transform.FindChild("Skill"+i).FindChild ("Proba").FindChild ("Title").GetComponent<TextMeshPro> ().color = probaColor;

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
					gameObject.transform.FindChild("SkillButton"+i).FindChild("Title").GetComponent<TextMeshPro>().text="Cette compétence sera accessible à partir du niveau 4";
				}
				else if(i==3)
				{
					gameObject.transform.FindChild("SkillButton"+i).FindChild("Title").gameObject.SetActive(true);
					gameObject.transform.FindChild("SkillButton"+i).FindChild("Title").GetComponent<TextMeshPro>().text="Cette compétence sera accessible à partir du niveau 8";
				}
			}
		}
	}
	public void displaySkillPopUp(int id)
	{
		this.isSkillPopUpDisplayed = true;
		this.skillPopUp.SetActive (true);
		Vector3 popUpPosition = gameObject.transform.FindChild("Skill"+(id-3)).position;
		popUpPosition.y=popUpPosition.y+1.3f*ApplicationDesignRules.nextLevelPopUpScale.x;
		this.skillPopUp.transform.position=popUpPosition;

		if (c.Skills [id-3].Power != 10 && c.Skills [id-3].Upgrades < 3) 
		{
			this.skillPopUp.transform.FindChild ("Picto").gameObject.SetActive(true);
			this.skillPopUp.transform.FindChild ("Title").gameObject.SetActive(true);
			this.skillPopUp.transform.FindChild ("Value").gameObject.SetActive(true);
			this.skillPopUp.transform.FindChild ("Description").gameObject.SetActive(true);
			this.skillPopUp.transform.FindChild ("Limit").gameObject.SetActive(false);
			this.skillPopUp.transform.FindChild ("Picto").GetComponent<SpriteRenderer>().sprite = this.getSkillSprite(c.Skills[id-3].nextLevel - 1);
			this.skillPopUp.transform.FindChild ("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors[this.c.Skills [id - 3].nextLevel-1];
			this.skillPopUp.transform.FindChild ("Title").GetComponent<TextMeshPro>().text="Augmenter "+c.Skills[id-3].Name;
			this.skillPopUp.transform.FindChild ("Value").GetComponent<TextMeshPro> ().text = "Passage au niveau " + (c.Skills [id - 3].Power + 1);
			this.skillPopUp.transform.FindChild ("Description").GetComponent<TextMeshPro> ().text = c.getSkillText(this.c.Skills [id - 3].nextDescription);	
			if(id-3!=0)
			{
				this.skillPopUp.transform.FindChild ("Description").GetComponent<TextMeshPro> ().text+=". P : "+this.c.Skills [id - 3].nextProba.ToString()+"%";
			}
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
				this.skillPopUp.transform.FindChild ("Limit").GetComponent<TextMeshPro>().text="Cette compétence a déjà été augmentée 3x, vous ne pouvez plus l'augmenter";

			}
			else if(c.Skills[id-3].Power==10)
			{
				this.skillPopUp.transform.FindChild ("Limit").GetComponent<TextMeshPro>().text="Niveau maximum atteint pour cette compétence.";	
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
				this.attributePopUp.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Augmenter l'attaque";
				this.attributePopUp.transform.FindChild ("NewValue").GetComponent<TextMeshPro> ().text = this.c.UpgradedAttack.ToString();
				this.attributePopUp.transform.FindChild ("NewPicto").GetComponent<SpriteRenderer> ().color = ressources.colors[this.c.UpgradedAttackLevel-1];
			}
			else if(id==1)
			{
				this.attributePopUp.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Augmenter la vie";
				this.attributePopUp.transform.FindChild ("NewValue").GetComponent<TextMeshPro> ().text = this.c.UpgradedLife.ToString();
				this.attributePopUp.transform.FindChild ("NewPicto").GetComponent<SpriteRenderer> ().color = ressources.colors[this.c.UpgradedLifeLevel-1];
			}
			else if(id==2)
			{
				this.attributePopUp.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Augmenter la vitesse";
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
			this.attributePopUp.transform.FindChild("Limit").GetComponent<TextMeshPro>().text="Niveau maximum atteint pour cette caractéristique";
			this.attributePopUp.transform.FindChild("Limit").GetComponent<TextMeshPro>().color=ApplicationDesignRules.redColor;
			this.attributePopUp.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
		}
		popUpPosition.y=popUpPosition.y+1.25f*ApplicationDesignRules.nextLevelPopUpScale.x;
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

