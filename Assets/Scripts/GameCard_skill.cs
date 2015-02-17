using UnityEngine;
using System.Collections;

public class GameCard_skill : MonoBehaviour {

	bool isHovered = false;
	Vector3 screenPos;
	GameObject Parent;
	int sizeSkill;

	public int skillNumber;

	// Use this for initialization
	void Start () {
		Parent = transform.parent.gameObject;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseOver() {


		isHovered = true;
		screenPos = Camera.main.camera.WorldToScreenPoint(transform.position);

	}

	void OnMouseExit() {

		isHovered = false;
	}


	void OnGUI () {
		if (isHovered && transform.GetComponent<TextMesh> ().text != ""){
			sizeSkill = Parent.GetComponent<GameCard> ().Card.Skills [skillNumber].Description.Length;
			Rect windowRect = GUI.Window(0, new Rect(screenPos.x, Screen.height-screenPos.y+10, 250, 30 + (sizeSkill / 2)), DoMyWindow, "Description de " + Parent.GetComponent<GameCard>().Card.Skills[skillNumber].Name);
		}


	}

	void DoMyWindow(int windowID) {


		GUI.Label (new Rect(10,15,230, 15 + (sizeSkill / 2)),"" + Parent.GetComponent<GameCard>().Card.Skills[skillNumber].Description) ;

		
	}

}
