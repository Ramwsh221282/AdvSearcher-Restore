using System.Text;

namespace SettingsConfigurationExtension
{
    public sealed record Option(string Key, string Value)
    {
        public ReadOnlySpan<char> BuildStringRepresentation()
        {
            if (string.IsNullOrEmpty(Key) || string.IsNullOrEmpty(Value))
                return string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(Key);
            stringBuilder.Append("\t");
            stringBuilder.Append(Value);
            return stringBuilder.ToString().AsSpan();
        }
    }

    public interface IOptionProcessor
    {
        Task<Option> Process(Option option);
    }

    public interface IWriteStrategy
    {
        void Invoke(string _fileName, Option option);
    }

    public sealed class WriteNewFileStrategy : IWriteStrategy
    {
        public void Invoke(string _fileName, Option option)
        {
            using (var fileStream = new FileStream(_fileName, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new StreamWriter(fileStream))
                {
                    writer.WriteLine(option.BuildStringRepresentation());
                }
            }
        }
    }

    public sealed class WriteInExistingFileStrategy : IWriteStrategy
    {
        public void Invoke(string _fileName, Option option)
        {
            using (var fileStream = new FileStream(_fileName, FileMode.Append, FileAccess.Write))
            {
                using (var writer = new StreamWriter(fileStream))
                {
                    writer.WriteLine(option.BuildStringRepresentation());
                }
            }
        }
    }

    public sealed class WriterStrategyResolver
    {
        private readonly string _fileName;

        public WriterStrategyResolver(string fileName) => _fileName = fileName;

        public IWriteStrategy Resolve()
        {
            bool existance = File.Exists(_fileName);
            return existance switch
            {
                true => new WriteInExistingFileStrategy(),
                false => new WriteNewFileStrategy(),
            };
        }
    }

    public sealed class OptionWriter : IOptionProcessor
    {
        private readonly string _fileName;
        private readonly IWriteStrategy _strategy;

        public OptionWriter(string fileName)
        {
            _fileName = fileName;
            _strategy = new WriterStrategyResolver(fileName).Resolve();
        }

        public async Task<Option> Process(Option option)
        {
            _strategy.Invoke(_fileName, option);
            return await Task.FromResult(option);
        }
    }

    public sealed class OptionReader : IOptionProcessor
    {
        private readonly string _fileName;

        public OptionReader(string fileName) => _fileName = fileName;

        public async Task<Option> Process(Option option)
        {
            using (var fileStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(fileStream))
                {
                    string lines = await reader.ReadToEndAsync();
                    if (string.IsNullOrEmpty(lines))
                        return new Option(string.Empty, string.Empty);
                    return GetKeyValuePair(lines, option);
                }
            }
        }

        private Option GetKeyValuePair(string lines, Option option)
        {
            ReadOnlySpan<string> splittedLines = lines.Split(
                "\r\n",
                StringSplitOptions.RemoveEmptyEntries
            );
            for (int index = 0; index < splittedLines.Length; index++)
            {
                ReadOnlySpan<string> currentKeyValuePair = splittedLines[index]
                    .Split("\t", StringSplitOptions.RemoveEmptyEntries);
                for (int subbindex = 0; subbindex < currentKeyValuePair.Length; subbindex++)
                {
                    if (currentKeyValuePair[subbindex] == option.Key)
                        return new Option(currentKeyValuePair[subbindex], currentKeyValuePair[^1]);
                }
            }
            return new Option(string.Empty, string.Empty);
        }
    }

    public sealed class OptionManager
    {
        public IOptionProcessor CreateReader(string fileName)
        {
            return new OptionReader(fileName);
        }

        public IOptionProcessor CreateWrite(string fileName)
        {
            return new OptionWriter(fileName);
        }
    }

    class Program
    {
        static void Main()
        {
            //IOptionProcessor writer = new OptionWriter("VK_KEYS.TXT");
            Option option = new Option("ACCESS_TOKEN", "BLABLABLABLA");
            IOptionProcessor reader = new OptionReader("VK_KEYS.TXT");
            Option existingOption = reader.Process(option).Result;
            int bpoint = 0;
        }
    }
}
