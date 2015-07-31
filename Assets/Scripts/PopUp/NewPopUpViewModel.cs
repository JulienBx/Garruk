using UnityEngine;

public class NewPopUpViewModel
{
	public Rect centralWindow;
	public GUIStyle centralWindowStyle;
	public GUIStyle centralWindowTitleStyle;
	public GUIStyle centralWindowButtonStyle;
	public GUIStyle centralWindowTextfieldStyle;
	public GUIStyle centralWindowErrorStyle;
	public GUIStyle transparentStyle;
	public GUIStyle centralWindowSelGridStyle;
	public bool guiEnabled;
	
	public NewPopUpViewModel ()
	{
		this.centralWindow = new Rect ();
		this.centralWindowStyle = new GUIStyle ();
		this.centralWindowTitleStyle = new GUIStyle ();
		this.centralWindowButtonStyle = new GUIStyle ();
		this.centralWindowTextfieldStyle = new GUIStyle ();
		this.centralWindowErrorStyle = new GUIStyle ();
		this.transparentStyle = new GUIStyle ();
		this.centralWindowSelGridStyle = new GUIStyle ();
		this.guiEnabled = true;
	}
	public void resize()
	{
		this.centralWindowTitleStyle.fontSize = Screen.height * 2 / 100;
		this.centralWindowTextfieldStyle.fontSize= Screen.height * 2 / 100;
		this.centralWindowButtonStyle.fontSize = Screen.height * 2 / 100;
		this.centralWindowErrorStyle.fontSize = Screen.height * 2 / 100;
		this.centralWindowSelGridStyle.fontSize = Screen.height * 2 / 100;
	}
}


