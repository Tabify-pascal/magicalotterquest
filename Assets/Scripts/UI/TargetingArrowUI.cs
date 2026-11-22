
using UnityEngine;
using UnityEngine.UI;

public class TargetingArrowUI : Graphic
{
    [SerializeField, Range(4, 40)]
    private int segments = 24;

    [SerializeField] private float thickness = 5f;
    [SerializeField] private float curveHeight = 80f;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private bool isVisible;

    public void Show(Vector2 start, Vector2 end)
    {
        startPoint = start;
        endPoint = end;
        isVisible = true;
        SetVerticesDirty();
    }

    public void Hide()
    {
        isVisible = false;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (!isVisible)
            return;

        Vector2 controlPoint =
            (startPoint + endPoint) * 0.5f +
            Vector2.up * curveHeight;

        Vector2 previous = startPoint;

        for (int i = 1; i <= segments; i++)
        {
            float t = i / (float)segments;
            float inverse = 1f - t;

            Vector2 current =
                inverse * inverse * startPoint +
                2f * inverse * t * controlPoint +
                t * t * endPoint;

            AddSegment(vh, previous, current);
            previous = current;
        }
    }

    private void AddSegment(
        VertexHelper vh,
        Vector2 from,
        Vector2 to)
    {
        Vector2 direction = (to - from).normalized;
        Vector2 normal =
            new Vector2(-direction.y, direction.x) *
            thickness * 0.5f;

        int index = vh.currentVertCount;

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = from - normal;
        vh.AddVert(vertex);

        vertex.position = from + normal;
        vh.AddVert(vertex);

        vertex.position = to + normal;
        vh.AddVert(vertex);

        vertex.position = to - normal;
        vh.AddVert(vertex);

        vh.AddTriangle(index, index + 1, index + 2);
        vh.AddTriangle(index, index + 2, index + 3);
    }
}
