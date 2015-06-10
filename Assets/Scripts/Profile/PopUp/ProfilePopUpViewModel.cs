using UnityEngine;

public class ProfilePopUpViewModel
{
	public int guiDepth;
	public Rect centralWindow;
	public bool guiEnabled;
	public GUIStyle[] styles;
	public GUIStyle centralWindowStyle;
	public GUIStyle centralWindowTitleStyle;
	public GUIStyle centralWindowButtonStyle;
	public GUIStyle centralWindowTextfieldStyle;
	public GUIStyle centralWindowErrorStyle;
	
	
	public ProfilePopUpViewModel ()
	{
		this.guiDepth = -1;
		this.guiEnabled = true;
		this.centralWindow = new Rect ();
		this.styles=new GUIStyle[0];
		this.centralWindowStyle = new GUIStyle ();
		this.centralWindowTitleStyle = new GUIStyle ();
		this.centralWindowButtonStyle = new GUIStyle ();
		this.centralWindowTextfieldStyle = new GUIStyle ();
		this.centralWindowErrorStyle = new GUIStyle ();
	}
	public void initStyles()
	{
		this.centralWindowStyle = this.styles [0];
		this.centralWindowTitleStyle = this.styles [1];
		this.centralWindowTextfieldStyle = this.styles [2];
		this.centralWindowButtonStyle = this.styles [3];
		this.centralWindowErrorStyle = this.styles [4];
	}
	public void resize()
	{
		this.centralWindowTitleStyle.fontSize = Screen.height * 2 / 100;
		this.centralWindowTextfieldStyle.fontSize= Screen.height * 2 / 100;
		this.centralWindowButtonStyle.fontSize = Screen.height * 2 / 100;
		this.centralWindowErrorStyle.fontSize = Screen.height * 2 / 100;
	}
}


