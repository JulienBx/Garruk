using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class FriendlyBoardViewModel {

	public GUIStyle mainLabelStyle;
	public GUIStyle subMainLabelStyle;
	public string mainLabelText;
	public string subMainLabelText;

	public FriendlyBoardViewModel (string mainlabeltext,string submainlabeltext){
		this.mainLabelText = mainlabeltext;
		this.subMainLabelText = submainlabeltext;
	}
}
