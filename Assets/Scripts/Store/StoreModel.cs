using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class StoreModel
{
	private string URLGetStoreData = ApplicationModel.host + "get_store_data.php";
	public User player;
	public IList<string> cardTypeList;
	public IList<Pack> packList;
	
	public StoreModel()
	{
	}
	public IEnumerator initializeStore () 
	{
		this.player = new User ();
		this.cardTypeList = new List<string> ();
		this.packList = new List<Pack> ();
		
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
		for(int i = 1 ; i < array.Length-1 ; i++)
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
			packList[i].Picture=packInformation[6];
		}
		return packList;
	}
}

