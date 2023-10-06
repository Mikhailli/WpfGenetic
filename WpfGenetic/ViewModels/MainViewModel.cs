using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WpfGenetic.Models;

namespace WpfGenetic.ViewModels;

public class MainViewModel : ViewModelBase
{
    private bool _isFirstFunctionPicked;
    private bool _isSecondFunctionPicked;
    private bool _isThirdFunctionPicked;
    private bool _isFourthFunctionPicked;
    private bool _isFifthFunctionPicked;

    private string _countOfElements;
    private string _leftBorder;
    private string _rightBorder;

    private string _populationQuantity;
    private string _crossoverRate;
    private string _mutateChance;
    private string _numberOfLives;

    private string _minimum;
    private bool _canStartGenetic;

    private ICommand? _geneticStartCommand;
    
    public bool CanStartGenetic
    {
        get => _canStartGenetic;
        set
        {
            _canStartGenetic = value;
            RaisePropertyChanged(nameof(CanStartGenetic));
        }
    }
    
    public string Minimum
    {
        get => _minimum;
        set
        {
            _minimum = value;
            RaisePropertyChanged(nameof(Minimum));
        }
    }

    public string PopulationQuantity
    {
        get => _populationQuantity;
        set
        {
            _populationQuantity = value;
            RaisePropertyChanged(nameof(PopulationQuantity));
            CanStart();
        }
    }
    public string CrossoverRate
    {
        get => _crossoverRate;
        set
        {
            _crossoverRate = value;
            RaisePropertyChanged(nameof(CrossoverRate));
            CanStart();
        }
    }
    public string MutateChance
    {
        get => _mutateChance;
        set
        {
            _mutateChance = value;
            RaisePropertyChanged(nameof(MutateChance));
            CanStart();
        }
    }
    public string NumberOfLives
    {
        get => _numberOfLives;
        set
        {
            _numberOfLives = value;
            RaisePropertyChanged(nameof(NumberOfLives));
            CanStart();
        }
    }

    public string CountOfElements
    {
        get => _countOfElements;
        set
        {
            _countOfElements = value;
            RaisePropertyChanged(nameof(CountOfElements));
        }
    }

    public string LeftBorder
    {
        get => _leftBorder;
        set
        {
            _leftBorder = value;
            RaisePropertyChanged(nameof(LeftBorder));
        }
    }

    public string RightBorder
    {
        get => _rightBorder;
        set
        {
            _rightBorder = value;
            RaisePropertyChanged(nameof(RightBorder));
        }
    }

    public bool IsFirstFunctionPicked
    {
        get => _isFirstFunctionPicked;
        set
        {
            _isFirstFunctionPicked = value;
            if (_isFirstFunctionPicked)
            {
                CountOfElements = "3";
                LeftBorder = "-5.12";
                RightBorder = "5.12";
            }
            
            CanStart();
        }
    }

    public bool IsSecondFunctionPicked
    {
        get => _isSecondFunctionPicked;
        set
        {
            _isSecondFunctionPicked = value;
            if (_isSecondFunctionPicked)
            {
                CountOfElements = "2";
                LeftBorder = "-2.048";
                RightBorder = "2.048";
            }
            
            CanStart();
        }
    }

    public bool IsThirdFunctionPicked
    {
        get => _isThirdFunctionPicked;
        set
        {
            _isThirdFunctionPicked = value;
            if (_isThirdFunctionPicked)
            {
                CountOfElements = "5";
                LeftBorder = "-5.12";
                RightBorder = "5.12";
            }
            
            CanStart();
        }
    }

    public bool IsFourthFunctionPicked
    {
        get => _isFourthFunctionPicked;
        set
        {
            _isFourthFunctionPicked = value;
            if (_isFourthFunctionPicked)
            {
                CountOfElements = "10";
                LeftBorder = "-600";
                RightBorder = "600";
            }
            
            CanStart();
        }
    }

    public bool IsFifthFunctionPicked
    {
        get => _isFifthFunctionPicked;
        set
        {
            _isFifthFunctionPicked = value;
            if (_isFifthFunctionPicked)
            {
                CountOfElements = "10";
                LeftBorder = "-500";
                RightBorder = "500";
            }

            CanStart();
        }
    }

    public MainViewModel()
    {
        _canStartGenetic = false;
        _minimum = "";
        _isFifthFunctionPicked = false;
        _isSecondFunctionPicked = false;
        _isThirdFunctionPicked = false;
        _isFourthFunctionPicked = false;
        _isFifthFunctionPicked = false;
        _countOfElements = "";
        _leftBorder = "";
        _rightBorder = "";
        PopulationQuantity = "3000";
        CrossoverRate = "0.5";
        MutateChance = "0.5";
        NumberOfLives = "2000";
    }

