using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StringMagician.Test
{
    [TestClass]
    public class StringMagicianTest
    {
        [TestMethod]
        public void NumberToAlphabetTest()
        {
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(1), "A");
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(5), "E");
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(7), "G");
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(27), "AA");
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(28), "AB");
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(51), "AY");
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(52), "AZ");
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(53), "BA");
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(54), "BB");
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(341), "MC");
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(702), "ZZ");
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(703), "AAA");
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(704), "AAB");
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(728), "AAZ");
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(983), "AKU");
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(1378), "AZZ");
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(1380), "BAB");
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(2369), "CMC");
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(18279), "AAAA");
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(17603), "ZAA");
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(457003), "YZAA");
            Assert.AreEqual(Magician.NumberToAlphabetByOrder(457679), "ZAAA");
        }

        [TestMethod]
        public void AlphabetToNumberTest()
        {
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("A"), 1);
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("E"), 5);
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("G"), 7);
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("AA"), 27);
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("AB"), 28);
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("AY"), 51);
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("AZ"), 52);
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("BA"), 53);
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("BB"), 54);
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("MC"), 341);
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("ZZ"), 702);
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("AAA"), 703);
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("AAB"), 704);
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("AAZ"), 728);
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("AKU"), 983);
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("AZZ"), 1378);
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("BAB"), 1380);
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("CMC"), 2369);
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("AAAA"), 18279);
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("ZAA"), 17603);
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("YZAA"), 457003);
            Assert.AreEqual(Magician.AlphabetToNumberByOrder("ZAAA"), 457679);
        }
    }
}
