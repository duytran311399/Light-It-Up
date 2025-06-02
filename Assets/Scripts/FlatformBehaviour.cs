using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DuyTran;

public class FlatformBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] float speed = 1f;
    private Material render;
    Color emissionColorStart;
    Color colorStart;
    float _intensity = 2f;
    bool isTigger;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>().material;
        emissionColorStart = render.GetColor("_EmissionColor");
        colorStart = render.GetColor("_Color");
    }
    private void OnEnable()
    {
        GameAction.OnResetLevel += DeSpawnFlatform;
    }
    private void OnDisable()
    {
        GameAction.OnResetLevel -= DeSpawnFlatform;
    }
    private void Start()
    {
    }
    private void Update()
    {
        //transform.Translate(Vector3.up * speed * Time.deltaTime * GameController.instance.speedScaler);
    }
    private void FixedUpdate()
    {
        //_rigidbody.AddForce(new Vector2(Mathf.Clamp(horizontalMove * Time.fixedDeltaTime * force, -1.25f, 1.25f), 0), ForceMode2D.Impulse);
        if(GameController.instance.CheckPlaying())
            _rigidbody.velocity = new Vector2(0, speed);
    }
    void ResetFlatform()
    {
        isTigger = false;
        render.SetColor("_EmissionColor", emissionColorStart);
        render.SetColor("_Color", colorStart);
    }
    void RandomFlatformColor()
    {
        float x = Random.Range(0, 1f);
        float y = Random.Range(0, 1f);
        float z = Random.Range(0, 1f);
        Color color = new Vector4(Mathf.Clamp01(x), Mathf.Clamp01(y), Mathf.Clamp01(z));
        render.SetColor("_Color", color);
    }
    public void SpawnFlatform(Vector3 pos, Quaternion rot)
    {
        GameAction.a_SpawnFlatform?.Invoke();
        SimplePool.Spawn(transform.gameObject, pos, rot);
        //MoveToUp(pos.y);
    }
    public void SetGravityFlatform(float gravity)
    {
        _rigidbody.gravityScale = gravity;
    }
    public void DeSpawnFlatform()
    {
        GameAction.a_DeSpawnFlatform?.Invoke();
        SimplePool.Despawn(this.gameObject);
        ResetFlatform();
    }
    private void OnCollisionEnter2D(Collision2D enter)
    {
        if (enter.gameObject.CompareTag("DeSpawn"))
        {
            DeSpawnFlatform();
        }
        if (enter.gameObject.CompareTag("Player") && !isTigger)
        {
            isTigger = true;
            AudioManager.instance.PlaySound(StringStatic.Ding);
            render.SetColor("_EmissionColor", emissionColorStart * _intensity);
            GameAction.OnClick(enter.transform.position);
            GameAction.OnUpdateScore?.Invoke();
        }
    }
}
