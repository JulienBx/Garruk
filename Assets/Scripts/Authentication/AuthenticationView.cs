using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AuthenticationView : MonoBehaviour
{
	private GUIStyle centralWindowStyle;
	private GUIStyle centralWindowTitleStyle;
	private GUIStyle centralWindowButtonStyle;
	private GUIStyle centralWindowTextFieldStyle;
	private GUIStyle centralWindowPasswordFieldStyle;
	private GUIStyle centralWindowToggleStyle;

	private Rect centralWindow;
	private int heightScreen;
	private int widthScreen;
	private bool isinitialized = false;

	public bool toDisplay = false;

	public AuthenticationViewModel authenticationViewModel;

	void Start ()
	{

	}

	void Update()
	{
		if (Screen.width != widthScreen || Screen.height != heightScreen) {
			setStyles();
		}
	}

	public void initStyles(GUIStyle centralWindowStyle, GUIStyle centralWindowTitleStyle, GUIStyle centralWindowButtonStyle, GUIStyle centralWindowTextFieldStyle, GUIStyle centralWindowPasswordFieldStyle, GUIStyle centralWindowToggleStyle) {
		this.centralWindowStyle = centralWindowStyle;
		this.centralWindowTitleStyle = centralWindowTitleStyle;
		this.centralWindowButtonStyle = centralWindowButtonStyle;
		this.centralWindowTextFieldStyle = centralWindowTextFieldStyle;
		this.centralWindowPasswordFieldStyle = centralWindowPasswordFieldStyle;
		this.centralWindowToggleStyle = centralWindowToggleStyle;
		setStyles ();
	}

	public void OnGUI() 
	{
		if (toDisplay)
		{
			Event e = Event.current;
			if (e.keyCode == KeyCode.Return)
			{
				StartCoroutine(AuthenticationController.instance.Login());
			}
			GUILayout.BeginArea(centralWindow,centralWindowStyle);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label ("Identifiant",centralWindowTitleStyle);
				GUI.SetNextControlName("formNick");
				authenticationViewModel.userName = GUILayout.TextField(authenticationViewModel.userName,centralWindowTextFieldStyle);
				GUILayout.FlexibleSpace();
				GUILayout.Label ("Mot de passe",centralWindowTitleStyle);
				GUI.SetNextControlName("formPassword");
				authenticationViewModel.password = GUILayout.PasswordField(authenticationViewModel.password,"*"[0],centralWindowPasswordFieldStyle);
				GUILayout.FlexibleSpace();
				authenticationViewModel.isMemorizingLogin = GUILayout.Toggle(authenticationViewModel.isMemorizingLogin, "Mémoriser ma session",centralWindowToggleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					GUI.SetNextControlName("Confirmer");
					if (GUILayout.Button("Confirmer",centralWindowButtonStyle)){
						StartCoroutine(AuthenticationController.instance.Login());
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				if(authenticationViewModel.connexionError != ""){
					GUILayout.FlexibleSpace();
					GUILayout.Label (authenticationViewModel.connexionError, centralWindowTitleStyle);
				}
				GUILayout.FlexibleSpace();
				if (!isinitialized){
					GUI.FocusControl("formNick");
					isinitialized=true;
				}
			}
			GUILayout.EndArea();
		}
	}
	private void setStyles() {

		heightScreen = Screen.height;
		widthScreen = Screen.width;

		this.centralWindow = new Rect (widthScreen * 0.25f, 0.12f * heightScreen, widthScreen * 0.50f, 0.30f * heightScreen);

		this.centralWindowTitleStyle.fontSize = heightScreen*2/100;
		this.centralWindowTitleStyle.fixedHeight = (int)heightScreen*3/100;
		
		this.centralWindowButtonStyle.fontSize = heightScreen*2/100;
		this.centralWindowButtonStyle.fixedHeight = (int)heightScreen*3/100;
		this.centralWindowButtonStyle.fixedWidth = (int)widthScreen*20/100;

		this.centralWindowTextFieldStyle.fontSize = heightScreen*2/100;
		this.centralWindowTextFieldStyle.fixedHeight = (int)heightScreen*3/100;

		this.centralWindowPasswordFieldStyle.fontSize = heightScreen*2/100;
		this.centralWindowPasswordFieldStyle.fixedHeight = (int)heightScreen*3/100;

		this.centralWindowToggleStyle.fontSize = heightScreen*2/100;
		this.centralWindowToggleStyle.fixedHeight = (int)heightScreen*3/100;

	}
}
