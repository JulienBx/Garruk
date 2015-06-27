using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RightStatsController : MonoBehaviour
{
	public GameObject name;

	public RightStatsController()
	{

	}

	public void setCharacterName(string name)
	{
		string text = this.name.transform.GetComponent<Text>().text = name;
	}
}

