using System.Diagnostics;

namespace Learning.Adapter
{
    /*
   * Usage examples: Because user does not know french it cannot interact with French person, thus it uses a FrnechPersonAdapter, that further talks with french person.
   * The client sees only the target interface and not the adapter. The adapter implements the target interface. Adapter delegates all requests to Adaptee.
   * Identification: When there is a composition of an object that does actual work and adapater is the path through 
   * which client interacts.
   */
    public class AdapterMain
    {
        public string Do()
        {
            var service = new FrenchPersonAdapter(new FrenchTranslator());
            return service.Greeting("Hello", "Amit");
        }
    }

    public interface IPerson
    {
        string Greeting(string message, string name);
    }


    /// <summary>
    /// Adapter class.
    /// </summary>
    public class FrenchPersonAdapter : IPerson
    {
        private readonly ITranslator _translator;
        private readonly IPerson _person;
        public FrenchPersonAdapter(ITranslator translator)
        {
            _translator = translator;

            //The adapter pattern we have implemented above is called Object Adapter Pattern because the adapter 
            // holds an instance of adaptee. There is also another type called Class Adapter Pattern which use 
            // inheritance instead of composition but you require multiple inheritance to implement it.
            // class diagram of Class Adapter Pattern:
            _person = new FrenchPerson();
        }

        public string Greeting(string message, string name)
        {
            message = _translator.Translate($"{message}");
            return _person.Greeting(message, name);
        }
    }

    public class FrenchPerson : IPerson
    {
        public string Greeting(string message, string name)
        {
            return $"{message} {name}";
        }
    }

    public interface ITranslator
    {
        string Translate(string text);
    }

    // class FrenchTranslator is an adaptee here.
    public class FrenchTranslator : ITranslator
    {
        public string Translate(string text)
        {
            switch (text.ToLower())
            {
                case "hello":
                    return "Bonjour";
            }

            throw new System.ArgumentException(nameof(text));
        }
    }
}