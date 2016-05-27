using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class AdminBoardController : MonoBehaviour
{

	public static AdminBoardController instance;
	private GameObject serverController;
	private AdminBoardModel model;
	private AdminBoardView view;
	public GameObject MenuObject;
	public GUIStyle[] adminBoardVMStyle;

	private DateTime startPeriod;
	private DateTime endPeriod;

	void Start()
	{
		instance = this;
		this.startPeriod = DateTime.Now.AddDays (-7);
		this.endPeriod = DateTime.Now;
		this.model = new AdminBoardModel ();
		this.view = Camera.main.gameObject.AddComponent <AdminBoardView>();
		this.MenuObject = Instantiate(this.MenuObject) as GameObject;
		StartCoroutine (this.initialization());
	}
	private IEnumerator initialization ()
	{
		this.initStyles ();
		this.resize ();
		yield return StartCoroutine (model.getAdminBoardData (startPeriod,endPeriod));
		view.VM.connectionsToday=model.connectionsToday.ToString();
		view.VM.playersToday = model.nbPlayersToday.ToString ();
		view.VM.cardsRenamedToday=model.cardsRenamedToday.ToString();
		view.VM.packBoughtToday=model.packBoughtToday.ToString();
		view.VM.xpBoughtToday=model.xpBoughtToday.ToString();
		view.VM.cardBoughtToday=model.cardBoughtToday.ToString();
		view.VM.cardSoldToday = model.cardSoldToday.ToString ();
		view.VM.connectionsOnPeriod=model.connectionsOnPeriod.ToString();
		view.VM.playersOnPeriod = model.nbPlayersOnPeriod.ToString ();
		view.VM.cardsRenamedOnPeriod=model.cardsRenamedOnPeriod.ToString();
		view.VM.packBoughtOnPeriod=model.packBoughtOnPeriod.ToString();
		view.VM.xpBoughtOnPeriod=model.xpBoughtOnPeriod.ToString();
		view.VM.cardBoughtOnPeriod=model.cardBoughtOnPeriod.ToString();
		view.VM.cardSoldOnPeriod = model.cardSoldOnPeriod.ToString ();
		view.VM.startPeriod = this.startPeriod.ToString ("dd/MM/yyyy");
		view.VM.endPeriod = this.endPeriod.ToString ("dd/MM/yyyy");
	}
	public void refreshPeriodHandler()
	{
		view.VM.error = this.checkDate (view.VM.startPeriod);
		if(view.VM.error=="")
		{
			view.VM.error = this.checkDate (view.VM.endPeriod);
		}
		if(view.VM.error=="")
		{
			this.startPeriod=DateTime.ParseExact(view.VM.startPeriod, "dd/MM/yyyy",null);
			this.endPeriod=DateTime.ParseExact(view.VM.endPeriod, "dd/MM/yyyy",null);
		}
		if (this.startPeriod > this.endPeriod) 
		{
			view.VM.error="La date de début de période ne peut être supérieure à la date de fin de période";
		}
		else
		{
			StartCoroutine (this.refreshPeriod());
		}
	}
	public string checkDate(string date)
	{
		if(!Regex.IsMatch(date, @"^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"))
		{
			return "Format de date non valide";
		}   
		return "";
	}
	private IEnumerator refreshPeriod()
	{
		yield return StartCoroutine (model.refreshPeriod (this.startPeriod,this.endPeriod));
		view.VM.connectionsOnPeriod=model.connectionsOnPeriod.ToString();
		view.VM.cardsRenamedOnPeriod=model.cardsRenamedOnPeriod.ToString();
		view.VM.packBoughtOnPeriod=model.packBoughtOnPeriod.ToString();
		view.VM.xpBoughtOnPeriod=model.xpBoughtOnPeriod.ToString();
		view.VM.cardBoughtOnPeriod=model.cardBoughtOnPeriod.ToString();
		view.VM.cardSoldOnPeriod = model.cardSoldOnPeriod.ToString ();
	}
	public void resize()
	{
		view.VM.resize ();
	}
	private void initStyles()
	{
		view.VM.styles=new GUIStyle[this.adminBoardVMStyle.Length];
		for(int i=0;i<this.adminBoardVMStyle.Length;i++)
		{
			view.VM.styles[i]=this.adminBoardVMStyle[i];
		}
		view.VM.initStyles();
	}
	public void filterData(bool toggle)
	{
		view.VM.filteredStats = toggle;
		if (toggle) 
		{
			view.VM.connectionsToday=model.connectionsToday.ToString();
			view.VM.playersToday = model.nbPlayersToday.ToString ();
			view.VM.cardsRenamedToday=model.cardsRenamedToday.ToString();
			view.VM.packBoughtToday=model.packBoughtToday.ToString();
			view.VM.xpBoughtToday=model.xpBoughtToday.ToString();
			view.VM.cardBoughtToday=model.cardBoughtToday.ToString();
			view.VM.cardSoldToday = model.cardSoldToday.ToString ();
			view.VM.connectionsOnPeriod=model.connectionsOnPeriod.ToString();
			view.VM.playersOnPeriod = model.nbPlayersOnPeriod.ToString ();
			view.VM.cardsRenamedOnPeriod=model.cardsRenamedOnPeriod.ToString();
			view.VM.packBoughtOnPeriod=model.packBoughtOnPeriod.ToString();
			view.VM.xpBoughtOnPeriod=model.xpBoughtOnPeriod.ToString();
			view.VM.cardBoughtOnPeriod=model.cardBoughtOnPeriod.ToString();
			view.VM.cardSoldOnPeriod = model.cardSoldOnPeriod.ToString ();
		}
		else
		{
			view.VM.connectionsToday=model.totalConnectionsToday.ToString();
			view.VM.playersToday = model.totalNbPlayersToday.ToString ();
			view.VM.cardsRenamedToday=model.totalCardsRenamedToday.ToString();
			view.VM.packBoughtToday=model.totalPackBoughtToday.ToString();
			view.VM.xpBoughtToday=model.totalXpBoughtToday.ToString();
			view.VM.cardBoughtToday=model.totalCardBoughtToday.ToString();
			view.VM.cardSoldToday = model.totalCardSoldToday.ToString ();
			view.VM.connectionsOnPeriod=model.totalConnectionsOnPeriod.ToString();
			view.VM.playersOnPeriod = model.totalNbPlayersOnPeriod.ToString ();
			view.VM.cardsRenamedOnPeriod=model.totalCardsRenamedOnPeriod.ToString();
			view.VM.packBoughtOnPeriod=model.totalPackBoughtOnPeriod.ToString();
			view.VM.xpBoughtOnPeriod=model.totalXpBoughtOnPeriod.ToString();
			view.VM.cardBoughtOnPeriod=model.totalCardBoughtOnPeriod.ToString();
			view.VM.cardSoldOnPeriod = model.totalCardSoldOnPeriod.ToString ();
		}
	}
}

