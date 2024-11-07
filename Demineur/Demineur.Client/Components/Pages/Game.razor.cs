using Microsoft.AspNetCore.Components;

namespace Demineur.Client.Components.Pages;

public class GameBase : ComponentBase
{
    #region Variables

    [Parameter] public int Id { get; set; }

    public Cell[,] Grid = new Cell[8, 8];

    private int _bombCount = 10;
    public bool IsGameOver;
    public bool IsWin;

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
        if (cell.IsBomb)
        {
            IsGameOver = true;
            RevealAllCells();
        }
        else
        {
            RevealAdjacentCells(cell);
            CheckWin();
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
    }

    #endregion
}