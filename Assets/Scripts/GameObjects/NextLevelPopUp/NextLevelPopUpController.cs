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
	public void initialize(Card c)
	{
		this.c = c;
		this.skillPopUp = gameObject.transform.FindChild ("SkillPopUp").gameObject;
		this.attributePopUp = gameObject.transform.FindChild ("AttributePopUp").gameObject;

		this.ressources = this.gameObject.GetComponent<NextLevelPopUpRessources> ();
		this.gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "NOUVEAU NIVEAU";
		this.gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild ("Description").GetComponent<TextMeshPro> ().text = c.Title + " est passé au niveau : " + c.ExperienceLevel + ", choisissez la caractéristique à augmenter";
		this.gameObject.transform.FindChild ("Description").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild ("Picto").GetComponent<SpriteRenderer> ().sprite = ressources.cardTypesSprites [c.IdClass];

		if(c.UpgradedAttack>c.Attack)
		{
			this.gameObject.transform.FindChild("Attack").FindChild("Limit").GetComponent<TextMeshPro>().text="";
			this.gameObject.transform.FindChild("Attack").FindChild("Title").GetComponent<TextMeshPro>().text = "Attaque";
			this.gameObject.transform.FindChild("Attack").FindChild("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors [c.AttackLevel - 1];
			this.gameObject.transform.FindChild("Attack").FindChild("Value").GetComponent<TextMeshPro>().text=c.Attack.ToString();
			this.gameObject.transform.FindChild("Attack").GetComponent<NextLevelPopUpAttributeController> ().initialize (0, c.UpgradedAttack, c.UpgradedAttackLevel);
		}
		else
		{
			this.gameObject.transform.FindChild("Attack").FindChild("Limit").GetComponent<TextMeshPro>().text="Niveau d'attaque maximum atteint";
			this.gameObject.transform.FindChild("Attack").FindChild("Picto").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("Attack").GetComponent<NextLevelPopUpAttributeController> ().setIsActive(false);
		}
		if(c.UpgradedLife>c.Life)
		{
			this.gameObject.transform.FindChild("Life").FindChild("Limit").GetComponent<TextMeshPro>().text="";
			this.gameObject.transform.FindChild("Life").FindChild("Title").GetComponent<TextMeshPro>().text = "Vie";
			this.gameObject.transform.FindChild("Life").FindChild("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors [c.LifeLevel - 1];
			this.gameObject.transform.FindChild("Life").FindChild("Value").GetComponent<TextMeshPro>().text=c.Life.ToString();
			this.gameObject.transform.FindChild("Life").GetComponent<NextLevelPopUpAttributeController> ().initialize (1, c.UpgradedLife, c.UpgradedLifeLevel);
		}
		else
		{
			this.gameObject.transform.FindChild("Life").FindChild("Limit").GetComponent<TextMeshPro>().text="Niveau de vie maximum atteint";
			this.gameObject.transform.FindChild("Life").FindChild("Picto").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("Life").GetComponent<NextLevelPopUpAttributeController> ().setIsActive(false);
		}
		if(c.UpgradedSpeed>c.Speed)
		{
			this.gameObject.transform.FindChild("Speed").FindChild("Limit").GetComponent<TextMeshPro>().text="";
			this.gameObject.transform.FindChild("Speed").FindChild("Title").GetComponent<TextMeshPro>().text = "Rapidité";
			this.gameObject.transform.FindChild("Speed").FindChild("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors [c.SpeedLevel - 1];
			this.gameObject.transform.FindChild("Speed").FindChild("Value").GetComponent<TextMeshPro>().text=c.Speed.ToString();
			this.gameObject.transform.FindChild("Speed").GetComponent<NextLevelPopUpAttributeController> ().initialize (2, c.UpgradedSpeed, c.UpgradedSpeedLevel);
		}
		else
		{
			this.gameObject.transform.FindChild("Speed").FindChild("Limit").GetComponent<TextMeshPro>().text="Niveau de rapidité maximum atteint";
			this.gameObject.transform.FindChild("Speed").FindChild("Picto").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("Speed").GetComponent<NextLevelPopUpAttributeController> ().setIsActive(false);
		}
		for(int i=0;i<4;i++)
		{
			if(i<c.Skills.Count && c.Skills[i].IsActivated==1)
			{
				if(c.Skills[i].Power!=10 && c.Skills[i].Upgrades<3)
				{
					gameObject.transform.FindChild("Skill"+i).FindChild("Limit").GetComponent<TextMeshPro>().text="";
					gameObject.transform.FindChild("Skill"+i).FindChild("Title").GetComponent<TextMeshPro>().text=c.Skills[i].Name;
					gameObject.transform.FindChild("Skill"+i).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.getSkillSprite(0);
					gameObject.transform.FindChild("Skill"+i).FindChild("Picto").GetComponent<SpriteRenderer>().color=ressources.colors[c.Skills[i].Level-1];
					gameObject.transform.FindChild("Skill"+i).FindChild("Level").GetComponent<TextMeshPro>().text="Niveau "+c.Skills[i].Power.ToString();
					if(c.ExperienceLevel==4 || c.ExperienceLevel==8)
					{
						if(i==(c.Skills.Count-1) || ((i+1)<c.Skills.Count && c.Skills[i+1].IsActivated!=1))
						{
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
					gameObject.transform.FindChild("Skill"+i).GetComponent<NextLevelPopUpAttributeController> ().initialize (i+3, c.Skills[i].Power+1, c.Skills[i].nextLevel);
				}
				else
				{
					if(c.Skills[i].Upgrades>=3)
					{
						gameObject.transform.FindChild("Skill"+i).FindChild("Limit").GetComponent<TextMeshPro>().text="Vous avez amélioré 3x cette compétence, vous ne pouvez plus l'améliorer.";
					}
					else if(c.Skills[i].Power==10)
					{
						gameObject.transform.FindChild("Skill"+i).FindChild("Limit").GetComponent<TextMeshPro>().text="Niveau maximum atteint pour cette compétence.";
					}
					this.gameObject.transform.FindChild("Skill" +i).GetComponent<NextLevelPopUpAttributeController> ().setIsActive(false);
					this.gameObject.transform.FindChild("Skill"+i).FindChild("Picto").gameObject.SetActive(false);
				}
			}
			else
			{
				if(i==2)
				{
					gameObject.transform.FindChild("Skill"+i).FindChild("Limit").GetComponent<TextMeshPro>().text="Cette compétence ne sera accessible que lorsque votre carte aura au moins atteint le niveau 4.";
				}
				else if(i==3)
				{
					gameObject.transform.FindChild("Skill"+i).FindChild("Limit").GetComponent<TextMeshPro>().text="Cette compétence ne sera accessible que lorsque votre carte aura au moins atteint le niveau 8.";
				}
				this.gameObject.transform.FindChild("Skill" +i).GetComponent<NextLevelPopUpAttributeController> ().setIsActive(false);
				this.gameObject.transform.FindChild("Skill"+i).FindChild("Picto").gameObject.SetActive(false);
			}
		}
	}
	public void displaySkillPopUp(int id)
	{
		this.isSkillPopUpDisplayed = true;
		this.skillPopUp.SetActive (true);
		this.skillPopUp.transform.FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.getSkillSprite(0);
		this.skillPopUp.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Passage au niveau " + (c.Skills [id - 3].Power + 1);
		this.skillPopUp.transform.FindChild ("Description").GetComponent<TextMeshPro> ().text = this.c.Skills [id - 3].nextDescription;
		this.skillPopUp.transform.FindChild ("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors[this.c.Skills [id - 3].nextLevel-1];
		Vector3 popUpPosition = gameObject.transform.FindChild("Skill"+(id-3)).position;
		popUpPosition.y=popUpPosition.y+2.2f;
		this.skillPopUp.transform.position=popUpPosition;
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
		this.attributePopUp.transform.FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.getAttributeSprite(id);
		this.attributePopUp.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Nouvelle valeur";
		if(id==0)
		{
			this.attributePopUp.transform.FindChild ("Description").GetComponent<TextMeshPro> ().text = this.c.UpgradedAttack.ToString();
			this.attributePopUp.transform.FindChild ("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors[this.c.UpgradedAttackLevel-1];
			popUpPosition = gameObject.transform.FindChild("Attack").position;
		}
		else if(id==1)
		{
			this.attributePopUp.transform.FindChild ("Description").GetComponent<TextMeshPro> ().text = this.c.UpgradedLife.ToString();
			this.attributePopUp.transform.FindChild ("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors[this.c.UpgradedLifeLevel-1];
			popUpPosition = gameObject.transform.FindChild("Life").position;
		}
		else if(id==2)
		{
			this.attributePopUp.transform.FindChild ("Description").GetComponent<TextMeshPro> ().text = this.c.UpgradedSpeed.ToString();
			this.attributePopUp.transform.FindChild ("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors[this.c.UpgradedSpeedLevel-1];
			popUpPosition = gameObject.transform.FindChild("Speed").position;
		}
		popUpPosition.y=popUpPosition.y+2.2f;
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

