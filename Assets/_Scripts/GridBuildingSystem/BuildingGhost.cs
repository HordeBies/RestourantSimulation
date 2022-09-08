using Bies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour {

    private Transform visual;

    private void Start()
    {
        ConstructionManager.OnSelectedChanged += Instance_OnSelectedChanged;
    }

    private void Instance_OnSelectedChanged(GridObject selectedGridObject)
    {
        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }

        GridObject placedObjectTypeSO = selectedGridObject;

        if (placedObjectTypeSO != null)
        {
            visual = Instantiate(placedObjectTypeSO.visual, Vector3.zero, Quaternion.identity);
            visual.parent = transform;
            visual.localPosition = Vector3.zero;
            visual.localEulerAngles = Vector3.zero;
            SetLayerRecursive(visual.gameObject, 11);
        }
    }

    private void LateUpdate()
    {
        if (GameManager.PointerOverUI)
        {
            visual?.gameObject.SetActive(false);
        }
        else
        {
            visual?.gameObject.SetActive(true);
        }
        Vector3 targetPosition = ConstructionManager.instance.GetMouseWorldSnappedPosition();
        targetPosition.y = 1f;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);

        transform.rotation = Quaternion.Lerp(transform.rotation, ConstructionManager.instance.GetSelectedGridObjectRotation(), Time.deltaTime * 15f);
    }

    private void SetLayerRecursive(GameObject targetGameObject, int layer)
    {
        targetGameObject.layer = layer;
        foreach (Transform child in targetGameObject.transform)
        {
            SetLayerRecursive(child.gameObject, layer);
        }
    }

}

