using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Action<Ball> _killAction;

    public void Init(Action<Ball> killAction)
    {
        _killAction = killAction;
    }

    private void OnCollisionEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            _killAction(this);
        }
    }
}
