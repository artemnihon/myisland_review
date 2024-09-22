using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    // プレイヤーがトリガーと接触したときに移動されるゲームオブジェクト
    public GameObject Play;

    // プレイヤーの移動先となるターゲット位置
    private Vector3 targetPosition = new Vector3(103, 3, 137);

    // 初期化処理
    void Start()
    {
     
    }

    // 他のオブジェクトがトリガーに入った際に呼び出される処理
    void OnTriggerEnter(Collider coll)
    {
        // 接触したオブジェクトがCharacterControllerを持っているか確認
        if (coll.CompareTag("Player"))
        {
            // 接触したオブジェクトの名前をコンソールに出力
            Debug.Log(coll.gameObject.name);

            // メッセージをコンソールに出力
            Debug.Log("!");

            // プレイヤーを指定した位置に移動
            Play.transform.position = targetPosition;
        }
    }
}