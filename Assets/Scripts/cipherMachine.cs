using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CipherMachine;
using KeepCoding;
using UnityEngine;
using Words;

public class cipherMachine : MonoBehaviour
{
    private static readonly CipherBase[] _allCiphers = CMTools.NewArray<CipherBase>(
        new ADFGXCipher(invert: false), new ADFGXCipher(invert: true),
        new AESCipher(),
        new AffineCipher(invert: false), new AffineCipher(invert: true),
        new AlbertiCipher(invert: false), new AlbertiCipher(invert: true),
        new AlphabeticalDisorderCipher(),
        new AMSCOTransposition(invert: false), new AMSCOTransposition(invert: true),
        new ArithmeticSequenceCipher(),
        new AtbashCipher(),
        new AutokeyCipher(invert: false), new AutokeyCipher(invert: true),
        new BazeriesCipher(invert: false), new BazeriesCipher(invert: true),
        new BellasoCipher(),
        new BinaryCipher(),
        new BinaryGearCipher(invert: false), new BinaryGearCipher(invert: true),
        new BitSwitchCipher(invert: false), new BitSwitchCipher(invert: true),
        new BookCipher(),
        new BurrowsWheelerTransform(),
        new CaesarCipher(invert: false), new CaesarCipher(invert: true),
        new CaesareanRoleSwitchingCipher(invert: false), new CaesareanRoleSwitchingCipher(invert: true),
        new CaesarShuffleCipher(invert: false), new CaesarShuffleCipher(invert: true),
        new ChainBitRotationCipher(invert: false), new ChainBitRotationCipher(invert: true),
        new ChainRotationCipher(invert: false), new ChainRotationCipher(invert: true),
        new Chaocipher(invert: false), new Chaocipher(invert: true),
        new ChineseRemainderCipher(invert: false), new ChineseRemainderCipher(invert: true),
        new CircleCipher(invert: false), new CircleCipher(invert: true),
        new CollonCipher(),
        new ColumnarTransposition(invert: false), new ColumnarTransposition(invert: true),
        new CompositeSpinningJumpingLeapfrogOrphanageCipher(invert: false), new CompositeSpinningJumpingLeapfrogOrphanageCipher(invert: true),
        new CondiCipher(invert: false), new CondiCipher(invert: true),
        new ConjugatedMatrixBifidCipher(invert: false), new ConjugatedMatrixBifidCipher(invert: true),
        new CubeCipher(invert: false), new CubeCipher(invert: true),
        new DifferentialCipher(invert: false), new DifferentialCipher(invert: true),
        new DigrafidCipher(invert: false), new DigrafidCipher(invert: true),
        new Dreamcipher(invert: false), new Dreamcipher(invert: true),
        new DualTriplexReflectorCipher(invert: false), new DualTriplexReflectorCipher(invert: true),
        new EnigmaCipher(),
        new ExtinctionTransposition(),
        new FoursquareCipher(invert: false), new FoursquareCipher(invert: true),
        new FractionatedMorseCipher(),
        new GlobalOffsetCipher(invert: false), new GlobalOffsetCipher(invert: true),
        new GracieCipher(invert: false), new GracieCipher(invert: true),
        new GrandpreCipher(),
        new GrilleTransposition(invert: false), new GrilleTransposition(invert: true),
        new GROMARKCipher(invert: false), new GROMARKCipher(invert: true),
        new HillCipher(invert: false), new HillCipher(invert: true),
        new HomophonicCipher(),
        new IncrementalPolyalphabeticCipher(invert: false), new IncrementalPolyalphabeticCipher(invert: true),
        new JumpOverCipher(invert: false), new JumpOverCipher(invert: true),
        new LogicCipher(),
        new LorenzCipher(),
        new M209Cipher(),
        new McDondaldsChickenNuggetBigMacCipher(),
        new MechanicalCipher(invert: false), new MechanicalCipher(invert: true),
        new MirroredMatrixCipher(),
        new MonoalphabeticCipher(invert: false), new MonoalphabeticCipher(invert: true),
        new MonosodiumGlutamateCipher(invert: false), new MonosodiumGlutamateCipher(invert: true),
        new RubiksCubeCipher(invert: false), new RubiksCubeCipher(invert: true), new MonoalphabeticRubiksCubeCipher(invert: false), new MonoalphabeticRubiksCubeCipher(invert: true),
        new MalespinCipher(),
        new MorbitCipher(),
        new MyszkowskiTransposition(invert: false), new MyszkowskiTransposition(invert: true),
        new NicodemusCipher(invert: false), new NicodemusCipher(invert: true),
        new NotreDameCipher(invert: false), new NotreDameCipher(invert: true),
        new OwOCipher(invert: false), new OwOCipher(invert: true),
        new PancakeTransposition(invert: false), new PancakeTransposition(invert: true),
        new ParallelogramCipher(),
        new PigpenMazeCipher(invert: false), new PigpenMazeCipher(invert: true),
        new PingPongStraddlingCheckerboardCipher(),
        new PizzaSliceCipher(invert: false), new PizzaSliceCipher(invert: true),
        new PlayfairCipher(invert: false), new PlayfairCipher(invert: true),
        new PortaCipher(),
        new PortaxCipher(),
        new PrissyCipher(invert: false), new PrissyCipher(invert: true),
        new QuagmireCipher(invert: false), new QuagmireCipher(invert: true),
        new QuadrantReflectionCipher(invert: false), new QuadrantReflectionCipher(invert: true),
        new RagbabyCipher(invert: false), new RagbabyCipher(invert: true),
        new RedefenceTransposition(invert: false), new RedefenceTransposition(invert: true),
        new RouteTransposition(invert: false), new RouteTransposition(invert: true),
        new RozierCipher(invert: false), new RozierCipher(invert: true),
        new RSACipher(),
        new ScytaleTransposition(invert: false), new ScytaleTransposition(invert: true),
        new SeanCipher(),
        new SemaphoreRotationCipher(invert: false), new SemaphoreRotationCipher(invert: true),
        new SlidefairCipher(),
        new SmokeyCipher(),
        new SolitaireCipher(invert: false), new SolitaireCipher(invert: true),
        new SquareCipher(),
        new StrangelyElusiveLetterCipher(),
        new StripCipher(invert: false), new StripCipher(invert: true),
        new StuntedBlindPolybiusCipher(invert: false), new StuntedBlindPolybiusCipher(invert: true),
        new TemptationStairwayCipher(),
        new TransposedHalvedPolybiusCipher(invert: false), new TransposedHalvedPolybiusCipher(invert: true),
        new TriangleCipher(invert: false), new TriangleCipher(invert: true),
        new TridigitalCipher(),
        new TrifidCipher(invert: false), new TrifidCipher(invert: true),
        new TripleTriplexReflectorCipher(),
        new TrisquareCipher(),
        new UbchiTransposition(invert: false), new UbchiTransposition(invert: true),
        new VarietyCipher(),
        new VICPhoneCipher(),
        new VigenereCipher(invert: false), new VigenereCipher(invert: true));

