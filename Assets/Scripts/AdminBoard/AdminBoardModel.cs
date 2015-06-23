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
	public int connectionsOnPeriod;
	public int cardsRenamedOnPeriod;
	public int packBoughtOnPeriod;
	public int xpBoughtOnPeriod;
	public int cardBoughtOnPeriod;
	public int cardSoldOnPeriod;
	
	public AdminBoardModel ()
	{
	}
	public IEnumerator getAdminBoardData(DateTime startPeriod, DateTime endPeriod)
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_startperiod", startPeriod.ToString("yyyy-MM-dd"));
		form.AddField("myform_endperiod", endPeriod.ToString("yyyy-MM-dd"));
		
		WWW w = new WWW(URLGetAdminBoardData, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			string[] data=w.text.Split(new string[] { "//" }, System.StringSplitOptions.None);
			this.connectionsToday=System.Convert.ToInt32(data[0]);
			this.cardBoughtToday=System.Convert.ToInt32(data[1]);
			this.cardsRenamedToday=System.Convert.ToInt32(data[2]);
			this.xpBoughtToday=System.Convert.ToInt32(data[3]);
			this.packBoughtToday=System.Convert.ToInt32(data[4]);
			this.cardSoldToday=System.Convert.ToInt32(data[5]);
			this.connectionsOnPeriod=System.Convert.ToInt32(data[6]);
			this.cardBoughtOnPeriod=System.Convert.ToInt32(data[7]);
			this.cardsRenamedOnPeriod=System.Convert.ToInt32(data[8]);
			this.xpBoughtOnPeriod=System.Convert.ToInt32(data[9]);
			this.packBoughtOnPeriod=System.Convert.ToInt32(data[10]);
			this.cardSoldOnPeriod=System.Convert.ToInt32(data[11]);
		}
	}
	public IEnumerator refreshPeriod(DateTime startPeriod, DateTime endPeriod)
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_startperiod", startPeriod.ToString("yyyy-MM-dd"));
		form.AddField("myform_endperiod", endPeriod.ToString("yyyy-MM-dd"));
		
		WWW w = new WWW(URLRefreshPeriod, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			string[] data=w.text.Split(new string[] { "//" }, System.StringSplitOptions.None);
			this.connectionsOnPeriod=System.Convert.ToInt32(data[0]);
			this.cardBoughtOnPeriod=System.Convert.ToInt32(data[1]);
			this.cardsRenamedOnPeriod=System.Convert.ToInt32(data[2]);
			this.xpBoughtOnPeriod=System.Convert.ToInt32(data[3]);
			this.packBoughtOnPeriod=System.Convert.ToInt32(data[4]);
			this.cardSoldOnPeriod=System.Convert.ToInt32(data[5]);
		}
	}
}

