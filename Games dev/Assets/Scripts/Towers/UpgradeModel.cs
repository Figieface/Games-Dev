using UnityEngine;

public class UpgradeModel : MonoBehaviour
{
    private string currentModelName = "TowerPrefab"; //always have model child be this name

    public void upgradeModel(GameObject newModel)
    {
        Transform oldModel = transform.Find(currentModelName); //get current model
        if (oldModel == null)
        {
            Debug.LogError("Cannot find model");
            return;
        }
        //save the position and rotation
        Vector3 position = oldModel.localPosition;
        Quaternion rotation = oldModel.localRotation;

        Destroy(oldModel.gameObject); //destroy old

        GameObject newModelInstance = Instantiate(newModel, transform);
        newModel.name = currentModelName; //so it can be reused cause it sets the name again, tho can manually set the upgraded tower name

        // Apply the original transform
        newModel.transform.localPosition = position;
        newModel.transform.localRotation = rotation;
    }
}
