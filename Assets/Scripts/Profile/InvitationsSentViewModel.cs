using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class InvitationsSentViewModel {
	
	public IList<User> Contacts;
	public IList<int> profilePictures;
	public int nbPages;
	public int pageDebut;
	public int pageFin;
	public int chosenPage;
	public int start;
	public int finish;
	public int elementPerRow;
	public string labelNo;
	public GUIStyle[] paginatorGuiStyle;
	//public GUIStyle[] styles;
	public Rect[] blocks;
	public float blocksWidth;
	public float blocksHeight;

	public InvitationsSentViewModel ()
	{
	}
	public void initStyle()
	{
		
	}
	public void resize(int heightScreen)
	{
		
	}
	public void displayPage(){
		
		this.start = this.chosenPage*(this.elementPerRow*3);
		if (this.Contacts.Count < (3*this.elementPerRow*(this.chosenPage+1)))
		{
			this.finish = this.Contacts.Count;
		}
		else{
			this.finish = (this.chosenPage+1)*(3 * this.elementPerRow);
		}
	}
}
