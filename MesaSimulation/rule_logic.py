def implement_equation(variable_value, equation, equation_type, value):
    to_compare_value = None
    if equation_type == 0:
        to_compare_value = int(value)
    elif equation_type == 1:
        to_compare_value = value
    elif equation_type == 2:
        to_compare_value = bool(value)
    else:
        raise Exception("Unknown equation type")

    if equation == 0:
        return variable_value == to_compare_value
    elif equation == 1:
        return variable_value != to_compare_value
    elif equation == 2:
        return variable_value > to_compare_value
    elif equation == 3:
        return variable_value < to_compare_value
    else:
        raise Exception("Unknown equation")


def check_conditions(obj, rule):
    obj_dict = vars(obj)

    is_all_conditions_are_true = True
    for condition in rule['Conditions']:
        variable_value = obj_dict[condition['Variable']]
        is_all_conditions_are_true = is_all_conditions_are_true and implement_equation(variable_value,
                                                                                       condition["Equation"],
                                                                                       condition["EquationType"],
                                                                                       condition["Value"])
        
    return is_all_conditions_are_true