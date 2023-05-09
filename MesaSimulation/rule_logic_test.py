import unittest
from rule_logic import implement_equation, check_conditions

class TestFunctions(unittest.TestCase):

    def test_implement_equation(self):
        # Test case 1: Test integer equality
        self.assertTrue(implement_equation(5, 0, 0, 5))
        self.assertFalse(implement_equation(5, 0, 0, 4))

        # Test case 2: Test string inequality
        self.assertTrue(implement_equation("hello", 1, 1, "world"))
        self.assertFalse(implement_equation("hello", 1, 1, "hello"))

        # Test case 3: Test boolean inequality
        self.assertTrue(implement_equation(True, 1, 2, False))
        self.assertFalse(implement_equation(False, 1, 2, False))

        # Test case 4: Test invalid equation type
        with self.assertRaises(Exception):
            implement_equation(3.14, 0, 3, 3)

        # Test case 5: Test invalid equation
        with self.assertRaises(Exception):
            implement_equation(10, 5, 0, 5)

    def test_check_conditions(self):
        class MyClass:
            def __init__(self, x, y):
                self.x = x
                self.y = y

        # Test case 1: Test all conditions are true
        obj = MyClass(5, "hello")
        rule = {'Conditions': [
            {'Variable': 'x', 'Equation': 0, 'EquationType': 0, 'Value': 5},
            {'Variable': 'y', 'Equation': 1, 'EquationType': 1, 'Value': 'world'}
        ]}
        self.assertTrue(check_conditions(obj, rule))

        # Test case 2: Test some conditions are false
        obj = MyClass(10, "hello")
        rule = {'Conditions': [
            {'Variable': 'x', 'Equation': 3, 'EquationType': 0, 'Value': 5},
            {'Variable': 'y', 'Equation': 1, 'EquationType': 1, 'Value': 'world'}
        ]}
        self.assertFalse(check_conditions(obj, rule))

        # Test case 3: Test all conditions are false
        obj = MyClass(10, "hello")
        rule = {'Conditions': [
            {'Variable': 'x', 'Equation': 3, 'EquationType': 0, 'Value': 5},
            {'Variable': 'y', 'Equation': 0, 'EquationType': 1, 'Value': 'world'}
        ]}
        self.assertFalse(check_conditions(obj, rule))

if __name__ == '__main__':
    unittest.main()