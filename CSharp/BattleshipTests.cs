using System.Diagnostics;
using Xunit;

public class BattleshipTests : IDisposable
{
    private Process process = new Process();

    public BattleshipTests()
    {
        process.StartInfo.FileName = "Battleship.exe";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
    }

    public void Dispose()
    {
        process.Close();
    }

    StreamWriter Input
    {
        get
        {
            return process.StandardInput;
        }
    }

    StreamReader Output
    {
        get
        {
            return process.StandardOutput;
        }
    }

    private void Start(string arguments = "")
    {
        process.StartInfo.Arguments = arguments;
        process.Start();
    }

    private string Read(int length)
    {
        Span<Char> buffer = new char[length];

        Output.ReadBlock(buffer);

        return buffer.ToString();
    }

    private void ExpectPrompt(string expected)
    {
        Assert.Equal(expected, Read(expected.Length));
    }

    private void IgnorePrompt(string expected)
    {
        Read(expected.Length);
    }

    private void IgnoreGrids()
    {
        for (int i = 0; i < 13; i++)
            Output.ReadLine();
    }

    private void Enter(string input)
    {
        Input.WriteLine(input);
    }

    private void ExpectPlayerGrid()
    {
        ExpectPlayerGrid(
            "          ",
            "          ",
            "          ",
            "          ",
            "          ",
            "          ",
            "          ",
            "          ",
            "          ",
            "          ");
    }

    private void ExpectPlayerGrid(params string[] gridRows)
    {
        ExpectLine(" #==========#");

        int rowNum = 0;

        foreach (var row in gridRows)
            Assert.Equal("" + rowNum++ + "|" + row + "|", Output.ReadLine().Replace("~", " "));

        ExpectLine(" #==========#");
        ExpectLine("  ABCDEFGHIJ");
    }

    private void ExpectGrids(params string[] gridRows)
    {
        ExpectLine(" #==========#   #==========#");

        for (int i = 0; i < gridRows.Length / 2; i++)
            Assert.Equal("" + i + "|" + gridRows[2*i] + "|   |" + gridRows[2*i + 1] + "|", Output.ReadLine().Replace("~", " "));

        ExpectLine(" #==========#   #==========#");
        ExpectLine("  ABCDEFGHIJ     ABCDEFGHIJ");
    }

    private void ExpectErrorMessage(string prompt, string expected, string input)
    {
        IgnoreGrids();
        ExpectPrompt(prompt);
        Enter(input);
        ExpectLine(expected);
    }

    private void ExpectPlaceErrorMessage(string expected, string input)
    {
        ExpectErrorMessage("Place ship with length 5: ", expected, input);
    }

    private void ExpectTargetErrorMessage(string expected, string input)
    {
        ExpectErrorMessage("Enter target coordinate: ", expected, input);
    }

    private void SetupPlayerGrid()
    {
        IgnoreGrids();
        IgnorePrompt("Place ship with length ?: ");
        Enter("A0V");
        IgnoreGrids();
        IgnorePrompt("Place ship with length ?: ");
        Enter("B0V");
        IgnoreGrids();
        IgnorePrompt("Place ship with length ?: ");
        Enter("C0V");
        IgnoreGrids();
        IgnorePrompt("Place ship with length ?: ");
        Enter("D0V");
        IgnoreGrids();
        IgnorePrompt("Place ship with length ?: ");
        Enter("E0V");
    }

    private void SetupGrids()
    {
        ComputerPlacesShipAt("B1V");
        ComputerPlacesShipAt("C1V");
        ComputerPlacesShipAt("D1V");
        ComputerPlacesShipAt("E1V");
        ComputerPlacesShipAt("F1V");

        SetupPlayerGrid();
    }

    private void ComputerPlacesShipAt(string position)
    {
        EnterCoordinate(position);
        Enter(position[2] == 'H' ? "0" : "1");
    }

    private void TargetCoordinate(string coordinate, string expected = null)
    {
        ExpectPrompt("Enter target coordinate: ");
        Enter(coordinate);
        ExpectLine(expected);
    }

