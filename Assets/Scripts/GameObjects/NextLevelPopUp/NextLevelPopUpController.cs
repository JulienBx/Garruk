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
		this.gameObject.transform.FindChild ("Description").GetComponent<TextMeshPro> ().text = c.Title + " est passé au niveau " + c.ExperienceLevel;
		this.gameObject.transform.FindChild ("Description").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild ("Description2").GetComponent<TextMeshPro> ().text = "Choisissez une caractéristique à augmenter";
		this.gameObject.transform.FindChild ("Description2").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild ("Picto").GetComponent<SpriteRenderer> ().sprite = ressources.cardTypesSprites [c.IdClass];

		this.gameObject.transform.FindChild("Attack").GetComponent<NextLevelPopUpAttributeController> ().initialize (0, c.UpgradedAttack, c.UpgradedAttackLevel);
		this.gameObject.transform.FindChild("Life").GetComponent<NextLevelPopUpAttributeController> ().initialize (1, c.UpgradedLife, c.UpgradedLifeLevel);
//		this.gameObject.transform.FindChild("Speed").GetComponent<NextLevelPopUpAttributeController> ().initialize (2, c.UpgradedSpeed, c.UpgradedSpeedLevel);
		this.gameObject.transform.FindChild("Attack").FindChild("Title").GetComponent<TextMeshPro>().text = "Attaque";
		this.gameObject.transform.FindChild("Attack").FindChild ("Title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild("Attack").FindChild("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors [c.AttackLevel - 1];
		this.gameObject.transform.FindChild("Life").FindChild("Title").GetComponent<TextMeshPro>().text = "Vie";
		this.gameObject.transform.FindChild("Life").FindChild ("Title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild("Life").FindChild("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors [c.LifeLevel - 1];
//		this.gameObject.transform.FindChild("Speed").FindChild("Title").GetComponent<TextMeshPro>().text = "Rapidité";
//		this.gameObject.transform.FindChild("Speed").FindChild ("Title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
//		this.gameObject.transform.FindChild("Speed").FindChild("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors [c.SpeedLevel - 1];


		if(c.UpgradedAttack<=c.Attack)
		{
			this.gameObject.transform.FindChild("Attack").GetComponent<NextLevelPopUpAttributeController> ().setIsNotClickable();
		}
		if(c.UpgradedLife<=c.Life)
		{
			this.gameObject.transform.FindChild("Life").GetComponent<NextLevelPopUpAttributeController> ().setIsNotClickable();
		}
//		if(c.UpgradedSpeed<=c.Speed)
//		{
//			this.gameObject.transform.FindChild("Speed").GetComponent<NextLevelPopUpAttributeController> ().setIsActive(false);
//		}
		for(int i=0;i<4;i++)
		{
			if(i<c.Skills.Count && c.Skills[i].IsActivated==1)
			{
				gameObject.transform.FindChild("Skill"+i).FindChild("Title").GetComponent<TextMeshPro>().text=c.Skills[i].Name;
				gameObject.transform.FindChild("Skill"+i).FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
				gameObject.transform.FindChild("Skill"+i).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.getSkillSprite(0);
				gameObject.transform.FindChild("Skill"+i).FindChild("Picto").GetComponent<SpriteRenderer>().color=ressources.colors[c.Skills[i].Level-1];
				if(c.Skills[i].Power!=10 && c.Skills[i].Upgrades<3)
				{
					if(c.ExperienceLevel==4 || c.ExperienceLevel==8)
					{
						if(i==(c.Skills.Count-1) || ((i+1)<c.Skills.Count && c.Skills[i+1].IsActivated!=1))
						{
							gameObject.transform.FindChild("Skill"+i).FindChild("New").gameObject.SetActive(true);
							gameObject.transform.FindChild("Skill"+i).FindChild("New").GetComponent<TextMeshPro>().text="Nouveau !";
							gameObject.transform.FindChild("Skill"+i).FindChild("New").GetComponent<TextMeshPro>().color=ApplicationDesignRules.redColor;
						}
						else
						{
							gameObject.transform.FindChild("Skill"+i).FindChild("New").GetComponent<TextMeshPro>().text="";
						}
					}
					else
					{
						gameObject.transform.FindChild("Skill"+i).FindChild("New").GetComponent<TextMeshPro>().text="";
					}
				}
				else
				{
					this.gameObject.transform.FindChild("Skill" +i).GetComponent<NextLevelPopUpAttributeController> ().setIsNotClickable();
				}
				gameObject.transform.FindChild("Skill"+i).GetComponent<NextLevelPopUpAttributeController> ().initialize (i+3, c.Skills[i].Power+1, c.Skills[i].nextLevel);
			}
			else
			{
				this.gameObject.transform.FindChild("Skill"+i).gameObject.SetActive(false);
			}
		}
	}
	public void displaySkillPopUp(int id)
	{
		this.isSkillPopUpDisplayed = true;
		this.skillPopUp.SetActive (true);
		Vector3 popUpPosition = gameObject.transform.FindChild("Skill"+(id-3)).position;
		popUpPosition.y=popUpPosition.y+1.5f*ApplicationDesignRules.nextLevelPopUpScale.x;
		this.skillPopUp.transform.position=popUpPosition;

		if (c.Skills [id-3].Power != 10 && c.Skills [id-3].Upgrades < 3) 
		{
			this.skillPopUp.transform.FindChild ("Picto").gameObject.SetActive(true);
			this.skillPopUp.transform.FindChild ("Title").gameObject.SetActive(true);
			this.skillPopUp.transform.FindChild ("Value").gameObject.SetActive(true);
			this.skillPopUp.transform.FindChild ("Description").gameObject.SetActive(true);
			this.skillPopUp.transform.FindChild ("Limit").gameObject.SetActive(false);
			this.skillPopUp.transform.FindChild ("Picto").GetComponent<SpriteRenderer>().sprite = this.getSkillSprite(0);
			this.skillPopUp.transform.FindChild ("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors[this.c.Skills [id - 3].nextLevel-1];
			this.skillPopUp.transform.FindChild ("Title").GetComponent<TextMeshPro>().text="Augmenter "+c.Skills[id-3].Name;
			this.skillPopUp.transform.FindChild ("Value").GetComponent<TextMeshPro> ().text = "Passage au niveau " + (c.Skills [id - 3].Power + 1);
			this.skillPopUp.transform.FindChild ("Description").GetComponent<TextMeshPro> ().text = this.c.Skills [id - 3].nextDescription;	
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
			this.attributePopUp.transform.FindChild("OldPicto").gameObject.SetActive(true);
			this.attributePopUp.transform.FindChild("NewPicto").gameObject.SetActive(true);
			this.attributePopUp.transform.FindChild("OldValue").gameObject.SetActive(true);
			this.attributePopUp.transform.FindChild("NewValue").gameObject.SetActive(true);
			this.attributePopUp.transform.FindChild("RightArrow").gameObject.SetActive(true);
			this.attributePopUp.transform.FindChild("Limit").gameObject.SetActive(false);
			this.attributePopUp.transform.FindChild("OldPicto").GetComponent<SpriteRenderer>().sprite = this.getAttributeSprite(id);
			this.attributePopUp.transform.FindChild("NewPicto").GetComponent<SpriteRenderer>().sprite = this.getAttributeSprite(id);
			this.attributePopUp.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;

			if(id==0)
			{
				this.attributePopUp.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Augmenter l'attaque";
				this.attributePopUp.transform.FindChild ("OldValue").GetComponent<TextMeshPro> ().text = this.c.Attack.ToString();
				this.attributePopUp.transform.FindChild ("OldPicto").GetComponent<SpriteRenderer> ().color = ressources.colors[this.c.AttackLevel-1];
				this.attributePopUp.transform.FindChild ("NewValue").GetComponent<TextMeshPro> ().text = this.c.UpgradedAttack.ToString();
				this.attributePopUp.transform.FindChild ("NewPicto").GetComponent<SpriteRenderer> ().color = ressources.colors[this.c.UpgradedAttackLevel-1];
			}
			else if(id==1)
			{
				this.attributePopUp.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Augmenter la vie";
				this.attributePopUp.transform.FindChild ("OldValue").GetComponent<TextMeshPro> ().text = this.c.Life.ToString();
				this.attributePopUp.transform.FindChild ("OldPicto").GetComponent<SpriteRenderer> ().color = ressources.colors[this.c.LifeLevel-1];
				this.attributePopUp.transform.FindChild ("NewValue").GetComponent<TextMeshPro> ().text = this.c.UpgradedLife.ToString();
				this.attributePopUp.transform.FindChild ("NewPicto").GetComponent<SpriteRenderer> ().color = ressources.colors[this.c.UpgradedLifeLevel-1];
			}
			else if(id==2)
			{
				this.attributePopUp.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Augmenter la vitesse";
				this.attributePopUp.transform.FindChild ("OldValue").GetComponent<TextMeshPro> ().text = this.c.Speed.ToString();
				this.attributePopUp.transform.FindChild ("OldPicto").GetComponent<SpriteRenderer> ().color = ressources.colors[this.c.SpeedLevel-1];
				this.attributePopUp.transform.FindChild ("NewValue").GetComponent<TextMeshPro> ().text = this.c.UpgradedSpeed.ToString();
				this.attributePopUp.transform.FindChild ("NewPicto").GetComponent<SpriteRenderer> ().color = ressources.colors[this.c.UpgradedSpeedLevel-1];
			}
		}
		else
		{
			this.attributePopUp.transform.FindChild("Title").gameObject.SetActive(false);
			this.attributePopUp.transform.FindChild("OldPicto").gameObject.SetActive(false);
			this.attributePopUp.transform.FindChild("NewPicto").gameObject.SetActive(false);
			this.attributePopUp.transform.FindChild("OldValue").gameObject.SetActive(false);
			this.attributePopUp.transform.FindChild("NewValue").gameObject.SetActive(false);
			this.attributePopUp.transform.FindChild("RightArrow").gameObject.SetActive(false);
			this.attributePopUp.transform.FindChild("Limit").gameObject.SetActive(true);
			this.attributePopUp.transform.FindChild("Limit").GetComponent<TextMeshPro>().text="Niveau maximum atteint pour cette caractéristique";
			this.attributePopUp.transform.FindChild("Limit").GetComponent<TextMeshPro>().color=ApplicationDesignRules.redColor;
			this.attributePopUp.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
		}
		popUpPosition.y=popUpPosition.y+1.5f*ApplicationDesignRules.nextLevelPopUpScale.x;
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
	public Sprite getAttributeSprite(int id)
	{
		return ressources.attributesSprites [id];
	}
}

