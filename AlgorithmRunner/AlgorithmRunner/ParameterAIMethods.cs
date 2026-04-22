using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TorchSharp;
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

        public static void TrainModel(int iter)
        {
            var optimizer = torch.optim.Adam(model.parameters(), lr: 0.01);

            var input = torch.randn(64, 5);
            var target = torch.randn(64, 5);

            for (int i = 0; i < iter; i++)
            {
                var eval = model.forward(input);
                var output = functional.mse_loss(eval, target, Reduction.Sum);

                optimizer.zero_grad();

                output.backward();

                optimizer.step();
            }
        }

        public static void SaveModel(string filePath)
        {
            model.save(filePath);
        }

    }
}
