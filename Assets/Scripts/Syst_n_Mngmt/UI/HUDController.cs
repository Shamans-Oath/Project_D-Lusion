using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;

    public Image lifeBar, shieldBar;
    public float lerpDuration;
    public Image parryBorder, dashBorder, parryCircle, dashCircle, parryIcon, dashIcon;
    public Color initialColor;
    public Color furyColor;
    public Image furyBorder;
    public Image furyImage;
    public Sprite[] furyIcons;
    
    float targetPointD, targetPointP, targetPointID, targetPointIP;

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
            parryBorder.color = Color.Lerp(new Color(parryBorder.color.r, parryBorder.color.g, parryBorder.color.b, 0.25f), new Color(parryBorder.color.r, parryBorder.color.g, parryBorder.color.b, 1), targetPointP);
            if(parryBorder.fillAmount == 1)
            targetPointIP += Time.deltaTime;
            parryIcon.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, targetPointIP);
        }
        else
        {
            targetPointP = 0;
            targetPointIP = 0;
            parryBorder.color = new Color(parryBorder.color.r, parryBorder.color.g, parryBorder.color.b, 0.25f);
            parryIcon.color = new Color(1, 1, 1,0);
        }

        if (dashCircle.fillAmount == 1)
        {
            targetPointD += Time.deltaTime;
            dashBorder.color = Color.Lerp(new Color(dashBorder.color.r, dashBorder.color.g, dashBorder.color.b, 0.25f), new Color(dashBorder.color.r, dashBorder.color.g, dashBorder.color.b, 1), targetPointD);
            if(dashBorder.fillAmount == 1)
            targetPointID += Time.deltaTime;
            dashIcon.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, targetPointID);
        }
        else
        {
            targetPointD = 0;
            targetPointID = 0;
            dashBorder.color = new Color(dashBorder.color.r, dashBorder.color.g, dashBorder.color.b, 0.25f);
            dashIcon.color = new Color(1, 1, 1, 0);
        }


    }

    public void UpdateParryCooldown(float timer, float duration)
    {
        float cooldownRatio = Mathf.InverseLerp(duration, duration/2, timer);
        float borderRatio = Mathf.InverseLerp(duration/2, 0, timer);
        parryCircle.fillAmount = cooldownRatio;
        //if(parryCircle.fillAmount == 1)
        parryBorder.fillAmount = borderRatio;
    }

    public void UpdateDashCooldown(float timer, float duration)
    {
        float cooldownRatio = Mathf.InverseLerp(duration, duration / 2, timer);
        float borderRatio = Mathf.InverseLerp(duration / 2, 0, timer);
        dashCircle.fillAmount = cooldownRatio;
        //if(dashCircle.fillAmount == 1)
        dashBorder.fillAmount = borderRatio;
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

    public void UpdateBorderColor(float currentFurry, float maxFurry)
    {
        float furryRatio = Mathf.InverseLerp(0, maxFurry, currentFurry);

        parryBorder.color = Color.Lerp(new Color(initialColor.r, initialColor.g, initialColor.b, parryBorder.color.a), new Color(furyColor.r, furyColor.g, furyColor.b, parryBorder.color.a), furryRatio);
        dashBorder.color = Color.Lerp(new Color(initialColor.r, initialColor.g, initialColor.b, dashBorder.color.a), new Color(furyColor.r, furyColor.g, furyColor.b, dashBorder.color.a), furryRatio);
    }

    public void UpdateFuryBorder(float currentFurry, float maxFurry)
    {
        float furryRatio = Mathf.InverseLerp(0, maxFurry, currentFurry);

        furyBorder.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, furryRatio);
    }

    public void UpdateFuryImage(float currentFurry, float maxFurry)
    {
        if(currentFurry == 0)
        {
            furyImage.sprite = furyIcons[0];
        }
        else if(currentFurry >= maxFurry/5 && currentFurry < (maxFurry / 5) * 2)
        {
            furyImage.sprite = furyIcons[1];
        }
        else if (currentFurry >= (maxFurry / 5) * 2 && currentFurry < (maxFurry / 5) * 3)
        {
            furyImage.sprite = furyIcons[2];
        }
        else if (currentFurry >= (maxFurry / 5) * 3 && currentFurry < (maxFurry / 5) * 4)
        {
            furyImage.sprite = furyIcons[3];
        }
        else if (currentFurry == 100)
        {
            furyImage.sprite = furyIcons[4];
        }
    }
}
