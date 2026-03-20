using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private LensDistortion lensDistortion;

    [SerializeField] private float intensity = -0.7f;
    [SerializeField] private float duration = 2.0f;

    [SerializeField] private GameObject gameClearEffect;

    private void Awake()
    {
        if (volume.profile.TryGet(out lensDistortion))
        {
            lensDistortion.intensity.value = 0f;
        }
    }

    private void Start()
    {
        GameManager.instance.OnGameClearEffect += GameClearEffect;
    }

    public void TriggerEffect()
    {
        StopAllCoroutines(); // 효과 중복 방지
        StartCoroutine(EffectRoutine());
    }

    private IEnumerator EffectRoutine()
    {
        if (lensDistortion == null) yield break;

        // 효과 발동
        lensDistortion.intensity.Override(intensity);

        yield return new WaitForSeconds(duration);

        // 효과 원복
        lensDistortion.intensity.Override(0f);
    }

    public void GameClearEffect(Transform playerTransform)
    {
        Instantiate(gameClearEffect, playerTransform.position + playerTransform.up * 4f + playerTransform.right * 4, Quaternion.identity);
        Instantiate(gameClearEffect, playerTransform.position + playerTransform.up * 4f - playerTransform.right * 4, Quaternion.identity);
    }

    private void OnDestory()
    {
        GameManager.instance.OnGameClearEffect -= GameClearEffect;
    }
}
