using CipherMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;
using Rnd = UnityEngine.Random;

public class PigpenMazeCipher : CipherBase
{
    private readonly bool invert;

    public override string Name { get { return invert ? "Inverted Pigpen Maze Cipher" : "Pigpen Maze Cipher"; } }
    public override string Code { get { return "PZ"; } }
    
    
    public override bool IsInvert { get { return invert; } }
    public PigpenMazeCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        Data data = new Data();
        
        var logMessages = new List<string>();
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXY", replaceZ = "", encrypt = "";
        logMessages.Add(string.Format("Before Replacing Zs: {0}", word));
        for (int i = 0; i < word.Length; i++)
        {
            if (word[i] == 'Z')
            {
                word = word.Substring(0, i) + "" + alpha[Rnd.Range(0, alpha.Length)] + "" + word.Substring(i + 1);
                replaceZ = replaceZ + "" + word[i];
            }
            else
                replaceZ = replaceZ + "" + alpha.Replace(word[i].ToString(), "")[Rnd.Range(0, 24)];
        }
        logMessages.Add(string.Format("After Replacing Zs: {0}", word));
        char[][] maze = generateMaze();
        string wallLetters = generatePigpenMazeLayout(maze);
        int[] cur = getPossiblePositions(maze, '.')[0];
        string startCoord = "ABCDE"[cur[1] / 2] + "" + (cur[0] / 2 + 1);
        string directionLetters = generateSpaces(maze, cur);
        logMessages.Add(string.Format("Starting Coordinate: {0}", startCoord));
        logMessages.Add(string.Format("Maze Layout: {0}", wallLetters));
        logMessages.Add(string.Format("Movement: {0}", directionLetters));
        foreach (char[] row in maze)
            logMessages.Add(string.Format("{0}", new string(row)));

        string kw = data.PickWord(3, word.Length);
        while(kw.Contains("Z"))
            kw = data.PickWord(3, word.Length);
        string kwVals = data.PickWord(4);
        string movePriority = getMovementPriority(kwVals);
        logMessages.Add(string.Format("Page 1 Screen 4: {0}", kwVals));
        logMessages.Add(string.Format("Movement Priority: {0}", movePriority));
        logMessages.Add(string.Format("Page 2 Screen 1: {0}", kw));


        if (invert)
        {
            for(int i = 0; i < word.Length; i++)
            {
                encrypt += encryptLetter(maze, word[i], kw[i % kw.Length], movePriority, invert);
                logMessages.Add(string.Format("{0} + {1} -> {2}", word[i], kw[i % kw.Length], encrypt[i]));
            }
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
            {
                encrypt += encryptLetter(maze, word[i], kw[i % kw.Length], movePriority, invert);
                logMessages.Add(string.Format("{0} + {1} -> {2}", word[i], kw[i % kw.Length], encrypt[i]));
            }
        }

