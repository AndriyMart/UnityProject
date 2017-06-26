using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		//[-1, 1]
		float value = Input.GetAxis ("Horizontal");

		Animator animator = GetComponent<Animator> ();
		if (Mathf.Abs(value) > 0) {
			animator.SetBool ("walking", true);
		} else {
			animator.SetBool ("walking", false);
		}

		/*Animator animator1 = GetComponent<Animator> ();
		if (Mathf.Abs(value) > 0) {
			animator.SetBool ("run", true);
		} else {
			animator.SetBool ("run", false);
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
