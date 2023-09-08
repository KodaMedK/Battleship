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

        process.Start();
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
        Assert.Equal(" #==========#", Output.ReadLine());

        int rowNum = 0;

        foreach (var row in gridRows)
            Assert.Equal("" + rowNum++ + "|" + row + "|", Output.ReadLine().Replace("~", " "));

        Assert.Equal(" #==========#", Output.ReadLine());
        Assert.Equal("  ABCDEFGHIJ", Output.ReadLine());
    }

    private void ExpectGrids(params string[] gridRows)
    {
        Assert.Equal(" #==========#   #==========#", Output.ReadLine());

        for (int i = 0; i < gridRows.Length / 2; i++)
            Assert.Equal("" + i + "|" + gridRows[2*i] + "|   |" + gridRows[2*i + 1] + "|", Output.ReadLine().Replace("~", " "));

        Assert.Equal(" #==========#   #==========#", Output.ReadLine());
        Assert.Equal("  ABCDEFGHIJ     ABCDEFGHIJ", Output.ReadLine());
    }

    private void ExpectErrorMessage(string prompt, string expected, string input)
    {
        IgnoreGrids();
        ExpectPrompt(prompt);
        Enter(input);
        Assert.Equal(expected, Output.ReadLine());
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

    [Fact]
    void PlaceShipPromptIsShown()
    {
        ExpectPlayerGrid();
        ExpectPrompt("Place ship with length 5: ");
    }

    [Fact]
    void InvalidInputGivesErrorMessage()
    {
        ExpectPlaceErrorMessage("Invalid format", "hello");
        ExpectPlaceErrorMessage("Invalid X", "123");
        ExpectPlaceErrorMessage("Invalid Y", "AAA");
        ExpectPlaceErrorMessage("Invalid orientation", "A1A");
        ExpectPlaceErrorMessage("Invalid placement", "J9H");
    }

    [Fact]
    void ShipsArePlacedOnPlayerGrid()
    {
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
        SetupPlayerGrid();

        ExpectTargetErrorMessage("Invalid format", "hel");
        ExpectTargetErrorMessage("Invalid X", "12");
        ExpectTargetErrorMessage("Invalid Y", "AA");
    }

    [Fact]
    void missedTargetIsMarkedOnGrid()
    {
        SetupPlayerGrid();
        IgnoreGrids();
        ExpectPrompt("Enter target coordinate: ");
        Enter("A0");
        Assert.Equal("Missed", Output.ReadLine());
        Output.ReadLine();
        Output.ReadLine();
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
            "          ", "          ");
    }
}
