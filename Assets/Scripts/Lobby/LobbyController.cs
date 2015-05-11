using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class LobbyController : MonoBehaviour
{

	private LobbyModel model;
	private LobbyView view;

	public int totalNbResultLimit;
	public GameObject MenuObject;

	void Start()
	{
		this.model = new LobbyModel ();
		this.view = new LobbyView ();
		this.MenuObject = Instantiate(this.MenuObject) as GameObject;
		StartCoroutine (this.initialization());
	}
	private IEnumerator initialization ()
	{
		yield return StartCoroutine (model.getLobbyData (this.totalNbResultLimit));
	}
	
}
