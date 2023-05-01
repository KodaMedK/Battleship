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
