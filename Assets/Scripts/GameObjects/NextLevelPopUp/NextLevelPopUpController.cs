using UnityEngine;
using TMPro;

public class NextLevelPopUpController : MonoBehaviour 
{
	private NextLevelPopUpRessources ressources;

	public virtual void clickOnAttribute(int index)
	{
	}
	public void show(Card c)
	{
		this.ressources = this.gameObject.GetComponent<NextLevelPopUpRessources> ();
		this.gameObject.transform.FindChild ("Text").GetComponent<TextMeshPro> ().text = "NOUVEAU NIVEAU !\nSélectionnez la caractéristique à améliorer";

		if(c.UpgradedAttack>c.Attack)
		{
			this.gameObject.transform.FindChild("Attack").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("AttackLimit").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("Attack").FindChild("Text").GetComponent<TextMeshPro>().text = c.Attack.ToString();
			this.gameObject.transform.FindChild("Attack").FindChild("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors [c.AttackLevel - 1];
			this.gameObject.transform.FindChild("Attack").FindChild("Text2").GetComponent<TextMeshPro>().text = c.UpgradedAttack.ToString();
			this.gameObject.transform.FindChild("Attack").FindChild("Picto2").GetComponent<SpriteRenderer> ().color = ressources.colors [c.AttackLevel - 1];
		}
		else
		{
			this.gameObject.transform.FindChild("Attack").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("AttackLimit").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("AttackLimit").GetComponent<TextMeshPro>().text="Niveau d'attaque maximum atteint";
		}
		if(c.UpgradedLife>c.Life)
		{
			this.gameObject.transform.FindChild("Life").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("LifeLimit").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("Life").FindChild("Text").GetComponent<TextMeshPro>().text = c.Life.ToString();
			this.gameObject.transform.FindChild("Life").FindChild("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors [c.LifeLevel - 1];
			this.gameObject.transform.FindChild("Life").FindChild("Text2").GetComponent<TextMeshPro>().text = c.UpgradedLife.ToString();
			this.gameObject.transform.FindChild("Life").FindChild("Picto2").GetComponent<SpriteRenderer> ().color = ressources.colors [c.LifeLevel - 1];
		}
		else
		{
			this.gameObject.transform.FindChild("Life").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("LifeLimit").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("LifeLimit").GetComponent<TextMeshPro>().text="Niveau de vie maximum atteint";
		}
		if(c.UpgradedSpeed>c.Speed)
		{
			this.gameObject.transform.FindChild("Quickness").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("QuicknessLimit").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("Quickness").FindChild("Text").GetComponent<TextMeshPro>().text = c.Speed.ToString();
			this.gameObject.transform.FindChild("Quickness").FindChild("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors [c.SpeedLevel - 1];
			this.gameObject.transform.FindChild("Quickness").FindChild("Text2").GetComponent<TextMeshPro>().text = c.UpgradedSpeed.ToString();
			this.gameObject.transform.FindChild("Quickness").FindChild("Picto2").GetComponent<SpriteRenderer> ().color = ressources.colors [c.SpeedLevel - 1];
		}
		else
		{
			this.gameObject.transform.FindChild("Quickness").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("QuicknessLimit").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("QuicknessLimit").GetComponent<TextMeshPro>().text="Niveau de rapidité maximum atteint";
		}
		for(int i=0;i<4;i++)
		{
			if(i<c.Skills.Count && c.Skills[i].IsActivated==1)
			{
				if(c.Skills[i].Power!=10 && c.Skills[i].Upgrades<3)
				{
					gameObject.transform.FindChild("Skill"+i).gameObject.SetActive(true);
					gameObject.transform.FindChild("Skill"+i+"Limit").gameObject.SetActive(false);
					gameObject.transform.FindChild("Skill"+i).FindChild("Name").GetComponent<TextMeshPro>().text=c.Skills[i].Name;
					gameObject.transform.FindChild("Skill"+i).FindChild("Text").GetComponent<TextMeshPro>().text=c.getSkillText(c.Skills[i].Description);
					gameObject.transform.FindChild("Skill"+i).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.getSkillSprite(c.Skills[i].Level - 1);
					if(c.GetNewSkill)
					{
						if(i==(c.Skills.Count-1) || ((i+1)<c.Skills.Count && c.Skills[i+1].IsActivated!=1))
						{
							gameObject.transform.FindChild("Skill"+i).FindChild("New").gameObject.SetActive(true);
							gameObject.transform.FindChild("Skill"+i).FindChild("New").GetComponent<TextMeshPro>().text="Nouveau !";
						}
						else
						{
							gameObject.transform.FindChild("Skill"+i).FindChild("New").gameObject.SetActive(false);
						}
					}
					else
					{
						gameObject.transform.FindChild("Skill"+i).FindChild("New").gameObject.SetActive(false);
					}
					gameObject.transform.FindChild("Skill"+i).FindChild("Name2").GetComponent<TextMeshPro>().text=c.UpgradedSkills[i].Name;
					gameObject.transform.FindChild("Skill"+i).FindChild("Name2").GetComponent<TextMeshPro>().text=c.Skills[i].Name;
					gameObject.transform.FindChild("Skill"+i).FindChild("Text2").GetComponent<TextMeshPro>().text=c.getSkillText(c.Skills[i].nextDescription);
					gameObject.transform.FindChild("Skill"+i).FindChild("Text2").GetComponent<TextMeshPro>().text=c.getSkillText(c.Skills[i].Description);
					//gameObject.transform.FindChild("Skill"+i).FindChild("Picto2").GetComponent<SpriteRenderer>().sprite = this.getSkillSprite(c.UpgradedSkills[i].Level - 1);
					gameObject.transform.FindChild("Skill"+i).FindChild("Picto").GetComponent<SpriteRenderer>().sprite = this.getSkillSprite(c.Skills[i].Level - 1);
					
				}
				else
				{
					gameObject.transform.FindChild("Skill"+i).gameObject.SetActive(false);
					gameObject.transform.FindChild("Skill"+i+"Limit").gameObject.SetActive(true);
					if(c.Skills[i].Upgrades>=3)
					{
						gameObject.transform.FindChild("Skill"+i+"Limit").GetComponent<TextMeshPro>().text="Vous avez amélioré 3x cette compétence, vous ne pouvez plus l'améliorer.";
					}
					else if(c.Skills[i].Power==10)
					{
						gameObject.transform.FindChild("Skill"+i+"Limit").GetComponent<TextMeshPro>().text="Niveau maximum atteint pour cette compétence.";
					}
				}
			}
			else
			{
				gameObject.transform.FindChild("Skill"+i).gameObject.SetActive(false);
				gameObject.transform.FindChild("Skill"+i+"Limit").gameObject.SetActive(true);
				if(i==2)
				{
					gameObject.transform.FindChild("Skill"+i+"Limit").GetComponent<TextMeshPro>().text="Cette compétence ne sera accessible que lorsque votre carte aura au moins atteint le niveau 4.";
				}
				else if(i==3)
				{
					gameObject.transform.FindChild("Skill"+i+"Limit").GetComponent<TextMeshPro>().text="Cette compétence ne sera accessible que lorsque votre carte aura au moins atteint le niveau 8.";
				}
			}
		}
	}
	public Sprite getSkillSprite(int id)
	{
		return ressources.skillsSprites [id];
	}
}

