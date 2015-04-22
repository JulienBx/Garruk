using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MyFriendsViewModel {

	public IList<User> contacts;
	public List<int> contactsDisplayed;
	public int nbPages;
	public int pageDebut;
	public int pageFin;
	public int chosenPage;
	public int start;
	public int finish;
	public int elementPerRow;
	public GUIStyle[] paginatorGuiStyle;
	//public GUIStyle[] styles;
	public string labelNo;
	public string title="Mes amis";
	public Rect[] blocks;
	public float blocksWidth;
	public float blocksHeight;
	
	public MyFriendsViewModel (){
	}

	public void displayPage(){
		
		this.start = this.chosenPage*(this.elementPerRow*3);
		if (this.contacts.Count < (3*this.elementPerRow*(this.chosenPage+1)))
		{
			this.finish = this.contacts.Count;
		}
		else{
			this.finish = (this.chosenPage+1)*(3 * this.elementPerRow);
		}
	}
	public void resize(int heightScreen){
	}
}
