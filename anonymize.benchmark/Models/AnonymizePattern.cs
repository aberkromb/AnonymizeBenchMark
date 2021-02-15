namespace Anonymize.Benchmark.Models
{
    /// <summary>
    /// Настройки шаблона анонимизации
    /// </summary>
    public class AnonymizePattern
    {
        /// <summary>
        /// Строки, которые нужно маскировать
        /// </summary>
        public string[] Params { get; set; }

        /// <summary>
        /// Маска
        /// </summary>
        public string Mask { get; set; }
    }
}