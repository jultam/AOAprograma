using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TorchSharp;
using static TorchSharp.torch;
using static TorchSharp.torch.nn;

namespace AlgorithmRunner
{
    public class ParameterModel : Module<Tensor, Tensor>
    {
        private readonly Module<Tensor, Tensor> layers;
        private Tensor scales;
        private Tensor offsets;

        public ParameterModel(string name) : base(name) 
        {
            layers = Sequential(
                Linear(5, 7),
                ReLU(), 
                Dropout(0.1), 
                Linear(7, 7),
                ReLU(),
                Linear(7, 5),
                Sigmoid()
            );

            // Scale outputs to acceptable ranges for algorithm parameters
            // PS (index 0): 5 to 50000
            // M_Iter (index 1): 5 to 5000
            // alpha (index 2): 1 to 10
            // mu (index 3): 0.1 to 1.0
            // epsilon (index 4): 0.000001 to 0.5
            var scaleValues = new float[] { 49995f, 4995f, 9f, 0.9f, 0.499999f };
            var offsetValues = new float[] { 5f, 5f, 1f, 0.1f, 0.000001f };

            scales = torch.tensor(scaleValues);
            offsets = torch.tensor(offsetValues);

            register_buffer("scales", scales);
            register_buffer("offsets", offsets);

            RegisterComponents();
        }

        public override Tensor forward(Tensor input)
        {
            var output = layers.forward(input);

            return output * scales + offsets;
        }
    }
}
