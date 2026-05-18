using ChekeBoardPosition;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using System.Linq;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class CheckerBoardPositionTests
{
    [Theory]
    [InlineData (1,4)]
    [InlineData (3, 4)]
    [InlineData (1, 8)]
    public void Xy_WereCreatedCorrectly(byte x, byte y)
    {
        var board = new CheckerBoardPosition(x, y);
        Assert.Equal(x, board.X);
        Assert.Equal(y, board.Y);
    }

    [Theory]
    [InlineData(1, 'A')]
    [InlineData(3, 'C')]
    [InlineData(8, 'H')]
    public void CorrectXInLetter (byte x, char xLetter)
    {
        var board = new CheckerBoardPosition(x, 1);
        char letter = board.XLetter;
        Assert.Equal(xLetter, letter);
    }

    [Theory]
    [InlineData(1, 4, "A4")]
    [InlineData(3, 4, "C4")]
    [InlineData(2, 8, "B8")]
    public void Str_vs_Byte(byte x, byte y, string xy)
    {
        var board = new CheckerBoardPosition(x, y);
        Assert.Equal(xy,board.ToString());
        var board2 = CheckerBoardPosition.Parse(xy, null);
        Assert.Equal(x, board2.X);
        Assert.Equal(y, board2.Y);
        Assert.Equal(xy, board2.ToString());
    }

    [Theory]
    [InlineData(0, 4)]
    [InlineData(0, 0)]
    [InlineData(5, 12)]
    public void CorrectExceptionOutOfRange(byte x, byte y)
    {
        var board = Assert.Throws<ArgumentOutOfRangeException>(() => new CheckerBoardPosition(x, y));
        bool xError = x is < 1 or > 8;
        bool yError = y is < 1 or > 8;
        if (xError && yError)
        {
            Assert.Equal("x", board.ParamName);
        }
        else if (xError)
        {
            Assert.Equal("x", board.ParamName);
        }
        else if (yError)
        {
            Assert.Equal("y", board.ParamName);
        }
    }

    [Theory]
    [InlineData("")]
    [InlineData("a5")]
    [InlineData("B2B")]
    public void CorrectExceptionFormat(string xy)
    {
        var board = Assert.Throws<FormatException>(() => CheckerBoardPosition.Parse(xy, null));
        Assert.Contains($"Invalid {nameof(CheckerBoardPosition)}", board.Message);
        if (xy != null)
        {
            Assert.Contains(xy, board.Message);
        }
    }

    [Theory]
    [InlineData("D1", 4, 1)]
    [InlineData("H3", 8, 3)]
    [InlineData("C6", 3, 6)]

    public void TrueTrue(string xy, byte x, byte y)
    {
        bool board = CheckerBoardPosition.TryParse(xy, null, out var position);
        Assert.True(board);
        Assert.NotNull(position);
        Assert.Equal(x, position.X);
        Assert.Equal(y, position.Y);
        Assert.Equal(xy, position.ToString());
    }

    [InlineData("i1", 0, 0)]
    [InlineData("3", 0, 0)]
    [InlineData("C", 0, 0)]
    public void FalseFalse(string xy)
    {
        bool board = CheckerBoardPosition.TryParse(xy, null, out var position);
        Assert.False(board);
        Assert.Null(position);
    }

    [Fact]
    public void EnLanguage()
    {
        var provider = new System.Globalization.CultureInfo("en-US");
        bool board = CheckerBoardPosition.TryParse("B3", provider, out var position);
        Assert.True(board);
        Assert.NotNull(position);
        Assert.Equal(2, position.X);
        Assert.Equal(3, position.Y);
    }
    [Fact]
    public void Board1_8()
    {
        for (byte x = 1; x <= 8; x++)
        {
            for (byte y = 1; y <= 8; y++)
            {
                var board = new CheckerBoardPosition(x, y);
                Assert.Equal(x, board.X);
                Assert.Equal(y, board.Y);
            }
        }
    }
    [Fact]
    public void BoardA_8()
    {
        var xs = new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
        var ys = new[] { '1', '2', '3', '4', '5', '6', '7', '8' };
        foreach (var x in xs )
        {
            foreach (var y in ys)
            {
                var xy = $"{x}{y}";
                var board = CheckerBoardPosition.Parse(xy, null);
                Assert.Equal(xy, board.ToString());
            }
        }
    }
    [Fact]
    public void IdenticalA1()
    {
        var xs = new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
        for (byte x = 1; x <= 8; x++)
        {
            var board = new CheckerBoardPosition(x, 1);
            Assert.Equal(xs[x - 1], board.XLetter);
        }
    }
}