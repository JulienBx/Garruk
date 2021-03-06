//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Linq;
//
//public class NewStoreModel
//{
//	public IList<string> cardTypeList;
//	public IList<Pack> packList;
//	public IList<DisplayedProduct> productList;
//	public IList<Skill> NewSkills;
//	public int CollectionPointsEarned;
//	public int CollectionPointsRanking;
//	public string Error;
//
//	private string URLGetStoreData = ApplicationModel.host + "get_store_data.php";
//	private string URLBuyPack = ApplicationModel.host + "buyPack.php";
//	
//	public NewStoreModel()
//	{
//		this.cardTypeList = new List<string> ();
//		this.packList = new List<Pack> ();
//	}
//	public IEnumerator initializeStore () 
//	{
//		WWWForm form = new WWWForm(); 											// Création de la connexion
//		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
//		form.AddField("myform_nick", ApplicationModel.player.Username);
//
//		ServerController.instance.setRequest(URLGetStoreData, form);
//		yield return ServerController.instance.StartCoroutine("executeRequest");
//
//		if(ServerController.instance.getError()=="")
//		{
//			string result = ServerController.instance.getResult();
//			string[] data=result.Split(new string[] { "END" }, System.StringSplitOptions.None);
//			this.cardTypeList = data[0].Split(new string[] { "\\" }, System.StringSplitOptions.None);
//			this.parsePlayer(data[1].Split(new string[] { "\\" }, System.StringSplitOptions.None));
//			this.packList=parsePacks(data[2].Split (new string[] {"PACK"}, System.StringSplitOptions.None));
//			this.productList=parseProducts(data[3].Split(new string[]{"PRODUCT"},System.StringSplitOptions.None));
//		}
//		else
//		{
//			Debug.Log(ServerController.instance.getError());
//			ServerController.instance.lostConnection();	
//		}
//	}
//	private void parsePlayer(string[] array)
//	{
//		ApplicationModel.player.CardTypesAllowed=new List<int>();
//		ApplicationModel.player.IsAdmin=System.Convert.ToBoolean(System.Convert.ToInt32(array[0]));
//		ApplicationModel.player.TutorialStep=System.Convert.ToInt32(array[1]);
//		ApplicationModel.player.NextLevelTutorial = System.Convert.ToBoolean (System.Convert.ToInt32 (array [2]));
//		for(int i = 3 ; i < array.Length-1 ; i++)
//		{
//			ApplicationModel.player.CardTypesAllowed.Add (System.Convert.ToInt32(array[i]));
//		}
//	}
//	private IList<Pack> parsePacks(string[] array)
//	{
//		IList<Pack> packList = new List<Pack> ();
//		
//		for(int i=0;i<array.Length-1;i++)
//		{
//			string[] packInformation = array[i].Split(new string[] { "\\" }, System.StringSplitOptions.None);
//			packList.Add (new Pack());
//			packList[i].Id = System.Convert.ToInt32(packInformation[0]);
//			packList[i].Name = WordingPacks.getName(System.Convert.ToInt32(packInformation[0])-1);
//			packList[i].NbCards=System.Convert.ToInt32(packInformation[1]);
//			packList[i].CardType = System.Convert.ToInt32(packInformation[2]);
//			packList[i].Price=System.Convert.ToInt32(packInformation[3]);
//			packList[i].New=System.Convert.ToBoolean(System.Convert.ToInt32(packInformation[4]));
//			packList[i].IdPicture=System.Convert.ToInt32(packInformation[5]);
//		}
//		return packList;
//	}
//	private IList<DisplayedProduct> parseProducts(string[] array)
//	{
//		IList<DisplayedProduct> productList = new List<DisplayedProduct> ();
//		
//		for(int i=0;i<array.Length-1;i++)
//		{
//			string[] PurchaseInformation = array[i].Split(new string[] { "\\" }, System.StringSplitOptions.None);
//			productList.Add (new DisplayedProduct());
//			productList[i].Id = System.Convert.ToInt32(PurchaseInformation[0]);
//			productList[i].PriceEUR = float.Parse(PurchaseInformation[1]);
//			productList[i].Crystals=float.Parse(PurchaseInformation[2]);
//			productList[i].ProductID=PurchaseInformation[3];
//			productList[i].ProductNameApple=PurchaseInformation[4];
//			productList[i].ProductNameGooglePlay=PurchaseInformation[5];
//			productList[i].PriceUSD = float.Parse(PurchaseInformation[6]);
//			productList[i].PriceGBP = float.Parse(PurchaseInformation[7]);
//			productList[i].IsActive = System.Convert.ToBoolean(System.Convert.ToInt32(PurchaseInformation[8]));
//		}
//		return productList;
//	}
//	public IEnumerator buyPack(int packId, int cardType, bool isTutorialPack, bool isTrainingPack)
//	{
//		string isTutorialPackToString = "0";
//		if(isTutorialPack)
//		{
//			isTutorialPackToString="1";
//		}
//		string isTrainingPackToString = "0";
//		if(isTrainingPack)
//		{
//			isTrainingPackToString="1";
//		}
//
//		this.packList[packId].Cards = new Cards ();
//		this.NewSkills = new List<Skill> ();
//		this.CollectionPointsEarned = -1;
//		this.CollectionPointsRanking = -1;
//		this.Error = "";
//		
//		WWWForm form = new WWWForm(); 											// Création de la connexion
//		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
//		form.AddField("myform_nick", ApplicationModel.player.Username);
//		form.AddField("myform_Id", this.packList[packId].Id.ToString());	
//		form.AddField("myform_cardtype", cardType.ToString());	
//		form.AddField("myform_istutorialpack", isTutorialPackToString);
//		form.AddField("myform_istrainingpack", isTrainingPackToString);
//
//		ServerController.instance.setRequest(URLBuyPack, form);
//		yield return ServerController.instance.StartCoroutine("executeRequest");
//		this.Error=ServerController.instance.getError();
//
//		if(this.Error=="")
//		{
//			string result = ServerController.instance.getResult();
//			string[] data = result.Split(new string[] { "END" }, System.StringSplitOptions.None);
//			if(data[0]!="")
//			{
//				this.NewSkills=parseNewSkills(data[0].Split(new string[] { "#S#" }, System.StringSplitOptions.None));
//			}
//			this.packList[packId].Cards.parseCards(data[1]);
//			this.CollectionPointsEarned=System.Convert.ToInt32(data[2]);
//			this.CollectionPointsRanking=System.Convert.ToInt32(data[3]);
//		}
//	}
//	public List<Skill> parseNewSkills(string[] array)
//	{
//		List<Skill> newSkills = new List<Skill>();
//		for(int i = 0 ; i < array.Length-1 ; i++)
//		{
//			newSkills.Add(new Skill());
//			newSkills[i].Id=System.Convert.ToInt32(array[i]);
//		}
//		return newSkills;
//	}
//}
//
