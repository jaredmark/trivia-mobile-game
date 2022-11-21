using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public static PlayerData data;

    public int coins;
    public int totalScore;
    public int highestScore;
    public float playTime;
    public Character equippedChar;
    public WeaponAugment equippedAugment;

    public int encounteredQuestions;
    public int correctAnswers;
    public int enemyKilled;
    public int highestStreak;
    public int currStreak;
    public int gamePlayed;

    public Dictionary<int, Character> ownedCharacters = new Dictionary<int, Character>();
    public Dictionary<int, WeaponAugment> ownedAugments = new Dictionary<int, WeaponAugment>();
    public Dictionary<int, Trophy> acquiredTrophies = new Dictionary<int, Trophy>();

    public int[] ownedCharactersID;
    public int[] ownedAugmentsID;
    public int[] acquiredTrophiesID;

    public int equippedCharacterID;
    public int equippedAugmentID;
    public bool hasEquippedAugment;

    public bool hasFinishedTutorial1;
    public bool hasFinishedTutorial2;

    public float volBGM;
    public float volSFX;

    public void AddCharacter(Character _char)
    {
        if (!ownedCharacters.ContainsKey(_char.id))
        {
            ownedCharacters.Add(_char.id, _char);
        }     
    }

    public void AddAugment(WeaponAugment aug)
    {
        if (!ownedAugments.ContainsKey(aug.id))
        {
            ownedAugments.Add(aug.id, aug);
        }       
    }

    public void AddTrophy(Trophy trophy)
    {
        if (!acquiredTrophies.ContainsKey(trophy.id))
        {
            acquiredTrophies.Add(trophy.id, trophy);
        }        
    }

    public void EquipCharacter(Character _char)
    {
        if (ownedCharacters.ContainsKey(_char.id))
        {
            equippedChar = _char;
            equippedCharacterID = _char.id;
            Debug.Log("Equipped! Char!");
        }
    }

    public void EquipAugment(WeaponAugment aug)
    {
        if (ownedAugments.ContainsKey(aug.id) || aug == null)
        {
            equippedAugment = aug;
            hasEquippedAugment = aug != null ? true : false;
            if (hasEquippedAugment) equippedAugmentID = equippedAugment.id;
        }
    }
}

public abstract class WeaponAugment
{
    public string name;
    public int id;
    public int cost;
    public Sprite icon;
    public Color emissionColor;
    public float emissionNoiseSpeed;
    public float emissionNoiseScale;
    public GameObject particlesPrefab;
    public GameObject attackHitPrefab;
    public AudioClip sfx;
}

public class WeaponAugment000 : WeaponAugment
{
    public WeaponAugment000()
    {
        name = "<color=#FF8B0E>Fire</color>";
        id = 0;
        cost = 20;
        icon = Resources.LoadAll<Sprite>("icon-aug")[1];
        emissionColor = new Color(1.135f, 0.565f, 0.35f);
        emissionNoiseSpeed = 0.05f;
        emissionNoiseScale = 35f;
        particlesPrefab = Resources.Load<GameObject>("Materials/Fire/ParticleFire");
        attackHitPrefab = Resources.Load<GameObject>("Particles/FireHit/PartFire");
        sfx = Resources.Load<AudioClip>("Sounds/AUGMENT-FIRE-HIT");
    }
}

public class WeaponAugment001 : WeaponAugment
{
    public WeaponAugment001()
    {
        name = "Wind";
        id = 1;
        cost = 20;
        icon = Resources.LoadAll<Sprite>("icon-aug")[3];
        emissionColor = new Color(0.475f, 1.0f, 0.715f);
        emissionNoiseSpeed = 0.1f;
        emissionNoiseScale = 35f;
        particlesPrefab = Resources.Load<GameObject>("Particles/Wind/ParticleWind");
        attackHitPrefab = Resources.Load<GameObject>("Particles/Wind/WindHit");
    }
}

public class WeaponAugment002 : WeaponAugment
{
    public WeaponAugment002()
    {
        name = "Darkness";
        id = 2;
        cost = 40;
        icon = Resources.LoadAll<Sprite>("icon-aug")[2];
        emissionColor = new Color(1.0f, 0f, 0.17f);
        emissionNoiseSpeed = 0.2f;
        emissionNoiseScale = 35f;
        particlesPrefab = Resources.Load<GameObject>("Particles/Darkness/ParticleDark");
        attackHitPrefab = Resources.Load<GameObject>("Particles/Darkness/DarkHit");
    }
}

public class WeaponAugment003 : WeaponAugment
{
    public WeaponAugment003()
    {
        name = "Light";
        id = 3;
        cost = 40;
        icon = Resources.LoadAll<Sprite>("icon-aug")[4];
        emissionColor = new Color(1.0f, 1.0f, 0f);
        emissionNoiseSpeed = 0.025f;
        emissionNoiseScale = 35f;
        particlesPrefab = Resources.Load<GameObject>("Particles/Light/ParticleLight");
        attackHitPrefab = Resources.Load<GameObject>("Particles/Light/LightHit");
    }
}

public abstract class Character
{
    public string name;
    public int id;
    public Sprite portrait;
    public int cost;
    public GameObject prefab;
    public AudioClip sfxAttack;
}

public class Character000 : Character
{
    public Character000()
    {
        name = "Ken";
        id = 0;
        cost = 0;
        prefab = Resources.Load<GameObject>("Player001");
        portrait = Resources.Load<Sprite>("Portraits/portrait-0");
        sfxAttack = Resources.Load<AudioClip>("Sounds/SWORD-SLASH");
    }
}

public class Character001 : Character
{
    public Character001()
    {
        name = "Jenny";
        id = 1;
        cost = 20;
        prefab = Resources.Load<GameObject>("Player002");
        portrait = Resources.Load<Sprite>("Portraits/portrait-1");
        sfxAttack = Resources.Load<AudioClip>("Sounds/BOW");
    }
}

public class Character002 : Character
{
    public Character002()
    {
        name = "Jack";
        id = 2;
        cost = 60;
        prefab = Resources.Load<GameObject>("Player003");
        portrait = Resources.Load<Sprite>("Portraits/portrait-2");
        sfxAttack = Resources.Load<AudioClip>("Sounds/SPEAR-HIT");
    }
}

public abstract class Trophy
{
    public string name;
    public int id;
    public Sprite sprite;

    public virtual string Condition { get; }
}

public class Trophy000 : Trophy
{
    public Trophy000()
    {
        name = "The Challenger";
        id = 0;
        sprite = Resources.LoadAll<Sprite>("trophies-2")[2];
    }

