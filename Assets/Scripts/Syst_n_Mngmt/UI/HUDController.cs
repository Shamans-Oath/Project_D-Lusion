using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;

    public Animator cmp_anim;
    bool iconReady = true;
    float tempHealth;

    public Image lifeBar, shieldBar;
    public float lerpDuration;
    public Image parryBorder, parryCircle, dashCircle, parryIcon, dashIcon;
    public Color initialColor;
    public Color furyColor;
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
        iconReady = true;
        tempHealth = 100;
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
        dashCircle.fillAmount = cooldownRatio;

        Debug.Log(iconReady);

        if (dashCircle.fillAmount == 1 && iconReady == false)
        {
            cmp_anim.SetTrigger("Ready");
            iconReady = true;
        }
        else if(dashCircle.fillAmount <= 0.5f)
        {
            iconReady = false;
        }
    }

    public void UpdateLifeBar(int currentHealth, int maxHealth)
    {
        float healthRatio = Mathf.InverseLerp(0, maxHealth, currentHealth);

        lifeBar.fillAmount = Mathf.Lerp(lifeBar.fillAmount, healthRatio, lerpDuration);

        if(currentHealth == 0)
        {
            lifeBar.fillAmount = 0;
        }
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
