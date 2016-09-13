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
		StopCoroutine(this.executeRequest());
		ApplicationModel.player.IsOnline=false;
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
	public IEnumerator executeRequest()
	{
		this.result="";
		this.isServerError = false;
		this.error="";
		this.timer=0f;
		this.toDetectTimeOut=true;
		WWW w =new WWW(this.URL, this.form);
		yield return w;
		this.toDetectTimeOut=false;
		if(w.error!=null)
		{
			ApplicationModel.player.IsOnline=false;
			this.error = w.error;
			this.isServerError = true;
			Debug.Log(WordingServerError.getReference(w.error,false));
		}
		if(ApplicationModel.player.IsOnline)
		{
			if(w.text.Contains("#ERROR#"))
			{
				string[] errors = w.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
				this.error = WordingServerError.getReference(errors [1],true);
			}
			this.result=w.text;
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

