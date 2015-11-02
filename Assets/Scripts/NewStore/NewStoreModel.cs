using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class NewStoreModel
{
	public User player;
	public IList<string> cardTypeList;
	public IList<Pack> packList;
	public IList<Skill> NewSkills;
	public int CollectionPointsEarned;
	public int CollectionPointsRanking;
	public string Error;

	private string URLGetStoreData = ApplicationModel.host + "get_store_data.php";
	private string URLBuyPack = ApplicationModel.host + "buyPack.php";
	
	public NewStoreModel()
	{
		this.player = new User ();
		this.cardTypeList = new List<string> ();
		this.packList = new List<Pack> ();
	}
	public IEnumerator initializeStore () 
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		
		WWW w = new WWW(URLGetStoreData, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.cardTypeList = data[0].Split(new string[] { "\\" }, System.StringSplitOptions.None);
			this.player=parsePlayer(data[1].Split(new string[] { "\\" }, System.StringSplitOptions.None));
			this.packList=parsePacks(data[2].Split (new string[] {"PACK"}, System.StringSplitOptions.None));
		}
	}
	private User parsePlayer(string[] array)
	{
		User player = new User();
		player.CardTypesAllowed=new List<int>();
		player.IsAdmin=System.Convert.ToBoolean(System.Convert.ToInt32(array[0]));
		player.TutorialStep=System.Convert.ToInt32(array[1]);
		for(int i = 2 ; i < array.Length-1 ; i++)
		{
			player.CardTypesAllowed.Add (System.Convert.ToInt32(array[i]));
		}
		return player;
	}
	private IList<Pack> parsePacks(string[] array)
	{
		IList<Pack> packList = new List<Pack> ();
		
		for(int i=0;i<array.Length-1;i++)
		{
			string[] packInformation = array[i].Split(new string[] { "\\" }, System.StringSplitOptions.None);
			packList.Add (new Pack());
			packList[i].Id = System.Convert.ToInt32(packInformation[0]);
			packList[i].Name = packInformation[1];
			packList[i].NbCards=System.Convert.ToInt32(packInformation[2]);
			packList[i].CardType = System.Convert.ToInt32(packInformation[3]);
			packList[i].Price=System.Convert.ToInt32(packInformation[4]);
			packList[i].New=System.Convert.ToBoolean(System.Convert.ToInt32(packInformation[5]));
			packList[i].IdPicture=System.Convert.ToInt32(packInformation[6]);
		}
		return packList;
	}
	public IEnumerator buyPack(int packId, int cardType=-1)
	{
		this.packList[packId].Cards = new Cards ();
		this.NewSkills = new List<Skill> ();
		this.CollectionPointsEarned = -1;
		this.CollectionPointsRanking = -1;
		this.Error = "";
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_Id", this.packList[packId].Id.ToString());	
		form.AddField("myform_cardtype", cardType.ToString());	
		
		WWW w = new WWW(URLBuyPack, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null)
		{
			this.Error = w.error;
		} 
		else
		{
			if(w.text.Contains("#ERROR#"))
			{
				Debug.Log(w.text);
				string[] errors = w.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
				this.Error=errors[1];
			}
			else
			{
				this.Error="";
				string[] data = w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
				if(data[0]!="")
				{
					this.NewSkills=parseNewSkills(data[0].Split(new string[] { "#S#" }, System.StringSplitOptions.None));
				}
				this.packList[packId].Cards.parseCards(data[1]);
				this.CollectionPointsEarned=System.Convert.ToInt32(data[2]);
				this.CollectionPointsRanking=System.Convert.ToInt32(data[3]);
			}
		}
	}
	public List<Skill> parseNewSkills(string[] array)
	{
		List<Skill> newSkills = new List<Skill>();
		for(int i = 0 ; i < array.Length-1 ; i++)
		{
			newSkills.Add(new Skill());
			newSkills[i].Name=array[i];
		}
		return newSkills;
	}
}

