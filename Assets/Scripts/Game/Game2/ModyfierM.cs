using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ModifyerM
{
	int amount;
	int idIcon;
	string description;
	string title;
	int duration;

	public ModifyerM(int Amount, int IdIcon, string Description, string Title, int d)
	{
		this.amount = Amount;
		this.idIcon = IdIcon;
		this.description = Description;
		this.title = Title;
		this.duration = d;
	}

	public int getAmount(){
		return this.amount;
	}

	public void addAmount(int i){
		this.amount+=i;
	}

	public int getIdIcon(){
		return this.idIcon;
	}

	public string getDescription(){
		return this.description;
	}

	public string getTitle(){
		return this.title;
	}

	public void setTitle(string s){
		this.title = s;
	}

	public int getDuration(){
		return this.duration;
	}
}
