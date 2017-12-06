using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MSIDialogManager : MonoBehaviour
{
    private static MSIDialogManager instance;
    public static MSIDialogManager Instance { get { return instance; } }

    [SerializeField] private float SubtitleTime = 7f;
    [SerializeField] private float waitingTimeBetweenLetter = 0.05f;
    [SerializeField] private Text subsText;
    [SerializeField] private GameObject subsPanel;
	

    
    void Start ()
    {
        instance = this;

        subsPanel.SetActive(false);
    }
	
	
    public void AddSubtitles(string description)
    {
        subsPanel.SetActive(true);
        subsText.text = "";
        StartCoroutine(_AddSubtitles(description.ToCharArray()));
    }

    IEnumerator _AddSubtitles(char [] description)
    {
        foreach(char character in description)
        {
            subsText.text += character;
            yield return new WaitForSeconds(waitingTimeBetweenLetter);
        }

        yield return new WaitForSeconds(SubtitleTime);

        RemoveSubtitles();
    }

    public void AddInitialSubtitles(string description, string description2)
    {
        subsPanel.SetActive(true);
        subsText.text = "";
        StartCoroutine(_initialSubtitlesCoroutine(description.ToCharArray(), description2.ToCharArray()));
    }
    IEnumerator _initialSubtitlesCoroutine(char[] description, char[] description2)
    {
        foreach (char character in description)
        {
            subsText.text += character;
            yield return new WaitForSeconds(waitingTimeBetweenLetter);
        }

        yield return new WaitForSeconds(SubtitleTime);

        subsText.text = "";
        foreach(char character in description2)
        {
            subsText.text += character;
            yield return new WaitForSeconds(waitingTimeBetweenLetter);
        }

        MSIDragon.Instance.animateLightEffect();

        yield return new WaitForSeconds(SubtitleTime);

        MSIDragon.Instance.StartGame();

        RemoveSubtitles();
    }

    public void RemoveSubtitles()
    {
        subsPanel.SetActive(false);
        subsText.text = "";
    }
}
