using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class CupBoardViewModel {

	public Cup cup;
	public IList<GUIStyle> roundsStyle=new List<GUIStyle>();
	public bool winCup;
	public bool stillInCup;
	public bool endCup;
	public string[] roundsName;

	public GUIStyle cupLabelStyle;
	public GUIStyle cupPrizeLabelStyle;
	
	public CupBoardViewModel (Cup cup, string[] roundsname){

		this.cup = cup;
		this.roundsName = roundsname;
	}
}
