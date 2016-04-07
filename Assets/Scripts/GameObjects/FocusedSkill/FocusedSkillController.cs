using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

public class FocusedSkillController : MonoBehaviour 
{
	public void show(Skill s)
	{
		gameObject.transform.FindChild("closebutton").GetComponent<FocusedSkillExitButtonController> ().reset ();
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro> ().text = WordingSkills.getName(s.Id);
		gameObject.transform.FindChild("CardType").GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnCardTypePicto (s.CardType.getPictureId());
		gameObject.transform.FindChild("SkillType").GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnSkillTypePicture (s.IdSkillType);
		gameObject.transform.FindChild("SkillType").FindChild("Title").GetComponent<TextMeshPro> ().text = WordingSkillTypes.getLetter(s.IdSkillType);
		gameObject.transform.FindChild("CardTypeTitle").GetComponent<TextMeshPro> ().text =WordingCardTypes.getName(s.CardType.Id);
		gameObject.transform.FindChild("CardTypeTitle").GetComponent<FocusedSkillCardTypeController>().setCardType(s.CardType.Id);
		gameObject.transform.FindChild("SkillTypeTitle").GetComponent<TextMeshPro> ().text = WordingSkillTypes.getName(s.IdSkillType);
		gameObject.transform.FindChild("SkillTypeTitle").GetComponent<FocusedSkillSkillTypeController>().setSkillType(s.IdSkillType);
		gameObject.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnSkillPicto(s.getPictureId());
		for(int i=0;i<10;i++)
		{
			gameObject.transform.FindChild("Skill"+i).FindChild("Title").GetComponent<TextMeshPro>().text=this.getDescription(s.Id,i);
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