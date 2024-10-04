using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;

    public Image lifeBar, shieldBar, parryCircle, dashCircle;
    public Image parryBorder, dashBorder, parryIcon, dashIcon;
    public float lerpDuration;
    float targetPointD, targetPointP;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(parryCircle.fillAmount == 1)
        {
            targetPointP += Time.deltaTime;
            parryBorder.color = Color.Lerp(new Color(1, 1, 1, 0.25f), Color.white, targetPointP);
            parryIcon.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, targetPointP);
        }
        else
        {
            targetPointP = 0;
            parryBorder.color = new Color(1, 1, 1, 0.25f);
            parryIcon.color = new Color(1, 1, 1,0);
        }

        if (dashCircle.fillAmount == 1)
        {
            targetPointD += Time.deltaTime;
            dashBorder.color = Color.Lerp(new Color(1, 1, 1, 0.25f), Color.white, targetPointD);
            dashIcon.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, targetPointD);
        }
        else
        {
            targetPointD = 0;
            dashBorder.color = new Color(1, 1, 1, 0.25f);
            dashIcon.color = new Color(1, 1, 1, 0);
        }
    }

    public void UpdateParryCooldown(float timer, float duration)
    {
        float cooldownRatio = Mathf.InverseLerp(duration, 0, timer);
        parryCircle.fillAmount = cooldownRatio;
        parryBorder.fillAmount = cooldownRatio;
    }

    public void UpdateDashCooldown(float timer, float duration)
    {
        float cooldownRatio = Mathf.InverseLerp(duration, 0, timer);
        dashCircle.fillAmount = cooldownRatio;
        dashBorder.fillAmount = cooldownRatio;
    }

    public void UpdateLifeBar(int currentHealth, int maxHealth)
    {
        float healthRatio = Mathf.InverseLerp(0, maxHealth, currentHealth);

        lifeBar.fillAmount = Mathf.Lerp(lifeBar.fillAmount, healthRatio, lerpDuration);
    }

    public void UpdateShieldBar(int currentShield, int maxShield)
    {
        float shieldRatio = Mathf.InverseLerp(0, maxShield, currentShield);

        shieldBar.fillAmount = Mathf.Lerp(shieldBar.fillAmount, shieldRatio, lerpDuration);
    }

    /*IEnumerator UpdateLife(int currentHealth, int maxHealth)
    {
        float t = 0;
        float healthRatio = Mathf.InverseLerp(0, maxHealth, currentHealth);

        while(true)
        {
            yield return null;

            t += Time.deltaTime;
            lifeBar.fillAmount = Mathf.Lerp(lifeBar.fillAmount, healthRatio, t/lerpDuration);

            if(t > lerpDuration)
            {
                break;
            }
        }
    }*/
}
