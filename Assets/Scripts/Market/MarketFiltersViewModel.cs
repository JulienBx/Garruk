using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MarketFiltersViewModel {
	
	public GUIStyle[] styles;
	public GUIStyle[] sortButtonStyle;
	public bool[] togglesCurrentStates;
	public bool isSkillChosen;
	public bool isSkillToDisplay;
	public int sortSelected;
	public int oldSortSelected;
	public IList<int> filtersCardType;
	public IList<string> matchValues;
	public GUIStyle filterTitleStyle;
	public GUIStyle textFieldStyle;
	public GUIStyle toggleStyle;
	public GUIStyle smallPoliceStyle;
	public GUIStyle minmaxPriceStyle;
	public GUIStyle skillListStyle;

	public string[] cardTypeList;
	public string minPrice;
	public string maxPrice;
	public string valueSkill;
	public float minLifeVal;
	public float maxLifeVal;
	public float minLifeLimit;
	public float maxLifeLimit;
	public float minAttackVal;
	public float maxAttackVal;
	public float minAttackLimit;
	public float maxAttackLimit;
	public float minMoveVal;
	public float maxMoveVal;
	public float minMoveLimit;
	public float maxMoveLimit;
	public float minQuicknessVal;
	public float maxQuicknessVal;
	public float minQuicknessLimit;
	public float maxQuicknessLimit;
	public float oldMinLifeVal;
	public float oldMaxLifeVal;
	public float oldMinAttackVal;
	public float oldMaxAttackVal;
	public float oldMinMoveVal;
	public float oldMaxMoveVal;
	public float oldMinQuicknessVal;
	public float oldMaxQuicknessVal;
	
	public MarketFiltersViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.togglesCurrentStates=new bool[0];
		this.matchValues=new List<string>();
		this.isSkillChosen = false;
		this.isSkillToDisplay = false;
		this.cardTypeList=new string[0];
		this.sortButtonStyle=new GUIStyle[10];
		this.filterTitleStyle = new GUIStyle ();
		this.textFieldStyle = new GUIStyle ();
		this.toggleStyle = new GUIStyle ();
		this.smallPoliceStyle = new GUIStyle ();
		this.minmaxPriceStyle = new GUIStyle ();
		for (int i =0;i<10;i++)
		{
			this.sortButtonStyle[i]=new GUIStyle();
		}
		this.setFilters ();
	}
	public void setFilters()
	{
		this.filtersCardType=new List<int>();
		this.minPrice = "";
		this.maxPrice = "";
		this.valueSkill = "";
		this.minLifeVal = 0;
		this.maxLifeVal = 200;
		this.oldMinLifeVal = 0;
		this.oldMaxLifeVal = 200;
		this.minAttackVal = 0;
		this.maxAttackVal = 100;
		this.oldMinAttackVal = 0;
		this.oldMaxAttackVal = 100;
		this.minMoveVal = 0;
		this.maxMoveVal = 10;
		this.oldMinMoveVal = 0;
		this.oldMaxMoveVal = 10;
		this.oldMinQuicknessVal = 0;
		this.oldMaxQuicknessVal = 100;
		this.sortSelected = 10;
		this.oldSortSelected = 10;
	}
	public void initStyles()
	{
		this.filterTitleStyle=this.styles[0];
		this.textFieldStyle=this.styles[1];
		this.toggleStyle=this.styles[2];
		this.smallPoliceStyle=this.styles[3];
		this.minmaxPriceStyle=this.styles[4];
		this.skillListStyle = this.styles [5];
	}
	public void resize(int heightScreen)
	{
		this.filterTitleStyle.fontSize = heightScreen * 2 / 100;
		this.textFieldStyle.fontSize = heightScreen * 2 / 100;
		this.toggleStyle.fontSize = heightScreen * 15 / 1000;
		this.smallPoliceStyle.fontSize = heightScreen * 15 / 1000;
		this.minmaxPriceStyle.fontSize = heightScreen * 15 / 1000;
		this.skillListStyle.fontSize = heightScreen * 15 / 1000;
	}
}