    public override string Condition => "Complete a game.";
}

public class Trophy001 : Trophy
{
    public Trophy001()
    {
        name = "The Perfectionist";
        id = 1;
        sprite = Resources.LoadAll<Sprite>("trophies-2")[0];
    }

    public override string Condition => "Complete a game by answering all questions correctly.";
}

public class Trophy002 : Trophy
{
    public int minutes;

    public Trophy002()
    {
        name = "The Lightning";
        id = 2;
        sprite = Resources.LoadAll<Sprite>("trophies-2")[1];
        minutes = 5;
    }

    public override string Condition => "Complete a game in under " + 5 + " minutes.";
}

public class Trophy003 : Trophy
{
    public int consec;

    public Trophy003()
    {
        name = "The Brainiac";
        id = 3;
        sprite = Resources.LoadAll<Sprite>("trophies-2")[3];
        consec = 100;
    }

    public override string Condition => "Consecutively answer " + consec + " questions correctly.";
}

public class Trophy004 : Trophy
{
    public int slayCount;

    public Trophy004()
    {
        name = "The Slayer";
        id = 4;
        sprite = Resources.LoadAll<Sprite>("trophies-2")[4];
        slayCount = 30;
    }

    public override string Condition => "Slay " + slayCount + " monsters.";
}

public abstract class Question 
{
    public enum Subject { LogicFormulation, ProblemSolving, None }
    public enum Type { TrueOrFalse, MultipleChoice, Identification }
    public const string highlightTagStart = "<color=#7E4000>";
    public const string highlightTagEnd = "</color>";

    public string title;
    public string explanation;
    public string answer;
    public bool isSample;
    public Sprite img;
    public Subject subj;
    public List<Question> otherQFormat = new List<Question>();
}

public class QuestionTF : Question
{
    public string questionT; //question if the answer is true
    public string questionF;
}

public class QuestionMult : Question
{
    public bool willJumble;

    public string question;
    public List<string> choices = new List<string>();
}

public class QuestionJumbleWrite : Question
{
    public string question;
    public bool willShowClue = true;
}

public class Question000 : QuestionTF
{
    public Question000()
    {
        subj = Subject.None;
        questionT = "This is a sample question. (Answer: True)";
        questionF = "This is a sample question. (Answer: False)";
        isSample = true;
    }
}

public class Question001 : QuestionMult
{
    public Question001()
    {
        subj = Subject.None;
        question = "This is a sample question. (Answer: A)";
        choices = new List<string>() { "A", "B", "C", "D" };
        answer = "A";
        isSample = true;
    }
}

public class Question002 : QuestionJumbleWrite
{
    public Question002()
    {
        subj = Subject.None;
        question = "This is a sample question. (Answer: SampleAnswer)";
        answer = "SampleAnswer";
        isSample = true;
    }
}

public class Question003 : QuestionMult
{
    public Question003()
    {
        subj = Subject.None;
        question = "This is a sample question. (Answer: axolotl)";
        answer = "axolotl";
        choices = new List<string>() { "axolotl", "frog", "wolf", "stingray"};
        willJumble = true;
        isSample = true;
    }
}

public class Question004 : QuestionTF
{
    public Question004()
    {
        title = "Computer Software";
        explanation = "Refers to all computer programs or routines which direct and control the computer hardware in the performance of its data processing functions.";

        questionT = highlightTagStart + "Computer Software" + highlightTagEnd + " refers to all" +
            " computer programs which direct and control computer hardware.";
        questionF = highlightTagStart + "Operating System" + highlightTagEnd + " refers to all computer" +
            " programs which direct and control computer hardware.";
    }
}

public class Question005 : QuestionTF
{
    public Question005()
    {
        title = "Major Categories of Software";
        explanation = "-Application Software\n-System Software";

        questionT = "The two major categories of software are " + highlightTagStart + "Application " +
            " Software" + highlightTagEnd + " and " + highlightTagStart + "System Software" + highlightTagEnd;
        questionF = "The two major categories of software are " + highlightTagStart + "Computer " +
            " Software" + highlightTagEnd + " and " + highlightTagStart + "System Software" + highlightTagEnd;
    }
}

public class Question006 : QuestionTF
{
    public Question006()
    {
        title = "Application Software";
        explanation = "A software that perform a specific task for computer users (e.g., Word processing).";

        questionT = highlightTagStart + "Application Software" + highlightTagEnd + " performs a specific task for computer users (e.g. Word processing).";
        questionF = highlightTagStart + "Application Software" + highlightTagEnd + " controls the system hardware to perform a specific task for computer users (e.g. Keyboard input)";
    }
}

public class Question007 : QuestionTF
{
    public Question007()
    {
        title = "System Software";
        explanation = "A software that was designed to help programmers or to control the computer system (e.g.OS; programming tools: translators, debuggers, editors).";

        questionT = highlightTagStart + "System Software" + highlightTagEnd + " was designed to control the computer system (e.g. Operating System)";
        questionF = highlightTagStart + "System Software" + highlightTagEnd + " was designed to allow computer users to perform a specific task in a computer (e.g. Word processing)";
    }
}

public class Question008 : QuestionTF
{
    public Question008()
    {
        title = "Program";
        explanation = "List of computer instructions required to arrive at the desired results";

        questionT = "A " + highlightTagStart + "program" + highlightTagEnd + " is a list of computer instructions required to arrive at the desired results";
        questionF = "A " + highlightTagStart + "sequence" + highlightTagEnd + " is a list of computer instructions required to arrive at the desired results";
    }
}

public class Question009 : QuestionTF
{
    public Question009()
    {
        title = "Algorithm";
        explanation = "A step-by-step list of instructions for solving a problem";

        questionT = "An " + highlightTagStart + "algorithm" + highlightTagEnd + " is a step-by-step list of instructions for solving a problem";
        questionF = "A " + highlightTagStart + "pseudocode" + highlightTagEnd + " is a step-by-step list of instructions for solving a problem";
    }
}

public class Question010 : QuestionTF
{
    public Question010()
    {
        title = "Programming Language";
        explanation = "A formalized notation that allows algorithms to be presented in a rigorous and precise way.";

        questionT = highlightTagStart + "Programming language" + highlightTagEnd + " is a formalized notation that allows algorithms to be presented in a rigorous and precise way.";
        questionF = highlightTagStart + "Coding" + highlightTagEnd + " is a formalized notation that allows algorithms to be presented in a rigorous and precise way.";
    }
}

