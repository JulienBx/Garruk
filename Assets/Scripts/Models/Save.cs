using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary; 

[System.Serializable]
public class Save { 

	public Player player;

	public float volBackOfficeFx;
	public float volMusic;

	public Packs packs;
	public DisplayedProducts products;

	public CardTypes cardTypes;
	public SkillTypes skillTypes;
	public Skills skills;
	public List<int> xpLevels;
	public Divisions divisions;

	public int friendlyGameEarnXp_W;
	public int friendlyGameEarnXp_L;
	public int friendlyGameEarnCredits_W;
	public int friendlyGameEarnCredits_L;

	public int trainingGameEarnXp_W;
	public int trainingGameEarnXp_L;
	public int trainingGameEarnCredits_W;
	public int trainingGameEarnCredits_L;

	public Save () 
	{
		player=new Player();
		volBackOfficeFx=new float();
		volMusic=new float();
		packs=new Packs();
		products=new DisplayedProducts();
		cardTypes=new CardTypes();
		skillTypes=new SkillTypes();
		skills=new Skills();
		xpLevels=new List<int>();
		divisions=new Divisions();
		friendlyGameEarnXp_W=new int();
		friendlyGameEarnXp_L=new int();
		friendlyGameEarnCredits_W=new int();
		friendlyGameEarnCredits_L=new int();
		trainingGameEarnXp_W=new int();
		trainingGameEarnXp_L=new int();
		trainingGameEarnCredits_W=new int();
		trainingGameEarnCredits_L=new int();
	}
	public void create()
	{
		player=ApplicationModel.player;
		volBackOfficeFx=ApplicationModel.volBackOfficeFx;
		volMusic = ApplicationModel.volMusic;
		packs = ApplicationModel.packs;
		products=ApplicationModel.products;
		cardTypes=ApplicationModel.cardTypes;
		skillTypes=ApplicationModel.skillTypes;
		skills=ApplicationModel.skills;
		xpLevels=ApplicationModel.xpLevels;
		divisions=ApplicationModel.divisions;
		friendlyGameEarnXp_W=ApplicationModel.friendlyGameEarnXp_W;
		friendlyGameEarnXp_L=ApplicationModel.friendlyGameEarnXp_L;
		friendlyGameEarnCredits_W=ApplicationModel.friendlyGameEarnCredits_W;
		friendlyGameEarnCredits_L=ApplicationModel.friendlyGameEarnCredits_L;
		trainingGameEarnXp_W=ApplicationModel.trainingGameEarnXp_W;
		trainingGameEarnXp_L=ApplicationModel.trainingGameEarnXp_L;
		trainingGameEarnCredits_W=ApplicationModel.trainingGameEarnCredits_W;
		trainingGameEarnCredits_L=ApplicationModel.trainingGameEarnCredits_L;
	}
	public void load()
	{
		ApplicationModel.player=player;
		ApplicationModel.volBackOfficeFx=volBackOfficeFx;
		ApplicationModel.volMusic=volMusic;
		ApplicationModel.packs=packs;
		ApplicationModel.products=products;
		ApplicationModel.cardTypes=cardTypes;
		ApplicationModel.skillTypes=skillTypes;
		ApplicationModel.skills=skills;
		ApplicationModel.xpLevels=xpLevels;
		ApplicationModel.divisions=divisions;
		ApplicationModel.friendlyGameEarnXp_W=friendlyGameEarnXp_W;
		ApplicationModel.friendlyGameEarnXp_L=friendlyGameEarnXp_L;
		ApplicationModel.friendlyGameEarnCredits_W=friendlyGameEarnCredits_W;
		ApplicationModel.friendlyGameEarnCredits_L=friendlyGameEarnCredits_L;
		ApplicationModel.trainingGameEarnXp_W=trainingGameEarnXp_W;
		ApplicationModel.trainingGameEarnXp_L=trainingGameEarnXp_L;
		ApplicationModel.trainingGameEarnCredits_W=trainingGameEarnCredits_W;
		ApplicationModel.trainingGameEarnCredits_L=trainingGameEarnCredits_L;
	}
	public string retrieveDataToSync()
	{
		string data = "";
		player.cardsToSync.setString ();
		Debug.Log(player.decksToSync.getCount());
		player.decksToSync.setString ();
		player.moneyToSync.ToString ();
		data += player.cardsToSync.String+"DATAEND";
		data += player.decksToSync.String + "DATAEND";
		data += player.moneyToSync.ToString () + "DATAEND";

		return data;
	}
}

