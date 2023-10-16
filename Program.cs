using System.Linq;

namespace LaVainaQuePidioWandy
{
    internal class Program
    {
        static List<Registry> Registries = new();
        static void Main(string[] args)
        {
            while(true)
            {
                int parsedOption;
                do
                {
                    Start();
                    parsedOption = ProcessInput();
                    if(parsedOption == 0)
                    {
                        Console.WriteLine("Presione cualquier tecla para volver a empezar");
                        WaitAndClear();
                    }
                } while (parsedOption == 0);

                if ((Options)parsedOption == Options.VerTodos)
                {
                    PrintALl();
                    Console.WriteLine("Operacion completada, presione cualquier tecla para volver al menu principal");
                    WaitAndClear();
                    continue;
                }

                else if ((Options)parsedOption == Options.BuscarPorNombreOApellido)
                {
                    FindByNameOrLastName();
                    Console.WriteLine("Operacion completada, presione cualquier tecla para volver al menu principal");
                    WaitAndClear();
                    continue;
                }

                else if ((Options)parsedOption == Options.Añadir)
                {
                    AddRegistry();
                    Console.WriteLine("Operacion completada, presione cualquier tecla para volver al menu principal");
                    WaitAndClear();
                    continue;
                }

                else if ((Options)parsedOption == Options.Editar)
                {
                    EditRegistry();
                    Console.WriteLine("Operacion completada, presione cualquier tecla para volver al menu principal");
                    WaitAndClear();
                    continue;
                }

                else if ((Options)parsedOption == Options.Eliminar)
                {
                    DeleteRegistry();
                    Console.WriteLine("Operacion completada, presione cualquier tecla para volver al menu principal");
                    WaitAndClear();
                    continue;
                }
            }
        }

        private static void WaitAndClear()
        {
            Console.ReadKey();
            Console.Clear();
        }

        private static void DeleteRegistry()
        {
            Console.WriteLine("Ingrese el Id del registro a eliminar: ");
            var id = Console.ReadLine();
            Registries.Remove(Registries.FirstOrDefault(x => x.Id == int.Parse(id)));
            Console.WriteLine("Registro eliminado");
        }

        private static void EditRegistry()
        {

            Console.WriteLine("Ingrese el Id del registro a editar");
            var validInt = int.TryParse(Console.ReadLine(), out int id);

            if(!validInt)
            {
                Console.WriteLine("El id ingresado no es valido");
                return;
            }

            var registry = Registries.FirstOrDefault(x => x.Id == id);
            if (registry is null)
            {
                Console.WriteLine($"El registro con Id {id} no existe");
            }
            else
            {
                Console.WriteLine("Nuevo nombre: ");
                registry.Name = Console.ReadLine() ?? registry.Name;
                Console.WriteLine("Nuevo apellido: ");
                registry.LastName = Console.ReadLine() ?? registry.LastName;
                Console.WriteLine("Nuevo Telefono: ");
                registry.PhoneNumber = Console.ReadLine() ?? registry.PhoneNumber;
            }
        }

        private static void AddRegistry()
        {
            Console.WriteLine("Nombre: ");
            var name = Console.ReadLine();

            Console.WriteLine("Apellido: ");
            var lastName = Console.ReadLine();

            Console.WriteLine("Telefono: ");
            var phone = Console.ReadLine();

            var newId = (Registries.Any() ? Registries.Max(x => x.Id) : 0) + 1;

            var registry = new Registry
            {
                Id = newId,
                Name = name,
                LastName = lastName,
                PhoneNumber = phone
            };

            Registries.Add(registry);
        }

        private static void FindByNameOrLastName()
        {
            Console.WriteLine("Ingrese el nombre o apellido que busca: ");
            var nombreOApellido = Console.ReadLine();
            Console.WriteLine("Resultados de la busqueda: ");
            var result = Registries.Where(x => x.Name.Contains(nombreOApellido, StringComparison.CurrentCultureIgnoreCase)
            || x.LastName.Contains(nombreOApellido, StringComparison.CurrentCultureIgnoreCase)).ToList();

            if(result.Any())
            {
                result.ForEach(x => Console.WriteLine(x.ToString()));
            }
            else
            {
                Console.WriteLine("No hay registros para mostrar");
            }
        }

        private static void PrintALl()
        {
            if (Registries.Any())
                Registries.ForEach(x => Console.WriteLine(x.ToString()));
            else Console.WriteLine("No hay registros para mostrar");
        }

        private static int ProcessInput()
        {
            Console.WriteLine("Elija el numero de la opción que desea ejecutar, luego presione enter");
            var input = Console.ReadLine();
            var parsedOption = ValidateOption(input);
            return parsedOption;
        }

        private static void Start()
        {
            PrintHeader();
            PrintOptions();
        }

        static void PrintHeader()
        {
            var header = @"
    $$\   $$\                                                $$\    $$\           $$\                     
    $$$\  $$ |                                               $$ |   $$ |          \__|                    
    $$$$\ $$ |$$\   $$\  $$$$$$\  $$\    $$\  $$$$$$\        $$ |   $$ | $$$$$$\  $$\ $$$$$$$\   $$$$$$\  
    $$ $$\$$ |$$ |  $$ |$$  __$$\ \$$\  $$  | \____$$\       \$$\  $$  | \____$$\ $$ |$$  __$$\  \____$$\ 
    $$ \$$$$ |$$ |  $$ |$$$$$$$$ | \$$\$$  /  $$$$$$$ |       \$$\$$  /  $$$$$$$ |$$ |$$ |  $$ | $$$$$$$ |
    $$ |\$$$ |$$ |  $$ |$$   ____|  \$$$  /  $$  __$$ |        \$$$  /  $$  __$$ |$$ |$$ |  $$ |$$  __$$ |
    $$ | \$$ |\$$$$$$  |\$$$$$$$\    \$  /   \$$$$$$$ |         \$  /   \$$$$$$$ |$$ |$$ |  $$ |\$$$$$$$ |
    \__|  \__| \______/  \_______|    \_/     \_______|          \_/     \_______|\__|\__|  \__| \_______|
-------------------------------------------------------------------------------------------------------------";
            Console.WriteLine(header);
        }

        static void PrintOptions()
        {
            var array = (Options[])Enum.GetValues(typeof(Options));
            var options = string.Join(Environment.NewLine, array.Select(x => $"$$$ - {(int)x} - {GetSpacedText(x.ToString())}"));
            Console.WriteLine(options);

            static string GetSpacedText(string text)
            {
                return string.Join("", text.Select((x, i) => char.IsUpper(x) && i != 0 ? $" {x}" : x.ToString()));
            }
        }

        static int ValidateOption(string option)
        {
            var isNumber = int.TryParse(option, out int numericOption);
            if(!isNumber)
            {
                Console.WriteLine("No se permiten letras o caracteres especiales, solo numeros");
                return 0;
            }

            if(!Enum.IsDefined(typeof(Options), numericOption))
            {
                Console.WriteLine("La opcion ingresada esta fuera del listado de opciones permitidas");
                return 0;
            }

            return numericOption;
        }
    }

    public class Registry
    {
        public int Id { get; internal set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Nombre: {Name}, Apellido: {LastName}, Telefono: {PhoneNumber}";
        }
    }

    public enum Options
    {
        VerTodos = 1,
        BuscarPorNombreOApellido = 2,
        Añadir = 3,
        Editar = 4,
        Eliminar = 5
    }
}