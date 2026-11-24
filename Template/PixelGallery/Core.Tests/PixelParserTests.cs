namespace Core.Tests;

using Core.Pixel;

[TestClass]
public class PixelParserTests
{
    [TestMethod]
    public void Parse_SingleColors_OneRunPerPixel()
    {
        var result = PixelParser.Parse("RGB");

        Assert.AreEqual(3, result.Runs.Count);
        Assert.AreEqual(new Run(0, 0, 1, 'R'), result.Runs[0]);
        Assert.AreEqual(new Run(0, 1, 1, 'G'), result.Runs[1]);
        Assert.AreEqual(new Run(0, 2, 1, 'B'), result.Runs[2]);
    }

    [TestMethod]
    public void Parse_RepeatPrefix_CreatesRunWithLength()
    {
        var result = PixelParser.Parse("3R2B");

        Assert.AreEqual(2, result.Runs.Count);
        Assert.AreEqual(new Run(0, 0, 3, 'R'), result.Runs[0]);
        Assert.AreEqual(new Run(0, 3, 2, 'B'), result.Runs[1]);
    }

    [TestMethod]
    public void Parse_MultiDigitPrefix_CreatesLongRun()
    {
        var result = PixelParser.Parse("12L");

        Assert.AreEqual(1, result.Runs.Count);
        Assert.AreEqual(new Run(0, 0, 12, 'L'), result.Runs[0]);
        Assert.AreEqual(12, result.Width);
    }

    [TestMethod]
    public void Parse_Transparent_NoRunButColumnAdvances()
    {
        var result = PixelParser.Parse("2.K3.K");

        Assert.AreEqual(2, result.Runs.Count);
        Assert.AreEqual(new Run(0, 2, 1, 'K'), result.Runs[0]);
        Assert.AreEqual(new Run(0, 6, 1, 'K'), result.Runs[1]);
        Assert.AreEqual(7, result.Width);
    }

    [TestMethod]
    public void Parse_UnknownCharacters_AreIgnored()
    {
        var result = PixelParser.Parse("2R x 2B\r\n");

        Assert.AreEqual(2, result.Runs.Count);
        Assert.AreEqual(new Run(0, 0, 2, 'R'), result.Runs[0]);
        Assert.AreEqual(new Run(0, 2, 2, 'B'), result.Runs[1]);
    }

    [TestMethod]
    public void Parse_MultipleRows_WidthAndHeight()
    {
        var result = PixelParser.Parse("2R\n\n4G\n.K\n");

        Assert.AreEqual(3, result.Height, "empty lines are not counted");
        Assert.AreEqual(4, result.Width,  "width is the longest row");
        Assert.AreEqual(new Run(1, 0, 4, 'G'), result.Runs[1]);
        Assert.AreEqual(new Run(2, 1, 1, 'K'), result.Runs[2]);
    }

    [TestMethod]
    public void Parse_TagsHeaderAndComments()
    {
        var result = PixelParser.Parse("# my picture\n#tags: love, red ,retro\nR");

        CollectionAssert.AreEqual(new[] { "love", "red", "retro" }, result.Tags.ToArray());
        Assert.AreEqual(1, result.Height, "comment lines are not rows");
        Assert.AreEqual(1, result.Runs.Count);
    }

    /// <summary>
    /// The repeat prefix is reset after every command (color letter or '.').
    /// Example: "3R.B" => R(len 3, col 0), B(len 1, col 4)
    /// </summary>
    [TestMethod]
    public void Parse_PrefixIsResetAfterCommand()
    {
        // TODO (Task 2): write this test
        Assert.Inconclusive("TODO: test not implemented yet");
    }
}