public class Question011 : QuestionTF
{
    public Question011()
    {
        title = "Flowchart";
        explanation = "A system of symbols for expressing algorithms; indicates flow of control/ sequence of operations.";

        questionT = "A " + highlightTagStart + "flowchart" + highlightTagEnd + " is a system of symbols for expressing algorithms; indicates flow of control/ sequence of operations.";
        questionF = "A " + highlightTagStart + "flowchart" + highlightTagEnd + " is a system of symbols for expressing the flow of data in an information system.";
    }
}

public class Question012 : QuestionTF
{
    public Question012()
    {
        title = "Pseudocode";
        explanation = "A textual representation of an algorithm; close to natural language; becomes part of the program documentation.";

        questionT = highlightTagStart + "Pseudocode" + highlightTagEnd + " is a textual representation of an algorithm; close to natural language";
        questionF = highlightTagStart + "Pseudocode" + highlightTagEnd + " is a set of instructions to direct the computer to perform a specific task.";
    } 
}

public class Question013 : QuestionJumbleWrite
{
    public Question013()
    {
        title = "Computational Problems";
        explanation = "A problem that can be solved on a computer which involves mathematical processing. (e.g. Calculation of a student's average grade)";

        question = "Problems involving some kind of mathematical processing.";
        answer = "Computational";
    }
}

public class Question014 : QuestionJumbleWrite
{
    public Question014()
    {
        title = "Logical Problems";
        explanation = "A problem that can be solved in a computer which involves relational or logical processing. (e.g. Determining whether a person is over 18 yrs. old or not)";

        question = "Problems involving relational or logical processing.";
        answer = "Logical";
    }
}

public class Question015 : QuestionJumbleWrite
{
    public Question015()
    {
        title = "Repetitive Problems";
        explanation = "A problem that can be solved in a computer that involves repeating a set of mathematical and/or logical instructions. (e.g. Calculating the grades of over 100 students)";

        question = "Problems that involves repeating a set of mathematical and/or logical instructions.";
        answer = "Repetitive";
    }
}

public class Question016 : QuestionTF
{
    public Question016()
    {
        title = "Logical Control Structures";
        explanation = "This specifies the flow of control of a program. The 4 logical control structures are:\n-Sequence\n-Selection/Decision\n-Iteration/Loop\n-Case";

        questionT = highlightTagStart + "Logical Control Structures" + highlightTagEnd + " specifies the flow of control of a program.";
        questionF = highlightTagStart + "Conditional Statements" + highlightTagEnd + " specifies the flow of control of a program.";
    }
}

public class Question017 : QuestionTF
{
    public Question017()
    {
        title = "Sequence";
        explanation = "Instructions which are executed in the order which they appear.\n\"Step-by-step\" execution of instructions.";

        questionT = "A " + highlightTagStart + "sequence" + highlightTagEnd + " is a set of instructions that are executed in the order which they appear.";
        questionF = "A " + highlightTagStart + "sequence" + highlightTagEnd + " is a set of conditions that controls the flow of a program.";
    }
}

public class Question018 : QuestionTF
{
    public Question018()
    {
        title = "Selection/Decision";
        explanation = "A logical control structure that execute instructions depending on the existence of a condition. Sometimes called an \"If-then-else\" logical control structure.";

        questionT = "A " + highlightTagStart + " selection/decision" + highlightTagEnd + " is a logical control structure that execute instructions depending on the existence of a condition.";
        questionF = "A " + highlightTagStart + " conditional statement " + highlightTagEnd + " is a logical control structure that execute instructions depending on the existence of a condition.";
    }
}

public class Question019 : QuestionMult
{
    public Question019()
    {
        title = "Data types";
        explanation = "Types of data are:\n-Numeric\n-Character\n-Logical\n-Date/Time";

        question = "The following are types of data except: ";
        choices.Add("Numeric");
        choices.Add("Character");
        choices.Add("Logical");
        choices.Add("Language");

        answer = "Language";
    }
}

public class Question020 : QuestionMult
{
    public Question020()
    {
        title = "Numerical Data";
        explanation = "A data type that can be used in calculations.\nSubtypes:\n-Integer\n-Float\n-Real";

        question = "Which of the following is a numerical data?";
        choices.Add("Boolean");
        choices.Add("Integer");
        choices.Add("String");
        choices.Add("Array");

        answer = "Integer";
    }
}

public class Question021 : QuestionMult
{
    public Question021()
    {
        title = "Character Data";
        explanation = "Consists of all numbers, letters, and special characters available to the computer(#, &, *, +, -, 0 - 9, A - Z, a - z) and placed within quotation marks.";

        question = "Which of the following is a character data?";
        choices.Add("f");
        choices.Add("true");
        choices.Add("Hello World!");
        choices.Add("3.14f");

        answer = "f";
    }
}

public class Question022 : QuestionMult
{
    public Question022()
    {
        title = "Logical Data";
        explanation = "Consist of two pieces of data in the data set � the words TRUE and FALSE. \n� Logical data are used in making a yes or no decision.";

        question = "Which of the following is a logical data?";
        choices.Add("true");
        choices.Add("100");
        choices.Add("if-else");
        choices.Add("==");

        answer = "true";
    }
}

public class Question023 : QuestionJumbleWrite
{
    public Question023()
    {
        title = "Operators";
        explanation = "� Are the data connectors within expressions and equations. \n� They tell the computer how to process the data. \n� They also tell the computer what type of processing needs to be done(i.e., mathematical, relational, or logical).";

        question = "They tell the computer how to process the data and are the data connectors within expression and equations.";
        answer = "Operators";
    }
}

public class Question024 : QuestionMult
{
    public Question024()
    {
        title = "Mathematical Operators";
        explanation = "Are operators used between operands to perform mathematical calculations. (e.g. + for addition, - for subtraction, * for multiplications, / for division, etc.)";

        question = "Mathematical operators are used between operands to perform _________";
        choices.Add("Calculations");
        choices.Add("Comparisons");
        choices.Add("Assignments");
        choices.Add("Concatenations");

        answer = "Calculations";
    }
}

public class Question025 : QuestionMult
{
    public Question025()
    {
        title = "Relational Operators";
        explanation = "These operators are used to compare numeric, character string, or logical data. These are used to make a decision regarding program flow (e.g. == for comparison, < if the value is less than)";

        question = "Relational operators are used between operands to perform _________";
        choices.Add("Calculations");
        choices.Add("Comparisons");
        choices.Add("Assignments");
        choices.Add("Concatenations");

        answer = "Comparisons";
    }
}

