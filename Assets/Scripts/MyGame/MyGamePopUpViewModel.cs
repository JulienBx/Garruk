using UnityEngine;
using System.Collections;

public class MyGamePopUpViewModel 
{
	public GUIStyle[] styles;
	public GUIStyle centralWindowStyle;
	public GUIStyle centralWindowTitleStyle;
	public GUIStyle centralWindowTextFieldStyle;
	public GUIStyle centralWindowButtonStyle;
	public GUIStyle smallCentralWindowButtonStyle;

	public Rect centralWindow;
	public Rect centralFocus;
	public int renameCost;

	public MyGamePopUpViewModel()
	{
		renameCost                      = 200;
	}

	public void initStyles()
	{
		centralWindowStyle              = styles[0];
		centralWindowTitleStyle         = styles[1];
		centralWindowTextFieldStyle     = styles[2];
		centralWindowButtonStyle        = styles[3];
		smallCentralWindowButtonStyle   = styles[4];
	}
}
