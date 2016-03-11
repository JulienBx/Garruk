
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using SimpleJSON;
using System;

namespace Xsolla 
{
	public abstract class BaseWWWRequest 
	{
		public Action<int, object[]> ObjectsRecived;
		private List<WWW> prepared;
		private int type;

		public BaseWWWRequest(int newType)
		{
			this.type = newType;
			prepared = new List<WWW> ();
		}

		public virtual BaseWWWRequest Prepare(bool isSandbox, Dictionary<string, object> requestParams)
		{
			WWWForm form = new WWWForm();
			foreach(KeyValuePair<string,object> post_arg in requestParams)
			{
				string argValue = post_arg.Value != null ? post_arg.Value.ToString() : "";
				form.AddField(post_arg.Key, argValue);
			}
			WWW www = new WWW(GetDomain(isSandbox) + GetMethod (), form);
			prepared.Add (www);
			return this;
		}

		public virtual BaseWWWRequest Prepare(bool isSandbox, string method, Dictionary<string, object> requestParams)
		{
			WWWForm form = new WWWForm();
			foreach(KeyValuePair<string,object> post_arg in requestParams)
			{
				string argValue = post_arg.Value != null ? post_arg.Value.ToString() : "";
				form.AddField(post_arg.Key, argValue);
			}
			WWW www = new WWW(GetDomain(isSandbox) + method, form);
			prepared.Add (www);
			return this;
		}

		public virtual IEnumerator Execute()
		{
			foreach(WWW www in prepared)
			{
				yield return www;
				if(www.error == null){
					JSONNode rootNode = JSON.Parse(www.text);
					if(!rootNode.AsObject.ContainsKey("error"))
						ObjectsRecived(type, ParseResult(rootNode));
					else
						Logger.Log("ErRoR");
				} else {
					Logger.Log("ErRoR");
				}
			}
		}

		protected abstract object[] ParseResult (JSONNode text);

		protected abstract string GetMethod ();

		private void OnObjectsRecieved(int type, object[] os)
		{
			if(ObjectsRecived != null)
				ObjectsRecived(type, os);
		}

		private string GetDomain(bool isSandbox)
		{
			if (!isSandbox) { 
				return "https://secure.xsolla.com";
			} else {
				return "https://sandbox-secure.xsolla.com";
			}
		}

	}
}