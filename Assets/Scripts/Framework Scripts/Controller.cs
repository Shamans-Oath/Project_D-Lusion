using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour, IActivable
{
    [Header("Settings")]
    public Settings settings;

    [Header("Control")]
    protected bool active;

    [Header("Components")]
    public Dictionary<System.Type, IFeatureSetup> features;

    private void Awake()
    {
        Setup();
        ToggleActive(true);
    }

    private void Update()
    {
        if (!active) return;

        UpdateFeatures();
    }

    private void FixedUpdate()
    {
        if (!active) return;

        FixedUpdateFeatures();
    }

    public virtual void Setup()
    {
        settings.AssemblySettings();
        List<IFeatureSetup> featureList = new List<IFeatureSetup>(GetComponents<IFeatureSetup>());
        features = new Dictionary<System.Type, IFeatureSetup>();

        foreach (IFeatureSetup feature in featureList)
        {
            features.Add(feature.GetType(), feature);
            feature.SetupFeature(this);
        }
    }

    public virtual void UpdateFeatures()
    {
        foreach (IFeatureSetup feature in features.Values)
        {
            IFeatureUpdate featureUpdate = feature as IFeatureUpdate;

            if (featureUpdate == null) continue;

            featureUpdate.UpdateFeature(this);
        }
    }

    public virtual void FixedUpdateFeatures()
    {
        foreach (IFeatureSetup feature in features.Values)
        {
            IFeatureFixedUpdate featureFixedUpdate = feature as IFeatureFixedUpdate;

            if (featureFixedUpdate == null) continue;

            featureFixedUpdate.FixedUpdateFeature(this);
        }
    }

    public T SearchFeature<T>() where T: IFeatureSetup
    {
        if (!features.ContainsKey(typeof(T))) return default(T);

        return (T)features[typeof(T)];
    }

    public void CallFeature<T>() where T : IFeatureSetup
    {
        if (!active) return;

        IFeatureSetup feature = SearchFeature<T>();

        IFeatureAction featureAction = feature as IFeatureAction;

        if (featureAction == null) return;

        featureAction.FeatureAction(this);
    }

    public bool GetActive()
    {
        return active;
    }

    public void ToggleActive(bool active)
    {
        this.active = active;

        foreach (IFeatureSetup feature in features.Values)
        {
            IActivable activableFeature = feature as IActivable;

            if (activableFeature != null) activableFeature.ToggleActive(active && activableFeature.GetActive());
        }
    }
}
