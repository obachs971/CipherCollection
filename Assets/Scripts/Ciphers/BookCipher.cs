using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCipher 
{
    public ResultInfo encrypt(string word, string id, string log)
    {
        Debug.LogFormat("{0} Begin Book Cipher", log);
        string key = new string("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().Shuffle()).Substring(0, 2), alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", storedLetters = "", encrypt = "";
        string[] screenText = { "", "", "", "" };
        List<List<string>> storedPositions = new List<List<string>>();
        string[][][] book = getBook(key);
        for (int i = 0; i < word.Length; i++)
        {
            if (storedLetters.IndexOf(word[i]) < 0)
            {
                storedLetters = storedLetters + "" + word[i];
                List<string> tempStorage = new List<string>();
                for (int aa = 0; aa < book.Length; aa++)
                {
                    for (int bb = 0; bb < book[aa].Length; bb++)
                    {
                        for (int cc = 0; cc < book[aa][bb].Length; cc++)
                        {
                            for (int dd = 0; dd < book[aa][bb][cc].Length; dd++)
                            {
                                if (book[aa][bb][cc][dd] == word[i])
                                {
                                    if (cc < 9)
                                        tempStorage.Add(alpha[aa] + "" + alpha[bb] + "0" + (cc + 1) + "" + alpha[dd]);
                                    else
                                        tempStorage.Add(alpha[aa] + "" + alpha[bb] + "" + (cc + 1) + "" + alpha[dd]);
                                }
                            }
                        }
                    }
                }
                storedPositions.Add(tempStorage);
            }
            int index = storedLetters.IndexOf(word[i]);
            string temp = storedPositions[index][UnityEngine.Random.Range(0, storedPositions[index].Count)];
            Debug.LogFormat("{0} [Book Cipher] {1}", log, temp);
            encrypt = encrypt + "" + temp[0];
            for (int j = 1; j < temp.Length; j++)
                screenText[j - 1] = screenText[j - 1] + "" + temp[j];
        }
        Debug.LogFormat("{0} [Book Cipher] {1} - > {2}", log, word, encrypt);
        for (int i = 0; i < screenText.Length; i++)
            Debug.LogFormat("{0} [Book Cipher] Screen {1}: {2}", log, (i + 1), screenText[i]);
        Debug.LogFormat("{0} [Book Cipher] Screen A: {1}", log, key);
        ScreenInfo[] screens = new ScreenInfo[9];
        for (int i = 0; i < 7; i += 2)
            screens[i] = new ScreenInfo(screenText[i / 2], new int[] { 35, 35, 35, 32, 28 }[word.Length - 4]);
        screens[1] = new ScreenInfo(key, 25);
        for (int i = 3; i < 8; i+=2)
            screens[i] = new ScreenInfo();
        screens[8] = new ScreenInfo(id, 35);
        return new ResultInfo
        {
            Encrypted = encrypt,
            Score = 5,
            Pages = new PageInfo[] { new PageInfo(screens) }
        };
    }
    private string[][][] getBook(string key)
    {
        string[][][] diary =
        {
            new string[][]
            {
                new string[]{"FIRST","OF","ALL","I","WANT","TO","GET","SOMETHING","STRAIGHT","THIS","IS","A","JOURNAL","NOT","A","DIARY","I","KNOW","WHAT","IT","SAYS","ON","THE","COVER","BUT","WHEN","MOM","WENT","OUT","TO","BUY","THIS","THING","I","SPECIFICALLY","SAID","TO","MAKE","SURE","IT","DIDNT","SAY","DIARY","ON","IT","SO","DONT","EXPECT","ME","TO","BE","ALL","DEAR","DIARY","THIS","AND","DEAR","DIARY","THAT"},
                new string[]{"ALL","I","NEED","IS","FOR","SOME","JERK","TO","CATCH","ME","CARRYING","THIS","THING","AROUND","AND","GET","THE","WRONG","IDEA"},
                new string[]{"THE","OTHER","THING","I","WANT","TO","CLEAR","UP","RIGHT","AWAY","IS","HOW","THIS","WAS","NOT","MY","IDEA","IT","WAS","MOMS"},
                new string[]{"THE","DEAL","IS","THAT","IF","I","WRITE","IN","THIS","BOOK","A","LITTLE","BIT","EACH","DAY","I","GET","OUT","OF","ONE","CHORE","ON","SATURDAYS","SO","OF","COURSE","I","PICKED","THE","ONE","I","HATE","THE","MOST","BUT","IF","RODRICK","EVER","FINDS","OUT","HES","SCRUBBING","TOILETS","BECAUSE","OF","THIS","BOOK","IM","DEAD"},
                new string[]{"OH","YEAH","RODRICKS","MY","BROTHER","I","TRY","TO","AVOID","HIM","ANYWAY","BUT","NOW","THAT","I","STRUCK","THIS","DEAL","WITH","MOM","I","BETTER","BE","EXTRA","CAREFUL"},
                new string[]{"ANYWAY","I","THINK","MOM","HAS","THIS","IDEA","IM","GOING","TO","WRITE","DOWN","MY","FEELINGS","AND","ALL","THAT","BUT","SHES","NOT","ACTUALLY","ALLOWED","TO","READ","IT","SO","I","FIGURE","ILL","JUST","WRITE","WHAT","I","WANT"},
                new string[]{"THE","REAL","REASON","I","AGREED","TO","DO","THIS","AT","ALL","IS","BECAUSE","I","FIGURE","LATER","ON","WHEN","IM","RICH","AND","FAMOUS","ILL","HAVE","BETTER","THINGS","TO","DO","THAN","ANSWER","PEOPLES","STUPID","QUESTIONS","ALL","DAY","LONG"},
                new string[]{"LIKE","I","SAID","ONE","DAY","I","WILL","DEFINITELY","BE","FAMOUS","BUT","FOR","NOW","IM","STUCK","IN","THE","SEVENTH","GRADE","WITH","A","BUNCH","OF","MORONS"},
                new string[]{"TODAY","IS","THE","FIRST","DAY","OF","SCHOOL","AND","RIGHT","NOW","WERE","JUST","WAITING","AROUND","FOR","THE","TEACHER","TO","HURRY","UP","AND","FINISH","THE","SEATING","CHART"},
                new string[]{"SO","I","FIGURED","I","MIGHT","AS","WELL","WRITE","IN","THIS","BOOK","AND","JUST","GET","IT","OVER","WITH","FOR","THE","DAY"},
                new string[]{"BUT","ILL","TELL","YOU","SOMETHING","ON","THE","FIRST","DAY","OF","SCHOOL","YOU","GOT","TO","BE","REAL","CAREFUL","OF","WHERE","YOU","SIT","YOU","WALK","INTO","THE","CLASSROOM","AND","JUST","PLUNK","YOUR","STUFF","DOWN","ON","ANY","OLD","DESK","AND","THE","NEXT","THING","YOU","KNOW","THE","TEACHER","IS","SAYING","I","HOPE","YOU","ALL","LIKE","WHERE","YOURE","SITTING","BECAUSE","THESE","ARE","YOUR","PERMANENT","SEATS"},
                new string[]{"SO","IN","THIS","CLASS","I","GOT","STUCK","WITH","CHRIS","HOSEY","IN","FRONT","OF","ME","AND","LIONEL","JAMES","IN","BACK","OF","ME","OTHA","HARRIS","CAME","IN","LATE","AND","ALMOST","SAT","NEXT","TO","ME","BUT","LUCKILY","I","DID","SOME","QUICK","THINKING","AND","GOT","MYSELF","OUT","OF","THAT","ONE"}
            },
            new string[][]
            {
                new string[]{"IM","THINKING","FOR","NEXT","PERIOD","I","SHOULD","JUST","SIT","IN","THE","MIDDLE","OF","A","BUNCH","OF","CUTE","GIRLS","AS","SOON","AS","I","STEP","IN","THE","ROOM"},
                new string[]{"THEN","AGAIN","IF","I","DO","THAT","IT","JUST","PROVES","THAT","I","DIDNT","LEARN","A","THING","FROM","LAST","YEAR"},
                new string[]{"PLUS","THE","OTHER","THING","I","GOT","TO","THINK","ABOUT","IS","THAT","GIRLS","DONT","LET","YOU","COPY","OFF","OF","THEM","WHICH","COULD","BE","A","REAL","PROBLEM","IN","A","CLASS","LIKE","PRE","ALGEBRA"},
                new string[]{"SPEAKING","OF","SEATING","SOMETHING","THAT","REALLY","STUNK","TODAY","IS","HOW","IN","HOME","ROOM","I","GOT","STUCK","WITH","SOME","TEACHER","WHO","HAD","RODRICK","IN","HIS","CLASS","A","FEW","YEARS","BACK"},
                new string[]{"THE","ONLY","GOOD","THING","I","CAN","THINK","ABOUT","THE","FIRST","DAY","OF","SCHOOL","IS","THAT","SOME","OF","THE","TEACHERS","ARE","NEW","AND","SO","YOU","CAN","SLIDE","A","LITTLE"},
                new string[]{"ANYWAY","THE","TEACHER","IS","ALMOST","DONE","WITH","THE","SEATING","CHART","AND","I","THINK","I","WROTE","ENOUGH","IN","THIS","BOOK","TO","KEEP","MOM","OFF","MY","BACK","FOR","TODAY"},
                new string[]{"THIS","MORNING","MOM","MADE","ME","LEND","MY","BROTHER","RODRICK","SOME","OF","MY","MONEY","SO","HE","COULD","BUY","LUNCH","WHICH","REALLY","STUNK","IM","STILL","MAD","AT","RODRICK","FOR","THE","TRICK","HE","PULLED","ON","ME","AT","THE","BEGINNING","OF","THE","SUMMER","SO","IM","REALLY","NOT","LOOKING","TO","DO","HIM","ANY","FAVORS"},
                new string[]{"WHAT","HAPPENED","WAS","THAT","ON","THE","FIRST","DAY","OF","SUMMER","VACATION","HE","WOKE","ME","UP","IN","THE","MIDDLE","OF","THE","NIGHT","DRESSED","UP","IN","HIS","SCHOOL","CLOTHES","HE","TOLD","ME","I","SLEPT","THROUGH","THE","WHOLE","SUMMER","BUT","THAT","LUCKILY","I","HAD","WOKEN","UP","IN","TIME","FOR","THE","FIRST","DAY","OF","SCHOOL"},
                new string[]{"YOU","MIGHT","THINK","IM","PRETTY","DUMB","FOR","FALLING","FOR","THAT","ONE","BUT","I","WAS","TOO","GROGGY","TO","KNOW","ANY","BETTER","AND","PLUS","RODRICK","HAD","SET","MY","CLOCK","AHEAD","AND","PULLED","THE","BLINDS","SHUT"},
                new string[]{"SO","I","JUST","GOT","UP","AND","GOT","DRESSED","AND","WENT","DOWNSTAIRS","TO","FIX","MYSELF","SOME","BREAKFAST","I","MUSTVE","MADE","A","BIG","RACKET","BECAUSE","THE","NEXT","THING","I","KNEW","DAD","WAS","IN","MY","FACE","WONDERING","WHAT","THE","HECK","I","WAS","DOING","EATING","CHEERIOS","AT",key[1] + "","AM"},
                new string[]{"THE","THING","ABOUT","DAD","WHEN","HE","COMES","DOWNSTAIRS","LATE","AT","NIGHT","IS","THAT","HES","ALWAYS","JUST","WEARING","A","TEE","SHIRT","AND","SOME","BOXER","SHORTS","I","DONT","KNOW","WHICH","IS","WORSE","GETTING","YELLED","OUT","OR","HAVING","TO","SEE","YOUR","FATHER","IN","HIS","UNDERWEAR"},
                new string[]{"I","KEEP","MEANING","TO","ASK","HIM","TO","PLEASE","PUT","ON","SOME","MORE","CLOTHES","THE","NEXT","TIME","HE","COMES","DOWNSTAIRS","BUT","THE","RIGHT","OPPORTUNITY","NEVER","COMES","UP"},
                new string[]{"ANYWAY","IT","TOOK","ME","A","COUPLE","OF","MINUTES","TO","FIGURE","OUT","WHAT","ALL","WAS","GOING","ON","WHEN","I","TOLD","DAD","RODRICK","TRICKED","ME","DAD","STOMPED","ON","DOWN","TO","RODRICKS","ROOM","IN","THE","BASEMENT","AND","I","FOLLOWED","ALONG"},
                new string[]{"I","WAS","PRETTY","EXCITED","TO","FINALLY","SEE","RODRICK","GET","WHAT","WAS","COMING","TO","HIM"}
            },
            new string[][]
            {
                new string[]{"BUT","WHEN","WE","GOT","DOWN","THERE","RODRICK","HAD","COVERED","UP","HIS","TRACKS","PRETTY","GOOD","AND","YOU","WOULD","NEVER","KNOW","HE","HAD","BEEN","UP","TO","SOMETHING"},
                new string[]{"DAD","JUST","THREW","UP","HIS","HANDS","AND","WENT","BACK","UP","TO","BED","SO","NOW","DAD","THOUGHT","I","WAS","AN","IDIOT","AND","A","LIAR"},
                new string[]{"COME","TO","THINK","OF","IT","EVER","SINCE","DAD","HAS","BEEN","REAL","SUSPICIOUS","AROUND","ME","LIKE","IM","TURNING","INTO","A","BAD","KID","OR","SOMETHING"},
                new string[]{"ILL","PUT","IT","TO","YOU","THIS","WAY","IF","IM","GOING","TO","DO","SOMETHING","BAD","AND","TAKE","THE","HEAT","LIKE","I","DID","THAT","NIGHT","YOU","BETTER","BELIEVE","IM","GOING","TO","COME","UP","WITH","SOMETHING","A","LOT","MORE","SATISFYING","THAN","EATING","A","BOWL","OF","CHEERIOS","IN","THE","MIDDLE","OF","THE","NIGHT"},
                new string[]{"TODAY","IN","SOCIAL","STUDIES","I","SCORED","PRETTY","BIG","THE","TEACHER","MADE","US","SIT","IN","ALPHABETICAL","ORDER","SO","THE","WAY","THINGS","FELL","OUT","I","ENDED","UP","RIGHT","NEXT","TO","ALEX","ARUDA","WHO","IS","THE","SMARTEST","KID","IN","THE","CLASS"},
                new string[]{"HES","SUPER","EASY","TO","COPY","OFF","OF","BECAUSE","HE","ALWAYS","FINISHES","HIS","TESTS","EARLY","AND","THEN","JUST","PUTS","HIS","PAPER","ON","THE","FLOOR","NEXT","TO","HIM","WHILE","HE","READS","A","SCIENCE","FICTION","NOVEL","OR","SOMETHING"},
                new string[]{"KIDS","WHOSE","LAST","NAMES","START","WITH","THE","EARLY","LETTERS","ALWAYS","END","UP","BEING","THE","SMARTEST","BECAUSE","THEY","GET","CALLED","ON","FIRST","SOME","PEOPLE","THINK","THATS","NOT","TRUE","BUT","IF","YOU","WANT","TO","COME","OVER","TO","MY","SCHOOL","I","CAN","PROVE","IT"},
                new string[]{"I","CAN","ONLY","THINK","OF","ONE","KID","WHO","BROKE","THE","LAST","NAME","RULE","AND","THATS","PETER","UTEGER","HE","WAS","THE","SMARTEST","KID","UNTIL","THE","MIDDLE","OF","THE","FIFTH","GRADE","THATS","WHEN","A","COUPLE","OF","US","STARTED","GIVING","HIM","A","HARD","TIME","ABOUT","WHAT","HIS","INITIALS","SPELLED","EVERY","CHANCE","WE","GOT"},
                new string[]{"NOW","HE","DOESNT","RAISE","HIS","HAND","AT","ALL","WHICH","MAKES","FOR","OTHER","KIDS","TO","STEP","FORWARD","AND","TAKE","THE","SMARTEST","KID","TITLE"},
                new string[]{"I","FEEL","A","LITTLE","BAD","ABOUT","THE","WHOLE","PU","THING","BECAUSE","IM","ONE","OF","THE","GUYS","WHO","STARTED","IT","BUT","ITS","HARD","NOT","TO","TAKE","CREDIT","FOR","IT","WHENEVER","IT","COMES","UP"},
                new string[]{"I","FIGURED","OUT","ANOTHER","GOOD","THING","ABOUT","WRITING","THIS","JOURNAL","WHEN","IM","FAMOUS","I","CAN","CASH","IN","ON","IT","I","JUST","HAVE","TO","REMEMBER","TO","KEEP","IT","AWAY","FROM","MANNY","MY","LITTLE","BROTHER"},
                new string[]{"IF","YOU","HAVE","SOMETHING","VALUABLE","IN","THE","HOUSE","BELIEVE","ME","MANNY","WILL","FIND","A","WAY","TO","DESTROY","IT"},
                new string[]{"BACK","BEFORE","MANNY","CAME","ALONG","I","REMEMBER","I","WAS","ALL","EXCITED","ABOUT","GETTING","A","LITTLE","BROTHER","AFTER","ALL","THOSE","YEARS","OF","RODRICK","PICKING","ON","ME","I","FIGURED","IT","WAS","MY","TURN","TO","BE","A","LITTLE","HIGHER","ON","THE","TOTEM","POLE"}
            },
            new string[][]
            {
                new string[]{"BUT","BEING","A","BIG","BROTHER","DIDNT","TURN","OUT","LIKE","I","EXPECTED","AT","ALL","MOM","AND","DAD","PROTECT","MANNY","SO","I","CANT","PICK","ON","HIM","EVEN","IF","HE","DOES","SOMETHING","TO","TICK","ME","OFF"},
                new string[]{"PLUS","HES","NEVER","GOTTEN","PUNISHED","FOR","ANYTHING","AND","BELIEVE","ME","HES","DESERVED","IT","A","BUNCH","OF","TIMES"},
                new string[]{"JUST","THE","OTHER","DAY","HE","SOMEHOW","GOT","INTO","MY","ROOM","AND","USED","A","BUNCH","OF","MAGIC","MARKERS","TO","DECORATE","MY","DOOR","I","THOUGHT","MOM","AND","DAD","WOULD","REALLY","LET","HIM","HAVE","IT","BUT","AS","USUAL","I","WAS","WRONG"},
                new string[]{"SO","NOW","IM","STUCK","WAKING","UP","TO","THIS","HORRIBLE","DRAWING","STARING","AT","ME","EVERYDAY","MOM","WONT","LET","ME","PAINT","OVER","IT","OR","EVEN","COVER","IT","UP","WITH","A","POSTER","BECAUSE","SHE","SAYS","IT","MIGHT","HURT","MANNYS","FEELINGS"},
                new string[]{"THE","ONLY","GOOD","THING","ABOUT","GETTING","A","LITTLE","BROTHER","IS","THAT","NOW","RODRICK","DOESNT","MAKE","ME","SELL","HIS","STUPID","CHOCOLATE","BARS","FOR","SCHOOL","FUNDRAISERS","ANY","MORE"},
                new string[]{"THE","WORST","THING","ABOUT","MANNY","IS","THAT","WHEN","HE","WAS","REAL","LITTLE","HE","COULDNT","PRONOUNCE","BROTHER","SO","HE","STARTED","CALLING","ME","BUBBY","AND","MOM","AND","DAD","DIDNT","MAKE","HIM","CALL","ME","MY","REAL","NAME","WHEN","HE","COULD","SAY","IT"},
                new string[]{"LUCKILY","NONE","OF","MY","FRIENDS","HAVE","FOUND","OUT","YET","BUT","I","HAVE","HAD","SOME","REALLY","CLOSE","CALLS"},
                new string[]{"YESTERDAY","WAS","THE","FIRST","DAY","OF","PE","AND","WE","STARTED","THE","FOOTBALL","UNIT","THE","FIRST","THING","I","DID","WAS","SNEAK","OFF","TO","THE","BASKETBALL","COURTS","AND","CHECK","TO","SEE","IF","THE","CHEESE","WAS","STILL","WHERE","IT","WAS","AT","THE","END","OF","LAST","SCHOOL","YEAR","AND","SURE","ENOUGH","IT","WAS"},
                new string[]{"THAT","THING","HAS","BEEN","SITTING","ON","THE","COURT","SINCE","AT","LEAST","LAST","FALL","AND","IT","HAS","CAUSED","A","WHOLE","LOT","OF","TROUBLE","ITS","ALL","MOLDLY","AND","NASTY","AND","EVER","SINCE","IT","SHOWED","UP","PEOPLE","HAVE","BEEN","TRYING","TO","AVOID","IT"},
                new string[]{"TO","GIVE","YOU","AN","IDEA","OF","HOW","PEOPLE","WILL","GO","OUT","OF","THEIR","WAY","TO","STAY","AWAY","FROM","THE","CHEESE","ITS","SITTING","RIGHT","UNDER","THE","ONLY","HOOP","WITH","A","NET","IN","IT","BUT","NOBODYS","PLAYED","ON","THAT","COURT","FOR","A","YEAR"},
                new string[]{"DARNELL","WASHINGTON","TRIPPED","AND","FELL","AND","BRUSHED","THE","CHEESE","WITH","HIS","FINGER","LAST","YEAR","AND","STARTED","THIS","WHOLE","THING","CALLED","THE","CHEESE","TOUCH","ITS","BASICALLY","LIKE","COOTIES","WHERE","IF","YOU","GET","TOUCHED","WITH","THE","CHEESE","TOUCH","THEN","YOU","HAVE","IT","UNTIL","YOU","PASS","IT","ON","TO","SOMEBODY","ELSE"},
                new string[]{"THE","ONLY","WAY","TO","PROTECT","YOURSELF","FROM","THE","CHEESE","TOUCH","WAS","TO","CROSS","YOUR","FINGERS","BUT","IT","WAS","REALLY","HARD","TO","REMEMBER","TO","KEEP","YOUR","FINGERS","CROSSED","ALL","THE","TIME","ESPECIALLY","WHEN","WHOEVER","HAD","THE","CHEESE","TOUCH","WAS","LOOKING","FOR","HIS","NEXT","VICTIM"}
            },
            new string[][]
            {
                new string[]{"SO","I","TAPED","MY","FINGERS","TOGETHER","FOR","THE","LAST","COUPLE","WEEKS","OF","SCHOOL","I","ENDED","UP","GETTING","A","D","IN","HANDWRITING","BUT","IT","WAS","TOTALLY","WORTH","IT"},
                new string[]{"THIS","ONE","KID","NAMED","ABE","HALL","GOT","THE","CHEESE","TOUCH","IN","APRIL","AND","NOBODY","WOULD","EVEN","SIT","AT","THE","SAME","TABLE","WITH","HIM","AT","LUNCH","FOR","THE","WHOLE","REST","OF","THE","YEAR"},
                new string[]{"THIS","SUMMER","HE","MOVED","AWAY","TO","CALIFORNIA","AND","HE","TOOK","THE","CHEESE","TOUCH","WITH","HIM","NOBODY","HAS","TOUCHED","THE","CHEESE","EVER","SINCE","THEN","NOT","EVEN","WITH","A","STICK"},
                new string[]{"WELL","THE","FIRST","WEEK","OF","SCHOOL","IS","FINALLY","OVER","SO","I","CAN","SLEEP","LATE","AGAIN","MOST","KIDS","SET","THEIR","ALARMS","AND","GET","UP","EARLY","ON","SATURDAY","MORNING","TO","WATCH","CARTOONS","OR","WHATEVER","BUT","NOT","ME","THE","WAY","I","KNOW","ITS","TIME","FOR","ME","TO","CRAWL","OUT","OF","BED","IS","WHEN","I","CANT","STAND","THE","TASTE","OF","MY","BREATH","ANYMORE"},
                new string[]{"UNFORTUNATELY","DAD","WAKES","UP","AT",key[0] + "" + key[0] + "" + key[0], "IN","THE","MORNING","NO","MATTER","WHAT","DAY","IT","IS","AND","HE","IS","NOT","REAL","CONSIDERATE","OF","THE","FACT","THAT","I","AM","TRYING","TO","ENJOY","MY","SATURDAY"},
                new string[]{"OTHER","THAN","THE","SATURDAY","MORNING","VACUUMING","ME","AND","DAD","GET","ALONG","PRETTY","GOOD","BUT","RODRICK","AND","DAD","IS","ANOTHER","STORY","IT","DOESNT","HELP","THAT","RODRICK","IS","A","TEENAGER","WHICH","IS","DADS","LEAST","FAVORITE","TYPE","OF","PERSON"},
                new string[]{"I","THINK","IF","THERE","WAS","A","PETITION","TO","SHIP","ALL","OF","THE","TEENAGERS","IN","THE","STATE","TO","AUSTRALIA","OR","ALCATRAZ","OR","SOMETHING","DAD","WOULD","BE","THE","FIRST","PERSON","TO","SIGN","IT"},
                new string[]{"AND","THE","FIRST","TEENAGER","HED","PUT","ON","THE","BOAT","WOULD","BE","THIS","KID","NAMED","LENWOOD","HEATH","LENWOOD","IS","ALWAYS","TOILET","PAPERING","PEOPLES","HOUSES","AND","GENERALLY","STIRRING","UP","TROUBLE","IN","THE","NEIGHBORHOOD"},
                new string[]{"DAD","HAS","SEEMED","A","LOT","MORE","RELAXED","EVER","SINCE","AUGUST","WHEN","LENWOODS","DAD","SHIPPED","HIM","OFF","TO","SOME","MILITARY","ACADEMY","IN","PENNSYLVANIA"},
                new string[]{"WHILE","IM","ON","THE","SUBJECT","OF","SATURDAY","I","SHOULD","MENTION","SOME","OF","MY","OTHER","GRIPES","FIRST","OF","ALL","THERES","NOTHING","ON","TV","AFTER",key[1] + "" + key[0] + "" + key[0], "PM","EXCEPT","GOLF","AND","BOWLING","SECOND","OF","ALL","THE","SUN","COMES","RIGHT","THROUGH","THE","SLIDING","GLASS","WINDOW","AND","YOU","CAN","HARDLY","SEE","WHATS","ON","THE","TV","ANYWAY","AND","ON","TOP","OF","THAT","YOU","GET","ALL","SWEATY","AND","STICK","TO","THE","COUCH","ITS","PRACTICALLY","LIKE","A","CONSIPRACY","AGAINST","KIDS","TO","MAKE","THEM","GO","OUTSIDE","AND","DO","SOMETHING","BESIDES","WATCH","TV"},
                new string[]{"TODAY","AFTER","DAD","WOKE","ME","UP","I","DECIDED","TO","JUST","SKIP","THE","WHOLE","TV","THING","AND","GO","OVER","TO","ROWLEYS"},
                new string[]{"I","KNOW","I","HAVENT","MENTIONED","ROWLEY","IN","THIS","JOURNAL","YET","EVEN","THOUGH","HES","TECHNICALLY","MY","BEST","FRIEND","BUT","THERES","A","PRETTY","GOOD","REASON","FOR","THAT"},
                new string[]{"ROWLEY","KIND","OF","TICKED","ME","OFF","ON","THE","FIRST","DAY","OF","SCHOOL","WITH","SOMETHING","HE","SAID","AT","THE","END","OF","THE","DAY","WHEN","WE","WERE","GETTING","OUR","STUFF","FROM","OUR","LOCKERS"}
            },
            new string[][]
            {
                new string[]{"I","TOLD","ROWLEY","AT","LEAST","A","BILLION","TIMES","THIS","SUMMER","THAT","NOW","THAT","WERE","IN","MIDDLE","SCHOOL","YOURE","SUPPOSED","TO","SAY","HANG","OUT","NOT","PLAY","BUT","NO","MATTER","HOW","MANY","TIMES","I","KICK","HIM","IN","THE","SHINS","WHEN","HE","SAYS","PLAY","HE","ALWAYS","FORGETS","FOR","THE","NEXT","TIME"},
                new string[]{"SO","I","GUESS","YOU","COULD","SAY","IVE","BEEN","AVOIDING","ROWLEY","THIS","WEEK","IVE","BEEN","TRYING","TO","BE","MORE","CAREFUL","ABOUT","MY","IMAGE","EVER","SINCE","THIS","SUMMER","WHEN","WE","GOT","CAUGHT","PLAYING","VIKINGS","AND","INDIANS","IN","THE","WOODS","BY","A","COUPLE","OF","EIGHTH","GRADERS"},
                new string[]{"WHAT","REALLY","BURNED","ME","UP","ABOUT","THAT","WHOLE","INCIDENT","IS","HOW","THAT","GUY","CALLED","ME","A","NERD","NOW","ILL","ADMIT","IM","NOT","EXACTLY","THE","MOST","MACHO","GUY","AROUND","IN","TERMS","OF","WANTING","TO","DO","PUSHUPS","ALL","THE","TIME","OR","WHATEVER","SO","IF","YOU","WANT","TO","CALL","ME","A","WIMP","THEN","FINE","BUT","I","KNOW","ONE","THING","FOR","SURE","AND","ITS","THAT","I","AM","NOT","A","NERD"},
                new string[]{"THE","TROUBLE","WITH","NERDS","IS","THAT","THEY","GIVE","WIMPY","KIDS","LIKE","ME","A","BAD","NAME","BECAUSE","PEOPLE","END","UP","LUMPING","US","ALL","IN","THE","SAME","CATEGORY","WHEN","I","THINK","OF","NERDS","I","THINK","OF","TEACHERS","PETS","AND","TICKLE","FIGHTS","AND","HALL","MONITORS","AND","THAT","IS","NOT","ME"},
                new string[]{"NOW","ROWLEY","CAN","SPEAK","FOR","HIMSELF","ON","THE","WHOLE","NERD","THING","BUT","I","WILL","JUST","MENTION","AS","A","SIDE","NOTE","THAT","HE","IS","THE","ONLY",key[1] + "" + key[0] + "YEAROLD","I","KNOW","WHO","STILL","HAS","A","BABYSITTER"},
                new string[]{"ROWLEY","MOVED","HERE","A","COUPLE","YEARS","AGO","AND","I","KIND","OF","TOOK","HIM","UNDER","MY","WING","MY","FORMER","BEST","FRIEND","BEN","MOVED","TO","PISCATAWAY","AND","I","FIGURED","ID","BETTER","FIND","MYSELF","A","NEW","FRIEND","TO","HANG","OUT","WITH","SO","HERE","COMES","ROWLEY","STRAIGHT","OUT","OF","OHIO","HIS","MOM","BOUGHT","HIM","SOME","BOOK","CALLED","HOW","TO","MAKE","FRIENDS","IN","NEW","PLACES","AND","HE","SHOWED","UP","AT","MY","DOOR","TRYING","OUT","ALL","THESE","GIMMICKS"},
                new string[]{"ALL","THAT","KID","WOULDVE","HAD","TO","HAVE","DONE","IS","TO","HAVE","COME","RIGHT","OUT","AND","TOLD","ME","HE","HAD","A","PLAYSTATION","WITH",key[0] + "" + key[0], "GAMES","AND","IT","WOULD","HAVE","SEALED","THE","DEAL"},
                new string[]{"THE","BEST","THING","ABOUT","HAVING","ROWLEY","AROUND","IS","THAT","I","GET","A","CHANCE","TO","USE","ALL","THE","TRICKS","RODRICK","USED","ON","ME","THAT","I","COULD","NEVER","GET","AWAY","WITH","PULLING","ON","MANNY"},
                new string[]{"ANOTHER","BONUS","ABOUT","ROWLEY","IS","THAT","HE","HAS","NEVER","SQUEALED","ON","ME","NOT","EVEN","ONCE","SO","IN","SOME","WAYS","I","GUESS","YOU","COULD","SAY","HES","THE","PERFECT","FRIEND"},
                new string[]{"TODAY","WAS","A","REALLY","BAD","DAY","DAD","ENDED","UP","RUNNING","INTO","MR","SWANN","AT","CHURCH","AND","MR","SWANN","WAS","TELLING","DAD","HOW","GREAT","BISHOP","GARRIGAN","HIGH","SCHOOL","IS","WHERE","HIS","SON","DAN","GOES"},
                new string[]{"DAD","SEEMED","REAL","INTERESTED","WHICH","IS","A","VERY","BAD","SIGN","FOR","ME","NOW","IM","SURE","BISHOP","GARRIGAN","IS","A","FINE","SCHOOL","AND","ALL","THAT","EXCEPT","FOR","THE","FACT","THAT","IT","IS","ALL","BOYS","NUMBER","ONE","I","WANT","TO","GO","TO","CROSSLAND","HIGH","SCHOOL","WHERE","THERE","ARE","BOYS","AND","GIRLS","AND","NUMBER","TWO","I","WOULDNT","SURVIVE","THE","FIRST","DAY","AT","BISHOP","GARRIGAN"}
            },
            new string[][]
            {
                new string[]{"RODRICK","DOESNT","HAVE","TO","WORRY","ABOUT","GETTING","SENT","TO","BISHOP","GARRIGAN","BECAUSE","HE","IS","ALREADY","A","JUNIOR","AT","CROSSLAND","BUT","I","HAD","DEFINITELY","BETTER","FIGURE","A","WAY","OUT","OF","THIS"},
                new string[]{"MR","SWANN","WENT","ON","AND","ON","ABOUT","HOW","BISHOP","GARRIGAN","MAKES","MEN","OUT","OF","BOYS","AND","FROM","THE","WAY","DAD","KEPT","LOOKING","OVER","AT","ME","I","KNEW","I","WAS","IN","TROUBLE","IT","DOESNT","HELP","THAT","MR","SWANN","HAS","THREE","BOYS","WHO","ARE","THE","SAME","AGES","AS","US","HEFFLEY","BOYS","AND","DADS","CARPOOL","PASSES","BY","THEIR","HOUSE","EVERY","NIGHT"},
                new string[]{"AS","FAR","AS","THE","WHOLE","MAKING","MEN","OUT","OF","BOYS","IDEA","GOES","I","THINK","THE","SWANN","BOYS","HAVE","A","PRETTY","GOOD","HEAD","START"},
                new string[]{"IVE","STILL","GOT","TWO","YEARS","BEFORE","I","GO","TO","HIGH","SCHOOL","AND","HOPEFULLY","DAD","WILL","FORGET","ABOUT","BISHOP","GARRIGAN","BY","THEN"},
                new string[]{"BUT","IF","THINGS","LOOK","BAD","DOWN","THE","ROAD","I","BETTER","START","WORKING","ON","MOM","TO","CHANGE","MY","FATE"},
                new string[]{"TODAY","I","WOKE","UP","AND","AT","FIRST","I","THOUGHT","IT","WAS","STILL","SUMMER","VACATION","WHICH","IS","A","REALLY","BAD","WAY","TO","START","A","SCHOOL","DAY"},
                new string[]{"THE","NEW","THING","IS","THAT","I","HAVE","TO","FIX","MANNY","HIS","CEREAL","EVERY","MORNING","WHILE","MOM","GETS","READY","FOR","WORK","MANNY","TAKES","HIS","BOWL","AND","SITS","RIGHT","IN","FRONT","OF","THE","TV","ON","HIS","PLASTIC","POTTY"},
                new string[]{"ITS","NOT","LIKE","HES","NOT","POTTY","TRAINED","BUT","HE","GOT","IN","THE","HABIT","OF","DOING","THIS","WHEN","HE","WAS","TWO","AND","HE","JUST","NEVER","QUIT"},
                new string[]{"THE","WORST","PART","IS","THAT","AFTER","HES","DONE","HE","DUMPS","WHATEVER","HE","DIDNT","EAT","RIGHT","INTO","THE","POTTY","AND","ITS","ALWAYS","ME","WHO","HAS","TO","CLEAN","IT","UP"},
                new string[]{"MOM","ALWAYS","GETS","ON","ME","ABOUT","NOT","FINISHING","MY","BREAKFAST","BUT","IF","YOU","HAD","TO","SCRAPE","A","BUNCH","OF","CHEERIOS","OUT","OF","A","POTTY","EVERY","MORNING","I","BET","YOU","WOULDNT","HAVE","ANY","APPETITE","EITHER"},
                new string[]{"TODAY","AT","SCHOOL","WE","GOT","ASSIGNED","TO","READING","GROUPS","I","WAS","LOOKING","FORWARD","TO","FINDING","OUT","WHICH","GROUP","I","WAS","GOING","TO","GET","PUT","INTO","BECAUSE","I","WANTED","TO","SEE","IF","A","BIG","PLAN","I","HATCHED","AT","THE","END","OF","LAST","YEAR","WAS","GOING","TO","WORK"},
                new string[]{"NOW","THEY","DONT","COME","RIGHT","OUT","AND","TELL","YOU","IF","YOURE","IN","THE","HARD","GROUP","OR","THE","EASY","GROUP","BUT","YOU","CAN","FIGURE","IT","OUT","RIGHT","AWAY","BY","LOOKING","AT","THE","COVERS","OF","THE","BOOKS","THEY","GIVE","YOU"},
                new string[]{"I","WAS","PRETTY","MAD","TO","FIND","OUT","I","GOT","PUT","IN","THE","HARD","GROUP","TODAY","WHICH","MEANT","MY","PLAN","FAILED","I","WAS","HOPING","TO","GET","IN","THE","EASY","GROUP","BECAUSE","THEY","ONLY","HAVE","TO","READ","ABOUT","A","TENTH","OF","THE","STUFF","THAT","THE","KIDS","IN","THE","HARD","GROUP","HAVE","TO","READ","AND","THERES","A","WHOLE","LOT","LESS","HOMEWORK"}
            },
            new string[][]
            {
                new string[]{"AT","THE","END","OF","LAST","YEAR","I","DID","MY","BEST","TO","MUFF","UP","MY","SCREENING","TEST","TO","MAKE","SURE","I","DIDNT","GET","PUT","IN","THE","HARD","GROUP"},
                new string[]{"ANOTHER","THING","I","DID","TO","MAKE","SURE","I","DIDNT","GET","PUT","IN","THE","HARD","GROUP","WAS","TO","MAKE","SURE","I","DIDNT","TRY","TO","HARD","ON","MY","ENDOFTHEYEAR","ESSAY"},
                new string[]{"THEY","MAKE","YOU","DO","THIS","FOUR","PAGE","PAPER","AT","THE","END","OF","THE","YEAR","WHICH","IS","ANOTHER","WAY","THEY","FIGURE","OUT","HOW","TO","PLACE","YOU"},
                new string[]{"IM","GUESSING","MOM","STEPPED","IN","AND","MADE","SURE","I","GOT","PUT","IN","THE","HARD","CLASSES","BECAUSE","SHE","KNOWS","THE","PRINCIPAL","OF","THE","SCHOOL"},
                new string[]{"MOMS","ALWAYS","SAYING","HOW","IM","A","REAL","SMART","KID","BUT","I","JUST","DONT","APPLY","MYSELF"},
                new string[]{"YOU","MIGHT","WONDER","WHY","ID","WANT","TO","GET","PUT","IN","THE","EASY","CLASSES","SINCE","I","PROBABLY","DESERVE","TO","BE","IN","THE","HARD","CLASSES","BUT","I","HAVE","A","PRETTY","GOOD","ANSWER","FOR","THAT"},
                new string[]{"IF","THERES","ONE","THING","I","LEARNED","FROM","RODRICK","ITS","TO","SET","PEOPLES","EXPECTATIONS","REAL","LOW","SO","YOU","END","UP","SURPRISING","EVERYONE","BY","DOING","ALMOST","NOTHING","AT","ALL"},
                new string[]{"IN","FACT","HE","DID","SOMETHING","ON","FRIDAY","THAT","TOTALLY","PROVES","MY","POINT"},
                new string[]{"ANYWAY","I","GUESS","IM","GLAD","MY","PLAN","DIDNT","WORK","BECAUSE","I","NOTICED","AT","LEAST","TWO","OF","THE","KIDS","IN","THE","BINK","SAYS","BOO","GROUP","WERE","HOLDING","THEIR","BOOKS","UPSIDE","DOWN","AND","I","DONT","THINK","THEY","WERE","JOKING","AROUND"},
                new string[]{"TODAY","AT","LUNCH","I","GOT","TO","LISTEN","TO","ALBERT","SANDY","BRAG","ABOUT","HOW","HIS","PARENTS","BOUGHT","HIM","A",key[1] + "" + key[0], "INCH","TV","AND","A","DVD","PLAYER","AND","A","BUNCH","OF","OTHER","STUFF","FOR","HIS","BEDROOM"},
                new string[]{"IT","REALLY","MAKES","ME","MAD","BECAUSE","MY","WHOLE","GOAL","THIS","SUMMER","WAS","TO","SAVE","UP","FOR","A","TV","SO","I","DIDNT","HAVE","TO","HANG","OUT","WITH","THE","REST","OF","MY","FAMILY","AND","WATCH","WHAT","THEY","WANT","TO","WATCH","ALL","THE","TIME"},
                new string[]{"SO","I","SPENT","THE","WHOLE","SUMMER","TAKING","CARE","OF","MR","AND","MRS","ROSES","DOG","WHILE","THEY","WERE","ON","A","TRIP"},
                new string[]{"THE","DEAL","WAS","THAT","I","HAD","TO","GO","OVER","TO","THEIR","HOUSE","TWICE","A","DAY","TO","TAKE","THEIR","DOG","STEVIE","OUT","AND","I","WAS","SUPPOSED","TO","GET","THREE","BUCKS","A","DAY","FOR","DOING","IT"},
                new string[]{"THE","BIG","PROBLEM","WITH","STEVIE","IS","THAT","I","GUESS","HE","IS","TOTALLY","SHY","WHEN","IT","COMES","TO","GOING","TO","THE","BATHROOM","IN","FRONT","OF","STRANGERS","SO","I","WASTED","A","WHOLE","LOT","OF","MY","SUMMER","STANDING","THERE","WAITING","FOR","THIS","STUPID","DOG","TO","HURRY","UP","AND","GO"},
                new string[]{"SO","ID","WAIT","AND","WAIT","AND","NOTHING","WOULD","HAPPEN","SO","ID","JUST","GO","HOME","BUT","EVERY","TIME","I","CAME","BACK","TO","THE","HOUSE","LATER","ON","STEVIE","HAD","MADE","A","MESS","IN","THE","KITCHEN","I","FINALLY","FIGURED","OUT","STEVIE","WAS","JUST","HOLDING","IT","UNTIL","THE","COAST","WAS","CLEAR"}
            },
            new string[][]
            {
                new string[]{"IN","FACT","ONE","DAY","I","TRIED","AN","EXPERIMENT","WHERE","I","LEFT","AND","THEN","CAME","BACK","FIVE","MINUTES","LATER","AND","SURE","ENOUGH","STEVIE","HAD","POOPED","RIGHT","ON","THE","KITCHEN","FLOOR"},
                new string[]{"AND","ITS","NOT","LIKE","I","DIDNT","GIVE","THE","DOG","A","CHANCE","TO","GO","THE","ROSES","HAVE","SATELLITE","TV","AND","TONS","OF","JUNK","FOOD","SO","I","BASICALLY","SPENT","THREE","HOURS","A","DAY","ON","MR","ROSES","LAZBOY","WITH","THE","AIR","CONDITIONER","ON","FULL","BLAST"},
                new string[]{"SO","THIS","ONE","DAY","I","FINALLY","FIGURED","OUT","IT","WAS","A","BIG","HASSLE","TO","CLEAN","UP","THIS","DOGS","MESS","EVERY","SINGLE","DAY","SO","I","DECIDED","TO","JUST","SAVE","MYSELF","SOME","TIME","AND","CLEAN","IT","UP","ALL","AT","ONCE"},
                new string[]{"I","LET","THINGS","GO","FOR","ABOUT","A","WEEK","THEN","THE","NIGHT","BEFORE","THE","ROSES","WERE","SUPPOSED","TO","GET","BACK","I","HEADED","UP","THE","HILL","WITH","ALL","MY","CLEANING","STUFF"},
                new string[]{"AND","WOULDNT","YOU","KNOW","IT","THE","ROSES","CAME","HOME","A","DAY","EARLY"},
                new string[]{"TO","MAKE","A","LONG","STORY","SHORT","I","DIDNT","GET","PAID","A","SINGLE","CENT","NOT","EVEN","FOR","THE","DAYS","I","DID","MY","JOB","LIKE","I","WAS","SUPPOSED","TO","SO","HEARING","ALBERT","SANDY","BRAGGING","ABOUT","HIS","TV","JUST","REMINDED","ME","HOW","I","GOT","STIFFED","AND","PUT","ME","IN","A","BAD","MOOD","FOR","THE","REST","OF","THE","DAY"},
                new string[]{"YOU","KNOW","HOW","I","SAID","MOM","IS","ALWAYS","TAKING","MANNYS","SIDE","WELL","TODAY","WAS","SOME","MORE","PROOF","OF","THAT"},
                new string[]{"I","MADE","MANNY","HIS","CEREAL","LIKE","I","ALWAYS","DO","BUT","THIS","TIME","I","ACCIDENTALLY","POURED","THE","MILK","IN","BEFORE","THE","CEREAL","AND","WHEN","I","POURED","THE","CEREAL","IN","ON","TOP","OF","THAT","MANNY","JUST","ABOUT","LOST","HIS","MIND","HE","MADE","A","HUGE","RACKET","CRYING","AND","HOLLERING","AND","ALL","THAT","AND","MOM","CAME","DOWN","TO","SEE","WHAT","WAS","GOING","ON","SO","I","TOLD","HER","WHAT","HAPPENED","AND","I","FIGURED","SHE","WOULD","JUST","TELL","MANNY","TO","PIPE","DOWN","AND","EAT","HIS","STUPID","CEREAL"},
                new string[]{"BUT","INSTEAD","SHE","SAYS","I","WOULDNT","EAT","IT","EITHER","AND","THEN","SHE","GIVES","MANNY","A","BIG","HUG","AND","MAKES","ME","POUR","HIM","A","NEW","BOWL","OF","CEREAL","THIS","TIME","IN","THE","RIGHT","ORDER"},
                new string[]{"I","GUESS","I","SHOULDVE","EXPECTED","IT","A","COUPLE","WEEKS","AGO","WHEN","MANNY","WAS","AT","DAYCARE","HE","OPENED","UP","HIS","LUNCHBOX","AND","WHEN","HE","TOOK","OUT","HIS","SANDWHICH","HE","HAD","A","FIT","BECAUSE","IT","WAS","CUT","IN","TWO","HALVES","NOT","FOUR","SQUARES","LIKE","HE","USUALLY","GETS","IT","SO","THE","DAYCARE","PEOPLE","HAD","TO","CALL","MOM","UP","AT","WORK","TO","GET","HER","TO","COME","OVER","SO","MANNY","WOULD","CALM","DOWN"},
                new string[]{"TODAY","I","GOT","TOTALLY","DISSED","BY","CHRISTINE","COOLIDGE","I","ASKED","HER","IF","SHE","WOULD","BE","MY","LAB","PARTNER","FOR","SCIENCE","AND","SHE","TOLD","ME","SHE","ALREADY","HAD","A","PARTNER"},
                new string[]{"BUT","THEN","LATER","ON","IN","THE","CLASS","I","SAW","HER","WALK","UP","TO","BRYCE","ANDERSON","AND","ASK","HIM","IF","HE","WOULD","BE","HER","LAB","PARTNER","IM","NOT","SURPRISED","SHE","WENT","AFTER","BRYCE","BECAUSE","HE","IS","THE","MOST","POPULAR","KID","IN","OUR","GRADE","FOR","SOMETHING","LIKE","THREE","YEARS","RUNNING","BUT","SHE","DIDNT","HAVE","TO","GO","AND","LIE","ABOUT","IT","EITHER"}
            },
            new string[][]
            {
                new string[]{"IT","USED","TO","BE","A","LOT","MORE","SIMPLE","WITH","GIRLS","WAY","BACK","IN","THE","THIRD","GRADE","THE","DEAL","WAS","IF","YOU","WERE","THE","FASTEST","RUNNER","IN","YOUR","GRADE","YOU","WERE","THE","KING","AND","IN","MY","GRADE","THAT","WAS","RONNIE","JONES"},
                new string[]{"NOWADAYS","ITS","A","LOT","MORE","COMPLICATED","AND","IM","SURE","KIDS","LIKE","RONNIE","JONES","ARE","SITTING","AROUND","SCRATCHING","THEIR","HEADS","WONDERING","WHAT","HAPPENED"},
                new string[]{"NOT","ONLY","DOES","BRYCE","ANDERSON","GET","ALL","THE","GIRLS","BUT","HE","ALSO","HAS","A","BIG","GROUP","OF","CRONIES","THAT","FOLLOWS","HIM","AROUND","AND","BASICALLY","WORSHIPS","EVERY","WORD","THAT","COMES","OUT","OF","HIS","MOUTH"},
                new string[]{"THE","THING","THAT","REALLY","STINKS","IS","THAT","I","HAVE","ALWAYS","BEEN","INTO","GIRLS","BUT","KIDS","LIKE","BRYCE","HAVE","ONLY","COME","AROUND","TO","LIKING","GIRLS","IN","THE","PAST","COUPLE","OF","YEARS","I","REMEMBER","HOW","BRYCE","USED","TO","BE","BACK","IN","FOURTH","AND","FIFTH","GRADE"},
                new string[]{"AND","OF","COURSE","NOW","I","DONT","GET","ANY","CREDIT","FROM","THE","GIRLS","FOR","STICKING","WITH","THEM","FOR","ALL","THIS","TIME"},
                new string[]{"LIKE","I","SAID","BRYCE","IS","THE","MOST","POPULAR","KID","IN","THE","SEVENTH","GRADE","SO","THAT","LEAVES","ALL","THE","REST","OF","US","SCRAMBLING","TO","MOVE","UP","THE","LADDER"},
                new string[]{"THE","BEST","WAY","TO","FIGURE","OUT","HOW","POPULAR","YOU","ARE","IS","TO","GET","A","HOLD","OF","ONE","OF","THE","SLAM","BOOKS","THAT","GETS","PASSED","AROUND","BASICALLY","THEYRE","NOTEBOOKS","WHERE","PEOPLE","PUT","RANKINGS","DOWN","FOR","MOST","POPULAR","BOY","MOST","POPULAR","GIRL","BEST","HAIR","CUTEST","BUTT","AND","ALL","OF","THAT"},
                new string[]{"THE","PROBLEM","WITH","THESE","BOOKS","THOUGH","IS","THAT","THEYRE","IN","REGULAR","NOTEBOOKS","WHICH","ONLY","HAVE",key[0] + "" + key[0], "LINES","ON","EACH","PAGE","SO","PEOPLE","LIKE","ME","WHO","DONT","MAKE","THE","TOP",key[0] + "" + key[0], "HAVE","TO","GUESS","WHERE","THEY","RANK","THE","BEST","I","CAN","FIGURE","RIGHT","NOW","IS","THAT","IM","SOMEWHERE","AROUND",key[1] + "" + key[0] + "ND","OR",key[1] + "" + key[1] + "RD","MOST","POPULAR","BUT","I","THINK","IM","ABOUT","TO","MOVE","UP","A","RANK","BECAUSE","CHARLIE","DAVIES","(WHO","IS","A","REALLY","NICE","KID)","GETS","HIS","BRACES","ON","TUESDAY"},
                new string[]{"WHAT","SOMEONE","NEEDS","TO","DO","IS","START","UP","A","SLAM","BOOK","WITH","ONE","OF","THOSE","YELLOW","LEGAL","PADS","OF","PAPER","BECAUSE","THEY","HAVE","SOMETHING","LIKE",key[1] + "" + key[0], "LINES","IN","THEM","SO","AT","LEAST","KIDS","LIKE","ME","COULD","GET","A","BETTER","PICTURE","OF","WHERE","THEY","STAND","BUT","THE","PROBLEM","IS","THAT","IT","TAKES","A","POPULAR","KID","TO","START","A","SLAM","BOOK","AND","IM","SURE","IF","I","STARTED","ONE","IT","WOULD","GET","FILLED","OUT","BY","ALL","NERDS"},
                new string[]{"TODAY","I","GOT","A","HOLD","OF","A","SLAM","BOOK","AND","I","WAS","TRYING","TO","EXPLAIN","ALL","THIS","TO","ROWLEY","ON","THE","BUS","RIDE","HOME","BUT","HONESTLY","SOMETIMES","WITH","HIM","I","FEEL","LIKE","ITS","JUST","IN","ONE","EAR","AND","OUT","THE","OTHER"},
                new string[]{"TONIGHT","DAD","WAS","SHOWING","ME","THE","NEWEST","STUFF","HE","ADDED","TO","HIS","CIVIL","WAR","DIORAMA","IN","THE","BASEMENT","AND","I","GOT","TO","ADMIT","ITS","PRETTY","COOL"},
                new string[]{"DADS","NOT","LIKE","THE","REGULAR","KINDS","OF","DADS","YOU","SEE","ON","TV","SITTING","AROUND","WATCHING","FOOTBALL","AND","DRINKING","BEER","AND","ALL","OF","THAT"}
            },
            new string[][]
            {
                new string[]{"ANY","FREE","SECOND","HE","GETS","YOU","CAN","BE","SURE","HES","DOWN","IN","HIS","WORKROOM","PAINTING","HIS","LITTLE","SOLDIERS","OR","MOVING","STUFF","AROUND","THE","BATTLEFIELD","TRYING","TO","MAKE","IT","AS","ACCURATE","AS","POSSIBLE"},
                new string[]{"DAD","WOULD","BE","HAPPY","TO","SPEND","THE","WHOLE","WEEKEND","WORKING","ON","HIS","DIORAMA","BUT","MOM","USUALLY","HAS","OTHER","IDEAS","MOM","LIKES","TO","RENT","THESE","ROMANTIC","COMEDIES","AND","DAD","HAS","TO","WATCH","THEM","WITH","HER","WHETHER","HE","WANTS","TO","OR","NOT"},
                new string[]{"A","COUPLE","WEEKS","AGO","WHEN","MOM","RENTED","ONE","OF","THESE","MOVIES","DAD","TRIED","TO","GET","CLEVER","AND","FAKE","OUT","MOM","WHEN","SHE","GOT","UP","TO","GO","TO","THE","BATHROOM","DAD","STUFFED","A","BUNCH","OF","PILLOWS","UNDER","THE","BLANKETS","TO","MAKE","IT","SEEM","LIKE","HE","HAD","FALLEN","ASLEEP"},
                new string[]{"SO","MOM","WATCHED","THE","REST","OF","THE","MOVIE","AND","DIDNT","CATCH","ON","THAT","DAD","HAD","MADE","A","DECOY","UNTIL","THE","MOVIE","WAS","OVER"},
                new string[]{"DAD","WAS","IN","THE","DOG","HOUSE","FOR","A","LONG","TIME","AFTER","THAT","ONE"},
                new string[]{"ANOTHER","THING","I","SHOULD","MENTION","ABOUT","DADS","WORKROOM","IS","THAT","HE","IS","REAL","PROTECTIVE","OF","IT","HE","KEEPS","THE","DOOR","BOLTED","SHUT","WITH","ONE","OF","THOSE","COMBINATION","LOCKS","SO","I","HARDLY","EVER","EVEN","GET","TO","STEP","FOOT","IN","THERE"},
                new string[]{"I","DONT","EVEN","THINK","MANNY","KNOWS","THE","DIORAMA","EXISTS","IVE","SEEN","DAD","SAY","SOME","THINGS","TO","MANNY","TO","MAKE","SURE","MANNY","KEEPS","CLEAR","OF","THAT","PART","OF","THE","BASEMENT"},
                new string[]{"ROWLEY","CAME","OVER","TONIGHT","AND","DAD","GETS","REAL","EDGY","WHEN","ROWLEYS","AROUND","FOR","SOME","REASON","DAD","HAS","IT","IN","HIS","HEAD","THAT","ROWLEY","IS","A","KLUTZ","AND","THAT","HES","GOING","TO","BREAK","SOMETHING","EVERY","TIME","HE","COMES","OVER"},
                new string[]{"DAD","TOLD","ME","ABOUT","THIS","NIGHTMARE","HE","ALWAYS","HAS","ABOUT","ROWLEY","RUINING","HIS","BATTLEFIELD","IN","ONE","KLUTZY","MOVE"},
                new string[]{"SO","EVERY","TIME","ROWLEY","COMES","OVER","HE","GETS","THE","SAME","GREETING","THE","BASEMENT","IS","OFFLIMITS"},
                new string[]{"TODAY","I","GOT","OUT","OF","BED","EARLY","BECAUSE","THE","VACUUMING","THING","WAS","MORE","THAN","I","COULD","HANDLE","I","WATCHED","CARTOONS","FOR","A","WHILE","BUT","THEN","I","REMEMBERED","THE","OTHER","REASON","I","DONT","GET","UP","EARLY","ON","SATURDAYS","WITH","DAD","DOING","HIS","CHORES","ALL","AROUND","YOU","IT","MAKES","YOU","FEEL","GUILTY","AND","TAKES","ALL","THE","JOY","OUT","OF","JUST","LAYING","AROUND","DOING","NOTHING"},
                new string[]{"TONIGHT","IM","GOING","TO","SPEND","THE","NIGHT","AT","ROWLEYS","ITS","A","PRETTY","BIG","DEAL","BECAUSE","I","HAVENT","SPENT","THE","NIGHT","OVER","THERE","IN","SOMETHING","LIKE","A","YEAR","AND","A","HALF"},
                new string[]{"THE","MAIN","REASON","I","HAVENT","SLEPT","OVER","THERE","IN","SO","LONG","IS","BECAUSE","ROWLEYS","DAD","REALLY","DOESNT","LIKE","ME"}
            },
            new string[][]
            {
                new string[]{"IT","ALL","GOES","BACK","TO","SOMETHING","THAT","HAPPENED","LAST","JUNE","WE","WERE","WATCHING","SOME","CORNY","MOVIE","ROWLEY","HAD","WHERE","THERE","WERE","THESE","KIDS","THAT","TAUGHT","THEMSELVES","A","SECRET","LANGUAGE","THAT","ONLY","THEY","COULD","UNDERSTAND"},
                new string[]{"ME","AND","ROWLEY","THOUGHT","THE","WHOLE","SECRET","LANGUAGE","THING","WAS","PRETTY","COOL","BUT","WE","COULDNT","FIGURE","OUT","HOW","TO","DO","IT","LIKE","THE","KIDS","IN","THE","MOVIE"},
                new string[]{"SO","WE","DECIDED","TO","MAKE","UP","OUR","OWN","SECRET","LANGUAGE","AND","WE","TRIED","IT","OUT","OVER","DINNER"},
                new string[]{"WELL","ROWLEYS","DAD","MUST","HAVE","CRACKED","OUR","CODE","BECAUSE","THE","NEXT","THING","I","KNEW","I","WAS","GOING","HOME","EARLY"},
                new string[]{"AND","THAT","WAS","THE","LAST","TIME","I","WAS","INVITED","FOR","A","SLEEPOVER"},
                new string[]{"SO","I","DONT","KNOW","WHY","MR","JEFFERSON","IS","GIVING","ME","A","SECOND","CHANCE","EITHER","HES","GOTTEN","OVER","THE","WHOLE","SECRET","LANGUAGE","THING","OR","HES","FORGOTTEN","WHY","HE","DOESNT","LIKE","ME"},
                new string[]{"THE","SLEEPOVER","AT","ROWLEYS","LAST","NIGHT","ENDED","UP","BEING","A","NIGHTMARE","THE","FIRST","HINT","I","HAD","THAT","THINGS","WERE","GOING","TO","GO","WRONG","WAS","WHEN","ROWLEYS","MOM","TOLD","US","THATS","ENOUGH","TV","FOR","THE","NIGHT","AT",key[1] + "" + key[0] + "" + key[0]},
                new string[]{"I","WAS","LIKE","WELL","WHAT","ARE","WE","SUPPOSED","TO","DO","NOW","AND","SHE","SAID","YOU","COULD","READ","A","BOOK"},
                new string[]{"SO","OF","COURSE","I","THOUGHT","SHE","WAS","JOKING","BUT","RIGHT","WHEN","I","WAS","TELLING","ROWLEY","HOW","I","THOUGHT","HIS","MOM","WAS","PRETTY","FUNNY","SHE","SHOWED","UP","AGAIN","WITH","HER","ARMS","FULL","OF","BOOKS"},
                new string[]{"I","REALIZED","RIGHT","THEN","I","WAS","IN","FOR","A","PRETTY","LONG","NIGHT","SINCE","THE","TV","WAS","OFF","LIMITS","VIDEO","GAMES","WERE","OUT","TOO","SO","I","TRIED","TO","THINK","UP","WAYS","WE","COULD","KEEP","OURSELVES","ENTERTAINED","I","BROKE","OUT","SOME","BOARD","GAMES","BUT","ROWLEY","HAD","TO","TAKE","A","BATHROOM","BREAK","SOMETHING","LIKE","EVERY","FIVE","MINUTES","SO","IT","MADE","OUR","GAME","OF","RISK","GO","ON","FOREVER"},
                new string[]{"EVERY","TIME","ROWLEY","CAME","BACK","FROM","A","BATHROOM","BREAK","HE","WOULD","RUN","DOWN","STAIRS","AND","KICK","THIS","GIANT","SOMBRERO","ACROSS","THE","ROOM"},
                new string[]{"IT","WAS","FUNNY","THE","FIRST","TEN","TIMES","OR","SO","BUT","AFTER","A","WHILE","IT","REALLY","STARTED","GETTING","ON","MY","NERVES","SO","THIS","ONE","TIME","WHEN","HE","WAS","UPSTAIRS","I","PUT","ONE","OF","HIS","DADS","DUMBELLS","UNDER","THE","HAT","TO","SEE","IF","HE","WOULD","STILL","KICK","IT"},
                new string[]{"AND","SURE","ENOUGH","ROWLEY","COMES","RUNNING","DOWN","THE","STAIRS","AND","GIVES","THE","HAT","A","BIG","KICK"},
                new string[]{"ROWLEYS","DAD","WAS","DOWN","THE","STAIRS","IN","NO","TIME","FLAT","I","DONT","THINK","ROWLEY","KNEW","I","PUT","THE","DUMBELL","UNDER","THE","HAT","BUT","ROWLEYS","DAD","SEEMED","PRETTY","SUSPICIOUS"}
            },
            new string[][]
            {
                new string[]{"ANYWAY","I","GUESS","HE","DIDNT","HAVE","ENOUGH","HARD","EVIDENCE","OR","HE","WOULD","HAVE","SENT","ME","HOME","RIGHT","THEN","I","FELT","A","LITTLE","BIT","BAD","ABOUT","DOING","WHAT","I","DID","BUT","IF","YOU","THINK","ABOUT","IT","IF","ROWLEYS","PARENTS","HADNT","MAKE","US","TURN","OFF","THE","TV","THIS","NEVER","WOULD","HAVE","HAPPENED"},
                new string[]{"AT",key[1] + "" + key[0] + "" + key[0], "ROWLEYS","MOM","CAME","DOWN","TO","SAY","IT","WAS","LIGHTS","OUT","IF","I","WOULDVE","KNOWN","ROWLEYS","BEDTIME","ON","WEEKENDS","WAS",key[1] + "" + key[0] + "" + key[0], "BELIEVE","ME","I","NEVER","WOULD","HAVE","COME","OVER"},
                new string[]{"AND","THEN","I","FOUND","OUT","ANOTHER","UGLY","SURPRISE","THERE","WAS","NO","GUEST","BED","SO","I","HAD","TO","SLEEP","IN","THE","SAME","BED","WITH","ROWLEY","I","TRIED","TO","LAY","AS","FAR","AWAY","FROM","ROWLEY","AS","POSSIBLE","BUT","IT","WAS","IMPOSSIBLE","TO","GET","TO","SLEEP","WITH","HALF","OF","MY","BODY","HANGING","OFF","THE","BED"},
                new string[]{"ROWLEY","FELL","ASLEEP","RIGHT","AWAY","BUT","IT","MUSTVE","TAKEN","ME","TWO","HOURS","BUT","RIGHT","WHEN","I","FINALLY","STARTED","TO","DRIFT","OFF","ROWLEY","LETS","OUT","THIS","SCREAM","WHICH","SCARED","ME","SO","BAD","I","DROPPED","RIGHT","OUT","OF","THE","BED","AND","ONTO","THE","HARDWOOD","FLOOR"},
                new string[]{"ROWLEYS","PARENTS","CAME","RUNNING","IN","AND","ROWLEY","STARTED","BABBLING","ALL","OF","THIS","INCOHERENT","GIBBERISH"},
                new string[]{"IT","TURNS","OUT","HE","HAD","A","NIGHTMARE","THAT","A","CHICKEN","WAS","HIDING","UNDERNEATH","HIM","AND","THATS","WHAT","MADE","HIM","YELL","OUT","BUT","I","THINK","ROWLEY","WAS","SO","OUT","OF","IT","HE","DIDNT","REALLY","REALIZE","IT","WAS","JUST","A","DREAM"},
                new string[]{"SO","ROWLEYS","PARENTS","TOOK","HIM","INTO","THEIR","ROOM","AND","SPENT","THE","NEXT","TWENTY","MINUTES","CALMING","HIM","DOWN","AND","TELLING","HIM","IT","WAS","JUST","A","DREAM","AND","HOW","THERE","REALLY","WAS","NO","CHICKEN"},
                new string[]{"MAN","IF","I","WOKE","MY","DAD","UP","WITH","SOME","NONSENSE","ABOUT","A","CHICKEN","YOU","BETTER","BELIEVE","HE","WOULDNT","BE","GIVING","ME","A","BIG","HUG","AND","TELLING","ME","EVERYTHING","WAS","OK","BUT","THAT","JUST","GOES","TO","SHOW","HOW","DIFFERENT","MY","PARENTS","ARE","FROM","ROWLEYS"},
                new string[]{"AND","I","JUST","WANTED","TO","MAKE","A","NOTE","THAT","NOBODY","SEEMED","ALL","THAT","CONCERNED","THAT","I","TOOK","A","THREEFOOT","FALL","ONTO","THE","FLOOR","EVEN","THOUGH","THAT","HAPPENED","FOR","REAL","AND","NOT","JUST","IN","SOME","STUPID","DREAM"},
                new string[]{"I","THINK","ROWLEY","SPENT","THE","NIGHT","IN","HIS","PARENTS","BED","WHICH","WAS","JUST","FINE","BY","ME","BECAUSE","WITHOUT","ROWLEY","AND","HIS","NIGHTMARES","I","WAS","FINALLY","ABLE","TO","GET","SOME","SLEEP"},
                new string[]{"BUT","THE","SECOND","I","WOKE","UP","THIS","MORNING","I","CAME","HOME","AND","POURED","MYSELF","A","BIG","BOWL","OF","JUNK","CEREAL","AND","DID","MY","BEST","TO","FORGET","ABOUT","THE","WHOLE","EXPERIENCE"},
                new string[]{"I","COULDNT","WAIT","FOR","SCHOOL","TO","BE","OVER","WITH","TODAY","SO","I","COULD","GO","HOME","AND","PLAY","TWISTED","WIZARD","A","VIDEO","GAME","IVE","BEEN","PLAYING","FOR","FIVE","DAYS","STRAIGHT"}
            },
            new string[][]
            {
                new string[]{"THE","ONLY","PROBLEM","WITH","TWISTED","WIZARD","IS","THAT","YOU","CANT","SAVE","YOUR","PROGRESS","SO","YOU","HAVE","TO","JUST","LEAVE","IT","ON","ALL","THE","TIME","SO","IMAGINE","HOW","I","FELT","TODAY","WHEN","I","REALIZED","I","GOT","HOME","ABOUT","FIVE","SECONDS","TOO","LATE"},
                new string[]{"BELIEVE","ME","FROM","NOW","ON","I","AM","GOING","TO","PUT","A","PIECE","OF","BLACK","TAPE","OVER","THE","POWER","LIGHT","SO","IT","NEVER","HAPPENS","AGAIN"},
                new string[]{"I","DONT","KNOW","IF","IVE","MENTIONED","IT","BEFORE","BUT","I","AM","SUPER","GOOD","AT","VIDEO","GAMES","I","DONT","KNOW","ANYONE","WHO","HAS","BEAT","AS","MANY","GAMES","AS","ME","AND","IVE","GOT","ALL","MY","VICTORIES","ON","VIDEO","TAPE","TO","PROVE","IT"},
                new string[]{"UNFORTUNATELY","DAD","DOES","NOT","EXACTLY","APPRECIATE","MY","VIDEO","GAME","SKILLS","HE","IS","ALWAYS","GETTING","ON","ME","ABOUT","GOING","OUTSIDE","AND","DOING","SOMETHING","ACTIVE"},
                new string[]{"SO","TODAY","I","TRIED","TO","EXPLAIN","TO","HIM","THAT","WITH","VIDEO","GAMES","YOU","CAN","PLAY","SPORTS","LIKE","FOOTBALL","AND","SOCCER","AND","YOU","DONT","EVEN","HAVE","TO","GET","ALL","HOT","AND","SWEATY"},
                new string[]{"BUT","AS","USUAL","DAD","DIDNT","GET","MY","LOGIC","HES","A","PRETTY","SMART","GUY","IN","GENERAL","BUT","WHEN","IT","COMES","TO","COMMON","SENSE","SOMETIMES","I","WONDER","ABOUT","HIM"},
                new string[]{"ANYWAYS","THATS","HOW","I","FOUND","MYSELF","SHUT","OUT","OF","THE","HOUSE","THIS","AFTERNOON"},
                new string[]{"IM","SURE","DAD","WOULD","DISMANTLE","MY","VIDEO","GAME","SYSTEM","IF","HE","COULD","FIGURE","OUT","HOW","TO","BUT","LUCKILY","THE","PEOPLE","WHO","MAKE","THESE","THINGS","MAKE","THEM","PARENTPROOF"},
                new string[]{"SO","LIKE","I","SAID","I","WAS","SHUT","OUT","OF","THE","HOUSE","LOOKING","FOR","SOME","WAY","TO","ENTERTAIN","MYSELF"},
                new string[]{"WHAT","I","ALWAYS","DO","WHEN","DAD","MAKES","ME","GO","OUTSIDE","IS","I","JUST","GO","OVER","TO","COLLINS","HOUSE"},
                new string[]{"IM","NOT","A","HUGE","FAN","OF","COLLINS","BUT","HE","HAS","TWO","THINGS","GOING","FOR","HIM","ONE","HE","DOESNT","MIND","WATCHING","ME","PLAY","HIS","VIDEO","GAMES","AND","TWO","HIS","DAD","HAS","EVERY","SPIDER","MAN","COMIC","BOOK","SINCE",key[1] + "" + key[1] + "" + key[0] + "" + key[0]},
                new string[]{"I","WOULD","GO","OVER","TO","ROWLEYS","HOUSE","TO","PLAY","VIDEO","GAMES","BUT","HE","ALWAYS","WANT","TO","TAKE","TURNS","WHICH","REALLY","BREAKS","MY","CONCENTRATION"},
                new string[]{"PLUS","ROWLEY","DOESNT","UNDERSTAND","THAT","IF","HE","PLAYS","USING","MY","MEMORY","CARD","IT","WILL","REALLY","SCREW","UP","MY","STATS"},
                new string[]{"THE","OTHER","THING","ABOUT","ROWLEY","IS","THAT","HES","NOT","A","SERIOUS","GAMER","LIKE","ME","HES","GOT","THIS","ONE","RACING","GAME","CALLED","FORMULA","ONE","RACING","IF","YOU","EVER","WANT","TO","BEAT","HIM","IN","IT","JUST","NAME","YOUR","CAR","SOMETHING","STUPID","AND","WATCH","WHAT","HAPPENS"},
                new string[]{"SO","TODAY","I","PLAYED","TWISTED","WIZARD","OVER","AT","COLLINS","UNTIL","IT","WAS","TIME","TO","COME","HOME","FOR","DINNER"}
            },
            new string[][]
            {
                new string[]{"ON","MY","WAY","UP","THE","HILL","I","MADE","SURE","TO","JUMP","THROUGH","THE","THOMPSONS","SPRINKLER","TO","MAKE","IT","LOOK","LIKE","I","WAS","ALL","HOT","AND","SWEATY","AND","THEN","I","TIMED","MY","ENTRANCE","PERFECTLY"},
                new string[]{"SO","MY","TRICK","WORKED","ON","DAD","BUT","IT","KIND","OF","BACKFIRED","WITH","MOM","BECAUSE","WHEN","SHE","SAW","ME","SHE","MADE","ME","TAKE","A","SHOWER","BEFORE","DINNER"},
                new string[]{"DAD","MUST","HAVE","BEEN","PRETTY","PROUD","OF","HIMSELF","FOR","HIS","IDEA","TO","KICK","ME","OUT","OF","THE","HOUSE","YESTERDAY","BECAUSE","HE","DID","IT","AGAIN","TODAY"},
                new string[]{"I","WAS","ACTUALLY","GOING","TO","GO","OUTSIDE","ANYWAY","BECAUSE","ROWLEY","HAD","GOTTEN","THIS","NEW","MODEL","ROCKET","WE","WANTED","TO","TRY","OUT","SO","WE","WENT","DOWN","TO","THE","SCHOOL","AND","SET","IT","OFF","BUT","THE","WIND","CARRIED","IT","ALL","THE","WAY","TO","THE","WOODS","AT","THE","END","OF","THE","FOOTBALL","FIELD"},
                new string[]{"I","DONT","KNOW","IF","I","EVER","MENTIONED","IT","BEFORE","BUT","THERES","A","BULLY","WHO","HANGS","OUT","IN","THOSE","WOODS","NAMED","HERBIE","REAMER","KIDS","LIKE","ME","AND","ROWLEY","STAY","AS","FAR","AWAY","FROM","THOSE","WOODS","AS","POSSIBLE"},
                new string[]{"ITS","A","REAL","PITY","TOO","BECAUSE","LIKE","I","SAID","IT","WAS","A","BRANDNEW","ROCKET"},
                new string[]{"I","HAVE","NO","IDEA","HOW","OLD","HERBIE","REAMER","IS","OR","WHERE","HE","LIVES","I","GUESS","ITS","POSSIBLE","THAT","HE","LIVES","RIGHT","THERE","IN","THE","WOODS","LIKE","A","WILD","ANIMAL","ALL","I","KNOW","IS","THAT","HES","BEEN","AROUND","LONG","ENOUGH","THAT","HE","TERRORIZED","RODRICK","AND","HIS","FRIENDS","WHEN","THEY","WERE","IN","MY","GRADE"},
                new string[]{"THE","THING","THAT","STINKS","IS","THAT","HERBIE","REAMERS","WOODS","ARE","RIGHT","BETWEEN","MY","HOUSE","AND","THE","SCHOOL","SO","IF","WE","COULD","CUT","THROUGH","THE","WOODS","IT","WOULD","SAVE","US","SOMETHING","LIKE","TWENTY","MINUTES","OF","WALKING"},
                new string[]{"I","WAS","TELLING","DAD","ALL","ABOUT","HERBIE","REAMER","THE","OTHER","DAY","DAD","TOLD","ME","ABOUT","THE","BULLY","FROM","WHEN","HE","WAS","GROWING","UP","SAM","SHARMAN"},
                new string[]{"DAD","SAID","SAM","SHARMAN","DID","THIS","PINCH","WHERE","HE","GRABBED","YOUR","SKIN","AND","TWISTED","IT","AROUND","TWO","TIMES"},
                new string[]{"DAD","TOLD","ME","THE","WAY","ALL","THE","NEIGHBORHOOD","KIDS","DEALT","WITH","SAM","SHARMAN","WAS","THAT","THEY","BANDED","TOGETHER","AND","TOLD","THE","PRINCIPAL","ON","HIM","DAD","SAID","SAM","CRIED","AND","THAT","HE","NEVER","DID","THE","SAM","SHARMAN","PINCH","AGAIN","AND","NOW","HES","AN","AIR","CONDITIONER","REPAIRMAN","AND","APPARENTLY","NOW","HES","A","REALLY","NICE","GUY"},
                new string[]{"WELL","FROM","THE","SOUND","OF","SAM","SHARMAN","HE","WOULDNT","LAST","TWO","SECONDS","AGAINST","HERBIE","REAMER","BUT","I","DIDNT","WANT","TO","HURT","DADS","FEELINGS","SO","I","JUST","TRIED","TO","ACT","IMPRESSED","BY","HIS","STORY"},
                new string[]{"AFTER","WE","LOST","THE","ROCKET","WE","WENT","UP","TO","ROWLEYS","TO","PLAY","CARDS","BUT","I","LOST","TRACK","OF","TIME","AND","I","WAS","LATE","FOR","DINNER","AT","MY","HOUSE","SO","ON","MY","WAY","DOWN","THE","HILL","I","TRIED","TO","THINK","OF","A","GOOD","EXCUSE","TO","GET","ME","OUT","OF","TROUBLE","WITH","MOM"}
            },
            new string[][]
            {
                new string[]{"MOM","WAS","PRETTY","HOT","AT","ME","FOR","BEING","LATE","JUST","LIKE","I","EXPECTED","SO","I","TOLD","HER","THAT","THE","CLOCK","IN","ROWLEYS","KITCHEN","MUST","BE","WRONG","AND","THAT","I","THOUGHT","I","WAS","RIGHT","ON","TIME"},
                new string[]{"AND","DO","YOU","KNOW","WHAT","MOM","DID","SHE","CALLED","ROWLEYS","MOM","AND","CAUGHT","ME","RED","HANDED"},
                new string[]{"SO","MOM","WAS","REALLY","MAD","THAT","I","LIED","BUT","AS","FAR","AS","BEING","TRICKY","GOES","MOM","SHOULDNT","BLAME","ME","BECAUSE","I","LEARNED","EVERYTHING","I","KNOW","FROM","HER"},
                new string[]{"I","REMEMBER","THIS","ONE","TIME","WHEN","I","WAS","IN","THE","SECOND","GRADE","AND","MOM","COULDNT","GET","ME","TO","BRUSH","MY","TEETH","SO","SHE","MADE","THIS","PRETEND","CALL","TO","THE","DENTIST","AND","I","TOTALLY","FELL","FOR","IT"},
                new string[]{"IN","FACT","THATS","WHEN","I","STARTED","BRUSHING","MY","TEETH","FIVE","TIMES","A","DAY"},
                new string[]{"MOM","SAID","SHE","WAS","GOING","TO","THINK","ABOUT","WHAT","MY","PUNISHMENT","SHOULD","BE","FOR","TELLING","A","LIE","AND","SHED","LET","ME","KNOW","AS","SOON","AS","SHE","CAME","UP","WITH","SOMETHING","THAT","FIT","THE","CRIME"},
                new string[]{"SEE","THATS","THE","DIFFERENCE","BETWEEN","MOM","AND","DAD","DAD","IS","PRETTY","SIMPLE","IF","YOU","MESS","UP","IN","FRONT","OF","HIM","HE","JUST","THROWS","WHATEVER","IS","IN","HIS","HAND","AT","YOU"},
                new string[]{"BUT","MOMS","A","LOT","MORE","CRAFTY","WITH","HER","PUNISHMENTS","SHE","THINKS","ABOUT","IT","FOR","A","FEW","DAYS","AND","THE","WAITING","ENDS","UP","BEING","JUST","ABOUT","AS","BAD","AS","THE","PUNISHMENT"},
                new string[]{"IN","THE","MEANTIME","YOU","END","UP","DOING","ALL","THESE","NICE","THINGS","HOPING","ITLL","GET","YOU","OFF","EASIER"},
                new string[]{"BUT","THEN","AFTER","A","FEW","DAYS","JUST","WHEN","YOU","FORGET","ABOUT","THE","PUNISHMENT","THATS","COMING","THATS","WHEN","SHE","GETS","YOU"},
                new string[]{"THE","ONLY","GOOD","THING","ABOUT","MOMS","PUNISHMENTS","IS","THAT","SHES","PRETTY","SOFT","SO","IF","YOU","JUST","LAY","LOW","FOR","A","WHILE","YOU","CAN","PRETTY","MUCH","ALWAYS","GET","OUT","OF","THE","PUNISHMENT","EARLY"},
                new string[]{"ANYWAY","THATS","WHAT","IM","COUNTING","ON","WHILE","I","WAIT","FOR","THIS","PUNISHMENT","TO","GET","HANDED","DOWN"},
                new string[]{"WELL","NOW","IVE","GONE","AND","DONE","IT","REMEMBER","HOW","I","SAID","YESTERDAY","THAT","WHILE","YOURE","WAITING","FOR","MOM","TO","HAND","DOWN","HER","PUNISHMENT","YOU","END","UP","DOING","ALL","THIS","GOOD","STUFF","TO","MAKE","HER","CHANGE","HER","MIND","IT","WAS","THAT","KIND","OF","THINKING","THAT","GOT","ME","IN","THIS","EXTRA","TROUBLE","IM","IN","NOW"},
                new string[]{"TODAY","AFTER","SCHOOL","I","THOUGHT","MAYBE","IF","I","WASHED","MOMS","CAR","BEFORE","SHE","GOT","HOME","SHE","MIGHT","GO","EASY","ON","ME","WITH","THE","PUNISHMENT","THING","SO","THATS","WHAT","I","DID","BUT","I","MADE","THE","MISTAKE","OF","USING","BRILLO","TO","RUB","OFF","ALL","THE","BUGS","AND","TAR","SPOTS","SO","I","WAS","IN","FOR","THE","SURPRISE","OF","MY","LIFE","WHEN","I","RINSED","THE","CAR","OFF"}
            },
            new string[][]
            {
                new string[]{"I","TOTALLY","RUINED","THE","PAINT","JOB","I","THOUGHT","ABOUT","JUST","DENYING","EVERYTHING","WHEN","MOM","ASKED","ME","HOW","HER","CAR","GOT","SCRATCHED","UP","BUT","MANNY","WAS","THERE","TO","SEE","EVERYTHING"},
                new string[]{"MANNY","HAS","BEEN","TELLING","ON","ME","SINCE","HE","COULD","TALK","IN","FACT","HE","HAS","TOLD","ON","ME","FOR","STUFF","I","DID","BEFORE","HE","COULD","TALK","ONE","TIME","WHEN","I","WAS","EIGHT","I","BROKE","THE","SLIDING","GLASS","DOOR","BUT","MOM","AND","DAD","COULDNT","PEG","IT","ON","ME","BUT","MANNY","WAS","THERE","TO","WITNESS","IT","AND","HE","SQUEALED","ON","ME","THREE","YEARS","LATER"},
                new string[]{"SO","AFTER","MANNY","STARTED","TALKING","I","HAD","TO","START","WORRYING","ABOUT","ALL","THE","BAD","THINGS","HE","SAW","ME","DO","WHEN","HE","WAS","REAL","SMALL"},
                new string[]{"I","USED","TO","BE","A","TATTLE","TALE","MYSELF","UNTIL","I","LEARNED","MY","LESSON","THIS","ONE","TIME","I","TOLD","ON","RODRICK","FOR","SAYING","A","BAD","WORD","MOM","ASKED","ME","WHICH","WORD","HE","SAID","SO","I","SPELLED","IT","OUT","AND","IT","WAS","A","LONG","ONE","TOO","NOT","ONLY","DID","RODRICK","GET","AWAY","SCOT","FREE","BUT","I","GOT","PUNISHED","FOR","KNOWING","HOW","TO","SPELL","A","BAD","WORD"},
                new string[]{"LIKE","I","SAID","I","KNEW","I","COULDNT","TALK","MANNY","OUT","OF","TELLING","ON","ME","FOR","USING","THE","BRILLO","ON","THE","CAR","SO","I","DECIDED","TO","TRY","A","TRICK","IVE","BEEN","SAVING","UP","FOR","A","SITUATION","LIKE","THIS"},
                new string[]{"I","PACKED","UP","A","BAG","AND","MADE","IT","SEEM","LIKE","I","WAS","GOING","TO","RUN","AWAY","FROM","HOME","RATHER","THAN","FACE","MOM","AND","DAD","FOR","WHAT","I","DID"},
                new string[]{"I","GOT","TO","GIVE","RODRICK","CREDIT","FOR","THIS","ONE","HE","USED","TO","PULL","IT","ON","ME","WHEN","HE","DID","SOMETHING","BAD","AND","HE","KNEW","I","WAS","GOING","TO","TELL","ON","HIM"},
                new string[]{"HED","BASICALLY","JUST","WALK","OUTSIDE","AND","THEN","COME","HOME","FIVE","MINUTES","LATER","BUT","BY","THAT","TIME","I","WAS","IN","PIECES","AND","COULDNT","EVEN","REMEMBER","THE","BAD","THING","THAT","HE","DID"},
                new string[]{"SO","I","LEFT","THE","HOUSE","AND","WAITED","FIVE","MINUTES","AND","THEN","CAME","BACK","INSIDE","I","WAS","EXPECTING","TO","FIND","MANNY","IN","THE","FOYER","BAWLING","LIKE","A","BABY","BUT","HE","WASNT","THERE","AT","ALL"},
                new string[]{"I","WENT","AROUND","THE","HOUSE","LOOKING","FOR","HIM","AND","I","STARTED","TO","GET","REALLY","NERVOUS","BECAUSE","IM","NOT","SUPPOSED","TO","LEAVE","MANNY","ALONE"},
                new string[]{"BUT","I","FOUND","HIM","IN","THE","KITCHEN","AND","GUESS","WHAT","HE","WAS","HALFWAY","FINISHED","WITH","MY","GIANT","HERSHEYS","KISS","IVE","BEEN","SAVING","SINCE","SUMMER"},
                new string[]{"SO","THINGS","TURNED","FROM","BAD","TO","WORSE"},
                new string[]{"WHEN","MOM","GOT","HOME","I","ACTUALLY","SPILLED","THE","BEANS","TO","HER","ABOUT","THE","CAR","AS","FAST","AS","I","COULD"},
                new string[]{"I","WAS","BASICALLY","TRYING","TO","KEEP","MANNY","QUIET","ABOUT","HOW","I","LEFT","THE","HOUSE","WHICH","WOULD","HAVE","GOTTEN","ME","IN","A","LOT","MORE","TROUBLE","THAN","RUINING","HER","CAR"}
            },
            new string[][]
            {
                new string[]{"MOM","JUST","LISTENED","WITH","A","FROWN","ON","HER","FACE","AND","THEN","TOLD","ME","WED","HAVE","TO","WAIT","UNTIL","DAD","GOT","HOME","TO","SEE","WHAT","HE","THOUGHT","ABOUT","WHAT","I","DID"},
                new string[]{"I","FIGURED","SHE","MIGHT","DO","THAT","SO","I","PULLED","ANOTHER","TRICK","I","LEARNED","FROM","RODRICK","I","INVITED","GRAMMA","OVER","FOR","DINNER","I","FIGURED","NOTHING","TOO","BAD","COULD","HAPPEN","IF","SHE","WAS","AROUND","AND","I","KNEW","I","COULD","USE","ANY","PROTECTION","I","COULD","GET"},
                new string[]{"SO","AT",key[0] + "" + key[0] + "" + key[0], "ON","THE","DOT","DAD","CAME","HOME","AND","OF","COURSE","HES","IN","A","GREAT","MOOD","FOR","SOME","REASON","THAT","ALWAYS","HAPPENS","TO","ME","WHEN","IM","IN","WAITFORDADTOGETHOME","KIND","OF","TROUBLE","IT","STINKS","BECAUSE","YOU","KNOW","HES","JUST","GONNA","BE","THAT","MUCH","MADDER","WHEN","HE","FINDS","OUT","WHAT","YOU","DID"},
                new string[]{"MOM","KEPT","QUIET","BECAUSE","GRAMMA","WAS","AROUND","SO","AT","LEAST","THAT","PART","OF","MY","PLAN","WORKED","OUT","AFTER","DINNER","I","JUST","SNUCK","UP","TO","MY","ROOM","AS","QUIETLY","AS","I","COULD","I","THINK","MOM","IS","TELLING","DAD","ABOUT","THE","CAR","RIGHT","NOW","BECAUSE","ITS","REAL","QUIET","DOWNSTAIRS"},
                new string[]{"RODRICK","HASNT","MADE","ME","FEEL","ANY","BETTER","ABOUT","THIS","WHOLE","THING","WHENEVER","IM","IN","TROUBLE","HE","TELLS","ME","THAT","IM","PROBABLY","GOING","TO","GET","THE","BELT"},
                new string[]{"AS","FAR","AS","I","KNOW","DAD","DOESNT","BELIEVE","IN","THAT","KIND","OF","PUNISHMENT","BUT","RODRICK","ALWAYS","SEEMS","TO","THINK","DAD","IS","GOING","TO","MAKE","AN","EXCEPTION","FOR","ME"},
                new string[]{"I","HAVE","DONE","MY","BEST","TO","MAKE","SURE","DAD","READS","ARTICLES","FROM","PARENTING","MAGAZINES","FROM","TIME","TO","TIME","TO","LET","HIM","KNOW","THAT","KIND","OF","THING","DOESNT","FLY","THESE","DAYS"},
                new string[]{"I","JUST","CUT","THOSE","ARTICLES","OUT","AND","SLIP","THEM","INTO","WHATEVER","BOOK","HES","READING","AT","THE","TIME"},
                new string[]{"I","THINK","I","HEAR","DAD","HEADING","UP","THE","STAIRS","NOW"},
                new string[]{"IF","THESE","ARE","THE","LAST","WORDS","I","EVER","WRITE","THEN","I","LEAVE","ALL","MY","COMIC","BOOKS","AND","ACTION","FIGURES","TO","ROWLEY","AND","PLEASE","THROW","THE","OTHER","HALF","OF","MY","HERSHEYS","KISS","AWAY","SO","MANNY","DOESNT","GET","IT"},
                new string[]{"WELL","I","LIVED","TO","SEE","ANOTHER","DAY","LAST","NIGHT","WHEN","DAD","KNOCKED","ON","MY","DOOR","I","PEEKED","THROUGH","THE","CRACK","AND","SAW","THAT","HE","WASNT","WEARING","THE","BELT","SO","I","LET","HIM","IN"},
                new string[]{"DAD","WASNT","EVEN","ALL","THAT","MAD","MOSTLY","BECAUSE","IT","WASNT","HIS","CAR","THAT","GOT","SCRAPED","UP","SO","HE","JUST","TOLD","ME","NOT","TO","USE","BRILLO","TO","CLEAN","A","CAR","AGAIN","AND","THAT","WAS","THAT"},
                new string[]{"MOM","WAS","ANOTHER","STORY","HER","PUNISHMENT","FOR","RUINING","HER","CAR","WAS","THAT","I","HAVE","TO","CLEAN","THE","WHOLE","BASEMENT"},
                new string[]{"AND","SHE","SAID","IF","SHE","CATCHES","ME","LYING","AGAIN","SHELL","TAKE","AWAY","MY","VIDEO","GAMES","FOR","GOOD","SO","I","BETTER","BE","PRETTY","HONEST","FROM","NOW","ON"},
                new string[]{"SO","NOW","THAT","ALL","THAT","STUFF","WAS","BEHIND","ME","I","COULD","SWITCH","MY","FOCUS","BACK","TO","SCHOOL"}
            },
            new string[][]
            {
                new string[]{"WE","JUST","GOT","OUR","FIRST","BIG","ASSIGNMENT","IN","ENGLISH","WHICH","IS","TO","DO","A","BOOK","REPORT","IVE","BEEN","MILKING","THE","SAME","BOOK","FOR","THE","PAST","FIVE","YEARS","ENCYCLOPEDIA","BROWN","DOES","IT","AGAIN","THERE","ARE","ABOUT",key[1] + "" + key[0], "SHORT","STORIES","IN","THERE","BUT","I","ALWAYS","JUST","TREAT","ONE","SHORT","STORY","LIKE","ITS","THE","WHOLE","BOOK","AND","THE","TEACHER","NEVER","NOTICES"},
                new string[]{"AND","TO","GIVE","YOU","AN","IDEA","OF","HOW","SHORT","EACH","STORY","IS","I","CAN","FINISH","ONE","IN","ABOUT","THREE","AND","A","HALF","MINUTES","WITHOUT","REALLY","TRYING"},
                new string[]{"THESE","ENCYCLOPEDIA","BROWN","STORIES","ARE","ALWAYS","THE","SAME","ITS","ALWAYS","ABOUT","HOW","SOMEBODY","COMMITS","SOME","LAME","CRIME","LIKE","STEALING","A","FISH","AND","THEN","ENCYCLOPEDIA","FIGURES","OUT","WHO","DID","IT","AND","MAKES","THEM","LOOK","STUPID"},
                new string[]{"I","HAVE","TO","SAY","THAT","NO","MATTER","HOW","HARD","I","TRY","I","HAVE","NEVER","FIGURED","OUT","ONE","OF","THESE","STORIES","BEFORE","THE","END","SO","I","GUESS","IM","NOT","AS","BRAINY","AS","ENCYCLOPEDIA"},
                new string[]{"IM","KIND","OF","AN","EXPERT","AT","WRITING","THESE","BOOK","REPORTS","BY","NOW","SO","I","KNOW","HOW","TO","WRITE","EXACTLY","WHAT","A","TEACHER","WANTS","TO","HEAR"},
                new string[]{"SO","FOR","ME","BOOK","REPORTS","ARE","NO","SWEAT"},
                new string[]{"I","FORGOT","TO","MENTION","THAT","THIS","AFTERNOON","I","TOOK","CARE","OF","PART","ONE","OF","MOMS","PUNISHMENT"},
                new string[]{"NOW","IVE","JUST","GOT","TO","KEEP","MY","NOSE","CLEAN","FOR","A","WHILE","AND","ILL","BE","OFF","THE","HOOK"},
                new string[]{"SO","FAR","ITS","BEEN","ABOUT",key[1] + "" + key[1], "HOURS","AND","IVE","KEPT","MY","PROMISE","TO","MOM","ABOUT","THE","WHOLE","HONESTY","THING","IT","REALLY","HASNT","BEEN","AS","BAD","AS","I","THOUGHT","AND","IN","FACT","IT","HAS","BEEN","KIND","OF","LIBERATING"},
                new string[]{"IVE","BEEN","IN","A","COUPLE","SITUATIONS","ALREADY","WHERE","IVE","BEEN","A","LOT","MORE","TRUTHFUL","THAN","I","WOULD","HAVE","BEEN","JUST","A","DAY","OR","TWO","AGO","LIKE","TODAY","WHEN","MAX","SMEDLEY","STARTED","TELLING","ME","HIS","BIG","PLANS"},
                new string[]{"AND","AT","ROWLEYS","GRANDPAS","BIRTHDAY","PARTY"},
                new string[]{"BUT","MOST","PEOPLE","DONT","SEEM","TO","REALLY","APPRECIATE","A","PERSON","AS","HONEST","AS","ME","LETS","JUST","SAY","I","CANT","UNDERSTAND","HOW","GEORGE","WASHINGTON","EVER","GOT","ELECTED","PRESIDENT"},
                new string[]{"IM","ACTUALLY","LOOKING","FORWARD","TO","TOMORROW","BECAUSE","THERE","ARE","A","COUPLE","OF","TEACHERS","WHO","COULD","USE","A","GOOD","DOES","OF","TRUTHFULNESS"},
                new string[]{"I","THINK","THE","HONESTY","PROMISE","HAS","BEEN","CANCELLED","AND","NOT","BY","ME","BUT","BY","MOM","TODAY","I","ANSWERED","THE","PHONE","AND","IT","WAS","MRS","GRETCHEN","FROM","THE","PTA","AND","SHE","WANTED","TO","TALK","TO","MOM","BUT","MOM","SIGNALED","TO","ME","THAT","I","SHOULD","TELL","MRS","GRETCHEN","THAT","SHE","WASNT","HOME","I","DIDNT","KNOW","IF","IT","WAS","A","TRICK","OR","WHAT","BUT","I","KNEW","THAT","I","WASNT","GOING","TO","GO","AND","BREAK","MY","HONESTY","STREAK","OVER","A","THING","LIKE","THIS","SO","I","MADE","MOM","GO","OUTSIDE","ON","THE","FRONT","PORCH","BEFORE","I","WOULD","SAY","A","WORD","TO","MRS","GRETCHEN"}
            },
            new string[][]
            {
                new string[]{"MOM","DIDNT","COME","RIGHT","OUT","AND","SAY","THE","HONESTY","DEAL","IS","OFF","BUT","SHE","DIDNT","SPEAK","TO","ME","FOR","THE","REST","OF","THE","NIGHT","SO","I","FIGURE","I","CAN","JUST","GO","BACK","TO","HOW","I","WAS","BEFORE"},
                new string[]{"THE","ONLY","OTHER","THING","THAT","HAPPENED","TODAY","THATS","WORTH","MENTIONING","IS","THAT","RODRICK","BROKE","HIS","OWN","SATURDAY","SLEEPING","RECORD","AT",key[1] + "" + key[1] + "" + key[0], "DAD","SAID","ENOUGH","IS","ENOUGH","AND","MADE","RODRICK","GET","OUT","OF","HIS","BED","IN","THE","BASEMENT"},
                new string[]{"BUT","RODRICK","JUST","TOOK","ALL","HIS","SLEEPING","GEAR","UPSTAIRS","AND","PLOPPED","HIMSELF","ON","THE","COUCH","UNTIL","IT","WAS","TIME","FOR","DINNER"},
                new string[]{"EVER","SINCE","MOM","DANGLED","THE","IDEA","OF","TAKING","AWAY","MY","VIDEO","GAMES","IVE","BEEN","TRYING","TO","BE","ON","MY","BEST","BEHAVIOR","SO","YOU","WOULDNT","THINK","I","COULD","GET","IN","TROUBLE","IN","THE","FIFTEEN","MINUTES","IT","TAKES","TO","DRIVE","TO","CHURCH","BUT","THATS","EXACTLY","WHAT","I","DID"},
                new string[]{"I","WAS","TRYING","TO","HAVE","SOME","FUN","WITH","MANNY","BY","MAKING","FUNNY","FACES","AT","HIM","IN","THE","BACK","SEAT","OF","THE","CAR","BUT","WHEN","I","FINALLY","GOT","A","LAUGH","OUT","OF","HIM","HE","SPIT","HIS","JUICE","ALL","OVER","THE","CAR","SEAT"},
                new string[]{"THAT","JUST","MADE","MANNY","LAUGH","EXTRA","HARD","BUT","THEN","THE","NEXT","THING","YOU","KNOW","MOM","SAYS","HE","COULD","HAVE","CHOKED","TO","DEATH"},
                new string[]{"WELL","I","GUESS","THAT","THOUGHT","WAS","JUST","TOO","MUCH","FOR","MANNY","TO","TAKE","ALL","I","KNOW","IS","THAT","THE","REST","OF","THE","RIDE","WAS","PRETTY","MISERABLE","FOR","EVERYONE"},
                new string[]{"SO","YOU","CAN","SEE","HOW","I","CAN","GO","FROM","BEING","THE","HERO","TO","THE","GOAT","IN","NO","TIME","FLAT"},
                new string[]{"AT","CHURCH","MY","OLD","BEST","FRIEND","BEN","WAS","SITTING","UP","FRONT","WITH","HIS","FAMILY","MOM","DOESNT","LET","US","SIT","TOO","CLOSE","TO","BENS","FAMILY","BECAUSE","ME","AND","BEN","USED","TO","ALWAYS","GET","INTO","GIGGLE","FITS","WHEN","WED","SIT","NEAR","EACH","OTHER"},
                new string[]{"OUR","BIG","ROUTINE","WAS","THAT","AT","THE","PART","IN","CHURCH","WHERE","YOURE","SUPPOSED","TO","SAY","PEACE","BE","WITH","YOU","AND","SHAKE","HANDS","WED","SAY","PEAS","BE","WITH","YOU","(LIKE","THE","VEGETABLE)"},
                new string[]{"MOM","SAID","IF","WE","DIDNT","STOP","LAUGHING","IN","CHURCH","SHE","WAS","GOING","TO","SEPARATE","US","SO","WE","BEHAVED","OURSELVES","FOR","A","WHILE"},
                new string[]{"BUT","THIS","ONE","SUNDAY","DURING","THE","PEACE","BE","WITH","YOU","PART","BEN","ACTUALLY","HANDED","ME","A","COUPLE","OF","DRIED","UP","PEAS","HE","HAD","BEEN","CARRYING","IN","HIS","POCKET","AND","WE","BOTH","TOTALLY","LOST","IT"},
                new string[]{"MOM","KEPT","GOOD","ON","HER","PROMISE","BECAUSE","WE","HAVENT","EVEN","SAT","ON","THE","SAME","SIDE","OF","THE","CHURCH","AS","BEN","SINCE","THAT","DAY"},
                new string[]{"ON","THE","WAY","HOME","FROM","CHURCH","WE","PASSED","BY","THE","SMEDLEYS","WHO","WERE","OUT","IN","THEIR","FRONT","YARD","THERE","ARE","ABOUT","SIX","BOYS","IN","THAT","FAMILY","BUT","ITS","HARD","TO","TELL","ANY","OF","THEM","APART"}
            },
            new string[][]
            {
                new string[]{"THE","SMEDLEYS","BIG","DREAM","IN","LIFE","IS","TO","WIN","THE","GRAND","PRIZE","ON","AMERICAS","FUNNIEST","HOME","VIDEOS","SO","THEYRE","ALWAYS","TRYING","TO","STAGE","SOME","KIND","OF","ACCIDENT"},
                new string[]{"I","BET","FOR","EVERY","TEN","TIMES","YOU","SEE","A","GUY","GET","HIT","IN","THE","GROIN","WITH","A","GOLF","BALL","ON","THAT","SHOW","NINE","OF","THEM","ARE","SENT","IN","BY","FAMILIES","LIKE","THE","SMEDLEYS","JUST","TRYING","TO","MAKE","A","BUCK"},
                new string[]{"TODAY","AT","SCHOOL","I","WAS","LOOKING","AT","THE","INSIDE","COVER","OF","MY","PREALGEBRA","BOOK","AND","I","SAW","THAT","IT","USED","TO","BELONG","TO","JIMMY","JURY","WHO","IS","THE","MOST","POPULAR","KID","IN","THE",key[0] + "TH","GRADE","I","FIGURED","I","MIGHT","BE","ABLE","TO","TRANSLATE","THIS","INTO","SOME","PRETTY","BIG","POPULARITY","POINTS","OF","MY","OWN","BUT","THE","PROBLEM","WAS","THAT","JIMMY","JURY","DIDNT","WRITE","HIS","NAME","ANYWHERE","ON","THE","OUTSIDE","OF","THE","BOOK","SO","I","TOOK","CARE","OF","THAT","DETAIL","ON","MY","OWN"},
                new string[]{"UNFORTUNATELY","I","ALSO","GOT","BRIAN","GLEESONS","SCIENCE","BOOK","SO","THINGS","SORT","OF","EVENED","OUT"},
                new string[]{"IN","PHYS","ED","TODAY","A","COUPLE","GIRLS","CAUSED","A","STIR","WHEN","THEY","PRESENTED","A","PETITION","TO","MR","UNDERWOOD","THE","GYM","TEACHER"},
                new string[]{"THE","WAY","THEY","GOT","THE","IDEA","OF","WRITING","A","PETITION","WAS","BECAUSE","IN","HISTORY","WERE","STUDYING","MARTIN","LUTHER","WHO","IS","A","GUY","WHO","WROTE","A","LIST","OF","DEMANDS","AND","POSTED","THEM","ON","A","CHURCH","DOOR"},
                new string[]{"SO","THE","GIRLS","GOT","IT","IN","THEIR","HEADS","THAT","IT","WAS","UNFAIR","THAT","THEY","HAD","TO","DO","GIRL","PUSHUPS","WHILE","THE","GUYS","GET","TO","DO","BOY","PUSHUPS","AND","THEY","ALL","SIGNED","THEIR","NAMES","TO","A","LIST","TO","PROTEST"},
                new string[]{"IF","I","WAS","THEM","I","WOULDNT","COMPLAIN","GIRL","PUSHUPS","ARE","ABOUT","TEN","TIMES","EASIER","THAN","BOY","PUSHUPS"},
                new string[]{"WITH","GIRL","PUSHUPS","YOU","GET","TO","KEEP","YOUR","KNEES","ON","THE","GROUND","SO","YOU","ONLY","HAVE","TO","WORK","HALF","AS","HARD"},
                new string[]{"SO","I","THINK","MR","UNDERWOOD","SURPRISED","THEM","WHEN","HE","SAID","SURE","YOU","GIRLS","CAN","GO","AHEAD","AND","DO","BOY","PUSHUPS"},
                new string[]{"I","THINK","THE","GIRLS","WERE","EXPECTING","A","LOT","BIGGER","FIGHT","AND","NOW","I","KNOW","AT","LEAST","HALF","OF","THEM","WISH","THEY","COULD","TAKE","THAT","PETITION","BACK"},
                new string[]{"I","KIND","OF","GOT","INSPIRED","BY","THE","WHOLE","EPISODE","AND","I","STARTED","TO","PUT","TOGETHER","A","PETITION","SAYING","WE","BOYS","WANTED","TO","BE","ALLOWED","TO","DO","GIRL","PUSHUPS"},
                new string[]{"BUT","WHEN","I","SAW","THE","GROUP","OF","GUYS","WHO","WAS","INTERESTED","IN","SIGNING","IT","I","DECIDED","TO","JUST","BAG","IT"}
            },
            new string[][]
            {
                new string[]{"TONIGHT","WAS","A","PRETTY","BIG","DEAL","FOR","ME","BECAUSE","IT","WAS","THE","START","OF","THE","NEW","TV","SEASON","I","HAVE","HAD","TO","WATCH","FIVE","MONTHS","OF","RERUNS","SO","YOU","CAN","PROBABLY","UNDERSTAND","THAT","I","WAS","FIRED","UP","TO","FINALLY","SEE","SOMETHING","NEW"},
                new string[]{"DAD","PUT","MANNY","TO","BED","AND","MOM","MADE","POPCORN","AND","I","WAS","ALL","SET","FOR","SOME","SERIOUS","TELEVISION","WATCHING","BUT","FIVE","MINUTES","INTO","THE","FIRST","SHOW","MANNY","MAKES","AN","APPEARANCE","IN","THE","FAMILY","ROOM"},
                new string[]{"BUT","INSTEAD","OF","PUTTING","MANNY","RIGHT","BACK","TO","BED","MOM","LET","HIM","STAY","UP","AND","WATCH","TV","WITH","US"},
                new string[]{"AND","HERES","THE","KICKER","MOM","MADE","ME","CHANGE","THE","CHANNEL","BECAUSE","ON","THE","SHOW","I","WAS","WATCHING","THE","KIDS","HAD","A","DISRESPECTFUL","ATTITUDE","TOWARDS","THE","ADULTS","AND","SHE","DIDNT","WANT","MANNY","EXPOSED","TO","THAT","SORT","OF","THING"},
                new string[]{"COP","SHOWS","WERE","OUT","TOO","BECAUSE","OF","THE","VIOLENCE","SO","GUESS","WHAT","MOM","MADE","ME","TURN","TO","THE","CARTOON","CHANNEL","WHICH","IS","EXACTLY","WHAT","MANNY","WAS","WATCHING","BEFORE","HE","WENT","TO","BED"},
                new string[]{"MAN","I","WAS","STEAMED","WHEN","I","WAS","A","KID","THERE","WASNT","ANY","OF","THIS","GETTING","OUT","OF","BED","AND","COMING","BACK","DOWNSTAIRS","STUFF"},
                new string[]{"I","THINK","I","DID","IT","ONCE","OR","TWICE","BUT","DAD","PUT","A","STOP","TO","IT","REAL","QUICK"},
                new string[]{"THERE","WAS","THIS","BOOK","DAD","USED","TO","READ","TO","ME","BEFORE","I","WENT","TO","SLEEP","CALLED","A","LIGHT","IN","THE","ATTIC"},
                new string[]{"IT","WAS","AN","AWESOME","BOOK","BUT","ON","THE","BACK","OF","IT","THERE","WAS","A","PICTURE","OF","THE","GUY","WHO","WROTE","IT","SHEL","SILVERSTEIN"},
                new string[]{"HAVE","YOU","EVER","SEEN","THAT","GUY","ALL","I","CAN","SAY","IS","THAT","HE","LOOKS","MORE","LIKE","A","BURGLAR","OR","A","PIRATE","THAN","A","GUY","WHO","WRITES","POEMS","FOR","KIDS"},
                new string[]{"WELL","DAD","USED","TO","LEAVE","THAT","BOOK","RIGHT","ON","MY","END","TABLE","EVERY","NIGHT","WITH","THE","BACK","OF","THE","BOOK","FACING","UP","AND","IT","REALLY","GAVE","ME","THE","HEEBIE","JEEBIES"},
                new string[]{"I","THINK","DAD","CAUGHT","ON","THAT","SHEL","SILVERSTEIN","KIND","OF","FREAKED","ME","OUT","BECAUSE","THE","FIRST","NIGHT","AFTER","I","PULLED","THE","KIND","OF","THING","MANNY","PULLED","TONIGHT","DAD","READ","ME","SOME","POEMS","FROM","A","LIGHT","IN","THE","ATTIC","AND","THEN","SAID","IF","YOU","GET","UP","AGAIN","TONIGHT","I","WONDER","IF","YOULL","RUN","INTO","SHEL","SILVERSTEIN","IN","THE","HALLWAY"},
                new string[]{"WELL","DAD","REALLY","HAD","MY","NUMBER","WITH","THAT","TRICK","I","NEVER","GOT","UP","AGAIN","NOT","EVEN","TO","GO","TO","THE","BATHROOM","I","WOULD","RATHER","HAVE","WET","THE","BED","THAN","TO","FIND","THAT","GUY","CREEPING","AROUND","UPSTAIRS"}
            },
            new string[][]
            {
                new string[]{"LAST","NIGHT","MANNY","FINALLY","WENT","TO","BED","AND","I","FINALLY","GOT","TO","WATCH","SOME","OF","THE","NEW","SHOWS","BUT","IVE","GOT","TO","SAY","IT","WASNT","WORTH","THE","WAIT"},
                new string[]{"ALL","THE","NEW","SITCOMS","ARE","THE","SAME","THEY","BASICALLY","TAKE","ONE","LAME","JOKE","AND","THEN","DRIVE","IT","INTO","THE","GROUND","FOR","THE","NEXT",key[1] + "" + key[0], "MINUTES","I","WANTED","TO","SEE","IF","I","COULD","WRITE","A","BETTER","SHOW","THAN","THESE","CLOWNS","WHO","ARE","MAKING","THOUSANDS","OF","DOLLARS","SO","AT","LUNCH","TODAY","I","GAVE","IT","A","SHOT"},
                new string[]{"I","SHOWED","MY","DRAWINGS","TO","MOM","AND","SHE","SURPRISED","ME","BY","BEING","REALLY","INTERESTED","SHE","TOLD","ME","THAT","IF","I","REALLY","WANTED","TO","WRITE","A","TV","SHOW","I","HAD","TO","COME","UP","WITH","A","WHOLE","PLOT","WITH","A","BEGINNING","AND","END","I","THINK","SHE","WAS","JUST","HAPPY","I","WAS","SHOWING","AN","INTEREST","IN","SOMETHING","OTHER","THAN","A","VIDEO","GAME","FOR","ONCE"},
                new string[]{"IT","SEEMS","LIKE","EVERY","NEW","SHOW","THAT","COMES","OUT","NOWADAYS","IS","ABOUT","SOME","DAD","WHO","DOES","OR","SAYS","SOMETHING","REALLY","IGNORANT","AT","THE","BEGINNING","OF","THE","SHOW","BUT","THEN","BY","THE","END","OF","THE","SHOW","HE","COMES","AROUND","AND","REALIZES","HE","WAS","BEING","A","NINCOMPOOP","SO","THE","NEXT","SHOW","I","WROTE","(CALLED","WISE","UP","MR","LOCKERMAN)","IS","BASED","ON","THAT","KIND","OF","IDEA"},
                new string[]{"I","SHOWED","THIS","SCRIPT","TO","MOM","WHEN","I","WAS","DONE","TO","BE","HONEST","WITH","YOU","I","DONT","THINK","SHE","REALLY","APPROVES","OF","MY","TYPE","OF","HUMOR","BUT","SHE","IS","PRETTY","EXCITED","TO","SEE","ME","WORKING","ON","THIS","STUFF","USUALLY","WHEN","MOM","GETS","TOO","ENTHUSIASTIC","ABOUT","SOMETHING","IM","DOING","THATS","MY","SIGNAL","TO","BACK","OFF","BUT","MOM","SAID","SHES","GOING","TO","TRY","TO","GET","A","HOLD","OF","A","VIDEO","CAMERA","FROM","WORK","TO","LET","ME","PUT","SOME","OF","MY","IDEAS","ON","FILM","SO","I","GUESS","I","CAN","DEAL","WITH","IT","FOR","NOW"},
                new string[]{"LAST","NIGHT","I","WAS","SO","EXCITED","ABOUT","GETTING","MY","HANDS","ON","A","VIDEO","CAMERA","I","COULDNT","SLEEP","SO","TODAY","IN","SCHOOL","I","FAKED","ABOUT","BEING","SICK","IN","GYM","SO","I","COULD","WRITE","DOWN","MY","IDEA","FOR","A","MOVIE"},
                new string[]{"MOM","WAS","ABLE","TO","GET","A","CAMERA","FROM","WORK","SO","I","FINISHED","UP","THE","SCRIPT","AND","BROUGHT","IT","UP","TO","ROWLEYS","HOUSE","TO","SHOW","HIM","I","THINK","ITS","MY","BEST","STUFF","YET","AND","THATS","SAYING","SOMETHING"},
                new string[]{"WELL","ROWLEYS","REACTION","WASNT","QUITE","WHAT","I","WAS","HOPING","FOR"},
                new string[]{"YOUD","THINK","ROWLEY","WOULD","BE","GRATEFUL","THAT","I","WAS","GOING","TO","MAKE","HIM","A","BIG","STAR","AND","OF","COURSE","I","GOT","NO","THANKS","FOR","WRITING","JUICY","ROLES","FOR","HIS","PARENTS"},
                new string[]{"TODAY","AFTER","SCHOOL","I","WALKED","UP","TO","ROWLEYS","HOUSE","TO","SHOW","HIM","SOME","REWRITES","I","DID","FOR","THE","BOY","WHOSE","FAMILY","THOUGHT","HE","WAS","A","DOG","BUT","ROWLEY","WOULDNT","ANSWER","THE","DOOR"},
                new string[]{"I","STARTED","TO","HEAD","HOME","AND","THEN","ALL","OF","THE","SUDDEN","MOM","PULLS","UP","ALONG","SIDE","ME","WITH","MACKIE","CREAVY","IN","THE","BACK","SEAT","OF","THE","CAR"},
                new string[]{"OH","MAN","I","COMPLETELY","FORGOT","ABOUT","SOCCER","DAD","MAKES","ME","DO","IT","EVERY","YEAR","SO","IM","WELL","ROUNDED"}
            },
            new string[][]
            {
                new string[]{"SO","I","GUESS","IVE","GOT","TO","PUT","MY","FILM","CAREER","ON","HOLD","AND","GO","GET","MY","SHINS","KICKED","FOR","A","COUPLE","OF","MONTHS"},
                new string[]{"THE","FIRST","NIGHT","OF","SOCCER","PRACTICE","IS","ALWAYS","THE","SAME","THEY","START","OFF","BY","DOING","THIS","SKILLS","TEST","TO","SEE","HOW","GOOD","YOU","ARE","USUALLY","I","DONT","CARE","HOW","I","GET","RANKED","BUT","THIS","YEAR","THE","GUY","WHO","WAS","DOING","THE","TESTING","WAS","MR","MATTHEWS","WHO","IS","THE","FATHER","OF","PIPER","MATTHEWS","THE","PRETTIEST","GIRL","IN","OUR","CHURCH","SO","I","FIGURED","ID","BETTER","DO","MY","BEST","IF","I","WANTED","TO","IMPRESS","MY","FUTURE","FATHERINLAW"},
                new string[]{"EVEN","THOUGH","I","TRIED","MY","HARDEST","I","STILL","GOT","RANKED","PRE","ALPHA","MINUS","WHICH","IS","ADULT","CODE","WORDS","FOR","YOU","STINK"},
                new string[]{"THE","NEXT","THING","THEY","DO","IS","PUT","EVERYONE","ON","A","TEAM","THEY","BASICALLY","TRY","TO","SPREAD","OUT","THE","REALLY","AWFUL","KIDS","LIKE","ME","SO","NO","ONE","TEAM","HAS","TOO","MANY","TERRIBLE","PLAYERS"},
                new string[]{"AND","WOULDNT","YOU","KNOW","MY","LUCK","I","GOT","PUT","ON","KENNY","KEITHS","TEAM","SO","THAT","MEANS","MY","COACH","IS","MR","KEITH","SAME","AS","LAST","YEAR"},
                new string[]{"MR","KEITH","HATES","ME","AND","I","TRACE","IT","ALL","BACK","TO","THE","FIRST","DAY","OF","PRACTICE","LAST","YEAR","A","BUNCH","OF","US","TERRIBLE","PLAYERS","WERE","SLACKING","OFF","HANGING","OUT","BY","THE","WATER","JUG","WHEN","MR","KEITH","YELLED","FOR","US","TO","GET","BACK","ON","THE","PLAYING","FIELD"},
                new string[]{"SO","AS","A","JOKE","I","RAN","BACKWARDS","WITH","MY","REAR","END","POINTED","AT","MR","KEITH"},
                new string[]{"I","THINK","IT","WOULD","HAVE","BEEN","FUNNIER","IF","ALL","THE","OTHER","GUYS","HAD","DONE","THE","SAME","THING","I","DID","BUT","THEY","KIND","OF","HUNG","ME","OUT","TO","DRY"},
                new string[]{"ANYWAY","YOU","CAN","PROBABLY","GUESS","THAT","MR","KEITH","DID","NOT","FIND","MY","JOKE","SO","AMUSING","AND","FROM","THEN","ON","HE","MADE","THINGS","PRETTY","MISERABLE","FOR","ME"},
                new string[]{"RIGHT","BEFORE","THE","FIRST","GAME","HE","GAVE","US","ALL","OUR","POSITIONS","AND","HE","TOLD","ME","MY","POSITION","WAS","SHAG","I","DIDNT","KNOW","A","WHOLE","LOT","ABOUT","SOCCER","BUT","I","WAS","PRETTY","PROUD","THAT","I","HAD","MY","VERY","OWN","POSITION","I","REMEMBER","BRAGGING","TO","RODRICK","ABOUT","IT"},
                new string[]{"BUT","RODRICK","KNEW","A","THING","OR","TWO","ABOUT","SOCCER","AND","HE","TOLD","ME","THAT","THE","SHAG","ISNT","ACTUALLY","A","REAL","POSITION","ON","THE","FIELD","ITS","JUST","A","KID","WHO","CHASES","ALL","THE","BALLS","THAT","GO","OUT","OF","BOUNDS"},
                new string[]{"AND","SURE","ENOUGH","RODRICK","WAS","RIGHT","MR","KEITH","NEVER","PUT","ME","IN","A","GAME","AND","I","WASNT","EVEN","THE","WORST","KID","ON","OUR","TEAM","WE","HAD","COLLIN","AND","MACKIE","CREAVY","AND","A","COUPLE","OTHER","KIDS","WHO","CAN","BARELY","KICK","A","SOCCER","BALL","AND","THERE","I","WAS","CHASING","BALLS","INTO","THE","STREET","AND","LET","ME","JUST","SAY","SOMETHING","IN","DEFENSE","OF","ALL","THOSE","SHAGS","OUT","THERE","SHAG","MIGHT","NOT","BE","THE","MOST","NOBLE","POSITION","IN","SOCCER","BUT","IT","IS","DEFINITELY","THE","MOST","STRESSFUL"}
            },
            new string[][]
            {
                new string[]{"TONIGHT","I","WAS","ALL","SET","TO","GO","OVER","TO","COLLINS","HOUSE","BUT","I","FOUND","OUT","DAD","HAD","RENTED","A","MOVIE","SO","I","CHANGED","MY","PLANS","AND","STAYED","HOME","WHENEVER","DAD","GETS","A","MOVIE","HE","NEVER","CHECKS","THE","RATING","SO","ITS","ALWAYS","WORTH","HANGING","AROUND","AND","SEEING","WHAT","HE","PICKED","OUT","AND","HALF","THE","TIME","HE","GETS","SOMETHING","MOM","WOULD","NEVER","LET","ME","WATCH","ON","MY","OWN"},
                new string[]{"THE","ONLY","DOWN","SIDE","ABOUT","WATCHING","MOVIES","WITH","DAD","IS","THAT","IF","THERE","IS","EVER","A","SCENE","WITH","ANYTHING","THE","LEAST","BIT","INAPPROPRIATE","SOMEHOW","MOM","SHOWS","UP","AT","THE","WORST","MOMENT","AND","MAKES","YOU","FEEL","ASHAMED","FOR","WATCHING","IT"},
                new string[]{"LUCKILY","I","HAVE","MASTERED","THE","KIND","OF","RESPONSE","THAT","GETS","ME","OFF","THE","HOOK","EVERY","TIME","ESPECIALLY","DURING","THE","RACY","SCENES"},
                new string[]{"I","JUST","MAKE","SURE","I","HEAD","BACK","DOWNSTAIRS","LATER","ON","TO","CATCH","UP","ON","ANYTHING","I","MISSED"},
                new string[]{"TODAY","AFTER","CHURCH","MOM","AND","I","WENT","OVER","TO","GRAMMAS","TO","CHECK","UP","ON","HER","MOM","WAS","PRETTY","WORRIED","BECAUSE","GRAMMA","HASNT","BEEN","ANSWERING","HER","PHONE","FOR","A","FEW","DAYS","SO","MOM","WANTED","TO","MAKE","SURE","GRAMMA","WAS","OK","BUT","WHEN","WE","GOT","THERE","WE","FOUND","GRAMMA","SITTING","IN","HER","KITCHEN","CLIPPING","COUPONS","LIKE","USUAL","SO","WHEN","MOM","ASKED","GRAMMA","WHY","SHE","HASNT","BEEN","ANSWERING","THE","TELEPHONE","GRAMMA","SAID","CORDLESS","TELEPHONES","ERASE","THE","MEMORY","OF","THE","ELDERLY"},
                new string[]{"WELL","THAT","KIND","OF","SET","MOM","OFF","BECAUSE","SHE","KNEW","EXACTLY","WHERE","GRAMMA","WAS","GETTING","HER","INFORMATION","FROM","THE","SUPERMARKET","TABLOIDS","BUT","SOMEHOW","GRAMMA","KEEPS","GETTING","A","HOLD","OF","THESE","THINGS","EVEN","THOUGH","SHE","DOESNT","DRIVE"},
                new string[]{"SO","WHEN","MOM","CONFRONTED","GRAMMA","ON","IT","AND","SAID","WHERE","DID","YOU","READ","THAT","MOM","GRAMMA","KNEW","SHE","WAS","CORNERED"},
                new string[]{"SO","MOM","FOUND","WHERE","GRAMMA","WAS","STASHING","THE","TABLOIDS","AND","WE","TOOK","IT","HOME","WITH","US","TO","THROW","AWAY","WHAT","MOM","DOESNT","KNOW","IS","THAT","I","ALWAYS","DIG","THOSE","THINGS","OUT","OF","THE","TRASH","AND","READ","THEM","WHEN","NO","ONES","AROUND"},
                new string[]{"THERES","ACTUALLY","A","BUNCH","OF","GOOD","STUFF","IN","THERE","LIKE","HOROSCOPES","AND","PREDICTIONS","IN","FACT","THE","REASON","I","TAKE","SCHOOL","WITH","A","GRAIN","OF","SALT","IS","BECAUSE","THIS","ONE","TABLOID","SAYS","THE","WHOLE","EAST","COAST","IS","GOING","TO","BE","UNDERWATER","WITHIN","FIVE","YEARS"},
                new string[]{"I","DONT","KNOW","IF","I","EVER","MENTIONED","THIS","BEFORE","BUT","EVERY","MORNING","WHEN","DAD","WAKES","ME","UP","HE","GIVES","THE","SAME","EXACT","SPEECH","ITS","TEN","OF","SEVEN","MOMS","IN","THE","SHOWER","AND","I","WANT","YOU","IN","THERE","THE","SECOND","SHES","OUT","SO","YOURE","NOT","LATE","FOR","YOUR","BUS","LETS","MOVE","IT","HUP","HUP","HUP"},
                new string[]{"I","DONT","KNOW","WHERE","DAD","GETS","HIS","MORNING","ENERGY","BUT","I","DEFINITELY","DID","NOT","INHERIT","THAT","GENE","FROM","HIM","AFTER","HE","WAKES","ME","UP","I","PROP","MYSELF","UP","ON","MY","ELBOW","AND","TRY","MY","HARDEST","NOT","TO","FALL","BACK","ASLEEP"}
            },
            new string[][]
            {
                new string[]{"THIS","ONE","DAY","I","ACCIDENTALLY","FELL","BACK","ASLEEP","AFTER","DAD","WOKE","ME","UP","AND","BELIEVE","ME","IT","WAS","THE","LAST","TIME","I","EVER","MADE","THAT","MISTAKE"},
                new string[]{"AT","SCHOOL","THIS","MORNING","THERE","WERE","A","BUNCH","OF","KIDS","FROM","MRS","BUNNS","HOMEROOM","CLASS","STANDING","AROUND","IN","THE","HALLWAY","TRIPPING","EVERY","OTHER","KID","THAT","WALKED","BY"},
                new string[]{"ITS","REALLY","SAD","TO","SEE","WHAT","PASSES","FOR","COMEDY","THESE","DAYS","BACK","IN","THE","FIFTH","GRADE","ME","AND","BEN","WERE","AN","AWESOME","COMEDY","TEAM","AND","WE","HAD","SOME","REALLY","GOOD","ROUTINES"},
                new string[]{"BUT","EVER","SINCE","BEN","LEFT","TOWN","THE","FUNNIEST","THING","THAT","EVER","HAPPENS","IN","SCHOOL","IS","WHEN","SOME","POOR","KID","DROPS","HIS","LUNCH","TRAY","IN","THE","CAFETERIA"},
                new string[]{"I","TRIED","TO","PICK","THE","COMEDY","THING","BACK","UP","WHEN","ROWLEY","CAME","ALONG","BUT","THINGS","NEVER","REALLY","WORKED","OUT"},
                new string[]{"I","THOUGHT","OF","ANOTHER","GOOD","REASON","TO","KEEP","A","JOURNAL","WHEN","I","GET","RICH","AND","FAMOUS","I","CAN","PULL","OUT","THIS","BOOK","TO","REMIND","MYSELF","WHY","I","SHOULDNT","LET","RODRICK","SWIM","IN","MY","POOL","OR","USE","MY","BOWLING","ALLEY","OR","ANYTHING","ELSE","LIKE","THAT"},
                new string[]{"TONIGHT","RODRICK","PULLED","HIS","GETOUTOFDOINGTHEDISHES","ROUTINE","JUST","LIKE","HE","DOES","EVERY","NIGHT","DAD","HAS","A","RULE","THAT","WERE","NOT","ALLOWED","TO","WATCH","TV","UNTIL","THE","DISHES","ARE","DONE","BUT","RIGHT","AFTER","DINNER","RODRICK","ALWAYS","GOES","UPSTAIRS","TO","THE","BATHROOM","AND","DOESNT","COME","DOWN","FOR","SOMETHING","LIKE",key[0] + "" + key[1], "MINUTES","BY","THAT","TIME","IVE","DONE","ALL","THE","DISHES","MYSELF"},
                new string[]{"WELL","TONIGHT","I","SAID","ENOUGH","IS","ENOUGH","AND","I","WENT","TO","COMPLAIN","TO","MOM","AND","DAD","BUT","OF","COURSE","RODRICK","HAD","AN","EXCUSE"},
                new string[]{"ALL","I","CAN","SAY","IS","THAT","IF","RODRICK","WANTS","TO","HANG","OUT","IN","MY","MANSION","WHEN","WERE","GROWN","UP","HE","BETTER","BRING","A","TOWEL","AND","SOME","SPONGES","BECAUSE","HES","GOING","TO","BE","DOING","A","WHOLE","LOT","OF","DISHES"},
                new string[]{"TONIGHT","SOCCER","PRACTICE","ENDED","A","FEW","MINUTES","EARLY","SO","THE","COACH","COULD","HAND","OUT","UNIFORMS","AND","WE","COULD","COME","UP","WITH","A","TEAM","NAME"},
                new string[]{"I","SUGGESTED","TWISTED","WIZARDS","AND","SAID","MAYBE","WE","COULD","GET","THE","GAME","ZONE","TO","SPONSOR","US","BUT","OF","COURSE","MY","PERFECTLY","GOOD","IDEA","GOT","SHOT","DOWN"},
                new string[]{"A","BUNCH","OF","OTHER","IDEAS","GOT","TOSSED","AROUND","UNTIL","SOME","IDIOT","CAME","UP","WITH","THE","NAME","RED","SOX","I","COULDNT","BELIEVE","IT","WHEN","A","BUNCH","OF","KIDS","THOUGHT","THAT","WAS","A","REALLY","GOOD","IDEA","AND","GUESS","WHAT","THATS","WHAT","EVERYONE","VOTED","ON"},
                new string[]{"NOW","NUMBER","ONE","THE","RED","SOX","IS","A","BASEBALL","TEAM","NOT","A","SOCCER","TEAM","AND","NUMBER","TWO","OUR","UNIFORMS","ARE","BLUE","INCLUDING","THE","SOCKS","BUT","OF","COURSE","NOBODY","WOULD","LISTEN","TO","ME"}
            }
        };
        return diary;
    }
}
