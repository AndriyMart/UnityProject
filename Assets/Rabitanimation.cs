using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabitanimation : HeroRabit {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void FixedUpdate () {
		//[-1, 1]
		float value = Input.GetAxis ("Horizontal");

		Animator animator = GetComponent<Animator> ();
		if (Mathf.Abs (value) > 0) {
			animator.SetBool ("walking", true);
		} else {
			animator.SetBool ("walking", false);
		}

		//Animator animator = GetComponent<Animator> ();
		if (this.isGrounded) {
			animator.SetBool ("jump", true);
		} else {
			animator.SetBool ("jump", false);
		}

	}
}