public class Question026 : QuestionMult
{
    public Question026()
    {
        title = "Logical Operators";
        explanation = "These are used to connect relational expressions (decision-making expressions) & to perform operations on logical data. (e.g. && for AND, ! for NOT, || for OR, etc.)";

        question = "Which of the following is not a logical operator?";
        choices.Add("&&");
        choices.Add("||");
        choices.Add("==");
        choices.Add("!");

        answer = "==";
    }
}

public class Question027 : QuestionTF
{
    public Question027()
    {
        title = "Process Symbol";

        questionT = "The flowchart symbol in the image is a " + highlightTagStart + "process symbol" + highlightTagEnd;
        questionF = "The flowchart symbol in the image is an " + highlightTagStart + "input/output symbol" + highlightTagEnd;
    }
}
public class Question028 : QuestionTF
{
    public Question028()
    {
        title = "Input/Output Symbol";

        questionF = "The flowchart symbol in the image is a " + highlightTagStart + "process symbol" + highlightTagEnd;
        questionT = "The flowchart symbol in the image is an " + highlightTagStart + "input/output symbol" + highlightTagEnd;
    }
}

public class Question029 : QuestionTF
{
    public Question029()
    {
        title = "Flowline Symbol";

        questionF = "The flowchart symbol in the image is a " + highlightTagStart + "dataflow symbol" + highlightTagEnd;
        questionT = "The flowchart symbol in the image is an " + highlightTagStart + "flowline symbol" + highlightTagEnd;
    }
}

public class Question030 : QuestionMult
{
    public Question030()
    {
        title = "Decision Symbol";

        question = "What flowchart symbol is in the image?";

        choices.Add("Decision symbol");
        choices.Add("Connector Symbol");
        choices.Add("Terminal Symbol");
        choices.Add("Branching Symbol");

        answer = choices[0];
    }
}

public class Question031 : QuestionMult
{
    public Question031()
    {
        title = "Terminal Symbol";

        question = "What flowchart symbol is in the image?";

        choices.Add("Import symbol");
        choices.Add("Connector Symbol");
        choices.Add("Terminal Symbol");
        choices.Add("Branching Symbol");

        answer = choices[2];
    }
}

public class Question032 : QuestionMult
{
    public Question032()
    {
        title = "Connector Symbol";

        question = "What flowchart symbol is in the image?";

        choices.Add("Import symbol");
        choices.Add("Connector Symbol");
        choices.Add("Terminal Symbol");
        choices.Add("Branching Symbol");

        answer = choices[1];
    }
}

public class Question033 : QuestionTF
{
    public Question033()
    {
        title = "Flowchart Sample #1";

        explanation = "Grade is not less than 75.";
        img = Resources.Load<Sprite>("QuestionImages/033");

        questionT = "Will the decision symbol in the flowchart will result in a NO?";
        questionF = "Will the decision symbol in the flowchart will result in a YES?";
    }
}

public class Question034 : QuestionMult
{
    public Question034()
    {
        title = "Basic Elements of Programming";
        explanation = "-Data\n-Input\n-Output\n-Operations\n-Conditions\n-Loops\n-Subroutines/Modules";

        question = "Which is not a basic element of programming?";

        choices.Add("Data");
        choices.Add("Conditions");
        choices.Add("Output");
        choices.Add("Sequence");

        answer = choices[3];
    }
}

public class Question035 : QuestionTF
{
    public Question035()
    {
        title = "Data";

        explanation = "Data are unorganized facts.";

        questionT = "Data are unorganized facts.";
        questionF = "Data are organized facts.";
    }
}

public class Question036 : QuestionTF
{
    public Question036()
    {
        title = "Constants";
        explanation = "A value that never changes during the processing of all the instructions in a solution.";

        questionT = "The value of a constant can't be changed.";
        questionF = "The value of a constant can be changed.";
    }
}

public class Question037 : QuestionTF
{
    public Question037()
    {
        title = "Variables";
        explanation = "The value of a variable does change during processing.";

        questionF = "The value of a variable can't be changed.";
        questionT = "The value of a variable can be changed.";
    }
}

public class Question038 : QuestionJumbleWrite
{
    public Question038()
    {
        title = "Flowchart Sample #2";
        explanation = "The flowchart prints \"Hello World\" 10 times.";

        question = "In the flowchart, how many times is \"Hello World\" printed?";
        img = Resources.Load<Sprite>("QuestionImages/038");
        answer = "10";
    }
}

public class Question039 : QuestionTF
{
    public Question039()
    {
        title = "Expressions";
        explanation = "An expression processes data (the operands) through the use of operators. (e.g. A + B, A < B, A && B)";

        questionT = highlightTagStart + "A + B" + highlightTagEnd + " is an expression.";
        questionF = highlightTagStart + "A = B" + highlightTagEnd + " is an expression.";
    }
}

public class Question040 : QuestionTF
{
    public Question040()
    {
        title = "Equations";
        explanation = "An equation stores the resultant of an expression in a memory location in the computer or in a variable through the equal sign( = ).\n�Equations are often called �assignment statements.� (e.g. A = B, A = B + C, A = B && C)";

        questionT = highlightTagStart + "A = B * C" + highlightTagEnd + " is an equation";
        questionF = highlightTagStart + "A + B * C" + highlightTagEnd + " is an equation";
    }
}

public class Question041 : QuestionMult
{
    public Question041()
    {
        title = "Flowchart Sample #3";
        explanation = "Variable A will have a value of 13 (following the rule of PEMDAS) in the first process symbol. Then at the decision symbol, since 13 is less than 15 it will go to the YES flow point. After that, \"The answer is 13\" is printed.";

        img = Resources.Load<Sprite>("QuestionImages/041");
        question = "In the flowchart, what will be printed?";

        choices.Add("The answer is 13");
        choices.Add("The answer is 16");
        choices.Add("The result is 13");
        choices.Add("The result is 16");

        answer = choices[0];
    }
}

public class Question042 : QuestionMult
{
    public Question042()
    {
        title = "Flowchart Sample #4";
        explanation = "Variable N will have a value of 4 (following the rule of PEMDAS) in the first process symbol. Since 4 is greater than 0, the condition will result in a YES. Finally, \"N is equal to 4\" will be printed.";

        img = Resources.Load<Sprite>("QuestionImages/042");
        question = "In the flowchart, what will be printed?";

        choices.Add("N is equal to 4");
        choices.Add("N is equal to 0");
        choices.Add("The value of N is 4");
        choices.Add("The value of N is 0");

        answer = choices[0];
    }
}

