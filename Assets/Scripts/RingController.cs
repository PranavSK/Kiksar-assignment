using System.Collections.Generic;
using Mediapipe;
using Mediapipe.Unity;
using Mediapipe.Unity.CoordinateSystem;
using Mediapipe.Unity.HandTracking;
using UnityEngine;


public class RingController : HandTrackingSolution
{
    const bool expectedToBeMirrored = true;
    public enum Finger { Thumb, Index, Middle, Ring, Pinky };
    private readonly int[][] FingerReferenceLandmarkIndices = {
        new int[] { 2, 3, 5 }, // Thumb
        new int[] { 5, 6, 9 }, // Index
        new int[] { 9, 10, 13 },
        new int[] { 13, 14, 17 },
        new int[] { 17, 18, 19 },
    };

    [SerializeField] private Finger _finger;
    [SerializeField] private HandLandmarkListAnnotation.Hand _hand;
    [SerializeField] private Transform _ring;

    private int _activeHandIndex = -1;

    private IList<NormalizedLandmarkList> _currentHandLandmarkLists;
    private IList<LandmarkList> _currentHandWorldLandmarkLists;
    private IList<ClassificationList> _currentHandedness;

    private List<PointAnnotation> _activeFingerAnnotations = new();
    private bool _isHandLandmarkDirty = false;

    void LateUpdate()
    {

        if (_isHandLandmarkDirty)
        {
            UpdateRingPosition();
        }
    }

    protected override void OnStartRun()
    {
        base.OnStartRun();
        graphRunner.OnHandLandmarksOutput += OnHandLandmarksOutput;
        graphRunner.OnHandWorldLandmarksOutput += OnHandWorldLandmarksOutput;
        graphRunner.OnHandednessOutput += OnHandednessOutput;
    }

    private void OnHandLandmarksOutput(object stream, OutputEventArgs<List<NormalizedLandmarkList>> eventArgs)
    {
        if (eventArgs.value != null || _currentHandLandmarkLists != null)
        {
            _currentHandLandmarkLists = eventArgs.value;
            _isHandLandmarkDirty = true;
        }
    }

    private void OnHandWorldLandmarksOutput(object stream, OutputEventArgs<List<LandmarkList>> eventArgs)
    {
        if (eventArgs.value != null || _currentHandWorldLandmarkLists != null)
        {
            _currentHandWorldLandmarkLists = eventArgs.value;
            _isHandLandmarkDirty = true;
        }
    }

    private void OnHandednessOutput(object stream, OutputEventArgs<List<ClassificationList>> eventArgs)
    {
        if (eventArgs.value != null || _currentHandedness != null)
        {
            _currentHandedness = eventArgs.value;
            UpdateActiveHandedness();
        }
    }

    private void UpdateRingPosition()
    {
        if (_currentHandLandmarkLists != null &&
            _activeHandIndex != -1 &&
            _currentHandedness.Count > _activeHandIndex)
        {
            var handLandmarkList = _currentHandLandmarkLists[_activeHandIndex];
            var requiredLandmarkIndices = FingerReferenceLandmarkIndices[(int)_finger];

            var imageSource = ImageSourceProvider.ImageSource;
            var isMirrored = expectedToBeMirrored ^ imageSource.isHorizontallyFlipped ^ imageSource.isFrontFacing;
            var rotationAngle = imageSource.rotation.Reverse();
            var screenRect = GetScreenRect();

            var bottomLandmark = handLandmarkList.Landmark[requiredLandmarkIndices[0]];
            var topLandmark = handLandmarkList.Landmark[requiredLandmarkIndices[1]];

            var bottomPosition = screenRect.GetPoint(bottomLandmark, imageSource.rotation.Reverse(), expectedToBeMirrored);
            bottomPosition.z = 0.0f;
            var topPosition = screenRect.GetPoint(topLandmark, imageSource.rotation.Reverse(), expectedToBeMirrored);
            topPosition.z = 0.0f;
            var direction = (topPosition - bottomPosition).normalized;

            var position = bottomPosition + direction * 110;
            position.z = 0.0f;
            position.x = -position.x;
            _ring.localPosition = position;
            Debug.Log($"UpdateRingPosition {_ring.localPosition} {position} {bottomPosition} {topPosition} {direction}");

        }

        if (_currentHandWorldLandmarkLists != null && 
            _activeHandIndex != -1 &&
            _currentHandedness.Count > _activeHandIndex) {
            var handWorldLandmarkList = _currentHandWorldLandmarkLists[_activeHandIndex];
            var requiredLandmarkIndices = FingerReferenceLandmarkIndices[(int)_finger];

            var bottomLandmark = handWorldLandmarkList.Landmark[requiredLandmarkIndices[0]];
            var topLandmark = handWorldLandmarkList.Landmark[requiredLandmarkIndices[1]];

            var bottomWorldPosition = new Vector3(bottomLandmark.X, bottomLandmark.Y, bottomLandmark.Z);
            var topWorldPosition = new Vector3(topLandmark.X, topLandmark.Y, topLandmark.Z);

            var direction = (topWorldPosition - bottomWorldPosition).normalized;

            var rotation = Quaternion.LookRotation(direction, Vector3.up);
            _ring.rotation = rotation;
        }
        _isHandLandmarkDirty = false;

    }

    private void UpdateActiveHandedness()
    {
        Debug.Log($"UpdateActiveHandedness {_currentHandedness}");
        if (_currentHandedness != null)
        {
            _activeHandIndex = -1;
            for (var i = 0; i < _currentHandedness.Count; i++)
            {
                var classification = _currentHandedness[i].Classification[0];
                if ((classification.Label == "Left" && _hand == HandLandmarkListAnnotation.Hand.Left) ||
                    (classification.Label == "Right" && _hand == HandLandmarkListAnnotation.Hand.Right))
                {
                    _activeHandIndex = i;
                    break;
                }

            }
        }
    }

    private UnityEngine.Rect GetScreenRect()
    {
        return screen.GetComponent<RectTransform>().rect;
    }

}
