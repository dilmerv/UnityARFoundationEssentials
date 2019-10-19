using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;


public class csShurikenEffectChanger : MonoBehaviour
{
	public void ShurikenParticleScaleChange(float _Value)
	{
		ParticleSystem[] ParticleSystems = GetComponentsInChildren<ParticleSystem>();

        transform.localScale *= _Value;

		foreach(ParticleSystem _ParticleSystem in ParticleSystems) {
			_ParticleSystem.startSpeed *= _Value;
			_ParticleSystem.startSize *= _Value;
			_ParticleSystem.gravityModifier *= _Value;
			SerializedObject _SerializedObject = new SerializedObject(_ParticleSystem);
			_SerializedObject.FindProperty("CollisionModule.particleRadius").floatValue *= _Value;
			_SerializedObject.FindProperty("ShapeModule.radius").floatValue *= _Value;
			_SerializedObject.FindProperty("ShapeModule.boxX").floatValue *= _Value;
			_SerializedObject.FindProperty("ShapeModule.boxY").floatValue *= _Value;
			_SerializedObject.FindProperty("ShapeModule.boxZ").floatValue *= _Value;
			_SerializedObject.FindProperty("VelocityModule.x.scalar").floatValue *= _Value;
			_SerializedObject.FindProperty("VelocityModule.y.scalar").floatValue *= _Value;
			_SerializedObject.FindProperty("VelocityModule.z.scalar").floatValue *= _Value;
			_SerializedObject.FindProperty("ClampVelocityModule.x.scalar").floatValue *= _Value;
			_SerializedObject.FindProperty("ClampVelocityModule.y.scalar").floatValue *= _Value;
			_SerializedObject.FindProperty("ClampVelocityModule.z.scalar").floatValue *= _Value;
			_SerializedObject.FindProperty("ClampVelocityModule.magnitude.scalar").floatValue *= _Value;
			_SerializedObject.ApplyModifiedProperties();
		}
	}
}
#endif
