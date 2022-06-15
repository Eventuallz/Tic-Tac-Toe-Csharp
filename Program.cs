// Constant variables for numbers so that possible errors are avoided.
const int Zero = 0;
const int Three = 3;

// Array for the playground.
char[,] board = new char[3, 3];

// Creating the characters and setting the starter player.
const char p1 = 'x';
const char p2 = 'o';

char turn = p1;

bool gameOver = false;

// Counting the moves played. This will be necessary if there is a draw.
int movesPlayed = 0;

// Initializing has to happen before the while loop, because all the values stored inside the array,
// aka the progress, aka the dots on the board are going to disappear, because they aren't being stored into the
// array. The while loop restarts every time, thus destroying everything that was stored in it. The only thing that
// always has to occur is the image of the board, it can be reloaded. The actual logic behind the game, however,
// has to be stored somewhere.
Initialize();

// Game runs as long as there is a win/draw.
while (!gameOver)
{
    Console.Clear();

    Print(board);

    Enter();

    CheckWin();

    Turn();
}

void Turn()
{
    if (turn == p1) // If the player one placed its character, player two gets its turn.
    {
        turn = p2;
    }
    else if (turn == p2) // If player two played its turn, player one gets its turn again.
    {
        turn = p1;
    }
}

void Print(char[,] board)
{
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("y".PadLeft(2));
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.White;
    for (int y = board.GetLength(1) - 1; y >= Zero; y--)
    {
        Console.Write($" {y} | ");                          // Start of the row.
        for (int x = 0; x < board.GetLength(0); x++)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(board[x, y]);                     // Printing the values stored in x and y onto the screen.
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" | ");                           // Repeat 3 columns(on y axis) in the x row.
        }
        Console.WriteLine();                                // Start of the next row, aka, doing the 3 columns again.
    }
    Console.Write("     0   1   2  ");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write("x".PadLeft(3));
    Console.ForegroundColor = ConsoleColor.White;
}

void Initialize()
{
    for (int y = board.GetLength(1) - 1; y >= Zero; y--)
    {
        for (int x = 0; x < board.GetLength(0); x++)
        {
            board[x, y] = ' ';      // Putting an empty space into the boxes on the board.
        }
    }
}

// This is the screen for entering the coordinates / to place characters on the board.
void Enter()
{
    int x = 0, y = 0;
    Console.WriteLine($"\n\n{turn}'s turn.");
    while (true)
    {
        try
        {
            Console.Write("\nEnter x coordinate: ");
            x = int.Parse(Console.ReadLine());

            Console.Write("Enter y coordinate: ");
            y = int.Parse(Console.ReadLine());

            if (x > 2 || x < 0)
            {
                Console.WriteLine("Coordinates can't be bigger than 2 or smaller than 0.");
                continue;
            }

            if (y > 2 || y < 0)
            {
                Console.WriteLine("Coordinates can't be bigger than 2 or smaller than 0.");
                continue;
            }
            break;
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Please try again...");
        }
        catch (FormatException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Please try again...");
        }
        catch (IOException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Please try again...");
        }
        catch (OutOfMemoryException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Please try again...");
        }
    }

    Console.WriteLine($" Row: {x}\n Column: {y}");

    // In the same scope, we have to check if player one or two placed their characters on eachother.
    if (!CheckCollision(x, y))
        board[x, y] = turn;

    // TODO:
    // Instance for if collision is true, change the player afterwards.
}

void CheckWin()
{
    // Check horizontal axis: x = 0-2.
    for (int i = 0; i < Three; i++)
    {
        if (turn == board[i, 0] && turn == board[i, 1] && turn == board[i, 2])
        {
            WinSchreen();
        }
    }

    // Check vertical axis: y = 0-2.
    for (int i = 0; i < Three; i++)
    {
        if (turn == board[0, i] && turn == board[1, i] && turn == board[2, i])
        {
            WinSchreen();
        }
    }

    // Check diagonal (slope of 1 and -1): x = 0, 1, 2 ; y = 0, 1, 2.
    if (turn == board[0, 0] && turn == board[1, 1] && turn == board[2, 2])
    {
        WinSchreen();
    }

    else if (turn == board[0, 2] && turn == board[1, 1] && turn == board[2, 0])
    {
        WinSchreen();
    }

    // If all possible moves are played, and non of the above conditions were fulfilled, the code below will be executed.
    // The game in this case is a draw.
    movesPlayed = movesPlayed + 1;
    if (movesPlayed == 9)
    {
        Console.WriteLine("Draw! Do you want to play again?\n [y] yes\n [n] no");
        ConsoleKeyInfo key = Console.ReadKey();
        switch (key.Key)
        {
            case ConsoleKey.Y:
                gameOver = false;
                Initialize();       // The board has to be reinitialized, unless it would reload the already finished game.
                movesPlayed = 0;
                turn = 'x';
                break;

            case ConsoleKey.N:
                gameOver = true;
                break;

            default:
                gameOver = false;
                Initialize();
                movesPlayed = 0;
                turn = 'x';
                break;
        }
    }

    // This screen is executed if one of the players wins the game.
    void WinSchreen()
    {
        Console.WriteLine($"\n{turn} has won the game!\n Do you want to play again?\n [y] yes\n [n] no");
        ConsoleKeyInfo key = Console.ReadKey();
        switch (key.Key)
        {
            case ConsoleKey.Y:
                gameOver = false;
                Initialize();
                movesPlayed = 0;
                turn = p1;
                break;

            case ConsoleKey.N:
                gameOver = true;
                break;

            default:
                gameOver = false;
                Initialize();
                movesPlayed = 0;
                turn = p1;
                break;
        }
    }
}

bool CheckCollision(int x, int y)
{
    // Vertical axis
    char check = turn;
    if (turn == p1 && board[x, y] != check && board[x, y] != ' ')
    {
        Console.WriteLine("Collision!");
        Console.ReadKey();
        turn = p2;
        movesPlayed--;
        return true;
    }
    if (turn == p2 && board[x, y] != check && board[x, y] != ' ')
    {
        Console.WriteLine("Collision!");
        Console.ReadKey();
        turn = p1;
        movesPlayed--;
        return true;
    }
    return false;
}