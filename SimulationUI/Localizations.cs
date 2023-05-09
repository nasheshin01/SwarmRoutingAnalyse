namespace SimulationUI;

public static class Localizations
{
    public static LocalizationElement ConditionVariableX = new LocalizationElement("x", "X");
    public static LocalizationElement ConditionVariableY = new LocalizationElement("y", "Y");
    public static LocalizationElement ConditionVariableEnergy = new LocalizationElement("energy", "Энергия");
    public static LocalizationElement ConditionVariableHibernationStatus = new LocalizationElement("hibernation_status", "Статус гибернации");
    public static LocalizationElement ConditionVariableScoutState = new LocalizationElement("scout_state", "Статус разведчика");
    public static LocalizationElement ConditionVariableIsNextDroneOutOfRadius = new LocalizationElement("is_next_drone_out_of_radius", "Дрон вне зоны связи");
    public static LocalizationElement ConditionVariableIsNextDroneInLoadingState = new LocalizationElement("is_next_drone_in_loading_state", "Дрон занят другим агентов");
    public static LocalizationElement ConditionVariableIsCurrentDroneSource = new LocalizationElement("is_current_drone_source", "Текущий дрон - начальный");
    public static LocalizationElement ConditionVariableIsCurrentDroneDestination = new LocalizationElement("is_current_drone_destination", "Текущий дрон - конечный");
    public static LocalizationElement ConditionVariableIsNextDroneChoosed = new LocalizationElement("is_next_drone_choosed", "Следующий дрон выбран");
    public static LocalizationElement ConditionVariableWorkerState = new LocalizationElement("worker_state", "Состояние работника");
    public static LocalizationElement ConditionVariableIsNextDroneBusy = new LocalizationElement("is_next_drone_busy", "Дрон занят другим агентов");
    public static LocalizationElement ConditionVariableIsNextDroneChosen = new LocalizationElement("is_next_drone_chosen", "Следующий дрон выбран");

    public static LocalizationElement ConditionValueHibernate = new LocalizationElement("Hibernate", "Включена гибернация");
    public static LocalizationElement ConditionValueNoHibernate = new LocalizationElement("NoHibernate", "Выключена гибернация");
    public static LocalizationElement ConditionValueScouting = new LocalizationElement("Scouting", "Разведка");
    public static LocalizationElement ConditionValueGoingToStart = new LocalizationElement("GoingToStart", "Возвращение назад");
    public static LocalizationElement ConditionValueScoutingEnded = new LocalizationElement("ScoutingEnded", "Разведка завершена");
    public static LocalizationElement ConditionValueSending = new LocalizationElement("Sending", "Отправка пакета");
    public static LocalizationElement ConditionValueSendingEnded = new LocalizationElement("SendingEnded", "Отправка пакета завершена");
    public static LocalizationElement ConditionValueTrue = new LocalizationElement("True", "Да");
    public static LocalizationElement ConditionValueFalse = new LocalizationElement("False", "Нет");

    public static LocalizationElement ConditionEquationEquals = new LocalizationElement("Equal", "=");
    public static LocalizationElement ConditionEquationNotEquals = new LocalizationElement("NotEqual", "!=");
    public static LocalizationElement ConditionEquationMore = new LocalizationElement("More", ">");
    public static LocalizationElement ConditionEquationLess = new LocalizationElement("Less", "<");
}