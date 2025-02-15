using UnityEngine;

public class BlasterMaterials : MonoBehaviour
{
    public Material MetalMaterial;
    public Material WoodMaterial;
    public Material PlasticMaterial;
    public Material LeatherMaterial;
    public Material TransparentMaterial;

    void Start()
    {
        // ������� ������������� ��������
        MetalMaterial = new Material(Shader.Find("Standard"));
        MetalMaterial.color = Color.gray;
        MetalMaterial.SetFloat("_Metallic", 1f);
        MetalMaterial.SetFloat("_Glossiness", 0.8f);

        // ������� ���������� ��������
        WoodMaterial = new Material(Shader.Find("Standard"));
        WoodMaterial.color = new Color(0.545f, 0.27f, 0.074f); // ����������
        WoodMaterial.SetFloat("_Metallic", 0f);
        WoodMaterial.SetFloat("_Glossiness", 0.2f);

        // ������� ����������� ��������
        PlasticMaterial = new Material(Shader.Find("Standard"));
        PlasticMaterial.color = Color.black;
        PlasticMaterial.SetFloat("_Metallic", 0.1f);
        PlasticMaterial.SetFloat("_Glossiness", 0.7f);

        // ������� ������� ��������
        LeatherMaterial = new Material(Shader.Find("Standard"));
        LeatherMaterial.color = new Color(0.36f, 0.25f, 0.20f); // Ҹ���-����������
        LeatherMaterial.SetFloat("_Metallic", 0f);
        LeatherMaterial.SetFloat("_Glossiness", 0.3f);

        // ������� �������������� �������� ��� �������
        TransparentMaterial = new Material(Shader.Find("Standard"));
        TransparentMaterial.color = new Color(1f, 0.5f, 0f, 0.5f); // ���������
        TransparentMaterial.SetFloat("_Mode", 3); // Transparent Mode
        TransparentMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        TransparentMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        TransparentMaterial.SetInt("_ZWrite", 0);
        TransparentMaterial.DisableKeyword("_ALPHATEST_ON");
        TransparentMaterial.EnableKeyword("_ALPHABLEND_ON");
        TransparentMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        TransparentMaterial.renderQueue = 3000;
    }
}
