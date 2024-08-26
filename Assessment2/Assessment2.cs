using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Assessment2
{
    public class Assessment2
    {
        private CancellationTokenSource cts = new CancellationTokenSource();
        List<int> globalList = new();
        private readonly object globalListLock = new();

        public async Task Execute()
        {
            Task task1 = Task.Run(() => AddOddNumbersToGlobalList(cts.Token), cts.Token);
            Task task2 = Task.Run(() => AddNegetivePrimeNumbersToGlobalList(cts.Token), cts.Token);
            try
            {
                if (globalList.Count > 250000)
                {
                    Task task3 = Task.Run(() => AddEvenNumbersToGlobalList(cts.Token), cts.Token);
                    await Task.WhenAny(task1, task2, task3);
                }
            }
            catch (OperationCanceledException e)
            {
                cts.Cancel();
                cts.Dispose();

                int[] globalListArray = BucketSortByASC(globalList.ToArray());
                DisplayOddAndEvenNumbers(globalListArray);
                DisplayBinary(globalListArray);
                DisplayXML(globalListArray);

            }
        }

        public Task AddOddNumbersToGlobalList(CancellationToken token)
        {
            int number = 0;
            while (!token.IsCancellationRequested)
            {
                if (globalList.Count == 1000000)
                {
                    Console.WriteLine("AddOddNumbersToGlobalList was canceled");
                    throw new OperationCanceledException();
                }

                if (number % 2 != 0)
                {
                    lock (globalListLock)
                    {
                        globalList.Add(number);
                    }

                }
                number = Random.Shared.Next();

            }
            return Task.CompletedTask;
        }

        public Task AddNegetivePrimeNumbersToGlobalList(CancellationToken token)
        {
            int number = 2;
            while (!token.IsCancellationRequested)
            {
                if (globalList.Count == 1000000)
                {
                    Console.WriteLine("Add negetive PrimeNumbers To GlobalList was canceled");
                    throw new OperationCanceledException();
                }

                if (IsPrime(number))
                {
                    lock (globalListLock)
                    {
                        globalList.Add(number * -1);
                    }

                }
                number++;
                if (globalList.Count == 1000000)
                    throw new OperationCanceledException();
            }
            return Task.CompletedTask;
        }

        public Task AddEvenNumbersToGlobalList(CancellationToken token)
        {
            int number = 1;
            while (!token.IsCancellationRequested)
            {
                if (globalList.Count == 1000000)
                {
                    Console.WriteLine("Add Even Numbers To GlobalList was canceled");
                    throw new OperationCanceledException();
                }
                if (IsEvenNumber(number))
                {
                    globalList.Add(number);
                }
                number++;
            }
            return Task.CompletedTask;
        }

        public static void DisplayOddAndEvenNumbers(int[] input)
        {
            Parallel.For(0, input.Length, i =>
            {
                int EvenNoCount = 0, OddNoCount = 0;

                _ = IsEvenNumber(input[i]) ? EvenNoCount++ : OddNoCount++;

                Console.WriteLine($"The Global List contains {EvenNoCount} numbers and {OddNoCount} Numbers.");
            });
        }
        public static bool IsPrime(int number)
        {
            //Source: Google - Gemini
            if (number <= 1)
            {
                return false; // 1 and below are not prime
            }

            for (int i = 2; i * i <= number; i++)
            {
                if (number % i == 0)
                {
                    return false; // Divisible by i, not prime
                }
            }

            return true; // Not divisible by any number from 2 to its square root, it's prime
        }

        public static int[] BucketSortByASC(int[] input)
        {
            //source https://code-maze.com/csharp-heap-sort/
            List<int> sortedList = [];
            var minValue = input[0];
            var maxValue = input[0];

            if (input == null || input.Length <= 1)
            {
                return [.. input];
            }

            Parallel.For(0, input.Length, i =>
            {
                if (input[i] > maxValue)
                {
                    maxValue = input[i];
                }
                if (input[i] < minValue)
                {
                    minValue = input[i];
                }
            });

            var numberOfBuckets = maxValue - minValue + 1;
            List<int>[] bucket = new List<int>[numberOfBuckets];

            for (int i = 0; i < numberOfBuckets; i++)
            {
                var selectedBucket = (input[i] / numberOfBuckets);
                bucket[selectedBucket].Add(input[i]);
            }

            Parallel.For(0, numberOfBuckets, i =>
            {
                int[] temp = BubbleSort(bucket[i]);
                lock (sortedList)
                {
                    sortedList.AddRange(temp);
                }
            });

            return sortedList.ToArray();
        }

        public static int[] BubbleSort(List<int> input)
        {
            //source https://code-maze.com/csharp-heap-sort/
            for (int i = 0; i < input.Count; i++)
            {
                for (int j = 0; j < input.Count; j++)
                {
                    if (input[i] < input[j])
                    {
                        var temp = input[i];
                        input[i] = input[j];
                        input[j] = temp;
                    }
                }
            }
            return input.ToArray();
        }

        public static bool IsEvenNumber(int input)
        {
            return input % 2 == 0;
        }

        public static void DisplayBinary(int[] input)
        {
            string binaryString = string.Join("", input.Select(num => Convert.ToString(num, 2)));
            Console.WriteLine($"Binary String {binaryString}");
        }

        public static void DisplayXML(int[] input)
        {
            using StringWriter writer = new StringWriter();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(int[]));
            xmlSerializer.Serialize(writer, input);

            Console.WriteLine($"XML {xmlSerializer}");
        }
    }
}
