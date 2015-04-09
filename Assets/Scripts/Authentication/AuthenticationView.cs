using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AuthenticationView : MonoBehaviour {

	public GUIStyle centralWindowStyle;
	public GUIStyle centralWindowTitleStyle;
	public GUIStyle centralWindowButtonStyle;
	public GUIStyle centralWindowTextFieldStyle;
	public GUIStyle centralWindowPasswordFieldStyle;
	public GUIStyle centralWindowToggleStyle;

	private Rect centralWindow;
	private int heightScreen;
	private int widthScreen;
	private string formNick="";
	private string formPassword="";
	private bool memorizeLogins;
	private bool isinitialized=false;
	private bool toDisplay ;

	private string error="";

	AuthenticationController controller ;
	
	void Start (){
		controller = GetComponent<AuthenticationController>();
		setStyles ();
	}
	void Update(){
		if (Screen.width != widthScreen || Screen.height != heightScreen) {
			setStyles();
		}
	}
	void OnGUI () 
	{
		if (toDisplay){
			Event e = Event.current;
			if (e.keyCode == KeyCode.Return)
			{
				StartCoroutine(controller.Login(formNick, formPassword,memorizeLogins));
			}
			GUILayout.BeginArea(centralWindow,centralWindowStyle);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label ("Identifiant",centralWindowTitleStyle);
				GUI.SetNextControlName("formNick");
				formNick = GUILayout.TextField(formNick,centralWindowTextFieldStyle);
				GUILayout.FlexibleSpace();
				GUILayout.Label ("Mot de passe",centralWindowTitleStyle);
				GUI.SetNextControlName("formPassword");
				formPassword = GUILayout.PasswordField(formPassword,"*"[0],centralWindowPasswordFieldStyle);
				GUILayout.FlexibleSpace();
				memorizeLogins = GUILayout.Toggle(memorizeLogins, "Mémoriser ma session",centralWindowToggleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					GUI.SetNextControlName("Confirmer");
					if (GUILayout.Button("Confirmer",centralWindowButtonStyle)){
						StartCoroutine(controller.Login(formNick, formPassword,memorizeLogins));
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				if(error!=""){
					GUILayout.FlexibleSpace();
					GUILayout.Label (error,centralWindowTitleStyle);
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
	public void toDisplayWindow(){
		this.toDisplay = true ;
	}
	public void setError(string text){
		this.error=text ;
	}
	public void toMemorizeLogins(){
		this.memorizeLogins=true ;
	}
	public void setNick(string nick){
		this.formNick = nick;
	}
}
