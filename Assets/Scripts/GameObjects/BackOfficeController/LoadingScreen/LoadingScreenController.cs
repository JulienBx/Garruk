using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class LoadingScreenController : MonoBehaviour
{
	private float angle;
	private float speed;
	private Quaternion target;
	private bool toShowLoading;

	void Awake()
	{
		this.speed = 300.0f;
		this.angle = 0f;
		this.toShowLoading=true;
	}
	void Update()
	{
		if(this.toShowLoading)
		{
			this.angle = this.angle + this.speed * Time.deltaTime;
			this.target = Quaternion.Euler (0f,this.angle, 0f);
			this.gameObject.transform.FindChild("loadingCircle").transform.rotation = target;
		}
	}
	public void changeLoadingScreenLabel(string label)
	{
		this.gameObject.transform.FindChild ("title").GetComponent<TextMeshPro> ().text = label;
	}
	public void displayButton(bool value)
	{
		this.gameObject.transform.FindChild ("button").gameObject.SetActive (value);
	}

	public void resize()
	{
		this.gameObject.transform.position = new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -9f);
	}
}


