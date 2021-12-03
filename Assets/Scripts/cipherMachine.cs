using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
using System;
using System.Linq;
using Words;
using CipherMachine;

public class cipherMachine : MonoBehaviour
{

    public TextMesh[] screenTexts;
    public MeshRenderer[] screenMeshes;
    public KMBombInfo Bomb;
    public KMBombModule module;
    public AudioClip[] sounds;
    public KMAudio Audio;
    public TextMesh submitText;
    private List<PageInfo> pages;
    private string answer;
    private int page;
    private bool submitScreen;
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;
    private List<List<String>> wordList;
    public KMSelectable leftArrow;
    public KMSelectable rightArrow;
    public KMSelectable submit;
    public KMSelectable[] keyboard;
    public Font DEFAULT_FONT;
    public Material DEFAULT_FONT_MAT;
    void Awake()
    {
        moduleId = moduleIdCounter++;
        leftArrow.OnInteract += delegate () { left(leftArrow); return false; };
        rightArrow.OnInteract += delegate () { right(rightArrow); return false; };
        submit.OnInteract += delegate () { submitWord(submit); return false; };
        foreach (KMSelectable keybutton in keyboard)
        {
            KMSelectable pressedButton = keybutton;
            pressedButton.OnInteract += delegate () { letterPress(pressedButton); return false; };
        }
    }
    // Use this for initialization
    void Start()
    {
        pages = new List<PageInfo>();
        wordList = new Data().allWords;
        submitText.text = "1";
        page = 0;
        //Generating random word
        int ansLength = UnityEngine.Random.Range(0, 5);
        answer = wordList[ansLength][UnityEngine.Random.Range(0, wordList[ansLength].Count)].ToUpper();
        Debug.LogFormat("[Cipher Machine #{0}] Generated Word: {1}", moduleId, answer);
        string encrypt = answer + "";
        
        PageInfo[] temp = new VigenereCipher().encrypt(encrypt, "AA", "[Cipher Machine #" + moduleId + "]", Bomb);
        encrypt = temp[0].Screens[0].ToString();
        pages.Insert(0, temp[1]);
        temp = new AtbashCipher().encrypt(encrypt, "AB", "[Cipher Machine #" + moduleId + "]");
        encrypt = temp[0].Screens[0].ToString();
        pages.Insert(0, temp[1]);
        temp = new AffineCipher().encrypt(encrypt, "AC", "[Cipher Machine #" + moduleId + "]", Bomb);
        encrypt = temp[0].Screens[0].ToString();
        pages.Insert(0, temp[1]);

        ScreenInfo[] firstScreen = new ScreenInfo[9];
        firstScreen[0] = new ScreenInfo(encrypt, 25);
        for(int i = 1; i < 9; i++)
            firstScreen[i] = new ScreenInfo();
        pages.Insert(0, new PageInfo(firstScreen));
        getScreens();
    }
    
