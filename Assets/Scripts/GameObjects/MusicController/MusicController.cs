using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour 
{
	public static MusicController instance;
	private AudioSource audio;
	public AudioClip[] musics;
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
				audio.volume=ApplicationModel.volMusic*musicVolRatio;
			}
			else
			{
				this.isFading=false;
				this.musicVolRatio=1f;
				this.currentTrack=0;
				this.playCurrentMusic();
			}
		}
		if(this.isInitialized && !audio.isPlaying && this.shouldPlay)
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
			this.audio = GetComponent<AudioSource>();
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
		audio.clip=this.musics[tracks[currentTrack]];
		audio.volume=ApplicationModel.volMusic;
		audio.Play();
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
}

