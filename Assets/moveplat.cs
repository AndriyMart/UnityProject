using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveplat : MonoBehaviour {

	//class moveplat
	public Vector3 MoveBy;
	public Vector3 pointA;
	public Vector3 pointB;
	public float Speed = 2f;
 	float time_to_wait;
	bool going_to_a;
	bool isArrived(Vector3 pos, Vector3 target) {
		pos.z = 0;
		target.z = 0;
		//target.x = 8;
		//target.y = -2;
		return Vector3.Distance(pos, target) < 0.02f;
	}

	// Use this for initialization
	void Start () {
		this.pointA = this.transform.position;
		this.pointB = this.pointA + MoveBy;
		this.going_to_a = false;
		this.time_to_wait = 0.05f;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		time_to_wait -= Time.deltaTime;
		if (time_to_wait <= 0) {
			//Do something
			Vector3 my_pos = this.transform.position;
			Vector3 target;
			if (going_to_a) {
				target = this.pointA;
			} else {
				target = this.pointB;
			}
			if (isArrived (my_pos, target)) {
				going_to_a = !going_to_a;
				time_to_wait = 0.05f;
			}
			else{
			Vector3 destination = target - my_pos;
			destination.z = 0;
			float move = this.Speed = Time.deltaTime;
			float distance = Vector3.Distance (destination, my_pos);
			Vector3 move_vector = destination.normalized * Mathf.Min (move, distance);
			this.transform.position += move_vector;
			}
		}
	}
}
