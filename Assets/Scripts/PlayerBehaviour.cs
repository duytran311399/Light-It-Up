using DuyTran;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public bool isDie;
    public event System.Action OnEnterFlatform;
    public Rigidbody2D _rigidbody;
    float _gravity = 9.8f;
    public float speedScaler = 1;

    private Vector3 startPlayerPostision;

    private void OnEnable()
    {
        OnEnterFlatform += EnterFlatform;
    }
    private void OnDisable()
    {
        OnEnterFlatform -= EnterFlatform;
    }
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _gravity = _rigidbody.gravityScale;
        startPlayerPostision = transform.position;
        DisablePlayer();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Flatform"))
        {
            OnEnterFlatform?.Invoke();
        }
        if (collision.gameObject.CompareTag("DeSpawn") || collision.gameObject.CompareTag("AreaDie"))
        {
            GameManager._instance.GameOver();
            //var hit = Physics2D.Raycast(transform.position, Vector2.down, 3f);
            AudioManager.instance.PlaySound(StringStatic.ButtonClick);
            GameController.instance.impactPool.SpanwEffectPool(1, transform.position);
            isDie = true;
        }
    }
    void EnterFlatform()
    {
        //AudioManager.instance.PlaySound(StringStatic.Ding);
    }
    public void DisablePlayer()
    {
        gameObject.SetActive(false);
    }
    public void EnablePlayerLightUp()
    {
        gameObject.SetActive(true);
        _rigidbody.gravityScale = 2;
        speedScaler = 1;
    }
    public void EnablePlayerChanleger()
    {
        gameObject.SetActive(true);
        _rigidbody.gravityScale = 0;
        speedScaler = 1;
    }
    public void UpdateSpeed(float speedScaler)
    {
        _rigidbody.gravityScale = Mathf.Clamp(speedScaler, 1, 10);
        this.speedScaler = speedScaler / _gravity + 1;
    }
    public void ResetPlayer()
    {
        gameObject.SetActive(true);
        isDie = false;
        transform.position = startPlayerPostision;
        _rigidbody.gravityScale = 0;
        speedScaler = 0;
    }
}
