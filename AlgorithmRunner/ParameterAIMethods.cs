using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra.Solvers;
using MathNet.Numerics.Statistics;
using Meta.Numerics.Statistics;
using TorchSharp;
using TorchSharp.Modules;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
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
            try
            {
                model.load(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        public static void TrainModel(int epochs, List<(IList<torch.Tensor> tensors, List<Form1.BenchmarkFunction> benchmarks)> batches,
            Form1.Components components)
        {
            var optimizer = torch.optim.Adam(model.parameters(), lr: 0.001);
            var scheduler = torch.optim.lr_scheduler.ReduceLROnPlateau(optimizer, mode: "max", factor: 0.5, patience: 3);

            Form1.AppendTextSafe($"=== Starting Training: {epochs} Epochs ===\n");
            DateTime startTime = DateTime.Now;

            for (int i = 0; i < epochs; i++)
            {
                Form1.AppendTextSafe($"\n>>> Epoch {i + 1}/{epochs} <<<\n");
                double totalReward = 0;
                int batchCount = 0;

                foreach (var batch in batches)
                {
                    var x = batch.tensors[0];
                    var y = batch.tensors[1];
                    var batchBenchmarks = batch.benchmarks;

                    Form1.AppendTextSafe($"------\nBatch {batchCount + 1}: x shape = [{string.Join(", ", x.shape)}], y shape = [{string.Join(", ", y.shape)}]\n");

                    model.train();
                    var prediction = model.forward(x);
                    //Form1.AppendTextSafe("| " + TensorToString(x) + " |\n " + TensorToString(prediction) + " |\n ");

                    double precisionReward;
                    double timeReward;
                    double batchReward = 0;
                    using (torch.no_grad())
                    {
                        var detachedPrediction = prediction.detach();

                        for (int s = 0; s < x.shape[0]; s++)
                        {
                            Form1.BenchmarkFunction benchmark = batchBenchmarks[s];

                            Form1.AppendTextSafe("---------\n"+benchmark.name + "\n" + TensorToString(x[s]) + "\n" + TensorToString(detachedPrediction[s]) + "\n");

                            TimeSpan timeout = TimeSpan.FromMinutes(2);
                            bool timedOut = false;
                            var startTimeout = DateTime.Now;
                            Results.AlgorithmResults result = null;

                            try
                            {
                                var task = System.Threading.Tasks.Task.Run(() =>
                                {
                                    return result = Algorithm.AOA(
                                    1,
                                    (int)detachedPrediction[s][0] * (int)x[s][2],
                                    (int)detachedPrediction[s][1],
                                    (int)x[s][2],
                                    (int)detachedPrediction[s][2],
                                    (double)detachedPrediction[s][3],
                                    (double)detachedPrediction[s][4],
                                    benchmark,
                                    components);
                                });

                                if (!task.Wait(timeout))
                                {
                                    Form1.AppendTextSafe("Timeout reached\n");
                                    timedOut = true;
                                } else
                                {
                                    result = task.Result;
                                }
                            } catch (Exception ex)
                            {
                                Form1.AppendTextSafe("Error: " + ex.Message + "\n");
                                timedOut = true;
                            }

                            double sampleReward = 0;
                            if (!timedOut)
                            {
                                Form1.AppendTextSafe("Found optimum: "+result.optimum.ToString() + "\nCalculation time: " + result.time.ToString() + "\n");

                                double targetValue = y[s].ToDouble();
                                double absError = Math.Abs(result.optimum - targetValue);
                                precisionReward = 1.0 / (1.0 + absError);
                                //precisionReward = Math.Exp(-Math.Abs(result.optimum - targetValue));

                                double targetTime = 50000.0;
                                timeReward = 1.0 / (1.0 + result.time / targetTime);
                                //timeReward = Math.Exp(-result.time / targetTime);

                                double precisionWeight = 0.7;
                                double timeWeight = 0.3;

                                sampleReward = precisionWeight * precisionReward + timeWeight * timeReward;
                                batchReward += sampleReward;

                                Form1.AppendTextSafe($"AOA Result: {result.optimum}, Target: {targetValue}, Reward: {sampleReward}\n");
                                Form1.AppendTextSafe($"Precision reward: {precisionReward}, Time reward: {timeReward}\n");
                            }
                        }
                    }

                    double avgBatchReward = batchReward / x.shape[0];

                    var meanPrediction = prediction.mean();
                    var regLoss = 0.01f * torch.sum(torch.pow(prediction - meanPrediction, 2));

                    var entropy = -torch.sum(prediction * torch.log(prediction + 1e-8));
                    var rewardTensor = torch.tensor((float)avgBatchReward, dtype: torch.float32);
                    var loss = -rewardTensor + regLoss - 0.1f * entropy;

                    optimizer.zero_grad();
                    loss.backward();

                    torch.nn.utils.clip_grad_norm_(model.parameters(), 1.0);

                    optimizer.step();

                    totalReward += avgBatchReward;
                    batchCount++;
                }
                double avgReward = totalReward / batchCount;
                scheduler.step(avgReward);

                TimeSpan elapsed = DateTime.Now - startTime;
                Form1.AppendTextSafe($"Epoch done, Time Elapsed: {elapsed:hh\\:mm\\:ss}\n");
            }
            Form1.AppendTextSafe("Training completed.\n");
        }

        public static void SaveModel(string filePath)
        {
            try
            {
                // Save the model
                model.save(filePath);

                Form1.AppendTextSafe($"Model saved successfully to: {Path.GetFullPath(filePath)}\n");
            }
            catch (Exception ex)
            {
                Form1.AppendTextSafe($"ERROR saving model: {ex.Message}\n");
                MessageBox.Show($"Failed to save model: {ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static (double, double) RandomSamples(Form1.BenchmarkFunction benchmark, int D, int samplesCount)
        {
            double ub = benchmark.arg_range_2; double lb = benchmark.arg_range_1;
            double[] values = new double[samplesCount];

            for (int i = 0; i < samplesCount; i++)
            {
                double[] X = new double[D];
                for (int j = 0; j < D; j++)
                {
                    double r = _rnd.NextDouble();
                    X[j] = r * (ub - lb) + lb;
                }
                values[i] = benchmark.function(X, D);
            }

            return (values.Mean(), values.StandardDeviation());
        }

        public static string TensorToString(torch.Tensor tensor)
        {
            var shape = tensor.shape;
            var dtype = tensor.dtype;

            if (dtype == torch.float32)
            {
                var data = tensor.data<float>().ToArray();
                return $"Shape: [{string.Join(", ", shape)}], Values: [{string.Join(", ", data.Select(v => v.ToString("F4")))}]";
            }
            else if (dtype == torch.float64)
            {
                var data = tensor.data<double>().ToArray();
                return $"Shape: [{string.Join(", ", shape)}], Values: [{string.Join(", ", data.Select(v => v.ToString("F4")))}]";
            }

            return tensor.ToString();
        }
    }
}
