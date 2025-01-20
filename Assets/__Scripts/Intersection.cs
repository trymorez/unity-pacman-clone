using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{
    [SerializeField] LayerMask wallLayer;
    public List<Vector2> OpenPath = new List<Vector2>();
    Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    Bounds bounds;

    void Awake()
    {
        bounds = GetComponent<Collider2D>().bounds;
    }
    void Start()
    {
        float layLength = 0.5f;

        foreach(var dir in directions)
        {
            Vector2 startPos = transform.position + (Vector3)(bounds.extents * dir);
            var hit = Physics2D.Raycast(startPos, dir, layLength, wallLayer);

            if (hit.collider == null)
            {
                OpenPath.Add(dir);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // ����� ������ ���������� ����

        foreach (var dir in OpenPath)
        {
            // ������� ���� ��ġ
            Vector2 startPos = (Vector2)transform.position + (Vector2)(bounds.extents * dir);
            // ����� �׸� �� ��ġ
            Vector2 endPos = startPos + dir * 0.5f; // ������� ���̸� ����

            // ����� ������ �׸���
            Gizmos.DrawLine(startPos, endPos);
        }
    }
}
