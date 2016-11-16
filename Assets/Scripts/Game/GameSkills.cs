using UnityEngine;

public class GameSkills : MonoBehaviour
{	
	public static GameSkills instance;
	void Awake()
	{
		instance = this;
	}

}

