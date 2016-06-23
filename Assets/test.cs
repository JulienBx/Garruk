using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

    private GameObject mountains;
    private GameObject clouds;
    private GameObject light;

    private float mountainsPosition;
    private float mountainsStartPosition;
    private float mountainsEndPosition;
    private float mountainsSpeed;

    private float cloudsPosition;
    private float cloudsStartPosition;
    private float cloudsEndPosition;
    private float cloudsSpeed;

    // Use this for initialization
	void Awake () {

    this.mountains=gameObject.transform.FindChild("mountains").gameObject;
    this.clouds=gameObject.transform.FindChild("clouds").gameObject;
    this.light=gameObject.transform.FindChild("light").gameObject;

    this.cloudsSpeed=0.12f;
    this.cloudsStartPosition=-19.15f;
    this.cloudsEndPosition=19.15f;
    this.cloudsPosition=this.cloudsStartPosition;
    this.clouds.transform.position=new Vector3(this.cloudsPosition,0f,0f);

    this.mountainsSpeed=0.075f;
    this.mountainsStartPosition=-19.15f;
    this.mountainsEndPosition=19.15f;
    this.mountainsPosition=this.mountainsStartPosition;
    this.mountains.transform.position=new Vector3(this.mountainsPosition,0f,0f);
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    this.mountainsPosition=this.mountainsPosition+Time.deltaTime*mountainsSpeed;
        if(this.mountainsPosition>=this.mountainsEndPosition)
        {
            this.mountainsPosition=this.mountainsStartPosition;
        }
        this.cloudsPosition=this.cloudsPosition+Time.deltaTime*cloudsSpeed;
        if(this.cloudsPosition>=this.cloudsEndPosition)
        {
            this.cloudsPosition=this.cloudsStartPosition;
        }
        Vector3 mountainsPosition = this.mountains.transform.position;
        mountainsPosition.x=this.mountainsPosition;
        this.mountains.transform.position=mountainsPosition;

        Vector3 cloudsPosition = this.clouds.transform.position;
        cloudsPosition.x=this.cloudsPosition;
        this.clouds.transform.position=cloudsPosition;
       
	}
}
