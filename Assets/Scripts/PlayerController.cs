using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PlayerBehaviour
{
    [SerializeField] float force = 5f;
    float horizontalMove = 0f;
    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * force;
    }
    private void FixedUpdate()
    {
        //_rigidbody.AddForce(new Vector2(Mathf.Clamp(horizontalMove * Time.fixedDeltaTime * force, -1.25f, 1.25f), 0), ForceMode2D.Impulse);

        if (!isDie)
            _rigidbody.velocity = new Vector2(horizontalMove * Time.fixedDeltaTime * force * speedScaler, _rigidbody.velocity.y);
    }
}
