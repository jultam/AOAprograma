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

        public ParameterModel(string name) : base(name) 
        {
            layers = Sequential(
                Linear(5, 7),
                ReLU(), 
                Dropout(0.1), 
                Linear(7, 5)
            );
            RegisterComponents();
        }

        public override Tensor forward(Tensor input)
        {
            return layers.forward(input);
        }
    }
}
