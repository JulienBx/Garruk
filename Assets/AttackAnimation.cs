using UnityEngine;
using System.Collections;

public class AttackAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	private IEnumerator DestroyOnAnimationEnd() {
		yield return new WaitForSeconds(0.2f);
		Destroy(gameObject);
	}
	
	void Update () {
		transform.Rotate(Vector3.back * Time.deltaTime * 500);
		StartCoroutine(DestroyOnAnimationEnd());
	}
}