public class Question043 : QuestionMult
{
    public Question043()
    {
        title = "Flowchart Sample #5";
        explanation = "Since the age is equal to 16 and it is less than 18, the first decision symbol will result to a NO. At the proceeding condition, since age is not greater than 16, it will result to a NO and remark will be assigned a value of \"adult\".";

        img = Resources.Load<Sprite>("QuestionImages/043");
        question = "In the flowchart, what will the value of the variable 'remark'?";

        choices.Add("student");
        choices.Add("underage");
        choices.Add("teenager");
        choices.Add("adult");

        answer = choices[3];
    }
}

public class Question044 : QuestionMult
{
    public Question044()
    {
        title = "Flowchart Sample #6";
        explanation = "The flowchart starts with a variable 'count' instantiated with a value of 0. Then at the second process, variable 'A' is also instantiated with a value of 1. At the decision symbol, it checks whether the value of 'count' variable is greater than 3. Since 'count' is not greater than 3 yet, it will go to the NO flow line where it leads to the process that adds 1 to 'count' and multiplies the value of 'A' to 2. After that, it leads to the process again which a value of 1 is assigned to 'A'. This process will repeat itself again until 'count' is greater than 3 but even if the value of 'A' is multiplied by 2, it is assigned with a value of 1 right after it so its value will still remain 1 in the end.";

        img = Resources.Load<Sprite>("QuestionImages/044");
        question = "What will be the output of the flowchart?";

        choices.Add("The value of A is 0");
        choices.Add("The value of A is 1");
        choices.Add("The value of A is 8");
        choices.Add("The value of A is 16");

        answer = choices[1];
    }
}

public class Question045 : QuestionMult
{
    public Question045()
    {
        title = "Flowchart Sample #7";
        explanation = "The flowchart starts with variables 'count' and 'N' with a value of 0. At the second process, 'count' is increased by 1 and 'N' is increased by 2. Their values will keep increasing until 'count' is equal or greater than 2. Finally, 'N' will have a value of 4.";

        img = Resources.Load<Sprite>("QuestionImages/045");
        question = "What will be the output of the flowchart?";

        choices.Add("N = 0");
        choices.Add("N = 2");
        choices.Add("N = 4");
        choices.Add("N = 6");

        answer = choices[2];
    }
}

public class Question046 : QuestionJumbleWrite
{
    public Question046()
    {
        title = "Flowchart Sample #8";
        explanation = "The flowchart starts with the creation of variables 'isAdult' with a value of false and age with a value of 21. At the decision symbol it checks if 'isAdult' is equal to true AND if age is greater than or equal to 21 which results to NO because both conditions are not true. Then, \"You are underaged\" will be printed.";

        img = Resources.Load<Sprite>("QuestionImages/046");
        question = "What will be printed with this flowchart?";
        answer = "You are underaged";
    }
}

public class Question047 : QuestionMult
{
    public Question047()
    {
        title = "Flowchart Sample #9";
        explanation = "\"I love programming\" will be printed 7 times.";

        img = Resources.Load<Sprite>("QuestionImages/047");
        question = "How many times will \"I love programming\" be printed in this flowchart?";
        choices.Add("7");
        choices.Add("6");
        choices.Add("9");
        choices.Add("10");
        answer = choices[0];
    }
}

public class Question048 : QuestionMult
{
    public Question048()
    {
        title = "Flowchart Sample #10";
        explanation = "The variable 'n' will have a value of 200 at the termination of this flowchart.";

        img = Resources.Load<Sprite>("QuestionImages/048");
        question = "What will be the final value of variable 'n' at the termination of this flowchart?";
        choices.Add("1000");
        choices.Add("100");
        choices.Add("200");
        choices.Add("1100");
        answer = choices[2];
    }
}

public class Question049 : QuestionJumbleWrite
{
    public Question049()
    {
        title = "Flowchart Sample #11";
        explanation = "Following the rule of PEMDAS, the value of 'average' will be 700 if 'income' is equal to 300.";

        img = Resources.Load<Sprite>("QuestionImages/049");
        question = "What is the final value of 'average' if 'income' is equal to 300?";

        answer = "700";
    }
}

public class Question050 : QuestionMult
{
    public Question050()
    {
        title = "Flowchart Sample #12";
        explanation = "The flowchart starts with a variable 'n' equal to 6. If 'n' is not greater than 28, the value of 'n' will keep increasing by 10 until it is greater than 28 which will print the value of 36.";

        img = Resources.Load<Sprite>("QuestionImages/050");
        question = "What will be the final value of variable 'n' at the termination of this flowchart?";
        choices.Add("28");
        choices.Add("26");
        choices.Add("30");
        choices.Add("36");
        answer = choices[3];
    }
}

public class Question051 : QuestionMult
{
    public Question051()
    {
        title = "Flowchart Sample #13";
        explanation = "At the first condition, since 'cash'(1000) is less than 2000, the decision symbol will proceed to the YES flowline. Then, 'cash' is less than 1200 but not greater than 1500. Since the OR operator was used in the condition, it will still result in a YES if at least one of the conditions is true. Then the action \"Go to diner\" will be executed.";

        img = Resources.Load<Sprite>("QuestionImages/051");
        question = "What action will be executed in the flowchart?";
        choices.Add("Buy shoes");
        choices.Add("Watch cinema");
        choices.Add("Play arcade");
        choices.Add("Go to diner");
        answer = choices[3];
    }
}

public class Question052 : QuestionJumbleWrite
{
    public Question052()
    {
        title = "Flowchart Sample #14";
        explanation = "In order for this flowchart to print \"Good morning!\", the value of 'gender' should be \"female\"";

        img = Resources.Load<Sprite>("QuestionImages/052");
        question = "What should be the value of 'female' in order for this flowchart to print \"Good morning!\"?";

        answer = "female";
    }
}

public class Question053 : QuestionJumbleWrite
{
    public Question053()
    {
        title = "Flowchart Sample #15";
        explanation = "\"Diamond\" will only be printed once since the 'count' is less than 100 which will hault the flowchart immediately.";

        img = Resources.Load<Sprite>("QuestionImages/053");
        question = "How many times \"Diamond\" will be printed?";

        answer = "1";
    }
}

public class Question054 : QuestionJumbleWrite
{
    public Question054()
    {
        title = "Flowchart Sample #16";
        explanation = "The variable 'count' starts with a value of 2 then \"Diamond\" was printed. The flowchart will keep adding 2 to 'count' and printing \"Diamond\" until 'count' is over 100. Therefore, it will print 51 times.";

        img = Resources.Load<Sprite>("QuestionImages/054");
        question = "How many times \"Diamond\" will be printed?";

        answer = "51";
    }
}

