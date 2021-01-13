using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //! 移動方向
    private Vector3 velocity = Vector3.zero;

    //! 移動のタイプ 
    private bool runMode = false;

    [Header("歩きの移動速度")]
    [SerializeField] private float walk_speed = 5.0f;

    [Header("走りの移動速度")]
    [SerializeField] private float run_speed = 10.0f;

    [Header("振り向き完了するまでの速度")]
    [SerializeField] private float apply_speed = 0.2f;

    [Header("重力の強さ")]
    [SerializeField] private float gravityScale = 10f;

    //! プレイヤーのカメラのスクリプト
    private PlayerCamera playerCamera = null;

    public void MyStart()
    {
        playerCamera = Camera.main.gameObject.GetComponent<PlayerCamera>();
        playerCamera.SearchFollowTarget(this.gameObject);
    }

    public void MyUpdate()
    {
        PlayerMoveAndRotation();
    }

    bool PlayerMoveAndRotation()
    {
        // WASD入力から、XZ平面(水平な地面)を移動する方向(velocity)を得ます
        velocity = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) { velocity.z += 1; }
        if (Input.GetKey(KeyCode.A)) { velocity.x -= 1; }
        if (Input.GetKey(KeyCode.S)) { velocity.z -= 1; }
        if (Input.GetKey(KeyCode.D)) { velocity.x += 1; }

        runMode = Input.GetKey(KeyCode.LeftShift) ? true : false;

        // 方向を正規化
        velocity = velocity.normalized;

        // いずれかの方向に移動している場合
        if (velocity.magnitude > 0)
        {
            // カメラの方向から、X-Z平面の単位ベクトルを取得
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            // 方向キーの入力値とカメラの向きから、移動方向を決定
            Vector3 moveForward = cameraForward * velocity.z + Camera.main.transform.right * velocity.x;

            // プレイヤーの回転(transform.rotation)の更新
            // 無回転状態のプレイヤーのZ+方向(後頭部)を、
            // カメラの水平回転(refCamera.hRotation)で回した移動の反対方向(-velocity)に回す回転に段々近づけます
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(moveForward),
                                                  apply_speed);

            // プレイヤーの位置(transform.position)の更新
            // カメラの水平回転(refCamera.hRotation)で回した移動方向(velocity)を足し込みます
            if (runMode)
            {
                transform.position += moveForward * run_speed * Time.deltaTime + Vector3.up * -gravityScale;
                //                playerCharacterController.Move(moveForward * run_speed * Time.deltaTime + Vector3.up * -gravityScale);
                return true;
            }
            else
            {
                transform.position += moveForward * walk_speed * Time.deltaTime + Vector3.up * -gravityScale;
                //                playerCharacterController.Move(moveForward * walk_speed * Time.deltaTime + Vector3.up * -gravityScale);
                return true;
            }
        }

        return false;
    }
}