using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AuthenticationViewModel {

	public string userMacAdress;
	public bool isRemembered = false; 
	public string userName;
	public string password;
	public bool isMemorizingLogin = false;
	public bool isLoginSuccessFull;
	public string connexionError;
	
	public AuthenticationViewModel (){
		this.userMacAdress = SystemInfo.deviceUniqueIdentifier;
		this.password = "";
	}
}