public class Question055 : QuestionJumbleWrite
{
    public Question055()
    {
        title = "Flowchart Sample #17";
        explanation = "The value of 'n1' should be 59 in order for 'n2' to be 158.";

        img = Resources.Load<Sprite>("QuestionImages/055");
        question = "What should be the value of 'n1' in order for the flowchart to print \"You are correct!\"?";

        answer = "59";
    }
}

public class Question056 : QuestionJumbleWrite
{
    public Question056()
    {
        title = "Flowchart Sample #18";
        explanation = "The value of 'n1' should be 11 in order for 'n2' to be 29.";

        img = Resources.Load<Sprite>("QuestionImages/056");
        question = "What should be the value of 'n1' in order for the flowchart to print \"You are correct!\"?";

        answer = "11";
    }
}

public class Question057 : QuestionTF
{
	public Question057()
	{
		title = "Computer";
        subj = Subject.ProblemSolving;
		explanation = "A computer is an electronic device that inputs (takes in) facts (known as data), and then processes (does something to or with) it.";
		
		questionT ="A computer is an electronic device that inputs facts and processes it.";
		questionF = "A CPU is am electronic device that inputs facts and processes it.";
	}
}

public class Question058 : QuestionTF
{
	public Question058()
	{
		title = "Main parts of a computer";
        subj = Subject.ProblemSolving;
        explanation = "There are two main parts of computers, hardware and software.";
		
		questionT = "The two main parts of a computer are hardware and software.";
		questionF = "The two main parts of a computer are hardware and appware";
	}
}

public class Question059 : QuestionTF
{
	public Question059()
	{
		title = "Computer Hardware";
        subj = Subject.ProblemSolving;
        explanation = "Computer hardware includes the physical parts of a computer, such as the case, central processing unit, random access memory,";
		
		questionT = "Computer hardware includes the physical parts of a computer";
		questionF = "Computer hardware includes the applications you use in a computer.";
	}
}

public class Question060 : QuestionTF
{
	public Question060()
	{
		title = "Computer Software";
        subj = Subject.ProblemSolving;
        explanation = "Software is a set of instructions, data or programs used to operate computers and execute specific tasks. (e.g. MS Office, Web browsers, Photoshop etc.)";
		
		questionT = "Software is a set of instructions, data or programs used to operate computers and execute specific tasks.";
		questionF = "Software are the physical components a computer can use to perform tasks in a computer.";
	}
}


public class Question061 : QuestionTF
{
	public Question061()
	{
		title = "Computer Data";
        subj = Subject.ProblemSolving;
        explanation = "Computers store all data in binary code, which is a number system that only uses ones and zeros.";
		
		questionT = "Computers store all data in binary code, which is a number system that only uses ones and zeros.";
		questionF = "Computers store all data in hexadecimal code, which is a number system that only uses ones and zeros.";
	}
}

public class Question062 : QuestionMult 
{
	public Question062()
	{
		title = "Supercomputers";
        subj = Subject.ProblemSolving;
        explanation = "They are the biggest and fastest computers(in terms of speed of processing data). Supercomputers are designed such that they can process a huge amount of data, like processing trillions of instructions or data just in a second.";
		
		question = "These are designed such that they can process a huge amount of data, like processing trillions of instructions or data just in a second";
		choices.Add("Supercomputers");
		choices.Add("Mainframe");
		choices.Add("Personal Computer");
		choices.Add("Workstation");
		
		answer = choices[0];
		
		//img = Resources.Load<Sprite>("QuestionImages/062");
	}
}

public class Question063 : QuestionMult 
{
	public Question063()
	{
		title = "Mainframe computers";
        subj = Subject.ProblemSolving;
        explanation = "Mainframe computers are designed in such a way that it can support hundreds or thousands of users at the same time. It also supports multiple programs simultaneously. They are mainly used for business.";
		
		question = "These computers are designed in such a way that it can support hundreds or thousands of users at the same time.";
		choices.Add("Supercomputers");

		choices.Add("Mainframe");
		choices.Add("Personal Computer");
		choices.Add("Workstation");
		
		answer = choices[1];
		
		//img = Resources.Load<Sprite>("QuestionImages/063");
	
		
	}
}

public class Question064 : QuestionMult
{
	public Question064()
	{
		title = "Workstation";
        subj = Subject.ProblemSolving;
        explanation = "Workstation is designed for technical or scientific applications. It consists of a fast microprocessor, with a large amount of RAM and high speed graphic adapter.";
		
		question = "Designed for technical or scientific applications. It consists of a fast microprocessor, with a large amount of RAM and high speed graphic adapter.";
		
		choices.Add("Supercomputers");
		choices.Add("Mainframe");
		choices.Add("Personal Computer");
		choices.Add("Workstation");
		
		answer = choices[3];
		
		//img = Resources.Load<Sprite>("QuestionImages/064");
	
	}
}

public class Question065 : QuestionMult
{
	public Question065()
	{
		title = "Input Devices";
        subj = Subject.ProblemSolving;
        explanation = "An input device is a piece of equipment used to provide data and control signals to an information processing system";
		question = "Which of the following is not an input device?";
		
		choices.Add("Printer");
		choices.Add("Mouse");
		choices.Add("Scanner");
		choices.Add("Microphone");
		
		answer = choices[0];
	}
}

public class Question066 : QuestionMult
{
	public Question066()
	{
		title = "Output Device";
        subj = Subject.ProblemSolving;
        explanation = "An output device is any hardware device used to send data from a computer to another device or user.";
		
		question = "Which of the following is not an output device?";
		choices.Add("Keyboard");
		choices.Add("Monitor");
		choices.Add("Speaker");
		choices.Add("Printer");
		
		answer = choices[0];
	}
}

public class Question067 : QuestionTF 
{
	public Question067()
	{
		title = "Problem Solving";
        subj = Subject.ProblemSolving;
        explanation = "Problem Solving is the sequential process of analyzing information related to a given situation and generating appropriate response options.";
		
		questionT = "Problem Solving is the sequential process of analyzing information related to a given situation and generating appropriate response options.";
		questionF = "Problem Identification is the sequential process of analyzing information related to a given situation and generating appropriate response options.";
	}
}

public class Question068 : QuestionTF
{
	public Question068()
	{
		title = "6 Steps in Problem Solving";
        subj = Subject.ProblemSolving;
        explanation = "There are 6 steps that you should follow in order to solve a problem: \n1. Understand the Problem \n2. Formulate a Model \n3. Develop an Algorithm \n4. Write the Program \n5. Test the Program \n6. Evaluate the Solution";
	
		questionT = "The first step in problem solving is Understand the Problem.";
		questionF = "The first step in problem solving is Formulate a Model.";
	}
}

