using MathNet.Numerics.LinearAlgebra.Solvers;
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

    }
}
