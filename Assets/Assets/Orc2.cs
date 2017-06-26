using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc2 : MonoBehaviour {

	public enum Mode {
		GoToA,
		GoToB,
		Dead,
		Attack
	}

	Transform heroParent = null;

	public static Orc1 lastRabit = null;

	public float speed = 1;
	public bool dieAnim = false;

	public bool isGrounded = false;

	Vector3 pointA;
	Vector3 pointB;

	public BoxCollider2D head;
	public BoxCollider2D body;

	public float patrolDistance = 4;

	Mode mode = Mode.GoToB;

	public Animator animator;


	float timeToDie =2f;
	float attackTime =1;

	public static bool orcDie = false;

	float getDirection (){
		if (this.mode == Mode.Dead) {
			return 0;
		}
		if (this.mode == Mode.Attack) {
			return 0;
		}
		Vector3 my_pos = this.transform.position;
		if(this.mode == Mode.GoToB){
			if (my_pos.x >= pointB.x) {
				this.mode = Mode.GoToA;
			} 
		}
		if(this.mode == Mode.GoToA){
			if (my_pos.x <= pointA.x) {
				this.mode = Mode.GoToB;
			} 
		}
		if(this.mode == Mode.GoToB){
			if (my_pos.x <= pointB.x) {
				return 1;
			} else {
				return-1;
			}
		}else if(this.mode == Mode.GoToA){
			if (my_pos.x >= pointA.x) {
				return -1;
			} else {
				return 1;
			}
		}
		return 0;
	}

	Rigidbody2D myBody = null;



	// Use this for initialization
	void Start () {
		pointA = this.transform.position;
		pointB = pointA;

		if (patrolDistance < 0) {
			pointA.x += patrolDistance;
		} else {
			pointB.x += patrolDistance;
		}

		myBody = this.GetComponent<Rigidbody2D> ();
		//class HeroRabit, void Start()
		//Зберігаємо позицію кролика на початку
		heroParent = this.transform.parent;	
		LevelController.current.setStartPosition (transform.position);
	}

	static void SetNewParent(Transform obj, Transform new_parent) {
		if(obj.transform.parent != new_parent) {
			//Засікаємо позицію у Глобальних координатах
			Vector3 pos = obj.transform.position;
			//Встановлюємо нового батька
			obj.transform.parent = new_parent;
			//Після зміни батька координати кролика зміняться
			//Оскільки вони тепер відносно іншого об’єкта
			//повертаємо кролика в ті самі глобальні координати
			obj.transform.position = pos;
		}
	}

	void FixedUpdate () {
		//[-1,1]
		float value = this.getDirection();

		Animator animator = GetComponent<Animator> ();

		if (Mathf.Abs (value) > 0) {
			animator.SetBool ("walking", true);
			Vector2 vel = myBody.velocity;
			vel.x = value * speed;
			myBody.velocity = vel;
		} else {
			animator.SetBool ("walking", false);
		}


		SpriteRenderer sr = GetComponent<SpriteRenderer>();

		if(value < 0){
			sr.flipX = false;
		} else if(value > 0){
			sr.flipX = true;
		}


		//class HeroRabit, void FixedUpdate()
		Vector3 from = transform.position + Vector3.up * 0.3f;
		Vector3 to = transform.position + Vector3.down * 0.1f;
		int layer_id = 1 << LayerMask.NameToLayer ("Ground");
		//Перевіряємо чи проходить лінія через Collider з шаром Ground
		RaycastHit2D hit = Physics2D.Linecast(from, to, layer_id);
		if(hit) {
			isGrounded = true;
			if (hit.transform != null
				&& hit.transform.GetComponent<moveplat>() != null)
			{
				//Приліпаємо до платформи
				this.transform.parent = hit.transform;
			}
		} else {
			isGrounded = false;
			this.transform.parent = this.heroParent;
		}
		//Намалювати лінію (для розробника)
		Debug.DrawLine (from, to, Color.red);

		if (orcDie) {
			orcDie = true;
			animator.SetTrigger("die");
			timeToDie -= Time.deltaTime;
			if (timeToDie <= 0) {
				orcDie = false;
				LevelController.current.onRabitDeath(GetComponent<HeroRabit>());
			}
		}

	}

	IEnumerator Wait(){

		Debug.Log ("Timer");
		yield return new WaitForSeconds (4);

	}


	public void onAttack(){
		Animator animator = GetComponent<Animator>();
		Debug.Log ("Attack");
		animator.SetTrigger("Attack");
		this.mode = Mode.Attack;
		/*Vector3 my_pos = this.transform.position;
		StartCoroutine (Wait ());
		if (my_pos.x >= pointB.x) {
			this.mode = Mode.GoToA;
		} else {
			this.mode = Mode.GoToB;
		}
	*/
	}

	public void onHealthChange(){
		Animator animator = GetComponent<Animator>();
		Debug.Log ("DieAnim");
		animator.SetTrigger("die");
		StartCoroutine (orc1Die ());
	}
	IEnumerator orc1Die() {
		orcDie = true;
		Debug.Log ("orc1Die");
		this.mode = Mode.Dead;
		foreach (BoxCollider2D collider in this.GetComponents<BoxCollider2D> ()) {
			if (collider.isTrigger) {
				collider.enabled = false;
			}
		}
		yield return new WaitForSeconds (2);
		Destroy (this.gameObject);
		orcDie = false;
	}

	public void Die(){

	}


	// Update is called once per frame
	void Update () {

	}
}


