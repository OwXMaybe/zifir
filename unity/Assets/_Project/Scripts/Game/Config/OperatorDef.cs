using UnityEngine;

namespace Zifir.Config
{
    /// <summary>
    /// Operator (playable class) definition. Speeds in world units/second
    /// (original px/s ÷ 16).
    /// </summary>
    [CreateAssetMenu(menuName = "Zifir/Operator", fileName = "Operator")]
    public class OperatorDef : ScriptableObject
    {
        [Header("Identity")]
        public string id = "sentinel";
        public string displayName = "SENTINEL";
        public Color visorColor = new(0.48f, 0.69f, 1f);

        [Header("Stats")]
        [Min(1f)] public float maxHealth = 130f;
        [Min(0.1f)] public float moveSpeed = 5.4f;
        [Range(0f, 3f)] public float noiseMultiplier = 1f;
        [Range(0.1f, 2f)] public float damageTakenMultiplier = 1f;
        public bool canDash = true;

        [Header("Sprint")]
        [Min(1f)] public float sprintMultiplier = 1.55f;
        [Min(0f)] public float staminaMax = 100f;
        [Min(0f)] public float staminaDrainPerSecond = 22f;
        [Min(0f)] public float staminaRegenPerSecond = 14f;
    }

    /// <summary>Code factory for built-in operators (phase 1: Sentinel).</summary>
    public static class OperatorLibrary
    {
        public static OperatorDef CreateSentinel()
        {
            var o = ScriptableObject.CreateInstance<OperatorDef>();
            o.name = "Operator_Sentinel";
            o.id = "sentinel";
            o.displayName = "SENTINEL";
            o.maxHealth = 130f;
            o.moveSpeed = 5.4f;
            o.visorColor = new Color(0.48f, 0.69f, 1f);
            return o;
        }
    }
}
