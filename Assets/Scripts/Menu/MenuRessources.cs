using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class MenuRessources : MonoBehaviour
{
	public GameObject loadingScreenObject;
	public int totalNbResultLimit;
	public int refreshInterval;
	public GameObject playPopUpObject;
	public GameObject transparentBackgroundObject;
	public GameObject invitationPopUpObject;
	public float startButtonPosition;
	public float endButtonPosition;
	public float speed;
	public GUISkin popUpSkin;
	public Sprite[] profilePictures;
	public Sprite[] largeProfilePictures;
	public Sprite[] packPictures;
	public Sprite[] tabsPictures;
	public Sprite[] competitionsPictures;
}

