using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class ServerController : MonoBehaviour 
{
	public static ServerController instance;

	private bool toDetectTimeOut;
	private bool detectedTimeOut;
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
				this.detectedTimeOut=true;
				StopCoroutine(this.executeRequest());
				if(ApplicationModel.player.isGettingProduct)
				{
					this.productDeliveryFail();
				}
				else
				{
					this.lostConnection();
				}
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
		this.toDetectTimeOut=true;
		this.detectedTimeOut=false;
		this.timer=0f;
		WWW w =new WWW(this.URL, this.form);
		yield return w;
		if(!this.detectedTimeOut)
		{
			this.toDetectTimeOut=false;
			if(w.error!=null)
			{
				this.error=WordingServerError.getReference(w.error,false);
			}
			else if(w.text.Contains("#ERROR#"))
			{
				string[] errors = w.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
				this.error = WordingServerError.getReference(errors [1],true);
			}
			this.result=w.text;
			if(ApplicationModel.player.isGettingProduct)
			{
				if(this.error!="")
				{
					this.error="";
					this.productDeliveryFail();
				}
				else
				{
					ApplicationModel.player.isGettingProduct=false;
					ApplicationModel.player.productOwner="";
					ApplicationModel.player.productValue=0;
				}
			}
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
	public void lostConnection()
	{
		if(!ApplicationModel.player.ToDeconnect)
		{
			ApplicationModel.player.hastLostConnection=true;
		}
		ApplicationModel.player.ToDeconnect=true;
		SceneManager.LoadScene("Authentication");
	}
	public void productDeliveryFail()
	{
		PlayerPrefs.SetString("Product", ApplicationModel.Encrypt(ApplicationModel.player.productValue.ToString()));
		PlayerPrefs.SetString("ProductOwner", ApplicationModel.Encrypt(ApplicationModel.player.productOwner));
		PlayerPrefs.Save();
		ApplicationModel.player.productValue=0;
		ApplicationModel.player.productOwner="";
		ApplicationModel.player.isGettingProduct=false;
		this.lostConnection();
	}
}

