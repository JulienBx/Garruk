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
	private string URLCreateRandomCards = ApplicationModel.host + "buyRandomCards.php";
	public User player;
	public IList<string> cardTypeList;
	public Card[] randomCards;
	public string error;
	
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
	public IEnumerator create5RandomCards(int cost, int cardType=-1)
	{
		this.randomCards = new Card[5];
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_cost", cost.ToString());	
		form.AddField ("myform_cardtype", cardType.ToString ());
		
		WWW w = new WWW(URLCreateRandomCards, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null)
		{
			this.error = w.error;
		} 
		else
		{
			string[] data = w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.error = data [0];
			if (this.error == "")
			{
				for(int i=1;i<6;i++)
				{
					this.randomCards[i-1]=parseCard(data [i].Split(new string[] { "\n" }, System.StringSplitOptions.None));
				}
			}
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
	private Card parseCard(string[] array)
	{
		Card card = new Card ();

		string[] cardInformation = array[0].Split(new string[] { "//" }, System.StringSplitOptions.None);
		card.Id = System.Convert.ToInt32(cardInformation [0]);
		card.Title = cardInformation [1];
		card.Life = System.Convert.ToInt32(cardInformation [2]);
		card.Attack = System.Convert.ToInt32(cardInformation [3]);
		card.Speed = System.Convert.ToInt32(cardInformation [4]);
		card.Move = System.Convert.ToInt32(cardInformation [5]);
		card.ArtIndex = System.Convert.ToInt32(cardInformation [6]);
		card.IdClass = System.Convert.ToInt32(cardInformation [7]);
		card.TitleClass = cardInformation [8];
		card.LifeLevel = System.Convert.ToInt32(cardInformation [9]);
		card.MoveLevel = System.Convert.ToInt32(cardInformation [10]);
		card.SpeedLevel = System.Convert.ToInt32(cardInformation [11]);
		card.AttackLevel = System.Convert.ToInt32(cardInformation [12]);
		card.onSale = 0;
		card.Experience = 0;

		card.Skills = new List<Skill>();
		for (int i = 1; i < 5; i++)
		{         
			cardInformation = array [i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			card.Skills.Add(new Skill(cardInformation [0], //skillName
			                          System.Convert.ToInt32(cardInformation [1]), // idskill
			                          System.Convert.ToInt32(cardInformation [2]), // isactivated
			                          System.Convert.ToInt32(cardInformation [3]), // level
			                          System.Convert.ToInt32(cardInformation [4]), // power
			                          System.Convert.ToInt32(cardInformation [5]), // manaCost
			                          cardInformation [6])); // description
		}
		return card;
	}
}