    public ICommand StartGeneticCommand
    {
        get
        {
            if (_geneticStartCommand is null)
            {
                _geneticStartCommand = new Command(StartGeneticCommandBehavior);
            }

            return _geneticStartCommand;
        }
    }

    private void StartGeneticCommandBehavior(object obj)
    {
        Func<List<double>, double>? function;
        
        if (IsFirstFunctionPicked)
            function = FirstCalculateFunction;
        else if (IsSecondFunctionPicked)
            function = SecondCalculateFunction;
        else if (IsThirdFunctionPicked)
            function = ThirdCalculateFunction;
        else if (IsFourthFunctionPicked)
            function = FourthCalculateFunction;
        else function = FifthCalculateFunction;
        
        var genetic = new Genetic(Convert.ToInt32(PopulationQuantity), 
            Convert.ToDouble(CrossoverRate.Replace('.', ',')), 30, 
            Convert.ToDouble(MutateChance.Replace('.', ',')),
            Convert.ToInt32(NumberOfLives), 
            Convert.ToDouble(LeftBorder.Replace('.', ',')), 
            Convert.ToDouble(RightBorder.Replace('.', ',')), 
            Convert.ToInt32(CountOfElements), function);

        var spaces = "          ";
        
        var stringBuilder = new StringBuilder();
        var coordinatesOfMinimum = genetic.StartGenetic();
        stringBuilder.Append("Минимум:" + spaces + genetic.Minimum.ToString(CultureInfo.InvariantCulture));
        stringBuilder.Append(Environment.NewLine);
        for(var i = 0; i <  coordinatesOfMinimum.Count; i++)
        {
            stringBuilder.Append($"X{i + 1}:");
            var length = i.ToString().Length;
            var difference = 6 - length;
            for (var j = 0; j < difference; j++)
            {
                stringBuilder.Append("   ");
            }
            stringBuilder.Append(spaces);
            stringBuilder.Append($"{coordinatesOfMinimum[i]}");
            stringBuilder.Append(Environment.NewLine);
        }

        Minimum = stringBuilder.ToString();
    }

    private void CanStart()
    {
        if (PopulationQuantity is null || CrossoverRate is null || MutateChance is null || NumberOfLives is null)
            return;
        var isFunctionPicked = IsFirstFunctionPicked || IsSecondFunctionPicked || IsThirdFunctionPicked
                               || IsFourthFunctionPicked || IsFifthFunctionPicked;
        var isPopulationQuantityCorrect = (int.TryParse(PopulationQuantity.Replace('.', ','), out _)
                                            && Convert.ToInt32(PopulationQuantity.Replace('.', ',')) > 0);

        var isCrossoverRateCorrect = (double.TryParse(CrossoverRate.Replace('.', ','), out _)
                                       && Convert.ToDouble(CrossoverRate.Replace('.', ',')) > 0
                                       && Convert.ToDouble(CrossoverRate.Replace('.', ',')) < 1);
        
        var isMutateChanceCorrect = (double.TryParse(MutateChance.Replace('.', ','), out _)
                                       && Convert.ToDouble(MutateChance.Replace('.', ',')) > 0
                                       && Convert.ToDouble(MutateChance.Replace('.', ',')) < 1);
        
        var isNumberOfLivesCorrect = (int.TryParse(NumberOfLives.Replace('.', ','), out _)
                                            && Convert.ToInt32(NumberOfLives.Replace('.', ',')) > 0);

        if (isFunctionPicked && isPopulationQuantityCorrect && isCrossoverRateCorrect && isMutateChanceCorrect &&
            isNumberOfLivesCorrect)
        {
            CanStartGenetic = true;
        }
        else
        {
            CanStartGenetic = false;
        }
    }

    double FirstCalculateFunction(List<double> functionComponents)
    {
        return functionComponents.Sum(component => Math.Pow(component, 2));
    }
    
    double SecondCalculateFunction(List<double> functionComponents)
    {
        return 100 * Math.Pow(Math.Pow(functionComponents[0], 2) - Math.Pow(functionComponents[1], 2), 2)
               + Math.Pow(1 - functionComponents[0], 2);
    }
    
    double ThirdCalculateFunction(List<double> functionComponents)
    {
        return functionComponents.Sum(Math.Floor);
    }
    
    double FourthCalculateFunction(List<double> functionComponents)
    {
        var sum = 1d;
        var multiplication = 1d;
        for (var i = 0; i < functionComponents.Count; i++)
        {
            sum += Math.Pow(functionComponents[i], 2) / 4000;
            multiplication *= Math.Cos(functionComponents[i] / Math.Sqrt(i + 1));
        }

        return sum - multiplication;
    }
    
    double FifthCalculateFunction(List<double> functionComponents)
    {
        var sum = 0d;
        foreach (var component in functionComponents)
        {
            sum -= component * Math.Sin(Math.Sqrt(Math.Abs(component)));
        }

        return sum;
    }
}