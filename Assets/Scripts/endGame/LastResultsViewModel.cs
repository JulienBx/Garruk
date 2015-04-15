using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class LastResultsViewModel {

	public IList<PlayerResult> lastResults;
	public string lastResultsLabel;
	public Texture2D[] profilePictures;
	public int profilePicturesSize;
	public IList<GUIStyle> profilePictureButtonStyle;
	public int nbPages;
	public int pageDebut;
	public int pageFin;
	public int chosenPage;
	public int start;
	public int finish;
	public GUIStyle[] paginatorGuiStyle;


	public GUIStyle lastResultsLabelStyle;
	public GUIStyle winLabelResultsListStyle;
	public GUIStyle defeatLabelResultsListStyle;
	public GUIStyle opponentsInformationsStyle;
	public GUIStyle paginationStyle;
	public GUIStyle paginationActivatedStyle;
	public GUIStyle winBackgroundResultsListStyle;
	public GUIStyle defeatBackgroundResultsListStyle;

	
	public LastResultsViewModel (IList<PlayerResult> lastResults){
		this.lastResults = lastResults;
		this.chosenPage = 0;
		this.profilePictureButtonStyle=new List<GUIStyle>();
	
	}
	public LastResultsViewModel (){

	}
	public void displayPage(){
		this.start=this.chosenPage*5;
		if (this.lastResults.Count < (5*(this.chosenPage+1)))
		{
			this.finish=this.lastResults.Count;
		}
		else{
			this.finish=(this.chosenPage+1)*5;
		}
	}
	public void paginationBehaviour(int value, int page=0){
		switch (value)
		{
		case 0:
			this.pageDebut = this.pageDebut-5;
			this.pageFin = this.pageDebut+5;
			break;
		case 1:
			this.paginatorGuiStyle[this.chosenPage]=this.paginationStyle;
			this.chosenPage=page;
			this.paginatorGuiStyle[page]=this.paginationActivatedStyle;
			this.displayPage();
			break;
		case 2:
			this.pageDebut = this.pageDebut+5;
			this.pageFin = Mathf.Min(this.pageFin+5, this.nbPages);
			break;
		}
	}
}
