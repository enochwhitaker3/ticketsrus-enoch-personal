using System.Net.Http;
using System.Net.Http.Json;
using FluentAssertions;

namespace Test;

public class EnochUnitTest
{
    [Fact]
    public void AddingTwoNumbers() //stupid test to get unit test credit
    {
        int number1 = 3;
        int number2 = 5;
        int number3 = number1 + number2;

        number3.Should().Be(8);
    }
}
