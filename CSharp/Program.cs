int[] shipLengths = new int[] { 5, 4, 3, 2, 2 };
int[] shipIds = new int[] { 1, 2, 3, 4, 5 };
int[,] playerGrid = new int[10, 10];
int[,] computerGrid = new int[10, 10];
Random random = new Random();

for (int i = 0; i < 5; i++)
{
    for (;;)
    {
        int x = random.Next(10);
        int y = random.Next(10);
        int z = random.Next(2);

        if (z == 0 && x + shipLengths[i] <= 10 || z == 1 && y + shipLengths[i] <= 10)
        {
            if (z == 0)
            {
                for (int j = 0; j < shipLengths[i]; j++)
                {
                    computerGrid[x + j, y] = shipIds[i];
                }
            }
            else
            {
                for (int j = 0; j < shipLengths[i]; j++)
                {
                    computerGrid[x, y + j] = shipIds[i];
                }
            }

            break;
        }
    }
}

for (int i = 0; i < 5; i++)
{
    for (;;)
    {
        Console.WriteLine(" #==========#");

        for (int row = 0; row < 10; row++)
        {
            Console.Write(row + "|");

            for (int col = 0; col < 10; col++)
            {
                if (playerGrid[row, col] == 0)
                {
                    if (random.Next(5) > 0)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("~");
                    }
                }
                else
                {
                    Console.Write(playerGrid[row, col]);
                }
            }

            Console.WriteLine("|");
        }

        Console.WriteLine(" #==========#");
        Console.WriteLine("  ABCDEFGHIJ");
        Console.Write("Place ship with length " + shipLengths[i] + ": ");

        String pos = Console.ReadLine();

        if (pos.Length == 3)
        {
            if ('A' <= pos[0] && pos[0] <= 'J')
            {
                if ('0' <= pos[1] && pos[1] <= '9')
                {
                    if (pos[2] == 'H' || pos[2] == 'V')
                    {
                        if (pos[2] == 'H' && pos[0] - 'A' + shipLengths[i] <= 10
                            || pos[2] == 'V' && pos[1] - '0' + shipLengths[i] <= 10)
                        {
                            if (pos[2] == 'H')
                            {
                                for (int j = 0; j < shipLengths[i]; j++)
                                {
                                    playerGrid[pos[0] - 'A' + j, pos[1] - '0'] = shipIds[i];
                                }
                            }
                            else
                            {
                                for (int j = 0; j < shipLengths[i]; j++)
                                {
                                    playerGrid[pos[0] - 'A', pos[1] - '0' + j] = shipIds[i];
                                }
                            }

                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid placement");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid orientation");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Y");
                }
            }
            else
            {
                Console.WriteLine("Invalid X");
            }
        }
        else
        {
            Console.WriteLine("Invalid format");
        }
    }
}

for (;;)
{
    for (;;)
    {
        Console.WriteLine(" #==========#   #==========#");

        for (int row = 0; row < 10; row++)
        {
            Console.Write(row + "|");

            for (int col = 0; col < 10; col++)
            {
                if (playerGrid[row, col] == 0)
                {
                    if (random.Next(5) > 0)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("~");
                    }
                }
                else if (playerGrid[row, col] == -1)
                {
                    Console.Write("o");
                }
                else if (playerGrid[row, col] == -2)
                {
                    Console.Write("X");
                }
                else
                {
                    Console.Write(playerGrid[row, col]);
                }
            }

            Console.Write("|   |");

            for (int col = 0; col < 10; col++)
            {
                if (computerGrid[row, col] == 0)
                {
                    if (random.Next(5) > 0)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("~");
                    }
                }
                else if (computerGrid[row, col] == -1)
                {
                    Console.Write("o");
                }
                else if (computerGrid[row, col] == -2)
                {
                    Console.Write("X");
                }
                else
                {
                    Console.Write(" ");
                }
            }

            Console.WriteLine("|");
        }

        Console.WriteLine(" #==========#   #==========#");
        Console.WriteLine("  ABCDEFGHIJ     ABCDEFGHIJ");
        Console.Write("Enter target coordinate: ");

        String pos = Console.ReadLine();

        if (pos.Length == 2)
        {
            if ('A' <= pos[0] && pos[0] <= 'J')
            {
                if ('0' <= pos[1] && pos[1] <= '9')
                {
                    if (computerGrid[pos[0] - 'A', pos[1] - '0'] == 0)
                    {
                        Console.WriteLine("Missed");
                        computerGrid[pos[0] - 'A', pos[1] - '0'] = -1;
                    }
                    else if (computerGrid[pos[0] - 'A', pos[1] - '0'] == 1)
                    {
                        Console.WriteLine("Hit");

                        int id = computerGrid[pos[0] - 'A', pos[1] - '0'];

                        computerGrid[pos[0] - 'A', pos[1] - '0'] = -2;

                        bool sunk = true;

                        for (int row = 0; row < 10; row++)
                        {
                            for (int col = 0; col < 10; col++)
                            {
                                if (computerGrid[row, col] == id)
                                {
                                    sunk = false;

                                    break;
                                }
                            }
                        }

                        if (sunk)
                        {
                            Console.WriteLine("You sunk my battleship");
                        }
                    }

                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Y");
                }
            }
            else
            {
                Console.WriteLine("Invalid X");
            }
        }
        else
        {
            Console.WriteLine("Invalid format");
        }
    }

    bool winner = true;

    for (int row = 0; row < 10; row++)
    {
        for (int col = 0; col < 10; col++)
        {
            if (computerGrid[row, col] > 0)
            {
                winner = false;

                break;
            }
        }
    }

    if (winner)
    {
        Console.WriteLine("You win");

        break;
    }

    for (;;)
    {
        int x = random.Next(10);
        int y = random.Next(10);

        if (playerGrid[x, y] >= 0)
        {
            Console.WriteLine("Computer is targeting coordinate " + (char)('A' + x) + y);

            if (playerGrid[x, y] == 0)
            {
                Console.WriteLine("Missed");
                playerGrid[x, y] = -1;
            }
            else
            {
                Console.WriteLine("Hit");

                int id = playerGrid[x, y];

                playerGrid[x, y] = -2;

                bool sunk = true;

                for (int row = 0; row < 10; row++)
                {
                    for (int col = 0; col < 10; col++)
                    {
                        if (playerGrid[row, col] == id)
                        {
                            sunk = false;

                            break;
                        }
                    }
                }

                if (sunk)
                {
                    Console.WriteLine("I sunk your battleship");
                }
            }

            break;
        }
    }

    bool loser = true;

    for (int row = 0; row < 10; row++)
    {
        for (int col = 0; col < 10; col++)
        {
            if (playerGrid[row, col] > 0)
            {
                loser = false;

                break;
            }
        }
    }

    if (loser)
    {
        Console.WriteLine("You loose!");

        break;
    }
}
