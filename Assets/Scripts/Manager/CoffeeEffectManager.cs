using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CoffeeEffectManager : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private LensDistortion lensDistortion;

    [SerializeField] private float intensity = -0.7f;
    [SerializeField] private float duration = 2.0f;

    private void Awake()
    {
        if (volume.profile.TryGet(out lensDistortion))
        {
            lensDistortion.intensity.value = 0f;
        }
    }

    public void TriggerEffect()
    {
        StopAllCoroutines(); // ȿ�� �ߺ� ����
        StartCoroutine(EffectRoutine());
    }

    private IEnumerator EffectRoutine()
    {
        if (lensDistortion == null) yield break;

        // ȿ�� �ߵ�
        lensDistortion.intensity.Override(intensity);

        yield return new WaitForSeconds(duration);

        // ȿ�� ����
        lensDistortion.intensity.Override(0f);
    }
}
