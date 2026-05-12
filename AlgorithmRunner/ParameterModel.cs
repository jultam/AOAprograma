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
            /*layers = Sequential(
                Linear(5, 7),
                LeakyReLU(), 
                Dropout(0.1), 
                Linear(7, 7),
                LeakyReLU(),
                Linear(7, 5),
                Sigmoid()
            );*/


            layers = Sequential(
                Linear(5, 10),
                BatchNorm1d(10),
                LeakyReLU(), 
                Dropout(0.1), 

                Linear(10, 10),
                BatchNorm1d(10),
                LeakyReLU(),
                Dropout(0.1), 

                Linear(10, 10),
                BatchNorm1d(10),
                LeakyReLU(),
                Dropout(0.1),

                Linear(10, 5),
                BatchNorm1d(5),
                Sigmoid()
            );

            
            /*layers = Sequential(
                Linear(5, 32),
                LeakyReLU(),
                Dropout(0.1),
                Linear(32, 16),
                LeakyReLU(),
                Dropout(0.1),
                Linear(16, 8),
                LeakyReLU(),
                Dropout(0.1),
                Linear(8, 5),
                Sigmoid()
            );*/
            

            // Scale outputs to acceptable ranges for algorithm parameters
            // PS (index 0): 5 to 2500
            // M_Iter (index 1): 5 to 2000
            // alpha (index 2): 1 to 10
            // mu (index 3): 0.1 to 0.499
            // epsilon (index 4): 0.000001 to 0.5
            var scaleValues = new float[] { 2495f, 1995f, 9f, 0.398f, 0.499999f };
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

        public Tensor GetScales()
        {
            return scales;
        }

        public Tensor GetOffsets()
        {
            return offsets;
        }
    }
}
