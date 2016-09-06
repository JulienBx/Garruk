using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable] 
public class File2
{
	public string Error="";
	public WWW LocalFile;
	public string FileExtension;
	public long FileSize;
	

	private int sizeMax = 3145728;
	private List<string> availableExtension = new List<string>(){".jpg", ".png"};


	public File2()
	{
	
	}
	
	public IEnumerator createProfilePicture(string path)
	{
		this.Error = "";
		var fileInfo = new System.IO.FileInfo(path);
		this.FileSize = fileInfo.Length;
		this.FileExtension = fileInfo.Extension;
		
		if (!this.availableExtension.Contains(this.FileExtension, StringComparer.OrdinalIgnoreCase))
		{
			this.Error ="Chargement annule, l'image doit etre au format .png ou .jpg";
			yield break;
		}
		if (this.FileSize > this.sizeMax) 
		{
			this.Error = "Chargement annule, l'image ne doit pas depasser " + (this.sizeMax / 1024) + "Mo";
			yield break;
		}
		this.LocalFile = new WWW("file:///" + path);
		yield return LocalFile;
		if (LocalFile.error == null)
		{
			//Debug.Log("Loaded file successfully");
		}
		else
		{
			//Debug.Log("Open file error: "+LocalFile.error);
			this.Error ="Le chargement de l'image a echoue, veuilez recommencer";
			yield break; // stop the coroutine here
		}
	}
}