using UnityEngine;
using System.Collections;

public class CharacterControllerBackCamera : MonoBehaviour {

	[SerializeField]
	private Animator animator;
	[SerializeField]
	private float DirectionDamptime = 0.25f;  //運動がどのぐらいスムーズに行くか


	private float speed = 0.0f;
	private float horizontal = 0.0f;
	private float vertical = 0.0f;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();

		if(animator.layerCount >= 2) {
			animator.SetLayerWeight(1, 1);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(animator) {
			//入力の認識
			horizontal = Input.GetAxis("Horizontal");
			vertical = Input.GetAxis("Vertical");

			//速度は、入力の値による計算されている
			speed = new Vector2(horizontal, vertical).sqrMagnitude;

			//メカニムの値を設定する
			animator.SetFloat("Speed", speed);
			animator.SetFloat("Direction", horizontal, DirectionDamptime, Time.deltaTime);
		}
	}
}
