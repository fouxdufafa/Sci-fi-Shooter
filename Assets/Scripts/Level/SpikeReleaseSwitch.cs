using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeReleaseSwitch : MonoBehaviour, IDamageable {

    SpikeTargetGroup group;

    // Use this for initialization
    void Start () {
        group = GetComponentInParent<SpikeTargetGroup>();
	}

    public void TakeDamage(Damager damager)
    {
        group.DisengageSpikes();
    }
}
