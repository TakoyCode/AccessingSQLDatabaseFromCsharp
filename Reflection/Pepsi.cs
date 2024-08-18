namespace Reflection.Main
{
    internal class Pepsi
    {
        public int tastiness { get; set; }

        public Pepsi()
        {
            
        }

        public Pepsi(int tastiness)
        {
            this.tastiness = tastiness;
        }

        public void Tasty()
        {
            Console.Write($"It has a tastiness of {tastiness}");
        }

        public int Dummy(string input)
        {
            return 2;
        }

        public int Dummy2(string input, bool trueing, int height)
        {
            return 2;
        }

    }
}
