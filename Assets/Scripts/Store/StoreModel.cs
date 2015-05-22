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
	
	public StoreModel()
	{
	}
	public IEnumerator initializeStore () 
	{
		this.player = new User ();
		this.cardTypeList = new List<string> ();
		
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
}

