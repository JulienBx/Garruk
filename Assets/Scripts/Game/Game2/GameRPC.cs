﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
	
public class GameRPC : Photon.MonoBehaviour
{	
	public static GameRPC instance;
	int compteurMine ;
	int compteurHis ;

	int targetMine;
	int targetHis;

	float failureTime = 20f;
	float timerFailure = 0f;

	bool failing;

	void Awake(){
		instance = this;
		this.failing = false;
	}

	public void initCompteurs(){
		compteurMine = 0 ;
		compteurHis = 0 ;
		targetMine = -1;
		targetHis = -1;
	}

	public void initTimer(){
		print("J'init le timer");
		this.timerFailure = 0f;
		this.failing = true;
		PhotonC.instance.displayLoadingScreen(WordingGame.getText(66));
	}

	public void stopTimer(){
		this.failing = false;
		PhotonC.instance.hideLoadingScreen();
	}

	public void addTime(float f){
		this.timerFailure+=f;
		if(this.timerFailure>this.failureTime){
			Game.instance.failToReconnect();
		}
		else if(this.timerFailure>3f){
			PhotonC.instance.reconnectToRoom();
		}
	}

	public bool isFailing(){
		return this.failing;
	}

	public IEnumerator launchRPC(string s){
		bool b = true ;
		while(b){
			try{
				photonView.RPC(s, PhotonTargets.AllBuffered, Game.instance.isFirstPlayer());
				b = false;
			}
			catch (Exception e){
				Debug.Log(e.ToString());
				if(!this.failing && !PhotonC.instance.isReconnecting()){
					this.initTimer();
				}
			}
			yield return new WaitForSeconds(0.5f);
		}
		this.stopTimer();
	}

	public IEnumerator launchRPC(string s, int x, int y, bool type){
		bool b = true ;
		while(b){
			try{
				photonView.RPC(s, PhotonTargets.AllBuffered, x, y, type, Game.instance.isFirstPlayer());
				b = false;
			}
			catch (Exception e){
				Debug.Log(e.ToString());
				if(!this.failing && !PhotonC.instance.isReconnecting()){
					this.initTimer();
				}
			}
			yield return new WaitForSeconds(0.5f);
		}
		this.stopTimer();
	}

	public IEnumerator launchRPC(string s, int x, int y, int z){
		bool b = true ;
		while(b){
			try{
				photonView.RPC(s, PhotonTargets.AllBuffered, x, y, z, Game.instance.isFirstPlayer());
				b = false;
			}
			catch (Exception e){
				Debug.Log(e.ToString());
				if(!this.failing && !PhotonC.instance.isReconnecting()){
					this.initTimer();
				}
			}
			yield return new WaitForSeconds(0.5f);
		}
		this.stopTimer();
	}

	[PunRPC]
	void moveOnRPC(int x, int y, int z, bool isFirstP)
	{
		this.updateRPCCompteurs(isFirstP);
		Game.instance.moveOn(x, y, z);
	}

	[PunRPC]
	void createTileRPC(int x, int y, bool type, bool isFirstP)
	{
		this.updateRPCCompteurs(isFirstP);
		Game.instance.createTile(x, y, type);
	}
	
	[PunRPC]
	void createBackgroundRPC(bool isFirstP)
	{
		this.updateRPCCompteurs(isFirstP);
		Game.instance.createBackground();
	}
	
	[PunRPC]
	void createCardsRPC(bool isFirstP)
	{
		this.updateRPCCompteurs(isFirstP);
		Game.instance.createCards();
	}

	public void resize(){
		photonView.RPC("resizeRPC", PhotonTargets.AllBuffered, Game.instance.isFirstPlayer());
	}
	
	[PunRPC]
	void resizeRPC(bool isFirstP)
	{
		this.updateRPCCompteurs(isFirstP);
		Game.instance.resize();
	}

	public void addPlayerToRoom(string s){
		photonView.RPC("addPlayerToRoomRPC", PhotonTargets.AllBuffered, Game.instance.isFirstPlayer(), s);
	}
	
	[PunRPC]
	void addPlayerToRoomRPC(bool isFirstP, string s)
	{
		this.updateRPCCompteurs(isFirstP);
		PhotonC.instance.addPlayer(s);
	}

	public void updateRPCCompteurs(bool b){
		if(b==Game.instance.isFirstPlayer()){
			compteurMine++;
			PlayerPrefs.SetInt("CompteurMine",compteurMine);
		}
		else{
			compteurHis++;
			PlayerPrefs.SetInt("CompteurHis",compteurHis);
		}
	}
}
