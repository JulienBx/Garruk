using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine.SceneManagement;

public class SoundController : MonoBehaviour 
{
	public static SoundController instance;
	private AudioSource[] sounds;
	private AudioSource musicSource;
	private AudioSource sfxSource;
	public AudioClip[] musics;
	public AudioClip[] backOfficeSfx;
	private bool isInitialized;
	private bool isFading;
	private float musicVolRatio;
	private float speed;
	private int[] tracks;
	private int currentTrack;
	private bool shouldPlay;

	void Update()
	{
		if(isFading)
		{
			this.musicVolRatio=this.musicVolRatio-Time.deltaTime*speed;
			if(this.musicVolRatio>0f)
			{
				musicSource.volume=ApplicationModel.volMusic*musicVolRatio;
			}
			else
			{
				this.isFading=false;
				this.musicVolRatio=1f;
				this.currentTrack=0;
				this.playCurrentMusic();
			}
		}
		if(this.isInitialized && !musicSource.isPlaying && this.shouldPlay)
		{
			this.shouldPlay=false;
			if(this.currentTrack<this.tracks.Length-1)
			{
				this.currentTrack++;
			}
			else
			{
				this.currentTrack=0;
			}
			this.playCurrentMusic();
		}
	}
	void Awake()
	{
		if(this.isInitialized)
		{
			Destroy(this.gameObject);
		}
	}
	public void initialize()
	{
		if(!this.isInitialized)
		{
			instance = this;
			this.sounds = GetComponents<AudioSource>();
			this.musicSource=sounds[0];
			this.sfxSource=sounds[1];
			this.speed=2f;
			this.musicVolRatio=1f;
			DontDestroyOnLoad(this.gameObject);
			this.isInitialized=true;
			this.currentTrack=-1;
		}
	}
	public void playMusic(int[] tracks)
	{
		this.shouldPlay=false;
		this.tracks=shuffle(tracks);
		if(this.currentTrack!=-1)
		{
			this.isFading=true;
		}
		else
		{
			this.currentTrack=0;
			this.playCurrentMusic();
		}
	}
	void playCurrentMusic()
	{
		musicSource.clip=this.musics[tracks[currentTrack]];
		musicSource.volume=ApplicationModel.volMusic;
		musicSource.Play();
		this.shouldPlay=true;
	}
	int[] shuffle(int[] array)
	{
		for(int i=0;i<array.Length;i++)
		{
			int tmp=array[i];
			int r=UnityEngine.Random.Range(i,array.Length);
			array[i]=array[r];
			array[r]=tmp;
		}
		return array;
	}
	public void playSound(int i)
	{
		sfxSource.PlayOneShot(this.backOfficeSfx[i],ApplicationModel.volBackOfficeFx);
	}
	public void stopPlayingSound()
	{
		sfxSource.Stop();
	}


	//0-> ShowFocusedSkill
	//1-> DropCard
	//2-> PickCard
	//3-> Notification
	//4-> ShowFocusedCard
	//5-> DeleteCard
	//6-> LevelUp
	//7-> FlipCard
	//8-> PopUpButton
	//9-> Filter
	//10-> Menu
	//11-> PackOpening
	//12-> Login
	//13-> Error



}