    public TextMesh[] screenTexts;
    public MeshRenderer[] screenTextMeshes;
    public MeshRenderer submitMesh;
    public Material[] materials;
    public KMBombInfo Bomb;
    public KMBombModule module;
    public KMGameInfo GameInfo;
    public AudioClip[] sounds;
    public KMAudio Audio;
    public TextMesh submitText;
    public KMSelectable leftArrow;
    public KMSelectable rightArrow;
    public KMSelectable submit;
    public KMSelectable[] keyboard;
    public TextMesh[] keyboardLtrs;

    public Font[] Fonts;
    public Material[] FontMaterials;

    private PageInfo[] pages;
    private string answer;
    private int overallTpScore = 0;
    private string overallTpScoreString = "";
    private int page = 0;
    private bool submitScreen;
    private static int moduleIdCounter = 1;
    private int moduleId;
    private bool moduleSolved;
    private bool moduleSelected;
    private readonly Color[] textColors = { Color.white, Color.black };

    private static List<MissionSettings> missionSettings;

    [TextArea(3, 10)]
    public string EditorMissionSettings;    // Only used in the Unity Editor

    void Awake()
    {
        if (Application.isEditor)
            for (var i = 0; i < _allCiphers.Length; i++)
            {
                for (var j = i + 1; j < _allCiphers.Length; j++)
                {
                    if (_allCiphers[i].Code == _allCiphers[j].Code && !_allCiphers[i].GetType().Equals(_allCiphers[j].GetType()))
                        Debug.LogErrorFormat(@"{0} ({1}) and {2} ({3}) use the same code ({4}).", _allCiphers[i].Name, _allCiphers[i].GetType().Name, _allCiphers[j].Name, _allCiphers[j].GetType().Name, _allCiphers[i].Code);
                    if (_allCiphers[i].Name == _allCiphers[j].Name)
                        Debug.LogErrorFormat(@"{0} ({1}) and {2} ({3}) have the same name.", _allCiphers[i].Name, _allCiphers[i].GetType().Name, _allCiphers[j].Name, _allCiphers[j].GetType().Name);
                }
            }

        moduleId = moduleIdCounter++;
        leftArrow.OnInteract += delegate () { left(leftArrow); return false; };
        rightArrow.OnInteract += delegate () { right(rightArrow); return false; };
        submit.OnInteract += delegate () { submitWord(submit); return false; };
        module.GetComponent<KMSelectable>().OnFocus += delegate { moduleSelected = true; };
        module.GetComponent<KMSelectable>().OnDefocus += delegate { moduleSelected = false; };
        foreach (KMSelectable keybutton in keyboard)
        {
            KMSelectable pressedButton = keybutton;
            pressedButton.OnInteract += delegate () { letterPress(pressedButton); return false; };
        }

        Bomb.OnBombExploded += delegate { missionSettings = null; };
        Bomb.OnBombSolved += delegate { missionSettings = null; };
        GameInfo.OnStateChange += state =>
        {
            if (state == KMGameInfo.State.Transitioning)
                missionSettings = null;
        };
    }

