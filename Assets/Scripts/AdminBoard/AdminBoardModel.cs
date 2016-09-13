using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Reflection;
using System.Linq;


public class AdminBoardModel
{

	private string URLGetAdminBoardData = ApplicationModel.host + "get_adminboard_data.php";
	private string URLRefreshPeriod = ApplicationModel.host + "refresh_adminboard_period.php";
	public int connectionsToday;
	public int cardsRenamedToday;
	public int packBoughtToday;
	public int xpBoughtToday;
	public int cardBoughtToday;
	public int cardSoldToday;
	public int totalConnectionsToday;
	public int totalCardsRenamedToday;
	public int totalPackBoughtToday;
	public int totalXpBoughtToday;
	public int totalCardBoughtToday;
	public int totalCardSoldToday;
	public int nbPlayersToday;
	public int totalNbPlayersToday;
	public int connectionsOnPeriod;
	public int cardsRenamedOnPeriod;
	public int packBoughtOnPeriod;
	public int xpBoughtOnPeriod;
	public int cardBoughtOnPeriod;
	public int cardSoldOnPeriod;
	public int totalConnectionsOnPeriod;
	public int totalCardsRenamedOnPeriod;
	public int totalPackBoughtOnPeriod;
	public int totalXpBoughtOnPeriod;
	public int totalCardBoughtOnPeriod;
	public int totalCardSoldOnPeriod;
	public int nbPlayersOnPeriod;
	public int totalNbPlayersOnPeriod;
	
	public AdminBoardModel ()
	{
	}
	public IEnumerator getAdminBoardData(DateTime startPeriod, DateTime endPeriod)
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_startperiod", startPeriod.ToString("yyyy-MM-dd"));
		form.AddField("myform_endperiod", endPeriod.ToString("yyyy-MM-dd"));

		ServerController.instance.setRequest(URLGetAdminBoardData, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");

		if(ServerController.instance.getError()=="")
		{
			string result = ServerController.instance.getResult();
			string[] data=result.Split(new string[] { "//" }, System.StringSplitOptions.None);
			this.connectionsToday=System.Convert.ToInt32(data[0]);
			this.cardBoughtToday=System.Convert.ToInt32(data[1]);
			this.cardsRenamedToday=System.Convert.ToInt32(data[2]);
			this.xpBoughtToday=System.Convert.ToInt32(data[3]);
			this.packBoughtToday=System.Convert.ToInt32(data[4]);
			this.cardSoldToday=System.Convert.ToInt32(data[5]);
			this.totalConnectionsToday=System.Convert.ToInt32(data[6]);
			this.totalCardBoughtToday=System.Convert.ToInt32(data[7]);
			this.totalCardsRenamedToday=System.Convert.ToInt32(data[8]);
			this.totalXpBoughtToday=System.Convert.ToInt32(data[9]);
			this.totalCardSoldToday=System.Convert.ToInt32(data[10]);
			this.totalPackBoughtToday=System.Convert.ToInt32(data[11]);
			this.nbPlayersToday=System.Convert.ToInt32(data[12]);
			this.totalNbPlayersToday=System.Convert.ToInt32(data[13]);
			this.connectionsOnPeriod=System.Convert.ToInt32(data[14]);
			this.cardBoughtOnPeriod=System.Convert.ToInt32(data[15]);
			this.cardsRenamedOnPeriod=System.Convert.ToInt32(data[16]);
			this.xpBoughtOnPeriod=System.Convert.ToInt32(data[17]);
			this.packBoughtOnPeriod=System.Convert.ToInt32(data[18]);
			this.cardSoldOnPeriod=System.Convert.ToInt32(data[19]);
			this.totalConnectionsOnPeriod=System.Convert.ToInt32(data[20]);
			this.totalCardBoughtOnPeriod=System.Convert.ToInt32(data[21]);
			this.totalCardsRenamedOnPeriod=System.Convert.ToInt32(data[22]);
			this.totalXpBoughtOnPeriod=System.Convert.ToInt32(data[23]);
			this.totalPackBoughtOnPeriod=System.Convert.ToInt32(data[24]);
			this.totalCardSoldOnPeriod=System.Convert.ToInt32(data[25]);
			this.nbPlayersOnPeriod=System.Convert.ToInt32(data[26]);
			this.totalNbPlayersOnPeriod=System.Convert.ToInt32(data[27]);
		}
		else
		{
			Debug.Log(ServerController.instance.getError());
			BackOfficeController.instance.displayDetectOfflinePopUp ();
		}
	}
	public IEnumerator refreshPeriod(DateTime startPeriod, DateTime endPeriod)
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_startperiod", startPeriod.ToString("yyyy-MM-dd"));
		form.AddField("myform_endperiod", endPeriod.ToString("yyyy-MM-dd"));
		
		ServerController.instance.setRequest(URLRefreshPeriod, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");

		if(ServerController.instance.getError()=="")
		{
			string result = ServerController.instance.getResult();
			string[] data=result.Split(new string[] { "//" }, System.StringSplitOptions.None);
			this.connectionsOnPeriod=System.Convert.ToInt32(data[0]);
			this.cardBoughtOnPeriod=System.Convert.ToInt32(data[1]);
			this.cardsRenamedOnPeriod=System.Convert.ToInt32(data[2]);
			this.xpBoughtOnPeriod=System.Convert.ToInt32(data[3]);
			this.packBoughtOnPeriod=System.Convert.ToInt32(data[4]);
			this.cardSoldOnPeriod=System.Convert.ToInt32(data[5]);
			this.totalConnectionsOnPeriod=System.Convert.ToInt32(data[6]);
			this.totalCardBoughtOnPeriod=System.Convert.ToInt32(data[7]);
			this.totalCardsRenamedOnPeriod=System.Convert.ToInt32(data[8]);
			this.totalXpBoughtOnPeriod=System.Convert.ToInt32(data[9]);
			this.totalPackBoughtOnPeriod=System.Convert.ToInt32(data[10]);
			this.totalCardSoldOnPeriod=System.Convert.ToInt32(data[11]);
			this.nbPlayersOnPeriod=System.Convert.ToInt32(data[12]);
			this.totalNbPlayersOnPeriod=System.Convert.ToInt32(data[13]);
		}
		else
		{
			Debug.Log(ServerController.instance.getError());
			//ServerController.instance.lostConnection();	
		}
	}
}