public class Question069 : QuestionMult
{
	public Question069()
	{
		title = "Understand the Problem";
        subj = Subject.ProblemSolving;
        explanation = "The first step to solving any problem is to make sure that you understand the problem that you are trying to solve. You need to know: \no What input data/information is available ?"
        + "\no What does it represent ? \no What format is it in ? \no Is anything missing ? \no Do I have everything that I need ? \no What output information am I trying to produce ? \no What do I want the result to look like … text, a picture, a graph … ? \no What am I going to have to compute ?";
	
		question = "The step in problem solving where you need to know what you will be dealing with.";
		choices.Add("Understanding the Problem");
		choices.Add("Formulate a Model");
		choices.Add("Develop an Algorithm");
		choices.Add("Write the Program");
		
		answer = choices[0];
	}
}

public class Question070 : QuestionJumbleWrite
{
	public Question070()
	{
		title = "Compiling";
        subj = Subject.ProblemSolving;
        explanation = "Compiling is the process of converting a program into instructions that can be understood by the computer.";
		
		question = "_______ is the process of converting a program into instructions that can be understood by the computer.";
		answer = title;
	}
}

public class Question071 : QuestionJumbleWrite
{
	public Question071()
	{
		title = "Running a Program";
        subj = Subject.ProblemSolving;
        explanation = "Running a program is the process of telling the computer to evaluate the compiled instructions.";
		question = "______ a program is the process of telling the computer to evaluate the compiled instructions.";
		answer = "Running";
	}
}

public class Question072 : QuestionJumbleWrite
{
	public Question072()
	{
		title = "Bugs";
        subj = Subject.ProblemSolving;
        explanation = "Bugs are problems/errors with a program that cause it to stop working or produce incorrect or undesirable results.";
		question = "___ are problems/errors with a program that cause it to stop working or produce incorrect or undesirable results.";
	
		answer = "bugs";
	}
	
}

public class Question073 : QuestionTF
{
	public Question073()
	{
		title = "The Five Generation of Computers";
        subj = Subject.ProblemSolving;
        explanation = "Each generation of computer is characterized by a major technological development that fundamentally changed the way computers operate, resulting in increasingly smaller, cheaper, more powerful and more efficient and reliable devices." +
		"1st Generation (1940 - 1956) - Vacuum Tubes\n2nd Generation - 1956-1963: Transistors\n3rd Generation - 1964-1971: Integrated Circuits\n4th Generation - 1971-Present: Microprocessors\n5th Generation - Present and Beyond: Artificial Intelligence";
		
		questionT = "The 3rd generation of computers use Integrated Circuits.";
		questionF = "The 1st generation of computers use Transistors.";
	}
}

public class Question074 : QuestionTF
{
	public Question074()
	{
		title = "1st Gen - Vacuum Tubes";
        subj = Subject.ProblemSolving;
        explanation = "The first computers used vacuum tubes for circuitry and magnetic drums for memory. Their characteristics are: " +
"\n Very big, taking up entire rooms \n Very expensive to operate \n Using a great deal of electricity \n Generated a lot of heat \n Often malfunctions \n Relied on machine language to perform operations \n Able to solve only one problem at a time.";
	
		questionT = "Vacuum tube computers are described as large as a room, very expensive, and generates a lot of heat.";
		questionF = "Transistor computers are described as large as a room, very expensive, and generates a lot of heat.";
	}
}

public class Question075 : QuestionTF
{
	public Question075()
	{
		title = "2nd Gen - Transistors";
        subj = Subject.ProblemSolving;
        explanation = "Transistors replaced vacuum tubes and ushered in the second generation of computers. Their characteristics are: " +
"\n The computers become smaller \n They are faster, cheaper and are more energy-efficient \n They are more reliable than the first-generation computers. \n They used assembly language to perform operations \n Generated lesser heat \n Second-generation computers still relied on punched cards for input and printouts for output.";
	
		questionT = "Transistors replaced vacuum tubes that made computers smaller, faster, cheaper, and more energy-efficient.";
		questionF = "Integrated circuits replaced vacuum tubes that made computers smaller, faster, cheaper, and more energy-efficient.";
		
	}
}

public class Question076 : QuestionMult
{
	public Question076()
	{
		title = "3rd Gen - Integrated Circuits";
        subj = Subject.ProblemSolving;
        explanation = "The development of the integrated circuit was the hallmark of the third generation of computers. Transistors were miniaturized and placed on silicon chips, called semiconductors. Characteristics of computers in this generation are:" +
		"\n\nThey were smaller and cheaper than their predecessors. \n Drastical increase in speed \n The computers are highly efficient \n Keyboard is used as input device \n Monitor and printouts are used for output " +
"\n\nUsers interacted with the third generation computers through an operating system, which allowed the device to run many different applications at one time with a central program that monitored the memory. Computers for the first time became accessible to a mass audience because.";
	
		question = "Computers that use these first introduced operating systems which allowed users to run multiple programs at the same time.";
		
		choices.Add("Integrated Circuits");
		choices.Add("Microprocessors");
		choices.Add("Artificial Intelligence");
		choices.Add("Central Programs");
		
		answer = choices[0];
		
	}
}

public class Question077 : QuestionMult
{
	public Question077()
	{
		title = "4th Gen - Microprocessors";
        subj = Subject.ProblemSolving;
        explanation = "The microprocessor brought the fourth generation of computers, as thousands of integrated circuits were built onto a single silicon chip. " +
"\n\n What in the first generation filled an entire room could now fit in the palm of the hand. The Intel 4004 chip, developed in 1971, located all the components of the computer - from the central processing unit and memory to input/output controls - on a single chip. " +
"\n In 1981 IBM introduced its first computer for the home user, and in 1984 Apple introduced the Macintosh. Microprocessors also moved out of the realm of desktop computers and into many areas of life as more and more everyday products began to use microprocessors. " +
"\n As these small computers became more powerful, they could be linked together to form networks, which eventually led to the development of the Internet. Fourth generation computers also saw the development of GUIs, the mouse and handheld devices.";
	
		question = "It replaced integrated circuits and could fit inside your palm. Paved the way of the development of the internet, GUI, and more everday life devices.";
		choices.Add("Transistors");

		choices.Add("Microprocessors");

		choices.Add("Artificial Intelligence");
		choices.Add("Central Programs");
		
		answer = choices[1];
	}
}

