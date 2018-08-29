using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetGroup : MonoBehaviour {

    List<TimedTarget> targets;

    Dictionary<TimedTarget, TimedTarget.LockState> targetStates;

	// Use this for initialization
	void Start () {
        targets = GetComponentsInChildren<TimedTarget>().ToList();
        targetStates = new Dictionary<TimedTarget, TimedTarget.LockState>();
        foreach (TimedTarget t in targets)
        {
            targetStates[t] = TimedTarget.LockState.Locked;
            t.onStateChangeListeners += OnTargetStateChange;
        }
	}

    void OnTargetStateChange(TimedTarget target, TimedTarget.LockState state)
    {
        if (state == TimedTarget.LockState.Unlocked) {
            return; // We're expecting this 
        }
        targetStates[target] = state;
        if (targetStates.Values.All(s => s == TimedTarget.LockState.Pending))
        {
            targets.ForEach(t => t.Unlock());
            GetComponent<AudioSource>().Play();
        }
    }
}
