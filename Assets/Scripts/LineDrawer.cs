using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;
    [SerializeField] private EdgeCollider2D _edgeCollider;
    [SerializeField] private float _drawingOffset = 1.5f;
    [SerializeField] private float _lineThickness = 0.2f;
    [SerializeField] LayerMask _layer;

    private Vector3 _hitPoint;
    private List<Vector3> _points;
    private List<Vector2> _pointsV2;

    private void Start()
    {
        _points = new List<Vector3>();
        _pointsV2 = new List<Vector2>();
        _line.startWidth = _lineThickness;
        _line.endWidth = _lineThickness;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_hitPoint, 0.2f);
    }

    void Update()
    {
        if (GameManager.instance.CurrentGameState != GameManager.GameState.Started)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            _points.Clear();
            _pointsV2.Clear();
            _edgeCollider.enabled = false;
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layer))
            {
                Debug.DrawLine(Camera.main.transform.position, hit.point);
                _hitPoint = hit.point;

                if (_points.Count == 0)
                {
                    _points.Add(hit.point - Vector3.forward * 0.05f);
                    _pointsV2.Add(hit.point);
                }
                else
                {
                    var distanceToLastPoint = GetDistanceToLastPoint(hit.point);

                    if (distanceToLastPoint > _drawingOffset)
                    {
                        _points.Add(hit.point - Vector3.forward * 0.05f);
                        _pointsV2.Add(hit.point);
                        _edgeCollider.enabled = true;
                    }
                }
            }
        }

        _line.positionCount = _points.Count;
        _line.SetPositions(_points.ToArray());

        _edgeCollider.SetPoints(_pointsV2);
    }

    private float GetDistanceToLastPoint(Vector3 point)
    {
        if (!_points.Any())
            return Mathf.Infinity;

        return Vector3.Distance(_points.Last(), point);
    }

    public Vector2 GetLaunchPoint()
    {
        return _pointsV2.OrderByDescending(p => p.x).First();
    }
}
