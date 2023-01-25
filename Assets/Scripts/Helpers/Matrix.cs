using CipherMachine;
using System;
using System.Linq;

public class Matrix
{
    public int Size { get; private set; }
    public int Modulus { get; private set; }

    private int[,] _matrix;

    public Matrix(int size, int modulus)
    {
        Size = size;
        Modulus = modulus < 13 ? 13 : modulus > int.MaxValue / 2 ? int.MaxValue / 2 : modulus;
        _matrix = new int[size, size];
    }

    public void SetEntry(int i, int j, int value)
    {
        _matrix[i, j] = value;
    }

    public int GetEntry(int i, int j)
    {
        return _matrix[i, j];
    }

    public Matrix SubMatrix(int a, int b)
    {
        Matrix matrix = new Matrix(Size - 1, Modulus);

        int entry = 0;
        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++)
            {
                if (i == a || j == b)
                    continue;
                matrix.SetEntry(entry / matrix.Size, entry % matrix.Size, _matrix[i, j]);
                entry++;
            }

        return matrix;
    }

    public int Determinant()
    {
        if (Size == 0)
            return 1;

        int determinant = 0;

        for (int i = 0; i < Size; i++)
            determinant += CMTools.mod(_matrix[0, i] * SubMatrix(0, i).Determinant() * (int)Math.Pow(-1, i), Modulus);

        determinant = CMTools.mod(determinant, Modulus);

        return determinant;
    }

    public Matrix Cofactor()
    {
        Matrix matrix = new Matrix(Size, Modulus);

        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++)
                matrix.SetEntry(i, j, CMTools.mod(SubMatrix(i, j).Determinant() * (int)Math.Pow(-1, i + j), Modulus));

        return matrix;
    }

    public Matrix Transpose()
    {
        Matrix matrix = new Matrix(Size, Modulus);

        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++)
                matrix.SetEntry(j, i, _matrix[i, j]);

        return matrix;
    }

    public Matrix Adjugate()
    {
        return Cofactor().Transpose();
    }

    public static int MultiplicativeInverse(int value, int modulus = int.MaxValue)
    {
        for (int i = 0; i < modulus; i++)
            if (i * value % modulus == 1)
                return i;
        return -1;
    }

    public Matrix InverseMatrix()
    {
        return Adjugate().ScalarMultiplication(MultiplicativeInverse(Determinant(), Modulus));
    }

    public Matrix ScalarMultiplication(int scalar)
    {
        Matrix matrix = new Matrix(Size, Modulus);

        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++)
                matrix.SetEntry(i, j, CMTools.mod(_matrix[i, j] * scalar, Modulus));

        return matrix;
    }

    public int[] MatrixVectorMultiplication(int[] vector)
    {
        int[] result = new int[Size];

        for (int i = 0; i < Size; i++)
            result[i] = CMTools.mod(Enumerable.Range(0, Size).Select(x => CMTools.mod(GetEntry(i, x) * vector[x], Modulus)).Sum(), Modulus);

        return result;
    }

    public static int GreatestCommonDivisor(int a, int b)
    {
        while (a * b != 0)
        {
            int s = a % b;
            a = b;
            b = s;
        }
        return new int[] { a, b }.Max();
    }

    public override string ToString()
    {
        return _matrix.Cast<int>().Join(", ");
    }

    public int[] MatrixToArray()
    {
        return _matrix.Cast<int>().ToArray();
    }
}
