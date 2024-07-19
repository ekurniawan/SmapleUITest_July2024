using CreditCards.BLL;

namespace CreditCards.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test_LuasSegitiga()
        {
            Segitiga segitiga = new Segitiga();

            double alas = 10;
            double tinggi = 5;

            double expected = 25;

            double actual = segitiga.LuasSegitiga(alas, tinggi);
            Assert.Equal(expected, actual);
        }
    }
}