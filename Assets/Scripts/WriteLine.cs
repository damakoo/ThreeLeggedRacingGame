using UnityEngine;

public class WriteLine : MonoBehaviour
{
    [SerializeField] BlackJackManager _BlackJackManager;
    [SerializeField] Color LineColor = Color.red; // Inspector����ݒ�\�Ȑ��̐F
    float AffordedDistance => _BlackJackManager.AffordedDisntace; // ��������
    private LineRenderer lineRenderer;

    void Start()
    {
        // LineRenderer�R���|�[�l���g�𓮓I�ɒǉ�
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f; // ���̊J�n��
        lineRenderer.endWidth = 0.05f;   // ���̏I����
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // �V���v���ȃ}�e���A����ݒ�
        lineRenderer.startColor = LineColor;
        lineRenderer.endColor = LineColor;
    }

    public void WritingLine(Vector3 StartPosition, Vector3 endPosition)
    {

        // �������v�Z
        float distance = Vector3.Distance(StartPosition, endPosition);

        // �������������̏ꍇ�ɐ���`��
        if (distance <= AffordedDistance)
        {
            lineRenderer.enabled = true; // ����L����
            lineRenderer.positionCount = 2; // �������ԃ|�C���g��
            lineRenderer.SetPosition(0, StartPosition); // �n�_
            lineRenderer.SetPosition(1, endPosition);  // �I�_
        }
        else
        {
            lineRenderer.enabled = false; // ���𖳌���
        }
    }
    public void fadeLine()
    {
        lineRenderer.enabled = false; // ���𖳌���
    }
}