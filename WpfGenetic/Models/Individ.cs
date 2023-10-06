using System;
using System.Collections.Generic;

namespace WpfGenetic.Models;

public class Individ
{
    private static readonly Random Random = new ();
        
    // Границы поиска минимума
    private readonly double _start;
    private readonly double _end;

    private readonly int _mutationSteps;

    private readonly int _countOfComponents;

    public readonly List<double> Components = new ();
    
    public double Score;

    public readonly Func<List<double>, double> CalculateFunction;
        
    public Individ(double start, double end, int mutationSteps, int countOfComponents, Func<List<double>, double> calculateFunction)
    {
        _start = start;
        _end = end;
        
        _mutationSteps = mutationSteps;

        _countOfComponents = countOfComponents;

        for (var i = 0; i < countOfComponents; i++)
        {
            Components.Add(GetRandomDoubleFromRange(start, end));
        }

        CalculateFunction = calculateFunction;
        
        Score = CalculateFunction(Components);
    }
    
    public void Mutate(double mutateChance)
    {
        if (Random.NextDouble() <= mutateChance)
        {
            for (var i = 0; i < _countOfComponents; i++)
            {
                var component = MutateComponent(Components[i]);
                Components[i] = component;
            }
        }
    }
    
    private double MutateComponent(double component)
    {
        var deltaX = 0d;

        for (var i = 0; i < _mutationSteps; i++)
        {
            if (Random.NextDouble() < 1d / _mutationSteps)
            {
                deltaX += 1d / Math.Pow(2, i);
            }
        }

        if (Random.NextDouble() > 0.5)
        {
            deltaX *= _end;
        }
        else
        {
            deltaX *= _start;
        }

        component += deltaX;
        ChangeInvalidComponent(ref component);
        
        return component;
    }

    private void ChangeInvalidComponent(ref double component)
    {
        if (component < _start)
        {
            component = _start;
        }

        if (component > _end)
        {
            component = _end;
        }
    }
    
    private static double GetRandomDoubleFromRange(double start, double end)
    {
        return Random.NextDouble() * (end - start) + start;
    }
}