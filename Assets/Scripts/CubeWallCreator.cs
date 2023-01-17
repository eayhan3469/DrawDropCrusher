using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeWallCreator : MonoBehaviour
{
    [SerializeField] private Transform _startingTransform;
    [SerializeField] private GameObject _cubeObject;
    [SerializeField] private Texture2D[] _wallTextures;

    [Header("Wall Parameters")]
    [SerializeField] private int _height;
    [SerializeField] private int _width;
    [SerializeField] private float _cubeSize;

    private Texture2D _selectedTexture;

    private MaterialPropertyBlock _materialPropertyBlock;

    private void Awake()
    {
        _materialPropertyBlock = new MaterialPropertyBlock();
        _selectedTexture = _wallTextures[Random.Range(0, _wallTextures.Length)];
    }

    void Start()
    {
        CreateWall();
        GameManager.instance.SetTotalCubeCount(_height * _width);
        DOTween.SetTweensCapacity(_height * _width * 2, 10);
    }

    private void CreateWall()
    {
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                var cube = Instantiate(_cubeObject, transform);
                cube.transform.position = new Vector3(_startingTransform.position.x + (x * _cubeSize), _startingTransform.position.y + (y * _cubeSize), Random.Range(-0.05f, 0.05f));
                cube.transform.localScale = Vector3.one * _cubeSize;
                cube.GetComponent<Rigidbody2D>().isKinematic = true;

                var renderer = cube.GetComponent<Renderer>();
                renderer.GetPropertyBlock(_materialPropertyBlock);
                _materialPropertyBlock.SetColor("_Color", GetColorFromTexture(x, y));
                //_materialPropertyBlock.SetColor("_Color", Random.ColorHSV(0.4f, 0.5f, 1f, 1f, 0.5f, 1f)); //TODO: Renk değişimi aynı rengin farklı tonlarında rastgele yapılacak
                renderer.SetPropertyBlock(_materialPropertyBlock);
            }
        }
    }

    private Color GetColorFromTexture(int x, int y)
    {
        var widthRatio = _selectedTexture.width / _width;
        var heightRatio = _selectedTexture.height / _height;

        return _selectedTexture.GetPixel(x * widthRatio, y * heightRatio);
    }
}
