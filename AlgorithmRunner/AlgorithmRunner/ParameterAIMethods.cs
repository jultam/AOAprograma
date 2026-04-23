using MathNet.Numerics.LinearAlgebra.Solvers;
using MathNet.Numerics.Statistics;
using Meta.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TorchSharp;
using TorchSharp.Modules;
using static TorchSharp.torch.nn;

namespace AlgorithmRunner
{
    internal class ParameterAIMethods
    {
        private static ParameterModel model;
        private static Random _rnd = new Random();

        public static void CreateModel()
        {
            model = new ParameterModel("parameterModel");
        }

        public static void LoadModel(string filePath)
        {
            model.load(filePath);
        }

        public static void TrainModel(int epochs, IterableDataLoader loader)
        {
            var optimizer = torch.optim.Adam(model.parameters(), lr: 0.01);
            var criterion = MSELoss();

            for (int i = 0; i < epochs; i++)
            {
                foreach (var batch in loader)
                {
                    var x = batch[0];
                    var y = batch[1];

                    var prediction = model.forward(x);

                    //Algorithm.AOA(1, prediction[0], prediction[1]);

                    var loss = criterion.forward(prediction, y);

                    optimizer.zero_grad();

                    loss.backward();

                    optimizer.step();
                }
            }
        }

        public static void SaveModel(string filePath)
        {
            model.save(filePath);
        }

        public static (double, double) RandomSamples(Form1.BenchmarkFunction benchmark, int D, int samplesCount)
        {
            double ub = benchmark.arg_range_2; double lb = benchmark.arg_range_1;
            double[] values = new double[samplesCount];

            for (int i = 0; i <= samplesCount; i++)
            {
                double[] X = new double[D];
                for (int j = 0; j <= D; j++)
                {
                    double r = _rnd.NextDouble();
                    X[j] = r * (ub - lb) + lb;
                }
                values[i] = benchmark.function(X, D);
            }

            return (values.Mean(), values.StandardDeviation());
        }

    }
}
