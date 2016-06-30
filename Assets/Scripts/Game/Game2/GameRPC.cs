using UnityEngine;
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

	void Awake(){

	}

	public void initCompteurs(){
		compteurMine = 0 ;
		compteurHis = 0 ;
		targetMine = -1;
		targetHis = -1;
	}

	public void createBackground(){
		photonView.RPC("createBackgroundRPC", PhotonTargets.AllBuffered, GameView.instance.getIsFirstPlayer());
	}
	
	[PunRPC]
	void createBackgroundRPC(bool isFirstP)
	{
		this.updateRPCCompteurs(isFirstP);
		GameView.instance.createBackground();
	}

	public void updateRPCCompteurs(bool b){
		if(b==GameView.instance.getIsFirstPlayer()){
			compteurMine++;
			PlayerPrefs.SetInt("CompteurMine",compteurMine);
		}
		else{
			compteurHis++;
			PlayerPrefs.SetInt("CompteurHis",compteurHis);
		}
	}
}

