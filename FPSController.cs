using UnityEngine;
using System.Collections;

public class FPSController : MonoBehaviour
{
    [Header("移動速度")]
    [Range(1.0f, 10.0f)]

    // 移動速度を設定
    public float speedMove; 

    [Header("マウス感度")]
    [Range(1.0f, 10.0f)]

    // マウスの感度を設定
    public float sensivity;

    [Header("ジャンプ力")]
    [Range(1.0f, 20.0f)]

    // ジャンプの強さを設定
    public float jump; 

    [Header("空中時の重力")]
    [Range(1.0f, 30.0f)]

    // 空中での重力の大きさを設定
    public float airGravity; 

    [Header("地上時の重力")]
    [Range(1.0f, 100.0f)]

    // 地上での重力の大きさを設定
    public float groundGravity;

    [Header("回転制限")]
    // 回転角度の制限を設定
    public Vector2 clampAngle; 

    // プレイヤーのTransformのキャッシュ
    private Transform _transform;
    // メインカメラのキャッシュ
    private Camera _mainCamera;

    // 回転の角度を保持する変数
    private Vector2 _angle;

    // 移動方向を保持する変数
    private Vector3 _dir;

    // マウスの入力値を保持
    private float _mouseX;
    private float _mouseY;

    // キーボード入力を保持
    private float _vertical;
    private float _horizontal;

    // キャラクターコントローラーのコンポーネント
    private CharacterController _controller;
    // カメラのアニメーター
    private Animator _camAnimator;
    // 音声コンポーネント
    private AudioSource _source;
    // モバイルジョイスティック入力のコンポーネント
    private MobileInput _input;
    // タッチフィールドのコンポーネント
    private TouchField _field;
    // キャラクターのアニメーター
    private Animator CharAnimator;

    // 現在の重力値を保持
    private float _gravity;

    // ジャンプ中かどうかを判定するフラグ
    private bool _isJumping;

    void Start()
    {
        // 各コンポーネントのキャッシュを取得
        _controller = GetComponent<CharacterController>();
        // メインカメラの参照を取得
        _mainCamera = Camera.main;
        // カメラのアニメーターを取得
        _camAnimator = _mainCamera.GetComponent<Animator>();
        // 音声コンポーネントを取得
        _source = GetComponent<AudioSource>();
        // モバイルジョイスティックの取得
        _input = GameObject.FindGameObjectWithTag("MobileJoystick").GetComponent<MobileInput>();
        // タッチフィールドの取得
        _field = GameObject.FindGameObjectWithTag("TouchField").GetComponent<TouchField>();
        // キャラクターのアニメーターを取得
        CharAnimator = GetComponent<Animator>();
        // プレイヤーのTransformをキャッシュ
        _transform = transform; 
    }

    void Update()
    {
        // 移動処理
        HandleMovement();
        // 重力処理
        ApplyGravity();
        // キャラクターの移動処理
        _controller.Move(_dir * Time.deltaTime); 
    }

    void LateUpdate()
    {
        // マウスによる視点移動の処理
        HandleMouseLook(); 
    }

    // 移動処理
    private void HandleMovement()
    {
        if (_controller.isGrounded)
        {
            // ジャンプアニメーションをオフ
            CharAnimator.SetBool("Jump", false);

            // 垂直・水平移動の入力値を取得
            _vertical = _input.Vertical();
            _horizontal = _input.Horizontal();

            // 移動方向を設定
            _dir = _transform.TransformDirection(_horizontal, 0.0f, _vertical) * speedMove;

            // 移動しているかどうかのフラグ
            bool isMoving = _vertical != 0.0f || _horizontal != 0.0f;

            // 移動中の処理
            if (isMoving)
            {
                // 移動アニメーションをオン
                CharAnimator.SetBool("Go", true);
                // カメラの移動アニメーションをオン
                _camAnimator.SetBool("Moving", true);
                // 移動音を再生
                if (!_source.isPlaying) _source.Play(); 
            }
            else
            {
                // 停止中の処理
                // 移動アニメーションをオフ
                CharAnimator.SetBool("Go", false);
                // カメラの移動アニメーションをオフ
                _camAnimator.SetBool("Moving", false);
                // 移動音を停止
                if (_source.isPlaying) _source.Stop(); 
            }

            // ジャンプ処理
            if (_isJumping)
            {
                // ジャンプ力を付加
                _dir.y = jump;
                // カメラの移動アニメーションをオフ
                _camAnimator.SetBool("Moving", false);
                // ジャンプアニメーションをオン
                CharAnimator.SetBool("Jump", true);
                // ジャンプフラグをリセット
                _isJumping = false; 
            }
        }
    }

    // 重力処理
    private void ApplyGravity()
    {
        // 地上か空中かで重力を切り替える
        _gravity = _controller.isGrounded ? groundGravity : airGravity;
        // 重力を適用
        _dir.y -= _gravity * Time.deltaTime; 
    }

    // マウス視点移動処理
    private void HandleMouseLook()
    {
        // マウスの入力値を取得
        _mouseX = Input.GetAxis("Mouse X");
        _mouseY = Input.GetAxis("Mouse Y");

        // マウスの移動量に基づいて角度を計算
        _angle.x -= _field.TouchDist.y * sensivity * Time.deltaTime;
        _angle.y += _field.TouchDist.x * sensivity * Time.deltaTime;

        // 回転制限を適用
        _angle.x = Mathf.Clamp(_angle.x, -clampAngle.x, clampAngle.y);

        // プレイヤーの回転を設定
        Quaternion rot = Quaternion.Euler(_angle.x, _angle.y, 0.0f);
        _transform.rotation = rot;
    }

    // ジャンプのトリガー
    public void Jumping()
    {
        if (_controller.isGrounded && !_isJumping)
        {
            // ジャンプフラグをセット
            _isJumping = true; 
        }
    }
}

