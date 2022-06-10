using MultiThreading.Task3.MatrixMultiplier.Matrices;
using System.Threading.Tasks;

namespace MultiThreading.Task3.MatrixMultiplier.Multipliers
{
    public class MatricesMultiplierParallel : IMatricesMultiplier
    {
        public IMatrix Multiply(IMatrix firstMatrix, IMatrix secondMatrix)
        {
            var resultMatrix = new Matrix(firstMatrix.RowCount, secondMatrix.ColCount);

            Parallel.For(0, firstMatrix.RowCount, i =>
            {
                Parallel.For(0, secondMatrix.ColCount, j =>
                {
                    long sum = 0;
                    for (long k = 0; k < firstMatrix.ColCount; k++)
                    {
                        sum += firstMatrix.GetElement(i, k) * secondMatrix.GetElement(k, j);
                    }
                    resultMatrix.SetElement(i, j, sum);
                });
            });

            return resultMatrix;
        }
    }
}
