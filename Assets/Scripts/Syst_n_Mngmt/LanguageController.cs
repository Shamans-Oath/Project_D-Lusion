using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageController : MonoBehaviour
{
    private bool _active = false;
    public GameData data;

    // Start is called before the first frame update
    void Start()
    {
        //int ID = PlayerPrefs.GetInt("LocaleKey", 1);
        ChangeLocale(data.language);
    }

    public void ChangeLocale(int localeID) 
    {
        if (_active) 
        {
            return;
        }
        StartCoroutine(SetLocale(localeID));
    }
    
    private IEnumerator SetLocale(int localeID) 
    {
        _active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        data.language = localeID;
        data.Save();
        _active = false;

    }

}
