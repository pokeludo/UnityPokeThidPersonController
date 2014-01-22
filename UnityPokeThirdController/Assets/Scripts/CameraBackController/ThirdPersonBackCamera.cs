using UnityEngine;
using System.Collections;

public class ThirdPersonBackCamera : MonoBehaviour {

	[SerializeField]
	private float distanceAway;
	[SerializeField]
	private float distanceUp;
	[SerializeField]
	private float smooth;
	[SerializeField]
	private Transform follow;

	private Vector3 targetPosition;

	// Use this for initialization
	void Start () {
		follow = GameObject.FindWithTag("Player").transform;  //キャラクターの(0,0,0)点
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate () {
		targetPosition = follow.position + follow.up * distanceUp - follow.forward * distanceAway; //カメラの位置を計算
		DebugVector();

		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth); //カメラのスムーズな移動
		transform.LookAt(follow); //強制的に、カメラをキャラクターを見させる。
	}


	void DebugVector () {
		Debug.DrawRay(follow.position, Vector3.up * distanceUp, Color.red); //キャラクターの(0,0,0)点からカメラの高さまでのベクトル
		Debug.DrawRay(follow.position, -1f * follow.forward * distanceAway, Color.blue); //キャラクターの(0,0,0)点から距離を保つベクトル
		Debug.DrawLine(follow.position, targetPosition, Color.magenta); //キャラクターの(0,0,0)点からカメラの位置までのベクトル
	}
}