    private void ComputerTargesCoordinate(string coordinate, string expected = null)
    {
        EnterCoordinate(coordinate);
        ExpectLine("Computer is targeting coordinate " + coordinate);
        ExpectLine(expected);
    }

    private void ExpectLine(string expected)
    {
        string actual = Output.ReadLine();

        if (expected != null)
            Assert.Equal(expected, actual);
    }

    private void EnterCoordinate(string coordinate)
    {
        Enter(Convert.ToString(coordinate[0] - 'A'));
        Enter(Convert.ToString(coordinate[1]));
    }

    private void PlayOneTurn(string playerTarget, string computerTarget)
    {
        TargetCoordinate(playerTarget);
        ComputerTargesCoordinate(computerTarget);
        IgnoreGrids();
    }

    [Fact]
    void PlaceShipPromptIsShown()
    {
        Start();

        ExpectPlayerGrid();
        ExpectPrompt("Place ship with length 5: ");
    }

    [Fact]
    void InvalidInputGivesErrorMessage()
    {
        Start();

        ExpectPlaceErrorMessage("Invalid format", "hello");
        ExpectPlaceErrorMessage("Invalid X", "123");
        ExpectPlaceErrorMessage("Invalid Y", "AAA");
        ExpectPlaceErrorMessage("Invalid orientation", "A1A");
        ExpectPlaceErrorMessage("Invalid placement", "J9H");
    }

    [Fact]
    void ShipsArePlacedOnPlayerGrid()
    {
        Start();

        ExpectPlayerGrid();
        ExpectPrompt("Place ship with length 5: ");
        Enter("A0H");
        ExpectPlayerGrid(
            "1         ",
            "1         ",
            "1         ",
            "1         ",
            "1         ",
            "          ",
            "          ",
            "          ",
            "          ",
            "          ");

        ExpectPrompt("Place ship with length 4: ");
        Enter("B1V");
        ExpectPlayerGrid(
            "1         ",
            "12222     ",
            "1         ",
            "1         ",
            "1         ",
            "          ",
            "          ",
            "          ",
            "          ",
            "          ");

        ExpectPrompt("Place ship with length 3: ");
        Enter("C2H");
        ExpectPlayerGrid(
            "1         ",
            "12222     ",
            "1 3       ",
            "1 3       ",
            "1 3       ",
            "          ",
            "          ",
            "          ",
            "          ",
            "          ");

        ExpectPrompt("Place ship with length 2: ");
        Enter("D3V");
        ExpectPlayerGrid(
            "1         ",
            "12222     ",
            "1 3       ",
            "1 344     ",
            "1 3       ",
            "          ",
            "          ",
            "          ",
            "          ",
            "          ");

        ExpectPrompt("Place ship with length 2: ");
        Enter("E4H");
        ExpectGrids(
            "1         ", "          ",
            "12222     ", "          ",
            "1 3       ", "          ",
            "1 344     ", "          ",
            "1 3 5     ", "          ",
            "    5     ", "          ",
            "          ", "          ",
            "          ", "          ",
            "          ", "          ",
            "          ", "          ");
    }

    [Fact]
    void InvalidTargetCoordinateGivesErrorMessage()
    {
        Start();

        SetupPlayerGrid();

        ExpectTargetErrorMessage("Invalid format", "hel");
        ExpectTargetErrorMessage("Invalid X", "12");
        ExpectTargetErrorMessage("Invalid Y", "AA");
    }

    [Fact]
    void MissedTargetIsMarkedOnGrid()
    {
        Start("test");

        SetupGrids();
        IgnoreGrids();
        TargetCoordinate("A0", "Missed");
        ComputerTargesCoordinate("J9", "Missed");
        ExpectGrids(
            "11111     ", "o         ",
            "2222      ", "          ",
            "333       ", "          ",
            "44        ", "          ",
            "55        ", "          ",
            "          ", "          ",
            "          ", "          ",
            "          ", "          ",
            "          ", "          ",
            "         o", "          ");
    }

