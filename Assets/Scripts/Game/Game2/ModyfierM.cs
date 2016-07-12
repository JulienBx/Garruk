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

	public ModifyerM(int Amount, int IdIcon, string Description, string Title)
	{
		this.amount = Amount;
		this.idIcon = IdIcon;
		this.description = Description;
		this.title = Title;
	}

	public int getAmount(){
		return this.amount;
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
}
