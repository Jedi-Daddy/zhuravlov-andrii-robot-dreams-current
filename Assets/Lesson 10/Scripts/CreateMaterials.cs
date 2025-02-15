using UnityEngine;

public class CreateMaterials : MonoBehaviour
{
    void Start()
    {
        // ������� ������������� ��������
        Material metal = new Material(Shader.Find("Standard"));
        metal.color = Color.gray;
        metal.SetFloat("_Metallic", 1f);
        metal.SetFloat("_Glossiness", 0.8f);

        // ������� ���������� ��������
        Material wood = new Material(Shader.Find("Standard"));
        wood.color = new Color(0.545f, 0.27f, 0.074f);
        wood.SetFloat("_Metallic", 0f);
        wood.SetFloat("_Glossiness", 0.2f);

        // ������� ����������� ��������
        Material plastic = new Material(Shader.Find("Standard"));
        plastic.color = Color.black;
        plastic.SetFloat("_Metallic", 0.1f);
        plastic.SetFloat("_Glossiness", 0.7f);

        // ������� ������� ��������
        Material leather = new Material(Shader.Find("Standard"));
        leather.color = new Color(0.36f, 0.25f, 0.20f);
        leather.SetFloat("_Metallic", 0f);
        leather.SetFloat("_Glossiness", 0.3f);

        // ������� �������������� �������� ��� �������
        Material transparent = new Material(Shader.Find("Standard"));
        transparent.color = new Color(1f, 0.5f, 0f, 0.5f); // ��������� � �������������
        transparent.SetFloat("_Mode", 3); // Transparent Mode
        transparent.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        transparent.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        transparent.SetInt("_ZWrite", 0);
        transparent.DisableKeyword("_ALPHATEST_ON");
        transparent.EnableKeyword("_ALPHABLEND_ON");
        transparent.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        transparent.renderQueue = 3000;

        // ��������� �������� � ������� (��������, ���� ������ ���������� "Blaster")
        GameObject blaster = GameObject.Find("Blaster");
        if (blaster != null)
        {
            Renderer rend = blaster.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material = metal; // ��������� ������������� ��������
            }
        }
    }
}
