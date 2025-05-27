using UnityEngine;
using Vuforia;

public class LetterTracker : ITrackableEventHandler
{
    private TrackableBehaviour trackable;
    public string letter; // e.g. "A", "M", "O"

    void Start()
    {
        trackable = GetComponent<TrackableBehaviour>();
        if (trackable) trackable.RegisterTrackableEventHandler(this);
    }

    public void OnTrackableStateChanged(
        TrackableBehaviour.Status prevStatus,
        TrackableBehaviour.Status newStatus)
    {
        bool isVisible = newStatus == TrackableBehaviour.Status.DETECTED ||
                         newStatus == TrackableBehaviour.Status.TRACKED ||
                         newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED;

        MemoryGameManager.Instance.SetLetterVisible(letter, gameObject, isVisible);
    }
}
