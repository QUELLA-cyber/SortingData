using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingData
{
    /// <summary>
    /// Объекты данных
    /// </summary>
    public class ExtractedData
    {
        /// <summary>
        /// Заголовок объекта данных.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Параметр подключения объекта данных.
        /// </summary>
        public string Connect { get; set; }

        /// <summary>
        /// Идентификатор объекта данных.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Список оригинальных строк, представляющих объект данных.
        /// </summary>
        public List<string> OriginalLines { get; set; } = new List<string>();
    }
}
