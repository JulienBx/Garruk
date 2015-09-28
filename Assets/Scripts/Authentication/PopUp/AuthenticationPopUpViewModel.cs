using UnityEngine;

public class AuthenticationPopUpViewModel
{
	public int guiDepth;
	public Rect centralWindow;
	public GUIStyle[] styles;
	public GUIStyle centralWindowStyle;
	public GUIStyle centralWindowTitleStyle;
	public GUIStyle centralWindowButtonStyle;
	public GUIStyle centralWindowTextfieldStyle;
	public GUIStyle centralWindowPasswordFieldStyle;
	public GUIStyle centralWindowErrorStyle;
	public GUIStyle instructionsStyle;
	
	
	public AuthenticationPopUpViewModel ()
	{
		this.guiDepth = -1;
		this.centralWindow = new Rect ();
		this.styles=new GUIStyle[0];
		this.centralWindowStyle = new GUIStyle ();
		this.centralWindowTitleStyle = new GUIStyle ();
		this.centralWindowButtonStyle = new GUIStyle ();
		this.centralWindowTextfieldStyle = new GUIStyle ();
		this.centralWindowErrorStyle = new GUIStyle ();
		this.instructionsStyle = new GUIStyle ();
		this.centralWindowPasswordFieldStyle = new GUIStyle ();
	}
	public void initStyles()
	{
		this.centralWindowStyle = this.styles [0];
		this.centralWindowTitleStyle = this.styles [1];
		this.centralWindowButtonStyle = this.styles [2];
		this.centralWindowTextfieldStyle = this.styles [4];
		this.centralWindowErrorStyle = this.styles [5];
		this.instructionsStyle = this.styles [6];
		this.centralWindowPasswordFieldStyle = this.styles [3];
	}
	public void resize()
	{
		this.centralWindowTitleStyle.fontSize = Screen.height * 2 / 100;
		this.centralWindowTextfieldStyle.fontSize= Screen.height * 2 / 100;
		this.centralWindowButtonStyle.fontSize = Screen.height * 2 / 100;
		this.centralWindowErrorStyle.fontSize = Screen.height * 2 / 100;
		this.instructionsStyle.fontSize = Screen.height * 18 / 1000;
		this.centralWindowPasswordFieldStyle.fontSize= Screen.height * 2 / 100;
	}
}


