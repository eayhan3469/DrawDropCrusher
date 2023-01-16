using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollector : Singleton<CubeCollector>
{
    [SerializeField] private float _pullPower;

    private List<Cube> _cubeList;

    protected override void Awake()
    {
        _cubeList = new List<Cube>();
    }

    private void Update()
    {
        foreach (var c in _cubeList)
        {
            c.RigidBody.AddForce(transform.position * Time.deltaTime * _pullPower);
        }
    }

    public void AddCube(Cube cube)
    {
        _cubeList.Add(cube);

        if ((float)_cubeList.Count / GameManager.instance.TotalCubeCount > 0.95f)
        {
            GameManager.instance.OnGameCompleted?.Invoke();
        }
    }
}
