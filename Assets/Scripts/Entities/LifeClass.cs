using UnityEditor.Build.Pipeline;
using UnityEngine;


[System.Serializable]
public class LifeClass : MonoBehaviour
{
    public int maxHealth = 100;

    public int currentHealth;

    public void GainHealth(int amount)
    {
        if(currentHealth + amount >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else 
        {
            currentHealth += amount;
        }
    }

    public void LooseHealth(int amount)
    {
        if (currentHealth - amount <= 0)
        {
            currentHealth = 0;
        }
        else
        {
            currentHealth -= amount;
        }
    }

    public void PercentageHealthGain(float percentage)
    {
        if(!(percentage > 0 && percentage <= 100)) 
        {
            Debug.Log("Invalid number");
            return;
        }
        else
        {
            int amount = (int)((maxHealth * percentage) / 100);

            GainHealth(amount);
        }
    }

    public void PercentageHealthLoss(float percentage)
    {
        if (!(percentage > 0 && percentage <= 100))
        {
            Debug.Log("Invalid number");
            return;
        }
        else
        {
            int amount = (int)((maxHealth * percentage) / 100);

            LooseHealth(amount);
        }
    }

    public void AddMaxHealth(int amount, bool readjustment)
    {
        maxHealth += amount;

        if(readjustment)
        {
            float readjustmentRatio = (maxHealth != 0) ? (currentHealth * 100) / maxHealth : 0;

            currentHealth = (int)((maxHealth * readjustmentRatio) / 100);
        }
    }

    public void ReduceMaxHealth(int amount, bool readjustment)
    {
        maxHealth -= amount;

        if (maxHealth <= 0)
        {
            maxHealth = 0;
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }        

        if (readjustment)
        {
            float readjustmentRatio = (maxHealth != 0) ? (currentHealth * 100) / maxHealth : 0;

            currentHealth = (int)((maxHealth * readjustmentRatio) / 100);
        }
    }

    public void AddPercetangeMaxHealth(float percentage)
    {
        if (!(percentage > 0 && percentage <= 100))
        {
            Debug.Log("Invalid number");
            return;
        }
        else
        {
            int amount = (int)((maxHealth * percentage) / 100);

            AddMaxHealth(amount, true);
        }
    }

    public void ReducePercetangeMaxHealth(float percentage)
    {
        if (!(percentage > 0 && percentage <= 100))
        {
            Debug.Log("Invalid number");
            return;
        }
        else
        {
            int amount = (int)((maxHealth * percentage) / 100);

            ReduceMaxHealth(amount, true);
        }
    }

    public void AddPercetangeMaxHealthNR(float percentage)
    {
        if (!(percentage > 0 && percentage <= 100))
        {
            Debug.Log("Invalid number");
            return;
        }
        else
        {
            int amount = (int)((maxHealth * percentage) / 100);

            AddMaxHealth(amount, false);
        }
    }

    public void ReducePercetangeMaxHealthNR(float percentage)
    {
        if (!(percentage > 0 && percentage <= 100))
        {
            Debug.Log("Invalid number");
            return;
        }
        else
        {
            int amount = (int)((maxHealth * percentage) / 100);

            ReduceMaxHealth(amount, false);
        }
    }
}
