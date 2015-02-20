using UnityEngine;
using System.Collections;

public class GameCard_skill : MonoBehaviour {

	bool isHovered = false;
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


	}

	void OnMouseExit() {

		isHovered = false;
	}


	void OnGUI () {
		if (isHovered && transform.Find("Skill" + skillNumber).GetComponent<TextMesh>().text != ""){

			Vector3 screenPos = new Vector3(transform.position.x-((renderer.bounds.max.x-renderer.bounds.min.x)/2),
			                        transform.position.y-((renderer.bounds.max.y-renderer.bounds.min.y)/2),
			                        transform.position.z);
			screenPos = Camera.main.camera.WorldToScreenPoint(screenPos);
			sizeSkill = Parent.GetComponent<GameCard> ().Card.Skills [skillNumber].Description.Length;
			Rect windowRect = GUI.Window(0, new Rect(screenPos.x, Screen.height-screenPos.y, 250, 30 + (sizeSkill / 2)), DoMyWindow, "Description de " + Parent.GetComponent<GameCard>().Card.Skills[skillNumber].Name);
		}


	}

	void DoMyWindow(int windowID) {


		GUI.Label (new Rect(10,15,230, 15 + (sizeSkill / 2)),"" + Parent.GetComponent<GameCard>().Card.Skills[skillNumber].Description) ;

		
	}

}
