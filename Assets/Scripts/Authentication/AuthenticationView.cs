using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AuthenticationView : MonoBehaviour
{
	public AuthenticationScreenViewModel authenticationScreenVM;


	public AuthenticationView()
	{
		this.authenticationScreenVM= new AuthenticationScreenViewModel();
	}
	void Update()
	{
		if (Screen.width != authenticationScreenVM.widthScreen || Screen.height != authenticationScreenVM.heightScreen) 
		{
			AuthenticationController.instance.resize();
		}
	}
}
