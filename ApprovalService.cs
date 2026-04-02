using System;
using System.Collections.Generic;
using System.Text;
using TDADOAMSSETEDF.Models;

namespace TDADOAMSSETEDF
{
    public class ApprovalService
    {
        private readonly Dictionary<int, ApprovalTask> _storage = new Dictionary<int, ApprovalTask>();

        //Запуск()
        public bool StartApproval(int documentId, List<string> approvers, int deadlineHours)
        {
            if (approvers == null || approvers.Count == 0)
            {
                Console.WriteLine("Список согласующих не может быть пустым");
                return false;
            }
            // Проверка на повторы
            if (_storage.ContainsKey(documentId))
            {
                Console.WriteLine($"Для документа {documentId} уже запущен процесс");
                return false;
            }
            // Создание задачи
            var task = new ApprovalTask(documentId, approvers, deadlineHours);
            task.Status = ApprovalStatus.InProgress;
            _storage[documentId] = task;
            // Уведомление первому согласующему
            Console.WriteLine($"{approvers[0]}, вам на согласование документ {documentId}. Срок: {deadlineHours} ч.");
            return true;
        }

        //статуса
        public ApprovalStatus GetApprovalStatus(int documentId)
        {
            if (!_storage.ContainsKey(documentId))
                return ApprovalStatus.Pending;

            var task = _storage[documentId];

            if (task.Status == ApprovalStatus.InProgress && DateTime.Now > task.Deadline)
            {
                task.Status = ApprovalStatus.Overdue;
                Console.WriteLine($"[ПРОСРОЧКА] Документ {documentId} просрочен!");
            }

            return task.Status;
        }

        //заключение
        public bool SubmitDecision(int documentId, string approver, string decision, string comment)
        {
            // поиск
            if (!_storage.ContainsKey(documentId))
            {
                Console.WriteLine($"Документ {documentId} не найден");
                return false;
            }

            var task = _storage[documentId];
            // проверка статуса
            if (task.Status != ApprovalStatus.InProgress)
            {
                Console.WriteLine($"Документ уже {task.Status}");
                return false;
            }
            //очередь
            var currentApprover = task.Approvers[task.CurrentApproverIndex];
            if (!currentApprover.Equals(approver, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Сейчас очередь {currentApprover}");
                return false;
            }
            // сохранение в историю
            task.History.Add(new ApprovalHistory(approver, decision, comment));
            //отклонение
            if (decision.Equals("rejected", StringComparison.OrdinalIgnoreCase))
            {
                task.Status = ApprovalStatus.Rejected;
                Console.WriteLine($"Документ {documentId} отклонен {approver}. Причина: {comment}");
                return true;
            }
            //согласование
            if (decision.Equals("approved", StringComparison.OrdinalIgnoreCase))
            {
                task.CurrentApproverIndex++;

                if (task.CurrentApproverIndex >= task.Approvers.Count)
                {
                    task.Status = ApprovalStatus.Approved;
                    Console.WriteLine($"Документ {documentId} прошел всех!");
                }
                else
                {
                    var next = task.Approvers[task.CurrentApproverIndex];
                    int hoursLeft = (int)(task.Deadline - DateTime.Now).TotalHours;
                    Console.WriteLine($"{next}, документ {documentId}. Осталось: {hoursLeft} ч.");
                }
                return true;
            }

            Console.WriteLine($"Неизвестное решение: {decision}");
            return false;
        }

        //История
        public List<ApprovalHistory> GetApprovalHistory(int documentId)
        {
            return _storage.ContainsKey(documentId)
                ? _storage[documentId].History
                : new List<ApprovalHistory>();
        }
    }
}
