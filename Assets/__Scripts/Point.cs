using TMPro;
using UnityEngine;
using DG.Tweening;

public class Point : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pointText;

    void Awake()
    {
        GameManager.OnGhostEatenPoint += OnGhostEatenPoint;
    }

    void OnDestroy()
    {
        GameManager.OnGhostEatenPoint -= OnGhostEatenPoint;
    }

    void OnGhostEatenPoint(Vector2 pos, int point)
    {
        var screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
        transform.position = screenPos;
        Debug.Log(screenPos);
        pointText.text = point.ToString();
        transform.DOScale(Vector3.one, 0.5f);
        transform.DOScale(Vector3.zero, 1f).SetDelay(1f);
    }

}
