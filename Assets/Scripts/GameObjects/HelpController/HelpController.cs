using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class HelpController : MonoBehaviour 
{
	public static HelpController instance;
	private HelpRessources ressources;

	private GameObject dialogBox;
	private GameObject dialogTitle;
	private GameObject nextButton;
	private GameObject nextButtonTitle;
	private GameObject companion;

	public int sequenceId;
	public string textDisplayed;


	void Update()
	{
		
	}
	public void initialize()
	{
		this.ressources = this.gameObject.GetComponent<HelpRessources> ();
		this.companion = this.gameObject.transform.FindChild ("Companion").gameObject;
		this.dialogBox = this.companion.transform.FindChild ("Dialog").gameObject;
		this.dialogTitle = this.dialogBox.transform.FindChild ("Title").gameObject;
		this.nextButton = this.companion.transform.FindChild ("NextButton").gameObject;
		this.nextButtonTitle = this.nextButton.transform.FindChild ("Title").gameObject;
	}
	public virtual void startHelp()
	{
		this.sequenceId = 0;
		this.showSequence ();
	}
	public virtual void nextButtonHandler()
	{
	}
	public virtual void showSequence()
	{
	}
	public void displayText()
	{
		if (textDisplayed.Length < 100) {
			this.dialogBox.GetComponent<SpriteRenderer> ().sprite = ressources.dialogs [0];
		} else if (textDisplayed .Length< 300) {
		} else {
		}

		//2.42 2.75 3.02
	}
}

