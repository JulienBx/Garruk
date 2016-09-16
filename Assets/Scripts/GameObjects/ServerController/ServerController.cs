using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class ServerController : MonoBehaviour 
{
	public static ServerController instance;

	private bool toDetectTimeOut;
	private bool isServerError;
	private float timer;
	private string URL;
	private WWWForm form;
	private string result;
	private string error;
	private string text;
	private bool stopCoroutine;
	private bool requestIsOver;

	void Update()
	{
		if(this.toDetectTimeOut)
		{
			this.timer=this.timer+Time.deltaTime;
			if(this.timer>ApplicationModel.timeOutDelay)
			{
				this.toDetectTimeOut=false;
                BackOfficeController.instance.displayOfflineModeButton(true);
			}
		}
	}
	public void switchOffline()
	{
		ApplicationModel.player.IsOnline=false;
		this.stopCoroutine = true;
	}
	public void initialize()
	{
		instance=this;
		DontDestroyOnLoad(this.gameObject);
	}
	public void setRequest(string URL, WWWForm form)
	{
		this.URL=URL;
		this.form=form;
	}
	public IEnumerator mainRequest()
	{
		this.text = "";
		this.error = "";
		WWW w =new WWW(this.URL, this.form);
		yield return w;
		this.requestIsOver = true;
		if (w.error != null) {
			this.error = w.error;
			Debug.Log ("servor Error");
		}
		else if (w.text != null) {
			this.text = w.text;
			Debug.Log ("it's fine");
		}
	}
	public IEnumerator executeRequest()
	{
		this.result="";
		this.isServerError = false;
		this.error="";
		this.timer=0f;
		this.toDetectTimeOut=true;
		this.stopCoroutine = false;
		this.requestIsOver = false;
		StartCoroutine ("mainRequest");
		bool continueLoop=true;
		while (continueLoop) {
			if (this.requestIsOver) {
				continueLoop = false;
			}
			if (this.stopCoroutine) {
				continueLoop = false;
				StopCoroutine ("mainRequest");
			}
		}
		yield return 0;
		this.toDetectTimeOut=false;
		
		if(this.error!=null || this.stopCoroutine)
		{
			ApplicationModel.player.IsOnline=false;
			if (this.error!=null) {
				this.isServerError = true;
				Debug.Log(WordingServerError.getReference(this.error,false));
			}
		}
		if(ApplicationModel.player.IsOnline)
		{
			if(this.text.Contains("#ERROR#"))
			{
				string[] errors = this.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
				this.error = WordingServerError.getReference(errors [1],true);
			}
			this.result=this.text;
		}
	}
	public string getResult()
	{
		return this.result;
	}
	public string getError()
	{
		return this.error;
	}
	public bool getIsServerError()
	{
		return this.isServerError;
	}
}

