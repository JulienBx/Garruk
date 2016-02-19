using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

public class FocusedSkillController : MonoBehaviour 
{
	public void show(Skill s, bool isPassiveSkill)
	{
		gameObject.transform.FindChild ("closebutton").GetComponent<FocusedSkillExitButtonController> ().reset ();
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingSkills.getName(s.Id);
		gameObject.transform.FindChild("CardType").GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnCardTypePicto (s.CardType.getPictureId(),1);
		gameObject.transform.FindChild("SkillType").GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnSkillTypePicture (s.IdSkillType);
		gameObject.transform.FindChild("SkillType").FindChild("Title").GetComponent<TextMeshPro> ().text = WordingSkillTypes.getName(s.IdSkillType).Substring (0, 1).ToUpper();
		gameObject.transform.FindChild ("CardTypeTitle").GetComponent<TextMeshPro> ().text =WordingCardTypes.getName(s.CardType.Id);
		gameObject.transform.FindChild ("SkillTypeTitle").GetComponent<TextMeshPro> ().text = WordingSkillTypes.getName(s.IdSkillType);
		if(isPassiveSkill)
		{
			this.gameObject.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnCardTypePicto(s.CardType.getPictureId(),1);
		}
		else
		{
			this.gameObject.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnSkillPicto(s.getPictureId());
		}
		for(int i=0;i<10;i++)
		{
			gameObject.transform.FindChild("Skill"+i).FindChild("Title").GetComponent<TextMeshPro>().text=this.getDescription(s.Id,i);
			if(s.AllProbas[i]>0)
			{
				gameObject.transform.FindChild("Skill"+i).FindChild("Proba").gameObject.SetActive(true);
				gameObject.transform.FindChild("Skill"+i).FindChild("Proba").FindChild("Title").GetComponent<TextMeshPro>().text=s.AllProbas[i].ToString();
				if(s.AllProbas[i]<50)
				{
					gameObject.transform.FindChild("Skill"+i).FindChild("Proba").FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.greyTextColor;
				}
				else if(s.AllProbas[i]<80)
				{
					gameObject.transform.FindChild("Skill"+i).FindChild("Proba").FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
				}
				else
				{
					gameObject.transform.FindChild("Skill"+i).FindChild("Proba").FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.redColor;
				}
			}
			else
			{
				gameObject.transform.FindChild("Skill"+i).FindChild("Proba").gameObject.SetActive(false);
			}
			if(i>7)
			{
				gameObject.transform.FindChild("Skill"+i).FindChild("Level").GetComponent<TextMeshPro>().color=ApplicationDesignRules.redColor;
			}
			else if(i>4)
			{
				gameObject.transform.FindChild("Skill"+i).FindChild("Level").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
			}
			else
			{
				gameObject.transform.FindChild("Skill"+i).FindChild("Level").GetComponent<TextMeshPro>().color=ApplicationDesignRules.greyTextColor;
			}

		}
	}
	public virtual string getDescription(int idSkill, int level)
	{
		return "";
	}
	public virtual void exit()
	{
	}
	public void highlightLevel(int level)
	{
		for(int i=0;i<10;i++)
		{
			gameObject.transform.FindChild("Skill"+i).FindChild("Stone").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		}
		gameObject.transform.FindChild("Skill"+level).FindChild("Stone").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
	}
}