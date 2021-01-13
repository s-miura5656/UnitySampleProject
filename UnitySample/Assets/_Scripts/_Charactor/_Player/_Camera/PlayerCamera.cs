using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    [Header("X軸回転の速度")]
    [SerializeField] private float h_turnSpeed = 5.0f;

    [Header("Y軸回転の速度")]
    [SerializeField] private float v_turnSpeed = 5.0f;

    private Transform player;

    [Header("カメラで見る対象との距離")]
    [SerializeField] private float distance = 5.0f;

    [Header("プレイヤー注目機能時にどの辺りをみるか(高さ)")]
    [SerializeField] private float player_height = 1.5f;

    [Header("カメラのズーム速度")]
    [SerializeField] private float zoom_speed = 0.5f;

    [Header("カメラの初期角度（X 軸）")]
    [SerializeField] private float initialize_angle_x = 30f;

    private Quaternion vRotation; // 垂直回転用変数   
    private Quaternion hRotation; // 水平回転用変数
    private float rotating_velocity_when_zoom = 1f; // ズーム時のカメラの回転速度

    [Header("TPS状態のX軸回転の上方向の制限")]
    [SerializeField] private float rotationUpLimitTps = 60f;

    [Header("TPS状態のX軸回転の下方向の制限")]
    [SerializeField] private float rotationDownLimitTps = 330f;

    [Header("FPS状態のX軸回転の上方向の制限")]
    [SerializeField] private float rotationUpLimitFps = 300f;

    [Header("FPS状態のX軸回転の下方向の制限")]
    [SerializeField] private float rotationDownLimitFps = 60f;

    // 回転の制限を付けるときに使う定数
    private const float rotation_half = 180f;             
    // プレイヤーとカメラの距離制限（最小値）
    private const float rotation_speed_min = 2f;          
    // プレイヤーとカメラの距離制限（最大値）
    private const float rotation_speed_max = 10f;         
    // マウススクロールの数字増やすための定数
    private const float constant_zoom_speed = 10f;        
    // FPS時のカメラの高さ
    private const float FPSCameraHeight = 1.5f;
    // FPS時のカメラの前後方向の距離
    private const float FPSForwardDistance = 0f;

    bool isFps = false;

    /// <summary>
    /// Y軸回転(横回転)
    /// </summary>
    public Quaternion HRotation
    {
        get { return hRotation; }
        set { hRotation = value; }
    }

    public Transform PlayerPos
    {
        get { return player; }
        set { player = value; }
    }

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        InitializedCamera();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            isFps = isFps ? false : true;
    }

    void LateUpdate()
    {
        if (isFps)
        {
            transform.position = player.position + transform.up * FPSCameraHeight + transform.forward * FPSForwardDistance;
            player.forward = new Vector3(transform.forward.x, 0, transform.forward.z);
            RotationCameraFPS();
        }
        else
        {
            ZoomCamera();
            RotationCameraTPS();
        }
    }

    void RotationCameraTPS()
    {
        float rotation_y = Input.GetAxis("Mouse X") * h_turnSpeed * rotating_velocity_when_zoom;
        float rotation_x = Input.GetAxis("Mouse Y") * v_turnSpeed * rotating_velocity_when_zoom;

        if (Input.GetMouseButton(1))
        {
            hRotation = Quaternion.Euler(0, rotation_y, 0);
            vRotation = Quaternion.Euler(rotation_x, 0, 0);

            // カメラの回転(transform.rotation)の更新
            // 方法1 : 垂直回転してから水平回転する合成回転とします
            Quaternion rote = hRotation * vRotation;

            transform.Rotate(rote.eulerAngles.x, 0f, 0f);
            transform.Rotate(0f, rote.eulerAngles.y, 0f);
        }

        // クォータニオン→オイラー
        Vector3 rotation = transform.rotation.eulerAngles;

        // 角度制限上
        if (rotation.x >= rotationUpLimitTps && rotation.x < rotation_half)
            rotation.x = rotationUpLimitTps;

        // 角度制限下
        if (rotation.x <= rotationDownLimitTps && rotation.x > rotation_half)
            rotation.x = rotationDownLimitTps;

        // オイラー→クォータニオン
        // Zを回転させたくないので 0 にして x,y だけ回転 
        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);

        transform.position = player.position + new Vector3(0f, player_height, 0f) - transform.rotation * Vector3.forward * distance;
    }

    void RotationCameraFPS()
    {
        float rotation_y = Input.GetAxis("Mouse X") * h_turnSpeed * rotating_velocity_when_zoom;
        float rotation_x = Input.GetAxis("Mouse Y") * v_turnSpeed * rotating_velocity_when_zoom;

        hRotation = Quaternion.Euler(0, rotation_y, 0);
        vRotation = Quaternion.Euler(rotation_x, 0, 0);

        // カメラの回転(transform.rotation)の更新
        // 方法1 : 垂直回転してから水平回転する合成回転とします
        Quaternion rote = hRotation * vRotation;

        transform.Rotate(rote.eulerAngles.x, 0f, 0f);
        transform.Rotate(0f, rote.eulerAngles.y, 0f);

        // クォータニオン→オイラー
        Vector3 rotation = transform.rotation.eulerAngles;

        // 角度制限上
        if (rotation.x <= rotationUpLimitFps && rotation.x > rotation_half)
            rotation.x = rotationUpLimitFps;

        // 角度制限下
        if (rotation.x >= rotationDownLimitFps && rotation.x < rotation_half)
            rotation.x = rotationDownLimitFps;

        // オイラー→クォータニオン
        // Zを回転させたくないので 0 にして x,y だけ回転 
        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
    }

    /// <summary>
    /// マウスホイールでのズーム機能
    /// </summary>
    void ZoomCamera()
    {
        distance += Input.GetAxis("Mouse ScrollWheel") * constant_zoom_speed;

        distance = Mathf.Clamp(distance, rotation_speed_min, rotation_speed_max);
    }

    /// <summary>
    /// 注視する対象を探す
    /// </summary>
    public void SearchFollowTarget(GameObject player_clone)
    {
        if (player == null)
            player = player_clone.transform;

        InitializedCamera();
    }
    
    private void InitializedCamera()
    {
        // 回転の初期化
        vRotation = Quaternion.Euler(initialize_angle_x, 0, 0);
        hRotation = Quaternion.identity;

        // 縦と横の回転の合成
        transform.rotation = hRotation * vRotation;

        if (isFps)
        {
            player.forward = transform.forward;
        }
        else
        {
            transform.position = player.position - transform.rotation * Vector3.forward * distance;
        }
    }
}