public class Question078 : QuestionMult 
{
	public Question078()
	{
		title = "5th Gen - Artificial Intelligence";
        subj = Subject.ProblemSolving;
        explanation = "Fifth generation computing devices are based on artificial intelligence. They are still in development. Applications that have been developed so far in this generation are: " +
"\n\n Voice recognition that is being used today. \n Parallel processing and superconductors which is helping to make artificial intelligence a reality. \n Quantum computation, molecular and nanotechnology will radically change the face of computers in years to come. " +
"\n\nThe goal of fifth-generation computing is to develop devices that respond to natural language input and are capable of learning and self-organization.";
	
		
		question = "Computing devices that use these have developed voice recognition, parallel processing, and quamtum computing.";
		choices.Add("Artificial Intelligence");
		choices.Add("High-end Processors");
		choices.Add("Microprocessors");
		choices.Add("Supercomputers");
		
		answer = choices[0];
	}
}

public class Question079 : QuestionMult 
{
	public Question079()
	{
		title = "Abacus";
        subj = Subject.ProblemSolving;
        explanation = "The abacus was an early aid for mathematical computations. Its only value is that it aids the memory of the human performing the calculation. A skilled abacus operator " +
"can work on addition and subtraction problems at the speed of a person equipped with a hand calculator (multiplication and division are slower). ";
	
		
		question ="In early days, humans used this device for mathematical computations";
		choices.Add("Abacus");
		choices.Add("Algebra");
		choices.Add("Pascaline");
		choices.Add("Napier's bones");

        answer = choices[0];
	}
}

public class Question080 : QuestionMult {
	public Question080()
	{
		title = "Computer Data Storage";
        subj = Subject.ProblemSolving;
        explanation = "Computer data storage, often called storage or memory, refers to computer components, devices, and recording media that retain digital data used for computing for some interval of time. ";
		
		question = "This refers to devices that retain digital data used for computing.";
		choices.Add("Storage Devices");
		choices.Add("Input Devices");
		choices.Add("Output Devices");
		choices.Add("Peripheral Devices");
		
		answer = choices[0];
	}
}

public class Question081 : QuestionJumbleWrite
{
	public Question081()
	{
		title = "Primary Storage";
        subj = Subject.ProblemSolving;
        explanation = "Primary storage, presently known as memory, is the only one directly accessible to the CPU. The CPU continuously reads instructions stored there and executes them. Any data actively operated on is also stored there in uniform manner. (e.g. RAM, CPU cache)";
		question = "_________ storage is directly accessible by the CPU. The CPU continuously reads data and executes instructions from it.";
		answer = "primary";
		
	}
}

public class Question082 : QuestionJumbleWrite
{
	public Question082()
	{
		title = "Secondary Storage";
        subj = Subject.ProblemSolving;
        explanation = "Secondary storage, or storage in popular usage, differs from primary storage in that it is not directly accessible by the CPU. The computer usually uses its input/output channels to access secondary storage and transfers the desired data using intermediate area in primary storage for long-term storage. (e.g. Hard disks, flash drives, CD/DVDs)";
		question = "________ storage is used to store data and programs for long-term storage.";
		answer = "Secondary";
	}
}

public class Question083 : QuestionJumbleWrite{
	public Question083()
	{
		title = "Primary vs Secondary Storage";
        subj = Subject.ProblemSolving;
        explanation = "Primary storage is volatile which means it loses its data once its power is shut off unlike secondary storage which retains its data even if its power is out. Secondary storage has more capacity and less expensive compared to primary storage.";
	
		question = "Primary storage is ______ which means it loses its data when its power is shut down.";
		answer = "volatile";
	}
}

public class Question084 : QuestionJumbleWrite
{
	public Question084()
	{
		title = "ASCII";
        subj = Subject.ProblemSolving;
        explanation = "ASCII (American Standard Code for Information Interchange) is the most common character encoding format for text data in computers and on the internet.  A set of 128 numeric codes that represent the English letters, various punctuation marks, and other characters.";
		question = " A set of 128 numeric codes that represent the English letters, various punctuation marks, and other characters is _______.";
		answer = "ASCII";
	}
}

public class Question085 : QuestionJumbleWrite{
	public Question085()
	{
		title = "Machine Language";
        subj = Subject.ProblemSolving;
        explanation = "Sometimes referred to as machine code or object code, machine language is a collection of binary digits or bits that the computer reads and interprets to perform operations. Machine language is the only language a computer is capable of understanding.";
		question = "______ language is a series of 1s and 0s that the computer can only understand and interprets to perform operations.";
		answer = "Machine";
	}
}

public class Question086 : QuestionJumbleWrite
{
	public Question086()
	{
		title = "Assembly Language";
        subj = Subject.ProblemSolving;
        explanation = "Computers can only execute programs that are written in machine language. A program can have thousands or even millions of binary instructions, and writing such a program would be very tedious and time consuming. Programming in machine language would also be very difficult because putting a 0 or a 1 in the wrong place will cause an error." +
		"For this reason, assembly language was created in the early days of computing2 as an alternative to machine language. Instead of using binary numbers for instructions, assembly language uses short words that are known as mnemonics. For example, in assembly language, the mnemonic add typically means to add numbers, mul typically means to multiply numbers, and mov typically means to move a value to a location in memory. When a programmer uses assembly language to write a program, he or she can write" +
        "short mnemonics instead of binary numbers. Assembly language programs cannot be executed by the CPU, however. The CPU only understands machine language, so a special program known as an assembler is used to translate an assembly language program to a machine language program. ";
        question = "Since it is difficult to write machine code, ________ language are mnemonics used by programmers to send instructions to the CPU.";
        answer = "Assembly";
	}
}

public class Question087 : QuestionJumbleWrite
{
	public Question087()
	{
		title = "High-Level Languages";
        subj = Subject.ProblemSolving;
        explanation = "Although assembly language makes it unnecessary to write binary machine language instructions, it is not without difficulties. Assembly language is primarily a direct substitute for machine language, and like machine language, it requires that you know a lot about the CPU. Assembly language also requires that you write a large number of instructions for " +
"even the simplest program. Because assembly language is so close in nature to machine language, it is referred to as a low-level language." +
"\n\nIn the 1950s, a new generation of programming languages known as high-level languages began to appear. A high-level language allows you to create powerful and complex programs without knowing how the CPU works, and without writing large numbers of low-level instructions. In addition, most high-level languages use words that are easy to understand. (e.g. C++, Java, C#)";
			
		question = "Creating a program with assembly language takes a lot of work. _______ languages were introduced to allow you to make complex programs without deep understanding of the CPU.";
		answer = "High-level";		
	}
}