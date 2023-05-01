# Battleship (C#)

This code requires *.NET 6* or above. However, I believe that all methods used are available in virtually all different versions of .NET, starting with *.NET Framework 1.1* (back in the good old days). Check out this [Microsoft guide](https://aka.ms/new-console-template) if you want to run the code using an older version of .NET.

## Usage

Start by running

```dotnet run```

Ships are placed by entering `COLUMN` `ROW` `ORIENTATION`, where *COLUMN* is a value between A and J, *ROW* is a value between 0 and 9, and *ORIENTATION* is either H(orizontal) or V(ertical). The example below will[^1] place a ship at squares B3, B4 and B5.

```Place ship with length 3: B3V```

The enemy's ships are targeted in the same way, i.e. by entering `COLUMN` `ROW`.

## Important

The code contains quite a few deliberate bugs. There is no need to submit any pull requests to fix the “bugs”, they are there for a reason. The same goes for the code itself. It’s not pretty, but it’s not supposed to be.

If you want to share the bugs you find or your thoughts on the code, feel free to leave a comment in the video.

https://youtu.be/uZ6_OoWjfYk

[^1]: Actually it won't, due to a bug in the code.
