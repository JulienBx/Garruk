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
	public IList<Skill> ownSkillsList;
	public IList<string> cardTypesList;
	public IList<int> cardIdsList;
	public IList<SkillType> skillTypesList;
	public User player;
	private string URLGetSkillBookData = ApplicationModel.host + "get_skillbook_data.php";
	
	public NewSkillBookModel ()
	{
	}
	public IEnumerator getSkillBookData()
	{
		this.player = new User ();
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		
		WWW w = new WWW(URLGetSkillBookData, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.cardTypesList = data[0].Split(new string[] { "//" }, System.StringSplitOptions.None);
			this.skillsList = parseSkills(data[1].Split(new string[] { "#SKILL#" }, System.StringSplitOptions.None));
			this.ownSkillsList = parseOwnSkills(data[2].Split(new string[] { "#SKILL#" }, System.StringSplitOptions.None));
			this.cardIdsList = parseCards(data[3].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.player.SkillBookTutorial=System.Convert.ToBoolean(System.Convert.ToInt32(data[4]));
			this.skillTypesList = parseSkillTypes(data[5].Split(new string[] {"#SKILLTYPE#"},System.StringSplitOptions.None));
			this.affectSkillTypes();
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
			skills[i].Name=skillInformation[1];
			skills[i].CardType=System.Convert.ToInt32(skillInformation[2]);
			skills[i].cible=System.Convert.ToInt32(skillInformation[3]);
			skills[i].AllDescriptions=new string[10];
			for(int j=0;j<skills[i].AllDescriptions.Length;j++)
			{
				skills[i].AllDescriptions[j]=skillInformation[4+j];
			}
			skills[i].AllProbas=new int[10];
			for(int j=0;j<skills[i].AllProbas.Length;j++)
			{
				skills[i].AllProbas[j]=System.Convert.ToInt32(skillInformation[14+j]);
			}
		}
		return skills;
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
		}
		return skills;
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
	private void affectSkillTypes()
	{
		for(int i=0;i<this.skillsList.Count;i++)
		{
			for(int j=0;j<this.skillTypesList.Count;j++)
			{
				if(this.skillsList[i].cible==this.skillTypesList[j].Id)
				{
					this.skillsList[i].SkillType=this.skillTypesList[j];
					break;
				}
			}
		}
	}
}

