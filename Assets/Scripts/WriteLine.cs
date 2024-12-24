using UnityEngine;

public class WriteLine : MonoBehaviour
{
    [SerializeField] BlackJackManager _BlackJackManager;
    [SerializeField] Color LineColor = Color.red; // Inspectorから設定可能な線の色
    float AffordedDistance => _BlackJackManager.AffordedDisntace; // 距離制限
    private LineRenderer lineRenderer;

    void Start()
    {
        // LineRendererコンポーネントを動的に追加
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f; // 線の開始幅
        lineRenderer.endWidth = 0.05f;   // 線の終了幅
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // シンプルなマテリアルを設定
        lineRenderer.startColor = LineColor;
        lineRenderer.endColor = LineColor;
    }

    public void WritingLine(Vector3 StartPosition, Vector3 endPosition)
    {

        // 距離を計算
        float distance = Vector3.Distance(StartPosition, endPosition);

        // 距離が制限内の場合に線を描画
        if (distance <= AffordedDistance)
        {
            lineRenderer.enabled = true; // 線を有効化
            lineRenderer.positionCount = 2; // 線を結ぶポイント数
            lineRenderer.SetPosition(0, StartPosition); // 始点
            lineRenderer.SetPosition(1, endPosition);  // 終点
        }
        else
        {
            lineRenderer.enabled = false; // 線を無効化
        }
    }
    public void fadeLine()
    {
        lineRenderer.enabled = false; // 線を無効化
    }
}