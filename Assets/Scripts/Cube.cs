using UnityEngine;

public class Cube : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;

    public Rigidbody2D RigidBody => _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    public void EffectFromBall()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(Vector2.left * 200f);
        _collider.isTrigger = false;
        gameObject.tag = "Untagged";
        CubeCollector.instance.AddCube(this);
    }
}
