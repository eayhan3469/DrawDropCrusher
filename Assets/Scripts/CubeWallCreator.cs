using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeWallCreator : MonoBehaviour
{
    [SerializeField] private Transform _startingTransform;
    [SerializeField] private GameObject _cubeObject;

    [Header("Wall Parameters")]
    [SerializeField] private int _height;
    [SerializeField] private int _width;
    [SerializeField] private float _cubeSize;

    private MaterialPropertyBlock _materialPropertyBlock;

    private void Awake()
    {
        _materialPropertyBlock = new MaterialPropertyBlock();
    }

    void Start()
    {
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                var cube = Instantiate(_cubeObject, transform);
                cube.transform.position = new Vector3(_startingTransform.position.x + (x * _cubeSize), _startingTransform.position.y + (y * _cubeSize), Random.Range(-0.1f, 0.1f));
                cube.transform.localScale = Vector3.one * _cubeSize;
                cube.GetComponent<Rigidbody2D>().isKinematic = true;

                var renderer = cube.GetComponent<Renderer>();
                renderer.GetPropertyBlock(_materialPropertyBlock);
                _materialPropertyBlock.SetColor("_Color", Random.ColorHSV(0.4f, 0.5f, 1f, 1f, 0.5f, 1f)); //TODO: Renk değişimi aynı rengin farklı tonlarında rastgele yapılacak
                renderer.SetPropertyBlock(_materialPropertyBlock);
            }
        }
    }
}
