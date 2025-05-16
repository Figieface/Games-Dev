
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private float previewYOffset = 0.1f;

    [SerializeField] private GameObject cellIndicator;
    private GameObject previewObject;

    [SerializeField] private Material previewMaterialsPrefab;
    private Material previewMaterialInstance;

    private Renderer cellIndicatorRenderer;

    private bool enoughCurrency = true;

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialsPrefab); 
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size, int cost) //instatises a prefab to show as preview
    {
        if (cost > StructureShop.currency)
        {
            enoughCurrency = false;
        }
        else
        {
            enoughCurrency = true;
        }
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject); //makes prefab use preview material
        PrepareCursor(size); //makes grid cursor fit to size of prefab
        cellIndicator.SetActive(true); //turns on grid cursor
    }

    private void PrepareCursor(Vector2Int size) //makes grid cursor fit to size of prefab
    {
        if(size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRenderer.material.mainTextureScale = size; //will scale the cursor tile tiling being used on my gridsquare to the scale we increase it to
        }
    }

    private void PreparePreview(GameObject previewObject) //cycles through all materials on an object to make it preview mat
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview() //removes the preview prefab
    {
        cellIndicator.SetActive(false);
        if(previewObject != null )
            Destroy(previewObject);
    }

    public void UpdatePosition(Vector3 position, bool validity) //moves cursor then checks what colour grid cursor should be
    {
        if (previewObject != null)
        {
            MovePreview(position);
            ApplyFeedbackToPreview(validity);
        }

        MoveCusor(position);
        ApplyFeedbackToCursor(validity);
    }

    private void ApplyFeedbackToPreview(bool validity) //sets material colour of preview based on validity
    {
        Color c = validity ? Color.white : Color.red; //if validity true then colour white, false then red

        c.a = 0.5f;
        if (enoughCurrency == false)
        {
            c = Color.red;
        }
        previewMaterialInstance.color = c;
    }

    private void ApplyFeedbackToCursor(bool validity) //sets material colour of grid cursor based on validity
    {
        Color c = validity ? Color.white : Color.red; //if validity true then colour white, false then red

        c.a = 0.5f;
        cellIndicatorRenderer.material.color = c;
    }

    private void MoveCusor(Vector3 position) //moves grid cursor position
    {
        cellIndicator.transform.position = position;
    }

    private void MovePreview(Vector3 position) //moves preview and adds yoffset so you can see the preview if ontop of another object
    {
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }

    internal void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedbackToCursor(false); //cursor colours inverted for removing preview

    }
}