    void Start()
    {
        var settings = GetMissionSettings();

        // Generate random word
        var word = answer = settings == null || settings.WordLengths == null ? new Data().PickWord(4, 8) : new Data().PickWord(settings.WordLengths.PickRandom());

        // For debugging
        //word = answer = "COZY";

        Debug.LogFormat("[Cipher Machine #{0}] Solution: {1}", moduleId, answer);

        // If there are no settings specified in the mission, or the settings don’t specify cipher codes, use default values
        if (settings == null)
            settings = new MissionSettings();
        if (settings.Ciphers == null)
        {
            settings.Ciphers = settings.Pick == null ? new string[] { "N", "T", "N" } : Enumerable.Repeat("A", settings.Pick.Value).ToArray();
            settings.CipherSettings = Enumerable.Repeat(CipherSetting.Random, settings.Pick ?? 3).ToArray();
        }

        // Determine which ciphers are possible candidate for each code
        var candidates = settings.Ciphers
            .Select((code, ix) =>
                code == "A" ? _allCiphers :
                code == "N" ? _allCiphers.Where(cipher => !cipher.IsTransposition).ToArray() :
                code == "T" ? _allCiphers.Where(cipher => cipher.IsTransposition).ToArray() :
                _allCiphers.Where(cipher => cipher.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase) && (settings.CipherSettings[ix] == CipherSetting.Random || (settings.CipherSettings[ix] == CipherSetting.EncryptOnly) == cipher.IsInvert)).ToArray())
            .ToArray();

        if (settings.Order == CipherOrder.Random)
            candidates.Shuffle();
        else
            Array.Reverse(candidates);

        // If ‘pick’ is specified, reduce the set of candidates to the desired number, but keep the order fixed
        if (settings.Pick != null)
            candidates = Enumerable.Range(0, candidates.Length).ToList().Shuffle().Take(settings.Pick.Value).OrderBy(ix => ix).Select(ix => candidates[ix]).ToArray();

