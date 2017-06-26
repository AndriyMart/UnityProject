using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRabit : MonoBehaviour {

	Transform heroParent = null;

	public static HeroRabit lastRabit = null;

	public bool isGrounded = false;
	bool JumpActive = false;
	float JumpTime = 0f;
	public float MaxJumpTime = 2f;
	public float JumpSpeed = 2f;
	public float speed = 1;

	bool BigScale;
	Animator animator;

	Rigidbody2D myBody = null;

	void Awake() {
		lastRabit = this;
	}

	// Use this for initialization
	void Start () {
		myBody = this.GetComponent<Rigidbody2D> ();
		//class HeroRabit, void Start()
		//Зберігаємо позицію кролика на початку
		heroParent = this.transform.parent;	
		LevelController.current.setStartPosition (transform.position);
		animator = GetComponent<Animator> ();
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

	public void OnTriggerEnter2D(Collider2D collider){
		if(rabitDie == false){
		Orc1 orc = collider.gameObject.GetComponent<Orc1>();
		Orc2 orc2 = collider.gameObject.GetComponent<Orc2>();
			if (orc != null) {
				if (collider == orc.body) {
					Debug.Log ("Body");
					orc.onAttack ();
					this.Die ();
				} else if (collider == orc.head) {
					Debug.Log ("Head");
					orc.onHealthChange ();
				}

			}
		}
		//if (rabit != null) {
		//	if(collider.)
		//	}
	}

	void FixedUpdate () {
		//[-1,1]
		float value = Input.GetAxis ("Horizontal");

		if (Mathf.Abs (value) > 0){
			Vector2 vel = myBody.velocity;
			vel.x = value * speed;
			myBody.velocity = vel;
		}

		SpriteRenderer sr = GetComponent<SpriteRenderer>();

		if(value < 0){
			sr.flipX = true;
		} else if(value > 0){
			sr.flipX = false;
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

		if(Input.GetButtonDown("Jump") && isGrounded) {
			this.JumpActive = true;
		}
		if(this.JumpActive) {
			//Якщо кнопку ще тримають
			if(Input.GetButton("Jump")) {
				this.JumpTime += Time.deltaTime;
				if (this.JumpTime < this.MaxJumpTime) {
					Vector2 vel = myBody.velocity;
					vel.y = JumpSpeed * (1.0f - JumpTime / MaxJumpTime);
					myBody.velocity = vel;
				}
			} else {
				this.JumpActive = false;
				this.JumpTime = 0;
			}
		}
	}
	bool rabitDie = false;
	float timeToDie;
	public void Die(){
		if (BigScale!=true){
		if (this.isGrounded) {
			timeToDie = 2f;
			animator.SetBool ("die", true);
			rabitDie = true;
		} else {
			//LevelController.current.onRabitDeath(GetComponent<HeroRabit>());
		}
		}else
			this.transform.localScale = new Vector3(1,1,0);
	}

	public void Bigscale(){
		this.transform.localScale = new Vector3(2,2,0);
		BigScale = true;
	}			
	// Update is called once per frame
	void Update () {
		if (rabitDie) {
			timeToDie -= Time.deltaTime;
			if (timeToDie <= 0) {
				animator.SetBool ("die", false);
				rabitDie = false;
				LevelController.current.onRabitDeath(GetComponent<HeroRabit>());
			}
		}
	}
}




