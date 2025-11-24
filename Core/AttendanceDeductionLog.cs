using AbonementModel = Core.Abonement.Abonement;

namespace Core
{
    public class AttendanceDeductionLog
    {
        public int AttendanceDeductionLogId { get; set; }

        // Связь с посещением
        public int AttendanceId { get; set; }

        public Attendance Attendance { get; set; }

        // Связь с абонементом
        public int AbonementId { get; set; }

        public AbonementModel Abonement { get; set; }

        // Информация о студенте и группе (для удобства запросов)
        public int StudentId { get; set; }

        public Student Student { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        // Тип операции (списание/возврат)
        public DeductionType OperationType { get; set; }

        // Количество посещений (обычно 1, но можно для гибкости)
        public int VisitsCount { get; set; }

        // Статус посещения до и после изменения
        public int PreviousStatus { get; set; }

        public int NewStatus { get; set; }

        // Баланс до и после операции
        public int BalanceBefore { get; set; }

        public int BalanceAfter { get; set; }

        // Дата и время операции
        public DateTime OperationDate { get; set; }

        // Кто выполнил операцию (если есть система пользователей)
        public string PerformedBy { get; set; }

        // Комментарий (например, причина отмены)
        public string Comment { get; set; }
    }
}