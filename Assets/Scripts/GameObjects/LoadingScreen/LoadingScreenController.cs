using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class LoadingScreenController : MonoBehaviour
{
	
	private float angle;
	private float speed;
	private Quaternion target;
	
	void Awake()
	{
		this.gameObject.transform.FindChild ("title").GetComponent<TextMeshPro> ().text = "Chargement ...";
		this.gameObject.transform.position = new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -9f);
		this.speed = 300.0f;
		this.angle = 0f;
	}
	void Update()
	{
		this.angle = this.angle + this.speed * Time.deltaTime;
		this.target = Quaternion.Euler (0f, 0f, this.angle);
		this.gameObject.transform.FindChild("loadingCircle").transform.rotation = target;
	}
	public void changeLoadingScreenLabel(string label)
	{
		this.gameObject.transform.FindChild ("title").GetComponent<TextMeshPro> ().text = label;
	}
	public void displayButton(bool value)
	{
		this.gameObject.transform.FindChild ("button").gameObject.SetActive (value);
	}
}


