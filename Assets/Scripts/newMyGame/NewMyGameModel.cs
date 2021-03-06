//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Linq;
//
//public class NewMyGameModel
//{
//	public Cards cards;
//	public IList<Deck> decks;
//	public string[] cardTypeList;
//	public IList<Skill> skillsList;
//	
//	private string URLGetMyGameData = ApplicationModel.host + "get_mygame_data.php"; 
//	
//	public NewMyGameModel()
//	{
//	}
//	public IEnumerator initializeMyGame () 
//	{
//		this.skillsList = new List<Skill> ();
//		this.cards = new Cards();
//		
//		WWWForm form = new WWWForm(); 											// Création de la connexion
//		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
//		form.AddField("myform_nick", ApplicationModel.player.Username);
//
//		ServerController.instance.setRequest(URLGetMyGameData, form);
//		yield return ServerController.instance.StartCoroutine("executeRequest");
//
//		if(ServerController.instance.getError()=="")
//		{
//			string result = ServerController.instance.getResult();
//			string[] data=result.Split(new string[] { "END" }, System.StringSplitOptions.None);
//			this.cardTypeList = data[0].Split(new string[] { "\\" }, System.StringSplitOptions.None);
//			this.skillsList=parseSkills(data[1].Split(new string[] { "#SK#" }, System.StringSplitOptions.None));
//			if(data[2]!="")
//			{
//				this.cards.parseCards(data[2]);
//			}
//			this.decks=parseDecks(data[3].Split(new string[] { "#D#" }, System.StringSplitOptions.None));
//			this.parseUser(data[4].Split(new string[] { "\\" }, System.StringSplitOptions.None));
//			this.retrieveCardsDeck();
//		}
//		else
//		{
//			Debug.Log(ServerController.instance.getError());
//			ServerController.instance.lostConnection();	
//		}
//	}
//	private void retrieveCardsDeck()
//	{
//		for(int i=0;i<this.cards.getCount();i++)
//		{
//			this.cards.getCard(i).Decks=new List<int>();
//		}
//		for (int i=0;i<this.decks.Count;i++)
//		{
//			for(int j=0;j<this.decks[i].cards.Count;j++)
//			{
//				for(int k=0;k<this.cards.getCount();k++)
//				{
//					if(this.cards.getCard(k).Id==decks[i].cards[j].Id)
//					{
//						this.cards.getCard(k).Decks.Add (decks[i].Id);
//					}
//				}
//			}
//		}
//	}
//	private List<Skill> parseSkills(string[] skillsIds)
//	{
//		List<Skill> skillsList = new List<Skill>();
//		for(int i = 0 ; i < skillsIds.Length-1 ; i++)
//		{
//			string [] tempString = skillsIds[i].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
//			skillsList.Add(new Skill());
//			skillsList[i].Id=System.Convert.ToInt32(tempString[0]);
//		}
//		return skillsList;
//	}
//	private void parseUser(string[] array)
//	{
//		ApplicationModel.player.SelectedDeckId = System.Convert.ToInt32 (array [0]);
//		ApplicationModel.player.TutorialStep = System.Convert.ToInt32 (array [1]);
//		ApplicationModel.player.NextLevelTutorial=System.Convert.ToBoolean(System.Convert.ToInt32(array[2]));
//	}
//	public List<Deck> parseDecks(string[] decksIds)
//	{
//		string[] deckData = null;
//		string[] deckInfo = null;
//		
//		List<Deck> decks = new List<Deck> (); 
//		
//		for (int i = 0; i < decksIds.Length - 1; i++) 		// On boucle sur les attributs d'un deck
//		{
//			deckData = decksIds[i].Split(new string[] { "#C#" }, System.StringSplitOptions.None);
//			for(int j=0 ; j<deckData.Length-1;j++)
//			{
//				
//				deckInfo=deckData[j].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
//				if(j==0)
//				{
//					
//					decks.Add(new Deck());
//					decks[i].Id=System.Convert.ToInt32(deckInfo [0]);
//					decks[i].Name=deckInfo [1];
//					decks[i].NbCards=System.Convert.ToInt32(deckInfo [2]);
//					
//					decks[i].cards = new List<Card>();
//				}
//				else
//				{
//					decks[i].cards.Add(new Card ());
//					decks[i].cards[j-1].Id=System.Convert.ToInt32(deckInfo [0]);
//					decks[i].cards[j-1].deckOrder=System.Convert.ToInt32(deckInfo[1]);
//				}
//			}	                     
//		}
//		return decks;
//	}
//}
//
