﻿//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//
//public class PlayPopUpModel {
//	
//	public IList<Deck> decks;
//	
//	private string URLGetUserPlayingData = ApplicationModel.host+"get_user_playing_data.php";
//	
//	public PlayPopUpModel()
//	{
//	}
//	public IEnumerator loadUserData()
//	{
//		WWWForm form = new WWWForm(); 								// Création de la connexion
//		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
//		form.AddField("myform_nick", ApplicationModel.player.Username); 	// Pseudo de l'utilisateur connecté
//		form.AddField ("myform_nbcardsbydeck", ApplicationModel.nbCardsByDeck.ToString ());
//
//		ServerController.instance.setRequest(URLGetUserPlayingData, form);
//		yield return ServerController.instance.StartCoroutine("executeRequest");
//
//		if(ServerController.instance.getError()=="")
//		{
//			string result = ServerController.instance.getResult();
//			string[] data=result.Split(new string[] { "END" }, System.StringSplitOptions.None);
//			ApplicationModel.player.SelectedDeckId=System.Convert.ToInt32(data [0]);
//            ApplicationModel.player.RankingPoints=System.Convert.ToInt32(data [1]);
//			this.decks = this.parseDecks(data[2].Split(new string[] { "#DECK#" }, System.StringSplitOptions.None));
//		}
//		else
//		{
//			Debug.Log(ServerController.instance.getError());
//			ServerController.instance.lostConnection();	
//		}
//	}
//	private List<Deck> parseDecks(string[] decksData)
//	{
//		List<Deck> decks = new List<Deck> ();
//		for(int i=0;i<decksData.Length-1;i++)
//		{
//			string[] deckInformation = decksData[i].Split(new string[] { "#DECKINFO#" }, System.StringSplitOptions.None);
//			decks.Add (new Deck());
//			decks[i].Id=System.Convert.ToInt32(deckInformation[0]);
//			decks[i].Name=deckInformation[1];
//			decks[i].NbCards=System.Convert.ToInt32(deckInformation[2]);
//			decks[i].cards=new List<Card>();
//			for(int j=0;j<decks[i].NbCards;j++)
//			{
//				decks[i].cards.Add(new Card());
//				decks[i].cards[j].CardType=new CardType();
//				decks[i].cards[j].CardType.Id=System.Convert.ToInt32(deckInformation[3+j]);
//			}
//		}
//		return decks;
//	}
//}
//