    string getKey(string k, string alpha, bool start)
    {
        for (int aa = 0; aa < k.Length; aa++)
        {
            for (int bb = aa + 1; bb < k.Length; bb++)
            {
                if (k[aa] == k[bb])
                {
                    k = k.Substring(0, bb) + "" + k.Substring(bb + 1);
                    bb--;
                }
            }
            alpha = alpha.Replace(k[aa].ToString(), "");
        }
        if (start)
            return (k + "" + alpha);
        else
            return (alpha + "" + k);
    }
    int correction(int p, int max)
    {
        while (p < 0)
            p += max;
        while (p >= max)
            p -= max;
        return p;
    }
    void left(KMSelectable arrow)
    {
        if (!moduleSolved)
        {
            Audio.PlaySoundAtTransform(sounds[0].name, transform);
            submitScreen = false;
            arrow.AddInteractionPunch();
            page--;
            page = correction(page, pages.Count);
            getScreens();
        }
    }
    void right(KMSelectable arrow)
    {
        if (!moduleSolved)
        {
            Audio.PlaySoundAtTransform(sounds[0].name, transform);
            submitScreen = false;
            arrow.AddInteractionPunch();
            page++;
            page = correction(page, pages.Count);
            getScreens();
        }
    }
    private void getScreens()
    {
        for (int aa = 0; aa < 8; aa++)
        {
            screenTexts[aa].text = pages[page].Screens[aa].Text;
            screenTexts[aa].fontSize = pages[page].Screens[aa].FontSize;
            if(pages[page].Screens[aa].TextFont == null)
            {
                screenTexts[aa].font = DEFAULT_FONT;
                screenMeshes[aa].material = DEFAULT_FONT_MAT;
            }
            else
            {
                screenTexts[aa].font = pages[page].Screens[aa].TextFont;
                screenMeshes[aa].material = pages[page].Screens[aa].FontMaterial;
            }
        }
        submitText.text = (page + 1) + "" + pages[page].Screens[8].Text;
    }
    void submitWord(KMSelectable submitButton)
    {
        if (!moduleSolved)
        {
            submitButton.AddInteractionPunch();
            if(submitScreen)
            {
                if (screenTexts[6].text.Equals(answer))
                {
                    Audio.PlaySoundAtTransform(sounds[2].name, transform);
                    module.HandlePass();
                    moduleSolved = true;
                    screenTexts[6].text = "";
                }
                else
                {
                    Audio.PlaySoundAtTransform(sounds[3].name, transform);
                    module.HandleStrike();
                    page = 0;
                    getScreens();
                    submitScreen = false;
                }
            }
        }
    }
    void letterPress(KMSelectable pressed)
    {
        if (!moduleSolved)
        {
            pressed.AddInteractionPunch();
            Audio.PlaySoundAtTransform(sounds[1].name, transform);
            if (submitScreen)
            {
                if (screenTexts[6].text.Length < answer.Length)
                    screenTexts[6].text = screenTexts[6].text + "" + pressed.GetComponentInChildren<TextMesh>().text;
            }
            else
            {
                submitText.text = "SUB";
                screenTexts[0].text = "";
                screenTexts[1].text = "";
                for(int aa = 0; aa < 8; aa++)
                    screenTexts[aa].text = "";
                screenTexts[6].text = pressed.GetComponentInChildren<TextMesh>().text;
                screenTexts[6].fontSize = 25;
                submitScreen = true;
            }
        }
    }
#pragma warning disable 414
    private string TwitchHelpMessage = "Move to other screens using !{0} right|left|r|l|. Submit the decrypted word with !{0} submit qwertyuiopasdfghjklzxcvbnm";
#pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {

        if (command.EqualsIgnoreCase("right") || command.EqualsIgnoreCase("r"))
        {
            yield return null;
            rightArrow.OnInteract();
            yield return new WaitForSeconds(0.1f);

        }
        if (command.EqualsIgnoreCase("left") || command.EqualsIgnoreCase("l"))
        {
            yield return null;
            leftArrow.OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        string[] split = command.ToUpperInvariant().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        if (split.Length != 2 || !split[0].Equals("SUBMIT") || split[1].Length != 6) yield break;
        int[] buttons = split[1].Select(getPositionFromChar).ToArray();
        if (buttons.Any(x => x < 0)) yield break;

        yield return null;

        yield return new WaitForSeconds(0.1f);
        foreach (char let in split[1])
        {
            keyboard[getPositionFromChar(let)].OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.1f);
        submit.OnInteract();
        yield return new WaitForSeconds(0.1f);
    }
    IEnumerator TwitchHandleForcedSolve()
    {
        if (submitScreen && !answer.StartsWith(screenTexts[2].text))
        {
            KMSelectable[] arrows = new KMSelectable[] { leftArrow, rightArrow };
            arrows[UnityEngine.Random.Range(0, 2)].OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        int start = submitScreen ? screenTexts[2].text.Length : 0;
        for (int i = start; i < 6; i++)
        {
            keyboard[getPositionFromChar(answer[i])].OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        submit.OnInteract();
        yield return new WaitForSeconds(0.1f);
    }
    private int getPositionFromChar(char c)
    {
        return "QWERTYUIOPASDFGHJKLZXCVBNM".IndexOf(c);
    }
}
