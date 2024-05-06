using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectsImpactPool", menuName = "ScriptableObject/EffectsImpactPool")]
public class EffectsImpactPool : ScriptableObject
{
    public static EffectsImpactPool instance;
    public List<EffectsImpact> effectImpacts = new();
    public EffectsImpact effectClick;
    public void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public void OnEnable()
    {
        GameAction.OnClick += SpanwEffectPool;
    }
    public void OnDisable()
    {
        GameAction.OnClick -= SpanwEffectPool;
    }
    public void OnValidate()
    {
        SortById();
    }
    public void SpanwEffectPool(int id, Vector3 pos)
    {
        var effectIndex = effectImpacts.Find(e => e.id == id);
        SimplePool.Spawn(effectIndex.effectImpact.gameObject, pos, Quaternion.identity);
    }
    public void SpanwEffectPool(Vector2 pos)
    {
        if (effectClick == null) { effectClick = effectImpacts.Find(e => e.id == 3); }
        Vector3 vector3 = new Vector3(pos.x, pos.y, 0);
        SimplePool.Spawn(effectClick.effectImpact.gameObject, vector3, Quaternion.identity);
    }

    public void SortById()
    {
        //effectImpacts.Sort((a, b) => type == 0 ? a.id - b.id : a.order - b.order);
        effectImpacts.Sort((a, b) => a.id - b.id);
        GanerateCodeNameId();
    }
    public void GanerateCodeNameId()
    {
        effectImpacts.ForEach(effect => effect.codeName = $"{"Effect"}_{effect.id.ToString().PadLeft(5, '0')}");
    }

}
[Serializable]
public class EffectsImpact
{
    [HideInInspector] public int order;
    public int id; 
    public string codeName;
    public ParticleSystem effectImpact;
    public bool loop;
    public bool playAwake;
    public ParticleSystemStopAction stopAction;
    public EffectsImpact()
    {
        codeName = "EffecImpact";
        loop = false;
        playAwake = true;
        stopAction = ParticleSystemStopAction.Destroy;
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(EffectsImpactPool))]
public class ItemDbInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EffectsImpactPool myScript = (EffectsImpactPool)target;

        if (GUILayout.Button("GenarateID"))
        {
            for (int i = 0; i < myScript.effectImpacts.Count; i++)
                myScript.effectImpacts[i].id = i;
            myScript.SortById();
        }

        if (GUILayout.Button("Sorting by id"))
        {
            myScript.SortById();
        }
    }
}
#endif
