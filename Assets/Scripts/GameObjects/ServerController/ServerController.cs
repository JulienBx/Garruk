using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ServerController : MonoBehaviour 
{
	public static ServerController instance;

	private bool isAccessingServer;
	private float timer;
	private string URL;
	private WWWForm form;
	private bool isTimedOut;
	private string result;
	private string error;

	void Update()
	{
		if(isAccessingServer)
		{
			this.timer=this.timer+Time.deltaTime;
			if(this.timer>2f)
			{
				this.isTimedOut=true;
				this.isAccessingServer=false;
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
		this.isAccessingServer=true;
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
			else
			{
				this.result=w.text;
			}
		}
		else
		{
			this.error=WordingServerError.getReference("5",true);
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

}

