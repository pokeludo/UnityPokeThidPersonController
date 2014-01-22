using UnityEngine;
using System.Collections;

public class MyThirdPersonCamera : MonoBehaviour {

	[SerializeField]
	private float distanceAway;
	[SerializeField]
	private float distanceUp;
	[SerializeField]
	private float smooth;
	[SerializeField]
	private Transform followX; //rigidbodyのないオブジェクトをフォローする為。

	private Vector3 targetPosition;
	private Vector3 lookDir;

	private Vector3 velocityCamSmooth = Vector3.zero;
	[SerializeField]
	private float camSmoothDampTime = 0.1f;

	// Use this for initialization
	void Start () {
		followX = GameObject.FindWithTag("Player").transform; //キャラクターの(0,0,0)点にあるオブジェクト
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//フレーム毎に、最終的に呼ばれている。全てのオブジェクトの位置が決まっているので、カメラの為に良く使われている。
	void LateUpdate() {

		Vector3 characterOffset = followX.position + new Vector3(0, distanceUp, 0); //キャラクターの(0,0,0)点からY軸の値までのベクトル
	
		lookDir = characterOffset - this.transform.position; //キャラクターとカメラのあいだの距離
		lookDir.y = 0;
		lookDir.Normalize();
		Debug.DrawRay(this.transform.position, lookDir, Color.green);
		
		targetPosition = characterOffset + followX.up * distanceUp - lookDir * distanceAway;  //カメラの位置の計算
	
		CompensateForWalls(characterOffset, ref targetPosition); //壁の調整
		smoothPosition(this.transform.position, targetPosition); //カメラの移動

		transform.LookAt(characterOffset); //カメラ見る方向の調整
	}

	//　カメラの現在のフレームの位置と次のフレームのカメラの位置のスムーズな移動
	private void smoothPosition(Vector3 fromPos, Vector3 toPos) {
		this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
	}

	//　カメラが壁に入らないような関数
	private void CompensateForWalls(Vector3 fromObject, ref Vector3 toTarget) {
		Debug.DrawLine(fromObject, toTarget, Color.cyan);

		RaycastHit wallHit = new RaycastHit();
		if(Physics.Linecast(fromObject, toTarget, out wallHit)) {  //キャラクターとカメラの現在位置のあいだの光線を発光
			Debug.DrawRay(wallHit.point, Vector3.left, Color.red); //何か接触が認識されたようだ
			toTarget = new Vector3(wallHit.point.x, toTarget.y, wallHit.point.z); //カメラの位置をこの接触点に止める
		}
	}

	private void debugVector() {
		Debug.DrawRay(followX.position, Vector3.up * distanceUp, Color.red);
		Debug.DrawRay(followX.position, -1f * followX.forward * distanceAway, Color.blue);
		Debug.DrawLine(followX.position, transform.position, Color.magenta);
	}
}