    [Fact]
    void HitTargetIsMarkedOnGrid()
    {
        Start("test");

        SetupGrids();
        IgnoreGrids();
        TargetCoordinate("B1", "Hit");
        ComputerTargesCoordinate("A0", "Hit");
        ExpectGrids(
            "X1111     ", "          ",
            "2222      ", " X        ",
            "333       ", "          ",
            "44        ", "          ",
            "55        ", "          ",
            "          ", "          ",
            "          ", "          ",
            "          ", "          ",
            "          ", "          ",
            "          ", "          ");
    }

    [Fact]
    void BattleshipIsSunk()
    {
        Start("test");

        SetupGrids();
        IgnoreGrids();
        PlayOneTurn("B1", "A0");
        PlayOneTurn("B2", "A1");
        PlayOneTurn("B3", "A2");
        PlayOneTurn("B4", "A3");
        TargetCoordinate("B5", "Hit");
        ExpectLine("You sunk my battleship");
        ComputerTargesCoordinate("A4", "Hit");
        ExpectLine("I sunk your battleship");
    }

    [Fact]
    void ComputerWinsGame()
    {
        Start("test");

        SetupGrids();
        IgnoreGrids();
        PlayOneTurn("G0", "A0");
        PlayOneTurn("G1", "A1");
        PlayOneTurn("G2", "A2");
        PlayOneTurn("G3", "A3");
        TargetCoordinate("G4");
        ComputerTargesCoordinate("A4", "Hit");
        ExpectLine("I sunk your battleship");

        IgnoreGrids();
        PlayOneTurn("G5", "B0");
        PlayOneTurn("G6", "B1");
        PlayOneTurn("G7", "B2");
        TargetCoordinate("G8");
        ComputerTargesCoordinate("B3", "Hit");
        ExpectLine("I sunk your battleship");

        IgnoreGrids();
        PlayOneTurn("G9", "C0");
        PlayOneTurn("H0", "C1");
        TargetCoordinate("H1");
        ComputerTargesCoordinate("C2", "Hit");
        ExpectLine("I sunk your battleship");

        IgnoreGrids();
        PlayOneTurn("H2", "D0");
        TargetCoordinate("H3");
        ComputerTargesCoordinate("D1", "Hit");
        ExpectLine("I sunk your battleship");

        IgnoreGrids();
        PlayOneTurn("H4", "E0");
        TargetCoordinate("H5");
        ComputerTargesCoordinate("E1", "Hit");
        ExpectLine("I sunk your battleship");
        ExpectLine("You loose!");

        process.WaitForExit(1000);
        Assert.True(process.HasExited);
    }

    [Fact]
    void PlayerWinsGame()
    {
        Start("test");

        SetupGrids();
        IgnoreGrids();
        PlayOneTurn("B1", "F0");
        PlayOneTurn("B2", "F1");
        PlayOneTurn("B3", "F2");
        PlayOneTurn("B4", "F3");
        TargetCoordinate("B5");
        ExpectLine("You sunk my battleship");
        ComputerTargesCoordinate("F4");

        IgnoreGrids();
        PlayOneTurn("C1", "G0");
        PlayOneTurn("C2", "G1");
        PlayOneTurn("C3", "G2");
        TargetCoordinate("C4");
        ExpectLine("You sunk my battleship");
        ComputerTargesCoordinate("G3");

        IgnoreGrids();
        PlayOneTurn("D1", "H0");
        PlayOneTurn("D2", "H1");
        TargetCoordinate("D3");
        ExpectLine("You sunk my battleship");
        ComputerTargesCoordinate("H2");

        IgnoreGrids();
        PlayOneTurn("E1", "I0");
        TargetCoordinate("E2");
        ExpectLine("You sunk my battleship");
        ComputerTargesCoordinate("I1");

        IgnoreGrids();
        PlayOneTurn("F1", "J0");
        TargetCoordinate("F2");
        ExpectLine("You sunk my battleship");
        ExpectLine("You win");

        process.WaitForExit(1000);
        Assert.True(process.HasExited);
    }
}
