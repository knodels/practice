using TDADOAMSSETEDF;


var service = new ApprovalService();

//Запуск
service.StartApproval(101, new List<string> { "Иванов", "Петров", "Сидорова" }, 72);

//Заключения
service.SubmitDecision(101, "Иванов", "approved", "Ок");
service.SubmitDecision(101, "Петров", "approved", "Норм");
service.SubmitDecision(101, "Сидорова", "rejected", "Ошибка в сумме");

//Результат
Console.WriteLine($"\nСтатус: {service.GetApprovalStatus(101)}");

Console.WriteLine("\nИстория:");
foreach (var h in service.GetApprovalHistory(101))
    Console.WriteLine($"{h.Approver}: {h.Decision} - {h.Comment}");