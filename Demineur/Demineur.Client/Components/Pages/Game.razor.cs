using Microsoft.AspNetCore.Components;

namespace Demineur.Client.Components.Pages;

public class GameBase : ComponentBase
{
    #region Variables

    [Parameter] public int Id { get; set; }

    public Cell[,] Grid = new Cell[8, 8];
    
    private System.Timers.Timer? _timer;
    private int _secondsElapsed;

    private int _bombCount = 10;
    public bool IsGameOver;
    public bool IsWin;
    
    private bool _timerIsStarted;

    #endregion

    #region ComponentBase

    protected override void OnInitialized()
    {
        CreateGame();
    }

    #endregion

    #region Fonctions

    public void CreateGame()
    {
        Init();
        InitGrid();
        AddBombs();
        AddNumbers();
    }

    private void Init()
    {
        if (_timer is not null)
        {
            _timer.Stop();
            _timer.Dispose();
            _timerIsStarted = false;
            _secondsElapsed = 0;
        }
        
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += UpdateTime;
        
        IsGameOver = false;
        IsWin = false;

        if (Id == 0)
        {
            _bombCount = 5;
            Grid = new Cell[8, 8];
            
        }
        else if (Id == 1)
        {
            _bombCount = 12;
            Grid = new Cell[10, 10];
        }
        else if (Id == 2)
        {
            _bombCount = 30;
            Grid = new Cell[12, 12];
        }
    }

    private void InitGrid()
    {

        for (var row = 0; row < Grid.GetLength(0); row++) // Grid.GetLength(0) = X donc 1er valeur dans ma grid
        {
            for (var column = 0;
                 column < Grid.GetLength(1);
                 column++) // Grid.GetLength(1) = Y donc 2eme valeur dans ma grid
            {
                Grid[row, column] = new Cell(row, column);
            }
        }
    }

    private void AddBombs()
    {
        var random = new Random();

        for (var i = 0; i < _bombCount; i++)
        {
            var randomRow = random.Next(Grid.GetLength(0));
            var randomColumn = random.Next(Grid.GetLength(1));

            var cell = Grid[randomRow, randomColumn];
            if (cell.IsBomb)
            {
                i--;
            }
            else
            {
                cell.IsBomb = true;
            }
        }
    }

    private void AddNumbers()
    {
        var random = new Random();

        for (var row = 0; row < Grid.GetLength(0); row++)
        {
            for (var column = 0; column < Grid.GetLength(1); column++)
            {
                var cell = Grid[row, column];

                if (cell.IsBomb)
                {
                    continue;
                }

                var bombCount = 0;

                for (var x = row - 1; x <= row + 1; x++)
                {
                    for (var y = column - 1; y <= column + 1; y++)
                    {
                        // || dans une condition if, signifie OU
                        // && dans une condition if, signifie ET

                        if (x <= -1 || x > Grid.GetLength(0) - 1 || y <= -1 || y > Grid.GetLength(1) - 1)
                        {
                            continue;
                        }

                        var infoBomb = Grid[x, y].IsBomb;

                        if (infoBomb == true)
                        {
                            bombCount++;
                        }
                    }
                }

                cell.Number = bombCount;
            }
        }
    }

    public void OnButtonClick(Cell cell)
    {
        if (_timer is not null || _timerIsStarted)
        {
            _timer.Start();
            _timerIsStarted = true;
        }
        
        if (cell.IsFlag)
        {
            
        }
        else
        {
            if (cell.IsBomb)
            {
                IsGameOver = true;
                RevealAllCells();
                if (_timer is not null)
                {
                    _timer.Stop();
                    _timer.Elapsed -= UpdateTime;
                }
            }
            else
            {
                RevealAdjacentCells(cell);
                CheckWin();
            }  
        }
        
    }

    public void SetFlag(Cell cell)
    {
        if (cell.IsDiscovered)
        {
            
        }
        else
        {
            cell.IsFlag = !cell.IsFlag;
        }
    }

    private void RevealAdjacentCells(Cell cell)
    {
        if (cell.IsDiscovered)
        {
            return;
        }

        cell.IsDiscovered = true;

        if (cell.Number == 0)
        {
            for (var x = cell.Row - 1; x <= cell.Row + 1; x++)
            {
                for (var y = cell.Column - 1; y <= cell.Column + 1; y++)
                {
                    if (x <= -1 || x > Grid.GetLength(0) - 1 || y <= -1 || y > Grid.GetLength(1) - 1)
                    {
                        continue;
                    }

                    var cellBis = Grid[x, y];

                    if (cellBis.IsDiscovered)
                    {
                        continue;
                    }

                    RevealAdjacentCells(cellBis);
                }
            }
        }
    }

    private void RevealAllCells()
    {
        for (var row = 0; row < Grid.GetLength(0); row++)
        {
            for (var column = 0; column < Grid.GetLength(1); column++)
            {
                var cell = Grid[row, column];
                cell.IsDiscovered = true;
            }
        }
    }

    private void CheckWin()
    {
        for (var row = 0; row < Grid.GetLength(0); row++)
        {
            for (var column = 0; column < Grid.GetLength(1); column++)
            {
                var cell = Grid[row, column];

                if (!cell.IsDiscovered && !cell.IsBomb)
                {
                    return;
                }
            }
        }

        IsWin = true;
        RevealAllCells();
        if (_timer is not null)
        {
            _timer.Stop();
            _timer.Elapsed -= UpdateTime;
        }
    }
    
    private void UpdateTime(object? sender, System.Timers.ElapsedEventArgs e)
    {
        _secondsElapsed++;
        InvokeAsync(StateHasChanged);
    }
    
    public string FormatTime()
    {
        var minutes = _secondsElapsed / 60;
        var seconds = _secondsElapsed % 60;
        return $"{minutes:D2}:{seconds:D2}";
    }

    #endregion
}