        logMessages.Add(string.Format("Page 2 Screen 2: {0}", replaceZ));
        string str = wallLetters + directionLetters;
        string screen3 = str.Substring(24), screenC = "";
        if(screen3.Length > 8)
        {
            screenC = screen3.Substring(8);
            screen3 = screen3.Substring(0, 8);
        }
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] {
                new PageInfo(new ScreenInfo[] { str.Substring(0, 8), str.Substring(8, 4), str.Substring(12, 8), str.Substring(20, 4), screen3, screenC, kwVals, startCoord }, invert),
                new PageInfo(new ScreenInfo[] { kw, null, replaceZ }, invert),
            },
            Score = 6 + (word.Length / 2)
        };
    }
    private char[][] generateMaze()
    {
        char[][] maze = getEmptyMaze();
        List<int[]> possWalls = getPossiblePositions(maze, ' ');
        foreach(int[] wall in possWalls)
        {
            maze[wall[0]][wall[1]] = '■';
            bool check;
            if (wall[0] % 2 == 1)
                check = checkMaze(maze, new int[] { wall[0], wall[1] - 1 }, new int[] { wall[0], wall[1] + 1 });
            else
                check = checkMaze(maze, new int[] { wall[0] - 1, wall[1] }, new int[] { wall[0] + 1, wall[1] });
            if(!(check))
                maze[wall[0]][wall[1]] = ' ';
        }
        return maze;
    }
    private char[][] getEmptyMaze()
    {
        char[][] maze = new char[11][];
        maze[0] = new char[] { '■', '■', '■', '■', '■', '■', '■', '■', '■', '■', '■' };
        maze[10] = new char[] { '■', '■', '■', '■', '■', '■', '■', '■', '■', '■', '■' };
        for (int i = 2; i < 10; i += 2)
        {
            maze[i - 1] = new char[] { '■', '.', ' ', '.', ' ', '.', ' ', '.', ' ', '.', '■' };
            maze[i] = new char[] { '■', ' ', '■', ' ', '■', ' ', '■', ' ', '■', ' ', '■' };
        }
        maze[9] = new char[] { '■', '.', ' ', '.', ' ', '.', ' ', '.', ' ', '.', '■' };
        return maze;
    }
    private List<int[]> getPossiblePositions(char[][] maze, char c)
    {
        List<int[]> possWalls = new List<int[]>();
        for(int i = 0; i < maze.Length; i++)
        {
            for(int j = 0; j < maze[i].Length; j++)
            {
                if (maze[i][j] == c)
                    possWalls.Add(new int[] { i, j });
            }
        }
        return possWalls.Shuffle();
    }
    private bool checkMaze(char[][] maze, int[] current, int[] goal)
    {
        char[][] temp = new char[maze.Length][];
        for (int i = 0; i < maze.Length; i++)
            temp[i] = new string(maze[i]).ToCharArray();
        string dir = "";
        while(current[0] != goal[0] || current[1] != goal[1])
        {
            if(temp[current[0] - 1][current[1]] != '■')
            {
                temp[current[0] - 1][current[1]] = '■';
                dir += "U";
                current[0] -= 2;
            }
            else if (temp[current[0]][current[1] + 1] != '■')
            {
                temp[current[0]][current[1] + 1] = '■';
                dir += "R";
                current[1] += 2;
            }
            else if (temp[current[0] + 1][current[1]] != '■')
            {
                temp[current[0] + 1][current[1]] = '■';
                dir += "D";
                current[0] += 2;
            }
            else if (temp[current[0]][current[1] - 1] != '■')
            {
                temp[current[0]][current[1] - 1] = '■';
                dir += "L";
                current[1] -= 2;
            }
            else
            {
                if (dir.Length == 0)
                    return false;
                switch(dir[dir.Length - 1])
                {
                    case 'U':
                        current[0] += 2;
                        break;
                    case 'R':
                        current[1] -= 2;
                        break;
                    case 'D':
                        current[0] -= 2;
                        break;
                    case 'L':
                        current[1] += 2;
                        break;
                }
                dir = dir.Substring(0, dir.Length - 1);
            }
        }
        return true;
    }
    private string generateSpaces(char[][] maze, int[] cur)
    {
        char[][] temp = new char[maze.Length][];
        for (int i = 0; i < maze.Length; i++)
            temp[i] = new string(maze[i]).ToCharArray();
        string dir = "", letters = "", dirList = "VTSU", notDirList = "ZXWY";
        string alpha = "BCDEFGHIJKLMNOPQRSTUVWXY";
        int index = 0;
        temp[cur[0]][cur[1]] = 'A';
        while (true)
        {
            string choice = "";
            if (temp[cur[0] - 1][cur[1]] != '■')
                choice += "0";
            if (temp[cur[0]][cur[1] + 1] != '■')
                choice += "1";
            if (temp[cur[0] + 1][cur[1]] != '■')
                choice += "2";
            if (temp[cur[0]][cur[1] - 1] != '■')
                choice += "3";
            if (choice.Length == 0)
            {
                if (dir.Length == 0)
                    break;
                switch (dir[dir.Length - 1])
                {
                    case '0':
                        cur[0] += 2;
                        break;
                    case '1':
                        cur[1] -= 2;
                        break;
                    case '2':
                        cur[0] -= 2;
                        break;
                    case '3':
                        cur[1] += 2;
                        break;
                }
                dir = dir.Substring(0, dir.Length - 1);
            }
            else
            {
                dir += choice[Rnd.Range(0, choice.Length)];
                if (choice.Length == 2 && Rnd.Range(0, 2) == 0)
                    letters += notDirList[choice.Replace(dir[dir.Length - 1] + "", "")[0] - '0'];
                else if (choice.Length > 1)
                    letters += dirList[dir[dir.Length - 1] - '0'];
                switch (dir[dir.Length - 1])
                {
                    case '0':
                        temp[cur[0] - 1][cur[1]] = '■';
                        cur[0] -= 2;
                        break;
                    case '1':
                        temp[cur[0]][cur[1] + 1] = '■';
                        cur[1] += 2;
                        break;
                    case '2':
                        temp[cur[0] + 1][cur[1]] = '■';
                        cur[0] += 2;
                        break;
                    case '3':
                        temp[cur[0]][cur[1] - 1] = '■';
                        cur[1] -= 2;
                        break;
                }
            }
            if (temp[cur[0]][cur[1]] == '.')
                temp[cur[0]][cur[1]] = alpha[index++];
        }
        for (int i = 1; i < maze.Length - 1; i++)
        {
            for (int j = 1; j < maze[i].Length - 1; j++)
            {
                if (maze[i][j] == '.')
                    maze[i][j] = temp[i][j];
            }
        }
        return letters;
    }
    private string generatePigpenMazeLayout(char[][] maze)
    {
        char[][] temp =
        {
            new char[]{ ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
            new char[]{ ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
            new char[]{ ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
            new char[]{ ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
            new char[]{ ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
            new char[]{ ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
            new char[]{ ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
            new char[]{ ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
            new char[]{ ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
            new char[]{ ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
            new char[]{ ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' }
        };
        string letters = "";
        for (int i = 1; i < maze.Length - 3; i+=2)
        {
            for (int j = 1; j < maze[i].Length - 3; j += 2)
            {
                string pigpen = getPigpen(maze[i][j - 1] + "" + maze[i - 1][j], temp[i][j - 1] + "" + temp[i - 1][j]);
                temp[i][j - 1] = pigpen[0];
                temp[i - 1][j] = pigpen[1];
                temp[i][j + 1] = pigpen[2];
                temp[i + 1][j] = pigpen[3];
                letters += pigpenToLetter(pigpen);
            }
            string rng = "■ ";
            char[] pp = { XOR(maze[i][maze.Length - 3], temp[i][temp.Length - 3]), XOR(maze[i - 1][maze.Length - 2], temp[i - 1][temp.Length - 2]), XOR(maze[i][maze.Length - 1], temp[i][temp.Length - 1]), rng[Rnd.Range(0, 2)] };
            char letter = pigpenToLetter(new string(pp));
            if(letter == ' ')
            {
                pp[pp.Length - 1] = invertChar(pp[pp.Length - 1]);
                letter = pigpenToLetter(new string(pp));
            }
            temp[i][temp.Length - 3] = pp[0];
            temp[i - 1][temp.Length - 2] = pp[1];
            temp[i][temp.Length - 1] = pp[2];
            temp[i + 1][temp.Length - 2] = pp[3];
            letters += letter;
        }
        for(int i = 1; i < maze[maze.Length - 2].Length - 3; i+=2)
        {
            string rng = "■ ";
            char[] pp = { XOR(maze[maze.Length - 2][i - 1], temp[temp.Length - 2][i - 1]), XOR(maze[maze.Length - 3][i], temp[temp.Length - 3][i]), rng[Rnd.Range(0, 2)], '■' };
            char letter = pigpenToLetter(new string(pp));
            if (letter == ' ')
            {
                pp[pp.Length - 2] = invertChar(pp[pp.Length - 2]);
                letter = pigpenToLetter(new string(pp));
            }
            temp[temp.Length - 2][i - 1] = pp[0];
            temp[temp.Length - 3][i] = pp[1];
            temp[temp.Length - 2][i + 1] = pp[2];
            temp[temp.Length - 1][i] = pp[3];
            letters += letter;
        }
        char[] PP = { XOR(maze[maze.Length - 2][maze.Length - 3], temp[temp.Length - 2][temp.Length - 3]), XOR(maze[maze.Length - 3][maze.Length - 2], temp[temp.Length - 3][temp.Length - 2]), '■', '■' };
        char let = pigpenToLetter(new string(PP));
        letters += let;
        return letters;
    }
    private string getPigpen(string s1, string s2)
    {
        string rng = "■ ";
        List<int> poss = new List<int>();
        for (int z = 0; z < 3; z++)
            poss.Add(z);
        poss.Shuffle();
        char[] pigpen = { XOR(s1[0], s2[0]), XOR(s1[1], s2[1]), rng[Rnd.Range(0, 2)], rng[Rnd.Range(0, 2)] };
        char letter = pigpenToLetter(new string(pigpen));
        if (letter == ' ')
        {
            for (int index = 0; index < poss.Count; index++)
            {
                char[] pigpenTemp;
                switch (poss[index])
                {
                    case 0:
                        pigpenTemp = new char[] { pigpen[0], pigpen[1], invertChar(pigpen[2]), pigpen[3] };
                        break;
                    case 1:
                        pigpenTemp = new char[] { pigpen[0], pigpen[1], pigpen[2], invertChar(pigpen[3]) };
                        break;
                    default:
                        pigpenTemp = new char[] { pigpen[0], pigpen[1], invertChar(pigpen[2]), invertChar(pigpen[3]) };
                        break;
                }
                letter = pigpenToLetter(new string(pigpenTemp));
                if (letter != ' ')
                    return new string(pigpenTemp);
            }
        }
        return new string(pigpen);
    }
    private char XOR(char c1, char c2)
    {
        if (c1 == c2)
            return ' ';
        else
            return '■';
    }
    private char pigpenToLetter(string pigpen)
    {
        switch(pigpen)
        {
            case "    ": return 'N';
            case "   ■": return 'Q';
            case "  ■ ": return 'O';
            case "  ■■": return "AR"[Rnd.Range(0, 2)];
            case " ■  ": return 'K';
            case " ■■ ": return "GL"[Rnd.Range(0, 2)];
            case " ■■■": return 'D';
            case "■   ": return 'M';
            case "■  ■": return "CP"[Rnd.Range(0, 2)];
            case "■ ■■": return 'B';
            case "■■  ": return "IJ"[Rnd.Range(0, 2)];
            case "■■ ■": return 'F';
            case "■■■ ": return 'H';
            case "■■■■": return 'E';
        }
        return ' ';
    }
    private char invertChar(char c)
    {
        if (c == ' ')
            return '■';
        return ' ';
    }
    private string getMovementPriority(string kw)
    {
        char[] order = kw.ToCharArray().Distinct().ToArray();
        Array.Sort(order);
        int[] vals = new int[4];
        int score = 0;
        for (int i = 0; i < order.Length; i++)
        {
            for (int j = 0; j < kw.Length; j++)
            {
                if (order[i] == kw[j])
                    vals[j] = score++;
            }
        }
        string movement = "URDL";
        string priority = "";
        for(int i = 0; i < vals.Length; i++)
        {
            for(int j = 0; j < vals.Length; j++)
            {
                if (i == vals[j])
                    priority += movement[j];

            }
        }
        return priority;
    }
    private char encryptLetter(char[][] maze, char letter, char kwLetter, string movePriority, bool invert)
    {
        int[] cur = getCursor(maze, kwLetter);
        char[][] temp = new char[maze.Length][];
        for (int i = 0; i < maze.Length; i++)
            temp[i] = new string(maze[i]).ToCharArray();
        
        string dir = "";
        if(invert)
        {
            int score = letter - 'A';
            while(score > 0)
            {
                if (temp[cur[0]][cur[1]] != '*')
                {
                    temp[cur[0]][cur[1]] = '*';
                    score--;
                }
                char move = getMovement(temp, movePriority, cur, dir);
                if (move == '!')
                {
                    cur = moveCur(temp, getOppositeMove(dir[dir.Length - 1]), cur);
                    dir = dir.Substring(0, dir.Length - 1);
                }
                else
                {
                    cur = moveCur(temp, move, cur);
                    dir += move;
                }
            }
            while(temp[cur[0]][cur[1]] == '*')
            {
                //Debug.LogFormat("{0} -> {1}", letter, maze[cur[0]][cur[1]]);
                char move = getMovement(temp, movePriority, cur, dir);
                if (move == '!')
                {
                    cur = moveCur(temp, getOppositeMove(dir[dir.Length - 1]), cur);
                    dir = dir.Substring(0, dir.Length - 1);
                }
                else
                {
                    cur = moveCur(temp, move, cur);
                    dir += move;
                }
            }
            return temp[cur[0]][cur[1]];
        }
        else
        {
            int score = 0;
            while (maze[cur[0]][cur[1]] != letter)
            {
                if (temp[cur[0]][cur[1]] != '*')
                {
                    temp[cur[0]][cur[1]] = '*';
                    score++;
                }
                char move = getMovement(temp, movePriority, cur, dir);
                if (move == '!')
                {
                    cur = moveCur(temp, getOppositeMove(dir[dir.Length - 1]), cur);
                    dir = dir.Substring(0, dir.Length - 1);
                }
                else
                {
                    cur = moveCur(temp, move, cur);
                    dir += move;
                }
            }
            return "ABCDEFGHIJKLMNOPQRSTUVWXY"[score];
        }
        
        
    }
    private int[] getCursor(char[][] maze, char letter)
    {
        for(int i = 1; i < maze.Length; i+=2)
        {
            for(int j = 1; j < maze[i].Length; j+=2)
            {
                if (maze[i][j] == letter)
                    return new int[] { i, j };
            }
        }
        return null;
    }
    private char getMovement(char[][] maze, string movePriority, int[] cur, string dir)
    {
        foreach(char move in movePriority)
        {
            if (canMove(maze, move, cur))
                return move;
        }
        return '!';
    }
    private bool canMove(char[][] maze, char move, int[] cur)
    {
        switch(move)
        {
            case 'U': return maze[cur[0] - 1][cur[1]] != '■';
            case 'R': return maze[cur[0]][cur[1] + 1] != '■';
            case 'D': return maze[cur[0] + 1][cur[1]] != '■';
            default: return maze[cur[0]][cur[1] - 1] != '■';
        }
    }
    private int[] moveCur(char[][] maze, char move, int[] cur)
    {
        switch(move)
        {
            case 'U':
                maze[cur[0] - 1][cur[1]] = '■';
                cur[0] -= 2;
                break;
            case 'R':
                maze[cur[0]][cur[1] + 1] = '■';
                cur[1] += 2;
                break;
            case 'D':
                maze[cur[0] + 1][cur[1]] = '■';
                cur[0] += 2;
                break;
            case 'L':
                maze[cur[0]][cur[1] - 1] = '■';
                cur[1] -= 2;
                break;
        }
        return cur;
    }
    private char getOppositeMove(char move)
    {
        switch(move)
        {
            case 'U': return 'D';
            case 'R': return 'L';
            case 'D': return 'U';
            default: return 'R';
        }
    }
}
