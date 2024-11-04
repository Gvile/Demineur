using Microsoft.AspNetCore.Components;

namespace Demineur.Client.Components.Pages;

public class GameBase : ComponentBase
{
    #region Variables

    [Parameter] public int Id { get; set; }

    public Cell[,] Grid = new Cell[8, 8];

    private int _bombCount = 10;

    #endregion

    #region ComponentBase

    protected override void OnInitialized()
    {
        CreateGame();
    }

    #endregion

    #region Fonctions

    private void CreateGame()
    {
        InitGrid();
        AddBombs();
        AddNumbers();
    }

    private void InitGrid()
    {
        Grid = new Cell[8, 8];

        for (var row = 0; row < Grid.GetLength(0); row++) // Grid.GetLength(0) = X donc 1er valeur dans ma grid
        {
            for (var column = 0; column < Grid.GetLength(1); column++) // Grid.GetLength(1) = Y donc 2eme valeur dans ma grid
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
                cell.Number = random.Next(10);
            }
        }
    }

    #endregion
}