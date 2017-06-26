using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroParrent : MonoBehaviour {

	Transform heroParent = null;

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



	void Start () {
		//Зберегти стандартний батьківський GameObject
		this.heroParent = this.transform.parent;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 from = transform.position + Vector3.up * 0.3f;
		Vector3 to = transform.position + Vector3.down * 0.1f;
		int layer_id = 1 << LayerMask.NameToLayer ("Ground");
		//Згадуємо ground check
		RaycastHit2D hit = Physics2D.Linecast(from, to, layer_id);
		if(hit) {
			//Перевіряємо чи ми опинились на платформі
			if(hit.transform != null
				&& hit.transform.GetComponent<moveplat>() != null){
				//Приліпаємо до платформи
				this.transform.parent = hit.transform;
			}
		} else {
			//Ми в повітрі відліпаємо під платформи
			this.transform.parent = this.heroParent;
		}
	}
}
