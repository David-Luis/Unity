using UnityEngine;

public class HealthBarUI : MonoBehaviour
{

    MaterialPropertyBlock matBlock;
    MeshRenderer meshRenderer;
    Camera mainCamera;
    DestructibleComponent destructible;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        matBlock = new MaterialPropertyBlock();
        destructible = GetComponentInParent<DestructibleComponent>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        meshRenderer.enabled = true;
        AlignCamera();
        UpdateParams();
    }

    private void UpdateParams()
    {
        meshRenderer.GetPropertyBlock(matBlock);
        matBlock.SetFloat("_Fill", destructible.GetHealth() / (float)destructible.GetMaxHealth());
        meshRenderer.SetPropertyBlock(matBlock);
    }

    private void AlignCamera()
    {
        if (mainCamera != null)
        {
            var camXform = mainCamera.transform;
            var forward = transform.position - camXform.position;
            forward.Normalize();
            var up = Vector3.Cross(forward, camXform.right);
            transform.rotation = Quaternion.LookRotation(forward, up);
        }
    }

}