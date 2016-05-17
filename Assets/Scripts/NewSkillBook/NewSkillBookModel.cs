using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;


public class NewSkillBookModel
{
	public IList<Skill> skillsList;
	public IList<int> ownSkillsIdList;
	public IList<Skill> ownSkillsList;
	public IList<CardType> cardTypesList;
	public IList<int> cardIdsList;
	public IList<SkillType> skillTypesList;
	private string URLGetSkillBookData = ApplicationModel.host + "get_skillbook_data.php";
	
	public NewSkillBookModel ()
	{
	}
	public IEnumerator getSkillBookData()
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.player.Username);

		ServerController.instance.setRequest(URLGetSkillBookData, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");

		if(ServerController.instance.getError()=="")
		{
			string result = ServerController.instance.getResult();
			string[] data=result.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.cardTypesList = parseCardTypes(data[0].Split(new string[] { "#CARDTYPE#" }, System.StringSplitOptions.None));
			this.skillsList = parseSkills(data[1].Split(new string[] { "#SKILL#" }, System.StringSplitOptions.None));
			this.ownSkillsList = parseOwnSkills(data[2].Split(new string[] { "#SKILL#" }, System.StringSplitOptions.None));
			this.ownSkillsIdList= parseIdSkills(data[2].Split(new string[] { "#SKILL#" }, System.StringSplitOptions.None));
			this.cardIdsList = parseCards(data[3].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.parsePlayer(data[4].Split (new string[]{"//"},System.StringSplitOptions.None));
			this.skillTypesList = parseSkillTypes(data[5].Split(new string[] {"#SKILLTYPE#"},System.StringSplitOptions.None));
			this.affectUserSkills();
		}
		else
		{
			Debug.Log(ServerController.instance.getError());
			ServerController.instance.lostConnection();	
		}
	}
	private IList<Skill> parseSkills(string[] array)
	{
		IList<Skill> skills = new List<Skill> ();
		for(int i=0;i<array.Length-1;i++)
		{
			string[] skillInformation = array[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			skills.Add (new Skill());
			skills[i].Id=System.Convert.ToInt32(skillInformation[0]);
			skills[i].CardType=new CardType();
			skills[i].CardType.Id=System.Convert.ToInt32(skillInformation[1]);
			skills[i].IdSkillType=System.Convert.ToInt32(skillInformation[2]);
			skills[i].IsActiveSkill = System.Convert.ToBoolean(System.Convert.ToInt32(skillInformation[3]));
			skills[i].AllProbas=new int[10];
			for(int j=0;j<skills[i].AllProbas.Length;j++)
			{
				skills[i].AllProbas[j]=System.Convert.ToInt32(skillInformation[4+j]);
			}
		}
		return skills;
	}
	private IList<CardType> parseCardTypes(string[] array)
	{
		IList<CardType> cardTypes = new List<CardType> ();
		for(int i=0;i<array.Length-1;i++)
		{
			string[] cardTypeInformation = array[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			cardTypes.Add (new CardType());
			cardTypes[i].Id=System.Convert.ToInt32(cardTypeInformation[0]);
			cardTypes[i].Name=cardTypeInformation[1];
			cardTypes[i].Description=cardTypeInformation[2];
		}
		return cardTypes;
	}
	private IList<Skill> parseOwnSkills(string[] array)
	{
		IList<Skill> skills = new List<Skill> ();
		for(int i=0;i<array.Length-1;i++)
		{
			string[] skillInformation = array[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			skills.Add (new Skill());
			skills[i].Id=System.Convert.ToInt32(skillInformation[0]);
			skills[i].Power=System.Convert.ToInt32(skillInformation[1]);
			skills[i].Level=System.Convert.ToInt32(skillInformation[2]);
		}
		return skills;
	}
	private IList<int> parseIdSkills(string[] array)
	{
		IList<int> idSkills = new List<int> ();
		for(int i=0;i<array.Length-1;i++)
		{
			string[] skillInformation = array[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			if(!idSkills.Contains(System.Convert.ToInt32(skillInformation[0])))
			{
				idSkills.Add (new int());
				idSkills[idSkills.Count-1]=System.Convert.ToInt32(skillInformation[0]);
			}
		}
		return idSkills;
	}
	private IList<int> parseCards(string[] array)
	{
		IList<int> cards = new List<int> ();
		for(int i=0;i<array.Length-1;i++)
		{
			cards.Add (System.Convert.ToInt32(array[i]));
		}
		return cards;
	}
	private IList<SkillType> parseSkillTypes(string[] array)
	{
		IList<SkillType> skillTypes = new List<SkillType> ();
		for(int i=0;i<array.Length-1;i++)
		{
			string[] skillTypeInformation = array[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			skillTypes.Add (new SkillType());
			skillTypes[i].Id=System.Convert.ToInt32(skillTypeInformation[0]);
			skillTypes[i].Name=skillTypeInformation[1];
			skillTypes[i].Description=skillTypeInformation[2];
		}
		return skillTypes;
	}
	private void parsePlayer(string[] array)
	{
		ApplicationModel.player.CollectionPoints = System.Convert.ToInt32 (array [0]);
		ApplicationModel.player.CollectionRanking = System.Convert.ToInt32 (array [1]);
		ApplicationModel.player.TutorialStep = System.Convert.ToInt32(array [2]);
		ApplicationModel.player.SkillBookTutorial= System.Convert.ToBoolean(System.Convert.ToInt32(array [3]));
	}
	private void affectUserSkills()
	{
		for(int i=0;i<this.skillsList.Count;i++)
		{
			this.skillsList[i].Level=0;
			this.skillsList[i].Power=0;
			for(int j=0;j<this.ownSkillsList.Count;j++)
			{
				if(this.skillsList[i].Id==this.ownSkillsList[j].Id && this.skillsList[i].Power<this.ownSkillsList[j].Power)
				{
					this.skillsList[i].Level=this.ownSkillsList[j].Level;
					this.skillsList[i].Power=this.ownSkillsList[j].Power;
				}
			}
		}
	}
}