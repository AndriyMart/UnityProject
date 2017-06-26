using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc1animation : Orc1 {
	float getDirection (){
			return 1;
	}
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
	void FixedUpdate () {
		//[-1, 1]
		float value = this.getDirection();

		Animator animator = GetComponent<Animator> ();
		if (Mathf.Abs (value) > 0) {
			animator.SetBool ("walking", true);
		} else {
			animator.SetBool ("walking", false);
		}


}
}
