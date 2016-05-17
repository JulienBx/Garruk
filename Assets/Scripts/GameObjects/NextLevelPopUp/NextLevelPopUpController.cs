using UnityEngine;
using TMPro;

public class NextLevelPopUpController : MonoBehaviour 
{
	private NextLevelPopUpRessources ressources;
	private Card c;
	private int selectedAttribute;
	private int newPower;
	private int newLevel;

	public virtual void clickOnAttribute(int index, int newPower, int newLevel)
	{
	}
	public virtual void resize()
	{
	}
	public void confirmButtonHandler()
	{
		this.clickOnAttribute(this.selectedAttribute,this.newPower,this.newLevel);
	}
	public void initialize(Card c)
	{
		this.c = c;
		this.selectedAttribute=-1;
		this.resize ();

		bool canUpgrade = false;

		this.gameObject.transform.FindChild("LifeButton").GetComponent<NextLevelPopUpAttributeController>().reset();
		this.gameObject.transform.FindChild("AttackButton").GetComponent<NextLevelPopUpAttributeController>().reset();
		this.gameObject.transform.FindChild("Skill0").GetComponent<NextLevelPopUpAttributeController>().reset();
		this.gameObject.transform.FindChild("Skill1").GetComponent<NextLevelPopUpAttributeController>().reset();
		this.gameObject.transform.FindChild("Skill2").GetComponent<NextLevelPopUpAttributeController>().reset();
		this.gameObject.transform.FindChild("Skill3").GetComponent<NextLevelPopUpAttributeController>().reset();
		this.gameObject.transform.FindChild("ConfirmButton").GetComponent<NextLevelPopUpConfirmButtonController>().reset();
		this.gameObject.transform.FindChild("ConfirmButton").FindChild("Title").GetComponent<TextMeshPro>().text=WordingNextLevelPopUp.getReference(16);
		this.gameObject.transform.FindChild("ConfirmButton").gameObject.SetActive(false);
		this.ressources = this.gameObject.GetComponent<NextLevelPopUpRessources> ();
		this.gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingNextLevelPopUp.getReference(0);
		this.gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild ("Description").GetComponent<TextMeshPro> ().text = c.getName() +  WordingNextLevelPopUp.getReference(1) + c.ExperienceLevel + WordingNextLevelPopUp.getReference(2);
		this.gameObject.transform.FindChild ("Description").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.gameObject.transform.FindChild("AttackButton").GetComponent<NextLevelPopUpAttributeController> ().setId (0);
		this.gameObject.transform.FindChild("LifeButton").GetComponent<NextLevelPopUpAttributeController> ().setId (1);
		this.gameObject.transform.FindChild("Avatar").GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCaracterAvatar(this.c.Skills[0].Id);

		this.gameObject.transform.FindChild("AttackButton").FindChild("Title").GetComponent<TextMeshPro>().text = c.Attack.ToString();
		this.gameObject.transform.FindChild("AttackButton").FindChild("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors [c.AttackLevel - 1];
		if(c.UpgradedAttack<=c.Attack || c.AttackNbUpgrades==3)
		{
			this.gameObject.transform.FindChild("AttackButton").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
			this.gameObject.transform.FindChild("AttackButton").GetComponent<NextLevelPopUpAttributeController> ().setIsActive(false);
			this.gameObject.transform.FindChild("AttackButton").FindChild ("Title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.redColor;
		}
		else
		{
			this.gameObject.transform.FindChild("AttackButton").FindChild ("Title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
			canUpgrade=true;
		}

		this.gameObject.transform.FindChild("LifeButton").FindChild("Title").GetComponent<TextMeshPro>().text = c.Life.ToString();
		this.gameObject.transform.FindChild ("LifeButton").FindChild ("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors [c.LifeLevel - 1];
		if(c.UpgradedLife<=c.Life || c.LifeNbUpgrades==3)
		{
			this.gameObject.transform.FindChild("LifeButton").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
			this.gameObject.transform.FindChild("LifeButton").GetComponent<NextLevelPopUpAttributeController> ().setIsActive(false);
			this.gameObject.transform.FindChild("LifeButton").FindChild ("Title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.redColor;
		}
		else
		{
			this.gameObject.transform.FindChild("LifeButton").FindChild ("Title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
			canUpgrade=true;

		}
		if(c.AttackNbUpgrades==0)
		{
			this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade1").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade2").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade3").gameObject.SetActive(false);
		}
		else if(c.AttackNbUpgrades==1)
		{
			this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade1").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade2").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade3").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(1);
		}
		else if(c.AttackNbUpgrades==2)
		{
			this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade1").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade2").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade3").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(2);
			this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade2").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(2);
		}
		else if(c.AttackNbUpgrades==3)
		{
			this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade1").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade2").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade3").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
			this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade2").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
			this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade3").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
		}
		if(c.LifeNbUpgrades==0)
		{
			this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade1").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade2").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade3").gameObject.SetActive(false);
		}
		else if(c.LifeNbUpgrades==1)
		{
			this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade1").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade2").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade3").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(1);
		}
		else if(c.LifeNbUpgrades==2)
		{
			this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade1").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade2").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade3").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(2);
			this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade2").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(2);
		}
		else if(c.LifeNbUpgrades==3)
		{
			this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade1").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade2").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade3").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
			this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade2").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
			this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade3").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
		}

		for(int i=0;i<4;i++)
		{
			if(c.Skills[i].Upgrades==0)
			{
				gameObject.transform.FindChild("Skill"+i+"Upgrade1").gameObject.SetActive(false);
				gameObject.transform.FindChild("Skill"+i+"Upgrade2").gameObject.SetActive(false);
				gameObject.transform.FindChild("Skill"+i+"Upgrade3").gameObject.SetActive(false);
				gameObject.transform.FindChild("Skill"+i).GetComponent<SpriteRenderer>().color=new Color(0f,0f,0f);
			}
			if(c.Skills[i].Upgrades==1)
			{
				gameObject.transform.FindChild("Skill"+i+"Upgrade1").gameObject.SetActive(true);
				gameObject.transform.FindChild("Skill"+i+"Upgrade2").gameObject.SetActive(false);
				gameObject.transform.FindChild("Skill"+i+"Upgrade3").gameObject.SetActive(false);
				gameObject.transform.FindChild("Skill"+i+"Upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(1);
			}
			if(c.Skills[i].Upgrades==2)
			{
				gameObject.transform.FindChild("Skill"+i+"Upgrade1").gameObject.SetActive(true);
				gameObject.transform.FindChild("Skill"+i+"Upgrade2").gameObject.SetActive(true);
				gameObject.transform.FindChild("Skill"+i+"Upgrade3").gameObject.SetActive(false);
				gameObject.transform.FindChild("Skill"+i+"Upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(2);
				gameObject.transform.FindChild("Skill"+i+"Upgrade2").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(2);
			}
			if(c.Skills[i].Upgrades==3)
			{
				gameObject.transform.FindChild("Skill"+i+"Upgrade1").gameObject.SetActive(true);
				gameObject.transform.FindChild("Skill"+i+"Upgrade2").gameObject.SetActive(true);
				gameObject.transform.FindChild("Skill"+i+"Upgrade3").gameObject.SetActive(true);
				gameObject.transform.FindChild("Skill"+i+"Upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
				gameObject.transform.FindChild("Skill"+i+"Upgrade2").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
				gameObject.transform.FindChild("Skill"+i+"Upgrade3").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
			}
			if(i<c.Skills.Count && c.Skills[i].IsActivated==1 && c.Skills[i].Power!=10 && c.Skills[i].Upgrades<3)
			{
				gameObject.transform.FindChild("Skill"+i).gameObject.SetActive(true);
				gameObject.transform.FindChild("SkillMessage"+i).gameObject.SetActive(false);
				gameObject.transform.FindChild("Skill"+i).GetComponent<NextLevelPopUpAttributeController> ().setId (i+3);
				gameObject.transform.FindChild("Skill"+i).FindChild ("Picto").GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnSkillPicto(this.c.Skills[i].getPictureId());
				gameObject.transform.FindChild("Skill"+i).FindChild ("Picto").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.returnCardColor(this.c.Skills[i].Level);
				gameObject.transform.FindChild("Skill"+i).FindChild ("Name").GetComponent<TextMeshPro> ().text = WordingSkills.getName(c.Skills[i].Id);
				gameObject.transform.FindChild("Skill"+i).FindChild ("Power").GetComponent<TextMeshPro> ().text = WordingNextLevelPopUp.getReference(15)+c.Skills[i].Power.ToString();
				gameObject.transform.FindChild("Skill"+i).FindChild ("Description").GetComponent<TextMeshPro> ().text = this.c.getSkillText(WordingSkills.getDescription(this.c.Skills[i].Id,this.c.Skills[i].Power-1));
				canUpgrade=true;
			}
			else
			{
				gameObject.transform.FindChild("SkillMessage"+i).GetComponent<SpriteRenderer>().sprite=this.getContourSprite(1);
				gameObject.transform.FindChild("SkillMessage"+i).GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
				gameObject.transform.FindChild("SkillMessage"+i).FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.redColor;
				gameObject.transform.FindChild("Skill"+i).gameObject.SetActive(false);

				if(c.Skills[i].Power==10)
				{
					gameObject.transform.FindChild("SkillMessage"+i).FindChild("Title").gameObject.SetActive(true);
					gameObject.transform.FindChild("SkillMessage"+i).FindChild("Title").GetComponent<TextMeshPro>().text= WordingNextLevelPopUp.getReference(10);
				}
				else if(c.Skills[i].Upgrades>=3)
				{
					gameObject.transform.FindChild("SkillMessage"+i).FindChild("Title").gameObject.SetActive(true);
					gameObject.transform.FindChild("SkillMessage"+i).FindChild("Title").GetComponent<TextMeshPro>().text= WordingNextLevelPopUp.getReference(9);
					gameObject.transform.FindChild("Skill"+i+"Upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
					gameObject.transform.FindChild("Skill"+i+"Upgrade2").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
					gameObject.transform.FindChild("Skill"+i+"Upgrade3").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
				}
				else if(i==2)
				{
					gameObject.transform.FindChild("SkillMessage"+i).FindChild("Title").gameObject.SetActive(true);
					gameObject.transform.FindChild("SkillMessage"+i).FindChild("Title").GetComponent<TextMeshPro>().text= WordingNextLevelPopUp.getReference(3);
				}
				else if(i==3)
				{
					gameObject.transform.FindChild("SkillMessage"+i).FindChild("Title").gameObject.SetActive(true);
					gameObject.transform.FindChild("SkillMessage"+i).FindChild("Title").GetComponent<TextMeshPro>().text=WordingNextLevelPopUp.getReference(4);
				}

			}
			if(!canUpgrade)
			{
				this.gameObject.transform.FindChild("ConfirmButton").gameObject.SetActive(true);
			}
		}
	}
	public void clickOnAttributeHandler(int index)
	{
		if(index!=this.selectedAttribute)
		{
			if(this.selectedAttribute!=-1)
			{
				this.showDefaultLevel(this.selectedAttribute);
			}
			this.showNextLevel(index);
			this.selectedAttribute=index;
			this.gameObject.transform.FindChild("ConfirmButton").gameObject.SetActive(true);
		}
		else
		{
			this.showDefaultLevel(index);
			this.selectedAttribute=-1;
			this.gameObject.transform.FindChild("ConfirmButton").gameObject.SetActive(false);
		}
	}
	private void showNextLevel(int index)
	{
		if(index==0)
		{
			this.gameObject.transform.FindChild("AttackButton").FindChild("Title").GetComponent<TextMeshPro>().text = c.UpgradedAttack.ToString();
			this.gameObject.transform.FindChild("AttackButton").FindChild("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors [c.UpgradedAttackLevel - 1];
			this.gameObject.transform.FindChild("AttackButton").GetComponent<NextLevelPopUpAttributeController>().setIsSelected(true);
			this.gameObject.transform.FindChild("AttackButton").GetComponent<NextLevelPopUpAttributeController>().setHoveredState();
			this.newLevel=c.UpgradedAttackLevel;
			this.newPower=c.UpgradedAttack;
			if(c.AttackNbUpgrades==0)
			{
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade1").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(1);
			}
			else if(c.AttackNbUpgrades==1)
			{
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade1").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade2").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(2);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade2").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(2);
			}
			else if(c.AttackNbUpgrades==2)
			{
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade1").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade2").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade3").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade2").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade3").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
			}
		}
		else if(index ==1)
		{
			this.gameObject.transform.FindChild("LifeButton").FindChild("Title").GetComponent<TextMeshPro>().text = c.UpgradedLife.ToString();
			this.gameObject.transform.FindChild("LifeButton").FindChild("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors [c.UpgradedLifeLevel - 1];
			this.gameObject.transform.FindChild("LifeButton").GetComponent<NextLevelPopUpAttributeController>().setIsSelected(true);
			this.gameObject.transform.FindChild("LifeButton").GetComponent<NextLevelPopUpAttributeController>().setHoveredState();
			this.newLevel=c.UpgradedLifeLevel;
			this.newPower=c.UpgradedLife;
			if(c.LifeNbUpgrades==0)
			{
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade1").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(1);
			}
			else if(c.LifeNbUpgrades==1)
			{
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade1").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade2").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(2);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade2").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(2);
			}
			else if(c.LifeNbUpgrades==2)
			{
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade1").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade2").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade3").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade2").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade3").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
			}
		}
		else if(index>2)
		{
			this.gameObject.transform.FindChild("Skill"+(index-3)).FindChild ("Description").GetComponent<TextMeshPro> ().text = this.c.getSkillText(WordingSkills.getDescription(this.c.Skills[index-3].Id,this.c.Skills[index-3].Power));
			this.gameObject.transform.FindChild("Skill"+(index-3)).FindChild ("Picto").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.returnCardColor(this.c.Skills[index-3].nextLevel);
			this.gameObject.transform.FindChild("Skill"+(index-3)).FindChild ("Power").GetComponent<TextMeshPro> ().text = WordingNextLevelPopUp.getReference(15)+(c.Skills[index-3].Power+1).ToString();
			this.gameObject.transform.FindChild("Skill"+(index-3)).GetComponent<NextLevelPopUpAttributeController>().setIsSelected(true);
			this.gameObject.transform.FindChild("Skill"+(index-3)).GetComponent<NextLevelPopUpAttributeController>().setHoveredState();
			this.newLevel=c.Skills[index-3].nextLevel;
			this.newPower=c.Skills[index-3].Power+1;
			if(c.Skills[index-3].Upgrades==0)
			{
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade1").gameObject.SetActive(true);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(1);
			}
			else if(c.Skills[index-3].Upgrades==1)
			{
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade1").gameObject.SetActive(true);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade2").gameObject.SetActive(true);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(2);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade2").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(2);
			}
			else if(c.Skills[index-3].Upgrades==2)
			{
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade1").gameObject.SetActive(true);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade2").gameObject.SetActive(true);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade3").gameObject.SetActive(true);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade2").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade3").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
			}
		}
	}
	private void showDefaultLevel(int index)
	{
		if(index==0)
		{
			this.gameObject.transform.FindChild("AttackButton").FindChild("Title").GetComponent<TextMeshPro>().text = c.Attack.ToString();
			this.gameObject.transform.FindChild("AttackButton").GetComponent<NextLevelPopUpAttributeController>().setIsSelected(false);
			this.gameObject.transform.FindChild("AttackButton").GetComponent<NextLevelPopUpAttributeController>().setInitialState();
			if(c.AttackNbUpgrades==0)
			{
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade1").gameObject.SetActive(false);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade2").gameObject.SetActive(false);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade3").gameObject.SetActive(false);
			}
			else if(c.AttackNbUpgrades==1)
			{
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade1").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade2").gameObject.SetActive(false);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade3").gameObject.SetActive(false);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(1);
			}
			else if(c.AttackNbUpgrades==2)
			{
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade1").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade2").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade3").gameObject.SetActive(false);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(2);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade2").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(2);
			}
			else if(c.AttackNbUpgrades==3)
			{
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade1").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade2").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade3").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade2").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
				this.gameObject.transform.FindChild("AttackButton").FindChild("upgrade3").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
			}
		}
		else if(index ==1)
		{
			this.gameObject.transform.FindChild("LifeButton").FindChild("Title").GetComponent<TextMeshPro>().text = c.Life.ToString();
			this.gameObject.transform.FindChild("LifeButton").GetComponent<NextLevelPopUpAttributeController>().setIsSelected(false);
			this.gameObject.transform.FindChild("LifeButton").GetComponent<NextLevelPopUpAttributeController>().setInitialState();
			if(c.LifeNbUpgrades==0)
			{
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade1").gameObject.SetActive(false);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade2").gameObject.SetActive(false);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade3").gameObject.SetActive(false);
			}
			else if(c.LifeNbUpgrades==1)
			{
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade1").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade2").gameObject.SetActive(false);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade3").gameObject.SetActive(false);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(1);
			}
			else if(c.LifeNbUpgrades==2)
			{
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade1").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade2").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade3").gameObject.SetActive(false);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(2);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade2").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(2);
			}
			else if(c.LifeNbUpgrades==3)
			{
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade1").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade2").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade3").gameObject.SetActive(true);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade2").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
				this.gameObject.transform.FindChild("LifeButton").FindChild("upgrade3").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
			}
		}
		else if(index>2)
		{
			this.gameObject.transform.FindChild("Skill"+(index-3)).FindChild ("Description").GetComponent<TextMeshPro> ().text = this.c.getSkillText(WordingSkills.getDescription(this.c.Skills[index-3].Id,this.c.Skills[index-3].Power-1));
			this.gameObject.transform.FindChild("Skill"+(index-3)).FindChild ("Picto").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.returnCardColor(this.c.Skills[index-3].Level);
			this.gameObject.transform.FindChild("Skill"+(index-3)).FindChild ("Power").GetComponent<TextMeshPro> ().text = WordingNextLevelPopUp.getReference(15)+c.Skills[index-3].Power.ToString();
			this.gameObject.transform.FindChild("Skill"+(index-3)).GetComponent<NextLevelPopUpAttributeController>().setIsSelected(false);
			this.gameObject.transform.FindChild("Skill"+(index-3)).GetComponent<NextLevelPopUpAttributeController>().setInitialState();
			if(c.Skills[index-3].Upgrades==0)
			{
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade1").gameObject.SetActive(false);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade2").gameObject.SetActive(false);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade3").gameObject.SetActive(false);
			}
			else if(c.Skills[index-3].Upgrades==1)
			{
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade1").gameObject.SetActive(true);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade2").gameObject.SetActive(false);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade3").gameObject.SetActive(false);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(1);
			}
			else if(c.Skills[index-3].Upgrades==2)
			{
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade1").gameObject.SetActive(true);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade2").gameObject.SetActive(true);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade3").gameObject.SetActive(false);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(2);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade2").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(2);
			}
			else if(c.Skills[index-3].Upgrades==3)
			{
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade1").gameObject.SetActive(true);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade2").gameObject.SetActive(true);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade3").gameObject.SetActive(true);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade1").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade2").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
				gameObject.transform.FindChild("Skill"+(index-3)+"Upgrade3").GetComponent<NextLevelPopUpNextLevelIconController>().setUpgrades(3);
			}
		}
	}
	public Sprite getContourSprite(int id)
	{
		return ressources.contours [id];
	}
}

