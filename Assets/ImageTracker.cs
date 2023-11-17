using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class ImageTracker : MonoBehaviour
{
    private ARTrackedImageManager trackedImages;
    public GameObject[] ArPrefabs;
    private Dictionary<string, GameObject> ARObjectMap = new Dictionary<string, GameObject>();
    
    void Awake()
    {
        trackedImages = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImages.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImages.trackedImagesChanged -= OnTrackedImagesChanged;
    }

   void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
{
    foreach (var trackedImage in eventArgs.added)
    {
        foreach (var arPrefab in ArPrefabs)
        {
            if (trackedImage.referenceImage.name == arPrefab.name)
            {
                var newPrefab = Instantiate(arPrefab, trackedImage.transform);
                ARObjectMap.Add(trackedImage.name, newPrefab);
            }
        }
    }

    foreach (var trackedImage in eventArgs.updated)
    {
        if (ARObjectMap.TryGetValue(trackedImage.name, out var gameObject))
        {
            gameObject.SetActive(trackedImage.trackingState == TrackingState.Tracking);
        }
    }

    foreach (var trackedImage in eventArgs.removed)
    {
        if (ARObjectMap.TryGetValue(trackedImage.name, out var gameObject))
        {
            Destroy(gameObject);
            ARObjectMap.Remove(trackedImage.name);
        }

    }
}


   
}









