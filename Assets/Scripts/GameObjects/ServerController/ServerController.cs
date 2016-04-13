using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class ServerController : MonoBehaviour 
{
	public static ServerController instance;

	private bool toDetectTimeOut;
	private float timer;
	private string URL;
	private WWWForm form;
	private bool isTimedOut;
	private string result;
	private string error;


	void Update()
	{
		if(this.toDetectTimeOut)
		{
			this.timer=this.timer+Time.deltaTime;
			if(this.timer>ApplicationModel.timeOutDelay)
			{
				this.isTimedOut=true;
				this.toDetectTimeOut=false;
				StopCoroutine(this.executeRequest());
			}
		}
	}
	public void initialize()
	{
		instance=this;
	}
	public void setRequest(string URL, WWWForm form)
	{
		this.URL=URL;
		this.form=form;
	}
	public IEnumerator executeRequest()
	{
		this.result="";
		this.error="";
		this.isTimedOut=false;
		this.toDetectTimeOut=toDetectTimeOut;
		this.timer=0f;
		WWW w =new WWW(this.URL, this.form);
		yield return w;
		if(!this.isTimedOut)
		{
			if(w.error!=null)
			{
				this.error=WordingServerError.getReference(w.error,false);
			}
			else if(w.text.Contains("#ERROR#"))
			{
				string[] errors = w.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
				this.error = WordingServerError.getReference(errors [1],true);
			}
		}
		else
		{
			this.error=WordingServerError.getReference("5",true);
		}
		this.result=w.text;
	}
	public string getResult()
	{
		return this.result;
	}
	public string getError()
	{
		return this.error;
	}
	public void lostConnection()
	{
		if(!ApplicationModel.player.ToDeconnect)
		{
			ApplicationModel.player.hastLostConnection=true;
		}
		ApplicationModel.player.ToDeconnect=true;
		SceneManager.LoadScene("Authentication");
	}

}

