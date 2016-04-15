using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class SoundSettingsPopUpController : MonoBehaviour 
{

	private float sfxVol;
	private float musicVol;

	public void reset(float sfxVol, float musicVol)
	{
		this.sfxVol=sfxVol;
		this.musicVol=musicVol;
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingSoundSettingsPopUp.getReference(0);
		gameObject.transform.FindChild ("Title2").GetComponent<TextMeshPro> ().text = WordingSoundSettingsPopUp.getReference(1);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text =WordingSoundSettingsPopUp.getReference(2);
		gameObject.transform.FindChild ("CloseButton").GetComponent<SoundSettingsPopUpCloseButtonController> ().reset ();
		gameObject.transform.FindChild ("Button").GetComponent<SoundSettingsPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("sfxDown").GetComponent<SoundSettingsPopUpSfxDownButtonController> ().reset ();
		gameObject.transform.FindChild ("sfxUp").GetComponent<SoundSettingsPopUpSfxUpButtonController> ().reset ();
		gameObject.transform.FindChild ("musicDown").GetComponent<SoundSettingsPopUpMusicDownButtonController> ().reset ();
		gameObject.transform.FindChild ("musicUp").GetComponent<SoundSettingsPopUpMusicUpButtonController> ().reset ();
		this.computeMusic();
		this.computeSfx();
	}
	public void sfxUpHandler()
	{
		this.sfxVol=this.sfxVol+0.1f;
		ApplicationModel.volBackOfficeFx=this.sfxVol*ApplicationModel.volMaxBackOfficeFx;
		SoundController.instance.playSound(8);
		this.computeSfx();
	}
	public void sfxDownHandler()
	{
		this.sfxVol=this.sfxVol-0.1f;
		ApplicationModel.volBackOfficeFx=this.sfxVol*ApplicationModel.volMaxBackOfficeFx;
		SoundController.instance.playSound(8);
		this.computeSfx();
	}
	public void musicUpHandler()
	{
		this.musicVol=this.musicVol+0.1f;
		ApplicationModel.volMusic=this.musicVol*ApplicationModel.volMaxMusic;
		SoundController.instance.playSound(8);
		this.computeMusic();
	}
	public void musicDownHandler()
	{
		this.musicVol=this.musicVol-0.1f;
		ApplicationModel.volMusic=this.musicVol*ApplicationModel.volMaxMusic;
		SoundController.instance.playSound(8);
		this.computeMusic();
	}
	private void computeSfx()
	{
		gameObject.transform.FindChild ("sfxVol").GetComponent<TextMeshPro> ().text = ((int)(this.sfxVol*100f)).ToString()+" %";
		if(this.sfxVol==0)
		{
			gameObject.transform.FindChild ("sfxDown").gameObject.SetActive(false);
		}
		else if(this.sfxVol==1)
		{
			gameObject.transform.FindChild ("sfxUp").gameObject.SetActive(false);
		}
		else
		{	
			gameObject.transform.FindChild ("sfxDown").gameObject.SetActive(true);
			gameObject.transform.FindChild ("sfxUp").gameObject.SetActive(true);
		}
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingSoundSettingsPopUp.getReference(0);
	}
	private void computeMusic()
	{
		gameObject.transform.FindChild ("musicVol").GetComponent<TextMeshPro> ().text = ((int)(this.musicVol*100f)).ToString()+" %";
		if(this.musicVol==0)
		{
			gameObject.transform.FindChild ("muiscDown").gameObject.SetActive(false);
		}
		else if(this.musicVol==1)
		{
			gameObject.transform.FindChild ("musicUp").gameObject.SetActive(false);
		}
		else
		{	
			gameObject.transform.FindChild ("musicDown").gameObject.SetActive(true);
			gameObject.transform.FindChild ("musicUp").gameObject.SetActive(true);
		}
	}
	public float getSfxVol()
	{
		return this.sfxVol;
	}
	public float getMusicVol()
	{
		return this.musicVol;
	}
	public void exitPopUp()
	{
		SoundController.instance.playSound(8);
		NewProfileController.instance.exitSoundSettingsHandler();
	}
	public void confirmButtonHandler()
	{
		SoundController.instance.playSound(8);
		NewProfileController.instance.confirmSoundSettingsHandler();
	}
}

