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
		gameObject.transform.FindChild ("closebutton").GetComponent<FocusedSkillExitButtonController> ().reset ();
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = s.Name;
		gameObject.transform.FindChild("CardType").GetComponent<SpriteRenderer> ().sprite = MenuController.instance.returnCardTypePicture (s.CardType.IdPicture);
		gameObject.transform.FindChild("SkillType").GetComponent<SpriteRenderer> ().sprite = MenuController.instance.returnSkillTypePicture (s.SkillType.IdPicture);
		gameObject.transform.FindChild("SkillType").FindChild("Title").GetComponent<TextMeshPro> ().text = s.SkillType.Name.Substring (0, 1).ToUpper();
		gameObject.transform.FindChild ("CardTypeTitle").GetComponent<TextMeshPro> ().text = s.CardType.Name;
		gameObject.transform.FindChild ("SkillTypeTitle").GetComponent<TextMeshPro> ().text = s.SkillType.Name;
		for(int i=0;i<10;i++)
		{
			gameObject.transform.FindChild("Skill"+i).FindChild("Title").GetComponent<TextMeshPro>().text=s.AllDescriptions[i];
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
	public virtual void exit()
	{
	}
}