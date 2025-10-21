using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class DamageTextMeshPro : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float fadeSpeed = 1f;
    public float lifeTime = 1.5f;

    private float totalLifeTime;

    private TextMeshProUGUI textMeshPro;
    private Color textColor;

    void Update()
    {
        textMeshPro.color = textColor;
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.up);

        // 检查是否进入放大阶段 (前10%生命周期)
        float scaleUpTime = totalLifeTime * 0.1f; // 10%的时间
        float scaleDownTime = totalLifeTime * 0.2f; // 20%的时间

        if (lifeTime > totalLifeTime - scaleUpTime)
        {
            // 放大阶段
            float scaleRatio = 1.0f + (scaleUpTime - (lifeTime - (totalLifeTime - scaleUpTime))) / scaleUpTime;
            transform.localScale = Vector3.one * Mathf.Lerp(1.0f, 1.5f, scaleRatio);
        }
        else if (lifeTime > totalLifeTime - scaleDownTime)
        {
            // 缩小恢复阶段
            float scaleRatio = (lifeTime - (totalLifeTime - scaleDownTime)) / (scaleDownTime - scaleUpTime);
            transform.localScale = Vector3.one * Mathf.Lerp(1.5f, 1.0f, 1.0f - scaleRatio);
        }

        // 最后80%时间逐渐透明
        if (lifeTime <= totalLifeTime * 0.8f)
        {
            textColor.a -= fadeSpeed * Time.deltaTime;
            textMeshPro.color = textColor;
        }

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetupDamage(int _damage, Color _color)
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        if (textMeshPro == null)
        {
            Destroy(gameObject);
            return;
        }
        totalLifeTime = lifeTime;
        textMeshPro.text = _damage.ToString();
        textColor = _color;
    }
}
