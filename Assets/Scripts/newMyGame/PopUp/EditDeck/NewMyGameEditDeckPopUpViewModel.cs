using System;
public class NewMyGameEditDeckPopUpViewModel
{
	
	public string oldName;
	public string newName;
	public string error;
	public bool guiEnabled;
	
	public NewMyGameEditDeckPopUpViewModel ()
	{
		this.oldName = "";
		this.newName = "";
		this.error = "";
		this.guiEnabled = true;
	}
}

