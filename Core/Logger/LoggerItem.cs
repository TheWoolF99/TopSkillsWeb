using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Logger
{
    /// <summary>
    /// Модель для записи в лог
    /// </summary>
    public class LoggerItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        /// <summary>
        /// Id пользователя IdentityUser
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// UserName пользователя IdentityUser
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Генерируется автоматически при добавлении записи в базу
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Имя сервиса где были изменения
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Название таблицы в которой были изменения
        /// </summary>
        public string? TableName { get; set; }

        public string? ObjectInfo { get; set; }
        public string? BeforeValue { get; set; }
        public string? AfterValue { get; set; }
        public string OperationName { get; set; }
    }

    public class LoggerLoginItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Browser { get; set; }
        public string BrowserVer { get; set; }
        public string OperationName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Date { get; set; }
    }

    public class LoggerFilter
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string? UserName { get; set; }

        public LoggerFilter()
        {
            DateStart = DateTime.Now.AddDays(-7);
            DateEnd = DateTime.Now;
            UserName = null;
        }
    }
}