        // Decide on the actual list of ciphers
        var ciphers = new List<CipherBase>();
        foreach (var candidateList in candidates)
        {
            // Avoid duplicate ciphers if possible, but fall back to duplicates if forced
            var list = candidateList.Where(newCipher => !ciphers.Any(existingCipher => existingCipher.Code == newCipher.Code)).ToArray();
            ciphers.Add(list.Length == 0 ? candidateList.PickRandom() : list.PickRandom());
        }

        // Generate the pages for all of the ciphers
        var pagesList = new List<PageInfo>();
        overallTpScore = 0;
        var overallTpScores = new List<string>();
        for (var i = 0; i < ciphers.Count; i++)
        {
            Debug.LogFormat("[Cipher Machine #{0}] Encrypting {1} with {2} ({3})", moduleId, word, ciphers[i].Name, ciphers[i].Code);
            var result = ciphers[i].Encrypt(word, Bomb);
            foreach (var msg in result.LogMessages)
                Debug.LogFormat("[Cipher Machine #{0}] [{1}] {2}", moduleId, ciphers[i].Name, msg);
            Debug.LogFormat("[Cipher Machine #{0}] Result: {1}", moduleId, result.Encrypted);
            var checksum = Enumerable.Range(0, word.Length).Sum(ix => (word[ix] - 'A' + 1) * (ix + 1)) % 23;
            word = result.Encrypted;
            foreach (var p in result.Pages)
            {
                p.Code = ciphers[i].Code;
                if (i > 0)
                    p.Checksum = checksum;
            }
            pagesList.InsertRange(0, result.Pages);
            overallTpScore += result.Score;
            overallTpScores.Add(string.Format("{0}:{1}", ciphers[i].Code, result.Score));
        }
        overallTpScoreString = string.Format("{0} = {1}", overallTpScores.Join(" + "), overallTpScore);
        pagesList.Insert(0, new PageInfo(new ScreenInfo[] { word }));
        pages = pagesList.ToArray();
        getScreens();
    }

    void left(KMSelectable arrow)
    {
        if (!moduleSolved)
        {
            Audio.PlaySoundAtTransform(sounds[0].name, transform);
            submitScreen = false;
            arrow.AddInteractionPunch();
            page = (page + pages.Length - 1) % pages.Length;
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
            page = (page + 1) % pages.Length;
            getScreens();
        }
    }
    private void getScreens()
    {
        for (int aa = 0; aa < 8; aa++)
        {
            if (aa >= pages[page].Screens.Length)
            {
                screenTexts[aa].text = "";
                screenTexts[aa].font = Fonts[0];
                screenTextMeshes[aa].material = FontMaterials[0];
            }
            else
            {
                screenTexts[aa].text = pages[page].Screens[aa].Text;
                if (pages[page].Screens[aa].Text != null)
                    screenTexts[aa].fontSize = getFontSize(pages[page].Screens[aa].Text.Length, aa % 2);
                screenTexts[aa].font = Fonts[(int) pages[page].Screens[aa].Font];
                screenTextMeshes[aa].material = FontMaterials[(int) pages[page].Screens[aa].Font];
            }
        }
        submitMesh.material = materials[pages[page].Invert ? 1 : 0];
        submitText.color = textColors[pages[page].Invert ? 1 : 0];
        submitText.text = (page + 1) + pages[page].Code;
        submitText.fontSize = getFontSize(submitText.text.Length, 2);
        for (var ltr = 0; ltr < 26; ltr++)
            keyboardLtrs[ltr].color = pages[page].Checksum == ltr ? Color.yellow : Color.white;
    }

    private int getFontSize(int length, int screenType)
    {
        switch (screenType)
        {
            case 0: return (length <= 6) ? 35 : (length == 7) ? 32 : 28;
            case 1: return (length <= 3) ? 25 : 20;
            default: return (length <= 3) ? 65 : (length == 4) ? 50 : 40;
        }
    }

    void submitWord(KMSelectable submitButton)
    {
        if (!moduleSolved)
        {
            submitButton.AddInteractionPunch();
            if (submitScreen)
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
                for (int aa = 0; aa < 8; aa++)
                    screenTexts[aa].text = "";
                screenTexts[6].text = pressed.GetComponentInChildren<TextMesh>().text;
                screenTexts[6].fontSize = new int[] { 35, 35, 35, 32, 28 }[answer.Length - 4];
                screenTexts[6].font = Fonts[0];
                screenTextMeshes[6].material = FontMaterials[0];
                submitScreen = true;
                submitMesh.material = materials[0];
                submitText.color = textColors[0];
                for (var ltr = 0; ltr < 26; ltr++)
                    keyboardLtrs[ltr].color = Color.white;
            }
        }
    }

    private MissionSettings GetMissionSettings()
    {
        if (missionSettings == null)
        {
            missionSettings = new List<MissionSettings>();
            string description = Application.isEditor ? EditorMissionSettings : Game.Mission.Description;
            if (description == null)
                return null;
            var matches = Regex.Matches(description, @"^\[Cipher ?Machine\] (.*)$", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            var warnings = new List<string>();
            for (var i = 0; i < matches.Count; i++)
            {
                var usedOptions = new HashSet<string>();
                var set = new MissionSettings();
                var opts = matches[i].Groups[1].Value.Replace("\r", "").Split(',');
                foreach (var opt in opts)
                {
                    var kv = opt.Split('=');
                    if (!usedOptions.Add(kv[0].Trim().ToLowerInvariant()))
                    {
                        warnings.Add(string.Format("Ignoring duplicate option: {0}.", opt));
                        continue;
                    }
                    switch (kv[0].Trim().ToLowerInvariant())
                    {
                        case "ciphers":
                            var ciphersRaw = kv[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            var ciphers = new List<string>();
                            var cipherSettings = new List<CipherSetting>();
                            foreach (var c in ciphersRaw)
                            {
                                if (c.Length < 1 || c.Length > 3 || (c.Length == 1 && !"ANT".Contains(c[0])))
                                {
                                    warnings.Add(string.Format("Invalid cipher: {0}. Valid values are: “A” (any cipher), “T” (random transposition), “N” (random non-transposition), a two-letter code (specific cipher), or a two-letter code followed by “d”/“e” for “decrypt-only”/“encrypt-only”.", c));
                                    continue;
                                }
                                var code = c;
                                var cs = CipherSetting.Random;
                                if (c.Length > 1)
                                {
                                    code = c.Substring(0, 2);
                                    if (!_allCiphers.Any(cipher => cipher.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase)))
                                    {
                                        warnings.Add(string.Format("Invalid cipher code: {0}. Valid cipher codes are: {1}.", code, _allCiphers.Select(cipher => cipher.Code).Distinct().OrderBy(x => x).Join(", ")));
                                        continue;
                                    }
                                    if (c.Length == 3)
                                    {
                                        var ed = c.Substring(2);
                                        if (ed.Equals("e", StringComparison.InvariantCultureIgnoreCase))
                                            cs = CipherSetting.EncryptOnly;
                                        else if (ed.Equals("d", StringComparison.InvariantCultureIgnoreCase))
                                            cs = CipherSetting.DecryptOnly;
                                        else
                                        {
                                            warnings.Add(string.Format("Invalid cipher code: {0}. The third character must be “d” (decrypt only), “e” (encrypt only) or absent (random).", c));
                                            continue;
                                        }
                                        if (!_allCiphers.Any(cipher => cipher.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase) && (cs == CipherSetting.EncryptOnly) == cipher.IsInvert))
                                        {
                                            warnings.Add(string.Format("Cipher {0} does not have {1} version.", code, cs == CipherSetting.EncryptOnly ? "an encrypt-only" : "a decrypt-only"));
                                            cs = CipherSetting.Random;
                                        }
                                    }
                                }
                                ciphers.Add(code);
                                cipherSettings.Add(cs);
                            }
                            if (ciphers.Count > 0)
                            {
                                set.Ciphers = ciphers.ToArray();
                                set.CipherSettings = cipherSettings.ToArray();
                            }
                            break;

                        case "order":
                            if (kv[1].Trim().Equals("fixed", StringComparison.InvariantCultureIgnoreCase))
                                set.Order = CipherOrder.Fixed;
                            else
                                warnings.Add("The ‘order’ option only accepts the value ‘fixed’. To force a random order of ciphers, simply omit the ‘order’ option.");
                            break;

                        case "len":
                            if (kv[1].Any(ch => !(ch >= '4' && ch <= '8') && ch != ' '))
                                warnings.Add("The ‘len’ option only accepts digits 4–8.");
                            set.WordLengths = kv[1].Where(ch => ch >= '4' && ch <= '8').Select(ch => ch - '0').ToArray();
                            if (set.WordLengths.Length == 0)
                                set.WordLengths = null;
                            break;

                        case "pick":
                            int pickNum;
                            if (int.TryParse(kv[1].Trim(), out pickNum))
                                set.Pick = pickNum;
                            else
                                warnings.Add(string.Format("The ‘pick’ option requires a number. {0} is not a number.", kv[1].Trim()));
                            break;

                        default:
                            warnings.Add(string.Format("Unrecognized option: {0}. Valid options are: ‘ciphers’, ‘order’, ‘len’.", kv[0].Trim()));
                            break;
                    }
                }
                missionSettings.Add(set);
            }

            foreach (var warning in warnings)
                Debug.LogWarningFormat("[Cipher Machine #{0}] Warning: {1}", moduleId, warning);
        }

        if (missionSettings.Count == 0)
            return null;

        var rndIx = UnityEngine.Random.Range(0, missionSettings.Count);
        var settings = missionSettings[rndIx];
        missionSettings.RemoveAt(rndIx);
        return settings;
    }

