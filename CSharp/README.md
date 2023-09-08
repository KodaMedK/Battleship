# Battleship (C#)

This code requires *.NET 6* or above. Check out this [Microsoft guide](https://aka.ms/new-console-template) if you want to run the code using an older version of .NET.

The unit testing framework used in this example is *xUnit.net* (for more information, visit https://xunit.net/). The tests are pretty basic, so any testing framework should work.

## Usage

Start by running

```dotnet run```

Ships are placed by entering `COLUMN` `ROW` `ORIENTATION`, where *COLUMN* is a value between A and J, *ROW* is a value between 0 and 9, and *ORIENTATION* is either H(orizontal) or V(ertical). The example below will[^1] place a ship at squares B3, B4 and B5.

```Place ship with length 3: B3V```

The enemy's ships are targeted in the same way, i.e. by entering `COLUMN` `ROW`.

## Tests

The tests can be invoked by running

```dotnet test```

## Important

If you want to share your thoughts on the code, feel free to leave a comment in the video.

https://youtu.be/K60O6DQBjoY

[^1]: Actually it won't, due to a bug in the code.
