using System;
using System.Collections.Generic;

namespace WordGeneticAlgorithm
{
    static class Utility
    {
        //global variables
        public const string target = "I have learnt to say this sentence by myself";
        public const string wordPool = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890, .-;:_!\"#%&/()=?@${[]}";
        
        //global methods
        public static int Divide(List<Individual> population, int low, int high)
        {
            Individual pivot = population[high];

            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (population[j].fitness > pivot.fitness)
                {
                    i++;

                    Individual tmp = population[i];
                    population[i] = population[j];
                    population[j] = tmp;
                }
            }

            Individual tmp1 = population[i + 1];
            population[i + 1] = population[high];
            population[high] = tmp1;

            return i + 1;
        }

        public static Individual Crossover(Individual parent1, Individual parent2)
        {
            Individual child = new Individual();
            for (int i = 0; i < target.Length; i++)
            {
                Random r = new Random();
                int los = r.Next(100);

                if (los < 45)
                {
                    child.genes[i] = parent1.genes[i];
                }

                else if (los < 90)
                {
                    child.genes[i] = parent2.genes[i];
                }
                else
                {
                    //mutation
                    Random r1 = new Random();
                    int los1 = r.Next(86);
                    child.genes[i] = wordPool[los1];
                }
            }
            child.fitness = child.CalculateFitness();
            return child;
        }


        public static void QuickSort(List<Individual> population, int low, int high)
        {
            if (low < high)
            {
                int p = Divide(population, low, high);

                QuickSort(population, low, p - 1);
                QuickSort(population, p + 1, high);
            }
        }
    }


    class Individual
    {
        public char[] genes = new char[Utility.target.Length];
        public int fitness = 0;
        public Individual()
        {
            for (int i = 0; i < genes.Length; i++)
            {
                Random r = new Random();
                genes[i] = Utility.wordPool[r.Next(86)];
            }
            fitness = CalculateFitness();
        }

        public int CalculateFitness()
        {
            int tmpFitness = 0;
            for (int i = 0; i < genes.Length; i++)
            {
                if (genes[i] == Utility.target[i])
                {
                    tmpFitness++;
                }
            }
            return tmpFitness;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < genes.Length; i++)
            {
                s += genes[i];
            }
            return s;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //success
            bool success = false;

            //population count
            int p = 100;

            //current generation
            int generation = 0;

            //initial population
            List<Individual> population = new List<Individual>();
            
            for (int i = 0; i < p; i++)
            {
                population.Add(new Individual());
            }

            Console.WriteLine();

            while (!success)
            {
                //sort population by fitness
                Utility.QuickSort(population, 0, population.Count - 1);
               
                if (population[0].fitness >= Utility.target.Length)
                {
                    success = true;
                    break;
                }

                //new population
                List<Individual> newPopulation = new List<Individual>();

                //top 10% of population will go to new one
                int l = (10 * population.Count) / 100;
                for (int i = 0; i < l; i++)
                {
                    newPopulation.Add(population[i]);
                }

                //rest 90% is going to be crossovered or mutated
                l = (90 * population.Count) / 100;
                for (int i = 0; i < l; i++)
                {
                    //choose random parents for a child from top 50% of the population
                    Random r = new Random();
                    int los1 = r.Next(population.Count/2);
                    int los2 = r.Next(population.Count/2);

                    Individual parent1 = population[los1];
                    Individual parent2 = population[los2];

                    Individual child = Utility.Crossover(parent1, parent2);

                    newPopulation.Add(child);
                }

                population = newPopulation;
                generation++;

                if(generation % 1000 == 0)
                {
                    Console.WriteLine(generation + " => " + population[0].fitness + " String: " + population[0]);
                }
            }
            Console.WriteLine(generation + " => " + population[0].fitness + " String: " + population[0]);
        }
    }
}