#pragma warning disable 414
    private string TwitchHelpMessage = "!{0} right/left/r/l [move to other screens] | !{0} submit qwerty [submit the decrypted word] | !{0} score [find out TP score]";
#pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        if (command.EqualsIgnoreCase("score"))
        {
            yield return null;
            yield return string.Format("sendtochat Score for this Cipher Machine: {0}", overallTpScoreString);
            yield break;
        }
        if (command.EqualsIgnoreCase("right") || command.EqualsIgnoreCase("r"))
        {
            yield return null;
            rightArrow.OnInteract();
            yield return new WaitForSeconds(0.1f);
            yield break;
        }
        if (command.EqualsIgnoreCase("left") || command.EqualsIgnoreCase("l"))
        {
            yield return null;
            leftArrow.OnInteract();
            yield return new WaitForSeconds(0.1f);
            yield break;
        }

        string[] split = command.ToUpperInvariant().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        if (split.Length != 2 || !split[0].Equals("SUBMIT")) yield break;
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

        yield return string.Format("awardpointsonsolve {0}", overallTpScore);
        submit.OnInteract();
    }
    IEnumerator TwitchHandleForcedSolve()
    {
        if (submitScreen && !answer.StartsWith(screenTexts[6].text))
        {
            KMSelectable[] arrows = new KMSelectable[] { leftArrow, rightArrow };
            arrows[UnityEngine.Random.Range(0, 2)].OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        int start = submitScreen ? screenTexts[6].text.Length : 0;
        for (int i = start; i < answer.Length; i++)
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
    void Update()
    {
        if (moduleSelected)
        {
            for (var ltr = 0; ltr < 26; ltr++)
                if (Input.GetKeyDown(((char) ('a' + ltr)).ToString()))
                    keyboard[getPositionFromChar((char) ('A' + ltr))].OnInteract();
            if (Input.GetKeyDown(KeyCode.Return))
                submit.OnInteract();
        }
    }
}
