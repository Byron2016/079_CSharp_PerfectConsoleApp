using HelloWorldLibrary.BusinessLogic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace HelloWorldTestProject.BusinessLogic
{
    public class MessagesTest
    {
        [Fact]
        public void Greting_InEnghis()
        {
            ILogger<Messages> logger = new NullLogger<Messages>();
            Messages messages = new(logger);

            string expected = "Hello World";
            string actual = messages.Greeting("en");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Greting_InSpanish()
        {
            ILogger<Messages> logger = new NullLogger<Messages>();
            Messages messages = new(logger);

            string expected = "Hola Mundo";
            string actual = messages.Greeting("es");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Greting_InFranch()
        {
            ILogger<Messages> logger = new NullLogger<Messages>();
            Messages messages = new(logger);

            string expected = "Salut tout le monde";
            string actual = messages.Greeting("fr");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Greting_Invalid()
        {
            ILogger<Messages> logger = new NullLogger<Messages>();
            Messages messages = new(logger);


            Assert.Throws<InvalidOperationException>(
                () => messages.Greeting("gr")
                );
        }

        [Fact]
        public void Greting_InvalidTwo()
        {
            ILogger<Messages> logger = new NullLogger<Messages>();
            Messages messages = new(logger);


            Assert.Throws<InvalidOperationException>(
                () => messages.Greeting("test")
                );
        }

    }
}
