using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfGenetic.Models;

public class Genetic
{
    private static readonly Random Random = new(); 
    
    // Размер популяции
    private readonly int _numberOfIndivids;

    // Какая часть популяции должна производить потомство
    private readonly double _crossoverRate;

    private readonly int _mutationSteps;

    // Вероятность мутации
    private readonly double _mutationChance;

    // Сколько раз будет появляться новое поколение
    private readonly int _numberLives;

    private readonly int _countOfComponents;
    
    // Координаты найденного минимума
    private List<double> _coordinatesOfMinimum = new ();

    // Найденный минимум
    public double Minimum;

    // Границы поиска минимума
    private readonly double _start;
    private readonly double _end;

    private readonly Func<List<double>, double> _calculateFunction;
    
    public Genetic(int numberOfIndivids, double crossoverRate, int mutationSteps, double mutationChance,
        int numberLives, double start, double end, int countOfComponents, Func<List<double>, double> calculateFunction)
    {
        _numberOfIndivids = numberOfIndivids;
        _crossoverRate = crossoverRate;
        _mutationSteps = mutationSteps;
        _mutationChance = mutationChance;
        _numberLives = numberLives;
        _countOfComponents = countOfComponents;

        for (var i = 0; i < countOfComponents; i++)
        {
            _coordinatesOfMinimum.Add(end);
        }
        
        Minimum = double.MaxValue;
        
        _start = start;
        _end = end;

        _calculateFunction = calculateFunction;
    }
    
    public List<double> StartGenetic()
    {
        var population = new List<Individ>();
        var ranks = new List<(int, int)>();

        var counter = 0;
        for (var i = 1; (int)(_numberOfIndivids * _crossoverRate) >= i; i++)
        {
            counter += i;
            ranks.Add(((int)(_numberOfIndivids * _crossoverRate) - i, counter));
        }
        
        for (var i = 0; i < _numberOfIndivids; i++)
        {
            population.Add(new Individ(_start, _end, _mutationSteps, _countOfComponents, _calculateFunction));
        }
        
        for (var i = 0; i < _numberLives; i++)
        {
            // Сортировка популяции по убыванию пригодности
            population = population.OrderBy(individ => individ.Score).ToList();

            // Отбор индивидов в промежуточную популяцию родителей
            var bestPopulation = population.Take((int)(_numberOfIndivids * _crossoverRate)).ToList();

            var sum = (1 + bestPopulation.Count) / 2 * bestPopulation.Count;
            
            foreach (var firstIndivid in bestPopulation)
            {
                var secondIndivid = bestPopulation.First();
                var randomIndex = Random.Next(1, sum);
                foreach (var (position, rank) in ranks)
                {
                    if (randomIndex <= rank)
                    {
                        secondIndivid = bestPopulation[position];
                        break;
                    }
                }

                while (firstIndivid == secondIndivid)
                {
                    randomIndex = Random.Next(1, sum);
                    foreach (var (position, rank) in ranks)
                    {
                        if (randomIndex <= rank)
                        {
                            secondIndivid = bestPopulation[position];
                            break;
                        }
                    }
                }

                var currentChilds = Crossover(firstIndivid, secondIndivid);
                population.AddRange(currentChilds);
            }

            foreach (var individ in population)
            {
                individ.Mutate(_mutationChance);
                var score = individ.CalculateFunction(individ.Components);
                individ.Score = score;
            }

            population = population.OrderBy(ind => ind.Score).Take(_numberOfIndivids).ToList();
            
            if (population[0].Score < Minimum)
            {
                Minimum = population[0].Score;

                _coordinatesOfMinimum = population[0].Components;
            }
        }

        return population[0].Components;
    }

    // Функция скрещивания
    private IEnumerable<Individ> Crossover(Individ firstParent, Individ secondParent)
    {
        var firstChild = new Individ(_start, _end, _mutationSteps, _countOfComponents, _calculateFunction);
        var secondChild = new Individ(_start, _end, _mutationSteps, _countOfComponents, _calculateFunction);

        for (var i = 0; i < _countOfComponents; i++)
        {
            var alpha = Random.NextDouble() * (1 - 0.01) + 0.01;
            firstChild.Components[i] = firstParent.Components[i] + alpha * (secondParent.Components[i] - firstParent.Components[i]);

            alpha = Random.NextDouble() * (1 - 0.01) + 0.01;
            secondChild.Components[i] = firstParent.Components[i] + alpha * (secondParent.Components[i] - firstParent.Components[i]);
        }

        var firstChildScore = firstChild.CalculateFunction(firstChild.Components);
        firstChild.Score = firstChildScore;
        
        var secondChildScore = secondChild.CalculateFunction(secondChild.Components);
        secondChild.Score = secondChildScore;

        return new List<Individ> { firstChild, secondChild };
    }
}