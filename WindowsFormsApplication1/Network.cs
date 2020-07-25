using System;

namespace Network
{
    class Net
    {
        double[] inputs = new double[4] {2,4,6,9};
        double[] outputs = new double[4] {1,5,3,8};
        Unit[] net = new Unit[4];

        public Net()
        {
            net[0] = new Unit(2.5, new double[] { 2 }, 2.5);
            net[1] = new Unit(3.5, new double[] { 4 }, 2.5);
            net[2] = new Unit(3.5, new double[] { 6 }, 2.5);
            net[3] = new Unit(3.5, new double[] { 8 }, 4.5);
        }
        public void SetInputOutput(double[] x, double[] y)
        {
            this.inputs = x;
            this.outputs = y;
        }

        public void train()
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                double output = outputs[i];
                double predictedoutput = 0;
                for (int j = 0; j < inputs.Length; j++)
                {
                    predictedoutput += net[j].phi(inputs[i]) * net[j].w;

                }
                //predictedoutput= Math.round(predictedoutput);

                for (int j = 0; j < inputs.Length; j++)
                {
                    net[j].update(inputs[i], output, predictedoutput);
                }
            }
        }

        public double test(double inputs)
        {
            double predictedOutput = 0;
            for (int i = 0; i < 4; i++)
            {
                predictedOutput += net[i].phi(inputs) * net[i].w;
                //System.out.print(Math.round(net[i].w) +"\t"+Math.round(net[i].c[0])+"\t"+Math.round(net[i].c[1])+"\t");
            }
            return predictedOutput;

        }

        class Unit
        {
            public double w;
            public double[] c = new double[1000];
            public double sigma;
            public double n1 = 0.1;
            public double n2 = 0.1;

            public Unit(double sigma, double[] center, double weight)
            {
                this.sigma = sigma;
                this.c = center;
                this.w = weight;
            }

            public double phi(double[] input)
            {
                double distance = 0;
                for (int i = 0; i < c.Length; i++)
                    distance += Math.Pow(input[i] - c[i], 2);
                return Math.Pow(Math.E, -distance / (2 * Math.Pow(sigma, 2)));
            }
            public double phi(double input)
            {
                double distance = 0;
                for (int i = 0; i < c.Length; i++)
                    distance += Math.Pow(input - c[i], 2);
                return Math.Pow(Math.E, -distance / (2 * Math.Pow(sigma, 2)));
            }

            public void update(double input, double desired, double output)
            {
                double phi1 = phi(input);
                double diffOutput = desired - output;

                for (int i = 0; i < c.Length; i++)
                    c[i] = c[i] + (n1 * diffOutput * w * phi1 * (input - c[i]) / (sigma * sigma));


                w = w + (n2 * diffOutput * phi1);
            }
        }
    }
}